using System;
using UnityEngine;

public class RacingGasTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.layer == 8)
        {
            gameObject = gameObject.transform.root.gameObject;
            if ((((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && (gameObject.GetPhotonView() != null)) && gameObject.GetPhotonView().isMine) && (gameObject.GetComponent<Hero>() != null))
            {
                gameObject.GetComponent<Hero>().fillGas();
            }
        }
    }
}