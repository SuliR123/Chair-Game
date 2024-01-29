using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

*/
public class MapGeneration : MonoBehaviour
{

    [SerializeField] private float width; 
    [SerializeField] private float height; 
    private float area; 

    private readonly float X_STEP = .5f; 
    private readonly float Y_STEP = .5f;  

    private float minX; // left border of the map 
    private float maxX; // right border of the map 
    private float maxY; // top border of the map 

    [SerializeField] private GameObject wallSprite; 

    [SerializeField] private GameObject curveSprite; 

    [SerializeField] private Transform parent;  

    // Start is called before the first frame update
    void Start()
    {
        area = width * height; 
        minX = -width / 2; 
        maxX = width / 2; 
        maxY = height; 
        createMap(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void createMap() {
         // create border outline 
        float halfX = width / 2; 
        for(float y = 0; y <= height; y++) {
            Instantiate(wallSprite, new Vector3(-halfX, y, 0), wallSprite.transform.rotation); 
            Instantiate(wallSprite, new Vector3(halfX, y, 0), wallSprite.transform.rotation); 
        }
        for(float x = -halfX; x <= halfX; x++) {
            Instantiate(wallSprite, new Vector3(x, 0, 0), wallSprite.transform.rotation); 
            Instantiate(wallSprite, new Vector3(x, height, 0), wallSprite.transform.rotation); 
        }

        Biome cave = new CaveBiome(-halfX, halfX, 0, height, wallSprite, curveSprite, parent); 
        cave.create(); 
    }

    /*
        COOL PROJECT IDEA 
        Create program that can take in a drawing as an input and generate a map based on the drawing by following the lines and spaces from the drawing 
        Program will place wall sprites given by the user to generate the map just based off black lines. 
        Eventually make it advanced enough to generate map based off any image and not just black and white lines. 
    */

}






