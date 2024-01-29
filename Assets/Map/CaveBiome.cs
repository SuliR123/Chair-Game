using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveBiome : ABiome {

    private readonly GameObject display; 
    private readonly Transform parent; 

    public CaveBiome(float minX, float maxX, float minY, float maxY, GameObject wallSprite, GameObject display, Transform parent) : base(minX, maxX, minY, maxY, wallSprite) {
        this.display = display; 
        this.parent = parent;  
    }
    
    public override void create() {
        // generate random number of random curves 
        float area = (maxX - minX) * (maxY - minY); 
        Curve[] curves = new Curve[(int) (.75f * (maxX - minX))]; 
        for(int i = 0; i < curves.Length; i++) {
            curves[i] = Curve.generateRandomCurve(minX, maxX, minY, maxY, 1f); 
            //curves[i].displayCurve(display); 
        }
        List<Shape> map = new List<Shape>(); 
        foreach(Curve c in curves) {
            foreach(Vector2 p in c.getPoints()) {
                Shape shape = new CaveShape(p, wallSprite, .25f * area, parent); 
                // check if the shape can be placed at this point   
                if(canPlace(shape, map)) {
                    map.Add(shape); 
                    shape.create(); 
                    area -= shape.area(); 
                    //Debug.Log("Area left: " + area); 
                } 
            }
        }
    }

    private bool canPlace(Shape shape, List<Shape> shapes) {
        foreach(Shape s in shapes) {
            if(shape.distance(s) < 5f) {
                return false; 
            }
        }
        return true; 
    }
}
