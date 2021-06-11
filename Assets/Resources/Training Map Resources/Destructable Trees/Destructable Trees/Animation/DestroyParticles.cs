using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       Object.Destroy(gameObject, 5.0f);
    }
}
