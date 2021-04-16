using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudStack : MonoBehaviour
{
    [SerializeField] GameObject Quad;
    [SerializeField] int horizontalStackSize = 50;
    float cloudHeight;
    [SerializeField] float offset = 5;
    [SerializeField] Material clouds;
    Vector3 StartPos;
    // Start is called before the first frame update
    void Start()
    {
        cloudHeight = transform.position.y;
        clouds.SetFloat("_midYValue", cloudHeight);
        clouds.SetFloat("_cloudHeight", cloudHeight);
        offset = cloudHeight / horizontalStackSize / 2f;
        StartPos = transform.position + (Vector3.up * (offset * horizontalStackSize / 2f));
        for (int i = 0; i < horizontalStackSize; i++)
        {
            Instantiate(Quad, StartPos - (Vector3.up * offset * i), transform.rotation, transform); 
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
