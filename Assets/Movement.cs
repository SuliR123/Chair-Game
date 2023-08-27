using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Class containing all the information for moving the player, attacking, and taking damage
*/

public class Movement : MonoBehaviour
{

    private int health = 3; 

    // variables for moving and keeping track of player position 
    [SerializeField] private float speed; 
    [SerializeField] private float jump; 
    private int numberOfJumps = 1; 
    private float horizontal; 
    private bool isFacingRight = true;
    private bool isGrounded; 
    private bool additionalJump; 

    // variables for dashing 
    private bool canDash = true; 
    private bool isDashing; 
    private float dashPower = 10; 
    private float dashingTime = .2f; 
    private float dashCooldown = 1f; 

    // variables for player interaction with the world 
    [SerializeField] private Rigidbody2D rb; 
    [SerializeField] private Transform[] groundCheck; 
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private TrailRenderer tr; 
    [SerializeField] private Transform leftHead;
    [SerializeField] private Transform rightHead;  
    [SerializeField] private Transform throwingHand;
    [SerializeField] private Transform cameraFollow; 

    // variables for attacking 
    [SerializeField] private GameObject weapon; 
    private GameObject throwable; 
    [HideInInspector] public bool attacking = false; 

    private Animator anim; 

    private bool gettingHit = false; 

    private Game g; 

    [SerializeField] private GameObject hitEffect; 

