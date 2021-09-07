using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Titan;
using UnityEngine;

/// <summary>
/// Used by Titan attacks to determine if it killed a <see cref="Human"/> or not
/// </summary>
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
                            num3 = Mathf.Max(5f, num2 - vector.magnitude);
                        }

                        object[] parameters = { (vector.normalized * num3) + (Vector3.up * 1f) };
                        component.transform.root.GetComponent<Hero>().photonView.RPC(nameof(Hero.BlowAway), PhotonTargets.All, parameters);
                    }
                    else if (!component.transform.root.GetComponent<Hero>().IsInvincible)
                    {
                        if (PhotonNetwork.offlineMode)
                        {
                            if (!component.transform.root.GetComponent<Hero>().IsGrabbed)
                            {
                                Vector3 vector4 = component.transform.root.transform.position - base.transform.position;
                                component.transform.root.GetComponent<Hero>().Die(((vector4.normalized * b) * 1000f) + (Vector3.up * 50f), this.isThisBite);
                            }
                        }
                        else if ((!component.transform.root.GetComponent<Hero>().HasDied()) && !component.transform.root.GetComponent<Hero>().IsGrabbed)
                        {
                            component.transform.root.GetComponent<Hero>().MarkDie();
                            int myOwnerViewID = -1;
                            string titanName = string.Empty;
                            if (base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>() != null)
                            {
                                myOwnerViewID = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID;
                                titanName = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().titanName;
                            }
                            Vector3 vector5 = component.transform.root.position - base.transform.position;
                            object[] netDieParameters = new object[5];
                            netDieParameters[0] = ((vector5.normalized * b) * 1000f) + (Vector3.up * 50f);
                            netDieParameters[1] = this.isThisBite;
                            netDieParameters[2] = myOwnerViewID;
                            netDieParameters[3] = titanName;
                            netDieParameters[4] = true;
                            component.transform.root.GetComponent<Hero>().photonView.RPC(nameof(Hero.NetDie), PhotonTargets.All, netDieParameters);
                        }
                    }
                }
            }
            else if (((other.gameObject.tag == "erenHitbox") && (this.dmg > 0)) && !other.gameObject.transform.root.gameObject.GetComponent<ErenTitan>().isHit)
            {
                other.gameObject.transform.root.gameObject.GetComponent<ErenTitan>().hitByTitan();
            }
        }
    }

    private void Start()
    {
        this.active_me = true;
        this.count = 0;
    }
}

