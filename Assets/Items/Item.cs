using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract item class creates protocol for all items 
public abstract class Item : MonoBehaviour
{

    public int damage; 
    [HideInInspector] public Rigidbody2D rb; 
    [HideInInspector] public BoxCollider2D boxCollider;
    private int ammo = 5; 
    public LayerMask mask; 
    [HideInInspector] public bool held = false; // checks if this object is held 
    public float force = 10; 

    private Game g;

    string key; 

    // Start is called before the first frame update
    public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
        boxCollider = gameObject.GetComponent<BoxCollider2D>(); 
        g = GameObject.Find("GameTime").GetComponent<Game>(); 
        key = gameObject.name; 
        key = key.Substring(0, key.IndexOf("("));
        StartCoroutine(Despawn()); 
    }

    // Update is called once per frame
    public void Update()
    {
        // if the item collides with the given collision paramaters then activate its effect 
        bool found = Physics2D.OverlapCircle(transform.position, .2f, mask); 
        if(found && held) {
            GameObject thing = Physics2D.OverlapCircle(transform.position, .2f, mask).gameObject; 
            StartCoroutine(Effect(thing)); 
        }
    }

    // displays this item's ui on the screen 
    public void DisplayItem() {
        g.DisplayItem(key); 
    }

    // sets if this item is held by the player 
    public void SetHeld(bool held) {
        this.held = held; 
    }

    // checks if the player can throw this item 
    public bool CanThrow() {
        return ammo > 0; 
    }

    // throws a copy of this item 
    public void Throw(float x, Transform hand) {
        if(ammo == 0)
            return; 
        ammo--; 
        GameObject wee = Instantiate(gameObject, hand.position, hand.rotation); 
        wee.SetActive(true); 
        wee.GetComponent<BoxCollider2D>().isTrigger = true; 
        wee.GetComponent<Rigidbody2D>().velocity = new Vector2(x * force, 3); 
        g.Throw(key, ammo); // disable sprite in ui 
    }

    // dropping this item 
    public void Drop(Transform player) {
        if(ammo == 0)
            return; 
        transform.SetParent(null);
        rb.isKinematic = false; 
        rb.gravityScale = 1; 
        rb.velocity = new Vector2(player.localScale.x * 10, 4);  
    }

    // when this item collides with the intended object activate its effect 
    public abstract IEnumerator Effect(GameObject thing); 

    // returns the direction given object is facing on the x axis 
    public float Direction(Rigidbody2D r) {
        float velocity = r.velocity.x; 
        if(velocity >= 0)
            return 1;
        else 
            return -1; 
    }

    // after 20 seconds if this item isn't held destroy it 
    private IEnumerator Despawn() {
        yield return new WaitForSeconds(20f); 
        if(gameObject.activeSelf)
            Destroy(gameObject); 
    }

}
