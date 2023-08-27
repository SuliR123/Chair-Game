using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Apple, throwable item 
the apple is an item that detonates in the air 1 second after thrown 
overrides the abstract classe items update because it doesn't activate the effect based on collision 
shoots 3 doctor tools on detonation 
*/

public class Apple : Item
{

    [SerializeField] private GameObject tool; 
    private GameObject[] tools;
    [SerializeField] GameObject sprites; 

    private bool blowUp = false; 
    private bool dontKeepBlowingUp = false; 

    public new void Start() {
        base.Start(); 
        tools = new GameObject[3]; 
        StartCoroutine(Timer()); 
    }

    public new void Update() {


        if(blowUp && !dontKeepBlowingUp && held) {
            StartCoroutine(Effect(gameObject)); 
        }

        for(int i = 0; i < tools.Length; i++) {
            GameObject g = tools[i]; 
            if(g != null && Physics2D.OverlapCircle(g.transform.position, .5f, mask)) {
                StartCoroutine(Physics2D.OverlapCircle(g.transform.position, .5f, mask)
                .GetComponent<Enemy>().Damage(damage, Direction(g.GetComponent<Rigidbody2D>()), 2, 2));   
                g.SetActive(false);      
                tools[i] = null;         
            }
        }
    }

    IEnumerator Timer() {
        yield return new WaitForSeconds(.2f); 
        blowUp = true; 
    }

    public override IEnumerator Effect(GameObject nothing) {
        dontKeepBlowingUp = true; 
        sprites.SetActive(false); 
        float angle = 2.5f; 
        for(int i = 0; i < 3; i++) {
            GameObject item = Instantiate(tool, transform.position, transform.rotation); 
            tools[i] = item; 
            item.GetComponent<Rigidbody2D>().gravityScale = 0; 
            item.GetComponent<Rigidbody2D>().velocity = new Vector2(Direction(rb) * 10, angle); 
            angle -= 2.5f; 
        }
        yield return new WaitForSeconds(0); 
    }

}
