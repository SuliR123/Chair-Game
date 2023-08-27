using System.Collections;
using System.Collections.Generic;
using UnityEngine; 


/*
Enemy class contains the basic information for an enemy 
defines basic fields such as health damage movespeed and groundcheck 
which checks if the enemy is colliding with the floor and determines when the enemy should rotate 
*/

public abstract class Enemy : MonoBehaviour
{

    [SerializeField] private int health = 2; 
    [SerializeField] private int damage = 1; 

    [HideInInspector] public Game g; 

    private int id; 

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public BoxCollider2D boxCollider; 
    public float speed = 2; 
    [SerializeField] private float jumpForce = 5; 
    [SerializeField] private float attackRange = 2; 
    [SerializeField] private float knockbackx = 2; 
    [SerializeField] private float knockbacky = 2; 
    public Transform collision; 
    [HideInInspector] public float direction = 1; 
    [HideInInspector] public float walkingTime; 
    private bool isFacingRight = true;

    private Transform player; 

    [HideInInspector] public Animator anim; 
    [HideInInspector] public bool attacking; 
    [HideInInspector] public bool gettingHit = false; 
    [SerializeField] private GameObject hitEffect;

    private bool spawning = true; 

    private bool dead = false; 

    // list of items this enemy can drop 
    [SerializeField] private GameObject[] drops; 

    // Start is called before the first frame update
    public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
        anim = gameObject.GetComponent<Animator>(); 
        boxCollider = gameObject.GetComponent<BoxCollider2D>(); 
        player = GameObject.Find("Chair").transform; 
        hitEffect.SetActive(false); 
        SetDirection();   
        g = GameObject.Find("GameTime").GetComponent<Game>();  
        StartCoroutine(Spawning()); 
    }

    // Update is called once per frame
    public void Update()
    {

        if(spawning || dead) {
            return; 
        }

        if(health <= 0) {
            Remove(); 
            StartCoroutine(Die()); 
            dead = true; 
            return; 
        }

        if(gettingHit) {
            anim.SetBool("Attack", false); 
            anim.SetBool("Walk", false); 
            return; 
        }

        walkingTime -= Time.deltaTime; 
        if(ChangeDirection()) {
            SetDirection(); 
            walkingTime = Random.Range(2, 7); 
        }

        if(Vector2.Distance(player.position, transform.position) < attackRange && !attacking) {
            FacePlayer(); 
            StartCoroutine(Attack()); 
        }
        else if(!attacking)
            Flip(); 

        if(rb.velocity.x != 0 && !attacking) 
            anim.SetBool("Walk", true); 
        else 
            anim.SetBool("Walk", false); 
    }

    void FixedUpdate() {
        if(!attacking && health > 0 && !gettingHit && !spawning)
            Walk(); 
    }

    // checks if this enemy should change directions or not 
    public virtual bool ChangeDirection() {
        return walkingTime <= 0 || 
        Physics2D.OverlapCircle(collision.position, .2f, LayerMask.GetMask("Wall")) 
        || !Physics2D.OverlapCircle(collision.position, .2f, LayerMask.GetMask("Ground")) && !attacking;
    }

    // method used for moving this enemy, can be overriden in child classes if 
    // enemy doesn't move like a normal ground enemy 
    public virtual void Walk() {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y); 
    }

    // flips the enemy to face the direction it isn't facing now 
    private void Flip() {
        if(rb.velocity.x < 0 && isFacingRight || !isFacingRight && rb.velocity.x > 0 && !attacking) {
            isFacingRight = !isFacingRight; 
            Vector3 localScale = transform.localScale;
            localScale.x *= -1; 
            transform.localScale = localScale;
        }
    }

    // Has this enemy face the player 
    public void FacePlayer() {
        float distance = transform.position.x - player.position.x; 
        if(distance > 0 && isFacingRight || distance < 0 && !isFacingRight) {
            isFacingRight = !isFacingRight; 
            Vector3 localScale = transform.localScale;
            localScale.x *= -1; 
            transform.localScale = localScale;
        }
    }

    // randomly assigns which direction this enemy should walk in 
    // can be overriden in child classes if choosing direction of the enemy is different 
    public virtual void SetDirection() {
        if((int) Random.Range(0,2) == 1) 
            direction = 1; 
        else 
            direction = -1; 
    }

    // Gives this enemy invincibility right when it spawns into the game 
    private IEnumerator Spawning() {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true; 
        rb.velocity = new Vector2(0,0); 
        float gravityScale = rb.gravityScale; 
        rb.gravityScale = 0; 
        yield return new WaitForSeconds(1); 
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        rb.gravityScale = gravityScale; 
        spawning = false;  
    }

    // will be overriden by child classes when this enemy attacks 
    public abstract IEnumerator Attack(); 
    
    // Waits 1 second for death animation to play then disables enemy 
    // can be overriden in child class if different enemies need different 
    // death effects 
    public virtual IEnumerator Die() {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false); 
    }

    // When this enemy collides with another object 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && health > 0 && !spawning)
        {
            StartCoroutine(collision.gameObject.GetComponent<Movement>().Damage(damage, direction, knockbackx, knockbacky)); 
        }
            
    }

    // When this enemy collides with another object 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && health > 0 && !spawning)
        {
            StartCoroutine(collision.gameObject.GetComponent<Movement>().Damage(damage, direction, knockbackx, knockbacky)); 
        }
            
    }

    // deals damage to this enemy and knocks it back based on given variables 
    public IEnumerator Damage(int damage, float direction, float knockbackx, float knockbacky) {
        if(spawning)
            yield break; 
        gettingHit = true; 
        health -= damage; 
        rb.velocity = new Vector2(direction * knockbackx, knockbacky); 
        hitEffect.SetActive(true); 
        yield return new WaitForSeconds(.5f); // apply knockback 
        hitEffect.SetActive(false); 
        yield return new WaitForSeconds(.5f); // enemy stun 
        gettingHit = false; 
    }

    // removes an enemy from hash table that stores all the alive enemies in the round 
    // also makes it so this enemy cannot be touched and freezes its position 
    public void Remove() {
        g.RemoveEnemy(this.id); // remove from list of enemies 
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true; // cant effect envirinment 
        rb.constraints = RigidbodyConstraints2D.FreezePosition; // stop moving 
        Instantiate(DropItem(), transform.position, transform.rotation); // drop item 
        anim.SetBool("Walk", false); // stop animating 
        anim.SetBool("Attack", false); 
        anim.SetBool("Die", true); 
    }

    // initializes the id of this enemy used for finding instance of enemy in hash table 
    public void SetId(int id) {
        this.id = id; 
    }

    // Returns is this enemy is alive 
    public bool Alive() {
        return health > 0; 
    }

    // returns this enemies id
    public int GetId() {
        return id; 
    }

    // drops a random item in this enemies list of items 
    private GameObject DropItem() {
        int rand = Random.Range(0, drops.Length);
        return drops[rand]; 
    }

}
