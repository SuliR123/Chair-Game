using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Abstract flying object class: contains the basic information of a flying enemy
overrides walk and direction changing in enemies to account for different movement 
in flying objects 
*/

public abstract class FlyingEnemy : Enemy
{
    private float yDirection = 1; 
    [SerializeField] private float radius = 1; 
    private bool colliding = false; 

    public new void Start() {
        base.Start(); 
        rb.gravityScale = 0; 
    }

    public override void Walk() {
        rb.velocity = new Vector2(direction * speed, yDirection * speed); 
    }

    public override void SetDirection() {
        direction = RandomDirection(); 
        yDirection = RandomDirection(); 
    }

    // chooses a random direction 
    private int RandomDirection() {
        if((int) Random.Range(0,2) == 1)
            return 1; 
        else 
            return -1; 
    }

    // flying enemy changes direction whenever it bumps into the ground or wall around it 
    // creates cool down to stop from bouncing from side to side in place 
    public override bool ChangeDirection() {
        bool answer = (Collision() || walkingTime <= 0) && !attacking && !colliding; 
        if(answer) {
            StartCoroutine(Delay()); 
        }
        return answer; 
    }

    private bool Collision() {
        return (Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Ground")) || 
        Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Wall"))) 
        && Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Ground")).gameObject.tag != "Jump"; 
    }

    private IEnumerator Delay() {
        colliding = true; 
        yield return new WaitForSeconds(1); 
        colliding = false; 
    }

    // Calculates the vector between this enemy and the player
    // necessary to laucch this enemy to the player 
    public Vector2 FlyIntoPlayer(float speed) {
        float xForce = 0; 
        float yForce = 0; 
        Vector3 player = GameObject.Find("Chair").transform.position; 
        Vector3 position = transform.position; 
        float distance = Vector2.Distance(player, position);
        float sin = (player.y - position.y) / distance; 
        float cos = (player.x - position.x) / distance; 
        yForce = sin * speed; 
        xForce = cos * speed; 
        return new Vector2(xForce, yForce); 
    }

}
