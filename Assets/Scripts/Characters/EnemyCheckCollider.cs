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
        if ((base.transform.root.gameObject.GetPhotonView().isMine) && this.active_me)
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

                        object[] parameters = new object[] { (Vector3) ((vector.normalized * num3) + (Vector3.up * 1f)) };
                        component.transform.root.GetComponent<Hero>().photonView.RPC(nameof(Hero.blowAway), PhotonTargets.All, parameters);
                    }
                    else if (!component.transform.root.GetComponent<Hero>().isInvincible())
                    {
                        if (PhotonNetwork.offlineMode)
                        {
                            if (!component.transform.root.GetComponent<Hero>().isGrabbed)
                            {
                                Vector3 vector4 = component.transform.root.transform.position - base.transform.position;
                                component.transform.root.GetComponent<Hero>().die((Vector3) (((vector4.normalized * b) * 1000f) + (Vector3.up * 50f)), this.isThisBite);
                            }
                        }
                        else if ((!component.transform.root.GetComponent<Hero>().HasDied()) && !component.transform.root.GetComponent<Hero>().isGrabbed)
                        {
                            component.transform.root.GetComponent<Hero>().markDie();
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
                            component.transform.root.GetComponent<Hero>().photonView.RPC("netDie", PhotonTargets.All, objArray2);
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

