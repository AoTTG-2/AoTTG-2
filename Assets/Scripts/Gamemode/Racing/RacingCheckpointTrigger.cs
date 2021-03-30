using Assets.Scripts;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Room.Chat;
using Assets.Scripts.Gamemode;
using UnityEngine;

public class RacingCheckpointTrigger : MonoBehaviour
{
    private InRoomChat chat;
    private RacingGamemode gamemode;

    private void Start()
    {
        chat = FindObjectOfType<InRoomChat>();
        gamemode = FindObjectOfType<RacingGamemode>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.layer == 8)
        {
            gameObject = gameObject.transform.root.gameObject;
            if (gameObject.GetPhotonView() != null && gameObject.GetPhotonView().isMine && gameObject.GetComponent<Hero>() != null)
            {
                chat.AddMessage("<color=#00ff00>Checkpoint set.</color>");
                gameObject.GetComponent<Hero>().FillGas();
                gamemode.racingSpawnPoint = base.gameObject.transform.position;
                gamemode.racingSpawnPointSet = true;
            }
        }
    }
}

