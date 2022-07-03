using Assets.Scripts;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Gamemode;
using UnityEngine;
using Assets.Scripts.Services;
using Assets.Scripts.Room;

/// <summary>
/// Used for the <see cref="RacingGamemode"/> which will set the GameObject to which this component is attached as the checkpoint. Once the player dies, they will respawn here.
/// </summary>
public class RacingCheckpointTrigger : MonoBehaviour
{
    public bool fillGas = true;
    private HumanSpawner spawner;
    private void OnTriggerEnter(Collider other)
    {
        Service.Spawn.RespawnSpawner = spawner;
        GameObject gameObject = other.gameObject;
        if (gameObject.layer == 8)
        {
            gameObject = gameObject.transform.root.gameObject;
            if (gameObject.GetPhotonView() != null && gameObject.GetPhotonView().isMine && gameObject.GetComponent<Hero>() != null)
            {
                FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#00ff00>Checkpoint set.</color>");
                if (fillGas)
                { gameObject.GetComponent<Hero>().FillGas(); }
                FengGameManagerMKII.instance.racingSpawnPoint = base.gameObject.transform.position;
                FengGameManagerMKII.instance.racingSpawnPointSet = true;
            }
        }
    }
    private void Awake()
    {
        gameObject.AddComponent<HumanSpawner>();
        spawner = GetComponent<HumanSpawner>();
    }
}

