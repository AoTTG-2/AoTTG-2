using System;
using UnityEngine;

public class RCRegion
{
    private float dimX;
    private float dimY;
    private float dimZ;
    public Vector3 location;
    public GameObject myBox;

    public RCRegion(Vector3 loc, float x, float y, float z)
    {
        this.location = loc;
        this.dimX = x;
        this.dimY = y;
        this.dimZ = z;
    }

    public float GetRandomX()
    {
        return (this.location.x + UnityEngine.Random.Range((float) (-this.dimX / 2f), (float) (this.dimX / 2f)));
    }

    public float GetRandomY()
    {
        return (this.location.y + UnityEngine.Random.Range((float) (-this.dimY / 2f), (float) (this.dimY / 2f)));
    }

    public float GetRandomZ()
    {
        return (this.location.z + UnityEngine.Random.Range((float) (-this.dimZ / 2f), (float) (this.dimZ / 2f)));
    }
}

