using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceRemoval : MonoBehaviour
{
    float countdown;
    public float Timer = 3f;
    // Start is called before the first frame update
    void Start()
    {
        countdown = Timer;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0f)
        {
            Destroy(gameObject);

        }
       
    }
}
