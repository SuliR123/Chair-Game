using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Represents an enemy that has 2 possible attacks, also has access to projectile attack methods. One attack will be physical 
attack, second attack will be projectile attack. 
*/
public abstract class TwoAttackEnemy : ThrowingEnemy
{

    [SerializeField] private int attackTwoRange; 

    public new void Update() {
        base.Update(); 

        if(Vector2.Distance(player.position, transform.position) < attackTwoRange && !attacking) {
            FacePlayer(); 
            StartCoroutine(Attack2()); 
        }
    }

    // Fires second attack for enemy
    public abstract IEnumerator Attack2(); 

}
