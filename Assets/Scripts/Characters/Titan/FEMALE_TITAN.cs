using Assets.Scripts.Gamemode.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;

public class FEMALE_TITAN : MonoBehaviour
{
    [CompilerGenerated]
    public static Dictionary<string, int> f__switchSmap1;
    [CompilerGenerated]
    public static Dictionary<string, int> f__switchSmap2;
    [CompilerGenerated]
    public static Dictionary<string, int> f__switchSmap3;
    private Vector3 abnorma_jump_bite_horizon_v;
    public int AnkleLHP = 200;
    private int AnkleLHPMAX = 200;
    public int AnkleRHP = 200;
    private int AnkleRHPMAX = 200;
    private string attackAnimation;
    private float attackCheckTime;
    private float attackCheckTimeA;
    private float attackCheckTimeB;
    private bool attackChkOnce;
    public float attackDistance = 13f;
    private bool attacked;
    public float attackWait = 1f;
    private float attention = 10f;
    public GameObject bottomObject;
    public float chaseDistance = 80f;
    private Transform checkHitCapsuleEnd;
    private Vector3 checkHitCapsuleEndOld;
    private float checkHitCapsuleR;
    private Transform checkHitCapsuleStart;
    public GameObject currentCamera;
    private Transform currentGrabHand;
    private float desDeg;
    private float dieTime;
    private GameObject eren;
    [CompilerGenerated]
    public static Dictionary<string, int> f__switchsmap1;
    [CompilerGenerated]
    public static Dictionary<string, int> f__switchsmap2;
    [CompilerGenerated]
    public static Dictionary<string, int> f__switchsmap3;
    private string fxName;
    private Vector3 fxPosition;
    private Quaternion fxRotation;
    private GameObject grabbedTarget;
    public GameObject grabTF;
    private float gravity = 120f;
    private bool grounded;
    public bool hasDie;
    private bool hasDieSteam;
    public bool hasspawn;
    public GameObject healthLabel;
    public float healthTime;
    public string hitAnimation;
    private bool isAttackMoveByCore;
    private bool isGrabHandLeft;
    public float lagMax;
    public int maxHealth;
    public float maxVelocityChange = 80f;
    public static float minusDistance = 99999f;
    public static GameObject minusDistanceEnemy;
    public float myDistance;
    public GameObject myHero;
    public int NapeArmor = 0x3e8;
    private bool needFreshCorePosition;
    private string nextAttackAnimation;
    private Vector3 oldCorePosition;
    private float sbtime;
    public float size;
    public float speed = 80f;
    private bool startJump;
    private string state = "idle";
    private int stepSoundPhase = 2;
    private float tauntTime;
    private string turnAnimation;
    private float turnDeg;
    private GameObject whoHasTauntMe;

    private void attack(string type)
    {
        state = "attack";
        attacked = false;
        if (attackAnimation == type)
        {
            attackAnimation = type;
            playAnimationAt("ft_attack_" + type, 0f);
        }
        else
        {
            attackAnimation = type;
            playAnimationAt("ft_attack_" + type, 0f);
        }
        startJump = false;
        attackChkOnce = false;
        nextAttackAnimation = null;
        fxName = null;
        isAttackMoveByCore = false;
        attackCheckTime = 0f;
        attackCheckTimeA = 0f;
        attackCheckTimeB = 0f;
        fxRotation = Quaternion.Euler(270f, 0f, 0f);
        string key = type;
        if (key != null)
        {
            int num;
            if (f__switchSmap2 == null)
            {
                f__switchSmap2 = new Dictionary<string, int>(0x11);
                f__switchSmap2.Add("combo_1", 0);
                f__switchSmap2.Add("combo_2", 1);
                f__switchSmap2.Add("combo_3", 2);
                f__switchSmap2.Add("combo_blind_1", 3);
                f__switchSmap2.Add("combo_blind_2", 4);
                f__switchSmap2.Add("combo_blind_3", 5);
                f__switchSmap2.Add("front", 6);
                f__switchSmap2.Add("jumpCombo_1", 7);
                f__switchSmap2.Add("jumpCombo_2", 8);
                f__switchSmap2.Add("jumpCombo_3", 9);
                f__switchSmap2.Add("jumpCombo_4", 10);
                f__switchSmap2.Add("sweep", 11);
                f__switchSmap2.Add("sweep_back", 12);
                f__switchSmap2.Add("sweep_front_left", 13);
                f__switchSmap2.Add("sweep_front_right", 14);
                f__switchSmap2.Add("sweep_head_b_l", 15);
                f__switchSmap2.Add("sweep_head_b_r", 0x10);
            }
            if (f__switchSmap2.TryGetValue(key, out num))
            {
                switch (num)
                {
                    case 0:
                        attackCheckTimeA = 0.63f;
                        attackCheckTimeB = 0.8f;
                        checkHitCapsuleEnd = transform.Find("Amarture/Core/Controller_Body/hip/thigh_R/shin_R/foot_R");
                        checkHitCapsuleStart = transform.Find("Amarture/Core/Controller_Body/hip/thigh_R");
                        checkHitCapsuleR = 5f;
                        isAttackMoveByCore = true;
                        nextAttackAnimation = "combo_2";
                        break;

                    case 1:
                        attackCheckTimeA = 0.27f;
                        attackCheckTimeB = 0.43f;
                        checkHitCapsuleEnd = transform.Find("Amarture/Core/Controller_Body/hip/thigh_L/shin_L/foot_L");
                        checkHitCapsuleStart = transform.Find("Amarture/Core/Controller_Body/hip/thigh_L");
                        checkHitCapsuleR = 5f;
                        isAttackMoveByCore = true;
                        nextAttackAnimation = "combo_3";
                        break;

                    case 2:
                        attackCheckTimeA = 0.15f;
                        attackCheckTimeB = 0.3f;
                        checkHitCapsuleEnd = transform.Find("Amarture/Core/Controller_Body/hip/thigh_R/shin_R/foot_R");
                        checkHitCapsuleStart = transform.Find("Amarture/Core/Controller_Body/hip/thigh_R");
                        checkHitCapsuleR = 5f;
                        isAttackMoveByCore = true;
                        break;

                    case 3:
                        isAttackMoveByCore = true;
                        attackCheckTimeA = 0.72f;
                        attackCheckTimeB = 0.83f;
                        checkHitCapsuleStart = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
                        checkHitCapsuleEnd = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
                        checkHitCapsuleR = 4f;
                        nextAttackAnimation = "combo_blind_2";
                        break;

                    case 4:
                        isAttackMoveByCore = true;
                        attackCheckTimeA = 0.5f;
                        attackCheckTimeB = 0.6f;
                        checkHitCapsuleStart = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
                        checkHitCapsuleEnd = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
                        checkHitCapsuleR = 4f;
                        nextAttackAnimation = "combo_blind_3";
                        break;

                    case 5:
                        isAttackMoveByCore = true;
                        attackCheckTimeA = 0.2f;
                        attackCheckTimeB = 0.28f;
                        checkHitCapsuleStart = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
                        checkHitCapsuleEnd = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
                        checkHitCapsuleR = 4f;
                        break;

                    case 6:
                        isAttackMoveByCore = true;
                        attackCheckTimeA = 0.44f;
                        attackCheckTimeB = 0.55f;
                        checkHitCapsuleStart = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
                        checkHitCapsuleEnd = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
                        checkHitCapsuleR = 4f;
                        break;

                    case 7:
                        isAttackMoveByCore = false;
                        nextAttackAnimation = "jumpCombo_2";
                        abnorma_jump_bite_horizon_v = Vector3.zero;
                        break;

                    case 8:
                        isAttackMoveByCore = false;
                        attackCheckTimeA = 0.48f;
                        attackCheckTimeB = 0.7f;
                        checkHitCapsuleStart = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
                        checkHitCapsuleEnd = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
                        checkHitCapsuleR = 4f;
                        nextAttackAnimation = "jumpCombo_3";
                        break;

                    case 9:
                        isAttackMoveByCore = false;
                        checkHitCapsuleEnd = transform.Find("Amarture/Core/Controller_Body/hip/thigh_L/shin_L/foot_L");
                        checkHitCapsuleStart = transform.Find("Amarture/Core/Controller_Body/hip/thigh_L");
                        checkHitCapsuleR = 5f;
                        attackCheckTimeA = 0.22f;
                        attackCheckTimeB = 0.42f;
                        break;

                    case 10:
                        isAttackMoveByCore = false;
                        break;

                    case 11:
                        isAttackMoveByCore = true;
                        attackCheckTimeA = 0.39f;
                        attackCheckTimeB = 0.6f;
                        checkHitCapsuleEnd = transform.Find("Amarture/Core/Controller_Body/hip/thigh_R/shin_R/foot_R");
                        checkHitCapsuleStart = transform.Find("Amarture/Core/Controller_Body/hip/thigh_R");
                        checkHitCapsuleR = 5f;
                        break;

                    case 12:
                        isAttackMoveByCore = true;
                        attackCheckTimeA = 0.41f;
                        attackCheckTimeB = 0.48f;
                        checkHitCapsuleStart = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
                        checkHitCapsuleEnd = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
                        checkHitCapsuleR = 4f;
                        break;

                    case 13:
                        isAttackMoveByCore = true;
                        attackCheckTimeA = 0.53f;
                        attackCheckTimeB = 0.63f;
                        checkHitCapsuleStart = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
                        checkHitCapsuleEnd = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
                        checkHitCapsuleR = 4f;
                        break;

                    case 14:
                        isAttackMoveByCore = true;
                        attackCheckTimeA = 0.5f;
                        attackCheckTimeB = 0.62f;
                        checkHitCapsuleStart = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L");
                        checkHitCapsuleEnd = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
                        checkHitCapsuleR = 4f;
                        break;

                    case 15:
                        isAttackMoveByCore = true;
                        attackCheckTimeA = 0.4f;
                        attackCheckTimeB = 0.51f;
                        checkHitCapsuleStart = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L");
                        checkHitCapsuleEnd = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
                        checkHitCapsuleR = 4f;
                        break;

                    case 0x10:
                        isAttackMoveByCore = true;
                        attackCheckTimeA = 0.4f;
                        attackCheckTimeB = 0.51f;
                        checkHitCapsuleStart = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
                        checkHitCapsuleEnd = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
                        checkHitCapsuleR = 4f;
                        break;
                }
            }
        }
        checkHitCapsuleEndOld = checkHitCapsuleEnd.transform.position;
        needFreshCorePosition = true;
    }

