using Assets.Scripts.Characters.Humans;
using UnityEngine;

public class RacingKillTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.layer == 8)
        {
            gameObject = gameObject.transform.root.gameObject;
            if (((gameObject.GetPhotonView() != null)) && gameObject.GetPhotonView().isMine)
            {
                Hero component = gameObject.GetComponent<Hero>();
                if (component != null)
                {
                    component.MarkDie();
                    component.photonView.RPC(nameof(Hero.NetDie2), PhotonTargets.All, new object[] { -1, "Server" });
                }
            }
        }
    }
}

