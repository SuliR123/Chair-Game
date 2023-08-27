using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPINNN : MonoBehaviour
{
    [SerializeField] private float angularVelocity; 

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,0,angularVelocity * Time.deltaTime)); 
    }
}
