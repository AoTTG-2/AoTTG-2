using System;
using UnityEngine;

public class OnStartDelete : MonoBehaviour
{
    private void Start()
    {
        UnityEngine.Object.DestroyObject(base.gameObject);
    }
}

