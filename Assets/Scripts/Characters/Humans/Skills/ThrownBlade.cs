using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Constants;
using Assets.Scripts.Events.Args;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Skills
{

    /// <summary>
    /// The logic for a blade that has been thrown by the <see cref="BladeThrowSkill"/>.
    /// </summary>
    public class ThrownBlade : Photon.MonoBehaviour
    {

        private const float BladeRotationSpeed = 1000f; // In degrees
        private const float BladeLifeTime = 10f; // In seconds
        private const int MinDamage = 200;

        private int viewID;
        private string ownerName;
        private int myTeam;

        private Hero owner;
        private float lifeTime;
        private Vector3 baseVelocity;

        [PunRPC]
        public void InitRPC(int viewID, Vector3 pos, int myTeam)
        {
            this.viewID = viewID;
            base.transform.position = pos;
            this.myTeam = myTeam;
        }

        private void Start()
        {
            if (!base.transform.root.gameObject.GetPhotonView().isMine)
            {
                base.enabled = false;
                return;
            }
            if (base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>() != null)
            {
                this.viewID = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID;
                this.ownerName = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().titanName;
            }
            transform.Find("weapontrail").GetComponent<MeleeWeaponTrail>().enabled = true;
            lifeTime = BladeLifeTime;
        }

        private void Update()
        {
            if (PhotonNetwork.isMasterClient)
            {
                transform.position += GetVelocity() * Time.deltaTime;
                transform.RotateAround(transform.Find("Pivot").transform.position, transform.Find("Pivot").transform.up, BladeRotationSpeed * Time.deltaTime);
            }

            bool objectHit = false;
            LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer();
            // I use smaller hitbox for these because they might destroy the blades before they hit.
            if (Physics.BoxCast(transform.position, GetComponent<BoxCollider>().size * 0.4f, GetVelocity(), transform.rotation, GetVelocity().magnitude * Time.deltaTime, (int) mask))
            {
                objectHit = true;
            }
            mask = Layers.PlayerHitBox.ToLayer() | Layers.EnemyHitBox.ToLayer();
            if (Physics.BoxCast(transform.position, GetComponent<BoxCollider>().size, GetVelocity(), transform.rotation, GetVelocity().magnitude * Time.deltaTime, (int) mask))
            {
                objectHit = true;
            }
            mask = Layers.PlayerHitBox.ToLayer() | Layers.EnemyHitBox.ToLayer() | Layers.EnemyBox.ToLayer();
            if (objectHit)
            {
                Vector3 p = transform.position + GetVelocity().normalized * 0.03f;
                foreach (RaycastHit hit in Physics.BoxCastAll(p, GetComponent<BoxCollider>().size, GetVelocity(), transform.rotation, GetVelocity().magnitude * Time.deltaTime, (int) mask))
                {
                    ObjectHit(hit.collider.gameObject);
                }
                Destroy();
            }

            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                Destroy();
            }
        }

        private void ObjectHit(GameObject gameObject)
        {
            switch (gameObject.tag)
            {
                case "playerHitbox":
                    if (GameSettings.PvP.Mode != PvpMode.Disabled && gameObject.TryGetComponent(out HitBox hitbox) && hitbox.transform.root != null && hitbox.transform.root.TryGetComponent(out Hero attackedHero))
                    {
                        Service.Player.HeroHit(new HeroHitEvent(attackedHero, owner));

                        HeroHit(attackedHero);
                    }
                    break;
                case "titanneck":
                    if (gameObject.TryGetComponent(out HitBox hitBox) && hitBox.transform.root.TryGetComponent(out TitanBase titanBase))
                    {

                        if (Vector3.Angle(-titanBase.Body.Head.forward, transform.position - titanBase.Body.Head.position) >= 70f)
                            break;

                        int damage = Mathf.Max(10, GetDamage());

                        Service.Player.TitanDamaged(new TitanDamagedEvent(titanBase, owner, damage));
                        Service.Player.TitanHit(new TitanHitEvent(titanBase, BodyPart.Nape, owner, false));

                        if (damage > MinDamage)
                        {
                            titanBase.photonView.RPC(nameof(TitanBase.OnNapeHitRpc), titanBase.photonView.owner, transform.root.gameObject.GetPhotonView().viewID, damage);
                        }
                        ShowCriticalHitFX();
                    }
                    break;
                case "titaneye":
                    {
                        GameObject rootObject = gameObject.transform.root.gameObject;

                        if (rootObject.TryGetComponent(out TitanBase titan))
                        {
                            Service.Player.TitanHit(new TitanHitEvent(titan, BodyPart.Eyes, owner, false));

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
                                ShowCriticalHitFX();
                            }
                            else if (titan is MindlessTitan)
                            {
                                var mindlessTitan = titan as MindlessTitan;

                                int damage = Mathf.Max(10, GetDamage());

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
                        GameObject rootObject = gameObject.transform.root.gameObject;

                        if (rootObject.TryGetComponent(out MindlessTitan mindlessTitan))
                        {
                            int damage = Mathf.Max(10, GetDamage());
                            BodyPart body = mindlessTitan.Body.GetBodyPart(gameObject.transform);

                            Service.Player.TitanHit(new TitanHitEvent(mindlessTitan, body, owner, false));
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
                        GameObject rootObj = gameObject.transform.root.gameObject;

                        int damage = Mathf.Max(10, GetDamage());

                        if (rootObj.TryGetComponent(out TitanBase titan))
                        {
                            Service.Player.TitanHit(new TitanHitEvent(titan, BodyPart.Ankle, owner, false));

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

                                if (gameObject.name == "ankleR")
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

        private Vector3 GetVelocity()
        {
            return baseVelocity * lifeTime / BladeLifeTime;
        }

        private int GetDamage()
        {
            return (int) ((baseVelocity.magnitude - BladeThrowSkill.GetBaseBladeSpeed) * lifeTime / BladeLifeTime * 5f);
        }

        private void HeroHit(Hero hero)
        {
            if (((hero != null) && !hero.HasDied()))
            {
                hero.GetComponent<Hero>().MarkDie();
                Debug.Log("blade hit player " + ownerName);
                object[] parameters = new object[] { (GetVelocity().normalized * 1000f) + (Vector3.up * 50f), false, viewID, ownerName, true };
                hero.GetComponent<Hero>().photonView.RPC(nameof(Hero.NetDie), PhotonTargets.All, parameters);
            }
        }

        private void ShowCriticalHitFX()
        {
            GameObject obj2 = PhotonNetwork.Instantiate("redCross", transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().StartShake(0.2f, 0.3f, 0.95f);
            obj2.transform.position = transform.position;
        }

        private void Destroy()
        {
            Destroy(this.transform.root.gameObject);
        }

        public void Initialize(Hero owner, Vector3 vel)
        {
            this.owner = owner;
            baseVelocity = vel;
        }

    }
}

