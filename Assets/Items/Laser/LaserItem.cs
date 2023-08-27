using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserItem : Item
{
    [SerializeField] private GameObject chargeEffect; 
    [SerializeField] private GameObject explodeEffect; 
    [SerializeField] private GameObject sprites; 

    // on collision with anything it explodes after one second 
    // stuns all the eneimes in the area
    public override IEnumerator Effect(GameObject enemy) {
        if(enemy.tag == "Jump") {
            yield break; 
        }
        boxCollider.isTrigger = false; 
        chargeEffect.SetActive(true); 
        rb.constraints = RigidbodyConstraints2D.FreezePosition; 
        yield return new WaitForSeconds(1); 
        sprites.SetActive(false); 
        explodeEffect.SetActive(true); 
        chargeEffect.SetActive(false); 
        boxCollider.isTrigger = true; 
        if(Physics2D.OverlapCircle(transform.position, .5f, LayerMask.GetMask("Enemy"))) {
            // gets all enemies within certain range and applies damage 
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 1.5f, LayerMask.GetMask("Enemy")); 
            foreach(Collider2D item in enemies) {
                StartCoroutine(item.gameObject.GetComponent<Enemy>().Damage(damage, 1, 0, 0)); 
            }
        }
        yield return new WaitForSeconds(1); 
        explodeEffect.SetActive(false); 
        transform.position = new Vector3(100,100,1); 
    }
}
