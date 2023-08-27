using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Microwave : FlyingEnemy
{

    [SerializeField] private GameObject chargeEffect;
    [SerializeField] private GameObject explosionEffect;  
    [SerializeField] private GameObject sprites; 

    public new void Start() {
        base.Start(); 
        chargeEffect.SetActive(false); 
        explosionEffect.SetActive(false);        
    }


    public override IEnumerator Attack() {
        attacking = true;
        rb.gravityScale = 0; 
        rb.velocity = new Vector2(0,0); 
        Vector2 velocity = FlyIntoPlayer(5);
        yield return new WaitForSeconds(1f); // wait 1 second before launching attack 
        anim.SetBool("Attack", true);
        rb.velocity = velocity; 
        boxCollider.isTrigger = true; 
        yield return new WaitForSeconds(1f); // attacking 
        boxCollider.isTrigger = false; 
        yield return new WaitForSeconds(1.5f); // cooldown 
        anim.SetBool("Attack", false); 
        attacking = false;  
    }
}
