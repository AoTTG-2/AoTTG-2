using System;
using Assets.Scripts.Characters.Titan;
using UnityEngine;

public class TitanTrigger : MonoBehaviour
{
    private MindlessTitan Titan { get; set; }

    void Start()
    {
        Titan = gameObject.GetComponentInParent<MindlessTitan>();
    }

    public void SetCollision(bool value)
    {
        Titan.IsColliding = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Titan != null && !Titan.IsColliding)
        {
            GameObject gameObject = other.transform.root.gameObject;
            if (gameObject.layer == 8)
            {
                if (gameObject.GetPhotonView().isMine)
                {
                    Titan.IsColliding = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Titan != null && Titan.IsColliding)
        {
            GameObject gameObject = other.transform.root.gameObject;
            if (gameObject.layer == 8)
            {
                if (gameObject.GetPhotonView().isMine)
                {
                    Titan.IsColliding = false;
                }
            }
        }
    }
}

