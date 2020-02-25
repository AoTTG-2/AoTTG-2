using Photon;
using System;
using UnityEngine;

public class EnemyCheckCollider : Photon.MonoBehaviour
{
    public bool active_me;
    private int count;
    public int dmg = 1;
    public bool isThisBite;

    private void FixedUpdate()
    {
        if (this.count > 1)
        {
            this.active_me = false;
        }
        else
        {
            this.count++;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER) || base.transform.root.gameObject.GetPhotonView().isMine) && this.active_me)
        {
            if (other.gameObject.tag == "playerHitbox")
            {
                float b = 1f - (Vector3.Distance(other.gameObject.transform.position, base.transform.position) * 0.05f);
                b = Mathf.Min(1f, b);
                HitBox component = other.gameObject.GetComponent<HitBox>();
                if ((component != null) && (component.transform.root != null))
                {
                    if (this.dmg == 0)
                    {
                        Vector3 vector = component.transform.root.transform.position - base.transform.position;
                        float num2 = 0f;
                        if (base.gameObject.GetComponent<SphereCollider>() != null)
                        {
                            num2 = base.transform.localScale.x * base.gameObject.GetComponent<SphereCollider>().radius;
                        }
                        if (base.gameObject.GetComponent<CapsuleCollider>() != null)
                        {
                            num2 = base.transform.localScale.x * base.gameObject.GetComponent<CapsuleCollider>().height;
                        }
                        float num3 = 5f;
                        if (num2 > 0f)
                        {
                            num3 = Mathf.Max((float) 5f, (float) (num2 - vector.magnitude));
                        }
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                        {
                            component.transform.root.GetComponent<HERO>().blowAway((Vector3) ((vector.normalized * num3) + (Vector3.up * 1f)));
                        }
                        else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                        {
                            object[] parameters = new object[] { (Vector3) ((vector.normalized * num3) + (Vector3.up * 1f)) };
                            component.transform.root.GetComponent<HERO>().photonView.RPC("blowAway", PhotonTargets.All, parameters);
                        }
                    }
                    else if (!component.transform.root.GetComponent<HERO>().isInvincible())
                    {
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                        {
                            if (!component.transform.root.GetComponent<HERO>().isGrabbed)
                            {
                                Vector3 vector4 = component.transform.root.transform.position - base.transform.position;
                                component.transform.root.GetComponent<HERO>().die((Vector3) (((vector4.normalized * b) * 1000f) + (Vector3.up * 50f)), this.isThisBite);
                            }
                        }
                        else if (((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && !component.transform.root.GetComponent<HERO>().HasDied()) && !component.transform.root.GetComponent<HERO>().isGrabbed)
                        {
                            component.transform.root.GetComponent<HERO>().markDie();
                            int myOwnerViewID = -1;
                            string titanName = string.Empty;
                            if (base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>() != null)
                            {
                                myOwnerViewID = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID;
                                titanName = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().titanName;
                            }
                            object[] objArray2 = new object[5];
                            Vector3 vector5 = component.transform.root.position - base.transform.position;
                            objArray2[0] = (Vector3) (((vector5.normalized * b) * 1000f) + (Vector3.up * 50f));
                            objArray2[1] = this.isThisBite;
                            objArray2[2] = myOwnerViewID;
                            objArray2[3] = titanName;
                            objArray2[4] = true;
                            component.transform.root.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, objArray2);
                        }
                    }
                }
            }
            else if (((other.gameObject.tag == "erenHitbox") && (this.dmg > 0)) && !other.gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().isHit)
            {
                other.gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByTitan();
            }
        }
    }

    private void Start()
    {
        this.active_me = true;
        this.count = 0;
    }
}

