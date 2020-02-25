using System;
using UnityEngine;

public class TestDontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
    }
}

