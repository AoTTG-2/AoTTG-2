using System;
using UnityEngine;

public class RotateSample : MonoBehaviour
{
    private void Start()
    {
        object[] args = new object[] { "x", 0.25, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", 0.4 };
        iTween.RotateBy(base.gameObject, iTween.Hash(args));
    }
}

