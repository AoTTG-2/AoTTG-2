using Assets.Scripts.Gamemode.Options;
using System;
using System.Collections;
using UnityEngine;

public class COLOSSAL_TITAN : Photon.MonoBehaviour
{
    private string actionName;
    private string attackAnimation;
    private float attackCheckTime;
    private float attackCheckTimeA;
    private float attackCheckTimeB;
    private bool attackChkOnce;
    private int attackCount;
    private int attackPattern = -1;
    public GameObject bottomObject;
    private Transform checkHitCapsuleEnd;
    private Vector3 checkHitCapsuleEndOld;
    private float checkHitCapsuleR;
    private Transform checkHitCapsuleStart;
    public GameObject door_broken;
    public GameObject door_closed;
    public bool hasDie;
    public bool hasspawn;
    public GameObject healthLabel;
    public float healthTime;
    private bool isSteamNeed;
    public float lagMax;
    public int maxHealth;
    public static float minusDistance = 99999f;
    public static GameObject minusDistanceEnemy;
    public float myDistance;
    public GameObject myHero;
    public int NapeArmor = 0x2710;
    public int NapeArmorTotal = 0x2710;
    public GameObject neckSteamObject;
    public float size;
    private string state = "idle";
    public GameObject sweepSmokeObject;
    public float tauntTime;
    private float waitTime = 2f;

