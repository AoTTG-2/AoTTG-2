using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TitanSpawner
{
    public TitanSpawner()
    {
        this.name = string.Empty;
        this.location = new Vector3(0f, 0f, 0f);
        this.time = 30f;
        this.endless = false;
        this.delay = 30f;
    }

    public void resetTime()
    {
        this.time = this.delay;
    }

    public float delay { get; set; }

    public bool endless { get; set; }

    public Vector3 location { get; set; }

    public string name { get; set; }

    public float time { get; set; }
}

