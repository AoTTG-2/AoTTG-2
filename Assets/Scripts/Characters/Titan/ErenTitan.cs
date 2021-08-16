using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Titan.Attacks;
using Assets.Scripts.Characters.Titan.Attacks.Eren;
using Assets.Scripts.Characters.Titan.Body;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.UI.Input;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan
{
    /// <summary>
    /// The Eren Titan. Requires refactoring
    /// </summary>
    public class ErenTitan : TitanBase
    {
        public new ErenTitanBody Body { get; protected set; }
        public AudioSource AudioSourceFoot;

        private string attackAnimation;
        private Transform attackBox;
        private bool attackChkOnce;
        public GameObject bottomObject;
        public bool canJump = true;
        private ArrayList checkPoints = new ArrayList();
        public Camera currentCamera;
        public Vector3 dashDirection;
        private float dieTime;
        private float facingDirection;
        private float gravity = 500f;
        private bool grounded;
        public bool hasDied;
        private bool hasDieSteam;
        public bool hasSpawn;
        private string hitAnimation;
        private float hitPause;
        private ArrayList hitTargets;
        private bool isAttack;
        public bool isHit;
        private bool isHitWhileCarryingRock;
        private bool isNextAttack;
        private bool isPlayRoar;
        private bool isROCKMOVE;
        public float jumpHeight = 2f;
        private bool justGrounded;
        public float lifeTime = 9999f;
        private float lifeTimeMax = 9999f;
        public float maxVelocityChange = 100f;
        public GameObject myNetWorkName;
        private float myR;
        private bool needFreshCorePosition;
        private bool needRoar;
        private Vector3 oldCorePosition;
        public GameObject realBody;
        public GameObject rock;
        private bool rockHitGround;
        public bool rockLift;
        private int rockPhase;
        public float speed = 80f;
        private float sqrt2 = Mathf.Sqrt(2f);
        private int stepSoundPhase = 2;
        private Vector3 targetCheckPt;
        private float waitCounter;

        protected override void Awake()
        {
            base.Awake();
            Body = GetComponent<ErenTitanBody>();
            Faction = FactionService.GetHumanity();
            Attacks = new Attack<TitanBase>[] {new PunchAttack()};
            foreach (var attack in Attacks)
            {
                attack.Initialize(this);
            }
        }
        
        public override void Initialize(TitanConfiguration configuration)
        {
            MaxHealth = Health = 10000;
            AnimationWalk = "run";
            AnimationDeath = "die";
            Speed = 18f;

            if (Health <= 0)
                Destroy(HealthLabel);

            if (Health < 0)
            {
                if (HealthLabel != null)
                {
                    Destroy(HealthLabel);
                }
            }
            else
            {
                var color = "7FFF00";
                var num2 = ((float) Health) / ((float) MaxHealth);
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
                HealthLabel.GetComponent<TextMesh>().text = $"<color=#{color}>{Health}</color>";
            }

            AttackDistance = Vector3.Distance(base.transform.position, Body.AttackFrontGround.position) * 1.65f;
            EntityService.Register(this);
        }

        public void born()
        {
            foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
            {
                if (obj2.GetComponent<FemaleTitan>() != null)
                {
                    obj2.GetComponent<FemaleTitan>().erenIsHere(gameObject);
                }
            }
            if (!bottomObject.GetComponent<CheckHitGround>().isGrounded)
            {
                playAnimation("jump_air");
                needRoar = true;
            }
            else
            {
                needRoar = false;
                playAnimation("born");
                isPlayRoar = false;
            }
            playSound("snd_eren_shift");
            if (photonView.isMine)
            {
                PhotonNetwork.Instantiate("FX/Thunder", transform.position + ((Vector3) (Vector3.up * 23f)), Quaternion.Euler(270f, 0f, 0f), 0);
            }
            float num2 = 30f;
            lifeTime = 30f;
            lifeTimeMax = num2;
        }

        private void crossFade(string aniName, float time)
        {
            GetComponent<Animation>().CrossFade(aniName, time);
            if (PhotonNetwork.connected && photonView.isMine)
            {
                object[] parameters = new object[] { aniName, time };
                photonView.RPC(nameof(netCrossFade), PhotonTargets.Others, parameters);
            }
        }

        [PunRPC]
        private void endMovingRock()
        {
            isROCKMOVE = false;
        }

        private void falseAttack()
        {
            isAttack = false;
            isNextAttack = false;
            hitTargets = new ArrayList();
            attackChkOnce = false;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            return;
            if (!IN_GAME_MAIN_CAMERA.isPausing)
            {
                if (rockLift)
                {
                    RockUpdate();
                }
                else if (photonView.isMine)
                {
                    if (hitPause > 0f)
                    {
                        GetComponent<Rigidbody>().velocity = Vector3.zero;
                    }
                    else if (hasDied)
                    {
                        GetComponent<Rigidbody>().velocity = Vector3.zero + ((Vector3) (Vector3.up * GetComponent<Rigidbody>().velocity.y));
                        GetComponent<Rigidbody>().AddForce(new Vector3(0f, -gravity * GetComponent<Rigidbody>().mass, 0f));
                    }
                    else if (photonView.isMine)
                    {
                        if (GetComponent<Rigidbody>().velocity.magnitude > 50f)
                        {
                            currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentCamera.GetComponent<Camera>().fieldOfView, Mathf.Min(100f, GetComponent<Rigidbody>().velocity.magnitude), 0.1f);
                        }
                        else
                        {
                            currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentCamera.GetComponent<Camera>().fieldOfView, 50f, 0.1f);
                        }
                        if (bottomObject.GetComponent<CheckHitGround>().isGrounded)
                        {
                            if (!grounded)
                            {
                                justGrounded = true;
                            }
                            grounded = true;
                            bottomObject.GetComponent<CheckHitGround>().isGrounded = false;
                        }
                        else
                        {
                            grounded = false;
                        }
                        float x = 0f;
                        float z = 0f;
                        if (!IN_GAME_MAIN_CAMERA.isTyping)
                        {
                            if (InputManager.Key(InputHuman.Forward))
                            {
                                z = 1f;
                            }
                            else if (InputManager.Key(InputHuman.Backward))
                            {
                                z = -1f;
                            }
                            else
                            {
                                z = 0f;
                            }
                            if (InputManager.Key(InputHuman.Left))
                            {
                                x = -1f;
                            }
                            else if (InputManager.Key(InputHuman.Right))
                            {
                                x = 1f;
                            }
                            else
                            {
                                x = 0f;
                            }
                        }
                        if (needFreshCorePosition)
                        {
                            oldCorePosition = transform.position - transform.Find("Amarture/Core").position;
                            needFreshCorePosition = false;
                        }
                        if (!isAttack && !isHit)
                        {
                            if (grounded)
                            {
                                Vector3 zero = Vector3.zero;
                                if (justGrounded)
                                {
                                    justGrounded = false;
                                    zero = GetComponent<Rigidbody>().velocity;
                                    if (GetComponent<Animation>().IsPlaying("jump_air"))
                                    {
                                        GameObject obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("FX/boom2_eren"), transform.position, Quaternion.Euler(270f, 0f, 0f));
                                        obj2.transform.localScale = (Vector3) (Vector3.one * 1.5f);
                                        if (needRoar)
                                        {
                                            playAnimation("born");
                                            needRoar = false;
                                            isPlayRoar = false;
                                        }
                                        else
                                        {
                                            playAnimation("jump_land");
                                        }
                                    }
                                }
                                if ((!GetComponent<Animation>().IsPlaying("jump_land") && !isAttack) && (!isHit && !GetComponent<Animation>().IsPlaying("born")))
                                {
                                    Vector3 vector7 = new Vector3(x, 0f, z);
                                    float y = currentCamera.transform.rotation.eulerAngles.y;
                                    float num4 = Mathf.Atan2(z, x) * Mathf.Rad2Deg;
                                    num4 = -num4 + 90f;
                                    float num5 = y + num4;
                                    float num6 = -num5 + 90f;
                                    float num7 = Mathf.Cos(num6 * Mathf.Deg2Rad);
                                    float num8 = Mathf.Sin(num6 * Mathf.Deg2Rad);
                                    zero = new Vector3(num7, 0f, num8);
                                    float num9 = (vector7.magnitude <= 0.95f) ? ((vector7.magnitude >= 0.25f) ? vector7.magnitude : 0f) : 1f;
                                    zero = (Vector3) (zero * num9);
                                    zero = (Vector3) (zero * speed);
                                    if ((x == 0f) && (z == 0f))
                                    {
                                        if (((!GetComponent<Animation>().IsPlaying("idle") && !GetComponent<Animation>().IsPlaying("dash_land")) && (!GetComponent<Animation>().IsPlaying("dodge") && !GetComponent<Animation>().IsPlaying("jump_start"))) && (!GetComponent<Animation>().IsPlaying("jump_air") && !GetComponent<Animation>().IsPlaying("jump_land")))
                                        {
                                            crossFade("idle", 0.1f);
                                            zero = (Vector3) (zero * 0f);
                                        }
                                        num5 = -874f;
                                    }
                                    else if ((!GetComponent<Animation>().IsPlaying("run") && !GetComponent<Animation>().IsPlaying("jump_start")) && !GetComponent<Animation>().IsPlaying("jump_air"))
                                    {
                                        crossFade("run", 0.1f);
                                    }
                                    if (num5 != -874f)
                                    {
                                        facingDirection = num5;
                                    }
                                }
                                Vector3 velocity = GetComponent<Rigidbody>().velocity;
                                Vector3 force = zero - velocity;
                                force.x = Mathf.Clamp(force.x, -maxVelocityChange, maxVelocityChange);
                                force.z = Mathf.Clamp(force.z, -maxVelocityChange, maxVelocityChange);
                                force.y = 0f;
                                if (GetComponent<Animation>().IsPlaying("jump_start") && (GetComponent<Animation>()["jump_start"].normalizedTime >= 1f))
                                {
                                    playAnimation("jump_air");
                                    force.y += 240f;
                                }
                                else if (GetComponent<Animation>().IsPlaying("jump_start"))
                                {
                                    force = -GetComponent<Rigidbody>().velocity;
                                }
                                GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
                                GetComponent<Rigidbody>().rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, facingDirection, 0f), Time.deltaTime * 10f);
                            }
                            else
                            {
                                if (GetComponent<Animation>().IsPlaying("jump_start") && (GetComponent<Animation>()["jump_start"].normalizedTime >= 1f))
                                {
                                    playAnimation("jump_air");
                                    GetComponent<Rigidbody>().AddForce((Vector3) (Vector3.up * 240f), ForceMode.VelocityChange);
                                }
                                if (!GetComponent<Animation>().IsPlaying("jump") && !isHit)
                                {
                                    Vector3 vector11 = new Vector3(x, 0f, z);
                                    float num10 = currentCamera.transform.rotation.eulerAngles.y;
                                    float num11 = Mathf.Atan2(z, x) * Mathf.Rad2Deg;
                                    num11 = -num11 + 90f;
                                    float num12 = num10 + num11;
                                    float num13 = -num12 + 90f;
                                    float num14 = Mathf.Cos(num13 * Mathf.Deg2Rad);
                                    float num15 = Mathf.Sin(num13 * Mathf.Deg2Rad);
                                    Vector3 vector13 = new Vector3(num14, 0f, num15);
                                    float num16 = (vector11.magnitude <= 0.95f) ? ((vector11.magnitude >= 0.25f) ? vector11.magnitude : 0f) : 1f;
                                    vector13 = (vector13 * num16);
                                    vector13 = (vector13 * (speed * 2f));
                                    if ((x == 0f) && (z == 0f))
                                    {
                                        num12 = -874f;
                                    }
                                    else
                                    {
                                        GetComponent<Rigidbody>().AddForce(vector13, ForceMode.Impulse);
                                    }
                                    if (num12 != -874f)
                                    {
                                        facingDirection = num12;
                                    }
                                    if ((!GetComponent<Animation>().IsPlaying(string.Empty) && !GetComponent<Animation>().IsPlaying("attack3_2")) && !GetComponent<Animation>().IsPlaying("attack5"))
                                    {
                                        GetComponent<Rigidbody>().rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, facingDirection, 0f), Time.deltaTime * 6f);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Vector3 vector4 = (transform.position - transform.Find("Amarture/Core").position) - oldCorePosition;
                            oldCorePosition = transform.position - transform.Find("Amarture/Core").position;
                            GetComponent<Rigidbody>().velocity = (Vector3) ((vector4 / Time.deltaTime) + (Vector3.up * GetComponent<Rigidbody>().velocity.y));
                            GetComponent<Rigidbody>().rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, facingDirection, 0f), Time.deltaTime * 10f);
                            if (justGrounded)
                            {
                                justGrounded = false;
                            }
                        }
                        GetComponent<Rigidbody>().AddForce(new Vector3(0f, -gravity * GetComponent<Rigidbody>().mass, 0f));
                    }
                }
            }
        }

        public void hitByFT(int phase)
        {
            if (!hasDied)
            {
                isHit = true;
                hitAnimation = "hit_annie_" + phase;
                falseAttack();
                playAnimation(hitAnimation);
                needFreshCorePosition = true;
                if (phase == 3)
                {
                    GameObject obj2;
                    hasDied = true;
                    Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                    if (PhotonNetwork.isMasterClient)
                    {
                        obj2 = PhotonNetwork.Instantiate("bloodExplore", transform.position + ((Vector3) ((Vector3.up * 1f) * 4f)), Quaternion.Euler(270f, 0f, 0f), 0);
                    }
                    else
                    {
                        obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("bloodExplore"), transform.position + ((Vector3) ((Vector3.up * 1f) * 4f)), Quaternion.Euler(270f, 0f, 0f));
                    }
                    obj2.transform.localScale = transform.localScale;
                    if (PhotonNetwork.isMasterClient)
                    {
                        obj2 = PhotonNetwork.Instantiate("bloodsplatter", transform.position, Quaternion.Euler(90f + transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 0);
                    }
                    else
                    {
                        obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("bloodsplatter"), transform.position, Quaternion.Euler(90f + transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
                    }
                    obj2.transform.localScale = transform.localScale;
                    obj2.transform.parent = transform;
                    if (PhotonNetwork.isMasterClient)
                    {
                        obj2 = PhotonNetwork.Instantiate("FX/justSmoke", transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
                    }
                    else
                    {
                        obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("FX/justSmoke"), transform.position, Quaternion.Euler(270f, 0f, 0f));
                    }
                    obj2.transform.parent = transform;
                }
            }
        }

        public void hitByFTByServer(int phase)
        {
            object[] parameters = new object[] { phase };
            photonView.RPC(nameof(hitByFTRPC), PhotonTargets.All, parameters);
        }

        [PunRPC]
        private void hitByFTRPC(int phase)
        {
            if (photonView.isMine)
            {
                hitByFT(phase);
            }
        }

        //public override void OnHit(Entity attacker, int damage)
        //{
        //    hitByTitan();
        //}

        public void hitByTitan()
        {
            if ((!isHit && !hasDied) && !GetComponent<Animation>().IsPlaying("born"))
            {
                if (rockLift)
                {
                    crossFade("die", 0.1f);
                    isHitWhileCarryingRock = true;
                    //TODO: 160, game lose
                    //this.gameWin2();
                    object[] parameters = new object[] { "set" };
                    photonView.RPC(nameof(rockPlayAnimation), PhotonTargets.All, parameters);
                }
                else
                {
                    isHit = true;
                    hitAnimation = "hit_titan";
                    falseAttack();
                    playAnimation(hitAnimation);
                    needFreshCorePosition = true;
                }
            }
        }

        public void hitByTitanByServer()
        {
            photonView.RPC(nameof(hitByTitanRPC), PhotonTargets.All, new object[0]);
        }

        [PunRPC]
        private void hitByTitanRPC()
        {
            if (photonView.isMine)
            {
                hitByTitan();
            }
        }

        public bool IsGrounded()
        {
            return bottomObject.GetComponent<CheckHitGround>().isGrounded;
        }

        public void LateUpdate()
        {
            if (((!IN_GAME_MAIN_CAMERA.isPausing) && !rockLift) && (photonView.isMine))
            {
                Quaternion to = Quaternion.Euler(GameObject.Find("MainCamera").transform.rotation.eulerAngles.x, GameObject.Find("MainCamera").transform.rotation.eulerAngles.y, 0f);
                GameObject.Find("MainCamera").transform.rotation = Quaternion.Lerp(GameObject.Find("MainCamera").transform.rotation, to, Time.deltaTime * 2f);
            }
        }

        public void loadskin()
        {
            if (photonView.isMine && (((int) FengGameManagerMKII.settings[1]) == 1))
            {
                photonView.RPC(nameof(loadskinRPC), PhotonTargets.AllBuffered, new object[] { (string) FengGameManagerMKII.settings[0x41] });
            }
        }

        public IEnumerator loadskinE(string url)
        {
            while (!hasSpawn)
            {
                yield return null;
            }
            bool mipmap = true;
            bool iteratorVariable1 = false;
            if (((int)FengGameManagerMKII.settings[0x3f]) == 1)
            {
                mipmap = false;
            }
            foreach (Renderer iteratorVariable4 in GetComponentsInChildren<Renderer>())
            {
                if (!FengGameManagerMKII.linkHash[2].ContainsKey(url))
                {
                    WWW link = new WWW(url);
                    yield return link;
                    Texture2D iteratorVariable6 = RCextensions.loadimage(link, mipmap, 0xf4240);
                    link.Dispose();
                    if (!FengGameManagerMKII.linkHash[2].ContainsKey(url))
                    {
                        iteratorVariable1 = true;
                        iteratorVariable4.material.mainTexture = iteratorVariable6;
                        FengGameManagerMKII.linkHash[2].Add(url, iteratorVariable4.material);
                        iteratorVariable4.material = (Material)FengGameManagerMKII.linkHash[2][url];
                    }
                    else
                    {
                        iteratorVariable4.material = (Material)FengGameManagerMKII.linkHash[2][url];
                    }
                }
                else
                {
                    iteratorVariable4.material = (Material)FengGameManagerMKII.linkHash[2][url];
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
                StartCoroutine(loadskinE(url));
            }
        }

        [PunRPC]
        private void netCrossFade(string aniName, float time)
        {
            GetComponent<Animation>().CrossFade(aniName, time);
        }

        [PunRPC]
        private void netPlayAnimation(string aniName)
        {
            GetComponent<Animation>().Play(aniName);
        }

        [PunRPC]
        private void netPlayAnimationAt(string aniName, float normalizedTime)
        {
            GetComponent<Animation>().Play(aniName);
            GetComponent<Animation>()[aniName].normalizedTime = normalizedTime;
        }

        [PunRPC]
        private void netTauntAttack(float tauntTime, float distance = 100f)
        {
            foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
            {
                if ((Vector3.Distance(obj2.transform.position, transform.position) < distance) && (obj2.GetComponent<MindlessTitan>() != null))
                {
                    throw new NotImplementedException("Mindless Titans should be taunted by Eren Titan");
                    //obj2.GetComponent<TITAN>().beTauntedBy(gameObject, tauntTime);
                }
                if (obj2.GetComponent<FemaleTitan>() != null)
                {
                    obj2.GetComponent<FemaleTitan>().erenIsHere(gameObject);
                }
            }
        }
        
        public void playAnimation(string aniName)
        {
            GetComponent<Animation>().Play(aniName);
            if (PhotonNetwork.connected && photonView.isMine)
            {
                object[] parameters = new object[] { aniName };
                photonView.RPC(nameof(netPlayAnimation), PhotonTargets.Others, parameters);
            }
        }

        private void playAnimationAt(string aniName, float normalizedTime)
        {
            GetComponent<Animation>().Play(aniName);
            GetComponent<Animation>()[aniName].normalizedTime = normalizedTime;
            if (PhotonNetwork.connected && photonView.isMine)
            {
                object[] parameters = new object[] { aniName, normalizedTime };
                photonView.RPC(nameof(netPlayAnimationAt), PhotonTargets.Others, parameters);
            }
        }

        private void playSound(string sndname)
        {
            playsoundRPC(sndname);
            object[] parameters = new object[] { sndname };
            photonView.RPC(nameof(playsoundRPC), PhotonTargets.Others, parameters);
        }

        [PunRPC]
        private void playsoundRPC(string sndname)
        {
            transform.Find(sndname).GetComponent<AudioSource>().Play();
        }

        [PunRPC]
        private void removeMe()
        {
            PhotonNetwork.RemoveRPCs(photonView);
            UnityEngine.Object.Destroy(gameObject);
        }

        [PunRPC]
        private void rockPlayAnimation(string anim)
        {
            rock.GetComponent<Animation>().Play(anim);
            rock.GetComponent<Animation>()[anim].speed = 1f;
        }

        private void RockUpdate()
        {
            if (!isHitWhileCarryingRock)
            {
                if (isROCKMOVE)
                {
                    rock.transform.position = transform.position;
                    rock.transform.rotation = transform.rotation;
                }
                if (photonView.isMine)
                {
                    if (rockPhase == 0)
                    {
                        GetComponent<Rigidbody>().AddForce(-GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);
                        GetComponent<Rigidbody>().AddForce(new Vector3(0f, -10f * GetComponent<Rigidbody>().mass, 0f));
                        waitCounter += Time.deltaTime;
                        if (waitCounter > 20f)
                        {
                            rockPhase++;
                            crossFade("idle", 1f);
                            waitCounter = 0f;
                            setRoute();
                        }
                    }
                    else if (rockPhase == 1)
                    {
                        GetComponent<Rigidbody>().AddForce(-GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);
                        GetComponent<Rigidbody>().AddForce(new Vector3(0f, -gravity * GetComponent<Rigidbody>().mass, 0f));
                        waitCounter += Time.deltaTime;
                        if (waitCounter > 2f)
                        {
                            rockPhase++;
                            crossFade("run", 0.2f);
                            waitCounter = 0f;
                        }
                    }
                    else if (rockPhase == 2)
                    {
                        Vector3 vector = transform.forward * 30f;
                        Vector3 velocity = GetComponent<Rigidbody>().velocity;
                        Vector3 force = vector - velocity;
                        force.x = Mathf.Clamp(force.x, -maxVelocityChange, maxVelocityChange);
                        force.z = Mathf.Clamp(force.z, -maxVelocityChange, maxVelocityChange);
                        force.y = 0f;
                        GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
                        if (transform.position.z < -238f)
                        {
                            transform.position = new Vector3(transform.position.x, 0f, -238f);
                            rockPhase++;
                            crossFade("idle", 0.2f);
                            waitCounter = 0f;
                        }
                    }
                    else if (rockPhase == 3)
                    {
                        GetComponent<Rigidbody>().AddForce(-GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);
                        GetComponent<Rigidbody>().AddForce(new Vector3(0f, -10f * GetComponent<Rigidbody>().mass, 0f));
                        waitCounter += Time.deltaTime;
                        if (waitCounter > 1f)
                        {
                            rockPhase++;
                            crossFade("rock_lift", 0.1f);
                            object[] parameters = new object[] { "lift" };
                            photonView.RPC(nameof(rockPlayAnimation), PhotonTargets.All, parameters);
                            waitCounter = 0f;
                            targetCheckPt = (Vector3) checkPoints[0];
                        }
                    }
                    else if (rockPhase == 4)
                    {
                        GetComponent<Rigidbody>().AddForce(-GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);
                        GetComponent<Rigidbody>().AddForce(new Vector3(0f, -gravity * GetComponent<Rigidbody>().mass, 0f));
                        waitCounter += Time.deltaTime;
                        if (waitCounter > 4.2f)
                        {
                            rockPhase++;
                            crossFade("rock_walk", 0.1f);
                            object[] objArray3 = new object[] { "move" };
                            photonView.RPC(nameof(rockPlayAnimation), PhotonTargets.All, objArray3);
                            rock.GetComponent<Animation>()["move"].normalizedTime = GetComponent<Animation>()["rock_walk"].normalizedTime;
                            waitCounter = 0f;
                            photonView.RPC(nameof(startMovingRock), PhotonTargets.All, new object[0]);
                        }
                    }
                    else if (rockPhase == 5)
                    {
                        if (Vector3.Distance(transform.position, targetCheckPt) < 10f)
                        {
                            if (checkPoints.Count > 0)
                            {
                                if (checkPoints.Count == 1)
                                {
                                    rockPhase++;
                                }
                                else
                                {
                                    Vector3 vector6 = (Vector3) checkPoints[0];
                                    targetCheckPt = vector6;
                                    checkPoints.RemoveAt(0);
                                    GameObject[] objArray = GameObject.FindGameObjectsWithTag("titanRespawn2");
                                    GameObject obj2 = GameObject.Find("titanRespawnTrost" + (7 - checkPoints.Count));
                                    if (obj2 != null)
                                    {
                                        foreach (GameObject obj3 in objArray)
                                        {
                                            if (obj3.transform.parent.gameObject == obj2)
                                            {
                                                throw new NotImplementedException("Eren titan requires punk disabling and titans to automatically start chasing");
                                                //var obj4 = FengGameManagerMKII.instance.SpawnTitan(obj3.transform.position, obj3.transform.rotation);
                                                //obj4.GetComponent<MindlessTitan>().isAlarm = true;
                                                //obj4.GetComponent<MindlessTitan>().chaseDistance = 999999f;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                rockPhase++;
                            }
                        }
                        if ((checkPoints.Count > 0) && (UnityEngine.Random.Range(0, 0xbb8) < (10 - checkPoints.Count)))
                        {
                            Quaternion quaternion;
                            RaycastHit hit;
                            if (UnityEngine.Random.Range(0, 10) > 5)
                            {
                                quaternion = transform.rotation * Quaternion.Euler(0f, UnityEngine.Random.Range((float) 150f, (float) 210f), 0f);
                            }
                            else
                            {
                                quaternion = transform.rotation * Quaternion.Euler(0f, UnityEngine.Random.Range((float) -30f, (float) 30f), 0f);
                            }
                            Vector3 vector7 = (Vector3) (quaternion * new Vector3(UnityEngine.Random.Range((float) 100f, (float) 200f), 0f, 0f));
                            Vector3 position = transform.position + vector7;
                            LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("Ground");
                            float y = 0f;
                            if (Physics.Raycast(position + ((Vector3) (Vector3.up * 500f)), -Vector3.up, out hit, 1000f, mask2.value))
                            {
                                y = hit.point.y;
                            }
                            position += (Vector3) (Vector3.up * y);
                            throw new NotImplementedException("Eren titan requires punk disabling and titans to automatically start chasing");
                            //GameObject obj5 = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().spawnTitan(70, position, transform.rotation, false);
                            //obj5.GetComponent<TITAN>().isAlarm = true;
                            //obj5.GetComponent<TITAN>().chaseDistance = 999999f;
                        }
                        Vector3 vector10 = (Vector3) (transform.forward * 16f);
                        Vector3 vector11 = GetComponent<Rigidbody>().velocity;
                        Vector3 vector12 = vector10 - vector11;
                        vector12.x = Mathf.Clamp(vector12.x, -maxVelocityChange, maxVelocityChange);
                        vector12.z = Mathf.Clamp(vector12.z, -maxVelocityChange, maxVelocityChange);
                        vector12.y = 0f;
                        GetComponent<Rigidbody>().AddForce(vector12, ForceMode.VelocityChange);
                        GetComponent<Rigidbody>().AddForce(new Vector3(0f, -gravity * GetComponent<Rigidbody>().mass, 0f));
                        Vector3 vector13 = targetCheckPt - transform.position;
                        float current = -Mathf.Atan2(vector13.z, vector13.x) * Mathf.Rad2Deg;
                        float num4 = -Mathf.DeltaAngle(current, gameObject.transform.rotation.eulerAngles.y - 90f);
                        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, gameObject.transform.rotation.eulerAngles.y + num4, 0f), 0.8f * Time.deltaTime);
                    }
                    else if (rockPhase == 6)
                    {
                        GetComponent<Rigidbody>().AddForce(-GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);
                        GetComponent<Rigidbody>().AddForce(new Vector3(0f, -10f * GetComponent<Rigidbody>().mass, 0f));
                        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                        rockPhase++;
                        crossFade("rock_fix_hole", 0.1f);
                        object[] objArray4 = new object[] { "set" };
                        photonView.RPC(nameof(rockPlayAnimation), PhotonTargets.All, objArray4);
                        photonView.RPC(nameof(endMovingRock), PhotonTargets.All, new object[0]);
                    }
                    else if (rockPhase == 7)
                    {
                        GetComponent<Rigidbody>().AddForce(-GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);
                        GetComponent<Rigidbody>().AddForce(new Vector3(0f, -10f * GetComponent<Rigidbody>().mass, 0f));
                        if (GetComponent<Animation>()["rock_fix_hole"].normalizedTime >= 1.2f)
                        {
                            crossFade("die", 0.1f);
                            rockPhase++;
                            //TODO: 160, game won
                            //this.gameWin2();
                        }
                        if ((GetComponent<Animation>()["rock_fix_hole"].normalizedTime >= 0.62f) && !rockHitGround)
                        {
                            rockHitGround = true;
                            if (PhotonNetwork.isMasterClient)
                            {
                                PhotonNetwork.Instantiate("FX/boom1_CT_KICK", new Vector3(0f, 30f, 684f), Quaternion.Euler(270f, 0f, 0f), 0);
                            }
                            else
                            {
                                UnityEngine.Object.Instantiate(Resources.Load("FX/boom1_CT_KICK"), new Vector3(0f, 30f, 684f), Quaternion.Euler(270f, 0f, 0f));
                            }
                        }
                    }
                }
            }
        }

        public void setRoute()
        {
            GameObject obj2 = GameObject.Find("routeTrost");
            checkPoints = new ArrayList();
            for (int i = 1; i <= 7; i++)
            {
                checkPoints.Add(obj2.transform.Find("r" + i).position);
            }
            checkPoints.Add("end");
        }

        private void showAimUI()
        {
            GameObject obj2 = GameObject.Find("cross1");
            GameObject obj3 = GameObject.Find("cross2");
            GameObject obj4 = GameObject.Find("crossL1");
            GameObject obj5 = GameObject.Find("crossL2");
            GameObject obj6 = GameObject.Find("crossR1");
            GameObject obj7 = GameObject.Find("crossR2");
            GameObject obj8 = GameObject.Find("Distance");
            Vector3 vector = (Vector3) (Vector3.up * 10000f);
            obj7.transform.localPosition = vector;
            obj6.transform.localPosition = vector;
            obj5.transform.localPosition = vector;
            obj4.transform.localPosition = vector;
            obj8.transform.localPosition = vector;
            obj3.transform.localPosition = vector;
            obj2.transform.localPosition = vector;
        }

        private void showSkillCD()
        {
            //GameObject.Find("skill_cd_eren").GetComponent<UISprite>().fillAmount = lifeTime / lifeTimeMax;
        }

        private void Start()
        {
            loadskin();
            if (rockLift)
            {
                rock = GameObject.Find("rock");
                rock.GetComponent<Animation>()["lift"].speed = 0f;
            }
            else
            {
                currentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
                oldCorePosition = transform.position - transform.Find("Amarture/Core").position;
                myR = sqrt2 * 6f;
                GetComponent<Animation>()["hit_annie_1"].speed = 0.8f;
                GetComponent<Animation>()["hit_annie_2"].speed = 0.7f;
                GetComponent<Animation>()["hit_annie_3"].speed = 0.7f;
            }
            hasSpawn = true;
        }

        [PunRPC]
        private void startMovingRock()
        {
            isROCKMOVE = true;
        }

        #region Animation Events

        public void Footstep()
        {
            AudioSourceFoot.PlayOneShot(AudioSourceFoot.clip);
        }

        #endregion

        protected override void Update()
        {
            base.Update();
            return;
            if ((!IN_GAME_MAIN_CAMERA.isPausing) && !rockLift)
            {
                if (GetComponent<Animation>().IsPlaying("run"))
                {
                    if ((((GetComponent<Animation>()["run"].normalizedTime % 1f) > 0.3f) && ((GetComponent<Animation>()["run"].normalizedTime % 1f) < 0.75f)) && (stepSoundPhase == 2))
                    {
                        stepSoundPhase = 1;
                        Transform transform = base.transform.Find("snd_eren_foot");
                        transform.GetComponent<AudioSource>().Stop();
                        transform.GetComponent<AudioSource>().Play();
                    }
                    if (((GetComponent<Animation>()["run"].normalizedTime % 1f) > 0.75f) && (stepSoundPhase == 1))
                    {
                        stepSoundPhase = 2;
                        Transform transform2 = transform.Find("snd_eren_foot");
                        transform2.GetComponent<AudioSource>().Stop();
                        transform2.GetComponent<AudioSource>().Play();
                    }
                }
                if (photonView.isMine)
                {
                    if (hasDied)
                    {
                        if ((GetComponent<Animation>()["die"].normalizedTime >= 1f) || (hitAnimation == "hit_annie_3"))
                        {
                            if (realBody != null)
                            {
                                realBody.GetComponent<Hero>().BackToHuman();
                                realBody.transform.position = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position + ((Vector3)(Vector3.up * 2f));
                                realBody = null;
                            }
                            dieTime += Time.deltaTime;
                            if ((dieTime > 2f) && !hasDieSteam)
                            {
                                hasDieSteam = true;
                                if (photonView.isMine)
                                {
                                    PhotonNetwork.Instantiate("FX/FXtitanDie1", transform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = transform.localScale;
                                }
                            }
                            if (dieTime > 5f)
                            {
                                if (photonView.isMine)
                                {
                                    PhotonNetwork.Instantiate("FX/FXtitanDie", transform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = transform.localScale;
                                    PhotonNetwork.Destroy(photonView);
                                }
                            }
                        }
                    }
                    else if (photonView.isMine)
                    {
                        if (isHit)
                        {
                            if (GetComponent<Animation>()[hitAnimation].normalizedTime >= 1f)
                            {
                                isHit = false;
                                falseAttack();
                                playAnimation("idle");
                            }
                        }
                        else
                        {
                            if (lifeTime > 0f)
                            {
                                lifeTime -= Time.deltaTime;
                                if (lifeTime <= 0f)
                                {
                                    hasDied = true;
                                    playAnimation("die");
                                    return;
                                }
                            }
                            if (((grounded && !isAttack) && (!GetComponent<Animation>().IsPlaying("jump_land") && !isAttack)) && !GetComponent<Animation>().IsPlaying("born"))
                            {
                                if (InputManager.KeyDown(InputHuman.Attack) || InputManager.KeyDown(InputHuman.AttackSpecial))
                                {
                                    bool flag = false;
                                    if (((GameCursor.CameraMode == CameraMode.WOW) && InputManager.Key(InputHuman.Backward)) || InputManager.KeyDown(InputHuman.AttackSpecial))
                                    {
                                        if (InputManager.KeyDown(InputHuman.AttackSpecial))
                                            flag = true;
                                    
                                        if (!flag)
                                            attackAnimation = "attack_kick";
                                    }
                                    else
                                    {
                                        attackAnimation = "attack_combo_001";
                                    }
                                    if (!flag)
                                    {
                                        playAnimation(attackAnimation);
                                        GetComponent<Animation>()[attackAnimation].time = 0f;
                                        isAttack = true;
                                        needFreshCorePosition = true;
                                        if (attackAnimation == "attack_combo_001")
                                        {
                                            attackBox = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
                                        }
                                        else if (attackAnimation == "attack_combo_002")
                                        {
                                            attackBox = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
                                        }
                                        else if (attackAnimation == "attack_kick")
                                        {
                                            attackBox = transform.Find("Amarture/Core/Controller_Body/hip/thigh_R/shin_R/foot_R");
                                        }
                                        hitTargets = new ArrayList();
                                    }
                                }
                                if (InputManager.KeyDown(InputHuman.Salute))
                                {
                                    crossFade("born", 0.1f);
                                    GetComponent<Animation>()["born"].normalizedTime = 0.28f;
                                    isPlayRoar = false;
                                }
                            }
                            if (!isAttack)
                            {
                                if ((grounded || GetComponent<Animation>().IsPlaying("idle")) 
                                    && ((!GetComponent<Animation>().IsPlaying("jump_start") 
                                         && !GetComponent<Animation>().IsPlaying("jump_air")) 
                                        && (!GetComponent<Animation>().IsPlaying("jump_land") 
                                            && InputManager.Key(InputHuman.HookBoth))))
                                {
                                    crossFade("jump_start", 0.1f);
                                }
                            }
                            else
                            {
                                if ((GetComponent<Animation>()[attackAnimation].time >= 0.1f) && InputManager.KeyDown(InputHuman.Attack))
                                {
                                    isNextAttack = true;
                                }
                                float num = 0f;
                                float num2 = 0f;
                                float num3 = 0f;
                                string str = string.Empty;
                                if (attackAnimation == "attack_combo_001")
                                {
                                    num = 0.4f;
                                    num2 = 0.5f;
                                    num3 = 0.66f;
                                    str = "attack_combo_002";
                                }
                                else if (attackAnimation == "attack_combo_002")
                                {
                                    num = 0.15f;
                                    num2 = 0.25f;
                                    num3 = 0.43f;
                                    str = "attack_combo_003";
                                }
                                else if (attackAnimation == "attack_combo_003")
                                {
                                    num3 = 0f;
                                    num = 0.31f;
                                    num2 = 0.37f;
                                }
                                else if (attackAnimation == "attack_kick")
                                {
                                    num3 = 0f;
                                    num = 0.32f;
                                    num2 = 0.38f;
                                }
                                else
                                {
                                    num = 0.5f;
                                    num2 = 0.85f;
                                }
                                if (hitPause > 0f)
                                {
                                    hitPause -= Time.deltaTime;
                                    if (hitPause <= 0f)
                                    {
                                        GetComponent<Animation>()[attackAnimation].speed = 1f;
                                        hitPause = 0f;
                                    }
                                }
                                if (((num3 > 0f) && isNextAttack) && (GetComponent<Animation>()[attackAnimation].normalizedTime >= num3))
                                {
                                    if (hitTargets.Count > 0)
                                    {
                                        Transform transform3 = (Transform)hitTargets[0];
                                        if (transform3 != null)
                                        {
                                            transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(transform3.position - transform.position).eulerAngles.y, 0f);
                                            facingDirection = transform.rotation.eulerAngles.y;
                                        }
                                    }
                                    falseAttack();
                                    attackAnimation = str;
                                    crossFade(attackAnimation, 0.1f);
                                    GetComponent<Animation>()[attackAnimation].time = 0f;
                                    GetComponent<Animation>()[attackAnimation].speed = 1f;
                                    isAttack = true;
                                    needFreshCorePosition = true;
                                    if (attackAnimation == "attack_combo_002")
                                    {
                                        attackBox = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
                                    }
                                    else if (attackAnimation == "attack_combo_003")
                                    {
                                        attackBox = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
                                    }
                                    hitTargets = new ArrayList();
                                }
                                if (((GetComponent<Animation>()[attackAnimation].normalizedTime >= num) && (GetComponent<Animation>()[attackAnimation].normalizedTime <= num2)) || (!attackChkOnce && (GetComponent<Animation>()[attackAnimation].normalizedTime >= num)))
                                {
                                    if (!attackChkOnce)
                                    {
                                        if (attackAnimation == "attack_combo_002")
                                        {
                                            playSound("snd_eren_swing2");
                                        }
                                        else if (attackAnimation == "attack_combo_001")
                                        {
                                            playSound("snd_eren_swing1");
                                        }
                                        else if (attackAnimation == "attack_combo_003")
                                        {
                                            playSound("snd_eren_swing3");
                                        }
                                        attackChkOnce = true;
                                    }
                                    Collider[] colliderArray = Physics.OverlapSphere(attackBox.transform.position, 8f);
                                    for (int i = 0; i < colliderArray.Length; i++)
                                    {
                                        throw new NotImplementedException("Mindless Titans are not supported for Eren Titan");
                                        //if (colliderArray[i].gameObject.transform.root.GetComponent<TITAN>() == null)
                                        //{
                                        //    continue;
                                        //}
                                        //bool flag2 = false;
                                        //for (int j = 0; j < hitTargets.Count; j++)
                                        //{
                                        //    if (colliderArray[i].gameObject.transform.root == (Transform)hitTargets[j])
                                        //    {
                                        //        flag2 = true;
                                        //        break;
                                        //    }
                                        //}
                                        //if (!flag2 && !colliderArray[i].gameObject.transform.root.GetComponent<TITAN>().hasDie)
                                        //{
                                        //    GetComponent<Animation>()[attackAnimation].speed = 0f;
                                        //    if (attackAnimation == "attack_combo_002")
                                        //    {
                                        //        hitPause = 0.05f;
                                        //        colliderArray[i].gameObject.transform.root.GetComponent<TITAN>().hitL(transform.position, hitPause);
                                        //        currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(1f, 0.03f, 0.95f);
                                        //    }
                                        //    else if (attackAnimation == "attack_combo_001")
                                        //    {
                                        //        currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(1.2f, 0.04f, 0.95f);
                                        //        hitPause = 0.08f;
                                        //        colliderArray[i].gameObject.transform.root.GetComponent<TITAN>().hitR(transform.position, hitPause);
                                        //    }
                                        //    else if (attackAnimation == "attack_combo_003")
                                        //    {
                                        //        currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(3f, 0.1f, 0.95f);
                                        //        hitPause = 0.3f;
                                        //        colliderArray[i].gameObject.transform.root.GetComponent<TITAN>().dieHeadBlow(transform.position, hitPause);
                                        //    }
                                        //    else if (attackAnimation == "attack_kick")
                                        //    {
                                        //        currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(3f, 0.1f, 0.95f);
                                        //        hitPause = 0.2f;
                                        //        if (colliderArray[i].gameObject.transform.root.GetComponent<TITAN>().TitanType == TitanType.TYPE_CRAWLER)
                                        //        {
                                        //            colliderArray[i].gameObject.transform.root.GetComponent<TITAN>().dieBlow(transform.position, hitPause);
                                        //        }
                                        //        else if (colliderArray[i].gameObject.transform.root.transform.localScale.x < 2f)
                                        //        {
                                        //            colliderArray[i].gameObject.transform.root.GetComponent<TITAN>().dieBlow(transform.position, hitPause);
                                        //        }
                                        //        else
                                        //        {
                                        //            colliderArray[i].gameObject.transform.root.GetComponent<TITAN>().hitR(transform.position, hitPause);
                                        //        }
                                        //    }
                                        //    hitTargets.Add(colliderArray[i].gameObject.transform.root);
                                        //    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                                        //    {
                                        //        PhotonNetwork.Instantiate("hitMeatBIG", (Vector3)((colliderArray[i].transform.position + attackBox.position) * 0.5f), Quaternion.Euler(270f, 0f, 0f), 0);
                                        //    }
                                        //    else
                                        //    {
                                        //        UnityEngine.Object.Instantiate(Resources.Load("hitMeatBIG"), (Vector3)((colliderArray[i].transform.position + attackBox.position) * 0.5f), Quaternion.Euler(270f, 0f, 0f));
                                        //    }
                                        //}
                                    }
                                }
                                if (GetComponent<Animation>()[attackAnimation].normalizedTime >= 1f)
                                {
                                    falseAttack();
                                    playAnimation("idle");
                                }
                            }
                            if (GetComponent<Animation>().IsPlaying("jump_land") && (GetComponent<Animation>()["jump_land"].normalizedTime >= 1f))
                            {
                                crossFade("idle", 0.1f);
                            }
                            if (GetComponent<Animation>().IsPlaying("born"))
                            {
                                if ((GetComponent<Animation>()["born"].normalizedTime >= 0.28f) && !isPlayRoar)
                                {
                                    isPlayRoar = true;
                                    playSound("snd_eren_roar");
                                }
                                if ((GetComponent<Animation>()["born"].normalizedTime >= 0.5f) && (GetComponent<Animation>()["born"].normalizedTime <= 0.7f))
                                {
                                    currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().StartShake(0.5f, 1f, 0.95f);
                                }
                                if (GetComponent<Animation>()["born"].normalizedTime >= 1f)
                                {
                                    crossFade("idle", 0.1f);
                                    if (PhotonNetwork.isMasterClient)
                                    {
                                        object[] parameters = new object[] { 10f, 500f };
                                        photonView.RPC(nameof(netTauntAttack), PhotonTargets.MasterClient, parameters);
                                    }
                                    else
                                    {
                                        netTauntAttack(10f, 500f);
                                    }
                                }
                            }
                            showAimUI();
                            showSkillCD();
                        }
                    }
                }
            }
        }
    }
}