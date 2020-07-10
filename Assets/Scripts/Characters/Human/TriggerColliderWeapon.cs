using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
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
    public int myTeam = 1;
    public float scoreMulti = 1f;

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
        Vector3 vector3 = currentCamera.main_object.GetComponent<Rigidbody>().velocity;
        int num2 = (int)((vector3.magnitude * 10f) * scoreMulti);
        num2 = Mathf.Max(10, num2);
        
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            titan.GetHit(num2);
        else
            titan.photonView.RPC(nameof(titan.GetHit), PhotonTargets.All, num2);

        FengGameManagerMKII.instance.netShowDamage(num2);
    }

    public void clearHits()
    {
        currentHitsII = new ArrayList();
        currentHits = new ArrayList();
    }

    private void napeMeat(Vector3 vkill, Transform titan)
    {
        Transform transform = titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
        GameObject obj2 = (GameObject) Instantiate(Resources.Load("titanNapeMeat"), transform.position, transform.rotation);
        obj2.transform.localScale = titan.localScale;
        obj2.GetComponent<Rigidbody>().AddForce((Vector3) (vkill.normalized * 15f), ForceMode.Impulse);
        obj2.GetComponent<Rigidbody>().AddForce((Vector3) (-titan.forward * 10f), ForceMode.Impulse);
        obj2.GetComponent<Rigidbody>().AddTorque(new Vector3((float) Random.Range(-100, 100), (float) Random.Range(-100, 100), (float) Random.Range(-100, 100)), ForceMode.Impulse);
    }

    private void OnTriggerStay(Collider other)
    {
        if (active_me)
        {
            var viewID = transform.root.gameObject.GetPhotonView().viewID;
            if (!currentHitsII.Contains(other.gameObject))
            {
                currentHitsII.Add(other.gameObject);
                currentCamera.startShake(0.1f, 0.1f, 0.95f);
                if (other.gameObject.transform.root.gameObject.tag == "titan")
                {
                    GameObject obj2;
                    currentCamera.main_object.GetComponent<Hero>().slashHit.Play();
                    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                    {
                        obj2 = PhotonNetwork.Instantiate("hitMeat", transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
                    }
                    else
                    {
                        obj2 = (GameObject) Instantiate(Resources.Load("hitMeat"));
                    }
                    obj2.transform.position = transform.position;
                    Equipment.Weapon.Use(0);
                }
            }
            if (other.gameObject.tag == "playerHitbox")
            {
                if (FengGameManagerMKII.Gamemode.Settings.Pvp != PvpMode.Disabled)
                {
                    float b = 1f - (Vector3.Distance(other.gameObject.transform.position, transform.position) * 0.05f);
                    b = Mathf.Min(1f, b);
                    HitBox hitBox = other.gameObject.GetComponent<HitBox>();
                    var hitHero = hitBox.transform.root.GetComponent<Hero>();
                    if ((((hitBox != null) && (hitBox.transform.root != null)) && (hitHero.myTeam != myTeam)) && !hitHero.IsInvincible())
                    {
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                        {
                            if (!hitHero.IsGrabbed)
                            {
                                Vector3 vector = hitBox.transform.root.transform.position - transform.position;
                                hitHero.Die((Vector3) (((vector.normalized * b) * 1000f) + (Vector3.up * 50f)), false);
                            }
                        }
                        else if (((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && !hitHero.HasDied()) && !hitHero.IsGrabbed)
                        {
                            hitHero.MarkDie();
                            hitHero.photonView.RPC(
                                nameof(hitHero.netDie),
                                PhotonTargets.All,
                                (hitBox.transform.root.position - transform.position).normalized * b * 1000f + Vector3.up * 50f,
                                false,
                                viewID,
                                PhotonView.Find(viewID).owner.CustomProperties[PhotonPlayerProperty.name],
                                false);
                        }
                    }
                }
            }
            else if (other.gameObject.tag == "titanneck")
            {
                HitBox item = other.gameObject.GetComponent<HitBox>();
                if (((item != null) && checkIfBehind(item.transform.root.gameObject)) && !currentHits.Contains(item))
                {
                    item.hitPosition = (Vector3) ((transform.position + item.transform.position) * 0.5f);
                    currentHits.Add(item);
                    meatDie.Play();
                    if (item.transform.root.GetComponent<MindlessTitan>() != null)
                    {
                        var vector4 = currentCamera.main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                        var damage = (int)((vector4.magnitude * 10f) * scoreMulti);
                        damage = Mathf.Max(10, damage);
                        var mindlessTitan = item.transform.root.GetComponent<MindlessTitan>();
                        if (PhotonNetwork.isMasterClient)
                        {
                            mindlessTitan.OnNapeHitRpc(viewID, damage);
                        }
                        else
                        {
                            mindlessTitan.photonView.RPC(nameof(mindlessTitan.OnNapeHitRpc), mindlessTitan.photonView.owner, viewID, damage);
                        }
                    }
                    else if (!PhotonNetwork.isMasterClient)
                    {
                        if (item.transform.root.GetComponent<FEMALE_TITAN>() != null)
                        {
                            Equipment.Weapon.Use(0x7fffffff);
                            var vector5 = currentCamera.main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                            int num4 = (int) ((vector5.magnitude * 10f) * scoreMulti);
                            num4 = Mathf.Max(10, num4);
                            if (!item.transform.root.GetComponent<FEMALE_TITAN>().hasDie)
                            {
                                object[] objArray3 = new object[] { viewID, num4 };
                                item.transform.root.GetComponent<FEMALE_TITAN>().photonView.RPC("titanGetHit", item.transform.root.GetComponent<FEMALE_TITAN>().photonView.owner, objArray3);
                            }
                        }
                        else if (item.transform.root.GetComponent<COLOSSAL_TITAN>() != null)
                        {
                            Equipment.Weapon.Use(0x7fffffff);
                            if (!item.transform.root.GetComponent<COLOSSAL_TITAN>().hasDie)
                            {
                                var vector6 = currentCamera.main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                                int num5 = (int) ((vector6.magnitude * 10f) * scoreMulti);
                                num5 = Mathf.Max(10, num5);
                                object[] objArray4 = new object[] { viewID, num5 };
                                item.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.RPC("titanGetHit", item.transform.root.GetComponent<COLOSSAL_TITAN>().photonView.owner, objArray4);
                            }
                        }
                        else if (item.transform.root.GetComponent<DummyTitan>())
                        {
                            DummyNapeHit(item.transform.root.GetComponent<DummyTitan>());
                        }
                    }
                    else if (item.transform.root.GetComponent<FEMALE_TITAN>() != null)
                    {
                        Equipment.Weapon.Use(0x7fffffff);
                        if (!item.transform.root.GetComponent<FEMALE_TITAN>().hasDie)
                        {
                            var vector8 = currentCamera.main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                            int num7 = (int) ((vector8.magnitude * 10f) * scoreMulti);
                            num7 = Mathf.Max(10, num7);
                            if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
                            {
                                GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(item.transform.position, num7, null, 0.02f);
                            }
                            item.transform.root.GetComponent<FEMALE_TITAN>().titanGetHit(viewID, num7);
                        }
                    }
                    else if (item.transform.root.GetComponent<COLOSSAL_TITAN>() != null)
                    {
                        Equipment.Weapon.Use(0x7fffffff);
                        if (!item.transform.root.GetComponent<COLOSSAL_TITAN>().hasDie)
                        {
                            var vector9 = currentCamera.main_object.GetComponent<Rigidbody>().velocity - item.transform.root.GetComponent<Rigidbody>().velocity;
                            int num8 = (int) ((vector9.magnitude * 10f) * scoreMulti);
                            num8 = Mathf.Max(10, num8);
                            if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
                            {
                                GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(item.transform.position, num8, null, 0.02f);
                            }
                            item.transform.root.GetComponent<COLOSSAL_TITAN>().titanGetHit(viewID, num8);
                        }
                    }
                    else if (item.transform.root.GetComponent<DummyTitan>())
                    {
                        DummyNapeHit(item.transform.root.GetComponent<DummyTitan>());
                    }
                    showCriticalHitFX();
                }
            }
            else if (other.gameObject.tag == "titaneye")
            {
                if (!currentHits.Contains(other.gameObject))
                {
                    currentHits.Add(other.gameObject);
                    GameObject gameObject = other.gameObject.transform.root.gameObject;
                    var femaleTitan = gameObject.GetComponent<FEMALE_TITAN>();
                    if (femaleTitan != null)
                    {
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                        {
                            if (!femaleTitan.hasDie)
                                femaleTitan.hitEye();
                        }
                        else if (!PhotonNetwork.isMasterClient)
                        {
                            if (!femaleTitan.hasDie)
                                femaleTitan.photonView.RPC(nameof(femaleTitan.hitEyeRPC), PhotonTargets.MasterClient, viewID);
                        }
                        else if (!femaleTitan.hasDie)
                        {
                            femaleTitan.hitEyeRPC(viewID);
                        }
                    }
                    else if (gameObject.GetComponent<MindlessTitan>() != null)
                    {
                        var vector4 = currentCamera.main_object.GetComponent<Rigidbody>().velocity - gameObject.GetComponent<Rigidbody>().velocity;
                        var damage = (int)((vector4.magnitude * 10f) * scoreMulti);
                        damage = Mathf.Max(10, damage);
                        var mindlessTitan = gameObject.GetComponent<MindlessTitan>();
                        if (PhotonNetwork.isMasterClient)
                        {
                            mindlessTitan.OnEyeHitRpc(viewID, damage);
                        }
                        else
                        {
                            mindlessTitan.photonView.RPC("OnEyeHitRpc", mindlessTitan.photonView.owner, viewID, damage);
                        }
                        showCriticalHitFX();
                    }
                }
            }
            else if (other.gameObject.tag == "titanbodypart")
            {
                if (!currentHits.Contains(other.gameObject))
                {
                    currentHits.Add(other.gameObject);
                    GameObject gameObject = other.gameObject.transform.root.gameObject;
                    if (gameObject.GetComponent<MindlessTitan>() != null)
                    {
                        var vector4 = currentCamera.main_object.GetComponent<Rigidbody>().velocity - gameObject.GetComponent<Rigidbody>().velocity;
                        var damage = (int)((vector4.magnitude * 10f) * scoreMulti);
                        damage = Mathf.Max(10, damage);
                        var mindlessTitan = gameObject.GetComponent<MindlessTitan>();
                        var body = mindlessTitan.TitanBody.GetBodyPart(other.transform);
                        if (PhotonNetwork.isMasterClient)
                        {
                            mindlessTitan.OnBodyPartHitRpc(body, damage);
                        }
                        else
                        {
                            mindlessTitan.photonView.RPC("OnBodyPartHitRpc", mindlessTitan.photonView.owner, body, damage);
                        }
                    }
                }
            }
            else if ((other.gameObject.tag == "titanankle") && !currentHits.Contains(other.gameObject))
            {
                currentHits.Add(other.gameObject);
                GameObject obj4 = other.gameObject.transform.root.gameObject;
                Vector3 vector10 = Vector3.zero;
                if (obj4.GetComponent<Rigidbody>())//patch for dummy titan
                {
                    vector10 = currentCamera.main_object.GetComponent<Rigidbody>().velocity - obj4.GetComponent<Rigidbody>().velocity;
                }
                int num9 = (int) ((vector10.magnitude * 10f) * scoreMulti);
                num9 = Mathf.Max(10, num9);
                var femaleTitan = obj4.GetComponent<FEMALE_TITAN>();
                var mindlessTitan = obj4.GetComponent<MindlessTitan>();
                if (mindlessTitan != null)
                {
                    mindlessTitan.OnAnkleHit(viewID, num9);
                    showCriticalHitFX();
                }
                else if (femaleTitan != null)
                {
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        if (other.gameObject.name == "ankleR")
                        {
                            if (femaleTitan != null && !femaleTitan.hasDie)
                            {
                                femaleTitan.hitAnkleR(num9);
                            }
                        }
                        else if (femaleTitan != null && !femaleTitan.hasDie)
                        {
                            femaleTitan.hitAnkleL(num9);
                        }
                    }
                    else if (other.gameObject.name == "ankleR")
                    {
                        if (!PhotonNetwork.isMasterClient)
                        {
                            if (!femaleTitan.hasDie)
                            {
                                femaleTitan.photonView.RPC(
                                    nameof(femaleTitan.hitAnkleRRPC),
                                    PhotonTargets.MasterClient,
                                    viewID,
                                    num9);
                            }
                        }
                        else if (!femaleTitan.hasDie)
                            femaleTitan.hitAnkleRRPC(viewID, num9);
                    }
                    else if (!PhotonNetwork.isMasterClient)
                    {
                        if (!femaleTitan.hasDie)
                            femaleTitan.photonView.RPC(
                                nameof(femaleTitan.hitAnkleLRPC),
                                PhotonTargets.MasterClient,
                                viewID,
                                num9);
                    }
                    else if (!femaleTitan.hasDie)
                        femaleTitan.hitAnkleLRPC(viewID, num9);

                    showCriticalHitFX();
                }
                else if(obj4.GetComponent<DummyTitan>())
                    showCriticalHitFX();
            }
        }
    }

    private void showCriticalHitFX()
    {
        GameObject obj2;
        currentCamera.startShake(0.2f, 0.3f, 0.95f);
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            obj2 = PhotonNetwork.Instantiate("redCross", transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
        }
        else
        {
            obj2 = (GameObject) Instantiate(Resources.Load("redCross"));
        }
        obj2.transform.position = transform.position;
    }

    private void Start()
    {
        currentCamera = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>();
        Equipment = transform.root.GetComponent<Equipment>();
    }
}

