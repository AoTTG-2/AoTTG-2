using Assets.Scripts.Gamemode;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings;
using System.Collections;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Titan.Configuration;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan
{
    /// <summary>
    /// The colossal titan. This class needs to be refactored.
    /// </summary>
    public class ColossalTitan : TitanBase
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
        public int NapeArmorTotal = 0x2710;
        public GameObject neckSteamObject;
        public float size;
        private string state = "idle";
        public GameObject sweepSmokeObject;
        public float tauntTime;
        private float waitTime = 2f;
        private FengGameManagerMKII manager;
        private GamemodeBase Gamemode;

        public override void Initialize(TitanConfiguration configuration)
        {
            EntityService.Register(this);
        }

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
            if (PhotonNetwork.isMasterClient)
            {
                base.photonView.RPC(nameof(startSweepSmoke), PhotonTargets.Others, new object[0]);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            base.GetComponent<Rigidbody>().freezeRotation = true;
            base.GetComponent<Rigidbody>().useGravity = false;
            base.GetComponent<Rigidbody>().isKinematic = true;
        }

        public void beTauntedBy(GameObject target, float tauntTime)
        {
        }

        public void blowPlayer(GameObject player, Transform neck)
        {
            Vector3 vector = -(Vector3) ((neck.position + (base.transform.forward * 50f)) - player.transform.position);
            float num = 20f;
            if (PhotonNetwork.isMasterClient)
            {
                object[] parameters = new object[] { (Vector3) ((vector.normalized * num) + (Vector3.up * 1f)) };
                player.GetComponent<Hero>().photonView.RPC(nameof(Hero.BlowAway), PhotonTargets.All, parameters);
            }
        }

        private void callTitanHAHA()
        {
            this.attackCount++;
        }

        [PunRPC]
        private void changeDoor()
        {
            if (door_broken == null)
            {
                door_broken = GameObject.Find("door_broke");
            }
            this.door_broken.SetActive(true);
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
                    if (gameObject.GetComponent<ErenTitan>() != null)
                    {
                        if (!gameObject.GetComponent<ErenTitan>().isHit)
                        {
                            gameObject.GetComponent<ErenTitan>().hitByTitan();
                        }
                        return gameObject;
                    }
                    if ((gameObject.GetComponent<Hero>() != null) && !gameObject.GetComponent<Hero>().IsInvincible)
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
            if (PhotonNetwork.isMasterClient)
            {
                object[] parameters = new object[] { aniName, time };
                base.photonView.RPC(nameof(netCrossFade), PhotonTargets.Others, parameters);
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
                if (((obj3.GetComponent<Hero>() == null) || !obj3.GetComponent<Hero>().HasDied()) && ((obj3.GetComponent<ErenTitan>() == null) || !obj3.GetComponent<ErenTitan>().hasDied))
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
                if (!hitHero.GetComponent<Hero>().HasDied())
                {
                    hitHero.GetComponent<Hero>().MarkDie();
                    object[] parameters = new object[] { (Vector3) (((hitHero.transform.position - position) * 15f) * 4f), false, -1, "Colossal Titan", true };
                    hitHero.GetComponent<Hero>().photonView.RPC(nameof(Hero.NetDie), PhotonTargets.All, parameters);
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
                string color = "7FFF00";
                float num2 = ((float) health) / ((float) maxHealth);
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
            {
                base.photonView.RPC(nameof(loadskinRPC), PhotonTargets.AllBuffered, new object[] { (string) FengGameManagerMKII.settings[0x43] });
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
            if (((int) FengGameManagerMKII.settings[0x3f]) == 1)
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
                            iteratorVariable2.material = (Material) FengGameManagerMKII.linkHash[2][url];
                        }
                        else
                        {
                            iteratorVariable2.material = (Material) FengGameManagerMKII.linkHash[2][url];
                        }
                    }
                    else
                    {
                        iteratorVariable2.material = (Material) FengGameManagerMKII.linkHash[2][url];
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
            if (PhotonNetwork.isMasterClient)
            {
                base.photonView.RPC(nameof(startNeckSteam), PhotonTargets.Others, new object[0]);
            }
            this.isSteamNeed = true;
            Transform neck = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
            float radius = 30f;
            foreach (Collider collider in Physics.OverlapSphere(neck.transform.position - ((Vector3) (base.transform.forward * 10f)), radius))
            {
                if (collider.transform.root.tag == "Player")
                {
                    GameObject gameObject = collider.transform.root.gameObject;
                    if ((gameObject.GetComponent<ErenTitan>() == null) && (gameObject.GetComponent<Hero>() != null))
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

        private void playAnimation(string aniName)
        {
            base.GetComponent<Animation>().Play(aniName);
            if (PhotonNetwork.isMasterClient)
            {
                object[] parameters = new object[] { aniName };
                base.photonView.RPC(nameof(netPlayAnimation), PhotonTargets.Others, parameters);
            }
        }

        private void playAnimationAt(string aniName, float normalizedTime)
        {
            base.GetComponent<Animation>().Play(aniName);
            base.GetComponent<Animation>()[aniName].normalizedTime = normalizedTime;
            if (PhotonNetwork.isMasterClient)
            {
                object[] parameters = new object[] { aniName, normalizedTime };
                base.photonView.RPC(nameof(netPlayAnimationAt), PhotonTargets.Others, parameters);
            }
        }

        private void playSound(string sndname)
        {
            this.playsoundRPC(sndname);
            if (PhotonNetwork.isMasterClient)
            {
                object[] parameters = new object[] { sndname };
                base.photonView.RPC(nameof(playsoundRPC), PhotonTargets.Others, parameters);
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
            manager = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>();
            Gamemode = FengGameManagerMKII.Gamemode;
            this.startMain();
            this.size = 20f;
            if (base.photonView.isMine)
            {
                //this.size = GameSettings.Titan.Colossal.Size.Value;
                base.photonView.RPC(nameof(setSize), PhotonTargets.AllBuffered, new object[] { this.size });
                this.lagMax = 150f + (this.size * 3f);
                this.healthTime = 0f;
                this.maxHealth = Health;
                if (GameSettings.Titan.Colossal.HealthMode != TitanHealthMode.Disabled)
                {
                    maxHealth = NapeArmorTotal = Health = GameSettings.Titan.Colossal.Health;
                }
                if (this.Health > 0)
                {
                    base.photonView.RPC(nameof(labelRPC), PhotonTargets.AllBuffered, new object[] { this.Health, this.maxHealth });
                }
                this.loadskin();
            }
            this.hasspawn = true;
        }

        private void startMain()
        {
            if (this.myHero == null)
            {
                this.findNearestHero();
            }
            base.name = "ColossalTitan";
            //else if (Gamemode.Settings.Difficulty == Difficulty.Hard)
            //{
            //    this.NapeArmor = !flag ? 8000 : 3500;
            //    IEnumerator enumerator = base.GetComponent<Animation>().GetEnumerator();
            //    try
            //    {
            //        while (enumerator.MoveNext())
            //        {
            //            AnimationState current = (AnimationState)enumerator.Current;
            //            if (current != null)
            //                current.speed = 1.02f;
            //        }
            //    }
            //    finally
            //    {
            //        IDisposable disposable = enumerator as IDisposable;
            //        if (disposable != null)
            //        {
            //            disposable.Dispose();
            //        }
            //    }
            //}
            //else if (Gamemode.Settings.Difficulty == Difficulty.Abnormal)
            //{
            //    this.NapeArmor = !flag ? 12000 : 5000;
            //    IEnumerator enumerator2 = base.GetComponent<Animation>().GetEnumerator();
            //    try
            //    {
            //        while (enumerator2.MoveNext())
            //        {
            //            AnimationState state2 = (AnimationState)enumerator2.Current;
            //            if (state2 != null)
            //                state2.speed = 1.05f;
            //        }
            //    }
            //    finally
            //    {
            //        IDisposable disposable2 = enumerator2 as IDisposable;
            //        if (disposable2 != null)
            //        {
            //            disposable2.Dispose();
            //        }
            //    }
            //}
            this.state = "wait";
            Transform transform = base.transform;
            transform.position += (Vector3) (-Vector3.up * 10000f);
            //if (FengGameManagerMKII.LAN)
            //{
            //    base.GetComponent<PhotonView>().enabled = false;
            //}
            //else
            //{
            //    base.GetComponent<NetworkView>().enabled = false;
            //}

            if (door_broken == null)
            {
                this.door_broken = GameObject.Find("door_broke");
                this.door_broken.SetActive(false);
            }

            this.door_closed = GameObject.Find("door_fine");
            if (door_closed != null)
                door_closed.SetActive(true);
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
        public override void OnNapeHitRpc(int viewID, int damage, PhotonMessageInfo info = new PhotonMessageInfo())
        {
            Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
            PhotonView view = PhotonView.Find(viewID);
            if (view != null)
            {
                Vector3 vector = view.gameObject.transform.position - transform.transform.position;
                if ((vector.magnitude < this.lagMax) && (this.healthTime <= 0f))
                {
                    if (damage >= GameSettings.Titan.MinimumDamage.Value)
                    {
                        this.Health -= damage;
                    }
                    if (this.maxHealth > 0f)
                    {
                        base.photonView.RPC(nameof(labelRPC), PhotonTargets.AllBuffered, new object[] { this.Health, this.maxHealth });
                    }
                    this.neckSteam();

                    if (this.Health <= 0)
                    {
                        this.Health = 0;
                        if (!this.hasDie)
                        {
                            base.photonView.RPC(nameof(netDie), PhotonTargets.OthersBuffered, new object[0]);
                            this.netDie();
                            manager.titanGetKill(view.owner, damage, base.name);
                        }
                    }
                    else
                    {
                        manager.sendKillInfo(false, (string) view.owner.CustomProperties[PhotonPlayerProperty.name], true, "Colossal Titan's neck", damage);
                        object[] parameters = new object[] { damage };
                        manager.photonView.RPC(nameof(FengGameManagerMKII.netShowDamage), view.owner, parameters);
                    }
                    this.healthTime = 0.2f;
                }
            }
        }

        protected override void Update()
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
                        Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().FlashBlind();
                        if (base.photonView.isMine)
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
                                if ((((gameObject.tag == "erenHitbox") && (this.attackAnimation == "combo_3")) && PhotonNetwork.isMasterClient))
                                {
                                    gameObject.transform.root.gameObject.GetComponent<ErenTitan>().hitByFTByServer(3);
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
                            base.photonView.RPC(nameof(stopSweepSmoke), PhotonTargets.Others, new object[0]);
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
                            PhotonNetwork.Destroy(door_closed);
                            base.photonView.RPC(nameof(changeDoor), PhotonTargets.OthersBuffered, new object[0]);
                            PhotonNetwork.Instantiate("FX/boom1_CT_KICK", (Vector3) ((base.transform.position + (base.transform.forward * 120f)) + (base.transform.right * 30f)), Quaternion.Euler(270f, 0f, 0f), 0);
                            PhotonNetwork.Instantiate("rock", (Vector3) ((base.transform.position + (base.transform.forward * 120f)) + (base.transform.right * 30f)), Quaternion.Euler(0f, 0f, 0f), 0);
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
                            obj4 = PhotonNetwork.Instantiate("FX/boom1", this.checkHitCapsuleStart.position, Quaternion.Euler(270f, 0f, 0f), 0);
                            if (obj4.GetComponent<EnemyfxIDcontainer>() != null)
                            {
                                obj4.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
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
                            PhotonNetwork.Instantiate("FX/colossal_steam", base.transform.position + ((Vector3) (base.transform.up * 185f)), Quaternion.Euler(270f, 0f, 0f), 0);
                            PhotonNetwork.Instantiate("FX/colossal_steam", base.transform.position + ((Vector3) (base.transform.up * 303f)), Quaternion.Euler(270f, 0f, 0f), 0);
                            PhotonNetwork.Instantiate("FX/colossal_steam", base.transform.position + ((Vector3) (base.transform.up * 50f)), Quaternion.Euler(270f, 0f, 0f), 0);
                        }
                        if (base.GetComponent<Animation>()[this.actionName].normalizedTime >= 1f)
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
                            if (this.hasDie)
                            {
                                if (PhotonNetwork.isMasterClient)
                                {
                                    PhotonNetwork.Destroy(base.photonView);
                                }
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
                    float current = -Mathf.Atan2(vector.z, vector.x) * Mathf.Rad2Deg;
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
}