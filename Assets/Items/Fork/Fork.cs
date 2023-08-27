using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
throwable item: on contact with enemy will zap up to 3 enemies if they are all close to each other 
*/

public class Fork : Item
{

    Hashtable alreadyHit = new Hashtable(); // change to HashSet
    [SerializeField] private GameObject stunEffect; 

    // zaps up to 3 enemies if they are within certain range of each other 
    public override IEnumerator Effect(GameObject enemy) {
        transform.position = new Vector3(100, 100, 0); // move this item away from collision point 
        Vector3 position = enemy.transform.position; // gets the enemies positon 
        int zapped = 3; 
        while(zapped > 0 && Physics2D.OverlapCircle(position, 3f, mask)) { 
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 3f, mask); // gets an array of all enemies within the range 
            foreach(Collider2D c in colliders) {
                GameObject e = c.gameObject; 
                Enemy enemyScript = e.GetComponent<Enemy>(); 
                if(!alreadyHit.Contains(enemyScript.GetId())) { // check to make sure we don't hit the same enemy twice 
                    // make zappy effect on enemy
                    Instantiate(stunEffect, e.transform.position, e.transform.rotation); 
                    StartCoroutine(enemyScript.Damage(1, 1, 0, 0)); // deal damage to enemy and change hit effect color 
                    position = e.transform.position; // get new position 
                    zapped--; 
                    alreadyHit.Add(enemyScript.GetId(), e); 
                    yield return new WaitForSeconds(1f); // create delay between hits 
                }
                else 
                    zapped = 0; // exit loop 
            }
            
        }
        yield return new WaitForSeconds(0); 
    }
}
