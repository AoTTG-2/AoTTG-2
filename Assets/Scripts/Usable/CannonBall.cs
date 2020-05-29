using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : Photon.MonoBehaviour
{
    private Vector3 correctPos;
    private Vector3 correctVelocity;
    public bool disabled;
    public Transform firingPoint;
    public bool isCollider;
    public Hero myHero;
    public List<TitanTrigger> myTitanTriggers;
    public float SmoothingDelay = 10f;

    private int BaseMask, GroundMask;

    private void Awake()
    {
        this.BaseMask = 1 << LayerMask.NameToLayer("PlayerAttackBox") | 1 << LayerMask.NameToLayer("EnemyBox");
        this.GroundMask = 1 << LayerMask.NameToLayer("Ground");

        if (base.photonView != null)
        {
            base.photonView.observed = this;
            this.correctPos = base.transform.position;
            this.correctVelocity = Vector3.zero;
            base.GetComponent<SphereCollider>().enabled = false;
            if (base.photonView.isMine)
            {
                base.StartCoroutine(this.WaitAndDestroy(10f));
                this.myTitanTriggers = new List<TitanTrigger>();
            }
        }
    }

    public void destroyMe()
    {
        if (!this.disabled)
        {
            this.disabled = true;
            foreach (EnemyCheckCollider collider in PhotonNetwork.Instantiate("FX/boom4", base.transform.position, base.transform.rotation, 0).GetComponentsInChildren<EnemyCheckCollider>())
            {
                collider.dmg = 0;
            }
            if (FengGameManagerMKII.Gamemode.Settings.PvpCannons)
            {
                foreach (Hero hero in FengGameManagerMKII.instance.getPlayers())
                {
                    if (((hero != null) && (Vector3.Distance(hero.transform.position, base.transform.position) <= 20f)) && !hero.photonView.isMine)
                    {
                        GameObject gameObject = hero.gameObject;
                        PhotonPlayer owner = gameObject.GetPhotonView().owner;
                        if (((FengGameManagerMKII.Gamemode.Settings.TeamMode != TeamMode.Disabled) && (PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam] != null)) && (owner.CustomProperties[PhotonPlayerProperty.RCteam] != null))
                        {
                            int num2 = RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]);
                            int num3 = RCextensions.returnIntFromObject(owner.CustomProperties[PhotonPlayerProperty.RCteam]);
                            if ((num2 == 0) || (num2 != num3))
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
            }
            if (this.myTitanTriggers != null)
            {
                for (int i = 0; i < this.myTitanTriggers.Count; i++)
                {
                    if (this.myTitanTriggers[i] != null)
                    {
                        this.myTitanTriggers[i].SetCollision(false);
                    }
                }
            }
            PhotonNetwork.Destroy(base.gameObject);
        }
    }

    public void FixedUpdate()
    {
        if (base.photonView.isMine && !this.disabled)
        {
            int mask = this.isCollider ? this.BaseMask : (this.BaseMask | this.GroundMask);
            Collider[] hitColliders = Physics.OverlapSphere(base.transform.position, 0.6f, mask);
            bool hitSelf = false;
            for (int i = 0; i < hitColliders.Length; i++)
            {
                GameObject currentGobj = hitColliders[i].gameObject;
                bool isGroundLayer = currentGobj.layer == 9;
                bool isEnemyBoxLayer = currentGobj.layer == 10;
                bool isPlayerAttackBoxLayer = currentGobj.layer == 16;
                if (isPlayerAttackBoxLayer)
                {
                    TitanTrigger titanTrigger = currentGobj.GetComponent<TitanTrigger>();
                    if (!((titanTrigger == null) || this.myTitanTriggers.Contains(titanTrigger)))
                    {
                        titanTrigger.SetCollision(true);
                        this.myTitanTriggers.Add(titanTrigger);
                    }
                }
                else if (isEnemyBoxLayer)
                {
                    //TitanBody foo = currentGobj.transform.root.gameObject.GetComponent<TitanBody>();
                    //foo.
                    //TITAN titan = currentGobj.transform.root.gameObject.GetComponent<TITAN>();
                    MindlessTitan titan = currentGobj.transform.root.GetComponent<MindlessTitan>();
                    if (titan != null)
                    {
                        titan.photonView.RPC("OnCannonHitRpc", titan.photonView.owner, myHero.photonView.viewID, currentGobj.name);
                        this.destroyMe();
                    }
                }
                else if (isGroundLayer && (currentGobj.transform.root.name.Contains("CannonWall") || currentGobj.transform.root.name.Contains("CannonGround")))
                    hitSelf = true;
            }
            if (!(this.isCollider || hitSelf))
            {
                this.isCollider = true;
                base.GetComponent<SphereCollider>().enabled = true;
            }
        }
    }

    public void OnCollisionEnter(Collision myCollision)
    {
        if (base.photonView.isMine)
        {
            this.destroyMe();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(base.transform.position);
            stream.SendNext(base.GetComponent<Rigidbody>().velocity);
        }
        else
        {
            this.correctPos = (Vector3) stream.ReceiveNext();
            this.correctVelocity = (Vector3) stream.ReceiveNext();
        }
    }

    public void Update()
    {
        if (!base.photonView.isMine)
        {
            base.transform.position = Vector3.Lerp(base.transform.position, this.correctPos, Time.deltaTime * this.SmoothingDelay);
            base.GetComponent<Rigidbody>().velocity = this.correctVelocity;
        }
    }

    public IEnumerator WaitAndDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        this.destroyMe();
    }
}