using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Constants;
using Assets.Scripts.Events.Args;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using System.Collections;
using UnityEngine;

/// <summary>
/// The logic for a blade that has been thrown by the <see cref="BladeThrowSkill"/>.
/// </summary>
public class ThrownBlade : Photon.MonoBehaviour
{

    private const float BladeRotationSpeed = 1000f; // In degrees

    private int viewID;
    private string ownerName;
    private int myTeam;

    private Hero owner;
    private float scoreMulti;
    private float bodyVelocity;
    private Vector3 velocity;

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
    }

    private void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
            transform.position += velocity * Time.deltaTime;
            transform.RotateAround(transform.Find("Pivot").transform.position, transform.Find("Pivot").transform.up, BladeRotationSpeed * Time.deltaTime);
        }

        bool objectHit = false;
        LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer();
        // I use smaller hitbox for these because they might destroy the blades before they hit.
        if (Physics.BoxCast(transform.position, GetComponent<BoxCollider>().size / 2, velocity, transform.rotation, velocity.magnitude * Time.deltaTime, (int) mask))
        {
            objectHit = true;
        }
        mask = Layers.PlayerHitBox.ToLayer() | Layers.EnemyHitBox.ToLayer();
        if (Physics.BoxCast(transform.position, GetComponent<BoxCollider>().size, velocity, transform.rotation, velocity.magnitude * Time.deltaTime, (int) mask))
        {
            objectHit = true;
        }
        if (objectHit)
        {
            Vector3 p = transform.position + velocity.normalized * 0.03f;
            foreach (RaycastHit hit in Physics.BoxCastAll(p, GetComponent<BoxCollider>().size, velocity, transform.rotation, velocity.magnitude * Time.deltaTime, (int) mask))
            {
                ObjectHit(hit.collider.gameObject);
            }
            Destroy();
        }
    }

    private void ObjectHit(GameObject gameObject)
    {
        switch (gameObject.tag)
        {
            case "playerHitbox":
                if (GameSettings.PvP.Mode != PvpMode.Disabled && gameObject.TryGetComponent(out HitBox hitbox))
                {
                    if (hitbox.transform.root != null && hitbox.transform.root.TryGetComponent(out Hero attackedHero))
                    {
                        Service.Player.HeroHit(new HeroHitEvent(attackedHero, owner));

                        HeroHit(attackedHero);
                    }
                }
                break;
            case "titanneck":
                if (gameObject.TryGetComponent(out HitBox hitBox) && hitBox.transform.root.TryGetComponent(out TitanBase titanBase))
                {

                    if (Vector3.Angle(-titanBase.Body.Head.forward, transform.position - titanBase.Body.Head.position) >= 70f)
                        break;

                    int damage = Mathf.Max(10, (int) ((bodyVelocity * 10f) * scoreMulti));

                    Service.Player.TitanDamaged(new TitanDamagedEvent(titanBase, owner, damage));
                    Service.Player.TitanHit(new TitanHitEvent(titanBase, BodyPart.Nape, owner, false));

                    titanBase.photonView.RPC(nameof(TitanBase.OnNapeHitRpc), titanBase.photonView.owner, transform.root.gameObject.GetPhotonView().viewID, damage);
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
                        }
                        else if (titan is MindlessTitan)
                        {
                            var mindlessTitan = titan as MindlessTitan;

                            int damage = Mathf.Max(10, (int) ((bodyVelocity * 10f) * scoreMulti));

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
                        int damage = Mathf.Max(10, (int) ((bodyVelocity * 10f) * scoreMulti));
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
                    }
                }
                break;
            case "titanankle":
                {
                    GameObject rootObj = gameObject.transform.root.gameObject;

                    int damage = Mathf.Max(10, (int) ((bodyVelocity * 10f) * scoreMulti));

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

    private void HeroHit(Hero hero)
    {
        if (((hero != null) && !hero.HasDied()))
        {
            hero.GetComponent<Hero>().MarkDie();
            Debug.Log("blade hit player " + ownerName);
            object[] parameters = new object[] { (velocity.normalized * 1000f) + (Vector3.up * 50f), false, viewID, ownerName, true };
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

    public void Initialize(Hero owner, float scoreMulti, float bodyVelocity, Vector3 vel)
    {
        this.owner = owner;
        this.scoreMulti = scoreMulti;
        this.bodyVelocity = bodyVelocity;
        velocity = vel;
    }

}