    private bool attackTarget(GameObject target)
    {
        int num5;
        float current = 0f;
        float f = 0f;
        Vector3 vector = target.transform.position - transform.position;
        current = -Mathf.Atan2(vector.z, vector.x) * 57.29578f;
        f = -Mathf.DeltaAngle(current, gameObject.transform.rotation.eulerAngles.y - 90f);
        if ((eren != null) && (myDistance < 35f))
        {
            attack("combo_1");
            return true;
        }
        int num3 = 0;
        string str = string.Empty;
        ArrayList list = new ArrayList();
        if (myDistance < 40f)
        {
            if (Mathf.Abs(f) < 90f)
            {
                if (f > 0f)
                {
                    num3 = 1;
                }
                else
                {
                    num3 = 2;
                }
            }
            else if (f > 0f)
            {
                num3 = 4;
            }
            else
            {
                num3 = 3;
            }
            float num4 = target.transform.position.y - transform.position.y;
            if (Mathf.Abs(f) < 90f)
            {
                if (((num4 > 0f) && (num4 < 12f)) && (myDistance < 22f))
                {
                    list.Add("attack_sweep");
                }
                if ((num4 >= 55f) && (num4 < 90f))
                {
                    list.Add("attack_jumpCombo_1");
                }
            }
            if (((Mathf.Abs(f) < 90f) && (num4 > 12f)) && (num4 < 40f))
            {
                list.Add("attack_combo_1");
            }
            if (Mathf.Abs(f) < 30f)
            {
                if (((num4 > 0f) && (num4 < 12f)) && ((myDistance > 20f) && (myDistance < 30f)))
                {
                    list.Add("attack_front");
                }
                if (((myDistance < 12f) && (num4 > 33f)) && (num4 < 51f))
                {
                    list.Add("grab_up");
                }
            }
            if (((Mathf.Abs(f) > 100f) && (myDistance < 11f)) && ((num4 >= 15f) && (num4 < 32f)))
            {
                list.Add("attack_sweep_back");
            }
            num5 = num3;
            switch (num5)
            {
                case 1:
                    if (myDistance >= 11f)
                    {
                        if (myDistance < 20f)
                        {
                            if ((num4 >= 12f) && (num4 < 21f))
                            {
                                list.Add("grab_bottom_right");
                            }
                            else if ((num4 >= 21f) && (num4 < 32f))
                            {
                                list.Add("grab_mid_right");
                            }
                            else if ((num4 >= 32f) && (num4 < 47f))
                            {
                                list.Add("grab_up_right");
                            }
                        }
                        break;
                    }
                    if ((num4 >= 21f) && (num4 < 32f))
                    {
                        list.Add("attack_sweep_front_right");
                    }
                    break;

                case 2:
                    if (myDistance >= 11f)
                    {
                        if (myDistance < 20f)
                        {
                            if ((num4 >= 12f) && (num4 < 21f))
                            {
                                list.Add("grab_bottom_left");
                            }
                            else if ((num4 >= 21f) && (num4 < 32f))
                            {
                                list.Add("grab_mid_left");
                            }
                            else if ((num4 >= 32f) && (num4 < 47f))
                            {
                                list.Add("grab_up_left");
                            }
                        }
                        break;
                    }
                    if ((num4 >= 21f) && (num4 < 32f))
                    {
                        list.Add("attack_sweep_front_left");
                    }
                    break;

                case 3:
                    if (myDistance >= 11f)
                    {
                        list.Add("turn180");
                        break;
                    }
                    if ((num4 >= 33f) && (num4 < 51f))
                    {
                        list.Add("attack_sweep_head_b_l");
                    }
                    break;

                case 4:
                    if (myDistance >= 11f)
                    {
                        list.Add("turn180");
                        break;
                    }
                    if ((num4 >= 33f) && (num4 < 51f))
                    {
                        list.Add("attack_sweep_head_b_r");
                    }
                    break;
            }
        }
        if (list.Count > 0)
        {
            str = (string) list[UnityEngine.Random.Range(0, list.Count)];
        }
        else if (UnityEngine.Random.Range(0, 100) < 10)
        {
            GameObject[] objArray = GameObject.FindGameObjectsWithTag("Player");
            myHero = objArray[UnityEngine.Random.Range(0, objArray.Length)];
            attention = UnityEngine.Random.Range((float) 5f, (float) 10f);
            return true;
        }
        string key = str;
        if (key != null)
        {
            if (f__switchSmap1 == null)
            {
                f__switchSmap1 = new Dictionary<string, int>(0x11);
                f__switchSmap1.Add("grab_bottom_left", 0);
                f__switchSmap1.Add("grab_bottom_right", 1);
                f__switchSmap1.Add("grab_mid_left", 2);
                f__switchSmap1.Add("grab_mid_right", 3);
                f__switchSmap1.Add("grab_up", 4);
                f__switchSmap1.Add("grab_up_left", 5);
                f__switchSmap1.Add("grab_up_right", 6);
                f__switchSmap1.Add("ft_attack_combo_1", 7);
                f__switchSmap1.Add("attack_front", 8);
                f__switchSmap1.Add("attack_jumpCombo_1", 9);
                f__switchSmap1.Add("attack_sweep", 10);
                f__switchSmap1.Add("attack_sweep_back", 11);
                f__switchSmap1.Add("attack_sweep_front_left", 12);
                f__switchSmap1.Add("attack_sweep_front_right", 13);
                f__switchSmap1.Add("attack_sweep_head_b_l", 14);
                f__switchSmap1.Add("attack_sweep_head_b_r", 15);
                f__switchSmap1.Add("turn180", 0x10);
            }
            if (f__switchSmap1.TryGetValue(key, out num5))
            {
                switch (num5)
                {
                    case 0:
                        grab("bottom_left");
                        return true;

                    case 1:
                        grab("bottom_right");
                        return true;

                    case 2:
                        grab("mid_left");
                        return true;

                    case 3:
                        grab("mid_right");
                        return true;

                    case 4:
                        grab("up");
                        return true;

                    case 5:
                        grab("up_left");
                        return true;

                    case 6:
                        grab("up_right");
                        return true;

                    case 7:
                        attack("combo_1");
                        return true;

                    case 8:
                        attack("front");
                        return true;

                    case 9:
                        attack("jumpCombo_1");
                        return true;

                    case 10:
                        attack("sweep");
                        return true;

                    case 11:
                        attack("sweep_back");
                        return true;

                    case 12:
                        attack("sweep_front_left");
                        return true;

                    case 13:
                        attack("sweep_front_right");
                        return true;

                    case 14:
                        attack("sweep_head_b_l");
                        return true;

                    case 15:
                        attack("sweep_head_b_r");
                        return true;

                    case 0x10:
                        turn180();
                        return true;
                }
            }
        }
        return false;
    }

