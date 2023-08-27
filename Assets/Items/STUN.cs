using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
STUN, basic item, when this item hits an enemy the enemy is stunned momentarily also deals 1 damage 
*/ 

public class STUN : Item
{
    public override IEnumerator Effect(GameObject thing) {
        if(thing.GetComponent<Enemy>().Alive()) {
            StartCoroutine(thing.GetComponent<Enemy>().Damage(damage, 1, 0, 0));
            transform.position = new Vector3(100, 100, 0); 
            yield return new WaitForSeconds(0); 
        }       
    }
}
