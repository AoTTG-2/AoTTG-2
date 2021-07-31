using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings;
using UnityEngine;

public class BombExplode : Photon.MonoBehaviour
{
    public GameObject myExplosion;
    public static float size;
    public void Start()
    {
        if (base.photonView != null)
        {
            size = size * 2;
            PhotonPlayer owner = base.photonView.owner;
            base.GetComponent<ParticleSystem>().startColor = new Color(255, 255, 255, 0.5f);
            base.GetComponent<ParticleSystem>().startSize = size;
        }
    }
}

