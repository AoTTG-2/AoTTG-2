using UnityEngine;

public class RacingCheckpointTrigger : Assets.Scripts.Gamemode.Racing.RacingGameComponent
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.layer == 8)
        {
            Hero collidingHero = gameObject.transform.root.gameObject.GetComponent<Hero>();
            if (collidingHero?.photonView.isMine??false)
            {
                collidingHero.fillGas();
                Gamemode.racingSpawnPoint = base.gameObject.transform.position;
                Gamemode.racingSpawnPointSet = true;
                Gamemode.InGameHUD.Chat.AddMessage("<color=#00ff00>Checkpoint set.</color>");
            }
        }
    }
}

