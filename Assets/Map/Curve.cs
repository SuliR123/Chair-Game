using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Curve {
    
    private readonly int[] polynomial; 
    private readonly float minX; 
    private readonly float maxX; 
    private readonly float minY; 
    private readonly float maxY; 
    private readonly float step; 
    private readonly float stretch; 
    private readonly int horizontalShift; 
    private readonly int rotate; 

    public Curve(float minX, float maxX, float minY, float maxY, float step, float stretch, int horizontalShift, int rotate, int[] numbers) {
        this.polynomial = numbers; 
        this.minY = minY; 
        this.maxY = maxY; 
        this.minX = minX; 
        this.maxX = maxX; 
        this.step = step; 
        this.stretch = stretch; 
        this.horizontalShift = horizontalShift; 
        this.rotate = rotate; 
    }

    public List<Vector2> getPoints() {
        List<Vector2> points = new List<Vector2>(); 
        for(float x = minX; x <= maxX; x+=step) {
            float y = 0; 
            for(int i = 0; i < polynomial.Length; i++) {
                y += polynomial[i] * Mathf.Pow(x + horizontalShift, i); 
            }
            y *= stretch; 
            Vector2 point; 
            if(rotate == 0) {
                    point = new Vector2(x, y);   
                } else {
                    point = new Vector2(y, -x); 
                }

            if(point.y >= minY && point.y <= maxY && point.x >= minX && point.x <= maxX) {
                points.Add(point); 
            }
                       
        }
        return points; 
    }

    public static Curve generateRandomCurve(float minX, float maxX, float minY, float maxY, float step) {
        int[] polynomial = new int[(int) Random.Range(2, 6)]; 
        float stretch = Random.Range(1, 5);
        if(Utils.randomDecision() == 0) {
            stretch = 1 / stretch; 
        } 
        for(int i = 1; i < polynomial.Length; i++) {
            polynomial[i] = (int) Random.Range(-5, 5); 
        }
        int halfY = (int) maxY / 4; 
        polynomial[0] = halfY;
        int halfX = (int) maxX / 2; 
        int horizontalShift = (int) Random.Range(-maxX, maxX); 
        return new Curve(minX, maxX, minY, maxY, step, stretch, horizontalShift, Utils.randomDecision(), polynomial); 
    }

    public void displayCurve(GameObject wallSprite) {
        foreach(Vector2 p in this.getPoints()) {
            Object.Instantiate(wallSprite, new Vector3(p.x, p.y, 0), wallSprite.transform.rotation); 
       }
    }
}
