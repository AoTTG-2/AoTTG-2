using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
using System.Collections;
using UnityEngine;

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
        if (((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER) || base.transform.root.gameObject.GetPhotonView().isMine) && this.active_me)
        {
            if (other.gameObject.tag == "playerHitbox")
            {
                if (FengGameManagerMKII.Gamemode.Settings.Pvp != PvpMode.Disabled)
                {
                    float b = 1f - (Vector3.Distance(other.gameObject.transform.position, base.transform.position) * 0.05f);
                    b = Mathf.Min(1f, b);
                    HitBox component = other.gameObject.GetComponent<HitBox>();
                    if ((((component != null) && (component.transform.root != null)) && (component.transform.root.GetComponent<Hero>().myTeam != this.myTeam)) && !component.transform.root.GetComponent<Hero>().isInvincible())
                    {
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                        {
                            if (!component.transform.root.GetComponent<Hero>().isGrabbed)
                            {
                                Vector3 vector = component.transform.root.transform.position - base.transform.position;
                                component.transform.root.GetComponent<Hero>().die((Vector3) (((vector.normalized * b) * 1000f) + (Vector3.up * 50f)), false);
                            }
                        }
                        else if (((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && !component.transform.root.GetComponent<Hero>().HasDied()) && !component.transform.root.GetComponent<Hero>().isGrabbed)
                        {
                            var enemy = component.transform.root.GetComponent<Hero>();
                            enemy.markDie();
                            Vector3 delta = component.transform.root.position - transform.position;
                            var parameters = new object[]
                            {
                                delta.normalized * b * 1000f + Vector3.up * 50f,
                                false,
                                viewID,
                                ownerName,
                                false
                            };
                            enemy.photonView.RPC(nameof(enemy.netDie), PhotonTargets.All, parameters);
                        }
                    }
                }
            }
            else if (other.gameObject.tag == "erenHitbox")
            {
                if ((this.dmg > 0) && !other.gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().isHit)
                {
                    other.gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByTitan();
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
                            mindlessTitan.OnNapeHitRpc(transform.root.gameObject.GetPhotonView().viewID, damage);
                        else
                            mindlessTitan.photonView.RPC(nameof(mindlessTitan.OnNapeHitRpc), mindlessTitan.photonView.owner, transform.root.gameObject.GetPhotonView().viewID, damage);
                    }
                    else if (!PhotonNetwork.isMasterClient)
                    {
                        if (item.transform.root.GetComponent<FEMALE_TITAN>() != null)
                        {
                            Vector3 vector5 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                            int speed = (int) ((vector5.magnitude * 10f) * this.scoreMulti);
                            speed = Mathf.Max(10, speed);
                            var femaleTitan = item.transform.root.GetComponent<FEMALE_TITAN>();
                            if (!femaleTitan.hasDie)
                                femaleTitan.photonView.RPC(nameof(femaleTitan.titanGetHit), femaleTitan.photonView.owner, transform.root.gameObject.GetPhotonView().viewID, speed);
                        }
                        else if ((item.transform.root.GetComponent<COLOSSAL_TITAN>() != null) && !item.transform.root.GetComponent<COLOSSAL_TITAN>().hasDie)
                        {
                            Vector3 vector6 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                            int speed = (int) ((vector6.magnitude * 10f) * this.scoreMulti);
                            speed = Mathf.Max(10, speed);
                            var colossalTitan = item.transform.root.GetComponent<COLOSSAL_TITAN>();
                            colossalTitan.photonView.RPC(nameof(colossalTitan.titanGetHit), colossalTitan.photonView.owner, transform.root.gameObject.GetPhotonView().viewID, speed);
                        }
                    }
                    else if (item.transform.root.GetComponent<FEMALE_TITAN>() != null)
                    {
                        if (!item.transform.root.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            Vector3 vector8 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                            int num7 = (int) ((vector8.magnitude * 10f) * this.scoreMulti);
                            num7 = Mathf.Max(10, num7);
                            if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
                            {
                                GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(item.transform.position, num7, null, 0.02f);
                            }
                            item.transform.root.GetComponent<FEMALE_TITAN>().titanGetHit(base.transform.root.gameObject.GetPhotonView().viewID, num7);
                        }
                    }
                    else if ((item.transform.root.GetComponent<COLOSSAL_TITAN>() != null) && !item.transform.root.GetComponent<COLOSSAL_TITAN>().hasDie)
                    {
                        Vector3 vector9 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                        int num8 = (int) ((vector9.magnitude * 10f) * this.scoreMulti);
                        num8 = Mathf.Max(10, num8);
                        if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
                        {
                            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(item.transform.position, num8, null, 0.02f);
                        }
                        item.transform.root.GetComponent<COLOSSAL_TITAN>().titanGetHit(base.transform.root.gameObject.GetPhotonView().viewID, num8);
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
                    var femaleTitan = gameObject.GetComponent<FEMALE_TITAN>();
                    if (femaleTitan != null)
                    {
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                        {
                            if (!femaleTitan.hasDie)
                            {
                                femaleTitan.hitEye();
                            }
                        }
                        else if (!PhotonNetwork.isMasterClient)
                        {
                            if (!femaleTitan.hasDie)
                            {
                                object[] objArray5 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID };
                                femaleTitan.photonView.RPC(nameof(femaleTitan.hitEyeRPC), PhotonTargets.MasterClient, objArray5);
                            }
                        }
                        else if (!femaleTitan.hasDie)
                        {
                            femaleTitan.hitEyeRPC(transform.root.gameObject.GetPhotonView().viewID);
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
                            mindlessTitan.photonView.RPC(nameof(mindlessTitan.OnEyeHitRpc), mindlessTitan.photonView.owner, transform.root.gameObject.GetPhotonView().viewID, damage);
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
                int speed = (int) ((vector10.magnitude * 10f) * this.scoreMulti);
                speed = Mathf.Max(10, speed);

                MindlessTitan mindlessTitan;
                FEMALE_TITAN femaleTitan;
                var playerViewID = transform.root.gameObject.GetPhotonView().viewID;
                if ((mindlessTitan = obj3.GetComponent<MindlessTitan>()) != null)
                {
                    mindlessTitan.OnAnkleHit(playerViewID, speed);
                    showCriticalHitFX(other.gameObject.transform.position);
                }
                else if ((femaleTitan = obj3.GetComponent<FEMALE_TITAN>()) != null)
                {
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        if (femaleTitan != null && !femaleTitan.hasDie)
                        {
                            if (other.gameObject.name == "ankleR")
                                femaleTitan.hitAnkleR(speed);
                            else
                                femaleTitan.hitAnkleL(speed);
                        }
                    }
                    else if (other.gameObject.name == "ankleR")
                    {
                        if (!femaleTitan.hasDie)
                        {
                            if (PhotonNetwork.isMasterClient)
                                femaleTitan.hitAnkleRRPC(playerViewID, speed);
                            else if (!femaleTitan.hasDie)
                                femaleTitan.photonView.RPC(nameof(femaleTitan.hitAnkleRRPC), PhotonTargets.MasterClient, playerViewID, speed);
                        }
                    }
                    else if (!PhotonNetwork.isMasterClient)
                    {
                        if (!femaleTitan.hasDie)
                            femaleTitan.photonView.RPC(nameof(femaleTitan.hitAnkleLRPC), PhotonTargets.MasterClient, playerViewID, speed);
                    }
                    else if (!femaleTitan.hasDie)
                    {
                        femaleTitan.hitAnkleLRPC(transform.root.gameObject.GetPhotonView().viewID, speed);
                    }

                    showCriticalHitFX(other.gameObject.transform.position);
                }
            }
        }
    }

    private void showCriticalHitFX(Vector3 position)
    {
        GameObject obj2;
        this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.2f, 0.3f, 0.95f);
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            obj2 = PhotonNetwork.Instantiate("redCross1", base.transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
        }
        else
        {
            obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("redCross1"));
        }
        obj2.transform.position = position;
    }

    private void Start()
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
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
        }
        else
        {
            this.myTeam = GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Hero>().myTeam;
        }
        this.active_me = true;
        this.count = 0;
        this.currentCamera = GameObject.Find("MainCamera");
    }
}

