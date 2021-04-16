using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer( typeof( TitleAttribute))]
public class Title : DecoratorDrawer
{
    Texture2D image;
    TitleAttribute _attribute = new TitleAttribute();
    

    public override void OnGUI(Rect position)
    {
        if(image == null)
           image = Resources.Load<Texture2D>("VenetianSkiesSystemTitle");

        _attribute = (TitleAttribute)attribute;

        GUI.DrawTexture(position, image);
    }
    public override float GetHeight()
    {
        return base.GetHeight() + _attribute.height;
    }
}
