using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
represents an enemy that shoots projectiles in a gun like fashion 
fires from firespot and instantiates given bullet 
*/ 

public class PEWPEW : EnemyThrowing
{

    [SerializeField] private Transform fireSpot; 
    [SerializeField] private GameObject bullet;

    // fires given bullet at fireSpot's position with passed in x and y values 
    public override void Throw(float x, float y) {
        GameObject pew = Instantiate(bullet, fireSpot.position, fireSpot.rotation); 
        pew.GetComponent<Rigidbody2D>().gravityScale = 0; 
        pew.GetComponent<EvilThrowing>().Start(); 
        pew.GetComponent<EvilThrowing>().Throw(x, y); 
    }

}
