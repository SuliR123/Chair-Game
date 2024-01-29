using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanBag : TwoAttackEnemy
{

    [SerializeField] private GameObject fartEffect; 
    [SerializeField] private Transform hitArea; 

    public new void Start() {
        base.Start(); 
        fartEffect.GetComponent<ParticleSystem>().Stop(); 
    }

    // Bean bag fart attack 
    public override IEnumerator Attack() {
        attacking = true; 
        yield return new WaitForSeconds(.5f); // attack delay
        anim.SetBool("Attack1", true); 
        yield return new WaitForSeconds(.5f); // half a second for fart to charge up LOL 
        Vector3 localScale = fartEffect.transform.localScale; 
        if(transform.localScale.x == -1) {
            localScale.x = 1;
        }
        else {
            localScale.x = -1; 
        }
        fartEffect.transform.localScale = localScale; 
        fartEffect.GetComponent<ParticleSystem>().Play(); 
        // Deal damage to player within fart radius 
        if(Physics2D.OverlapCircle(hitArea.position, 1f, LayerMask.GetMask("Player"))) {
            StartCoroutine(Physics2D.OverlapCircle(hitArea.position, 3f, LayerMask.GetMask("Player")).GetComponent<Movement>().Damage(damage, direction, 0, 0)); 
        }
        yield return new WaitForSeconds(2.5f); // cool down and let the fart play out 
        fartEffect.GetComponent<ParticleSystem>().Stop(); 
        attacking = false; 
        anim.SetBool("Attack1", false); 
    }

    // SHOOTS A BEAN FROM THE GLOCK 
    public override IEnumerator Attack2() {
        print("attack 2"); 
        attacking = true; 
        yield return new WaitForSeconds(.5f); // attack delay 
        anim.SetBool("Attack2", true); 
        StartCoroutine(ThrowItems(0, 10, 0)); // FIRE ONE BEAN 
        yield return new WaitForSeconds(1f); // attack cooldown 
        attacking = false; 
        anim.SetBool("Attack2", false); 
    }
}
