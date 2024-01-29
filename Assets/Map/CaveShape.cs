using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CaveShape : Shape {
    private readonly Vector2 point; 
    private readonly GameObject sprite; 
    private readonly List<Vector2> outline; 
    private readonly List<Vector2> inside; 
    private readonly Transform parent; 

    private readonly float MAX_RADIUS; 

    private readonly float MIN_RADIUS = 1; 

    private readonly float stepSize; 

    private int maxX; 
    private int minX; 
    private int maxY; 
    private int minY = int.MaxValue; 

    public CaveShape(Vector2 point, GameObject sprite, float area, Transform parent) {
        this.point = point; 
        this.sprite = sprite; 
        this.parent = parent; 
        this.MAX_RADIUS = Mathf.Sqrt((area * Random.Range(.07f, .18f)) / Utils.pi()); // cave shape can take up at most 15 percent of the area of the board. 
        this.stepSize = Utils.toRadians(Random.Range(15, 45)); 
        this.outline = generateOutline();
        this.inside = generateInside(); 
    }

    // Creates the given wall sprite based on the outline of this shape. 
    public void create() {
        foreach(Vector2 p in outline) {
            GameObject g = Object.Instantiate(sprite, new Vector3(p.x, p.y, 0), sprite.transform.rotation); 
            g.transform.SetParent(parent); 
        }
        foreach(Vector2 p in inside) {
            GameObject g = Object.Instantiate(sprite, new Vector3(p.x, p.y, 0), sprite.transform.rotation); 
            g.transform.SetParent(parent); 
        }
    }

    // moves this shape to be centered around the given point. 
    public void shiftTo(Vector2 point) {
        float xDiff = point.x - this.point.x; 
        float yDiff = point.y - this.point.y; 
        for(int i = 0; i < outline.Count; i++) {
            Vector2 p = outline[i]; 
            outline[i] = new Vector2(p.x + xDiff, p.y + yDiff); 
        }
    }

    // returns true iff a point in the given shape is contained within this shape 
    public bool overlap(Shape shape) {
        List<Vector2> points = shape.getOutline(); 
        foreach(Vector2 p in points) {
            if(this.contains(p)) {
                return true; 
            }
        }
        return false; 
    }

    // returns a list of points that define the outline of a shape 
    public List<Vector2> getOutline() {
        return this.outline; 
    }

    // checks if this shape contains the given point
    public bool contains(Vector2 point) {
        // a point is within the shape iff in all 4 directions there is a piece in the outline
        float x = maxX - minX; 
        float y = maxY - minY;
        Vector2 leftPoint = point; 
        Vector2 rightPoint = point; 
        bool left = false; bool right = false; 
        for(int i = 0; i < x; i++) {
            leftPoint = new Vector2(leftPoint.x - 1, leftPoint.y); 
            rightPoint = new Vector2(rightPoint.x + 1, rightPoint.y); 
            if(outline.Contains(leftPoint)) {
                left = true; 
            }
            if(outline.Contains(rightPoint)) {
                right = true; 
            }
        }
        Vector2 upPoint = point; 
        Vector2 downPoint = point; 
        bool up = false; bool down = false; 
        for(int i = 0; i < y; i++) {
            upPoint = new Vector2(upPoint.x, upPoint.y + 1);
            downPoint = new Vector2(downPoint.x, downPoint.y - 1); 
            if(outline.Contains(upPoint)) {
                up = true; 
            }
            if (outline.Contains(downPoint)) {
                down = true; 
            }
        }
        return right && left && up && down;  
    }

    public float distance(Shape shape) {
        float distance = int.MaxValue; 
        foreach(Vector2 p in outline) {
            foreach(Vector2 other in shape.getOutline()) {
                float currentDistance = Vector2.Distance(p, other); 
                if(currentDistance < distance) {
                    distance = currentDistance; 
                }
            }
        }
        return distance; 
    }

    // returns the area of square the encompasses the shape (estimate of the area of the shape to save on calculation time)
    public float area() {
        return outline.Count + inside.Count; 
    }
    
    // creates cave shape by: 
    // 1) creating a circle with randomly selected degree step size
    // 2) At each step in the circle select a random radius center of the shape, radius = O(.75f * area shape is in), and place a sprite 
    // 3) Connect each sprite in the shape with a randomly generated path
    private List<Vector2> generateOutline() {
        List<Vector2> range = new List<Vector2>(); 
        // Create outline for shape 
        for(float i = 0; i < Utils.toRadians(360); i += stepSize) {
            float radius = Random.Range(MIN_RADIUS, MAX_RADIUS); 
            float x = radius * Utils.cos(i) + point.x;
            float y = radius * Utils.sin(i) + point.y; 
            this.maxX = (int) Mathf.Max(maxX, x); 
            this.maxY = (int) Mathf.Max(maxY, y); 
            this.minX = (int) Mathf.Min(minX, x); 
            this.minY = (int) Mathf.Min(minY, y); 
            range.Add(new Vector2((int) x, (int) y)); 
        }
        // connect outline together via lines between the points 
        List<Vector2> list = new List<Vector2>(); 
        for(int i = 0; i < range.Count; i++) {
            Vector2 start = range[i]; 
            Vector2 end;
            if(i == range.Count - 1) {  // connect the last point to the first point 
                end = range[0]; 
            } else {
                end = range[i + 1]; 
            }
            list.AddRange(Utils.createLineBetweenPoints(start, end));  
        }

        foreach(Vector2 p in list) {
            range.Add(p); 
        }
        return range; 
    }

    private List<Vector2> generateInside() {
        List<Vector2> list = new List<Vector2>(); 
        for(int r = minY; r <= maxY; r++) {
            for(int c = minX; c <= maxX; c++) {
                Vector2 current = new Vector2(c, r); 
                if(contains(current)) {
                    list.Add(current); 
                }
            }
        }
        return list; 
    }

    
}
