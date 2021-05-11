using System;
using Assets.Scripts.Characters.Humans;
using UnityEngine;

public class LevelTriggerGas : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<Hero>().photonView.isMine)
            {
                other.gameObject.GetComponent<Hero>().FillGas();
                UnityEngine.Object.Destroy(base.gameObject);
            }
        }
    }

    private void Start()
    {
    }
}

