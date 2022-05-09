using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings;
using System.Collections;
using UnityEngine;

/// <summary>
/// AHSS Collider logic
/// </summary>
public class AHSSShotGunCollider : MonoBehaviour
{
    public bool active_me;
    private int count;
    public GameObject currentCamera;
    public ArrayList currentHits = new ArrayList();
    public int dmg = 1;
    private int myTeam = 1;
    private string ownerName = string.Empty;
    public float scoreMulti;
    private int viewID = -1;

    private bool checkIfBehind(GameObject titan)
    {
        Transform transform = titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
        Vector3 to = base.transform.position - transform.transform.position;
        Debug.DrawRay(transform.transform.position, (Vector3) (-transform.transform.forward * 10f), Color.white, 5f);
        Debug.DrawRay(transform.transform.position, (Vector3) (to * 10f), Color.green, 5f);
        return (Vector3.Angle(-transform.transform.forward, to) < 100f);
    }

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
                if (GameSettings.PvP.Mode != PvpMode.Disabled)
                {
                    float b = 1f - (Vector3.Distance(other.gameObject.transform.position, base.transform.position) * 0.05f);
                    b = Mathf.Min(1f, b);
                    HitBox component = other.gameObject.GetComponent<HitBox>();
                    if ((((component != null) && (component.transform.root != null)) && (component.transform.root.GetComponent<Hero>().myTeam != this.myTeam)) && !component.transform.root.GetComponent<Hero>().IsInvincible)
                    {
                        if ((!component.transform.root.GetComponent<Hero>().HasDied()) && !component.transform.root.GetComponent<Hero>().IsGrabbed)
                        {
                            component.transform.root.GetComponent<Hero>().MarkDie();
                            object[] parameters = new object[5];
                            Vector3 vector2 = component.transform.root.position - base.transform.position;
                            parameters[0] = (Vector3) (((vector2.normalized * b) * 1000f) + (Vector3.up * 50f));
                            parameters[1] = false;
                            parameters[2] = this.viewID;
                            parameters[3] = this.ownerName;
                            parameters[4] = false;
                            component.transform.root.GetComponent<Hero>().photonView.RPC(nameof(Hero.NetDie), PhotonTargets.All, parameters);
                        }
                    }
                }
            }
            else if (other.gameObject.tag == "erenHitbox")
            {
                if ((this.dmg > 0) && !other.gameObject.transform.root.gameObject.GetComponent<ErenTitan>().isHit)
                {
                    other.gameObject.transform.root.gameObject.GetComponent<ErenTitan>().hitByTitan();
                }
            }
            else if (other.gameObject.tag == "titanneck")
            {
                HitBox item = other.gameObject.GetComponent<HitBox>();
                if (((item != null) && this.checkIfBehind(item.transform.root.gameObject)) && !this.currentHits.Contains(item))
                {
                    item.hitPosition = (Vector3) ((base.transform.position + item.transform.position) * 0.5f);
                    this.currentHits.Add(item);

                    if (item.transform.root.GetComponent<MindlessTitan>() != null)
                    {
                        Vector3 vector4 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                        var damage = (int)((vector4.magnitude * 10f) * this.scoreMulti);
                        damage = Mathf.Max(10, damage);
                        var mindlessTitan = item.transform.root.GetComponent<MindlessTitan>();
                        if (PhotonNetwork.isMasterClient)
                        {
                            mindlessTitan.OnNapeHitRpc(transform.root.gameObject.GetPhotonView().viewID, damage);
                        }
                        else
                        {
                            mindlessTitan.photonView.RPC(nameof(MindlessTitan.OnNapeHitRpc), mindlessTitan.photonView.owner, transform.root.gameObject.GetPhotonView().viewID, damage);
                        }
                    }
                    else if (!PhotonNetwork.isMasterClient)
                    {
                        if (item.transform.root.GetComponent<FemaleTitan>() != null)
                        {
                            Vector3 vector5 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                            int num4 = (int) ((vector5.magnitude * 10f) * this.scoreMulti);
                            num4 = Mathf.Max(10, num4);
                            if (!item.transform.root.GetComponent<FemaleTitan>().hasDie)
                            {
                                object[] objArray3 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID, num4 };
                                item.transform.root.GetComponent<FemaleTitan>().photonView.RPC("titanGetHit", item.transform.root.GetComponent<FemaleTitan>().photonView.owner, objArray3);
                            }
                        }
                        else if ((item.transform.root.GetComponent<ColossalTitan>() != null) && !item.transform.root.GetComponent<ColossalTitan>().hasDie)
                        {
                            Vector3 vector6 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                            int num5 = (int) ((vector6.magnitude * 10f) * this.scoreMulti);
                            num5 = Mathf.Max(10, num5);
                            object[] objArray4 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID, num5 };
                            item.transform.root.GetComponent<ColossalTitan>().photonView.RPC("titanGetHit", item.transform.root.GetComponent<ColossalTitan>().photonView.owner, objArray4);
                        }
                    }
                    else if (item.transform.root.GetComponent<FemaleTitan>() != null)
                    {
                        if (!item.transform.root.GetComponent<FemaleTitan>().hasDie)
                        {
                            Vector3 vector8 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                            int num7 = (int) ((vector8.magnitude * 10f) * this.scoreMulti);
                            num7 = Mathf.Max(10, num7);
                            if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
                            {
                                GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().StartSnapShot2(item.transform.position, num7, null, 0.02f);
                            }
                            item.transform.root.GetComponent<FemaleTitan>().OnNapeHitRpc(base.transform.root.gameObject.GetPhotonView().viewID, num7);
                        }
                    }
                    else if ((item.transform.root.GetComponent<ColossalTitan>() != null) && !item.transform.root.GetComponent<ColossalTitan>().hasDie)
                    {
                        Vector3 vector9 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                        int num8 = (int) ((vector9.magnitude * 10f) * this.scoreMulti);
                        num8 = Mathf.Max(10, num8);
                        if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
                        {
                            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().StartSnapShot2(item.transform.position, num8, null, 0.02f);
                        }
                        item.transform.root.GetComponent<ColossalTitan>().OnNapeHitRpc(transform.root.gameObject.GetPhotonView().viewID, num8);
                    }
                    this.showCriticalHitFX(other.gameObject.transform.position);
                }
            }
            else if (other.gameObject.tag == "titaneye")
            {
                if (!this.currentHits.Contains(other.gameObject))
                {
                    this.currentHits.Add(other.gameObject);
                    GameObject gameObject = other.gameObject.transform.root.gameObject;
                    if (gameObject.GetComponent<FemaleTitan>() != null)
                    {
                        if (!PhotonNetwork.isMasterClient)
                        {
                            if (!gameObject.GetComponent<FemaleTitan>().hasDie)
                            {
                                object[] objArray5 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID };
                                gameObject.GetComponent<FemaleTitan>().photonView.RPC(nameof(FemaleTitan.hitEyeRPC), PhotonTargets.MasterClient, objArray5);
                            }
                        }
                        else if (!gameObject.GetComponent<FemaleTitan>().hasDie)
                        {
                            gameObject.GetComponent<FemaleTitan>().hitEyeRPC(base.transform.root.gameObject.GetPhotonView().viewID);
                        }
                    }
                    else if (gameObject.GetComponent<MindlessTitan>() != null)
                    {
                        Vector3 vector4 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity - gameObject.GetComponent<Rigidbody>().velocity;
                        var damage = (int)((vector4.magnitude * 10f) * this.scoreMulti);
                        damage = Mathf.Max(10, damage);
                        var mindlessTitan = gameObject.GetComponent<MindlessTitan>();
                        if (PhotonNetwork.isMasterClient)
                        {
                            mindlessTitan.OnEyeHitRpc(transform.root.gameObject.GetPhotonView().viewID, damage);
                        }
                        else
                        {
                            mindlessTitan.photonView.RPC(nameof(MindlessTitan.OnEyeHitRpc), mindlessTitan.photonView.owner, transform.root.gameObject.GetPhotonView().viewID, damage);
                        }
                        this.showCriticalHitFX(other.gameObject.transform.position);
                    }
                }
            }
            else if ((other.gameObject.tag == "titanankle") && !this.currentHits.Contains(other.gameObject))
            {
                this.currentHits.Add(other.gameObject);
                GameObject obj3 = other.gameObject.transform.root.gameObject;
                Vector3 vector10 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity - obj3.GetComponent<Rigidbody>().velocity;
                int num9 = (int) ((vector10.magnitude * 10f) * this.scoreMulti);
                num9 = Mathf.Max(10, num9);
                if (obj3.GetComponent<MindlessTitan>() != null)
                {
                    var mindlessTitan = obj3.GetComponent<MindlessTitan>();
                    mindlessTitan.OnAnkleHit(transform.root.gameObject.GetPhotonView().viewID, num9);
                    showCriticalHitFX(other.gameObject.transform.position);
                }
                else if (obj3.GetComponent<FemaleTitan>() != null)
                {
                    if (other.gameObject.name == "ankleR")
                    {
                        if (!PhotonNetwork.isMasterClient)
                        {
                            if (!obj3.GetComponent<FemaleTitan>().hasDie)
                            {
                                object[] objArray8 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID, num9 };
                                obj3.GetComponent<FemaleTitan>().photonView.RPC(nameof(FemaleTitan.hitAnkleRRPC), PhotonTargets.MasterClient, objArray8);
                            }
                        }
                        else if (!obj3.GetComponent<FemaleTitan>().hasDie)
                        {
                            obj3.GetComponent<FemaleTitan>().hitAnkleRRPC(base.transform.root.gameObject.GetPhotonView().viewID, num9);
                        }
                    }
                    else if (!PhotonNetwork.isMasterClient)
                    {
                        if (!obj3.GetComponent<FemaleTitan>().hasDie)
                        {
                            object[] objArray9 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID, num9 };
                            obj3.GetComponent<FemaleTitan>().photonView.RPC(nameof(FemaleTitan.hitAnkleLRPC), PhotonTargets.MasterClient, objArray9);
                        }
                    }
                    else if (!obj3.GetComponent<FemaleTitan>().hasDie)
                    {
                        obj3.GetComponent<FemaleTitan>().hitAnkleLRPC(base.transform.root.gameObject.GetPhotonView().viewID, num9);
                    }
                    this.showCriticalHitFX(other.gameObject.transform.position);
                }
            }
        }
    }

    private void showCriticalHitFX(Vector3 position)
    {
        this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().StartShake(0.2f, 0.3f, 0.95f);
        var redCrossEffect = PhotonNetwork.Instantiate("redCross1", base.transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
        redCrossEffect.transform.position = position;
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
            this.myTeam = PhotonView.Find(this.viewID).gameObject.GetComponent<Hero>().myTeam;
        }
        this.active_me = true;
        this.count = 0;
        this.currentCamera = GameObject.Find("MainCamera");
    }
}

