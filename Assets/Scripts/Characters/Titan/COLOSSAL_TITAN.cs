using Assets.Scripts.Gamemode;
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

    private GamemodeBase Gamemode;

    private void attack_sweep(string type = "")
    {
        callTitanHAHA();
        state = "attack_sweep";
        attackAnimation = "sweep" + type;
        attackCheckTimeA = 0.4f;
        attackCheckTimeB = 0.57f;
        checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R");
        checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
        checkHitCapsuleR = 20f;
        crossFade("attack_" + attackAnimation, 0.1f);
        attackChkOnce = false;
        sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = true;
        sweepSmokeObject.GetComponent<ParticleSystem>().Play();
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
            if (FengGameManagerMKII.LAN)
            {
                if (Network.peerType != NetworkPeerType.Server)
                {
                }
            }
            else if (PhotonNetwork.isMasterClient)
                photonView.RPC(nameof(startSweepSmoke), PhotonTargets.Others);
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
        var vector = -(neck.position + base.transform.forward * 50f - player.transform.position);
        var num = 20f;
        var hero = player.GetComponent<Hero>();
        var force = vector.normalized * num + Vector3.up * 1f;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            hero.blowAway(force);
        else if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
            hero.photonView.RPC(nameof(hero.blowAway), PhotonTargets.All, force);
    }

    private void callTitanHAHA()
    {
        attackCount++;
    }

    [PunRPC]
    private void changeDoor()
    {
        door_broken.SetActive(true);
        door_closed.SetActive(false);
    }

    private RaycastHit[] checkHitCapsule(Vector3 start, Vector3 end, float r)
    {
        return Physics.SphereCastAll(start, r, end - start, Vector3.Distance(start, end));
    }

    private GameObject checkIfHitHand(Transform hand)
    {
        foreach (var collider in Physics.OverlapSphere(hand.GetComponent<SphereCollider>().transform.position, 31f))
        {
            if (collider.transform.root.tag == "Player")
            {
                var gameObject = collider.transform.root.gameObject;
                if (gameObject.GetComponent<TITAN_EREN>() != null)
                {
                    if (!gameObject.GetComponent<TITAN_EREN>().isHit)
                    {
                        gameObject.GetComponent<TITAN_EREN>().hitByTitan();
                    }
                    return gameObject;
                }
                if ((gameObject.GetComponent<Hero>() != null) && !gameObject.GetComponent<Hero>().IsInvincible())
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
            photonView.RPC(nameof(netCrossFade), PhotonTargets.Others, aniName, time);
    }

    private void findNearestHero()
    {
        myHero = getNearestHero();
    }

    private GameObject getNearestHero()
    {
        var objArray = GameObject.FindGameObjectsWithTag("Player");
        GameObject obj2 = null;
        var positiveInfinity = float.PositiveInfinity;
        foreach (var obj3 in objArray)
        {
            if (((obj3.GetComponent<Hero>() == null) || !obj3.GetComponent<Hero>().HasDied()) && ((obj3.GetComponent<TITAN_EREN>() == null) || !obj3.GetComponent<TITAN_EREN>().hasDied))
            {
                var num3 = Mathf.Sqrt(((obj3.transform.position.x - base.transform.position.x) * (obj3.transform.position.x - base.transform.position.x)) + ((obj3.transform.position.z - base.transform.position.z) * (obj3.transform.position.z - base.transform.position.z)));
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
        state = "idle";
        crossFade("idle", 0.2f);
    }

    private void kick()
    {
        state = "kick";
        actionName = "attack_kick_wall";
        attackCheckTime = 0.64f;
        attackChkOnce = false;
        crossFade(actionName, 0.1f);
    }

    private void killPlayer(GameObject hitHero)
    {
        if (hitHero != null)
        {
            var hero = hitHero.GetComponent<Hero>();
            var position = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                if (!hero.HasDied())
                    hero.Die((hitHero.transform.position - position) * 15f * 4f, false);
            }
            else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            {
                if (FengGameManagerMKII.LAN)
                {
                    if (!hero.HasDied())
                        hero.MarkDie();
                }
                else if (!hero.HasDied())
                {
                    hero.MarkDie();
                    hero.photonView.RPC(
                        nameof(hero.netDie),
                        PhotonTargets.All,
                        (hitHero.transform.position - position) * 15f * 4f,
                        false,
                        -1,
                        "Colossal Titan",
                        true);
                }
            }
        }
    }

    [PunRPC]
    public void labelRPC(int health, int maxHealth)
    {
        if (health < 0)
        {
            if (healthLabel != null)
            {
                UnityEngine.Object.Destroy(healthLabel);
            }
        }
        else
        {
            if (healthLabel == null)
            {
                healthLabel = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
                healthLabel.name = "LabelNameOverHead";
                healthLabel.transform.parent = base.transform;
                healthLabel.transform.localPosition = new Vector3(0f, 430f, 0f);
                var a = 15f;
                if ((size > 0f) && (size < 1f))
                {
                    a = 15f / size;
                    a = Mathf.Min(a, 100f);
                }
                healthLabel.transform.localScale = new Vector3(a, a, a);
            }
            var color = "7FFF00";
            var num2 = ((float) health) / ((float) maxHealth);
            if ((num2 < 0.75f) && (num2 >= 0.5f))
            {
                color = "f2b50f";
            }
            else if ((num2 < 0.5f) && (num2 >= 0.25f))
            {
                color = "ff8100";
            }
            else if (num2 < 0.25f)
            {
                color = "ff3333";
            }
            healthLabel.GetComponent<TextMesh>().text = $"<color=#{color}>{health}</color>";
        }
    }

    public void loadskin()
    {
        if (PhotonNetwork.isMasterClient && (((int) FengGameManagerMKII.settings[1]) == 1))
            photonView.RPC(nameof(loadskinRPC), PhotonTargets.AllBuffered, (string) FengGameManagerMKII.settings[0x43]);
    }

    public IEnumerator loadskinE(string url)
    {
        while (!hasspawn)
        {
            yield return null;
        }
        var mipmap = true;
        var iteratorVariable1 = false;
        if (((int)FengGameManagerMKII.settings[0x3f]) == 1)
        {
            mipmap = false;
        }
        foreach (var iteratorVariable2 in GetComponentsInChildren<Renderer>())
        {
            if (iteratorVariable2.name.Contains("hair"))
            {
                if (!FengGameManagerMKII.linkHash[2].ContainsKey(url))
                {
                    var link = new WWW(url);
                    yield return link;
                    var iteratorVariable4 = RCextensions.loadimage(link, mipmap, 0xf4240);
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
            base.StartCoroutine(loadskinE(url));
        }
    }

    private void neckSteam()
    {
        neckSteamObject.GetComponent<ParticleSystem>().Stop();
        neckSteamObject.GetComponent<ParticleSystem>().Play();
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
            if (FengGameManagerMKII.LAN)
            {
                if (Network.peerType != NetworkPeerType.Server)
                {
                }
            }
            else if (PhotonNetwork.isMasterClient)
                photonView.RPC(nameof(startNeckSteam), PhotonTargets.Others);
        }
        isSteamNeed = true;
        var neck = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
        var radius = 30f;
        foreach (var collider in Physics.OverlapSphere(neck.transform.position - ((Vector3) (base.transform.forward * 10f)), radius))
        {
            if (collider.transform.root.tag == "Player")
            {
                var gameObject = collider.transform.root.gameObject;
                if ((gameObject.GetComponent<TITAN_EREN>() == null) && (gameObject.GetComponent<Hero>() != null))
                {
                    blowPlayer(gameObject, neck);
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
        if (!hasDie)
        {
            hasDie = true;
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
        GetComponent<Animation>().Play(aniName);
        if ((!FengGameManagerMKII.LAN && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)) && PhotonNetwork.isMasterClient)
            photonView.RPC(nameof(netPlayAnimation), PhotonTargets.Others, aniName);
    }

    private void playAnimationAt(string aniName, float normalizedTime)
    {
        GetComponent<Animation>().Play(aniName);
        GetComponent<Animation>()[aniName].normalizedTime = normalizedTime;
        if ((!FengGameManagerMKII.LAN && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)) && PhotonNetwork.isMasterClient)
            photonView.RPC(nameof(netPlayAnimationAt), PhotonTargets.Others, aniName, normalizedTime);
    }

    private void playSound(string sndname)
    {
        playsoundRPC(sndname);
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
            if (FengGameManagerMKII.LAN)
            {
                if (Network.peerType != NetworkPeerType.Server)
                {
                }
            }
            else if (PhotonNetwork.isMasterClient)
                photonView.RPC(nameof(playsoundRPC), PhotonTargets.Others, sndname);
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
            var transform = base.transform;
            transform.localScale = (Vector3) (transform.localScale * (size * 0.05f));
            this.size = size;
        }
    }

    private void slap(string type)
    {
        callTitanHAHA();
        state = "slap";
        attackAnimation = type;
        if ((type == "r1") || (type == "r2"))
        {
            checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
        }
        if ((type == "l1") || (type == "l2"))
        {
            checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
        }
        attackCheckTime = 0.57f;
        attackChkOnce = false;
        crossFade("attack_slap_" + attackAnimation, 0.1f);
    }

    private void Start()
    {
        Gamemode = FengGameManagerMKII.Gamemode;
        startMain();
        size = 20f;
        if (base.photonView.isMine)
        {
            if (FengGameManagerMKII.Gamemode.Settings.TitanCustomSize)
            {
                var sizeLower = FengGameManagerMKII.Gamemode.Settings.TitanMinimumSize;
                var sizeUpper = FengGameManagerMKII.Gamemode.Settings.TitanMaximumSize;
                size = UnityEngine.Random.Range(sizeLower, sizeUpper);
                photonView.RPC(nameof(setSize), PhotonTargets.AllBuffered, size);
            }
            lagMax = 150f + (size * 3f);
            healthTime = 0f;
            maxHealth = NapeArmor;
            if (FengGameManagerMKII.Gamemode.Settings.TitanHealthMode != TitanHealthMode.Disabled)
                maxHealth = NapeArmor = UnityEngine.Random.Range(FengGameManagerMKII.Gamemode.Settings.TitanHealthMinimum, FengGameManagerMKII.Gamemode.Settings.TitanHealthMaximum);

            if (NapeArmor > 0)
                photonView.RPC(nameof(labelRPC), PhotonTargets.AllBuffered, NapeArmor, maxHealth);

            loadskin();
        }
        hasspawn = true;
    }

    private void startMain()
    {
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().addCT(this);
        if (myHero == null)
        {
            findNearestHero();
        }
        base.name = "COLOSSAL_TITAN";
        NapeArmor = 0x3e8;
        var flag = FengGameManagerMKII.Gamemode.Settings.RespawnMode == RespawnMode.NEVER;
        if (IN_GAME_MAIN_CAMERA.difficulty == 0)
        {
            NapeArmor = !flag ? 0x1388 : 0x7d0;
        }
        else if (IN_GAME_MAIN_CAMERA.difficulty == 1)
        {
            NapeArmor = !flag ? 0x1f40 : 0xdac;
            var enumerator = base.GetComponent<Animation>().GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    var current = (AnimationState)enumerator.Current;
                    if (current != null)
                        current.speed = 1.02f;
                }
            }
            finally
            {
                var disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
        else if (IN_GAME_MAIN_CAMERA.difficulty == 2)
        {
            NapeArmor = !flag ? 0x2ee0 : 0x1388;
            var enumerator2 = base.GetComponent<Animation>().GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    var state2 = (AnimationState)enumerator2.Current;
                    if (state2 != null)
                        state2.speed = 1.05f;
                }
            }
            finally
            {
                var disposable2 = enumerator2 as IDisposable;
                if (disposable2 != null)
                {
                    disposable2.Dispose();
                }
            }
        }
        NapeArmorTotal = NapeArmor;
        state = "wait";
        var transform = base.transform;
        transform.position += (Vector3)(-Vector3.up * 10000f);
        //if (FengGameManagerMKII.LAN)
        //{
        //    base.GetComponent<PhotonView>().enabled = false;
        //}
        //else
        //{
        //    base.GetComponent<NetworkView>().enabled = false;
        //}
        door_broken = GameObject.Find("door_broke");
        door_closed = GameObject.Find("door_fine");
        door_broken.SetActive(false);
        door_closed.SetActive(true);
    }

    [PunRPC]
    private void startNeckSteam()
    {
        neckSteamObject.GetComponent<ParticleSystem>().Stop();
        neckSteamObject.GetComponent<ParticleSystem>().Play();
    }

    [PunRPC]
    private void startSweepSmoke()
    {
        sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = true;
        sweepSmokeObject.GetComponent<ParticleSystem>().Play();
    }

    private void steam()
    {
        callTitanHAHA();
        state = "steam";
        actionName = "attack_steam";
        attackCheckTime = 0.45f;
        crossFade(actionName, 0.1f);
        attackChkOnce = false;
    }

    [PunRPC]
    private void stopSweepSmoke()
    {
        sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = false;
        sweepSmokeObject.GetComponent<ParticleSystem>().Stop();
    }

    [PunRPC]
    public void titanGetHit(int viewID, int speed)
    {
        var transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
        var view = PhotonView.Find(viewID);
        if (view != null)
        {
            var vector = view.gameObject.transform.position - transform.transform.position;
            if ((vector.magnitude < lagMax) && (healthTime <= 0f))
            {
                if (speed >= FengGameManagerMKII.Gamemode.Settings.DamageMode)
                    NapeArmor -= speed;

                if (maxHealth > 0f)
                    photonView.RPC(nameof(labelRPC), PhotonTargets.AllBuffered, NapeArmor, maxHealth);

                neckSteam();
                if (NapeArmor <= 0)
                {
                    NapeArmor = 0;
                    if (!hasDie)
                    {
                        if (FengGameManagerMKII.LAN)
                        {
                            netDie();
                        }
                        else
                        {
                            photonView.RPC(nameof(netDie), PhotonTargets.OthersBuffered);
                            netDie();
                            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().titanGetKill(view.owner, speed, name);
                        }
                    }
                }
                else
                {
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(false, (string) view.owner.CustomProperties[PhotonPlayerProperty.name], true, "Colossal Titan's neck", speed);
                    var parameters = new object[] { speed };
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("netShowDamage", view.owner, parameters);
                }
                healthTime = 0.2f;
            }
        }
    }

    public void update2()
    {
        healthTime -= Time.deltaTime;
        updateLabel();
        if (state != "null")
        {
            if (state == "wait")
            {
                waitTime -= Time.deltaTime;
                if (waitTime <= 0f)
                {
                    base.transform.position = new Vector3(30f, 0f, 784f);
                    UnityEngine.Object.Instantiate(Resources.Load("FX/ThunderCT"), base.transform.position + ((Vector3) (Vector3.up * 350f)), Quaternion.Euler(270f, 0f, 0f));
                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().flashBlind();
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        idle();
                    }
                    else if (base.photonView.isMine)
                    {
                        idle();
                    }
                    else
                    {
                        state = "null";
                    }
                }
            }
            else if (state != "idle")
            {
                if (state == "attack_sweep")
                {
                    if ((attackCheckTimeA != 0f) && !(((base.GetComponent<Animation>()["attack_" + attackAnimation].normalizedTime < attackCheckTimeA) || (base.GetComponent<Animation>()["attack_" + attackAnimation].normalizedTime > attackCheckTimeB)) ? (attackChkOnce || (base.GetComponent<Animation>()["attack_" + attackAnimation].normalizedTime < attackCheckTimeA)) : false))
                    {
                        if (!attackChkOnce)
                        {
                            attackChkOnce = true;
                        }
                        foreach (var hit in checkHitCapsule(checkHitCapsuleStart.position, checkHitCapsuleEnd.position, checkHitCapsuleR))
                        {
                            var gameObject = hit.collider.gameObject;
                            if (gameObject.tag == "Player")
                            {
                                killPlayer(gameObject);
                            }
                            if ((((gameObject.tag == "erenHitbox") && (attackAnimation == "combo_3")) && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)) && (!FengGameManagerMKII.LAN ? PhotonNetwork.isMasterClient : Network.isServer))
                            {
                                gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(3);
                            }
                        }
                        foreach (var hit2 in checkHitCapsule(checkHitCapsuleEndOld, checkHitCapsuleEnd.position, checkHitCapsuleR))
                        {
                            var hitHero = hit2.collider.gameObject;
                            if (hitHero.tag == "Player")
                            {
                                killPlayer(hitHero);
                            }
                        }
                        checkHitCapsuleEndOld = checkHitCapsuleEnd.position;
                    }
                    if (base.GetComponent<Animation>()["attack_" + attackAnimation].normalizedTime >= 1f)
                    {
                        sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = false;
                        sweepSmokeObject.GetComponent<ParticleSystem>().Stop();

                        if (!((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER) || FengGameManagerMKII.LAN))
                            photonView.RPC(nameof(stopSweepSmoke), PhotonTargets.Others);

                        findNearestHero();
                        idle();
                        playAnimation("idle");
                    }
                }
                else if (state == "kick")
                {
                    if (!attackChkOnce && (base.GetComponent<Animation>()[actionName].normalizedTime >= attackCheckTime))
                    {
                        attackChkOnce = true;
                        door_broken.SetActive(true);
                        door_closed.SetActive(false);
                        if (!((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER) || FengGameManagerMKII.LAN))
                            photonView.RPC(nameof(changeDoor), PhotonTargets.OthersBuffered);

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
                    if (base.GetComponent<Animation>()[actionName].normalizedTime >= 1f)
                    {
                        findNearestHero();
                        idle();
                        playAnimation("idle");
                    }
                }
                else if (state == "slap")
                {
                    if (!attackChkOnce && (base.GetComponent<Animation>()["attack_slap_" + attackAnimation].normalizedTime >= attackCheckTime))
                    {
                        GameObject obj4;
                        attackChkOnce = true;
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                        {
                            if (FengGameManagerMKII.LAN)
                            {
                                obj4 = (GameObject) Network.Instantiate(Resources.Load("FX/boom1"), checkHitCapsuleStart.position, Quaternion.Euler(270f, 0f, 0f), 0);
                            }
                            else
                            {
                                obj4 = PhotonNetwork.Instantiate("FX/boom1", checkHitCapsuleStart.position, Quaternion.Euler(270f, 0f, 0f), 0);
                            }
                            if (obj4.GetComponent<EnemyfxIDcontainer>() != null)
                            {
                                obj4.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
                            }
                        }
                        else
                        {
                            obj4 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("FX/boom1"), checkHitCapsuleStart.position, Quaternion.Euler(270f, 0f, 0f));
                        }
                        obj4.transform.localScale = new Vector3(5f, 5f, 5f);
                    }
                    if (base.GetComponent<Animation>()["attack_slap_" + attackAnimation].normalizedTime >= 1f)
                    {
                        findNearestHero();
                        idle();
                        playAnimation("idle");
                    }
                }
                else if (state == "steam")
                {
                    if (!attackChkOnce && (base.GetComponent<Animation>()[actionName].normalizedTime >= attackCheckTime))
                    {
                        attackChkOnce = true;
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
                    if (base.GetComponent<Animation>()[actionName].normalizedTime >= 1f)
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
                                var obj5 = PhotonNetwork.Instantiate("FX/colossal_steam_dmg", base.transform.position + ((Vector3) (base.transform.up * 185f)), Quaternion.Euler(270f, 0f, 0f), 0);
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
                        if (hasDie)
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
                        findNearestHero();
                        idle();
                        playAnimation("idle");
                    }
                }
                else if (state == string.Empty)
                {
                }
            }
            else if (attackPattern == -1)
            {
                slap("r1");
                attackPattern++;
            }
            else if (attackPattern == 0)
            {
                attack_sweep(string.Empty);
                attackPattern++;
            }
            else if (attackPattern == 1)
            {
                steam();
                attackPattern++;
            }
            else if (attackPattern == 2)
            {
                kick();
                attackPattern++;
            }
            else if (isSteamNeed || hasDie)
            {
                steam();
                isSteamNeed = false;
            }
            else if (myHero == null)
            {
                findNearestHero();
            }
            else
            {
                var vector = myHero.transform.position - base.transform.position;
                var current = -Mathf.Atan2(vector.z, vector.x) * 57.29578f;
                var f = -Mathf.DeltaAngle(current, base.gameObject.transform.rotation.eulerAngles.y - 90f);
                myDistance = Mathf.Sqrt(((myHero.transform.position.x - base.transform.position.x) * (myHero.transform.position.x - base.transform.position.x)) + ((myHero.transform.position.z - base.transform.position.z) * (myHero.transform.position.z - base.transform.position.z)));
                var num4 = myHero.transform.position.y - base.transform.position.y;
                if ((myDistance < 85f) && (UnityEngine.Random.Range(0, 100) < 5))
                {
                    steam();
                }
                else
                {
                    if ((num4 > 310f) && (num4 < 350f))
                    {
                        if (Vector3.Distance(myHero.transform.position, base.transform.Find("APL1").position) < 40f)
                        {
                            slap("l1");
                            return;
                        }
                        if (Vector3.Distance(myHero.transform.position, base.transform.Find("APL2").position) < 40f)
                        {
                            slap("l2");
                            return;
                        }
                        if (Vector3.Distance(myHero.transform.position, base.transform.Find("APR1").position) < 40f)
                        {
                            slap("r1");
                            return;
                        }
                        if (Vector3.Distance(myHero.transform.position, base.transform.Find("APR2").position) < 40f)
                        {
                            slap("r2");
                            return;
                        }
                        if ((myDistance < 150f) && (Mathf.Abs(f) < 80f))
                        {
                            attack_sweep(string.Empty);
                            return;
                        }
                    }
                    if (((num4 < 300f) && (Mathf.Abs(f) < 80f)) && (myDistance < 85f))
                    {
                        attack_sweep("_vertical");
                    }
                    else
                    {
                        switch (UnityEngine.Random.Range(0, 7))
                        {
                            case 0:
                                slap("l1");
                                break;

                            case 1:
                                slap("l2");
                                break;

                            case 2:
                                slap("r1");
                                break;

                            case 3:
                                slap("r2");
                                break;

                            case 4:
                                attack_sweep(string.Empty);
                                break;

                            case 5:
                                attack_sweep("_vertical");
                                break;

                            case 6:
                                steam();
                                break;
                        }
                    }
                }
            }
        }
    }

    public void updateLabel()
    {
        if ((healthLabel != null)) // && this.healthLabel.GetComponent<UILabel>().isVisible)
        {
            healthLabel.transform.LookAt(((Vector3) (2f * healthLabel.transform.position)) - Camera.main.transform.position);
        }
    }
}