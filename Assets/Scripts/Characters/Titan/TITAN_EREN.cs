using Assets.Scripts.Characters.Titan;
using System;
using System.Collections;
using UnityEngine;

public class TITAN_EREN : Photon.MonoBehaviour
{
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
    public FengCustomInputs inputManager;
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

    public void born()
    {
        foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
        {
            if (obj2.GetComponent<FEMALE_TITAN>() != null)
            {
                obj2.GetComponent<FEMALE_TITAN>().erenIsHere(base.gameObject);
            }
        }
        if (!this.bottomObject.GetComponent<CheckHitGround>().isGrounded)
        {
            this.playAnimation("jump_air");
            this.needRoar = true;
        }
        else
        {
            this.needRoar = false;
            this.playAnimation("born");
            this.isPlayRoar = false;
        }
        this.playSound("snd_eren_shift");
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            UnityEngine.Object.Instantiate(Resources.Load("FX/Thunder"), base.transform.position + ((Vector3) (Vector3.up * 23f)), Quaternion.Euler(270f, 0f, 0f));
        }
        else if (base.photonView.isMine)
        {
            PhotonNetwork.Instantiate("FX/Thunder", base.transform.position + ((Vector3) (Vector3.up * 23f)), Quaternion.Euler(270f, 0f, 0f), 0);
        }
        float num2 = 30f;
        this.lifeTime = 30f;
        this.lifeTimeMax = num2;
    }

    private void crossFade(string aniName, float time)
    {
        base.GetComponent<Animation>().CrossFade(aniName, time);
        if (PhotonNetwork.connected && base.photonView.isMine)
            photonView.RPC<string, float>(netCrossFade, PhotonTargets.Others, aniName, time);
    }

    [PunRPC]
    private void endMovingRock()
    {
        this.isROCKMOVE = false;
    }

    private void falseAttack()
    {
        this.isAttack = false;
        this.isNextAttack = false;
        this.hitTargets = new ArrayList();
        this.attackChkOnce = false;
    }

    private void FixedUpdate()
    {
        if (!IN_GAME_MAIN_CAMERA.isPausing || (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE))
        {
            if (this.rockLift)
            {
                this.RockUpdate();
            }
            else if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
            {
                if (this.hitPause > 0f)
                {
                    base.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                else if (this.hasDied)
                {
                    base.GetComponent<Rigidbody>().velocity = Vector3.zero + ((Vector3) (Vector3.up * base.GetComponent<Rigidbody>().velocity.y));
                    base.GetComponent<Rigidbody>().AddForce(new Vector3(0f, -this.gravity * base.GetComponent<Rigidbody>().mass, 0f));
                }
                else if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
                {
                    if (base.GetComponent<Rigidbody>().velocity.magnitude > 50f)
                    {
                        this.currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(this.currentCamera.GetComponent<Camera>().fieldOfView, Mathf.Min(100f, base.GetComponent<Rigidbody>().velocity.magnitude), 0.1f);
                    }
                    else
                    {
                        this.currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(this.currentCamera.GetComponent<Camera>().fieldOfView, 50f, 0.1f);
                    }
                    if (this.bottomObject.GetComponent<CheckHitGround>().isGrounded)
                    {
                        if (!this.grounded)
                        {
                            this.justGrounded = true;
                        }
                        this.grounded = true;
                        this.bottomObject.GetComponent<CheckHitGround>().isGrounded = false;
                    }
                    else
                    {
                        this.grounded = false;
                    }
                    float x = 0f;
                    float z = 0f;
                    if (!IN_GAME_MAIN_CAMERA.isTyping)
                    {
                        if (this.inputManager.isInput[InputCode.up])
                        {
                            z = 1f;
                        }
                        else if (this.inputManager.isInput[InputCode.down])
                        {
                            z = -1f;
                        }
                        else
                        {
                            z = 0f;
                        }
                        if (this.inputManager.isInput[InputCode.left])
                        {
                            x = -1f;
                        }
                        else if (this.inputManager.isInput[InputCode.right])
                        {
                            x = 1f;
                        }
                        else
                        {
                            x = 0f;
                        }
                    }
                    if (this.needFreshCorePosition)
                    {
                        this.oldCorePosition = base.transform.position - base.transform.Find("Amarture/Core").position;
                        this.needFreshCorePosition = false;
                    }
                    if (!this.isAttack && !this.isHit)
                    {
                        if (this.grounded)
                        {
                            Vector3 zero = Vector3.zero;
                            if (this.justGrounded)
                            {
                                this.justGrounded = false;
                                zero = base.GetComponent<Rigidbody>().velocity;
                                if (base.GetComponent<Animation>().IsPlaying("jump_air"))
                                {
                                    GameObject obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("FX/boom2_eren"), base.transform.position, Quaternion.Euler(270f, 0f, 0f));
                                    obj2.transform.localScale = (Vector3) (Vector3.one * 1.5f);
                                    if (this.needRoar)
                                    {
                                        this.playAnimation("born");
                                        this.needRoar = false;
                                        this.isPlayRoar = false;
                                    }
                                    else
                                    {
                                        this.playAnimation("jump_land");
                                    }
                                }
                            }
                            if ((!base.GetComponent<Animation>().IsPlaying("jump_land") && !this.isAttack) && (!this.isHit && !base.GetComponent<Animation>().IsPlaying("born")))
                            {
                                Vector3 vector7 = new Vector3(x, 0f, z);
                                float y = this.currentCamera.transform.rotation.eulerAngles.y;
                                float num4 = Mathf.Atan2(z, x) * 57.29578f;
                                num4 = -num4 + 90f;
                                float num5 = y + num4;
                                float num6 = -num5 + 90f;
                                float num7 = Mathf.Cos(num6 * 0.01745329f);
                                float num8 = Mathf.Sin(num6 * 0.01745329f);
                                zero = new Vector3(num7, 0f, num8);
                                float num9 = (vector7.magnitude <= 0.95f) ? ((vector7.magnitude >= 0.25f) ? vector7.magnitude : 0f) : 1f;
                                zero = (Vector3) (zero * num9);
                                zero = (Vector3) (zero * this.speed);
                                if ((x == 0f) && (z == 0f))
                                {
                                    if (((!base.GetComponent<Animation>().IsPlaying("idle") && !base.GetComponent<Animation>().IsPlaying("dash_land")) && (!base.GetComponent<Animation>().IsPlaying("dodge") && !base.GetComponent<Animation>().IsPlaying("jump_start"))) && (!base.GetComponent<Animation>().IsPlaying("jump_air") && !base.GetComponent<Animation>().IsPlaying("jump_land")))
                                    {
                                        this.crossFade("idle", 0.1f);
                                        zero = (Vector3) (zero * 0f);
                                    }
                                    num5 = -874f;
                                }
                                else if ((!base.GetComponent<Animation>().IsPlaying("run") && !base.GetComponent<Animation>().IsPlaying("jump_start")) && !base.GetComponent<Animation>().IsPlaying("jump_air"))
                                {
                                    this.crossFade("run", 0.1f);
                                }
                                if (num5 != -874f)
                                {
                                    this.facingDirection = num5;
                                }
                            }
                            Vector3 velocity = base.GetComponent<Rigidbody>().velocity;
                            Vector3 force = zero - velocity;
                            force.x = Mathf.Clamp(force.x, -this.maxVelocityChange, this.maxVelocityChange);
                            force.z = Mathf.Clamp(force.z, -this.maxVelocityChange, this.maxVelocityChange);
                            force.y = 0f;
                            if (base.GetComponent<Animation>().IsPlaying("jump_start") && (base.GetComponent<Animation>()["jump_start"].normalizedTime >= 1f))
                            {
                                this.playAnimation("jump_air");
                                force.y += 240f;
                            }
                            else if (base.GetComponent<Animation>().IsPlaying("jump_start"))
                            {
                                force = -base.GetComponent<Rigidbody>().velocity;
                            }
                            base.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
                            base.GetComponent<Rigidbody>().rotation = Quaternion.Lerp(base.gameObject.transform.rotation, Quaternion.Euler(0f, this.facingDirection, 0f), Time.deltaTime * 10f);
                        }
                        else
                        {
                            if (base.GetComponent<Animation>().IsPlaying("jump_start") && (base.GetComponent<Animation>()["jump_start"].normalizedTime >= 1f))
                            {
                                this.playAnimation("jump_air");
                                base.GetComponent<Rigidbody>().AddForce((Vector3) (Vector3.up * 240f), ForceMode.VelocityChange);
                            }
                            if (!base.GetComponent<Animation>().IsPlaying("jump") && !this.isHit)
                            {
                                Vector3 vector11 = new Vector3(x, 0f, z);
                                float num10 = this.currentCamera.transform.rotation.eulerAngles.y;
                                float num11 = Mathf.Atan2(z, x) * 57.29578f;
                                num11 = -num11 + 90f;
                                float num12 = num10 + num11;
                                float num13 = -num12 + 90f;
                                float num14 = Mathf.Cos(num13 * 0.01745329f);
                                float num15 = Mathf.Sin(num13 * 0.01745329f);
                                Vector3 vector13 = new Vector3(num14, 0f, num15);
                                float num16 = (vector11.magnitude <= 0.95f) ? ((vector11.magnitude >= 0.25f) ? vector11.magnitude : 0f) : 1f;
                                vector13 = (Vector3) (vector13 * num16);
                                vector13 = (Vector3) (vector13 * (this.speed * 2f));
                                if ((x == 0f) && (z == 0f))
                                {
                                    num12 = -874f;
                                }
                                else
                                {
                                    base.GetComponent<Rigidbody>().AddForce(vector13, ForceMode.Impulse);
                                }
                                if (num12 != -874f)
                                {
                                    this.facingDirection = num12;
                                }
                                if ((!base.GetComponent<Animation>().IsPlaying(string.Empty) && !base.GetComponent<Animation>().IsPlaying("attack3_2")) && !base.GetComponent<Animation>().IsPlaying("attack5"))
                                {
                                    base.GetComponent<Rigidbody>().rotation = Quaternion.Lerp(base.gameObject.transform.rotation, Quaternion.Euler(0f, this.facingDirection, 0f), Time.deltaTime * 6f);
                                }
                            }
                        }
                    }
                    else
                    {
                        Vector3 vector4 = (base.transform.position - base.transform.Find("Amarture/Core").position) - this.oldCorePosition;
                        this.oldCorePosition = base.transform.position - base.transform.Find("Amarture/Core").position;
                        base.GetComponent<Rigidbody>().velocity = (Vector3) ((vector4 / Time.deltaTime) + (Vector3.up * base.GetComponent<Rigidbody>().velocity.y));
                        base.GetComponent<Rigidbody>().rotation = Quaternion.Lerp(base.gameObject.transform.rotation, Quaternion.Euler(0f, this.facingDirection, 0f), Time.deltaTime * 10f);
                        if (this.justGrounded)
                        {
                            this.justGrounded = false;
                        }
                    }
                    base.GetComponent<Rigidbody>().AddForce(new Vector3(0f, -this.gravity * base.GetComponent<Rigidbody>().mass, 0f));
                }
            }
        }
    }

    public void hitByFT(int phase)
    {
        if (!this.hasDied)
        {
            this.isHit = true;
            this.hitAnimation = "hit_annie_" + phase;
            this.falseAttack();
            this.playAnimation(this.hitAnimation);
            this.needFreshCorePosition = true;
            if (phase == 3)
            {
                GameObject obj2;
                this.hasDied = true;
                Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
                {
                    obj2 = PhotonNetwork.Instantiate("bloodExplore", transform.position + ((Vector3) ((Vector3.up * 1f) * 4f)), Quaternion.Euler(270f, 0f, 0f), 0);
                }
                else
                {
                    obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("bloodExplore"), transform.position + ((Vector3) ((Vector3.up * 1f) * 4f)), Quaternion.Euler(270f, 0f, 0f));
                }
                obj2.transform.localScale = base.transform.localScale;
                if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
                {
                    obj2 = PhotonNetwork.Instantiate("bloodsplatter", transform.position, Quaternion.Euler(90f + transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 0);
                }
                else
                {
                    obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("bloodsplatter"), transform.position, Quaternion.Euler(90f + transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
                }
                obj2.transform.localScale = base.transform.localScale;
                obj2.transform.parent = transform;
                if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
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
        photonView.RPC<int>(hitByFTRPC, PhotonTargets.All, phase);
    }

    [PunRPC]
    private void hitByFTRPC(int phase)
    {
        if (photonView.isMine)
            hitByFT(phase);
    }

    public void hitByTitan()
    {
        if ((!this.isHit && !this.hasDied) && !base.GetComponent<Animation>().IsPlaying("born"))
        {
            if (this.rockLift)
            {
                this.crossFade("die", 0.1f);
                this.isHitWhileCarryingRock = true;
                FengGameManagerMKII.instance.gameLose2();
                photonView.RPC<string>(rockPlayAnimation, PhotonTargets.All, "set");
            }
            else
            {
                this.isHit = true;
                this.hitAnimation = "hit_titan";
                this.falseAttack();
                this.playAnimation(this.hitAnimation);
                this.needFreshCorePosition = true;
            }
        }
    }

    public void hitByTitanByServer()
    {
        photonView.RPC(hitByTitanRPC, PhotonTargets.All);
    }

    [PunRPC]
    private void hitByTitanRPC()
    {
        if (base.photonView.isMine)
        {
            this.hitByTitan();
        }
    }

    public bool IsGrounded()
    {
        return this.bottomObject.GetComponent<CheckHitGround>().isGrounded;
    }

    public void lateUpdate()
    {
        if (((!IN_GAME_MAIN_CAMERA.isPausing || (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)) && !this.rockLift) && ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine))
        {
            Quaternion to = Quaternion.Euler(GameObject.Find("MainCamera").transform.rotation.eulerAngles.x, GameObject.Find("MainCamera").transform.rotation.eulerAngles.y, 0f);
            GameObject.Find("MainCamera").transform.rotation = Quaternion.Lerp(GameObject.Find("MainCamera").transform.rotation, to, Time.deltaTime * 2f);
        }
    }

    public void loadskin()
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            string url = (string) FengGameManagerMKII.settings[0x41];
            if ((((int) FengGameManagerMKII.settings[1]) == 1) && ((url.EndsWith(".jpg") || url.EndsWith(".png")) || url.EndsWith(".jpeg")))
            {
                base.StartCoroutine(this.loadskinE(url));
            }
        }
        else if (base.photonView.isMine && (((int) FengGameManagerMKII.settings[1]) == 1))
        {
            photonView.RPC<string>(loadskinRPC, PhotonTargets.AllBuffered, (string) FengGameManagerMKII.settings[0x41]);
        }
    }

    public IEnumerator loadskinE(string url)
    {
        while (!this.hasSpawn)
        {
            yield return null;
        }
        bool mipmap = true;
        bool iteratorVariable1 = false;
        if (((int)FengGameManagerMKII.settings[0x3f]) == 1)
        {
            mipmap = false;
        }
        foreach (Renderer iteratorVariable4 in this.GetComponentsInChildren<Renderer>())
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
            base.StartCoroutine(this.loadskinE(url));
        }
    }

    [PunRPC]
    private void netCrossFade(string aniName, float time)
    {
        base.GetComponent<Animation>().CrossFade(aniName, time);
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

    [PunRPC]
    private void netTauntAttack(float tauntTime, float distance = 100f)
    {
        foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
        {
            if ((Vector3.Distance(obj2.transform.position, base.transform.position) < distance) && (obj2.GetComponent<MindlessTitan>() != null))
            {
                throw new NotImplementedException("Mindless Titans should be taunted by Eren Titan");
                //obj2.GetComponent<TITAN>().beTauntedBy(base.gameObject, tauntTime);
            }
            if (obj2.GetComponent<FEMALE_TITAN>() != null)
            {
                obj2.GetComponent<FEMALE_TITAN>().erenIsHere(base.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        if (GameObject.Find("MultiplayerManager") != null)
        {
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().removeET(this);
        }
    }

    public void playAnimation(string aniName)
    {
        base.GetComponent<Animation>().Play(aniName);
        if (PhotonNetwork.connected && base.photonView.isMine)
            photonView.RPC<string>(netPlayAnimation, PhotonTargets.Others, aniName);
    }

    private void playAnimationAt(string aniName, float normalizedTime)
    {
        GetComponent<Animation>().Play(aniName);
        GetComponent<Animation>()[aniName].normalizedTime = normalizedTime;
        if (PhotonNetwork.connected && photonView.isMine)
            photonView.RPC<string, float>(netPlayAnimationAt, PhotonTargets.Others, aniName, normalizedTime);
    }

    private void playSound(string sndname)
    {
        playsoundRPC(sndname);
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
            photonView.RPC<string>(playsoundRPC, PhotonTargets.Others, sndname);
    }

    [PunRPC]
    private void playsoundRPC(string sndname)
    {
        base.transform.Find(sndname).GetComponent<AudioSource>().Play();
    }

    [PunRPC]
    private void removeMe()
    {
        PhotonNetwork.RemoveRPCs(base.photonView);
        UnityEngine.Object.Destroy(base.gameObject);
    }

    [PunRPC]
    private void rockPlayAnimation(string anim)
    {
        this.rock.GetComponent<Animation>().Play(anim);
        this.rock.GetComponent<Animation>()[anim].speed = 1f;
    }

    private void RockUpdate()
    {
        if (!this.isHitWhileCarryingRock)
        {
            if (this.isROCKMOVE)
            {
                this.rock.transform.position = base.transform.position;
                this.rock.transform.rotation = base.transform.rotation;
            }
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
            {
                if (this.rockPhase == 0)
                {
                    base.GetComponent<Rigidbody>().AddForce(-base.GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);
                    base.GetComponent<Rigidbody>().AddForce(new Vector3(0f, -10f * base.GetComponent<Rigidbody>().mass, 0f));
                    this.waitCounter += Time.deltaTime;
                    if (this.waitCounter > 20f)
                    {
                        this.rockPhase++;
                        this.crossFade("idle", 1f);
                        this.waitCounter = 0f;
                        this.setRoute();
                    }
                }
                else if (this.rockPhase == 1)
                {
                    base.GetComponent<Rigidbody>().AddForce(-base.GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);
                    base.GetComponent<Rigidbody>().AddForce(new Vector3(0f, -this.gravity * base.GetComponent<Rigidbody>().mass, 0f));
                    this.waitCounter += Time.deltaTime;
                    if (this.waitCounter > 2f)
                    {
                        this.rockPhase++;
                        this.crossFade("run", 0.2f);
                        this.waitCounter = 0f;
                    }
                }
                else if (this.rockPhase == 2)
                {
                    Vector3 vector = (Vector3) (base.transform.forward * 30f);
                    Vector3 velocity = base.GetComponent<Rigidbody>().velocity;
                    Vector3 force = vector - velocity;
                    force.x = Mathf.Clamp(force.x, -this.maxVelocityChange, this.maxVelocityChange);
                    force.z = Mathf.Clamp(force.z, -this.maxVelocityChange, this.maxVelocityChange);
                    force.y = 0f;
                    base.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
                    if (base.transform.position.z < -238f)
                    {
                        base.transform.position = new Vector3(base.transform.position.x, 0f, -238f);
                        this.rockPhase++;
                        this.crossFade("idle", 0.2f);
                        this.waitCounter = 0f;
                    }
                }
                else if (this.rockPhase == 3)
                {
                    base.GetComponent<Rigidbody>().AddForce(-base.GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);
                    base.GetComponent<Rigidbody>().AddForce(new Vector3(0f, -10f * base.GetComponent<Rigidbody>().mass, 0f));
                    this.waitCounter += Time.deltaTime;
                    if (this.waitCounter > 1f)
                    {
                        this.rockPhase++;
                        this.crossFade("rock_lift", 0.1f);
                        photonView.RPC<string>(rockPlayAnimation, PhotonTargets.All, "lift");
                        this.waitCounter = 0f;
                        this.targetCheckPt = (Vector3) this.checkPoints[0];
                    }
                }
                else if (this.rockPhase == 4)
                {
                    base.GetComponent<Rigidbody>().AddForce(-base.GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);
                    base.GetComponent<Rigidbody>().AddForce(new Vector3(0f, -this.gravity * base.GetComponent<Rigidbody>().mass, 0f));
                    this.waitCounter += Time.deltaTime;
                    if (this.waitCounter > 4.2f)
                    {
                        this.rockPhase++;
                        this.crossFade("rock_walk", 0.1f);
                        photonView.RPC<string>(rockPlayAnimation, PhotonTargets.All, "move");
                        this.rock.GetComponent<Animation>()["move"].normalizedTime = base.GetComponent<Animation>()["rock_walk"].normalizedTime;
                        this.waitCounter = 0f;
                        photonView.RPC(startMovingRock, PhotonTargets.All);
                    }
                }
                else if (this.rockPhase == 5)
                {
                    if (Vector3.Distance(base.transform.position, this.targetCheckPt) < 10f)
                    {
                        if (this.checkPoints.Count > 0)
                        {
                            if (this.checkPoints.Count == 1)
                            {
                                this.rockPhase++;
                            }
                            else
                            {
                                Vector3 vector6 = (Vector3) this.checkPoints[0];
                                this.targetCheckPt = vector6;
                                this.checkPoints.RemoveAt(0);
                                GameObject[] objArray = GameObject.FindGameObjectsWithTag("titanRespawn2");
                                GameObject obj2 = GameObject.Find("titanRespawnTrost" + (7 - this.checkPoints.Count));
                                if (obj2 != null)
                                {
                                    foreach (GameObject obj3 in objArray)
                                    {
                                        if (obj3.transform.parent.gameObject == obj2)
                                        {
                                            throw new NotImplementedException("Eren titan requires punk disabling and titans to automatically start chasing");
                                            var obj4 = FengGameManagerMKII.instance.SpawnTitan(obj3.transform.position, obj3.transform.rotation);
                                            //obj4.GetComponent<MindlessTitan>().isAlarm = true;
                                            //obj4.GetComponent<MindlessTitan>().chaseDistance = 999999f;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.rockPhase++;
                        }
                    }
                    if ((this.checkPoints.Count > 0) && (UnityEngine.Random.Range(0, 0xbb8) < (10 - this.checkPoints.Count)))
                    {
                        Quaternion quaternion;
                        RaycastHit hit;
                        if (UnityEngine.Random.Range(0, 10) > 5)
                        {
                            quaternion = base.transform.rotation * Quaternion.Euler(0f, UnityEngine.Random.Range((float) 150f, (float) 210f), 0f);
                        }
                        else
                        {
                            quaternion = base.transform.rotation * Quaternion.Euler(0f, UnityEngine.Random.Range((float) -30f, (float) 30f), 0f);
                        }
                        Vector3 vector7 = (Vector3) (quaternion * new Vector3(UnityEngine.Random.Range((float) 100f, (float) 200f), 0f, 0f));
                        Vector3 position = base.transform.position + vector7;
                        LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("Ground");
                        float y = 0f;
                        if (Physics.Raycast(position + ((Vector3) (Vector3.up * 500f)), -Vector3.up, out hit, 1000f, mask2.value))
                        {
                            y = hit.point.y;
                        }
                        position += (Vector3) (Vector3.up * y);
                        throw new NotImplementedException("Eren titan requires punk disabling and titans to automatically start chasing");
                        //GameObject obj5 = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().spawnTitan(70, position, base.transform.rotation, false);
                        //obj5.GetComponent<TITAN>().isAlarm = true;
                        //obj5.GetComponent<TITAN>().chaseDistance = 999999f;
                    }
                    Vector3 vector10 = (Vector3) (base.transform.forward * 6f);
                    Vector3 vector11 = base.GetComponent<Rigidbody>().velocity;
                    Vector3 vector12 = vector10 - vector11;
                    vector12.x = Mathf.Clamp(vector12.x, -this.maxVelocityChange, this.maxVelocityChange);
                    vector12.z = Mathf.Clamp(vector12.z, -this.maxVelocityChange, this.maxVelocityChange);
                    vector12.y = 0f;
                    base.GetComponent<Rigidbody>().AddForce(vector12, ForceMode.VelocityChange);
                    base.GetComponent<Rigidbody>().AddForce(new Vector3(0f, -this.gravity * base.GetComponent<Rigidbody>().mass, 0f));
                    Vector3 vector13 = this.targetCheckPt - base.transform.position;
                    float current = -Mathf.Atan2(vector13.z, vector13.x) * 57.29578f;
                    float num4 = -Mathf.DeltaAngle(current, base.gameObject.transform.rotation.eulerAngles.y - 90f);
                    base.gameObject.transform.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, Quaternion.Euler(0f, base.gameObject.transform.rotation.eulerAngles.y + num4, 0f), 0.8f * Time.deltaTime);
                }
                else if (this.rockPhase == 6)
                {
                    base.GetComponent<Rigidbody>().AddForce(-base.GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);
                    base.GetComponent<Rigidbody>().AddForce(new Vector3(0f, -10f * base.GetComponent<Rigidbody>().mass, 0f));
                    base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    this.rockPhase++;
                    this.crossFade("rock_fix_hole", 0.1f);
                    base.photonView.RPC<string>(rockPlayAnimation, PhotonTargets.All, "set");
                    base.photonView.RPC(endMovingRock, PhotonTargets.All);
                }
                else if (this.rockPhase == 7)
                {
                    base.GetComponent<Rigidbody>().AddForce(-base.GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);
                    base.GetComponent<Rigidbody>().AddForce(new Vector3(0f, -10f * base.GetComponent<Rigidbody>().mass, 0f));
                    if (base.GetComponent<Animation>()["rock_fix_hole"].normalizedTime >= 1.2f)
                    {
                        this.crossFade("die", 0.1f);
                        this.rockPhase++;
                        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameWin2();
                    }
                    if ((base.GetComponent<Animation>()["rock_fix_hole"].normalizedTime >= 0.62f) && !this.rockHitGround)
                    {
                        this.rockHitGround = true;
                        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
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
        this.checkPoints = new ArrayList();
        for (int i = 1; i <= 7; i++)
        {
            this.checkPoints.Add(obj2.transform.Find("r" + i).position);
        }
        this.checkPoints.Add("end");
    }

    private void showAimUI()
    {
        GameObject obj2 = GameObject.Find("cross1");
        GameObject obj3 = GameObject.Find("cross2");
        GameObject obj4 = GameObject.Find("crossL1");
        GameObject obj5 = GameObject.Find("crossL2");
        GameObject obj6 = GameObject.Find("crossR1");
        GameObject obj7 = GameObject.Find("crossR2");
        GameObject obj8 = GameObject.Find("LabelDistance");
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
        //GameObject.Find("skill_cd_eren").GetComponent<UISprite>().fillAmount = this.lifeTime / this.lifeTimeMax;
    }

    private void Start()
    {
        this.loadskin();
        FengGameManagerMKII.instance.addET(this);
        if (this.rockLift)
        {
            this.rock = GameObject.Find("rock");
            this.rock.GetComponent<Animation>()["lift"].speed = 0f;
        }
        else
        {
            this.currentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            this.inputManager = GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();
            this.oldCorePosition = base.transform.position - base.transform.Find("Amarture/Core").position;
            this.myR = this.sqrt2 * 6f;
            base.GetComponent<Animation>()["hit_annie_1"].speed = 0.8f;
            base.GetComponent<Animation>()["hit_annie_2"].speed = 0.7f;
            base.GetComponent<Animation>()["hit_annie_3"].speed = 0.7f;
        }
        this.hasSpawn = true;
    }

    [PunRPC]
    private void startMovingRock()
    {
        this.isROCKMOVE = true;
    }

    public void update()
    {
        if ((!IN_GAME_MAIN_CAMERA.isPausing || (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)) && !this.rockLift)
        {
            if (base.GetComponent<Animation>().IsPlaying("run"))
            {
                if ((((base.GetComponent<Animation>()["run"].normalizedTime % 1f) > 0.3f) && ((base.GetComponent<Animation>()["run"].normalizedTime % 1f) < 0.75f)) && (this.stepSoundPhase == 2))
                {
                    this.stepSoundPhase = 1;
                    Transform transform = base.transform.Find("snd_eren_foot");
                    transform.GetComponent<AudioSource>().Stop();
                    transform.GetComponent<AudioSource>().Play();
                }
                if (((base.GetComponent<Animation>()["run"].normalizedTime % 1f) > 0.75f) && (this.stepSoundPhase == 1))
                {
                    this.stepSoundPhase = 2;
                    Transform transform2 = base.transform.Find("snd_eren_foot");
                    transform2.GetComponent<AudioSource>().Stop();
                    transform2.GetComponent<AudioSource>().Play();
                }
            }
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
            {
                if (this.hasDied)
                {
                    if ((base.GetComponent<Animation>()["die"].normalizedTime >= 1f) || (this.hitAnimation == "hit_annie_3"))
                    {
                        if (this.realBody != null)
                        {
                            this.realBody.GetComponent<Hero>().backToHuman();
                            this.realBody.transform.position = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position + ((Vector3)(Vector3.up * 2f));
                            this.realBody = null;
                        }
                        this.dieTime += Time.deltaTime;
                        if ((this.dieTime > 2f) && !this.hasDieSteam)
                        {
                            this.hasDieSteam = true;
                            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                            {
                                GameObject obj2 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/FXtitanDie1"));
                                obj2.transform.position = base.transform.Find("Amarture/Core/Controller_Body/hip").position;
                                obj2.transform.localScale = base.transform.localScale;
                            }
                            else if (base.photonView.isMine)
                            {
                                PhotonNetwork.Instantiate("FX/FXtitanDie1", base.transform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = base.transform.localScale;
                            }
                        }
                        if (this.dieTime > 5f)
                        {
                            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                            {
                                GameObject obj4 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/FXtitanDie"));
                                obj4.transform.position = base.transform.Find("Amarture/Core/Controller_Body/hip").position;
                                obj4.transform.localScale = base.transform.localScale;
                                UnityEngine.Object.Destroy(base.gameObject);
                            }
                            else if (base.photonView.isMine)
                            {
                                PhotonNetwork.Instantiate("FX/FXtitanDie", base.transform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = base.transform.localScale;
                                PhotonNetwork.Destroy(base.photonView);
                            }
                        }
                    }
                }
                else if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
                {
                    if (this.isHit)
                    {
                        if (base.GetComponent<Animation>()[this.hitAnimation].normalizedTime >= 1f)
                        {
                            this.isHit = false;
                            this.falseAttack();
                            this.playAnimation("idle");
                        }
                    }
                    else
                    {
                        if (this.lifeTime > 0f)
                        {
                            this.lifeTime -= Time.deltaTime;
                            if (this.lifeTime <= 0f)
                            {
                                this.hasDied = true;
                                this.playAnimation("die");
                                return;
                            }
                        }
                        if (((this.grounded && !this.isAttack) && (!base.GetComponent<Animation>().IsPlaying("jump_land") && !this.isAttack)) && !base.GetComponent<Animation>().IsPlaying("born"))
                        {
                            if (this.inputManager.isInputDown[InputCode.attack0] || this.inputManager.isInputDown[InputCode.attack1])
                            {
                                bool flag = false;
                                if (((GameCursor.CameraMode == CameraMode.WOW) && this.inputManager.isInput[InputCode.down]) || this.inputManager.isInputDown[InputCode.attack1])
                                {
                                    if (this.inputManager.isInputDown[InputCode.attack1] && this.inputManager.inputKey[11] == KeyCode.Mouse1)
                                        flag = true;
                                    
                                    if (!flag)
                                        this.attackAnimation = "attack_kick";
                                }
                                else
                                {
                                    this.attackAnimation = "attack_combo_001";
                                }
                                if (!flag)
                                {
                                    this.playAnimation(this.attackAnimation);
                                    base.GetComponent<Animation>()[this.attackAnimation].time = 0f;
                                    this.isAttack = true;
                                    this.needFreshCorePosition = true;
                                    if ((this.attackAnimation == "attack_combo_001") || (this.attackAnimation == "attack_combo_001"))
                                    {
                                        this.attackBox = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
                                    }
                                    else if (this.attackAnimation == "attack_combo_002")
                                    {
                                        this.attackBox = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
                                    }
                                    else if (this.attackAnimation == "attack_kick")
                                    {
                                        this.attackBox = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_R/shin_R/foot_R");
                                    }
                                    this.hitTargets = new ArrayList();
                                }
                            }
                            if (this.inputManager.isInputDown[InputCode.salute])
                            {
                                this.crossFade("born", 0.1f);
                                base.GetComponent<Animation>()["born"].normalizedTime = 0.28f;
                                this.isPlayRoar = false;
                            }
                        }
                        if (!this.isAttack)
                        {
                            if ((this.grounded || base.GetComponent<Animation>().IsPlaying("idle")) && ((!base.GetComponent<Animation>().IsPlaying("jump_start") && !base.GetComponent<Animation>().IsPlaying("jump_air")) && (!base.GetComponent<Animation>().IsPlaying("jump_land") && this.inputManager.isInput[InputCode.bothRope])))
                            {
                                this.crossFade("jump_start", 0.1f);
                            }
                        }
                        else
                        {
                            if ((base.GetComponent<Animation>()[this.attackAnimation].time >= 0.1f) && this.inputManager.isInputDown[InputCode.attack0])
                            {
                                this.isNextAttack = true;
                            }
                            float num = 0f;
                            float num2 = 0f;
                            float num3 = 0f;
                            string str = string.Empty;
                            if (this.attackAnimation == "attack_combo_001")
                            {
                                num = 0.4f;
                                num2 = 0.5f;
                                num3 = 0.66f;
                                str = "attack_combo_002";
                            }
                            else if (this.attackAnimation == "attack_combo_002")
                            {
                                num = 0.15f;
                                num2 = 0.25f;
                                num3 = 0.43f;
                                str = "attack_combo_003";
                            }
                            else if (this.attackAnimation == "attack_combo_003")
                            {
                                num3 = 0f;
                                num = 0.31f;
                                num2 = 0.37f;
                            }
                            else if (this.attackAnimation == "attack_kick")
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
                            if (this.hitPause > 0f)
                            {
                                this.hitPause -= Time.deltaTime;
                                if (this.hitPause <= 0f)
                                {
                                    base.GetComponent<Animation>()[this.attackAnimation].speed = 1f;
                                    this.hitPause = 0f;
                                }
                            }
                            if (((num3 > 0f) && this.isNextAttack) && (base.GetComponent<Animation>()[this.attackAnimation].normalizedTime >= num3))
                            {
                                if (this.hitTargets.Count > 0)
                                {
                                    Transform transform3 = (Transform)this.hitTargets[0];
                                    if (transform3 != null)
                                    {
                                        base.transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(transform3.position - base.transform.position).eulerAngles.y, 0f);
                                        this.facingDirection = base.transform.rotation.eulerAngles.y;
                                    }
                                }
                                this.falseAttack();
                                this.attackAnimation = str;
                                this.crossFade(this.attackAnimation, 0.1f);
                                base.GetComponent<Animation>()[this.attackAnimation].time = 0f;
                                base.GetComponent<Animation>()[this.attackAnimation].speed = 1f;
                                this.isAttack = true;
                                this.needFreshCorePosition = true;
                                if (this.attackAnimation == "attack_combo_002")
                                {
                                    this.attackBox = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
                                }
                                else if (this.attackAnimation == "attack_combo_003")
                                {
                                    this.attackBox = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
                                }
                                this.hitTargets = new ArrayList();
                            }
                            if (((base.GetComponent<Animation>()[this.attackAnimation].normalizedTime >= num) && (base.GetComponent<Animation>()[this.attackAnimation].normalizedTime <= num2)) || (!this.attackChkOnce && (base.GetComponent<Animation>()[this.attackAnimation].normalizedTime >= num)))
                            {
                                if (!this.attackChkOnce)
                                {
                                    if (this.attackAnimation == "attack_combo_002")
                                    {
                                        this.playSound("snd_eren_swing2");
                                    }
                                    else if (this.attackAnimation == "attack_combo_001")
                                    {
                                        this.playSound("snd_eren_swing1");
                                    }
                                    else if (this.attackAnimation == "attack_combo_003")
                                    {
                                        this.playSound("snd_eren_swing3");
                                    }
                                    this.attackChkOnce = true;
                                }
                                Collider[] colliderArray = Physics.OverlapSphere(this.attackBox.transform.position, 8f);
                                for (int i = 0; i < colliderArray.Length; i++)
                                {
                                    throw new NotImplementedException("Mindless Titans are not supported for Eren Titan");
                                    //if (colliderArray[i].gameObject.transform.root.GetComponent<TITAN>() == null)
                                    //{
                                    //    continue;
                                    //}
                                    //bool flag2 = false;
                                    //for (int j = 0; j < this.hitTargets.Count; j++)
                                    //{
                                    //    if (colliderArray[i].gameObject.transform.root == (Transform)this.hitTargets[j])
                                    //    {
                                    //        flag2 = true;
                                    //        break;
                                    //    }
                                    //}
                                    //if (!flag2 && !colliderArray[i].gameObject.transform.root.GetComponent<TITAN>().hasDie)
                                    //{
                                    //    base.GetComponent<Animation>()[this.attackAnimation].speed = 0f;
                                    //    if (this.attackAnimation == "attack_combo_002")
                                    //    {
                                    //        this.hitPause = 0.05f;
                                    //        colliderArray[i].gameObject.transform.root.GetComponent<TITAN>().hitL(base.transform.position, this.hitPause);
                                    //        this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(1f, 0.03f, 0.95f);
                                    //    }
                                    //    else if (this.attackAnimation == "attack_combo_001")
                                    //    {
                                    //        this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(1.2f, 0.04f, 0.95f);
                                    //        this.hitPause = 0.08f;
                                    //        colliderArray[i].gameObject.transform.root.GetComponent<TITAN>().hitR(base.transform.position, this.hitPause);
                                    //    }
                                    //    else if (this.attackAnimation == "attack_combo_003")
                                    //    {
                                    //        this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(3f, 0.1f, 0.95f);
                                    //        this.hitPause = 0.3f;
                                    //        colliderArray[i].gameObject.transform.root.GetComponent<TITAN>().dieHeadBlow(base.transform.position, this.hitPause);
                                    //    }
                                    //    else if (this.attackAnimation == "attack_kick")
                                    //    {
                                    //        this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(3f, 0.1f, 0.95f);
                                    //        this.hitPause = 0.2f;
                                    //        if (colliderArray[i].gameObject.transform.root.GetComponent<TITAN>().TitanType == TitanType.TYPE_CRAWLER)
                                    //        {
                                    //            colliderArray[i].gameObject.transform.root.GetComponent<TITAN>().dieBlow(base.transform.position, this.hitPause);
                                    //        }
                                    //        else if (colliderArray[i].gameObject.transform.root.transform.localScale.x < 2f)
                                    //        {
                                    //            colliderArray[i].gameObject.transform.root.GetComponent<TITAN>().dieBlow(base.transform.position, this.hitPause);
                                    //        }
                                    //        else
                                    //        {
                                    //            colliderArray[i].gameObject.transform.root.GetComponent<TITAN>().hitR(base.transform.position, this.hitPause);
                                    //        }
                                    //    }
                                    //    this.hitTargets.Add(colliderArray[i].gameObject.transform.root);
                                    //    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                                    //    {
                                    //        PhotonNetwork.Instantiate("hitMeatBIG", (Vector3)((colliderArray[i].transform.position + this.attackBox.position) * 0.5f), Quaternion.Euler(270f, 0f, 0f), 0);
                                    //    }
                                    //    else
                                    //    {
                                    //        UnityEngine.Object.Instantiate(Resources.Load("hitMeatBIG"), (Vector3)((colliderArray[i].transform.position + this.attackBox.position) * 0.5f), Quaternion.Euler(270f, 0f, 0f));
                                    //    }
                                    //}
                                }
                            }
                            if (base.GetComponent<Animation>()[this.attackAnimation].normalizedTime >= 1f)
                            {
                                this.falseAttack();
                                this.playAnimation("idle");
                            }
                        }
                        if (base.GetComponent<Animation>().IsPlaying("jump_land") && (base.GetComponent<Animation>()["jump_land"].normalizedTime >= 1f))
                        {
                            this.crossFade("idle", 0.1f);
                        }
                        if (base.GetComponent<Animation>().IsPlaying("born"))
                        {
                            if ((base.GetComponent<Animation>()["born"].normalizedTime >= 0.28f) && !this.isPlayRoar)
                            {
                                this.isPlayRoar = true;
                                this.playSound("snd_eren_roar");
                            }
                            if ((GetComponent<Animation>()["born"].normalizedTime >= 0.5f) && (GetComponent<Animation>()["born"].normalizedTime <= 0.7f))
                            {
                                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.5f, 1f, 0.95f);
                            }
                            if (GetComponent<Animation>()["born"].normalizedTime >= 1f)
                            {
                                crossFade("idle", 0.1f);
                                if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                                {
                                    if (PhotonNetwork.isMasterClient)
                                        photonView.RPC<float, float>(netTauntAttack, PhotonTargets.MasterClient, 10f, 500f);
                                    else
                                        netTauntAttack(10f, 500f);
                                }
                                else
                                    netTauntAttack(10f, 500f);
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