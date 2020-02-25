using System;
using UnityEngine;

public class RandomHouse : MonoBehaviour
{
    private void Start()
    {
        base.transform.localScale = new Vector3(4f + UnityEngine.Random.Range((float) 0f, (float) 4f), 4f + UnityEngine.Random.Range((float) 0f, (float) 6f), 4f + UnityEngine.Random.Range((float) 2f, (float) 18f));
    }

    private void Update()
    {
    }
}

