using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
represents the most abstract type of projectile an enemy can throw 
contains a method to be overriden that launches this object 
*/

public abstract class EnemyThrowing : MonoBehaviour
{
    // launches an object given an x and y value 
    public abstract void Throw(float x, float y); 
}
