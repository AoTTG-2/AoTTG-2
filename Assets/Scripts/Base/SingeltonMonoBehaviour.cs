using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class SingeltonMonoBehaviour<Tclass> : MonoBehaviour where Tclass : class
{
    public static Tclass Instance;

    protected virtual void Awake()
    {
        CheckSingleton();
    }

    private void CheckSingleton()
    {
        if (Instance is null)
        {
            Instance = this as Tclass;
            DontDestroyOnLoad(gameObject);
        }
        else if (this as Tclass != Instance)
        {
            Destroy(gameObject);
        }
    }
}
