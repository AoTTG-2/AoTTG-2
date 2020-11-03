using Assets.Scripts;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings;
using System.Collections;
using UnityEngine;

public class TriggerColliderWeapon : MonoBehaviour
{
    public Equipment Equipment { get; set; }

    public bool active_me;
    public IN_GAME_MAIN_CAMERA currentCamera;
    public ArrayList currentHits = new ArrayList();
    public ArrayList currentHitsII = new ArrayList();
    public AudioSource meatDie;
    public Hero hero;
    private FengGameManagerMKII manager;
    public int myTeam = 1;
    public float scoreMulti = 1f;
    public Rigidbody body;

    private bool checkIfBehind(GameObject titan)
    {
        if (titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head") != null)
        {
            Transform transform = titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
            Vector3 to = base.transform.position - transform.transform.position;
            return (Vector3.Angle(-transform.transform.forward, to) < 70f);
        }
        else if (titan.transform.Find("BodyPivot/HeadPos") != null)// dummy titan
        {
            Transform transform = titan.transform.Find("BodyPivot/HeadPos");
            Vector3 to = base.transform.position - transform.transform.position;
            return (Vector3.Angle(-transform.transform.forward, to) < 70f);
        }
        return false;
    }

    public void DummyNapeHit(DummyTitan titan)
    {
        Vector3 velocity = body.velocity;
        int damage = (int) ((velocity.magnitude * 10f) * this.scoreMulti);
        damage = Mathf.Max(10, damage);

        titan.photonView.RPC("GetHit", PhotonTargets.All, new object[] { damage });
        manager.netShowDamage(damage);
    }

    public void ClearHits()
    {
        currentHitsII = new ArrayList();
        currentHits = new ArrayList();
    }

    private void HeroHit(Hero hero, HitBox hitbox, float distance)
    {
        if ((hero.myTeam != myTeam) && !hero.isInvincible() && hero.HasDied() && !hero.isGrabbed)
        {
            float b = Mathf.Min(1f, 1f - (distance * 0.05f));

            hero.markDie();
            hero.photonView.RPC("netDie", PhotonTargets.All, new object[]
            {
                ((hitbox.transform.root.position - transform.position.normalized * b) * 1000f) + (Vector3.up * 50f),
                false,
                transform.root.gameObject.GetPhotonView().viewID,
                PhotonView.Find(transform.root.gameObject.GetPhotonView().viewID).owner.CustomProperties[PhotonPlayerProperty.name],
                false
            });
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (active_me)
        {
            if (!currentHitsII.Contains(collider.gameObject))
            {
                currentHitsII.Add(collider.gameObject);
                currentCamera.startShake(0.1f, 0.1f, 0.95f);
                if (collider.gameObject.transform.root.gameObject.tag == "titan")
                {
                    GameObject meat;
                    hero.slashHit.Play();
                    meat = PhotonNetwork.Instantiate("hitMeat", transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
                    meat.transform.position = transform.position;
                    Equipment.Weapon.Use(0);
                }
            }

            if (currentHits.Contains(collider.gameObject))
                return;

            switch (collider.gameObject.tag)
            {
                case "playerHitbox":
                    if (GameSettings.PvP.Mode != PvpMode.Disabled && collider.gameObject.TryGetComponent(out HitBox hitbox) && (hitbox.transform.root != null) && hitbox.transform.root.TryGetComponent(out Hero hero))
                    {
                        HeroHit(hero, hitbox, Vector3.Distance(collider.gameObject.transform.position, transform.position));
                    }
                    break;
                case "titanneck":
                    if ((collider.gameObject.TryGetComponent(out HitBox item) && checkIfBehind(item.transform.root.gameObject)) && item.transform.root.TryGetComponent(out TitanBase tb))
                    {
                        Vector3 velocity = body.velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                        var damage = Mathf.Max(10, (int) ((velocity.magnitude * 10f) * scoreMulti));

                        tb.photonView.RPC(nameof(tb.OnNapeHitRpc2), tb.photonView.owner, transform.root.gameObject.GetPhotonView().viewID, damage);
                    }
                    break;
                case "titaneye":
                    {
                        currentHits.Add(collider.gameObject);
                        GameObject gameObject = collider.gameObject.transform.root.gameObject;
                        if (gameObject.TryGetComponent(out FemaleTitan ft) && !ft.hasDie)
                        {
                            if (!PhotonNetwork.isMasterClient)
                            {
                                object[] objArray5 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID };
                                ft.photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, objArray5);
                            }
                            else
                            {
                                ft.hitEyeRPC(base.transform.root.gameObject.GetPhotonView().viewID);
                            }
                        }
                        else if (gameObject.TryGetComponent(out MindlessTitan mt))
                        {
                            Vector3 vector4 = body.velocity - gameObject.GetComponent<Rigidbody>().velocity;
                            var damage = Mathf.Max(10, (int) ((vector4.magnitude * 10f) * scoreMulti));
                            if (PhotonNetwork.isMasterClient)
                            {
                                mt.OnEyeHitRpc(transform.root.gameObject.GetPhotonView().viewID, damage);
                            }
                            else
                            {
                                mt.photonView.RPC("OnEyeHitRpc", mt.photonView.owner, transform.root.gameObject.GetPhotonView().viewID, damage);
                            }
                            ShowCriticalHitFX();
                        }
                    }
                    break;
                case "titanbodypart":
                    {
                        currentHits.Add(collider.gameObject);
                        GameObject gameObject = collider.gameObject.transform.root.gameObject;
                        if (gameObject.TryGetComponent(out MindlessTitan mt))
                        {
                            Vector3 vector4 = this.body.velocity - gameObject.GetComponent<Rigidbody>().velocity;
                            var damage = Mathf.Max(10, (int) ((vector4.magnitude * 10f) * scoreMulti));
                            var body = mt.Body.GetBodyPart(collider.transform);
                            if (PhotonNetwork.isMasterClient)
                            {
                                mt.OnBodyPartHitRpc(body, damage);
                            }
                            else
                            {
                                mt.photonView.RPC("OnBodyPartHitRpc", mt.photonView.owner, body, damage);
                            }
                        }
                    }
                    break;
                case "titanankle":
                    {
                        currentHits.Add(collider.gameObject);
                        GameObject rootObj = collider.gameObject.transform.root.gameObject;
                        Vector3 vel = Vector3.zero;
                        if (rootObj.TryGetComponent(out Rigidbody rigidbody))//patch for dummy titan
                        {
                            vel = body.velocity - rigidbody.velocity;
                        }

                        int damage = Mathf.Max(10, (int) ((vel.magnitude * 10f) * this.scoreMulti));

                        if (rootObj.TryGetComponent(out MindlessTitan mt))
                        {
                            mt.OnAnkleHit(transform.root.gameObject.GetPhotonView().viewID, damage);
                            ShowCriticalHitFX();
                        }
                        else if (rootObj.TryGetComponent(out FemaleTitan ft) && !ft.hasDie)
                        {
                            if (collider.gameObject.name == "ankleR")
                            {
                                if (!PhotonNetwork.isMasterClient)
                                {
                                    object[] objArray8 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID, damage };
                                    ft.photonView.RPC("hitAnkleRRPC", PhotonTargets.MasterClient, objArray8);
                                }
                                else
                                {
                                    ft.hitAnkleRRPC(base.transform.root.gameObject.GetPhotonView().viewID, damage);
                                }
                            }
                            else if (!PhotonNetwork.isMasterClient)
                            {
                                object[] objArray9 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID, damage };
                                ft.photonView.RPC("hitAnkleLRPC", PhotonTargets.MasterClient, objArray9);
                            }
                            else
                            {
                                ft.hitAnkleLRPC(base.transform.root.gameObject.GetPhotonView().viewID, damage);
                            }
                            ShowCriticalHitFX();
                        }
                        else if (rootObj.TryGetComponent(out DummyTitan dummyTitan))
                        {
                            ShowCriticalHitFX();
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void ShowCriticalHitFX()
    {
        GameObject obj2;
        currentCamera.startShake(0.2f, 0.3f, 0.95f);
        obj2 = PhotonNetwork.Instantiate("redCross", transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
        obj2.transform.position = transform.position;
    }

    private void Start()
    {
        currentCamera = GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>();
        body = currentCamera.main_object.GetComponent<Rigidbody>();
        Equipment = base.transform.root.GetComponent<Equipment>();
        hero = currentCamera.main_object.GetComponent<Hero>();
        manager = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>();
    }
}