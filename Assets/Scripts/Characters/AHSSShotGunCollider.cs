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
                if (FengGameManagerMKII.Gamemode.Pvp != PvpMode.Disabled)
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
                            component.transform.root.GetComponent<Hero>().markDie();
                            object[] parameters = new object[5];
                            Vector3 vector2 = component.transform.root.position - base.transform.position;
                            parameters[0] = (Vector3) (((vector2.normalized * b) * 1000f) + (Vector3.up * 50f));
                            parameters[1] = false;
                            parameters[2] = this.viewID;
                            parameters[3] = this.ownerName;
                            parameters[4] = false;
                            component.transform.root.GetComponent<Hero>().photonView.RPC("netDie", PhotonTargets.All, parameters);
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
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        if ((item.transform.root.GetComponent<TITAN>() != null) && !item.transform.root.GetComponent<TITAN>().hasDie)
                        {
                            Vector3 vector3 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                            int num2 = (int) ((vector3.magnitude * 10f) * this.scoreMulti);
                            num2 = Mathf.Max(10, num2);
                            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().netShowDamage(num2);
                            if (num2 > (item.transform.root.GetComponent<TITAN>().myLevel * 100f))
                            {
                                item.transform.root.GetComponent<TITAN>().die();
                                if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
                                {
                                    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(item.transform.position, num2, item.transform.root.gameObject, 0.02f);
                                }
                                GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().playerKillInfoSingleUpdate(num2);
                            }
                        }
                    }
                    else if (!PhotonNetwork.isMasterClient)
                    {
                        if (item.transform.root.GetComponent<TITAN>() != null)
                        {
                            if (!item.transform.root.GetComponent<TITAN>().hasDie)
                            {
                                Vector3 vector4 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                                int num3 = (int) ((vector4.magnitude * 10f) * this.scoreMulti);
                                num3 = Mathf.Max(10, num3);
                                if (num3 > (item.transform.root.GetComponent<TITAN>().myLevel * 100f))
                                {
                                    if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
                                    {
                                        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(item.transform.position, num3, item.transform.root.gameObject, 0.02f);
                                        item.transform.root.GetComponent<TITAN>().asClientLookTarget = false;
                                    }
                                    object[] objArray2 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID, num3 };
                                    item.transform.root.GetComponent<TITAN>().photonView.RPC("titanGetHit", item.transform.root.GetComponent<TITAN>().photonView.owner, objArray2);
                                }
                            }
                        }
                        else if (item.transform.root.GetComponent<FEMALE_TITAN>() != null)
                        {
                            Vector3 vector5 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                            int num4 = (int) ((vector5.magnitude * 10f) * this.scoreMulti);
                            num4 = Mathf.Max(10, num4);
                            if (!item.transform.root.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                object[] objArray3 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID, num4 };
                                item.transform.root.GetComponent<FEMALE_TITAN>().photonView.RPC("titanGetHit", item.transform.root.GetComponent<FEMALE_TITAN>().photonView.owner, objArray3);
                            }
                        }
                        else if ((item.transform.root.GetComponent<COLOSSAL_TITAN>() != null) && !item.transform.root.GetComponent<COLOSSAL_TITAN>().hasDie)
                        {
                            Vector3 vector6 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                            int num5 = (int) ((vector6.magnitude * 10f) * this.scoreMulti);
                            num5 = Mathf.Max(10, num5);
                            object[] objArray4 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID, num5 };
                            item.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.RPC("titanGetHit", item.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.owner, objArray4);
                        }
                    }
                    else if (item.transform.root.GetComponent<TITAN>() != null)
                    {
                        if (!item.transform.root.GetComponent<TITAN>().hasDie)
                        {
                            Vector3 vector7 = this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                            int num6 = (int) ((vector7.magnitude * 10f) * this.scoreMulti);
                            num6 = Mathf.Max(10, num6);
                            if (num6 > (item.transform.root.GetComponent<TITAN>().myLevel * 100f))
                            {
                                if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
                                {
                                    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(item.transform.position, num6, item.transform.root.gameObject, 0.02f);
                                }
                                item.transform.root.GetComponent<TITAN>().titanGetHit(base.transform.root.gameObject.GetPhotonView().viewID, num6);
                            }
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
                    if (gameObject.GetComponent<FEMALE_TITAN>() != null)
                    {
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                        {
                            if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                gameObject.GetComponent<FEMALE_TITAN>().hitEye();
                            }
                        }
                        else if (!PhotonNetwork.isMasterClient)
                        {
                            if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                object[] objArray5 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID };
                                gameObject.GetComponent<FEMALE_TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, objArray5);
                            }
                        }
                        else if (!gameObject.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            gameObject.GetComponent<FEMALE_TITAN>().hitEyeRPC(base.transform.root.gameObject.GetPhotonView().viewID);
                        }
                    }
                    else if (gameObject.GetComponent<TITAN>().TitanType != TitanType.TYPE_CRAWLER)
                    {
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                        {
                            if (!gameObject.GetComponent<TITAN>().hasDie)
                            {
                                gameObject.GetComponent<TITAN>().hitEye();
                            }
                        }
                        else if (!PhotonNetwork.isMasterClient)
                        {
                            if (!gameObject.GetComponent<TITAN>().hasDie)
                            {
                                object[] objArray6 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID };
                                gameObject.GetComponent<TITAN>().photonView.RPC("hitEyeRPC", PhotonTargets.MasterClient, objArray6);
                            }
                        }
                        else if (!gameObject.GetComponent<TITAN>().hasDie)
                        {
                            gameObject.GetComponent<TITAN>().hitEyeRPC(base.transform.root.gameObject.GetPhotonView().viewID);
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
                if ((obj3.GetComponent<TITAN>() != null) && (obj3.GetComponent<TITAN>().TitanType != TitanType.TYPE_CRAWLER))
                {
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        if (!obj3.GetComponent<TITAN>().hasDie)
                        {
                            obj3.GetComponent<TITAN>().hitAnkle();
                        }
                    }
                    else
                    {
                        if (!PhotonNetwork.isMasterClient)
                        {
                            if (!obj3.GetComponent<TITAN>().hasDie)
                            {
                                object[] objArray7 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID };
                                obj3.GetComponent<TITAN>().photonView.RPC("hitAnkleRPC", PhotonTargets.MasterClient, objArray7);
                            }
                        }
                        else if (!obj3.GetComponent<TITAN>().hasDie)
                        {
                            obj3.GetComponent<TITAN>().hitAnkle();
                        }
                        this.showCriticalHitFX(other.gameObject.transform.position);
                    }
                }
                else if (obj3.GetComponent<FEMALE_TITAN>() != null)
                {
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        if (other.gameObject.name == "ankleR")
                        {
                            if ((obj3.GetComponent<FEMALE_TITAN>() != null) && !obj3.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                obj3.GetComponent<FEMALE_TITAN>().hitAnkleR(num9);
                            }
                        }
                        else if ((obj3.GetComponent<FEMALE_TITAN>() != null) && !obj3.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            obj3.GetComponent<FEMALE_TITAN>().hitAnkleL(num9);
                        }
                    }
                    else if (other.gameObject.name == "ankleR")
                    {
                        if (!PhotonNetwork.isMasterClient)
                        {
                            if (!obj3.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                object[] objArray8 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID, num9 };
                                obj3.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleRRPC", PhotonTargets.MasterClient, objArray8);
                            }
                        }
                        else if (!obj3.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            obj3.GetComponent<FEMALE_TITAN>().hitAnkleRRPC(base.transform.root.gameObject.GetPhotonView().viewID, num9);
                        }
                    }
                    else if (!PhotonNetwork.isMasterClient)
                    {
                        if (!obj3.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            object[] objArray9 = new object[] { base.transform.root.gameObject.GetPhotonView().viewID, num9 };
                            obj3.GetComponent<FEMALE_TITAN>().photonView.RPC("hitAnkleLRPC", PhotonTargets.MasterClient, objArray9);
                        }
                    }
                    else if (!obj3.GetComponent<FEMALE_TITAN>().hasDie)
                    {
                        obj3.GetComponent<FEMALE_TITAN>().hitAnkleLRPC(base.transform.root.gameObject.GetPhotonView().viewID, num9);
                    }
                    this.showCriticalHitFX(other.gameObject.transform.position);
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

