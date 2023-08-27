using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeChair : ThrowingEnemy
{

    [SerializeField] private GameObject[] effects; 

    public new void Start() {
        base.Start(); 
        foreach(GameObject g in effects) {
            g.SetActive(false); 
        }
    }

    public override IEnumerator Attack() {
        attacking = true; 
        foreach(GameObject g in effects) {
            g.SetActive(true); 
        }
        anim.SetBool("Attack", true); 
        yield return new WaitForSeconds(1f); // display effect 
        StartCoroutine(ThrowItems(.5f, 10, 0));
        foreach(GameObject g in effects) {
            g.SetActive(false); 
        }
        yield return new WaitForSeconds(.5f); // cooldown 
        attacking = false; 
        anim.SetBool("Attack", false); 
    }

}
