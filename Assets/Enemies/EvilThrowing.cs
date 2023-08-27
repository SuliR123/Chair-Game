using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
represents a basic projectile 
deals damage and knockback on collision with the player 
can apply a force to this gameObject 
*/ 

public class EvilThrowing : EnemyThrowing
{

    [SerializeField] private int damage; 
    private Rigidbody2D rb; 
    [SerializeField] private float knockbackx; 
    [SerializeField] private float knockbacky; 


    // Start is called before the first frame update
    public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    public void Update()
    {

    }

    // fires this object with given x and y value 
    // direction will be passed in with the x force 
    public override void Throw(float x, float y) {
        rb.velocity = new Vector2(x, y); 
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            float direction = 1; 
            // if the object is coming from the right apply knockback left else apply knockback right 
            if(transform.position.x - collision.gameObject.transform.position.x > 0)
                direction = -1; 
            StartCoroutine(collision.gameObject.GetComponent<Movement>().Damage(damage, direction, knockbackx, knockbacky)); 
            transform.position = new Vector3(100,100,0); 
        }
    }

}
