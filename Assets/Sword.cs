using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Players sword class, handles damaging enemies 
public class Sword : MonoBehaviour
{

    private Transform player; 
    private int damage = 1; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Chair").transform; 
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag.Equals("Enemy") && player.GetComponent<Movement>().attacking) {
            StartCoroutine(collision.gameObject.GetComponent<Enemy>().Damage(damage, player.transform.localScale.x, 3, 3)); 
        }
    }
}
