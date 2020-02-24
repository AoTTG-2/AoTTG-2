using System;
using UnityEngine;

public class CatchDestroy : MonoBehaviour
{
    public GameObject target;

    private void OnDestroy()
    {
        if (this.target != null)
        {
            UnityEngine.Object.Destroy(this.target);
        }
    }
}