    private void attack_sweep(string type = "")
    {
        this.callTitanHAHA();
        this.state = "attack_sweep";
        this.attackAnimation = "sweep" + type;
        this.attackCheckTimeA = 0.4f;
        this.attackCheckTimeB = 0.57f;
        this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R");
        this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
        this.checkHitCapsuleR = 20f;
        this.crossFade("attack_" + this.attackAnimation, 0.1f);
        this.attackChkOnce = false;
        this.sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = true;
        this.sweepSmokeObject.GetComponent<ParticleSystem>().Play();
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
            if (FengGameManagerMKII.LAN)
            {
                if (Network.peerType != NetworkPeerType.Server)
                {
                }
            }
            else if (PhotonNetwork.isMasterClient)
            {
                base.photonView.RPC("startSweepSmoke", PhotonTargets.Others, new object[0]);
            }
        }
    }

    private void Awake()
    {
        base.GetComponent<Rigidbody>().freezeRotation = true;
        base.GetComponent<Rigidbody>().useGravity = false;
        base.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void beTauntedBy(GameObject target, float tauntTime)
    {
    }

    public void blowPlayer(GameObject player, Transform neck)
    {
        Vector3 vector = -(Vector3)((neck.position + (base.transform.forward * 50f)) - player.transform.position);
        float num = 20f;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            player.GetComponent<Hero>().blowAway((Vector3) ((vector.normalized * num) + (Vector3.up * 1f)));
        }
        else if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
        {
            object[] parameters = new object[] { (Vector3) ((vector.normalized * num) + (Vector3.up * 1f)) };
            player.GetComponent<Hero>().photonView.RPC("blowAway", PhotonTargets.All, parameters);
        }
    }

    private void callTitan(bool special = false)
    {
        if (special || (GameObject.FindGameObjectsWithTag("titan").Length <= 6))
        {
            GameObject obj4;
            GameObject[] objArray = GameObject.FindGameObjectsWithTag("titanRespawn");
            ArrayList list = new ArrayList();
            foreach (GameObject obj2 in objArray)
            {
                if (obj2.transform.parent.name == "titanRespawnCT")
                {
                    list.Add(obj2);
                }
            }
            GameObject obj3 = (GameObject) list[UnityEngine.Random.Range(0, list.Count)];
            string[] strArray = new string[] { "TITAN_VER3.1" };
            if (FengGameManagerMKII.LAN)
            {
                obj4 = (GameObject) Network.Instantiate(Resources.Load(strArray[UnityEngine.Random.Range(0, strArray.Length)]), obj3.transform.position, obj3.transform.rotation, 0);
            }
            else
            {
                obj4 = PhotonNetwork.Instantiate(strArray[UnityEngine.Random.Range(0, strArray.Length)], obj3.transform.position, obj3.transform.rotation, 0);
            }
            if (special)
            {
                GameObject[] objArray3 = GameObject.FindGameObjectsWithTag("route");
                GameObject route = objArray3[UnityEngine.Random.Range(0, objArray3.Length)];
                while (route.name != "routeCT")
                {
                    route = objArray3[UnityEngine.Random.Range(0, objArray3.Length)];
                }
                obj4.GetComponent<TITAN>().setRoute(route);
                obj4.GetComponent<TITAN>().setAbnormalType2(TitanType.TYPE_I, false);
                obj4.GetComponent<TITAN>().activeRad = 0;
                obj4.GetComponent<TITAN>().toCheckPoint((Vector3) obj4.GetComponent<TITAN>().checkPoints[0], 10f);
            }
            else
            {
                float num2 = 0.7f;
                float num3 = 0.7f;
                if (IN_GAME_MAIN_CAMERA.difficulty != 0)
                {
                    if (IN_GAME_MAIN_CAMERA.difficulty == 1)
                    {
                        num2 = 0.4f;
                        num3 = 0.7f;
                    }
                    else if (IN_GAME_MAIN_CAMERA.difficulty == 2)
                    {
                        num2 = -1f;
                        num3 = 0.7f;
                    }
                }
                if (GameObject.FindGameObjectsWithTag("titan").Length == 5)
                {
                    obj4.GetComponent<TITAN>().setAbnormalType2(TitanType.TYPE_JUMPER, false);
                }
                else if (UnityEngine.Random.Range((float) 0f, (float) 1f) >= num2)
                {
                    if (UnityEngine.Random.Range((float) 0f, (float) 1f) < num3)
                    {
                        obj4.GetComponent<TITAN>().setAbnormalType2(TitanType.TYPE_JUMPER, false);
                    }
                    else
                    {
                        obj4.GetComponent<TITAN>().setAbnormalType2(TitanType.TYPE_CRAWLER, false);
                    }
                }
                obj4.GetComponent<TITAN>().activeRad = 200;
            }
            if (FengGameManagerMKII.LAN)
            {
                GameObject obj6 = (GameObject) Network.Instantiate(Resources.Load("FX/FXtitanSpawn"), obj4.transform.position, Quaternion.Euler(-90f, 0f, 0f), 0);
                obj6.transform.localScale = obj4.transform.localScale;
            }
            else
            {
                PhotonNetwork.Instantiate("FX/FXtitanSpawn", obj4.transform.position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = obj4.transform.localScale;
            }
        }
    }

    private void callTitanHAHA()
    {
        this.attackCount++;
        int num = 4;
        int num2 = 7;
        if (IN_GAME_MAIN_CAMERA.difficulty != 0)
        {
            if (IN_GAME_MAIN_CAMERA.difficulty == 1)
            {
                num = 4;
                num2 = 6;
            }
            else if (IN_GAME_MAIN_CAMERA.difficulty == 2)
            {
                num = 3;
                num2 = 5;
            }
        }
        if ((this.attackCount % num) == 0)
        {
            this.callTitan(false);
        }
        if (this.NapeArmor < (this.NapeArmorTotal * 0.3))
        {
            if ((this.attackCount % ((int) (num2 * 0.5f))) == 0)
            {
                this.callTitan(true);
            }
        }
        else if ((this.attackCount % num2) == 0)
        {
            this.callTitan(true);
        }
    }

    [PunRPC]
    private void changeDoor()
    {
        this.door_broken.SetActive(true);
        this.door_closed.SetActive(false);
    }

    private RaycastHit[] checkHitCapsule(Vector3 start, Vector3 end, float r)
    {
        return Physics.SphereCastAll(start, r, end - start, Vector3.Distance(start, end));
    }

    private GameObject checkIfHitHand(Transform hand)
    {
        foreach (Collider collider in Physics.OverlapSphere(hand.GetComponent<SphereCollider>().transform.position, 31f))
        {
            if (collider.transform.root.tag == "Player")
            {
                GameObject gameObject = collider.transform.root.gameObject;
                if (gameObject.GetComponent<TITAN_EREN>() != null)
                {
                    if (!gameObject.GetComponent<TITAN_EREN>().isHit)
                    {
                        gameObject.GetComponent<TITAN_EREN>().hitByTitan();
                    }
                    return gameObject;
                }
                if ((gameObject.GetComponent<Hero>() != null) && !gameObject.GetComponent<Hero>().isInvincible())
                {
                    return gameObject;
                }
            }
        }
        return null;
    }

    private void crossFade(string aniName, float time)
    {
        base.GetComponent<Animation>().CrossFade(aniName, time);
        if ((!FengGameManagerMKII.LAN && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)) && PhotonNetwork.isMasterClient)
        {
            object[] parameters = new object[] { aniName, time };
            base.photonView.RPC("netCrossFade", PhotonTargets.Others, parameters);
        }
    }

    private void findNearestHero()
    {
        this.myHero = this.getNearestHero();
    }

    private GameObject getNearestHero()
    {
        GameObject[] objArray = GameObject.FindGameObjectsWithTag("Player");
        GameObject obj2 = null;
        float positiveInfinity = float.PositiveInfinity;
        foreach (GameObject obj3 in objArray)
        {
            if (((obj3.GetComponent<Hero>() == null) || !obj3.GetComponent<Hero>().HasDied()) && ((obj3.GetComponent<TITAN_EREN>() == null) || !obj3.GetComponent<TITAN_EREN>().hasDied))
            {
                float num3 = Mathf.Sqrt(((obj3.transform.position.x - base.transform.position.x) * (obj3.transform.position.x - base.transform.position.x)) + ((obj3.transform.position.z - base.transform.position.z) * (obj3.transform.position.z - base.transform.position.z)));
                if (((obj3.transform.position.y - base.transform.position.y) < 450f) && (num3 < positiveInfinity))
                {
                    obj2 = obj3;
                    positiveInfinity = num3;
                }
            }
        }
        return obj2;
    }

    private void idle()
    {
        this.state = "idle";
        this.crossFade("idle", 0.2f);
    }

    private void kick()
    {
        this.state = "kick";
        this.actionName = "attack_kick_wall";
        this.attackCheckTime = 0.64f;
        this.attackChkOnce = false;
        this.crossFade(this.actionName, 0.1f);
    }

    private void killPlayer(GameObject hitHero)
    {
        if (hitHero != null)
        {
            Vector3 position = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                if (!hitHero.GetComponent<Hero>().HasDied())
                {
                    hitHero.GetComponent<Hero>().die((Vector3) (((hitHero.transform.position - position) * 15f) * 4f), false);
                }
            }
            else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            {
                if (FengGameManagerMKII.LAN)
                {
                    if (!hitHero.GetComponent<Hero>().HasDied())
                    {
                        hitHero.GetComponent<Hero>().markDie();
                    }
                }
                else if (!hitHero.GetComponent<Hero>().HasDied())
                {
                    hitHero.GetComponent<Hero>().markDie();
                    object[] parameters = new object[] { (Vector3) (((hitHero.transform.position - position) * 15f) * 4f), false, -1, "Colossal Titan", true };
                    hitHero.GetComponent<Hero>().photonView.RPC("netDie", PhotonTargets.All, parameters);
                }
            }
        }
    }

    [PunRPC]
    public void labelRPC(int health, int maxHealth)
    {
        if (health < 0)
        {
            if (this.healthLabel != null)
            {
                UnityEngine.Object.Destroy(this.healthLabel);
            }
        }
        else
        {
            if (this.healthLabel == null)
            {
                this.healthLabel = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
                this.healthLabel.name = "LabelNameOverHead";
                this.healthLabel.transform.parent = base.transform;
                this.healthLabel.transform.localPosition = new Vector3(0f, 430f, 0f);
                float a = 15f;
                if ((this.size > 0f) && (this.size < 1f))
                {
                    a = 15f / this.size;
                    a = Mathf.Min(a, 100f);
                }
                this.healthLabel.transform.localScale = new Vector3(a, a, a);
            }
            string str = "[7FFF00]";
            float num2 = ((float) health) / ((float) maxHealth);
            if ((num2 < 0.75f) && (num2 >= 0.5f))
            {
                str = "[f2b50f]";
            }
            else if ((num2 < 0.5f) && (num2 >= 0.25f))
            {
                str = "[ff8100]";
            }
            else if (num2 < 0.25f)
            {
                str = "[ff3333]";
            }
            //this.healthLabel.GetComponent<UILabel>().text = str + Convert.ToString(health);
        }
    }

    public void loadskin()
    {
        if (PhotonNetwork.isMasterClient && (((int) FengGameManagerMKII.settings[1]) == 1))
        {
            base.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, new object[] { (string) FengGameManagerMKII.settings[0x43] });
        }
    }

    public IEnumerator loadskinE(string url)
    {
        while (!this.hasspawn)
        {
            yield return null;
        }
        bool mipmap = true;
        bool iteratorVariable1 = false;
        if (((int)FengGameManagerMKII.settings[0x3f]) == 1)
        {
            mipmap = false;
        }
        foreach (Renderer iteratorVariable2 in this.GetComponentsInChildren<Renderer>())
        {
            if (iteratorVariable2.name.Contains("hair"))
            {
                if (!FengGameManagerMKII.linkHash[2].ContainsKey(url))
                {
                    WWW link = new WWW(url);
                    yield return link;
                    Texture2D iteratorVariable4 = RCextensions.loadimage(link, mipmap, 0xf4240);
                    link.Dispose();
                    if (!FengGameManagerMKII.linkHash[2].ContainsKey(url))
                    {
                        iteratorVariable1 = true;
                        iteratorVariable2.material.mainTexture = iteratorVariable4;
                        FengGameManagerMKII.linkHash[2].Add(url, iteratorVariable2.material);
                        iteratorVariable2.material = (Material)FengGameManagerMKII.linkHash[2][url];
                    }
                    else
                    {
                        iteratorVariable2.material = (Material)FengGameManagerMKII.linkHash[2][url];
                    }
                }
                else
                {
                    iteratorVariable2.material = (Material)FengGameManagerMKII.linkHash[2][url];
                }
            }
        }
        if (iteratorVariable1)
        {
            FengGameManagerMKII.instance.unloadAssets();
        }
    }

    [PunRPC]
    public void loadskinRPC(string url)
    {
        if ((((int) FengGameManagerMKII.settings[1]) == 1) && ((url.EndsWith(".jpg") || url.EndsWith(".png")) || url.EndsWith(".jpeg")))
        {
            base.StartCoroutine(this.loadskinE(url));
        }
    }

    private void neckSteam()
    {
        this.neckSteamObject.GetComponent<ParticleSystem>().Stop();
        this.neckSteamObject.GetComponent<ParticleSystem>().Play();
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
            if (FengGameManagerMKII.LAN)
            {
                if (Network.peerType != NetworkPeerType.Server)
                {
                }
            }
            else if (PhotonNetwork.isMasterClient)
            {
                base.photonView.RPC("startNeckSteam", PhotonTargets.Others, new object[0]);
            }
        }
        this.isSteamNeed = true;
        Transform neck = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
        float radius = 30f;
        foreach (Collider collider in Physics.OverlapSphere(neck.transform.position - ((Vector3) (base.transform.forward * 10f)), radius))
        {
            if (collider.transform.root.tag == "Player")
            {
                GameObject gameObject = collider.transform.root.gameObject;
                if ((gameObject.GetComponent<TITAN_EREN>() == null) && (gameObject.GetComponent<Hero>() != null))
                {
                    this.blowPlayer(gameObject, neck);
                }
            }
        }
    }

    [PunRPC]
    private void netCrossFade(string aniName, float time)
    {
        base.GetComponent<Animation>().CrossFade(aniName, time);
    }

    [PunRPC]
    public void netDie()
    {
        if (!this.hasDie)
        {
            this.hasDie = true;
        }
    }

    [PunRPC]
    private void netPlayAnimation(string aniName)
    {
        base.GetComponent<Animation>().Play(aniName);
    }

    [PunRPC]
    private void netPlayAnimationAt(string aniName, float normalizedTime)
    {
        base.GetComponent<Animation>().Play(aniName);
        base.GetComponent<Animation>()[aniName].normalizedTime = normalizedTime;
    }

    private void OnDestroy()
    {
        if (GameObject.Find("MultiplayerManager") != null)
        {
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().removeCT(this);
        }
    }

    private void playAnimation(string aniName)
    {
        base.GetComponent<Animation>().Play(aniName);
        if ((!FengGameManagerMKII.LAN && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)) && PhotonNetwork.isMasterClient)
        {
            object[] parameters = new object[] { aniName };
            base.photonView.RPC("netPlayAnimation", PhotonTargets.Others, parameters);
        }
    }

    private void playAnimationAt(string aniName, float normalizedTime)
    {
        base.GetComponent<Animation>().Play(aniName);
        base.GetComponent<Animation>()[aniName].normalizedTime = normalizedTime;
        if ((!FengGameManagerMKII.LAN && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)) && PhotonNetwork.isMasterClient)
        {
            object[] parameters = new object[] { aniName, normalizedTime };
            base.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, parameters);
        }
    }

    private void playSound(string sndname)
    {
        this.playsoundRPC(sndname);
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
            if (FengGameManagerMKII.LAN)
            {
                if (Network.peerType != NetworkPeerType.Server)
                {
                }
            }
            else if (PhotonNetwork.isMasterClient)
            {
                object[] parameters = new object[] { sndname };
                base.photonView.RPC("playsoundRPC", PhotonTargets.Others, parameters);
            }
        }
    }

    [PunRPC]
    private void playsoundRPC(string sndname)
    {
        base.transform.Find(sndname).GetComponent<AudioSource>().Play();
    }

    [PunRPC]
    private void removeMe()
    {
        UnityEngine.Object.Destroy(base.gameObject);
    }

    [PunRPC]
    public void setSize(float size, PhotonMessageInfo info)
    {
        size = Mathf.Clamp(size, 0.1f, 50f);
        if (info.sender.isMasterClient)
        {
            Transform transform = base.transform;
            transform.localScale = (Vector3) (transform.localScale * (size * 0.05f));
            this.size = size;
        }
    }

    private void slap(string type)
    {
        this.callTitanHAHA();
        this.state = "slap";
        this.attackAnimation = type;
        if ((type == "r1") || (type == "r2"))
        {
            this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
        }
        if ((type == "l1") || (type == "l2"))
        {
            this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
        }
        this.attackCheckTime = 0.57f;
        this.attackChkOnce = false;
        this.crossFade("attack_slap_" + this.attackAnimation, 0.1f);
    }

    private void Start()
    {
        this.startMain();
        this.size = 20f;
        if (Minimap.instance != null)
        {
            Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.black, false, true, Minimap.IconStyle.CIRCLE);
        }
        if (base.photonView.isMine)
        {
            if (FengGameManagerMKII.Gamemode.TitanCustomSize)
            {
                float sizeLower = FengGameManagerMKII.Gamemode.TitanMinimumSize;
                float sizeUpper = FengGameManagerMKII.Gamemode.TitanMaximumSize;
                this.size = UnityEngine.Random.Range(sizeLower, sizeUpper);
                base.photonView.RPC("setSize", PhotonTargets.AllBuffered, new object[] { this.size });
            }
            this.lagMax = 150f + (this.size * 3f);
            this.healthTime = 0f;
            this.maxHealth = this.NapeArmor;
            if (FengGameManagerMKII.Gamemode.TitanHealthMode != TitanHealthMode.Disabled)
            {
                this.maxHealth = this.NapeArmor = UnityEngine.Random.Range(FengGameManagerMKII.Gamemode.TitanHealthMinimum, FengGameManagerMKII.Gamemode.TitanHealthMaximum);
            }
            if (this.NapeArmor > 0)
            {
                base.photonView.RPC("labelRPC", PhotonTargets.AllBuffered, new object[] { this.NapeArmor, this.maxHealth });
            }
            this.loadskin();
        }
        this.hasspawn = true;
    }

    private void startMain()
    {
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().addCT(this);
        if (this.myHero == null)
        {
            this.findNearestHero();
        }
        base.name = "COLOSSAL_TITAN";
        this.NapeArmor = 0x3e8;
        var flag = FengGameManagerMKII.Gamemode.RespawnMode == RespawnMode.NEVER;
        if (IN_GAME_MAIN_CAMERA.difficulty == 0)
        {
            this.NapeArmor = !flag ? 0x1388 : 0x7d0;
        }
        else if (IN_GAME_MAIN_CAMERA.difficulty == 1)
        {
            this.NapeArmor = !flag ? 0x1f40 : 0xdac;
            IEnumerator enumerator = base.GetComponent<Animation>().GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AnimationState current = (AnimationState)enumerator.Current;
                    if (current != null)
                        current.speed = 1.02f;
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
        else if (IN_GAME_MAIN_CAMERA.difficulty == 2)
        {
            this.NapeArmor = !flag ? 0x2ee0 : 0x1388;
            IEnumerator enumerator2 = base.GetComponent<Animation>().GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    AnimationState state2 = (AnimationState)enumerator2.Current;
                    if (state2 != null)
                        state2.speed = 1.05f;
                }
            }
            finally
            {
                IDisposable disposable2 = enumerator2 as IDisposable;
                if (disposable2 != null)
                {
                    disposable2.Dispose();
                }
            }
        }
        this.NapeArmorTotal = this.NapeArmor;
        this.state = "wait";
        Transform transform = base.transform;
        transform.position += (Vector3)(-Vector3.up * 10000f);
        //if (FengGameManagerMKII.LAN)
        //{
        //    base.GetComponent<PhotonView>().enabled = false;
        //}
        //else
        //{
        //    base.GetComponent<NetworkView>().enabled = false;
        //}
        this.door_broken = GameObject.Find("door_broke");
        this.door_closed = GameObject.Find("door_fine");
        this.door_broken.SetActive(false);
        this.door_closed.SetActive(true);
    }

    [PunRPC]
    private void startNeckSteam()
    {
        this.neckSteamObject.GetComponent<ParticleSystem>().Stop();
        this.neckSteamObject.GetComponent<ParticleSystem>().Play();
    }

    [PunRPC]
    private void startSweepSmoke()
    {
        this.sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = true;
        this.sweepSmokeObject.GetComponent<ParticleSystem>().Play();
    }

    private void steam()
    {
        this.callTitanHAHA();
        this.state = "steam";
        this.actionName = "attack_steam";
        this.attackCheckTime = 0.45f;
        this.crossFade(this.actionName, 0.1f);
        this.attackChkOnce = false;
    }

    [PunRPC]
    private void stopSweepSmoke()
    {
        this.sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = false;
        this.sweepSmokeObject.GetComponent<ParticleSystem>().Stop();
    }

    [PunRPC]
    public void titanGetHit(int viewID, int speed)
    {
        Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
        PhotonView view = PhotonView.Find(viewID);
        if (view != null)
        {
            Vector3 vector = view.gameObject.transform.position - transform.transform.position;
            if ((vector.magnitude < this.lagMax) && (this.healthTime <= 0f))
            {
                if (speed >= FengGameManagerMKII.Gamemode.DamageMode)
                {
                    this.NapeArmor -= speed;
                }
                if (this.maxHealth > 0f)
                {
                    base.photonView.RPC("labelRPC", PhotonTargets.AllBuffered, new object[] { this.NapeArmor, this.maxHealth });
                }
                this.neckSteam();
                if (this.NapeArmor <= 0)
                {
                    this.NapeArmor = 0;
                    if (!this.hasDie)
                    {
                        if (FengGameManagerMKII.LAN)
                        {
                            this.netDie();
                        }
                        else
                        {
                            base.photonView.RPC("netDie", PhotonTargets.OthersBuffered, new object[0]);
                            this.netDie();
                            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().titanGetKill(view.owner, speed, base.name);
                        }
                    }
                }
                else
                {
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(false, (string) view.owner.CustomProperties[PhotonPlayerProperty.name], true, "Colossal Titan's neck", speed);
                    object[] parameters = new object[] { speed };
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("netShowDamage", view.owner, parameters);
                }
                this.healthTime = 0.2f;
            }
        }
    }

    public void update2()
    {
        this.healthTime -= Time.deltaTime;
        this.updateLabel();
        if (this.state != "null")
        {
            if (this.state == "wait")
            {
                this.waitTime -= Time.deltaTime;
                if (this.waitTime <= 0f)
                {
                    base.transform.position = new Vector3(30f, 0f, 784f);
                    UnityEngine.Object.Instantiate(Resources.Load("FX/ThunderCT"), base.transform.position + ((Vector3) (Vector3.up * 350f)), Quaternion.Euler(270f, 0f, 0f));
                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().flashBlind();
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        this.idle();
                    }
                    else if (base.photonView.isMine)
                    {
                        this.idle();
                    }
                    else
                    {
                        this.state = "null";
                    }
                }
            }
            else if (this.state != "idle")
            {
                if (this.state == "attack_sweep")
                {
                    if ((this.attackCheckTimeA != 0f) && !(((base.GetComponent<Animation>()["attack_" + this.attackAnimation].normalizedTime < this.attackCheckTimeA) || (base.GetComponent<Animation>()["attack_" + this.attackAnimation].normalizedTime > this.attackCheckTimeB)) ? (this.attackChkOnce || (base.GetComponent<Animation>()["attack_" + this.attackAnimation].normalizedTime < this.attackCheckTimeA)) : false))
                    {
                        if (!this.attackChkOnce)
                        {
                            this.attackChkOnce = true;
                        }
                        foreach (RaycastHit hit in this.checkHitCapsule(this.checkHitCapsuleStart.position, this.checkHitCapsuleEnd.position, this.checkHitCapsuleR))
                        {
                            GameObject gameObject = hit.collider.gameObject;
                            if (gameObject.tag == "Player")
                            {
                                this.killPlayer(gameObject);
                            }
                            if ((((gameObject.tag == "erenHitbox") && (this.attackAnimation == "combo_3")) && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)) && (!FengGameManagerMKII.LAN ? PhotonNetwork.isMasterClient : Network.isServer))
                            {
                                gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(3);
                            }
                        }
                        foreach (RaycastHit hit2 in this.checkHitCapsule(this.checkHitCapsuleEndOld, this.checkHitCapsuleEnd.position, this.checkHitCapsuleR))
                        {
                            GameObject hitHero = hit2.collider.gameObject;
                            if (hitHero.tag == "Player")
                            {
                                this.killPlayer(hitHero);
                            }
                        }
                        this.checkHitCapsuleEndOld = this.checkHitCapsuleEnd.position;
                    }
                    if (base.GetComponent<Animation>()["attack_" + this.attackAnimation].normalizedTime >= 1f)
                    {
                        this.sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = false;
                        this.sweepSmokeObject.GetComponent<ParticleSystem>().Stop();
                        if (!((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER) || FengGameManagerMKII.LAN))
                        {
                            base.photonView.RPC("stopSweepSmoke", PhotonTargets.Others, new object[0]);
                        }
                        this.findNearestHero();
                        this.idle();
                        this.playAnimation("idle");
                    }
                }
                else if (this.state == "kick")
                {
                    if (!this.attackChkOnce && (base.GetComponent<Animation>()[this.actionName].normalizedTime >= this.attackCheckTime))
                    {
                        this.attackChkOnce = true;
                        this.door_broken.SetActive(true);
                        this.door_closed.SetActive(false);
                        if (!((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER) || FengGameManagerMKII.LAN))
                        {
                            base.photonView.RPC("changeDoor", PhotonTargets.OthersBuffered, new object[0]);
                        }
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                        {
                            if (FengGameManagerMKII.LAN)
                            {
                                Network.Instantiate(Resources.Load("FX/boom1_CT_KICK"), (Vector3) ((base.transform.position + (base.transform.forward * 120f)) + (base.transform.right * 30f)), Quaternion.Euler(270f, 0f, 0f), 0);
                                Network.Instantiate(Resources.Load("rock"), (Vector3) ((base.transform.position + (base.transform.forward * 120f)) + (base.transform.right * 30f)), Quaternion.Euler(0f, 0f, 0f), 0);
                            }
                            else
                            {
                                PhotonNetwork.Instantiate("FX/boom1_CT_KICK", (Vector3) ((base.transform.position + (base.transform.forward * 120f)) + (base.transform.right * 30f)), Quaternion.Euler(270f, 0f, 0f), 0);
                                PhotonNetwork.Instantiate("rock", (Vector3) ((base.transform.position + (base.transform.forward * 120f)) + (base.transform.right * 30f)), Quaternion.Euler(0f, 0f, 0f), 0);
                            }
                        }
                        else
                        {
                            UnityEngine.Object.Instantiate(Resources.Load("FX/boom1_CT_KICK"), (Vector3) ((base.transform.position + (base.transform.forward * 120f)) + (base.transform.right * 30f)), Quaternion.Euler(270f, 0f, 0f));
                            UnityEngine.Object.Instantiate(Resources.Load("rock"), (Vector3) ((base.transform.position + (base.transform.forward * 120f)) + (base.transform.right * 30f)), Quaternion.Euler(0f, 0f, 0f));
                        }
                    }
                    if (base.GetComponent<Animation>()[this.actionName].normalizedTime >= 1f)
                    {
                        this.findNearestHero();
                        this.idle();
                        this.playAnimation("idle");
                    }
                }
                else if (this.state == "slap")
                {
                    if (!this.attackChkOnce && (base.GetComponent<Animation>()["attack_slap_" + this.attackAnimation].normalizedTime >= this.attackCheckTime))
                    {
                        GameObject obj4;
                        this.attackChkOnce = true;
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                        {
                            if (FengGameManagerMKII.LAN)
                            {
                                obj4 = (GameObject) Network.Instantiate(Resources.Load("FX/boom1"), this.checkHitCapsuleStart.position, Quaternion.Euler(270f, 0f, 0f), 0);
                            }
                            else
                            {
                                obj4 = PhotonNetwork.Instantiate("FX/boom1", this.checkHitCapsuleStart.position, Quaternion.Euler(270f, 0f, 0f), 0);
                            }
                            if (obj4.GetComponent<EnemyfxIDcontainer>() != null)
                            {
                                obj4.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
                            }
                        }
                        else
                        {
                            obj4 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("FX/boom1"), this.checkHitCapsuleStart.position, Quaternion.Euler(270f, 0f, 0f));
                        }
                        obj4.transform.localScale = new Vector3(5f, 5f, 5f);
                    }
                    if (base.GetComponent<Animation>()["attack_slap_" + this.attackAnimation].normalizedTime >= 1f)
                    {
                        this.findNearestHero();
                        this.idle();
                        this.playAnimation("idle");
                    }
                }
                else if (this.state == "steam")
                {
                    if (!this.attackChkOnce && (base.GetComponent<Animation>()[this.actionName].normalizedTime >= this.attackCheckTime))
                    {
                        this.attackChkOnce = true;
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                        {
                            if (FengGameManagerMKII.LAN)
                            {
                                Network.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + ((Vector3) (base.transform.up * 185f)), Quaternion.Euler(270f, 0f, 0f), 0);
                                Network.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + ((Vector3) (base.transform.up * 303f)), Quaternion.Euler(270f, 0f, 0f), 0);
                                Network.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + ((Vector3) (base.transform.up * 50f)), Quaternion.Euler(270f, 0f, 0f), 0);
                            }
                            else
                            {
                                PhotonNetwork.Instantiate("FX/colossal_steam", base.transform.position + ((Vector3) (base.transform.up * 185f)), Quaternion.Euler(270f, 0f, 0f), 0);
                                PhotonNetwork.Instantiate("FX/colossal_steam", base.transform.position + ((Vector3) (base.transform.up * 303f)), Quaternion.Euler(270f, 0f, 0f), 0);
                                PhotonNetwork.Instantiate("FX/colossal_steam", base.transform.position + ((Vector3) (base.transform.up * 50f)), Quaternion.Euler(270f, 0f, 0f), 0);
                            }
                        }
                        else
                        {
                            UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + ((Vector3) (base.transform.forward * 185f)), Quaternion.Euler(270f, 0f, 0f));
                            UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + ((Vector3) (base.transform.forward * 303f)), Quaternion.Euler(270f, 0f, 0f));
                            UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + ((Vector3) (base.transform.forward * 50f)), Quaternion.Euler(270f, 0f, 0f));
                        }
                    }
                    if (base.GetComponent<Animation>()[this.actionName].normalizedTime >= 1f)
                    {
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                        {
                            if (FengGameManagerMKII.LAN)
                            {
                                Network.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + ((Vector3) (base.transform.up * 185f)), Quaternion.Euler(270f, 0f, 0f), 0);
                                Network.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + ((Vector3) (base.transform.up * 303f)), Quaternion.Euler(270f, 0f, 0f), 0);
                                Network.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + ((Vector3) (base.transform.up * 50f)), Quaternion.Euler(270f, 0f, 0f), 0);
                            }
                            else
                            {
                                GameObject obj5 = PhotonNetwork.Instantiate("FX/colossal_steam_dmg", base.transform.position + ((Vector3) (base.transform.up * 185f)), Quaternion.Euler(270f, 0f, 0f), 0);
                                if (obj5.GetComponent<EnemyfxIDcontainer>() != null)
                                {
                                    obj5.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
                                }
                                obj5 = PhotonNetwork.Instantiate("FX/colossal_steam_dmg", base.transform.position + ((Vector3) (base.transform.up * 303f)), Quaternion.Euler(270f, 0f, 0f), 0);
                                if (obj5.GetComponent<EnemyfxIDcontainer>() != null)
                                {
                                    obj5.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
                                }
                                obj5 = PhotonNetwork.Instantiate("FX/colossal_steam_dmg", base.transform.position + ((Vector3) (base.transform.up * 50f)), Quaternion.Euler(270f, 0f, 0f), 0);
                                if (obj5.GetComponent<EnemyfxIDcontainer>() != null)
                                {
                                    obj5.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
                                }
                            }
                        }
                        else
                        {
                            UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + ((Vector3) (base.transform.forward * 185f)), Quaternion.Euler(270f, 0f, 0f));
                            UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + ((Vector3) (base.transform.forward * 303f)), Quaternion.Euler(270f, 0f, 0f));
                            UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + ((Vector3) (base.transform.forward * 50f)), Quaternion.Euler(270f, 0f, 0f));
                        }
                        if (this.hasDie)
                        {
                            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                            {
                                UnityEngine.Object.Destroy(base.gameObject);
                            }
                            else if (FengGameManagerMKII.LAN)
                            {
                                if (base.photonView.isMine)
                                {
                                }
                            }
                            else if (PhotonNetwork.isMasterClient)
                            {
                                PhotonNetwork.Destroy(base.photonView);
                            }
                            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameWin2();
                        }
                        this.findNearestHero();
                        this.idle();
                        this.playAnimation("idle");
                    }
                }
                else if (this.state == string.Empty)
                {
                }
            }
            else if (this.attackPattern == -1)
            {
                this.slap("r1");
                this.attackPattern++;
            }
            else if (this.attackPattern == 0)
            {
                this.attack_sweep(string.Empty);
                this.attackPattern++;
            }
            else if (this.attackPattern == 1)
            {
                this.steam();
                this.attackPattern++;
            }
            else if (this.attackPattern == 2)
            {
                this.kick();
                this.attackPattern++;
            }
            else if (this.isSteamNeed || this.hasDie)
            {
                this.steam();
                this.isSteamNeed = false;
            }
            else if (this.myHero == null)
            {
                this.findNearestHero();
            }
            else
            {
                Vector3 vector = this.myHero.transform.position - base.transform.position;
                float current = -Mathf.Atan2(vector.z, vector.x) * 57.29578f;
                float f = -Mathf.DeltaAngle(current, base.gameObject.transform.rotation.eulerAngles.y - 90f);
                this.myDistance = Mathf.Sqrt(((this.myHero.transform.position.x - base.transform.position.x) * (this.myHero.transform.position.x - base.transform.position.x)) + ((this.myHero.transform.position.z - base.transform.position.z) * (this.myHero.transform.position.z - base.transform.position.z)));
                float num4 = this.myHero.transform.position.y - base.transform.position.y;
                if ((this.myDistance < 85f) && (UnityEngine.Random.Range(0, 100) < 5))
                {
                    this.steam();
                }
                else
                {
                    if ((num4 > 310f) && (num4 < 350f))
                    {
                        if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("APL1").position) < 40f)
                        {
                            this.slap("l1");
                            return;
                        }
                        if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("APL2").position) < 40f)
                        {
                            this.slap("l2");
                            return;
                        }
                        if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("APR1").position) < 40f)
                        {
                            this.slap("r1");
                            return;
                        }
                        if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("APR2").position) < 40f)
                        {
                            this.slap("r2");
                            return;
                        }
                        if ((this.myDistance < 150f) && (Mathf.Abs(f) < 80f))
                        {
                            this.attack_sweep(string.Empty);
                            return;
                        }
                    }
                    if (((num4 < 300f) && (Mathf.Abs(f) < 80f)) && (this.myDistance < 85f))
                    {
                        this.attack_sweep("_vertical");
                    }
                    else
                    {
                        switch (UnityEngine.Random.Range(0, 7))
                        {
                            case 0:
                                this.slap("l1");
                                break;

                            case 1:
                                this.slap("l2");
                                break;

                            case 2:
                                this.slap("r1");
                                break;

                            case 3:
                                this.slap("r2");
                                break;

                            case 4:
                                this.attack_sweep(string.Empty);
                                break;

                            case 5:
                                this.attack_sweep("_vertical");
                                break;

                            case 6:
                                this.steam();
                                break;
                        }
                    }
                }
            }
        }
    }

    public void updateLabel()
    {
        if ((this.healthLabel != null)) // && this.healthLabel.GetComponent<UILabel>().isVisible)
        {
            this.healthLabel.transform.LookAt(((Vector3) (2f * this.healthLabel.transform.position)) - Camera.main.transform.position);
        }
    }
}