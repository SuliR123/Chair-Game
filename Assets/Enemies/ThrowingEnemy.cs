using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Represents an enemy that fires given items 
*/

public abstract class ThrowingEnemy : Enemy
{
    [SerializeField] private GameObject[] throwingItems; 

    // fires every item in items array with delay between shots and given x and y speed
    // direction doesn't need to be accounted for when passing in x speed 
    public IEnumerator ThrowItems(float delay, float x, float y) {
        foreach (GameObject item in throwingItems)
        {
            if(Alive()) {
                EnemyThrowing e = item.GetComponent<EnemyThrowing>(); 
                e.Throw(transform.localScale.x * x, y); 
                yield return new WaitForSeconds(delay); // create delay between shots 
            }
        }
    }

}