    // Start is called before the first frame update
    public void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        g = GameObject.Find("GameTime").GetComponent<Game>(); 
        health = 3; 
        throwable = null; 
    }

    // Update is called once per frame
    void Update()
    {

        // attacking  
        if(Input.GetKeyDown(KeyCode.X) && !attacking) {
            StartCoroutine(Swing()); 
        }

        if(rb.velocity.x != 0 && isGrounded && !additionalJump) 
            anim.SetBool("Walk", true); 
        else 
            anim.SetBool("Walk", false);    

        isGrounded = false;
        int index = 0; 
        for(int i = 0; i < groundCheck.Length; i++) {
            Transform t = groundCheck[i]; 
            if(Physics2D.OverlapCircle(t.position, .2f, groundLayer)) 
                index = i; 
            isGrounded = isGrounded || Physics2D.OverlapCircle(t.position, .2f, groundLayer);
        }

        // reseting the number of jumps the player has 
        // if the player is on the ground and isn't jumping 
        // or the player collides with a flower and gets an extra jump when they fall into the flower 
        // or the player is coming from below and jumps into the flower 
        if(isGrounded && rb.velocity.y <= 0 || isGrounded && 
            Physics2D.OverlapCircle(groundCheck[index].position, .2f, groundLayer).gameObject.tag.Equals("Jump") && 
                (rb.velocity.y <= 0 
                    || Physics2D.OverlapCircle(groundCheck[index].position, .2f, groundLayer).gameObject.transform.position.y - transform.position.y >= 0)) {
            if(Physics2D.OverlapCircle(groundCheck[index].position, .2f, groundLayer).gameObject.tag.Equals("Jump")) {
                additionalJump = true; 
                canDash = true; 
            }
            else {
                additionalJump = false; 
            }              
            numberOfJumps = 1; 
        }

        if(isDashing || gettingHit)
            return;

        horizontal = Input.GetAxisRaw("Horizontal"); 
        Flip();    

        // finding a throwable weapon on the floor 
        bool findWeapon = Physics2D.OverlapCircle(transform.position, 2f, LayerMask.GetMask("Item")); 
        if(findWeapon && Input.GetKeyDown(KeyCode.V)) {
            GameObject foundWeapon = Physics2D.OverlapCircle(transform.position, 2f, LayerMask.GetMask("Item")).gameObject; 
            Item item = foundWeapon.GetComponent<Item>(); 
            // prevents the player from picking up items that have been thrown 
            if(!item.held){
                throwable = foundWeapon; 
                foundWeapon.GetComponent<Item>().SetHeld(true); 
                foundWeapon.SetActive(false);
                item.DisplayItem(); 
            }
        }

        // throwing item in the players hand 
        if(throwable != null && Input.GetKeyDown(KeyCode.C)) {
            Item i = throwable.GetComponent<Item>(); 
            if(i.CanThrow())
                throwable.GetComponent<Item>().Throw(transform.localScale.x, throwingHand); 
            else {
                throwable = null; 
            }
        }

        // jumping through platforms from below 
        bool headCollision = Physics2D.OverlapArea(leftHead.position, rightHead.position, groundLayer);
        if(headCollision && rb.velocity.y > 0) {
            GameObject other = Physics2D.OverlapArea(leftHead.position, rightHead.position, groundLayer).gameObject; 
            StartCoroutine(Platform(other));
        }

        // for letting players pass through platform when on top of it 
        if(isGrounded && Input.GetKeyDown(KeyCode.DownArrow)) {
            GameObject other = Physics2D.OverlapCircle(groundCheck[index].position, .2f, groundLayer).gameObject; 
            StartCoroutine(Platform(other)); 
        }

        // lets the player jump 
        if(Input.GetButtonDown("Jump") && numberOfJumps > 0) {
            anim.SetBool("Jump", true); 
            rb.velocity = new Vector2(rb.velocity.x, jump); 
            numberOfJumps -= 1; 
        }
        else 
            anim.SetBool("Jump", false); 

        // short jump 
        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0){
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2); 
        }

        // dashing 
        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash){
            StartCoroutine(Dash()); 
        }
    }

    void FixedUpdate(){
        if(isDashing || gettingHit)
            return;
        // moving the player left and right 
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);     
    }

    // rotates the player depending on the direction they are moving in 
    private void Flip() {
        if((horizontal < 0 && isFacingRight || !isFacingRight && horizontal > 0)) {
            isFacingRight = !isFacingRight; 
            Vector3 localScale = transform.localScale;
            localScale.x *= -1; 
            transform.localScale = localScale;
        }
    }

    // plays the animation for swinging the weapon
    private IEnumerator Swing() {
        anim.SetBool("Sword", true); 
        attacking = true; 
        weapon.GetComponent<TrailRenderer>().emitting = true; 
        yield return new WaitForSeconds(.6f); 
        weapon.GetComponent<TrailRenderer>().emitting = false; 
        anim.SetBool("Sword", false); 
        attacking = false; 
    }

    // lets the player pass through a platform 
    private IEnumerator Platform(GameObject other) {
        // if the other gameobject isn't a flower and its a platform 
        if(!other.tag.Equals("Jump") && other.tag == "Platform"){
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true; 
            yield return new WaitForSeconds(.5f);
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false; 
        }       
    }

    // lets the player dash 
    private IEnumerator Dash(){
        canDash = false; 
        isDashing = true; 
        float gravityScale = rb.gravityScale; 
        rb.gravityScale = 0; 
        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0); 
        tr.emitting = true; 
        yield return new WaitForSeconds(dashingTime); 
        tr.emitting = false; 
        rb.gravityScale = gravityScale; 
        isDashing = false; 
        yield return new WaitForSeconds(dashCooldown); 
        canDash = true;        
    }
 
    // The player takes damage also adds knockback force to the player based on inputs 
    public IEnumerator Damage(int damage, float direction, float knockbackx, float knockbacky) {
        health -= damage; 
        g.PlayerHealth(health, false); 
        gettingHit = true; 
        rb.velocity = new Vector2(direction * knockbackx, knockbacky); 
        hitEffect.SetActive(true); 
        yield return new WaitForSeconds(.5f); // apply knockback for .5 seconds 
        hitEffect.SetActive(false); 
        gettingHit = false; 
    }

    // checks if this player is alive 
    public bool Alive() {
        return health > 0; 
    }

    // resets the players stats back to the original 
    public void Reset() {
        health = 3; 
        throwable = null; 
    }

    // healths the player and updates the ui displaying the health 
    public void AddHealth() {
        if(health == 3)
            return;  
        g.PlayerHealth(health, true); 
        health++; 
    }

}
