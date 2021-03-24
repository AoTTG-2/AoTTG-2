using System;
using UnityEngine;

public class ParticleScaling : MonoBehaviour
{
    public void OnWillRenderObject()
    {
        base.GetComponent<ParticleSystem>().GetComponent<Renderer>().material.SetVector("_Center", base.transform.position);
        base.GetComponent<ParticleSystem>().GetComponent<Renderer>().material.SetVector("_Scaling", base.transform.lossyScale);
    }
}

