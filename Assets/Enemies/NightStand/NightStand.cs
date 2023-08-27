using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; 

public class NightStand : Enemy
{

    public override IEnumerator Attack() {
        attacking = true; 
        yield return new WaitForSeconds(.5f); // delay before attacking
        anim.SetBool("Attack", true); 
        yield return new WaitForSeconds(.1f); // based on animation 
        rb.velocity = new Vector2(transform.localScale.x * 3, 3); 
        yield return new WaitForSeconds(.2f); //based on animation 
        anim.SetBool("Attack", false); 
        yield return new WaitForSeconds(2); // cooldown 
        attacking = false; 
    }

    public override IEnumerator Die() {
        yield return new WaitForSeconds(2f); 
        gameObject.SetActive(false);       
    }

}
