using System;
using UnityEngine;

public class LevelTriggerGas : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                other.gameObject.GetComponent<Hero>().currentEquipment.FillGas();
                UnityEngine.Object.Destroy(base.gameObject);
            }
            else if (other.gameObject.GetComponent<Hero>().photonView.isMine)
            {
                other.gameObject.GetComponent<Hero>().currentEquipment.FillGas();
                UnityEngine.Object.Destroy(base.gameObject);
            }
        }
    }

    private void Start()
    {
    }
}

