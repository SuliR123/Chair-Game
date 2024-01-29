using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Shape {

    // Generates all the wall sprites this shape has 
    void create(); 

    // Moves this shape to given point 
    void shiftTo(Vector2 point); 

    // checks if this shape overlaps with another shape 
    bool overlap(Shape shape);

    List<Vector2> getOutline();  

    bool contains(Vector2 point); 

    // distance between the 2 closest points in this shape and given shape 
    float distance(Shape shape); 

    // calculates the area this shape takes up 
    float area(); 

}
