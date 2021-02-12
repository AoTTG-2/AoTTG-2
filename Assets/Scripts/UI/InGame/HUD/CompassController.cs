using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassController : MonoBehaviour
{

    public bool compassRun;
    public RawImage compassImage;
    public Transform cam;

    void Start()
    {

    }

    void Update()
    {
        if(compassRun)
        {
            cam = GameObject.Find("MainCamera").transform;
            compassImage.uvRect = new Rect (cam.localEulerAngles.y / 360f, 0f, 1f, 1f);
        }
    }
}
