using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRIDGE : ThrowingEnemy 
{

    public override IEnumerator Attack() {
        attacking = true; 
        anim.SetBool("Attack", true); 
        yield return new WaitForSeconds(.5f); // slight delay on attack 
        StartCoroutine(ThrowItems(.2f, 10, 0)); 
        yield return new WaitForSeconds(1.4f); // firing all the items 
        anim.SetBool("Attack", false); 
        yield return new WaitForSeconds(1f); // cooldown 
        attacking = false; 
    }

}
