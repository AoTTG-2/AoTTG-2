using System;
using UnityEngine;

public class RacingCheckpointTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.layer == 8)
        {
            gameObject = gameObject.transform.root.gameObject;
            if ((((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && (gameObject.GetPhotonView() != null)) && gameObject.GetPhotonView().isMine) && (gameObject.GetComponent<HERO>() != null))
            {
                FengGameManagerMKII.instance.chatRoom.addLINE("<color=#00ff00>Checkpoint set.</color>");
                gameObject.GetComponent<HERO>().fillGas();
                FengGameManagerMKII.instance.racingSpawnPoint = base.gameObject.transform.position;
                FengGameManagerMKII.instance.racingSpawnPointSet = true;
            }
        }
    }
}

