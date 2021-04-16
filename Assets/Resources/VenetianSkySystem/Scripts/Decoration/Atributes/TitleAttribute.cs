using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAttribute : PropertyAttribute
{
    public int height;
    public TitleAttribute()
    {
        height = 30;
    }
    public TitleAttribute(int _height)
    {
        height = _height;
    }
}
