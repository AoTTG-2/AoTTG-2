using System;
using UnityEngine;

public class LevelTriggerCheckPoint : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<Hero>().photonView.isMine)
            {
                FengGameManagerMKII.instance.checkpoint = gameObject;
            }
        }
    }

    private void Start()
    {
    }
}