    private void Awake()
    {
        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    public void beTauntedBy(GameObject target, float tauntTime)
    {
        whoHasTauntMe = target;
        this.tauntTime = tauntTime;
    }

    private void chase()
    {
        state = "chase";
        crossFade("ft_run", 0.5f);
    }

    private RaycastHit[] checkHitCapsule(Vector3 start, Vector3 end, float r)
    {
        return Physics.SphereCastAll(start, r, end - start, Vector3.Distance(start, end));
    }

    private GameObject checkIfHitHand(Transform hand)
    {
        foreach (Collider collider in Physics.OverlapSphere(hand.GetComponent<SphereCollider>().transform.position, 10.6f))
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

    private GameObject checkIfHitHead(Transform head, float rad)
    {
        float num = rad * 4f;
        foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("Player"))
        {
            if ((obj2.GetComponent<TITAN_EREN>() == null) && !obj2.GetComponent<Hero>().isInvincible())
            {
                float num3 = obj2.GetComponent<CapsuleCollider>().height * 0.5f;
                if (Vector3.Distance(obj2.transform.position + ((Vector3) (Vector3.up * num3)), head.transform.position + ((Vector3) ((Vector3.up * 1.5f) * 4f))) < (num + num3))
                {
                    return obj2;
                }
            }
        }
        return null;
    }

    private void crossFade(string aniName, float time)
    {
        GetComponent<Animation>().CrossFade(aniName, time);
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
            photonView.RPC<string, float>(netCrossFade, PhotonTargets.Others, aniName, time);
    }

    private void eatSet(GameObject grabTarget)
    {
        var heroToGrab = grabTarget.GetComponent<Hero>();
        if (!heroToGrab.isGrabbed)
        {
            grabToRight();
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
            {
                heroToGrab.photonView.RPC<int, bool>(heroToGrab.netGrabbed, PhotonTargets.All, photonView.viewID, false);
                heroToGrab.photonView.RPC<string>(heroToGrab.netPlayAnimation, PhotonTargets.All, "grabbed");
                photonView.RPC(grabToRight, PhotonTargets.Others);
            }
            else
            {
                heroToGrab.grabbed(gameObject, false);
                heroToGrab.GetComponent<Animation>().Play("grabbed");
            }
        }
    }

    private void eatSetL(GameObject grabTarget)
    {
        var grabbedHero = grabTarget.GetComponent<Hero>();
        if (!grabbedHero.isGrabbed)
        {
            grabToLeft();
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
            {
                grabbedHero.photonView.RPC<int, bool>(grabbedHero.netGrabbed, PhotonTargets.All, photonView.viewID, true);
                grabbedHero.photonView.RPC<string>(grabbedHero.netPlayAnimation, PhotonTargets.All, "grabbed");
                photonView.RPC(grabToLeft, PhotonTargets.Others);
            }
            else
            {
                grabbedHero.grabbed(gameObject, true);
                grabbedHero.GetComponent<Animation>().Play("grabbed");
            }
        }
    }

    public void erenIsHere(GameObject target)
    {
        myHero = eren = target;
    }

    private void findNearestHero()
    {
        myHero = getNearestHero();
        attention = UnityEngine.Random.Range((float) 5f, (float) 10f);
    }

    private void FixedUpdate()
    {
        if ((!IN_GAME_MAIN_CAMERA.isPausing || (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)) && ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || photonView.isMine))
        {
            if (bottomObject.GetComponent<CheckHitGround>().isGrounded)
            {
                grounded = true;
                bottomObject.GetComponent<CheckHitGround>().isGrounded = false;
            }
            else
            {
                grounded = false;
            }
            if (needFreshCorePosition)
            {
                oldCorePosition = transform.position - transform.Find("Amarture/Core").position;
                needFreshCorePosition = false;
            }
            if (((state != "attack") || !isAttackMoveByCore) && (((state != "hit") && (state != "turn180")) && (state != "anklehurt")))
            {
                if (state == "chase")
                {
                    if (myHero == null)
                    {
                        return;
                    }
                    Vector3 vector3 = (Vector3) (transform.forward * speed);
                    Vector3 velocity = GetComponent<Rigidbody>().velocity;
                    Vector3 force = vector3 - velocity;
                    force.y = 0f;
                    GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
                    float current = 0f;
                    Vector3 vector6 = myHero.transform.position - transform.position;
                    current = -Mathf.Atan2(vector6.z, vector6.x) * 57.29578f;
                    float num2 = -Mathf.DeltaAngle(current, gameObject.transform.rotation.eulerAngles.y - 90f);
                    gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, gameObject.transform.rotation.eulerAngles.y + num2, 0f), speed * Time.deltaTime);
                }
                else if (grounded && !GetComponent<Animation>().IsPlaying("ft_attack_jumpCombo_1"))
                {
                    GetComponent<Rigidbody>().AddForce(new Vector3(-GetComponent<Rigidbody>().velocity.x, 0f, -GetComponent<Rigidbody>().velocity.z), ForceMode.VelocityChange);
                }
            }
            else
            {
                Vector3 vector = (transform.position - transform.Find("Amarture/Core").position) - oldCorePosition;
                GetComponent<Rigidbody>().velocity = (Vector3) ((vector / Time.deltaTime) + (Vector3.up * GetComponent<Rigidbody>().velocity.y));
                oldCorePosition = transform.position - transform.Find("Amarture/Core").position;
            }
            GetComponent<Rigidbody>().AddForce(new Vector3(0f, -gravity * GetComponent<Rigidbody>().mass, 0f));
        }
    }

    private void getDown()
    {
        state = "anklehurt";
        playAnimation("ft_legHurt");
        AnkleRHP = AnkleRHPMAX;
        AnkleLHP = AnkleLHPMAX;
        needFreshCorePosition = true;
    }

    private GameObject getNearestHero()
    {
        GameObject[] objArray = GameObject.FindGameObjectsWithTag("Player");
        GameObject obj2 = null;
        float positiveInfinity = float.PositiveInfinity;
        Vector3 position = transform.position;
        foreach (GameObject obj3 in objArray)
        {
            if (((obj3.GetComponent<Hero>() == null) || !obj3.GetComponent<Hero>().HasDied()) && ((obj3.GetComponent<TITAN_EREN>() == null) || !obj3.GetComponent<TITAN_EREN>().hasDied))
            {
                Vector3 vector2 = obj3.transform.position - position;
                float sqrMagnitude = vector2.sqrMagnitude;
                if (sqrMagnitude < positiveInfinity)
                {
                    obj2 = obj3;
                    positiveInfinity = sqrMagnitude;
                }
            }
        }
        return obj2;
    }

    private float getNearestHeroDistance()
    {
        GameObject[] objArray = GameObject.FindGameObjectsWithTag("Player");
        float positiveInfinity = float.PositiveInfinity;
        Vector3 position = transform.position;
        foreach (GameObject obj2 in objArray)
        {
            Vector3 vector2 = obj2.transform.position - position;
            float magnitude = vector2.magnitude;
            if (magnitude < positiveInfinity)
            {
                positiveInfinity = magnitude;
            }
        }
        return positiveInfinity;
    }

    private void grab(string type)
    {
        state = "grab";
        attacked = false;
        attackAnimation = type;
        if (GetComponent<Animation>().IsPlaying("ft_attack_grab_" + type))
        {
            GetComponent<Animation>()["ft_attack_grab_" + type].normalizedTime = 0f;
            playAnimation("ft_attack_grab_" + type);
        }
        else
        {
            crossFade("ft_attack_grab_" + type, 0.1f);
        }
        isGrabHandLeft = true;
        grabbedTarget = null;
        attackCheckTime = 0f;
        string key = type;
        if (key != null)
        {
            int num;
            if (f__switchSmap3 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(7);
                dictionary.Add("bottom_left", 0);
                dictionary.Add("bottom_right", 1);
                dictionary.Add("mid_left", 2);
                dictionary.Add("mid_right", 3);
                dictionary.Add("up", 4);
                dictionary.Add("up_left", 5);
                dictionary.Add("up_right", 6);
                f__switchSmap3 = dictionary;
            }
            if (f__switchSmap3.TryGetValue(key, out num))
            {
                switch (num)
                {
                    case 0:
                        attackCheckTimeA = 0.28f;
                        attackCheckTimeB = 0.38f;
                        attackCheckTime = 0.65f;
                        isGrabHandLeft = false;
                        break;

                    case 1:
                        attackCheckTimeA = 0.27f;
                        attackCheckTimeB = 0.37f;
                        attackCheckTime = 0.65f;
                        break;

                    case 2:
                        attackCheckTimeA = 0.27f;
                        attackCheckTimeB = 0.37f;
                        attackCheckTime = 0.65f;
                        isGrabHandLeft = false;
                        break;

                    case 3:
                        attackCheckTimeA = 0.27f;
                        attackCheckTimeB = 0.36f;
                        attackCheckTime = 0.66f;
                        break;

                    case 4:
                        attackCheckTimeA = 0.25f;
                        attackCheckTimeB = 0.32f;
                        attackCheckTime = 0.67f;
                        break;

                    case 5:
                        attackCheckTimeA = 0.26f;
                        attackCheckTimeB = 0.4f;
                        attackCheckTime = 0.66f;
                        break;

                    case 6:
                        attackCheckTimeA = 0.26f;
                        attackCheckTimeB = 0.4f;
                        attackCheckTime = 0.66f;
                        isGrabHandLeft = false;
                        break;
                }
            }
        }
        if (isGrabHandLeft)
        {
            currentGrabHand = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
        }
        else
        {
            currentGrabHand = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
        }
    }

    [PunRPC]
    public void grabbedTargetEscape()
    {
        grabbedTarget = null;
    }

    [PunRPC]
    public void grabToLeft()
    {
        Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
        grabTF.transform.parent = transform;
        grabTF.transform.parent = transform;
        grabTF.transform.position = transform.GetComponent<SphereCollider>().transform.position;
        grabTF.transform.rotation = transform.GetComponent<SphereCollider>().transform.rotation;
        Transform transform1 = grabTF.transform;
        transform1.localPosition -= (Vector3) ((Vector3.right * transform.GetComponent<SphereCollider>().radius) * 0.3f);
        Transform transform2 = grabTF.transform;
        transform2.localPosition -= (Vector3) ((Vector3.up * transform.GetComponent<SphereCollider>().radius) * 0.51f);
        Transform transform3 = grabTF.transform;
        transform3.localPosition -= (Vector3) ((Vector3.forward * transform.GetComponent<SphereCollider>().radius) * 0.3f);
        grabTF.transform.localRotation = Quaternion.Euler(grabTF.transform.localRotation.eulerAngles.x, grabTF.transform.localRotation.eulerAngles.y + 180f, grabTF.transform.localRotation.eulerAngles.z + 180f);
    }

    [PunRPC]
    public void grabToRight()
    {
        Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
        grabTF.transform.parent = transform;
        grabTF.transform.position = transform.GetComponent<SphereCollider>().transform.position;
        grabTF.transform.rotation = transform.GetComponent<SphereCollider>().transform.rotation;
        Transform transform1 = grabTF.transform;
        transform1.localPosition -= (Vector3) ((Vector3.right * transform.GetComponent<SphereCollider>().radius) * 0.3f);
        Transform transform2 = grabTF.transform;
        transform2.localPosition += (Vector3) ((Vector3.up * transform.GetComponent<SphereCollider>().radius) * 0.51f);
        Transform transform3 = grabTF.transform;
        transform3.localPosition -= (Vector3) ((Vector3.forward * transform.GetComponent<SphereCollider>().radius) * 0.3f);
        grabTF.transform.localRotation = Quaternion.Euler(grabTF.transform.localRotation.eulerAngles.x, grabTF.transform.localRotation.eulerAngles.y + 180f, grabTF.transform.localRotation.eulerAngles.z);
    }

    public void hit(int dmg)
    {
        NapeArmor -= dmg;
        if (NapeArmor <= 0)
        {
            NapeArmor = 0;
        }
    }

    public void hitAnkleL(int dmg)
    {
        if (!hasDie && (state != "anklehurt"))
        {
            AnkleLHP -= dmg;
            if (AnkleLHP <= 0)
            {
                getDown();
            }
        }
    }

    [PunRPC]
    public void hitAnkleLRPC(int viewID, int dmg)
    {
        if (!hasDie && (state != "anklehurt"))
        {
            PhotonView view = PhotonView.Find(viewID);
            if (view != null)
            {
                if (grabbedTarget != null)
                {
                    var grabbedHero = grabbedTarget.GetComponent<Hero>();
                    grabbedHero.photonView.RPC(grabbedHero.netUngrabbed, PhotonTargets.All);
                }
                Vector3 vector = view.gameObject.transform.position - transform.Find("Amarture/Core/Controller_Body").transform.position;
                if (vector.magnitude < 20f)
                {
                    AnkleLHP -= dmg;
                    if (AnkleLHP <= 0)
                        getDown();

                    FengGameManagerMKII.instance.sendKillInfo(false, (string) view.owner.CustomProperties[PhotonPlayerProperty.name], true, "Female Titan's ankle", dmg);
                    FengGameManagerMKII.instance.photonView.RPC<int>(FengGameManagerMKII.instance.netShowDamage, view.owner, dmg);
                }
            }
        }
    }

    public void hitAnkleR(int dmg)
    {
        if (!hasDie && (state != "anklehurt"))
        {
            AnkleRHP -= dmg;
            if (AnkleRHP <= 0)
            {
                getDown();
            }
        }
    }

    [PunRPC]
    public void hitAnkleRRPC(int viewID, int dmg)
    {
        if (!hasDie && (state != "anklehurt"))
        {
            PhotonView view = PhotonView.Find(viewID);
            if (view != null)
            {
                if (grabbedTarget != null)
                {
                    var grabbedHero = grabbedTarget.GetComponent<Hero>();
                    grabbedHero.photonView.RPC(grabbedHero.netUngrabbed, PhotonTargets.All);
                }

                Vector3 vector = view.gameObject.transform.position - transform.Find("Amarture/Core/Controller_Body").transform.position;
                if (vector.magnitude < 20f)
                {
                    AnkleRHP -= dmg;
                    if (AnkleRHP <= 0)
                        getDown();

                    FengGameManagerMKII.instance.sendKillInfo(false, (string) view.owner.CustomProperties[PhotonPlayerProperty.name], true, "Female Titan's ankle", dmg);
                    FengGameManagerMKII.instance.photonView.RPC<int>(FengGameManagerMKII.instance.netShowDamage, view.owner, dmg);
                }
            }
        }
    }

    public void hitEye()
    {
        if (!hasDie)
            justHitEye();
    }

    [PunRPC]
    public void hitEyeRPC(int viewID)
    {
        if (!hasDie)
        {
            if (grabbedTarget != null)
            {
                var hero = grabbedTarget.GetComponent<Hero>();
                hero.photonView.RPC(hero.netUngrabbed, PhotonTargets.All);
            }

            Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
            PhotonView view = PhotonView.Find(viewID);
            if (view != null)
            {
                Vector3 vector = view.gameObject.transform.position - transform.transform.position;
                if (vector.magnitude < 20f)
                    justHitEye();
            }
        }
    }

    private void idle(float sbtime = 0f)
    {
        this.sbtime = sbtime;
        this.sbtime = Mathf.Max(0.5f, this.sbtime);
        state = "idle";
        crossFade("ft_idle", 0.2f);
    }

    public bool IsGrounded()
    {
        return bottomObject.GetComponent<CheckHitGround>().isGrounded;
    }

    private void justEatHero(GameObject target, Transform hand)
    {
        var hero = target.GetComponent<Hero>();
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
        {
            if (!hero.HasDied())
            {
                hero.markDie();
                hero.photonView.RPC<int, string, PhotonMessageInfo>(
                    hero.netDie2,
                    PhotonTargets.All,
                    -1,
                    "Female Titan");
            }
        }
        else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            hero.die2(hand);
    }

    private void justHitEye()
    {
        attack("combo_blind_1");
    }

    private void killPlayer(GameObject hitHero)
    {
        if (hitHero != null)
        {
            var hero = hitHero.GetComponent<Hero>();
            Vector3 position = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                if (!hero.HasDied())
                    hero.die((hitHero.transform.position - position) * 15f * 4f, false);
            }
            else if (((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient) && !hero.HasDied())
            {
                hero.markDie();
                hero.photonView.RPC(netDie,
                    PhotonTargets.All,
                    (hitHero.transform.position - position) * 15f * 4f,
                    false,
                    -1,
                    "Female Titan",
                    true);
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
                Destroy(healthLabel);
            }
        }
        else
        {
            if (healthLabel == null)
            {
                healthLabel = (GameObject) Instantiate(Resources.Load("UI/LabelNameOverHead"));
                healthLabel.name = "LabelNameOverHead";
                healthLabel.transform.parent = transform;
                healthLabel.transform.localPosition = new Vector3(0f, 52f, 0f);
                float a = 4f;
                if ((size > 0f) && (size < 1f))
                {
                    a = 4f / size;
                    a = Mathf.Min(a, 15f);
                }
                healthLabel.transform.localScale = new Vector3(a, a, a);
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

    public void lateUpdate()
    {
        if (!IN_GAME_MAIN_CAMERA.isPausing || (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE))
        {
            if (GetComponent<Animation>().IsPlaying("ft_run"))
            {
                if ((((GetComponent<Animation>()["ft_run"].normalizedTime % 1f) > 0.1f) && ((GetComponent<Animation>()["ft_run"].normalizedTime % 1f) < 0.6f)) && (stepSoundPhase == 2))
                {
                    stepSoundPhase = 1;
                    Transform transform = base.transform.Find("snd_titan_foot");
                    transform.GetComponent<AudioSource>().Stop();
                    transform.GetComponent<AudioSource>().Play();
                }
                if (((GetComponent<Animation>()["ft_run"].normalizedTime % 1f) > 0.6f) && (stepSoundPhase == 1))
                {
                    stepSoundPhase = 2;
                    Transform transform2 = transform.Find("snd_titan_foot");
                    transform2.GetComponent<AudioSource>().Stop();
                    transform2.GetComponent<AudioSource>().Play();
                }
            }
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || photonView.isMine)
            {
            }
        }
    }

    public void lateUpdate2()
    {
        if (!IN_GAME_MAIN_CAMERA.isPausing || (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE))
        {
            if (GetComponent<Animation>().IsPlaying("ft_run"))
            {
                if ((((GetComponent<Animation>()["ft_run"].normalizedTime % 1f) > 0.1f) && ((GetComponent<Animation>()["ft_run"].normalizedTime % 1f) < 0.6f)) && (stepSoundPhase == 2))
                {
                    stepSoundPhase = 1;
                    Transform transform = base.transform.Find("snd_titan_foot");
                    transform.GetComponent<AudioSource>().Stop();
                    transform.GetComponent<AudioSource>().Play();
                }
                if (((GetComponent<Animation>()["ft_run"].normalizedTime % 1f) > 0.6f) && (stepSoundPhase == 1))
                {
                    stepSoundPhase = 2;
                    Transform transform2 = transform.Find("snd_titan_foot");
                    transform2.GetComponent<AudioSource>().Stop();
                    transform2.GetComponent<AudioSource>().Play();
                }
            }
            updateLabel();
            healthTime -= Time.deltaTime;
        }
    }

    public void loadskin()
    {
        if (((int) FengGameManagerMKII.settings[1]) == 1)
            photonView.RPC<string>(loadskinRPC, PhotonTargets.AllBuffered, (string) FengGameManagerMKII.settings[0x42]);
    }

    public IEnumerator loadskinE(string url)
    {
        while (!hasspawn)
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
    public void netDie()
    {
        if (!hasDie)
        {
            hasDie = true;
            crossFade("ft_die", 0.05f);
        }
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

    private void OnDestroy()
    {
        if (GameObject.Find("MultiplayerManager") != null)
        {
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().removeFT(this);
        }
    }

    private void playAnimation(string aniName)
    {
        GetComponent<Animation>().Play(aniName);
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
            photonView.RPC<string>(netPlayAnimation, PhotonTargets.Others, aniName);
    }

    private void playAnimationAt(string aniName, float normalizedTime)
    {
        GetComponent<Animation>().Play(aniName);
        GetComponent<Animation>()[aniName].normalizedTime = normalizedTime;
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
            photonView.RPC<string, float>(netPlayAnimationAt, PhotonTargets.Others, aniName, normalizedTime);
    }

    private void playSound(string sndname)
    {
        playsoundRPC(sndname);
        if (Network.peerType == NetworkPeerType.Server)
            photonView.RPC<string>(playsoundRPC, PhotonTargets.Others, sndname);
    }

    [PunRPC]
    private void playsoundRPC(string sndname)
    {
        transform.Find(sndname).GetComponent<AudioSource>().Play();
    }

    [PunRPC]
    public void setSize(float size, PhotonMessageInfo info)
    {
        size = Mathf.Clamp(size, 0.2f, 30f);
        if (info.sender.isMasterClient)
        {
            Transform transform = base.transform;
            transform.localScale = (Vector3) (transform.localScale * (size * 0.25f));
            this.size = size;
        }
    }

    private void Start()
    {
        startMain();
        size = 4f;
        if (photonView.isMine)
        {
            if (FengGameManagerMKII.Gamemode.Settings.TitanCustomSize)
            {
                float sizeLower = FengGameManagerMKII.Gamemode.Settings.TitanMinimumSize;
                float sizeUpper = FengGameManagerMKII.Gamemode.Settings.TitanMaximumSize;
                size = UnityEngine.Random.Range(sizeLower, sizeUpper);
                photonView.RPC<float, PhotonMessageInfo>(setSize, PhotonTargets.AllBuffered, size);
            }
            lagMax = 150f + (size * 3f);
            healthTime = 0f;
            maxHealth = NapeArmor;
            if (FengGameManagerMKII.Gamemode.Settings.TitanHealthMode != TitanHealthMode.Disabled)
                maxHealth = NapeArmor = UnityEngine.Random.Range(FengGameManagerMKII.Gamemode.Settings.TitanHealthMinimum, FengGameManagerMKII.Gamemode.Settings.TitanHealthMaximum);

            if (NapeArmor > 0)
                photonView.RPC<int, int>(labelRPC, PhotonTargets.AllBuffered, NapeArmor, maxHealth);

            loadskin();
        }
        hasspawn = true;
    }

    private void startMain()
    {
        FengGameManagerMKII.instance.addFT(this);
        name = "Female Titan";
        grabTF = new GameObject();
        grabTF.name = "titansTmpGrabTF";
        currentCamera = GameObject.Find("MainCamera");
        oldCorePosition = transform.position - transform.Find("Amarture/Core").position;
        if (myHero == null)
        {
            findNearestHero();
        }
        IEnumerator enumerator = GetComponent<Animation>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                if (current != null)
                    current.speed = 0.7f;
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
        GetComponent<Animation>()["ft_turn180"].speed = 0.5f;
        NapeArmor = 0x3e8;
        AnkleLHP = 50;
        AnkleRHP = 50;
        AnkleLHPMAX = 50;
        AnkleRHPMAX = 50;
        var flag = FengGameManagerMKII.Gamemode.Settings.RespawnMode == RespawnMode.NEVER;
        if (IN_GAME_MAIN_CAMERA.difficulty == 0)
        {
            NapeArmor = !flag ? 0x3e8 : 0x3e8;
            AnkleLHP = AnkleLHPMAX = !flag ? 50 : 50;
            AnkleRHP = AnkleRHPMAX = !flag ? 50 : 50;
        }
        else if (IN_GAME_MAIN_CAMERA.difficulty == 1)
        {
            NapeArmor = !flag ? 0xbb8 : 0x9c4;
            AnkleLHP = AnkleLHPMAX = !flag ? 200 : 100;
            AnkleRHP = AnkleRHPMAX = !flag ? 200 : 100;
            IEnumerator enumerator2 = GetComponent<Animation>().GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    AnimationState state2 = (AnimationState) enumerator2.Current;
                    if (state2 != null)
                        state2.speed = 0.7f;
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
            GetComponent<Animation>()["ft_turn180"].speed = 0.7f;
        }
        else if (IN_GAME_MAIN_CAMERA.difficulty == 2)
        {
            NapeArmor = !flag ? 0x1770 : 0xfa0;
            AnkleLHP = AnkleLHPMAX = !flag ? 0x3e8 : 200;
            AnkleRHP = AnkleRHPMAX = !flag ? 0x3e8 : 200;
            IEnumerator enumerator3 = GetComponent<Animation>().GetEnumerator();
            try
            {
                while (enumerator3.MoveNext())
                {
                    AnimationState state3 = (AnimationState) enumerator3.Current;
                    if (state3 != null)
                        state3.speed = 1f;
                }
            }
            finally
            {
                IDisposable disposable3 = enumerator3 as IDisposable;
                if (disposable3 != null)
                {
                    disposable3.Dispose();
                }
            }
            GetComponent<Animation>()["ft_turn180"].speed = 0.9f;
        }
        NapeArmor *= (int) FengGameManagerMKII.Gamemode.Settings.FemaleTitanHealthModifier;
        GetComponent<Animation>()["ft_legHurt"].speed = 1f;
        GetComponent<Animation>()["ft_legHurt_loop"].speed = 1f;
        GetComponent<Animation>()["ft_legHurt_getup"].speed = 1f;
    }

    [PunRPC]
    public void titanGetHit(int viewID, int speed)
    {
        Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
        PhotonView view = PhotonView.Find(viewID);
        if (view != null)
        {
            Vector3 vector = view.gameObject.transform.position - transform.transform.position;
            if ((vector.magnitude < lagMax) && (healthTime <= 0f))
            {
                var gameManager = FengGameManagerMKII.instance;
                if (speed >= FengGameManagerMKII.Gamemode.Settings.DamageMode)
                    NapeArmor -= speed;

                if (maxHealth > 0f)
                    photonView.RPC<int, int>(labelRPC, PhotonTargets.AllBuffered, NapeArmor, maxHealth);

                if (NapeArmor <= 0)
                {
                    NapeArmor = 0;
                    if (!hasDie)
                    {
                        photonView.RPC(netDie, PhotonTargets.OthersBuffered);
                        if (grabbedTarget != null)
                        {
                            var grabbedHero = grabbedTarget.GetComponent<Hero>();
                            grabbedHero.photonView.RPC(grabbedHero.netUngrabbed, PhotonTargets.All);
                        }
                        
                        netDie();
                        gameManager.titanGetKill(view.owner, speed, name);
                    }
                }
                else
                {
                    gameManager.sendKillInfo(false, (string) view.owner.CustomProperties[PhotonPlayerProperty.name], true, "Female Titan's neck", speed);
                    gameManager.photonView.RPC<int>(gameManager.netShowDamage, view.owner, speed);
                }

                healthTime = 0.2f;
            }
        }
    }

    private void turn(float d)
    {
        if (d > 0f)
        {
            turnAnimation = "ft_turnaround1";
        }
        else
        {
            turnAnimation = "ft_turnaround2";
        }
        playAnimation(turnAnimation);
        GetComponent<Animation>()[turnAnimation].time = 0f;
        d = Mathf.Clamp(d, -120f, 120f);
        turnDeg = d;
        desDeg = gameObject.transform.rotation.eulerAngles.y + turnDeg;
        state = "turn";
    }

    private void turn180()
    {
        turnAnimation = "ft_turn180";
        playAnimation(turnAnimation);
        GetComponent<Animation>()[turnAnimation].time = 0f;
        state = "turn180";
        needFreshCorePosition = true;
    }

    public void update()
    {
        if ((!IN_GAME_MAIN_CAMERA.isPausing || (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)) && ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || photonView.isMine))
        {
            if (hasDie)
            {
                dieTime += Time.deltaTime;
                if (GetComponent<Animation>()["ft_die"].normalizedTime >= 1f)
                {
                    playAnimation("ft_die_cry");
                    if (FengGameManagerMKII.Gamemode.Settings.SpawnTitansOnFemaleTitanDefeat)
                    {
                        for (int i = 0; i < 15; i++)
                        {
                            throw new NotImplementedException("Add mindless titan spawners to Female Titan");
                            //GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().randomSpawnOneTitan("titanRespawn", 50).GetComponent<TITAN>().beTauntedBy(base.gameObject, 20f);
                        }
                    }
                }
                if ((dieTime > 2f) && !hasDieSteam)
                {
                    hasDieSteam = true;
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        GameObject obj3 = (GameObject) Instantiate(Resources.Load("FX/FXtitanDie1"));
                        obj3.transform.position = transform.Find("Amarture/Core/Controller_Body/hip").position;
                        obj3.transform.localScale = transform.localScale;
                    }
                    else if (photonView.isMine)
                    {
                        PhotonNetwork.Instantiate("FX/FXtitanDie1", transform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = transform.localScale;
                    }
                }
                if (dieTime > FengGameManagerMKII.Gamemode.Settings.FemaleTitanDespawnTimer)
                {
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        GameObject obj5 = (GameObject) Instantiate(Resources.Load("FX/FXtitanDie"));
                        obj5.transform.position = transform.Find("Amarture/Core/Controller_Body/hip").position;
                        obj5.transform.localScale = transform.localScale;
                        Destroy(gameObject);
                    }
                    else if (photonView.isMine)
                    {
                        PhotonNetwork.Instantiate("FX/FXtitanDie", transform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = transform.localScale;
                        PhotonNetwork.Destroy(gameObject);
                    }
                }
            }
            else
            {
                if (attention > 0f)
                {
                    attention -= Time.deltaTime;
                    if (attention < 0f)
                    {
                        attention = 0f;
                        GameObject[] objArray = GameObject.FindGameObjectsWithTag("Player");
                        myHero = objArray[UnityEngine.Random.Range(0, objArray.Length)];
                        attention = UnityEngine.Random.Range((float) 5f, (float) 10f);
                    }
                }
                if (whoHasTauntMe != null)
                {
                    tauntTime -= Time.deltaTime;
                    if (tauntTime <= 0f)
                    {
                        whoHasTauntMe = null;
                    }
                    myHero = whoHasTauntMe;
                }
                if (eren != null)
                {
                    if (!eren.GetComponent<TITAN_EREN>().hasDied)
                    {
                        myHero = eren;
                    }
                    else
                    {
                        eren = null;
                        myHero = null;
                    }
                }
                if (myHero == null)
                {
                    findNearestHero();
                    if (myHero != null)
                    {
                        return;
                    }
                }
                if (myHero == null)
                {
                    myDistance = float.MaxValue;
                }
                else
                {
                    myDistance = Mathf.Sqrt(((myHero.transform.position.x - transform.position.x) * (myHero.transform.position.x - transform.position.x)) + ((myHero.transform.position.z - transform.position.z) * (myHero.transform.position.z - transform.position.z)));
                }
                if (state == "idle")
                {
                    if (myHero != null)
                    {
                        float current = 0f;
                        float f = 0f;
                        Vector3 vector9 = myHero.transform.position - transform.position;
                        current = -Mathf.Atan2(vector9.z, vector9.x) * 57.29578f;
                        f = -Mathf.DeltaAngle(current, gameObject.transform.rotation.eulerAngles.y - 90f);
                        if (!attackTarget(myHero))
                        {
                            if (Mathf.Abs(f) < 90f)
                            {
                                chase();
                            }
                            else if (UnityEngine.Random.Range(0, 100) < 1)
                            {
                                turn180();
                            }
                            else if (Mathf.Abs(f) <= 100f)
                            {
                                if ((Mathf.Abs(f) > 45f) && (UnityEngine.Random.Range(0, 100) < 30))
                                {
                                    turn(f);
                                }
                            }
                            else if (UnityEngine.Random.Range(0, 100) < 10)
                            {
                                turn180();
                            }
                        }
                    }
                }
                else if (state == "attack")
                {
                    if ((!attacked && (attackCheckTime != 0f)) && (GetComponent<Animation>()["ft_attack_" + attackAnimation].normalizedTime >= attackCheckTime))
                    {
                        GameObject obj7;
                        attacked = true;
                        fxPosition = transform.Find("ap_" + attackAnimation).position;
                        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
                        {
                            obj7 = PhotonNetwork.Instantiate("FX/" + fxName, fxPosition, fxRotation, 0);
                        }
                        else
                        {
                            obj7 = (GameObject) Instantiate(Resources.Load("FX/" + fxName), fxPosition, fxRotation);
                        }
                        obj7.transform.localScale = transform.localScale;
                        float b = 1f - (Vector3.Distance(currentCamera.transform.position, obj7.transform.position) * 0.05f);
                        b = Mathf.Min(1f, b);
                        currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(b, b, 0.95f);
                    }
                    if ((attackCheckTimeA != 0f) && (((GetComponent<Animation>()["ft_attack_" + attackAnimation].normalizedTime >= attackCheckTimeA) && (GetComponent<Animation>()["ft_attack_" + attackAnimation].normalizedTime <= attackCheckTimeB)) || (!attackChkOnce && (GetComponent<Animation>()["ft_attack_" + attackAnimation].normalizedTime >= attackCheckTimeA))))
                    {
                        if (!attackChkOnce)
                        {
                            attackChkOnce = true;
                            playSound("snd_eren_swing" + UnityEngine.Random.Range(1, 3));
                        }
                        foreach (RaycastHit hit in checkHitCapsule(checkHitCapsuleStart.position, checkHitCapsuleEnd.position, checkHitCapsuleR))
                        {
                            GameObject gameObject = hit.collider.gameObject;
                            if (gameObject.tag == "Player")
                            {
                                killPlayer(gameObject);
                            }
                            if (gameObject.tag == "erenHitbox")
                            {
                                if (attackAnimation == "combo_1")
                                {
                                    if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
                                    {
                                        gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(1);
                                    }
                                }
                                else if (attackAnimation == "combo_2")
                                {
                                    if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
                                    {
                                        gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(2);
                                    }
                                }
                                else if (((attackAnimation == "combo_3") && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)) && PhotonNetwork.isMasterClient)
                                {
                                    gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(3);
                                }
                            }
                        }
                        foreach (RaycastHit hit2 in checkHitCapsule(checkHitCapsuleEndOld, checkHitCapsuleEnd.position, checkHitCapsuleR))
                        {
                            GameObject hitHero = hit2.collider.gameObject;
                            if (hitHero.tag == "Player")
                            {
                                killPlayer(hitHero);
                            }
                        }
                        checkHitCapsuleEndOld = checkHitCapsuleEnd.position;
                    }
                    if (((attackAnimation == "jumpCombo_1") && (GetComponent<Animation>()["ft_attack_" + attackAnimation].normalizedTime >= 0.65f)) && (!startJump && (myHero != null)))
                    {
                        startJump = true;
                        float y = myHero.GetComponent<Rigidbody>().velocity.y;
                        float num7 = -20f;
                        float gravity = this.gravity;
                        float num9 = transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position.y;
                        float num10 = (num7 - gravity) * 0.5f;
                        float num11 = y;
                        float num12 = myHero.transform.position.y - num9;
                        float num13 = Mathf.Abs((float) ((Mathf.Sqrt((num11 * num11) - ((4f * num10) * num12)) - num11) / (2f * num10)));
                        Vector3 vector14 = (Vector3) ((myHero.transform.position + (myHero.GetComponent<Rigidbody>().velocity * num13)) + ((((Vector3.up * 0.5f) * num7) * num13) * num13));
                        float num14 = vector14.y;
                        if ((num12 < 0f) || ((num14 - num9) < 0f))
                        {
                            idle(0f);
                            num13 = 0.5f;
                            vector14 = transform.position + ((Vector3) ((num9 + 5f) * Vector3.up));
                            num14 = vector14.y;
                        }
                        float num15 = num14 - num9;
                        float num16 = Mathf.Sqrt((2f * num15) / this.gravity);
                        float num17 = (this.gravity * num16) + 20f;
                        num17 = Mathf.Clamp(num17, 20f, 90f);
                        Vector3 vector15 = (Vector3) ((vector14 - transform.position) / num13);
                        abnorma_jump_bite_horizon_v = new Vector3(vector15.x, 0f, vector15.z);
                        Vector3 velocity = GetComponent<Rigidbody>().velocity;
                        Vector3 vector17 = new Vector3(abnorma_jump_bite_horizon_v.x, num17, abnorma_jump_bite_horizon_v.z);
                        if (vector17.magnitude > 90f)
                        {
                            vector17 = (Vector3) (vector17.normalized * 90f);
                        }
                        Vector3 force = vector17 - velocity;
                        GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
                        float num18 = Vector2.Angle(new Vector2(transform.position.x, transform.position.z), new Vector2(myHero.transform.position.x, myHero.transform.position.z));
                        num18 = Mathf.Atan2(myHero.transform.position.x - transform.position.x, myHero.transform.position.z - transform.position.z) * 57.29578f;
                        gameObject.transform.rotation = Quaternion.Euler(0f, num18, 0f);
                    }
                    if (attackAnimation == "jumpCombo_3")
                    {
                        if ((GetComponent<Animation>()["ft_attack_" + attackAnimation].normalizedTime >= 1f) && IsGrounded())
                        {
                            attack("jumpCombo_4");
                        }
                    }
                    else if (GetComponent<Animation>()["ft_attack_" + attackAnimation].normalizedTime >= 1f)
                    {
                        if (nextAttackAnimation != null)
                        {
                            attack(nextAttackAnimation);
                            if (eren != null)
                            {
                                gameObject.transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(eren.transform.position - transform.position).eulerAngles.y, 0f);
                            }
                        }
                        else
                        {
                            findNearestHero();
                            idle(0f);
                        }
                    }
                }
                else if (state == "grab")
                {
                    if (((GetComponent<Animation>()["ft_attack_grab_" + attackAnimation].normalizedTime >= attackCheckTimeA) && (GetComponent<Animation>()["ft_attack_grab_" + attackAnimation].normalizedTime <= attackCheckTimeB)) && (grabbedTarget == null))
                    {
                        GameObject grabTarget = checkIfHitHand(currentGrabHand);
                        if (grabTarget != null)
                        {
                            if (isGrabHandLeft)
                            {
                                eatSetL(grabTarget);
                                grabbedTarget = grabTarget;
                            }
                            else
                            {
                                eatSet(grabTarget);
                                grabbedTarget = grabTarget;
                            }
                        }
                    }
                    if ((GetComponent<Animation>()["ft_attack_grab_" + attackAnimation].normalizedTime > attackCheckTime) && (grabbedTarget != null))
                    {
                        justEatHero(grabbedTarget, currentGrabHand);
                        grabbedTarget = null;
                    }
                    if (GetComponent<Animation>()["ft_attack_grab_" + attackAnimation].normalizedTime >= 1f)
                    {
                        idle(0f);
                    }
                }
                else if (state == "turn")
                {
                    gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, desDeg, 0f), (Time.deltaTime * Mathf.Abs(turnDeg)) * 0.1f);
                    if (GetComponent<Animation>()[turnAnimation].normalizedTime >= 1f)
                    {
                        idle(0f);
                    }
                }
                else if (state == "chase")
                {
                    if (((((eren == null) || (myDistance >= 35f)) || !attackTarget(myHero)) && (((getNearestHeroDistance() >= 50f) || (UnityEngine.Random.Range(0, 100) >= 20)) || !attackTarget(getNearestHero()))) && (myDistance < (attackDistance - 15f)))
                    {
                        idle(UnityEngine.Random.Range((float) 0.05f, (float) 0.2f));
                    }
                }
                else if (state == "turn180")
                {
                    if (GetComponent<Animation>()[turnAnimation].normalizedTime >= 1f)
                    {
                        gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y + 180f, gameObject.transform.rotation.eulerAngles.z);
                        idle(0f);
                        playAnimation("ft_idle");
                    }
                }
                else if (state == "anklehurt")
                {
                    if (GetComponent<Animation>()["ft_legHurt"].normalizedTime >= 1f)
                    {
                        crossFade("ft_legHurt_loop", 0.2f);
                    }
                    if (GetComponent<Animation>()["ft_legHurt_loop"].normalizedTime >= 3f)
                    {
                        crossFade("ft_legHurt_getup", 0.2f);
                    }
                    if (GetComponent<Animation>()["ft_legHurt_getup"].normalizedTime >= 1f)
                    {
                        idle(0f);
                        playAnimation("ft_idle");
                    }
                }
            }
        }
    }

    public void updateLabel()
    {
        if ((healthLabel != null)) //&& this.healthLabel.GetComponent<UILabel>().isVisible)
        {
            healthLabel.transform.LookAt(((Vector3) (2f * healthLabel.transform.position)) - Camera.main.transform.position);
        }
    }
}