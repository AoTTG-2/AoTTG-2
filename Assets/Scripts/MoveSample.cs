using System;
using UnityEngine;

public class MoveSample : MonoBehaviour
{
    private void Start()
    {
        object[] args = new object[] { "x", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", 0.1 };
        iTween.MoveBy(base.gameObject, iTween.Hash(args));
    }
}

