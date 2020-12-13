using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings;
using UnityEngine;

public class BombExplode : Photon.MonoBehaviour
{
    public GameObject myExplosion;

    public void Start()
    {
        if (base.photonView != null)
        {
            float num2;
            float num3;
            float num4;
            float num5;
            PhotonPlayer owner = base.photonView.owner;
            if (GameSettings.Gamemode.TeamMode != TeamMode.Disabled)
            {
                int num = RCextensions.returnIntFromObject(owner.CustomProperties[PhotonPlayerProperty.RCteam]);
                if (num == 1)
                {
                    base.GetComponent<ParticleSystem>().startColor = Color.cyan;
                }
                else if (num == 2)
                {
                    base.GetComponent<ParticleSystem>().startColor = Color.magenta;
                }
                else
                {
                    num2 = RCextensions.returnFloatFromObject(owner.CustomProperties[PhotonPlayerProperty.RCBombR]);
                    num3 = RCextensions.returnFloatFromObject(owner.CustomProperties[PhotonPlayerProperty.RCBombG]);
                    num4 = RCextensions.returnFloatFromObject(owner.CustomProperties[PhotonPlayerProperty.RCBombB]);
                    num5 = RCextensions.returnFloatFromObject(owner.CustomProperties[PhotonPlayerProperty.RCBombA]);
                    num5 = Mathf.Max(0.5f, num5);
                    base.GetComponent<ParticleSystem>().startColor = new Color(num2, num3, num4, num5);
                }
            }
            else
            {
                num2 = RCextensions.returnFloatFromObject(owner.CustomProperties[PhotonPlayerProperty.RCBombR]);
                num3 = RCextensions.returnFloatFromObject(owner.CustomProperties[PhotonPlayerProperty.RCBombG]);
                num4 = RCextensions.returnFloatFromObject(owner.CustomProperties[PhotonPlayerProperty.RCBombB]);
                num5 = RCextensions.returnFloatFromObject(owner.CustomProperties[PhotonPlayerProperty.RCBombA]);
                num5 = Mathf.Max(0.5f, num5);
                base.GetComponent<ParticleSystem>().startColor = new Color(num2, num3, num4, num5);
            }
            float num6 = RCextensions.returnFloatFromObject(owner.CustomProperties[PhotonPlayerProperty.RCBombRadius]) * 2f;
            num6 = Mathf.Clamp(num6, 40f, 120f);
            base.GetComponent<ParticleSystem>().startSize = num6;
        }
    }
}

