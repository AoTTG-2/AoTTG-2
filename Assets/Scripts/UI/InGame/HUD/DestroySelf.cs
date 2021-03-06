using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float countdown = 5f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(countdown > 0)
        {
            countdown -= Time.deltaTime;
        }

        if(countdown <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
