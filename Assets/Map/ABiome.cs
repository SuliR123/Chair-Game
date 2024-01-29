using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABiome : Biome {

    protected readonly float minX; 
    protected readonly float maxX; 
    protected readonly float minY; 
    protected readonly float maxY; 
    protected readonly GameObject wallSprite; 

    public ABiome(float minX, float maxX, float minY, float maxY, GameObject wallSprite) {
        this.minX = minX; 
        this.minY = minY; 
        this.maxX = maxX; 
        this.maxY = maxY; 
        this.wallSprite = wallSprite; 
    }

    public abstract void create(); 

}
