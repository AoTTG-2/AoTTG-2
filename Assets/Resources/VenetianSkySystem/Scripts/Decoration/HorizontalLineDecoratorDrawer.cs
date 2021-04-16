using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer( typeof( HorizontalLineAttribute))]
public class HorizontalLineDecoratorDrawer : DecoratorDrawer
{
    public override float GetHeight()
    {
        return base.GetHeight();
    }
    public override void OnGUI(Rect position)
    {
        GUI.Box(new Rect(position.xMin, position.yMin + 4, position.width, 3), GUIContent.none);
    }
}
