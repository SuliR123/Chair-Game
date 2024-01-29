using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Utils {

    // Creates a line between 2 points
    // While the x and y distances are different between the points 
    // choose to increment in the x or y direction 
    // if either directions are fulfilled move in other direction till the points are the same 
    public static List<Vector2> createLineBetweenPoints(Vector2 start, Vector2 end) {
        List<Vector2> list = new List<Vector2>(); 
        int directionX = getDirection(start.x, end.x); 
        int directionY = getDirection(start.y, end.y); 
        Vector2 temp = start; 
        // create line between 2 points 
        while(temp != end) {
            int direction = randomDecision(); 
            if(direction == 0 && temp.x == end.x) {
                direction = 1; 
            } else if(direction == 1 && temp.y == end.y) {
                direction = 0; 
            }
            float x = temp.x; 
            float y = temp.y; 
            if(direction == 0) { // progress in x 
                x += directionX; 
            } else { // progress in y 
                y += directionY; 
            }
            temp = new Vector2(x, y); 
            list.Add(temp); 
        }
        return list; 
    }

    public static int randomDecision() {
        return (int) Random.Range(0, 2); 
    }

    private static int getDirection(float start, float end) {
        if(start > end) {
            return -1; 
        } else {
            return 1; 
        }
    }

    public static float toRadians(float degrees) {
        return degrees * pi() / 180; 
    }

    public static float pi() {
        return Mathf.PI;
    }

    public static float ninety() {
        return pi() / 2; 
    }

    public static float twoSeventy() {
        return (3 * pi()) / 2; 
    }

    public static float sin(float radians) {
        return Mathf.Sin(radians); 
    }

    public static float cos(float radians) {
        return Mathf.Cos(radians); 
    }
}
