using System;
using UnityEngine;

[Obsolete("Investigate if this is ever used")]
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

