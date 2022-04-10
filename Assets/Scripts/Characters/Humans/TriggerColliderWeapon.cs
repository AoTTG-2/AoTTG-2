using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Events.Args;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans
{
    /// <summary>
    /// Contains logic regarding the hit detection for the Blades weapon
    /// </summary>
    public class TriggerColliderWeapon : MonoBehaviour
    {
        public Equipment.Equipment Equipment { get; set; }

        public bool IsActive;
        public bool RightHand;
        public IN_GAME_MAIN_CAMERA currentCamera;

        //Move this to Hero.cs after rewrite is made so events don't trigger per weapon.
        public ArrayList currentHits = new ArrayList();
        public ArrayList currentHitsII = new ArrayList();

        public AudioSource meatDie;
        public Hero hero;
        public int myTeam = 1;
        public float scoreMulti = 1f;
        public Rigidbody body;



        private void Start()
        {
            currentCamera = GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>();
            body = currentCamera.main_object.GetComponent<Rigidbody>();
            Equipment = transform.root.GetComponent<Equipment.Equipment>();
            hero = currentCamera.main_object.GetComponent<Hero>();
        }

        public void ClearHits()
        {
            currentHitsII.Clear();
            currentHits.Clear();
        }

        private void HeroHit(Hero hero, HitBox hitbox, float distance)
        {
            Service.Player.HeroHit(new HeroHitEvent(hero, hero));
            if (hero.myTeam != myTeam && !hero.IsInvincible && hero.HasDied() && !hero.IsGrabbed)
            {
                // I honestly don't have a clue as to what this does
                float b = Mathf.Min(1f, 1f - (distance * 0.05f));

                Service.Player.HeroKill(new HeroKillEvent(hero, hero));
                hero.MarkDie();
                hero.photonView.RPC(nameof(Hero.NetDie), PhotonTargets.All, new object[]
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
            if (!IsActive) return;

            if (!currentHitsII.Contains(collider.gameObject))
            {
                currentHitsII.Add(collider.gameObject);
                currentCamera.StartShake(0.1f, 0.1f, 0.95f);
                if (collider.gameObject.transform.root.gameObject.CompareTag("titan"))
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
                    if (GameSettings.PvP.Mode != PvpMode.Disabled && collider.gameObject.TryGetComponent(out HitBox hitbox))
                    {
                        if (hitbox.transform.root != null && hitbox.transform.root.TryGetComponent(out Hero attackedHero))
                        {
                            Service.Player.HeroHit(new HeroHitEvent(attackedHero, hero));

                            HeroHit(attackedHero, hitbox, Vector3.Distance(collider.gameObject.transform.position, transform.position));
                        }
                    }
                    break;
                case "titanneck":
                    if (collider.gameObject.TryGetComponent(out HitBox hitBox) && hitBox.transform.root.TryGetComponent(out TitanBase titanBase))
                    {

                        if (Vector3.Angle(-titanBase.Body.Head.forward, transform.position - titanBase.Body.Head.position) >= 70f)
                            break;

                        Vector3 velocity = body.velocity - hitBox.transform.root.GetComponent<Rigidbody>().velocity;
                        int damage = Mathf.Max(10, (int) ((velocity.magnitude * 10f) * scoreMulti));

                        Service.Player.TitanDamaged(new TitanDamagedEvent(titanBase, hero, damage));
                        Service.Player.TitanHit(new TitanHitEvent(titanBase, BodyPart.Nape, hero, RightHand));

                        titanBase.photonView.RPC(nameof(TitanBase.OnNapeHitRpc), titanBase.photonView.owner, transform.root.gameObject.GetPhotonView().viewID, damage);
                    }
                    break;
                case "titaneye":
                    {
                        currentHits.Add(collider.gameObject);
                        GameObject rootObject = collider.gameObject.transform.root.gameObject;

                        if (rootObject.TryGetComponent(out TitanBase titan))
                        {
                            Service.Player.TitanHit(new TitanHitEvent(titan, BodyPart.Eyes, hero, RightHand));

                            if (titan is FemaleTitan)
                            {
                                var femaleTitan = titan as FemaleTitan;

                                if (femaleTitan.hasDie) return;

                                if (!PhotonNetwork.isMasterClient)
                                {
                                    object[] infoArray = new object[] { transform.root.gameObject.GetPhotonView().viewID };
                                    femaleTitan.photonView.RPC(nameof(FemaleTitan.hitEyeRPC), PhotonTargets.MasterClient, infoArray);
                                }
                                else
                                {
                                    femaleTitan.hitEyeRPC(transform.root.gameObject.GetPhotonView().viewID);
                                }
                            }
                            else if (titan is MindlessTitan)
                            {
                                var mindlessTitan = titan as MindlessTitan;

                                Vector3 velocity = body.velocity - rootObject.GetComponent<Rigidbody>().velocity;
                                int damage = Mathf.Max(10, (int) ((velocity.magnitude * 10f) * scoreMulti));

                                if (PhotonNetwork.isMasterClient)
                                {
                                    mindlessTitan.OnEyeHitRpc(transform.root.gameObject.GetPhotonView().viewID, damage);
                                }
                                else
                                {
                                    mindlessTitan.photonView.RPC(nameof(MindlessTitan.OnEyeHitRpc), mindlessTitan.photonView.owner, transform.root.gameObject.GetPhotonView().viewID, damage);
                                }
                                ShowCriticalHitFX();
                            }
                        }
                    }
                    break;
                case "titanbodypart":
                    {
                        currentHits.Add(collider.gameObject);
                        GameObject rootObject = collider.gameObject.transform.root.gameObject;

                        if (rootObject.TryGetComponent(out MindlessTitan mindlessTitan))
                        {
                            Vector3 velocity = this.body.velocity - rootObject.GetComponent<Rigidbody>().velocity;
                            int damage = Mathf.Max(10, (int) ((velocity.magnitude * 10f) * scoreMulti));
                            BodyPart body = mindlessTitan.Body.GetBodyPart(collider.transform);

                            Service.Player.TitanHit(new TitanHitEvent(mindlessTitan, body, hero, RightHand));
                            if (PhotonNetwork.isMasterClient)
                            {
                                mindlessTitan.OnBodyPartHitRpc(body, damage);
                            }
                            else
                            {
                                mindlessTitan.photonView.RPC(nameof(MindlessTitan.OnBodyPartHitRpc), mindlessTitan.photonView.owner, body, damage);
                            }
                            ShowCriticalHitFX();
                        }
                    }
                    break;
                case "titanankle":
                    {
                        currentHits.Add(collider.gameObject);
                        GameObject rootObj = collider.gameObject.transform.root.gameObject;
                        Vector3 velocity = Vector3.zero;

                        if (rootObj.TryGetComponent(out Rigidbody rigidbody))//patch for dummy titan
                        {
                            velocity = body.velocity - rigidbody.velocity;
                        }

                        int damage = Mathf.Max(10, (int) ((velocity.magnitude * 10f) * scoreMulti));

                        if (rootObj.TryGetComponent(out TitanBase titan))
                        {
                            Service.Player.TitanHit(new TitanHitEvent(titan, BodyPart.Ankle, hero, RightHand));

                            if (titan is MindlessTitan)
                            {
                                var mindlessTitan = titan as MindlessTitan;

                                mindlessTitan.OnAnkleHit(transform.root.gameObject.GetPhotonView().viewID, damage);
                                ShowCriticalHitFX();
                            }
                            else if (titan is FemaleTitan)
                            {
                                var femaleTitan = titan as FemaleTitan;

                                if (femaleTitan.hasDie) return;

                                if (collider.gameObject.name == "ankleR")
                                {
                                    if (!PhotonNetwork.isMasterClient)
                                    {
                                        object[] infoArray = new object[] { transform.root.gameObject.GetPhotonView().viewID, damage };
                                        femaleTitan.photonView.RPC(nameof(FemaleTitan.hitAnkleRRPC), PhotonTargets.MasterClient, infoArray);
                                    }
                                    else
                                    {
                                        femaleTitan.hitAnkleRRPC(transform.root.gameObject.GetPhotonView().viewID, damage);
                                    }
                                }
                                else if (!PhotonNetwork.isMasterClient)
                                {
                                    object[] infoArray = new object[] { transform.root.gameObject.GetPhotonView().viewID, damage };
                                    femaleTitan.photonView.RPC(nameof(FemaleTitan.hitAnkleLRPC), PhotonTargets.MasterClient, infoArray);
                                }
                                else
                                {
                                    femaleTitan.hitAnkleLRPC(transform.root.gameObject.GetPhotonView().viewID, damage);
                                }
                                ShowCriticalHitFX();
                            }
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

        private void ShowCriticalHitFX()
        {
            GameObject obj2 = PhotonNetwork.Instantiate("redCross", transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
            currentCamera.StartShake(0.2f, 0.3f, 0.95f);
            obj2.transform.position = transform.position;
        }
    }
}