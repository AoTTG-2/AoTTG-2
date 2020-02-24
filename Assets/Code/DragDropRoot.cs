using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Drag and Drop Root")]
public class DragDropRoot : MonoBehaviour
{
    public static Transform root;

    private void Awake()
    {
        root = base.transform;
    }
}

