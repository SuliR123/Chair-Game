using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanItem : Item
{

    [SerializeField] private GameObject beanPlant; 

    public override IEnumerator Effect(GameObject other) {
        GameObject plant = Instantiate(beanPlant, transform.position, beanPlant.transform.rotation); 
        yield break; 
    }
}
