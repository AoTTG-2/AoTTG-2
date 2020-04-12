using Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Assets.Scripts.Gamemode.Options;
using UnityEngine;

public class Bomb : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos = Vector3.zero;
    private Quaternion correctPlayerRot = Quaternion.identity;
    private Vector3 correctPlayerVelocity = Vector3.zero;
    public bool disabled;
    public GameObject myExplosion;
    public float SmoothingDelay = 10f;

    public void Awake()
    {
        if (base.photonView != null)
        {
            float num2;
            float num3;
            float num4;
            base.photonView.observed = this;
            this.correctPlayerPos = base.transform.position;
            this.correctPlayerRot = Quaternion.identity;
            PhotonPlayer owner = base.photonView.owner;
            if (FengGameManagerMKII.Gamemode.TeamMode != TeamMode.Disabled)
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
                    base.GetComponent<ParticleSystem>().startColor = new Color(num2, num3, num4, 1f);
                }
            }
            else
            {
                num2 = RCextensions.returnFloatFromObject(owner.CustomProperties[PhotonPlayerProperty.RCBombR]);
                num3 = RCextensions.returnFloatFromObject(owner.CustomProperties[PhotonPlayerProperty.RCBombG]);
                num4 = RCextensions.returnFloatFromObject(owner.CustomProperties[PhotonPlayerProperty.RCBombB]);
                base.GetComponent<ParticleSystem>().startColor = new Color(num2, num3, num4, 1f);
            }
        }
    }

    public void destroyMe()
    {
        if (base.photonView.isMine)
        {
            if (this.myExplosion != null)
            {
                PhotonNetwork.Destroy(this.myExplosion);
            }
            PhotonNetwork.Destroy(base.gameObject);
        }
    }

    public void Explode(float radius)
    {
        this.disabled = true;
        base.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Vector3 position = base.transform.position;
        this.myExplosion = PhotonNetwork.Instantiate("RCAsset/BombExplodeMain", position, Quaternion.Euler(0f, 0f, 0f), 0);
        foreach (Hero hero in FengGameManagerMKII.instance.getPlayers())
        {
            GameObject gameObject = hero.gameObject;
            if (((Vector3.Distance(gameObject.transform.position, position) < radius) && !gameObject.GetPhotonView().isMine) && !hero.bombImmune)
            {
                PhotonPlayer owner = gameObject.GetPhotonView().owner;
                if (((FengGameManagerMKII.Gamemode.TeamMode != TeamMode.Disabled) && (PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam] != null)) && (owner.CustomProperties[PhotonPlayerProperty.RCteam] != null))
                {
                    int num = RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]);
                    int num2 = RCextensions.returnIntFromObject(owner.CustomProperties[PhotonPlayerProperty.RCteam]);
                    if ((num == 0) || (num != num2))
                    {
                        gameObject.GetComponent<Hero>().markDie();
                        gameObject.GetComponent<Hero>().photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]) + " " });
                        FengGameManagerMKII.instance.playerKillInfoUpdate(PhotonNetwork.player, 0);
                    }
                }
                else
                {
                    gameObject.GetComponent<Hero>().markDie();
                    gameObject.GetComponent<Hero>().photonView.RPC("netDie2", PhotonTargets.All, new object[] { -1, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]) + " " });
                    FengGameManagerMKII.instance.playerKillInfoUpdate(PhotonNetwork.player, 0);
                }
            }
        }
        base.StartCoroutine(this.WaitAndFade(1.5f));
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(base.transform.position);
            stream.SendNext(base.transform.rotation);
            stream.SendNext(base.GetComponent<Rigidbody>().velocity);
        }
        else
        {
            this.correctPlayerPos = (Vector3) stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
            this.correctPlayerVelocity = (Vector3) stream.ReceiveNext();
        }
    }

    public void Update()
    {
        if (!(this.disabled || base.photonView.isMine))
        {
            base.transform.position = Vector3.Lerp(base.transform.position, this.correctPlayerPos, Time.deltaTime * this.SmoothingDelay);
            base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.correctPlayerRot, Time.deltaTime * this.SmoothingDelay);
            base.GetComponent<Rigidbody>().velocity = this.correctPlayerVelocity;
        }
    }

    private IEnumerator WaitAndFade(float time)
    {
        yield return new WaitForSeconds(time);
        PhotonNetwork.Destroy(this.myExplosion);
        PhotonNetwork.Destroy(this.gameObject);
    }
}