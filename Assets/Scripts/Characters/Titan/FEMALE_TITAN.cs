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
        this.state = "attack";
        this.attacked = false;
        if (this.attackAnimation == type)
        {
            this.attackAnimation = type;
            this.playAnimationAt("ft_attack_" + type, 0f);
        }
        else
        {
            this.attackAnimation = type;
            this.playAnimationAt("ft_attack_" + type, 0f);
        }
        this.startJump = false;
        this.attackChkOnce = false;
        this.nextAttackAnimation = null;
        this.fxName = null;
        this.isAttackMoveByCore = false;
        this.attackCheckTime = 0f;
        this.attackCheckTimeA = 0f;
        this.attackCheckTimeB = 0f;
        this.fxRotation = Quaternion.Euler(270f, 0f, 0f);
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
                        this.attackCheckTimeA = 0.63f;
                        this.attackCheckTimeB = 0.8f;
                        this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_R/shin_R/foot_R");
                        this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_R");
                        this.checkHitCapsuleR = 5f;
                        this.isAttackMoveByCore = true;
                        this.nextAttackAnimation = "combo_2";
                        break;

                    case 1:
                        this.attackCheckTimeA = 0.27f;
                        this.attackCheckTimeB = 0.43f;
                        this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_L/shin_L/foot_L");
                        this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_L");
                        this.checkHitCapsuleR = 5f;
                        this.isAttackMoveByCore = true;
                        this.nextAttackAnimation = "combo_3";
                        break;

                    case 2:
                        this.attackCheckTimeA = 0.15f;
                        this.attackCheckTimeB = 0.3f;
                        this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_R/shin_R/foot_R");
                        this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_R");
                        this.checkHitCapsuleR = 5f;
                        this.isAttackMoveByCore = true;
                        break;

                    case 3:
                        this.isAttackMoveByCore = true;
                        this.attackCheckTimeA = 0.72f;
                        this.attackCheckTimeB = 0.83f;
                        this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
                        this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
                        this.checkHitCapsuleR = 4f;
                        this.nextAttackAnimation = "combo_blind_2";
                        break;

                    case 4:
                        this.isAttackMoveByCore = true;
                        this.attackCheckTimeA = 0.5f;
                        this.attackCheckTimeB = 0.6f;
                        this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
                        this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
                        this.checkHitCapsuleR = 4f;
                        this.nextAttackAnimation = "combo_blind_3";
                        break;

                    case 5:
                        this.isAttackMoveByCore = true;
                        this.attackCheckTimeA = 0.2f;
                        this.attackCheckTimeB = 0.28f;
                        this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
                        this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
                        this.checkHitCapsuleR = 4f;
                        break;

                    case 6:
                        this.isAttackMoveByCore = true;
                        this.attackCheckTimeA = 0.44f;
                        this.attackCheckTimeB = 0.55f;
                        this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
                        this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
                        this.checkHitCapsuleR = 4f;
                        break;

                    case 7:
                        this.isAttackMoveByCore = false;
                        this.nextAttackAnimation = "jumpCombo_2";
                        this.abnorma_jump_bite_horizon_v = Vector3.zero;
                        break;

                    case 8:
                        this.isAttackMoveByCore = false;
                        this.attackCheckTimeA = 0.48f;
                        this.attackCheckTimeB = 0.7f;
                        this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
                        this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
                        this.checkHitCapsuleR = 4f;
                        this.nextAttackAnimation = "jumpCombo_3";
                        break;

                    case 9:
                        this.isAttackMoveByCore = false;
                        this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_L/shin_L/foot_L");
                        this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_L");
                        this.checkHitCapsuleR = 5f;
                        this.attackCheckTimeA = 0.22f;
                        this.attackCheckTimeB = 0.42f;
                        break;

                    case 10:
                        this.isAttackMoveByCore = false;
                        break;

                    case 11:
                        this.isAttackMoveByCore = true;
                        this.attackCheckTimeA = 0.39f;
                        this.attackCheckTimeB = 0.6f;
                        this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_R/shin_R/foot_R");
                        this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_R");
                        this.checkHitCapsuleR = 5f;
                        break;

                    case 12:
                        this.isAttackMoveByCore = true;
                        this.attackCheckTimeA = 0.41f;
                        this.attackCheckTimeB = 0.48f;
                        this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
                        this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
                        this.checkHitCapsuleR = 4f;
                        break;

                    case 13:
                        this.isAttackMoveByCore = true;
                        this.attackCheckTimeA = 0.53f;
                        this.attackCheckTimeB = 0.63f;
                        this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
                        this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
                        this.checkHitCapsuleR = 4f;
                        break;

                    case 14:
                        this.isAttackMoveByCore = true;
                        this.attackCheckTimeA = 0.5f;
                        this.attackCheckTimeB = 0.62f;
                        this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L");
                        this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
                        this.checkHitCapsuleR = 4f;
                        break;

                    case 15:
                        this.isAttackMoveByCore = true;
                        this.attackCheckTimeA = 0.4f;
                        this.attackCheckTimeB = 0.51f;
                        this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L");
                        this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
                        this.checkHitCapsuleR = 4f;
                        break;

                    case 0x10:
                        this.isAttackMoveByCore = true;
                        this.attackCheckTimeA = 0.4f;
                        this.attackCheckTimeB = 0.51f;
                        this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
                        this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
                        this.checkHitCapsuleR = 4f;
                        break;
                }
            }
        }
        this.checkHitCapsuleEndOld = this.checkHitCapsuleEnd.transform.position;
        this.needFreshCorePosition = true;
    }

    private bool attackTarget(GameObject target)
    {
        int num5;
        float current = 0f;
        float f = 0f;
        Vector3 vector = target.transform.position - base.transform.position;
        current = -Mathf.Atan2(vector.z, vector.x) * 57.29578f;
        f = -Mathf.DeltaAngle(current, base.gameObject.transform.rotation.eulerAngles.y - 90f);
        if ((this.eren != null) && (this.myDistance < 35f))
        {
            this.attack("combo_1");
            return true;
        }
        int num3 = 0;
        string str = string.Empty;
        ArrayList list = new ArrayList();
        if (this.myDistance < 40f)
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
            float num4 = target.transform.position.y - base.transform.position.y;
            if (Mathf.Abs(f) < 90f)
            {
                if (((num4 > 0f) && (num4 < 12f)) && (this.myDistance < 22f))
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
                if (((num4 > 0f) && (num4 < 12f)) && ((this.myDistance > 20f) && (this.myDistance < 30f)))
                {
                    list.Add("attack_front");
                }
                if (((this.myDistance < 12f) && (num4 > 33f)) && (num4 < 51f))
                {
                    list.Add("grab_up");
                }
            }
            if (((Mathf.Abs(f) > 100f) && (this.myDistance < 11f)) && ((num4 >= 15f) && (num4 < 32f)))
            {
                list.Add("attack_sweep_back");
            }
            num5 = num3;
            switch (num5)
            {
                case 1:
                    if (this.myDistance >= 11f)
                    {
                        if (this.myDistance < 20f)
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
                    if (this.myDistance >= 11f)
                    {
                        if (this.myDistance < 20f)
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
                    if (this.myDistance >= 11f)
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
                    if (this.myDistance >= 11f)
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
            this.myHero = objArray[UnityEngine.Random.Range(0, objArray.Length)];
            this.attention = UnityEngine.Random.Range((float) 5f, (float) 10f);
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
                        this.grab("bottom_left");
                        return true;

                    case 1:
                        this.grab("bottom_right");
                        return true;

                    case 2:
                        this.grab("mid_left");
                        return true;

                    case 3:
                        this.grab("mid_right");
                        return true;

                    case 4:
                        this.grab("up");
                        return true;

                    case 5:
                        this.grab("up_left");
                        return true;

                    case 6:
                        this.grab("up_right");
                        return true;

                    case 7:
                        this.attack("combo_1");
                        return true;

                    case 8:
                        this.attack("front");
                        return true;

                    case 9:
                        this.attack("jumpCombo_1");
                        return true;

                    case 10:
                        this.attack("sweep");
                        return true;

                    case 11:
                        this.attack("sweep_back");
                        return true;

                    case 12:
                        this.attack("sweep_front_left");
                        return true;

                    case 13:
                        this.attack("sweep_front_right");
                        return true;

                    case 14:
                        this.attack("sweep_head_b_l");
                        return true;

                    case 15:
                        this.attack("sweep_head_b_r");
                        return true;

                    case 0x10:
                        this.turn180();
                        return true;
                }
            }
        }
        return false;
    }

    private void Awake()
    {
        base.GetComponent<Rigidbody>().freezeRotation = true;
        base.GetComponent<Rigidbody>().useGravity = false;
    }

    public void beTauntedBy(GameObject target, float tauntTime)
    {
        this.whoHasTauntMe = target;
        this.tauntTime = tauntTime;
    }

    private void chase()
    {
        this.state = "chase";
        this.crossFade("ft_run", 0.5f);
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
        base.GetComponent<Animation>().CrossFade(aniName, time);
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
        {
            object[] parameters = new object[] { aniName, time };
            base.photonView.RPC("netCrossFade", PhotonTargets.Others, parameters);
        }
    }

    private void eatSet(GameObject grabTarget)
    {
        if (!grabTarget.GetComponent<Hero>().isGrabbed)
        {
            this.grabToRight();
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
            {
                object[] parameters = new object[] { base.photonView.viewID, false };
                grabTarget.GetPhotonView().RPC("netGrabbed", PhotonTargets.All, parameters);
                object[] objArray2 = new object[] { "grabbed" };
                grabTarget.GetPhotonView().RPC("netPlayAnimation", PhotonTargets.All, objArray2);
                base.photonView.RPC("grabToRight", PhotonTargets.Others, new object[0]);
            }
            else
            {
                grabTarget.GetComponent<Hero>().grabbed(base.gameObject, false);
                grabTarget.GetComponent<Hero>().GetComponent<Animation>().Play("grabbed");
            }
        }
    }

    private void eatSetL(GameObject grabTarget)
    {
        if (!grabTarget.GetComponent<Hero>().isGrabbed)
        {
            this.grabToLeft();
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
            {
                object[] parameters = new object[] { base.photonView.viewID, true };
                grabTarget.GetPhotonView().RPC("netGrabbed", PhotonTargets.All, parameters);
                object[] objArray2 = new object[] { "grabbed" };
                grabTarget.GetPhotonView().RPC("netPlayAnimation", PhotonTargets.All, objArray2);
                base.photonView.RPC("grabToLeft", PhotonTargets.Others, new object[0]);
            }
            else
            {
                grabTarget.GetComponent<Hero>().grabbed(base.gameObject, true);
                grabTarget.GetComponent<Hero>().GetComponent<Animation>().Play("grabbed");
            }
        }
    }

    public void erenIsHere(GameObject target)
    {
        this.myHero = this.eren = target;
    }

    private void findNearestHero()
    {
        this.myHero = this.getNearestHero();
        this.attention = UnityEngine.Random.Range((float) 5f, (float) 10f);
    }

    private void FixedUpdate()
    {
        if ((!IN_GAME_MAIN_CAMERA.isPausing || (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)) && ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine))
        {
            if (this.bottomObject.GetComponent<CheckHitGround>().isGrounded)
            {
                this.grounded = true;
                this.bottomObject.GetComponent<CheckHitGround>().isGrounded = false;
            }
            else
            {
                this.grounded = false;
            }
            if (this.needFreshCorePosition)
            {
                this.oldCorePosition = base.transform.position - base.transform.Find("Amarture/Core").position;
                this.needFreshCorePosition = false;
            }
            if (((this.state != "attack") || !this.isAttackMoveByCore) && (((this.state != "hit") && (this.state != "turn180")) && (this.state != "anklehurt")))
            {
                if (this.state == "chase")
                {
                    if (this.myHero == null)
                    {
                        return;
                    }
                    Vector3 vector3 = (Vector3) (base.transform.forward * this.speed);
                    Vector3 velocity = base.GetComponent<Rigidbody>().velocity;
                    Vector3 force = vector3 - velocity;
                    force.y = 0f;
                    base.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
                    float current = 0f;
                    Vector3 vector6 = this.myHero.transform.position - base.transform.position;
                    current = -Mathf.Atan2(vector6.z, vector6.x) * 57.29578f;
                    float num2 = -Mathf.DeltaAngle(current, base.gameObject.transform.rotation.eulerAngles.y - 90f);
                    base.gameObject.transform.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, Quaternion.Euler(0f, base.gameObject.transform.rotation.eulerAngles.y + num2, 0f), this.speed * Time.deltaTime);
                }
                else if (this.grounded && !base.GetComponent<Animation>().IsPlaying("ft_attack_jumpCombo_1"))
                {
                    base.GetComponent<Rigidbody>().AddForce(new Vector3(-base.GetComponent<Rigidbody>().velocity.x, 0f, -base.GetComponent<Rigidbody>().velocity.z), ForceMode.VelocityChange);
                }
            }
            else
            {
                Vector3 vector = (base.transform.position - base.transform.Find("Amarture/Core").position) - this.oldCorePosition;
                base.GetComponent<Rigidbody>().velocity = (Vector3) ((vector / Time.deltaTime) + (Vector3.up * base.GetComponent<Rigidbody>().velocity.y));
                this.oldCorePosition = base.transform.position - base.transform.Find("Amarture/Core").position;
            }
            base.GetComponent<Rigidbody>().AddForce(new Vector3(0f, -this.gravity * base.GetComponent<Rigidbody>().mass, 0f));
        }
    }

    private void getDown()
    {
        this.state = "anklehurt";
        this.playAnimation("ft_legHurt");
        this.AnkleRHP = this.AnkleRHPMAX;
        this.AnkleLHP = this.AnkleLHPMAX;
        this.needFreshCorePosition = true;
    }

    private GameObject getNearestHero()
    {
        GameObject[] objArray = GameObject.FindGameObjectsWithTag("Player");
        GameObject obj2 = null;
        float positiveInfinity = float.PositiveInfinity;
        Vector3 position = base.transform.position;
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
        Vector3 position = base.transform.position;
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
        this.state = "grab";
        this.attacked = false;
        this.attackAnimation = type;
        if (base.GetComponent<Animation>().IsPlaying("ft_attack_grab_" + type))
        {
            base.GetComponent<Animation>()["ft_attack_grab_" + type].normalizedTime = 0f;
            this.playAnimation("ft_attack_grab_" + type);
        }
        else
        {
            this.crossFade("ft_attack_grab_" + type, 0.1f);
        }
        this.isGrabHandLeft = true;
        this.grabbedTarget = null;
        this.attackCheckTime = 0f;
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
                        this.attackCheckTimeA = 0.28f;
                        this.attackCheckTimeB = 0.38f;
                        this.attackCheckTime = 0.65f;
                        this.isGrabHandLeft = false;
                        break;

                    case 1:
                        this.attackCheckTimeA = 0.27f;
                        this.attackCheckTimeB = 0.37f;
                        this.attackCheckTime = 0.65f;
                        break;

                    case 2:
                        this.attackCheckTimeA = 0.27f;
                        this.attackCheckTimeB = 0.37f;
                        this.attackCheckTime = 0.65f;
                        this.isGrabHandLeft = false;
                        break;

                    case 3:
                        this.attackCheckTimeA = 0.27f;
                        this.attackCheckTimeB = 0.36f;
                        this.attackCheckTime = 0.66f;
                        break;

                    case 4:
                        this.attackCheckTimeA = 0.25f;
                        this.attackCheckTimeB = 0.32f;
                        this.attackCheckTime = 0.67f;
                        break;

                    case 5:
                        this.attackCheckTimeA = 0.26f;
                        this.attackCheckTimeB = 0.4f;
                        this.attackCheckTime = 0.66f;
                        break;

                    case 6:
                        this.attackCheckTimeA = 0.26f;
                        this.attackCheckTimeB = 0.4f;
                        this.attackCheckTime = 0.66f;
                        this.isGrabHandLeft = false;
                        break;
                }
            }
        }
        if (this.isGrabHandLeft)
        {
            this.currentGrabHand = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
        }
        else
        {
            this.currentGrabHand = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
        }
    }

    [PunRPC]
    public void grabbedTargetEscape()
    {
        this.grabbedTarget = null;
    }

    [PunRPC]
    public void grabToLeft()
    {
        Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
        this.grabTF.transform.parent = transform;
        this.grabTF.transform.parent = transform;
        this.grabTF.transform.position = transform.GetComponent<SphereCollider>().transform.position;
        this.grabTF.transform.rotation = transform.GetComponent<SphereCollider>().transform.rotation;
        Transform transform1 = this.grabTF.transform;
        transform1.localPosition -= (Vector3) ((Vector3.right * transform.GetComponent<SphereCollider>().radius) * 0.3f);
        Transform transform2 = this.grabTF.transform;
        transform2.localPosition -= (Vector3) ((Vector3.up * transform.GetComponent<SphereCollider>().radius) * 0.51f);
        Transform transform3 = this.grabTF.transform;
        transform3.localPosition -= (Vector3) ((Vector3.forward * transform.GetComponent<SphereCollider>().radius) * 0.3f);
        this.grabTF.transform.localRotation = Quaternion.Euler(this.grabTF.transform.localRotation.eulerAngles.x, this.grabTF.transform.localRotation.eulerAngles.y + 180f, this.grabTF.transform.localRotation.eulerAngles.z + 180f);
    }

    [PunRPC]
    public void grabToRight()
    {
        Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
        this.grabTF.transform.parent = transform;
        this.grabTF.transform.position = transform.GetComponent<SphereCollider>().transform.position;
        this.grabTF.transform.rotation = transform.GetComponent<SphereCollider>().transform.rotation;
        Transform transform1 = this.grabTF.transform;
        transform1.localPosition -= (Vector3) ((Vector3.right * transform.GetComponent<SphereCollider>().radius) * 0.3f);
        Transform transform2 = this.grabTF.transform;
        transform2.localPosition += (Vector3) ((Vector3.up * transform.GetComponent<SphereCollider>().radius) * 0.51f);
        Transform transform3 = this.grabTF.transform;
        transform3.localPosition -= (Vector3) ((Vector3.forward * transform.GetComponent<SphereCollider>().radius) * 0.3f);
        this.grabTF.transform.localRotation = Quaternion.Euler(this.grabTF.transform.localRotation.eulerAngles.x, this.grabTF.transform.localRotation.eulerAngles.y + 180f, this.grabTF.transform.localRotation.eulerAngles.z);
    }

    public void hit(int dmg)
    {
        this.NapeArmor -= dmg;
        if (this.NapeArmor <= 0)
        {
            this.NapeArmor = 0;
        }
    }

    public void hitAnkleL(int dmg)
    {
        if (!this.hasDie && (this.state != "anklehurt"))
        {
            this.AnkleLHP -= dmg;
            if (this.AnkleLHP <= 0)
            {
                this.getDown();
            }
        }
    }

    [PunRPC]
    public void hitAnkleLRPC(int viewID, int dmg)
    {
        if (!this.hasDie && (this.state != "anklehurt"))
        {
            PhotonView view = PhotonView.Find(viewID);
            if (view != null)
            {
                if (this.grabbedTarget != null)
                {
                    this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All, new object[0]);
                }
                Vector3 vector = view.gameObject.transform.position - base.transform.Find("Amarture/Core/Controller_Body").transform.position;
                if (vector.magnitude < 20f)
                {
                    this.AnkleLHP -= dmg;
                    if (this.AnkleLHP <= 0)
                    {
                        this.getDown();
                    }
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(false, (string) view.owner.CustomProperties[PhotonPlayerProperty.name], true, "Female Titan's ankle", dmg);
                    object[] parameters = new object[] { dmg };
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("netShowDamage", view.owner, parameters);
                }
            }
        }
    }

    public void hitAnkleR(int dmg)
    {
        if (!this.hasDie && (this.state != "anklehurt"))
        {
            this.AnkleRHP -= dmg;
            if (this.AnkleRHP <= 0)
            {
                this.getDown();
            }
        }
    }

    [PunRPC]
    public void hitAnkleRRPC(int viewID, int dmg)
    {
        if (!this.hasDie && (this.state != "anklehurt"))
        {
            PhotonView view = PhotonView.Find(viewID);
            if (view != null)
            {
                if (this.grabbedTarget != null)
                {
                    this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All, new object[0]);
                }
                Vector3 vector = view.gameObject.transform.position - base.transform.Find("Amarture/Core/Controller_Body").transform.position;
                if (vector.magnitude < 20f)
                {
                    this.AnkleRHP -= dmg;
                    if (this.AnkleRHP <= 0)
                    {
                        this.getDown();
                    }
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(false, (string) view.owner.CustomProperties[PhotonPlayerProperty.name], true, "Female Titan's ankle", dmg);
                    object[] parameters = new object[] { dmg };
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("netShowDamage", view.owner, parameters);
                }
            }
        }
    }

    public void hitEye()
    {
        if (!this.hasDie)
        {
            this.justHitEye();
        }
    }

    [PunRPC]
    public void hitEyeRPC(int viewID)
    {
        if (!this.hasDie)
        {
            if (this.grabbedTarget != null)
            {
                this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All, new object[0]);
            }
            Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
            PhotonView view = PhotonView.Find(viewID);
            if (view != null)
            {
                Vector3 vector = view.gameObject.transform.position - transform.transform.position;
                if (vector.magnitude < 20f)
                {
                    this.justHitEye();
                }
            }
        }
    }

    private void idle(float sbtime = 0f)
    {
        this.sbtime = sbtime;
        this.sbtime = Mathf.Max(0.5f, this.sbtime);
        this.state = "idle";
        this.crossFade("ft_idle", 0.2f);
    }

    public bool IsGrounded()
    {
        return this.bottomObject.GetComponent<CheckHitGround>().isGrounded;
    }

    private void justEatHero(GameObject target, Transform hand)
    {
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
        {
            if (!target.GetComponent<Hero>().HasDied())
            {
                target.GetComponent<Hero>().markDie();
                object[] parameters = new object[] { -1, "Female Titan" };
                target.GetComponent<Hero>().photonView.RPC("netDie2", PhotonTargets.All, parameters);
            }
        }
        else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            target.GetComponent<Hero>().die2(hand);
        }
    }

    private void justHitEye()
    {
        this.attack("combo_blind_1");
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
            else if (((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient) && !hitHero.GetComponent<Hero>().HasDied())
            {
                hitHero.GetComponent<Hero>().markDie();
                object[] parameters = new object[] { (Vector3) (((hitHero.transform.position - position) * 15f) * 4f), false, -1, "Female Titan", true };
                hitHero.GetComponent<Hero>().photonView.RPC("netDie", PhotonTargets.All, parameters);
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
                this.healthLabel.transform.localPosition = new Vector3(0f, 52f, 0f);
                float a = 4f;
                if ((this.size > 0f) && (this.size < 1f))
                {
                    a = 4f / this.size;
                    a = Mathf.Min(a, 15f);
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

    public void lateUpdate()
    {
        if (!IN_GAME_MAIN_CAMERA.isPausing || (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE))
        {
            if (base.GetComponent<Animation>().IsPlaying("ft_run"))
            {
                if ((((base.GetComponent<Animation>()["ft_run"].normalizedTime % 1f) > 0.1f) && ((base.GetComponent<Animation>()["ft_run"].normalizedTime % 1f) < 0.6f)) && (this.stepSoundPhase == 2))
                {
                    this.stepSoundPhase = 1;
                    Transform transform = base.transform.Find("snd_titan_foot");
                    transform.GetComponent<AudioSource>().Stop();
                    transform.GetComponent<AudioSource>().Play();
                }
                if (((base.GetComponent<Animation>()["ft_run"].normalizedTime % 1f) > 0.6f) && (this.stepSoundPhase == 1))
                {
                    this.stepSoundPhase = 2;
                    Transform transform2 = base.transform.Find("snd_titan_foot");
                    transform2.GetComponent<AudioSource>().Stop();
                    transform2.GetComponent<AudioSource>().Play();
                }
            }
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
            {
            }
        }
    }

    public void lateUpdate2()
    {
        if (!IN_GAME_MAIN_CAMERA.isPausing || (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE))
        {
            if (base.GetComponent<Animation>().IsPlaying("ft_run"))
            {
                if ((((base.GetComponent<Animation>()["ft_run"].normalizedTime % 1f) > 0.1f) && ((base.GetComponent<Animation>()["ft_run"].normalizedTime % 1f) < 0.6f)) && (this.stepSoundPhase == 2))
                {
                    this.stepSoundPhase = 1;
                    Transform transform = base.transform.Find("snd_titan_foot");
                    transform.GetComponent<AudioSource>().Stop();
                    transform.GetComponent<AudioSource>().Play();
                }
                if (((base.GetComponent<Animation>()["ft_run"].normalizedTime % 1f) > 0.6f) && (this.stepSoundPhase == 1))
                {
                    this.stepSoundPhase = 2;
                    Transform transform2 = base.transform.Find("snd_titan_foot");
                    transform2.GetComponent<AudioSource>().Stop();
                    transform2.GetComponent<AudioSource>().Play();
                }
            }
            this.updateLabel();
            this.healthTime -= Time.deltaTime;
        }
    }

    public void loadskin()
    {
        if (((int) FengGameManagerMKII.settings[1]) == 1)
        {
            base.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, new object[] { (string) FengGameManagerMKII.settings[0x42] });
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
    public void netDie()
    {
        if (!this.hasDie)
        {
            this.hasDie = true;
            this.crossFade("ft_die", 0.05f);
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
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().removeFT(this);
        }
    }

    private void playAnimation(string aniName)
    {
        base.GetComponent<Animation>().Play(aniName);
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
        {
            object[] parameters = new object[] { aniName };
            base.photonView.RPC("netPlayAnimation", PhotonTargets.Others, parameters);
        }
    }

    private void playAnimationAt(string aniName, float normalizedTime)
    {
        base.GetComponent<Animation>().Play(aniName);
        base.GetComponent<Animation>()[aniName].normalizedTime = normalizedTime;
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
        {
            object[] parameters = new object[] { aniName, normalizedTime };
            base.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, parameters);
        }
    }

    private void playSound(string sndname)
    {
        this.playsoundRPC(sndname);
        if (Network.peerType == NetworkPeerType.Server)
        {
            object[] parameters = new object[] { sndname };
            base.photonView.RPC("playsoundRPC", PhotonTargets.Others, parameters);
        }
    }

    [PunRPC]
    private void playsoundRPC(string sndname)
    {
        base.transform.Find(sndname).GetComponent<AudioSource>().Play();
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
        this.startMain();
        this.size = 4f;
        if (base.photonView.isMine)
        {
            if (FengGameManagerMKII.Gamemode.Settings.TitanCustomSize)
            {
                float sizeLower = FengGameManagerMKII.Gamemode.Settings.TitanMinimumSize;
                float sizeUpper = FengGameManagerMKII.Gamemode.Settings.TitanMaximumSize;
                this.size = UnityEngine.Random.Range(sizeLower, sizeUpper);
                base.photonView.RPC("setSize", PhotonTargets.AllBuffered, new object[] { this.size });
            }
            this.lagMax = 150f + (this.size * 3f);
            this.healthTime = 0f;
            this.maxHealth = this.NapeArmor;
            if (FengGameManagerMKII.Gamemode.Settings.TitanHealthMode != TitanHealthMode.Disabled)
            {
                this.maxHealth = this.NapeArmor = UnityEngine.Random.Range(FengGameManagerMKII.Gamemode.Settings.TitanHealthMinimum, FengGameManagerMKII.Gamemode.Settings.TitanHealthMaximum);
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
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().addFT(this);
        base.name = "Female Titan";
        this.grabTF = new GameObject();
        this.grabTF.name = "titansTmpGrabTF";
        this.currentCamera = GameObject.Find("MainCamera");
        this.oldCorePosition = base.transform.position - base.transform.Find("Amarture/Core").position;
        if (this.myHero == null)
        {
            this.findNearestHero();
        }
        IEnumerator enumerator = base.GetComponent<Animation>().GetEnumerator();
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
        base.GetComponent<Animation>()["ft_turn180"].speed = 0.5f;
        this.NapeArmor = 0x3e8;
        this.AnkleLHP = 50;
        this.AnkleRHP = 50;
        this.AnkleLHPMAX = 50;
        this.AnkleRHPMAX = 50;
        var flag = FengGameManagerMKII.Gamemode.Settings.RespawnMode == RespawnMode.NEVER;
        if (IN_GAME_MAIN_CAMERA.difficulty == 0)
        {
            this.NapeArmor = !flag ? 0x3e8 : 0x3e8;
            this.AnkleLHP = this.AnkleLHPMAX = !flag ? 50 : 50;
            this.AnkleRHP = this.AnkleRHPMAX = !flag ? 50 : 50;
        }
        else if (IN_GAME_MAIN_CAMERA.difficulty == 1)
        {
            this.NapeArmor = !flag ? 0xbb8 : 0x9c4;
            this.AnkleLHP = this.AnkleLHPMAX = !flag ? 200 : 100;
            this.AnkleRHP = this.AnkleRHPMAX = !flag ? 200 : 100;
            IEnumerator enumerator2 = base.GetComponent<Animation>().GetEnumerator();
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
            base.GetComponent<Animation>()["ft_turn180"].speed = 0.7f;
        }
        else if (IN_GAME_MAIN_CAMERA.difficulty == 2)
        {
            this.NapeArmor = !flag ? 0x1770 : 0xfa0;
            this.AnkleLHP = this.AnkleLHPMAX = !flag ? 0x3e8 : 200;
            this.AnkleRHP = this.AnkleRHPMAX = !flag ? 0x3e8 : 200;
            IEnumerator enumerator3 = base.GetComponent<Animation>().GetEnumerator();
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
            base.GetComponent<Animation>()["ft_turn180"].speed = 0.9f;
        }
        NapeArmor *= (int) FengGameManagerMKII.Gamemode.Settings.FemaleTitanHealthModifier;
        base.GetComponent<Animation>()["ft_legHurt"].speed = 1f;
        base.GetComponent<Animation>()["ft_legHurt_loop"].speed = 1f;
        base.GetComponent<Animation>()["ft_legHurt_getup"].speed = 1f;
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
                if (speed >= FengGameManagerMKII.Gamemode.Settings.DamageMode)
                {
                    this.NapeArmor -= speed;
                }
                if (this.maxHealth > 0f)
                {
                    base.photonView.RPC("labelRPC", PhotonTargets.AllBuffered, new object[] { this.NapeArmor, this.maxHealth });
                }
                if (this.NapeArmor <= 0)
                {
                    this.NapeArmor = 0;
                    if (!this.hasDie)
                    {
                        base.photonView.RPC("netDie", PhotonTargets.OthersBuffered, new object[0]);
                        if (this.grabbedTarget != null)
                        {
                            this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All, new object[0]);
                        }
                        this.netDie();
                        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().titanGetKill(view.owner, speed, base.name);
                    }
                }
                else
                {
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(false, (string) view.owner.CustomProperties[PhotonPlayerProperty.name], true, "Female Titan's neck", speed);
                    object[] parameters = new object[] { speed };
                    GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("netShowDamage", view.owner, parameters);
                }
                this.healthTime = 0.2f;
            }
        }
    }

    private void turn(float d)
    {
        if (d > 0f)
        {
            this.turnAnimation = "ft_turnaround1";
        }
        else
        {
            this.turnAnimation = "ft_turnaround2";
        }
        this.playAnimation(this.turnAnimation);
        base.GetComponent<Animation>()[this.turnAnimation].time = 0f;
        d = Mathf.Clamp(d, -120f, 120f);
        this.turnDeg = d;
        this.desDeg = base.gameObject.transform.rotation.eulerAngles.y + this.turnDeg;
        this.state = "turn";
    }

    private void turn180()
    {
        this.turnAnimation = "ft_turn180";
        this.playAnimation(this.turnAnimation);
        base.GetComponent<Animation>()[this.turnAnimation].time = 0f;
        this.state = "turn180";
        this.needFreshCorePosition = true;
    }

    public void update()
    {
        if ((!IN_GAME_MAIN_CAMERA.isPausing || (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)) && ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine))
        {
            if (this.hasDie)
            {
                this.dieTime += Time.deltaTime;
                if (base.GetComponent<Animation>()["ft_die"].normalizedTime >= 1f)
                {
                    this.playAnimation("ft_die_cry");
                    if (FengGameManagerMKII.Gamemode.Settings.SpawnTitansOnFemaleTitanDefeat)
                    {
                        for (int i = 0; i < 15; i++)
                        {
                            throw new NotImplementedException("Add mindless titan spawners to Female Titan");
                            //GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().randomSpawnOneTitan("titanRespawn", 50).GetComponent<TITAN>().beTauntedBy(base.gameObject, 20f);
                        }
                    }
                }
                if ((this.dieTime > 2f) && !this.hasDieSteam)
                {
                    this.hasDieSteam = true;
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        GameObject obj3 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("FX/FXtitanDie1"));
                        obj3.transform.position = base.transform.Find("Amarture/Core/Controller_Body/hip").position;
                        obj3.transform.localScale = base.transform.localScale;
                    }
                    else if (base.photonView.isMine)
                    {
                        PhotonNetwork.Instantiate("FX/FXtitanDie1", base.transform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = base.transform.localScale;
                    }
                }
                if (this.dieTime > FengGameManagerMKII.Gamemode.Settings.FemaleTitanDespawnTimer)
                {
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        GameObject obj5 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("FX/FXtitanDie"));
                        obj5.transform.position = base.transform.Find("Amarture/Core/Controller_Body/hip").position;
                        obj5.transform.localScale = base.transform.localScale;
                        UnityEngine.Object.Destroy(base.gameObject);
                    }
                    else if (base.photonView.isMine)
                    {
                        PhotonNetwork.Instantiate("FX/FXtitanDie", base.transform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = base.transform.localScale;
                        PhotonNetwork.Destroy(base.gameObject);
                    }
                }
            }
            else
            {
                if (this.attention > 0f)
                {
                    this.attention -= Time.deltaTime;
                    if (this.attention < 0f)
                    {
                        this.attention = 0f;
                        GameObject[] objArray = GameObject.FindGameObjectsWithTag("Player");
                        this.myHero = objArray[UnityEngine.Random.Range(0, objArray.Length)];
                        this.attention = UnityEngine.Random.Range((float) 5f, (float) 10f);
                    }
                }
                if (this.whoHasTauntMe != null)
                {
                    this.tauntTime -= Time.deltaTime;
                    if (this.tauntTime <= 0f)
                    {
                        this.whoHasTauntMe = null;
                    }
                    this.myHero = this.whoHasTauntMe;
                }
                if (this.eren != null)
                {
                    if (!this.eren.GetComponent<TITAN_EREN>().hasDied)
                    {
                        this.myHero = this.eren;
                    }
                    else
                    {
                        this.eren = null;
                        this.myHero = null;
                    }
                }
                if (this.myHero == null)
                {
                    this.findNearestHero();
                    if (this.myHero != null)
                    {
                        return;
                    }
                }
                if (this.myHero == null)
                {
                    this.myDistance = float.MaxValue;
                }
                else
                {
                    this.myDistance = Mathf.Sqrt(((this.myHero.transform.position.x - base.transform.position.x) * (this.myHero.transform.position.x - base.transform.position.x)) + ((this.myHero.transform.position.z - base.transform.position.z) * (this.myHero.transform.position.z - base.transform.position.z)));
                }
                if (this.state == "idle")
                {
                    if (this.myHero != null)
                    {
                        float current = 0f;
                        float f = 0f;
                        Vector3 vector9 = this.myHero.transform.position - base.transform.position;
                        current = -Mathf.Atan2(vector9.z, vector9.x) * 57.29578f;
                        f = -Mathf.DeltaAngle(current, base.gameObject.transform.rotation.eulerAngles.y - 90f);
                        if (!this.attackTarget(this.myHero))
                        {
                            if (Mathf.Abs(f) < 90f)
                            {
                                this.chase();
                            }
                            else if (UnityEngine.Random.Range(0, 100) < 1)
                            {
                                this.turn180();
                            }
                            else if (Mathf.Abs(f) <= 100f)
                            {
                                if ((Mathf.Abs(f) > 45f) && (UnityEngine.Random.Range(0, 100) < 30))
                                {
                                    this.turn(f);
                                }
                            }
                            else if (UnityEngine.Random.Range(0, 100) < 10)
                            {
                                this.turn180();
                            }
                        }
                    }
                }
                else if (this.state == "attack")
                {
                    if ((!this.attacked && (this.attackCheckTime != 0f)) && (base.GetComponent<Animation>()["ft_attack_" + this.attackAnimation].normalizedTime >= this.attackCheckTime))
                    {
                        GameObject obj7;
                        this.attacked = true;
                        this.fxPosition = base.transform.Find("ap_" + this.attackAnimation).position;
                        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
                        {
                            obj7 = PhotonNetwork.Instantiate("FX/" + this.fxName, this.fxPosition, this.fxRotation, 0);
                        }
                        else
                        {
                            obj7 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("FX/" + this.fxName), this.fxPosition, this.fxRotation);
                        }
                        obj7.transform.localScale = base.transform.localScale;
                        float b = 1f - (Vector3.Distance(this.currentCamera.transform.position, obj7.transform.position) * 0.05f);
                        b = Mathf.Min(1f, b);
                        this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(b, b, 0.95f);
                    }
                    if ((this.attackCheckTimeA != 0f) && (((base.GetComponent<Animation>()["ft_attack_" + this.attackAnimation].normalizedTime >= this.attackCheckTimeA) && (base.GetComponent<Animation>()["ft_attack_" + this.attackAnimation].normalizedTime <= this.attackCheckTimeB)) || (!this.attackChkOnce && (base.GetComponent<Animation>()["ft_attack_" + this.attackAnimation].normalizedTime >= this.attackCheckTimeA))))
                    {
                        if (!this.attackChkOnce)
                        {
                            this.attackChkOnce = true;
                            this.playSound("snd_eren_swing" + UnityEngine.Random.Range(1, 3));
                        }
                        foreach (RaycastHit hit in this.checkHitCapsule(this.checkHitCapsuleStart.position, this.checkHitCapsuleEnd.position, this.checkHitCapsuleR))
                        {
                            GameObject gameObject = hit.collider.gameObject;
                            if (gameObject.tag == "Player")
                            {
                                this.killPlayer(gameObject);
                            }
                            if (gameObject.tag == "erenHitbox")
                            {
                                if (this.attackAnimation == "combo_1")
                                {
                                    if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
                                    {
                                        gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(1);
                                    }
                                }
                                else if (this.attackAnimation == "combo_2")
                                {
                                    if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
                                    {
                                        gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(2);
                                    }
                                }
                                else if (((this.attackAnimation == "combo_3") && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)) && PhotonNetwork.isMasterClient)
                                {
                                    gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(3);
                                }
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
                    if (((this.attackAnimation == "jumpCombo_1") && (base.GetComponent<Animation>()["ft_attack_" + this.attackAnimation].normalizedTime >= 0.65f)) && (!this.startJump && (this.myHero != null)))
                    {
                        this.startJump = true;
                        float y = this.myHero.GetComponent<Rigidbody>().velocity.y;
                        float num7 = -20f;
                        float gravity = this.gravity;
                        float num9 = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position.y;
                        float num10 = (num7 - gravity) * 0.5f;
                        float num11 = y;
                        float num12 = this.myHero.transform.position.y - num9;
                        float num13 = Mathf.Abs((float) ((Mathf.Sqrt((num11 * num11) - ((4f * num10) * num12)) - num11) / (2f * num10)));
                        Vector3 vector14 = (Vector3) ((this.myHero.transform.position + (this.myHero.GetComponent<Rigidbody>().velocity * num13)) + ((((Vector3.up * 0.5f) * num7) * num13) * num13));
                        float num14 = vector14.y;
                        if ((num12 < 0f) || ((num14 - num9) < 0f))
                        {
                            this.idle(0f);
                            num13 = 0.5f;
                            vector14 = base.transform.position + ((Vector3) ((num9 + 5f) * Vector3.up));
                            num14 = vector14.y;
                        }
                        float num15 = num14 - num9;
                        float num16 = Mathf.Sqrt((2f * num15) / this.gravity);
                        float num17 = (this.gravity * num16) + 20f;
                        num17 = Mathf.Clamp(num17, 20f, 90f);
                        Vector3 vector15 = (Vector3) ((vector14 - base.transform.position) / num13);
                        this.abnorma_jump_bite_horizon_v = new Vector3(vector15.x, 0f, vector15.z);
                        Vector3 velocity = base.GetComponent<Rigidbody>().velocity;
                        Vector3 vector17 = new Vector3(this.abnorma_jump_bite_horizon_v.x, num17, this.abnorma_jump_bite_horizon_v.z);
                        if (vector17.magnitude > 90f)
                        {
                            vector17 = (Vector3) (vector17.normalized * 90f);
                        }
                        Vector3 force = vector17 - velocity;
                        base.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
                        float num18 = Vector2.Angle(new Vector2(base.transform.position.x, base.transform.position.z), new Vector2(this.myHero.transform.position.x, this.myHero.transform.position.z));
                        num18 = Mathf.Atan2(this.myHero.transform.position.x - base.transform.position.x, this.myHero.transform.position.z - base.transform.position.z) * 57.29578f;
                        base.gameObject.transform.rotation = Quaternion.Euler(0f, num18, 0f);
                    }
                    if (this.attackAnimation == "jumpCombo_3")
                    {
                        if ((base.GetComponent<Animation>()["ft_attack_" + this.attackAnimation].normalizedTime >= 1f) && this.IsGrounded())
                        {
                            this.attack("jumpCombo_4");
                        }
                    }
                    else if (base.GetComponent<Animation>()["ft_attack_" + this.attackAnimation].normalizedTime >= 1f)
                    {
                        if (this.nextAttackAnimation != null)
                        {
                            this.attack(this.nextAttackAnimation);
                            if (this.eren != null)
                            {
                                base.gameObject.transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(this.eren.transform.position - base.transform.position).eulerAngles.y, 0f);
                            }
                        }
                        else
                        {
                            this.findNearestHero();
                            this.idle(0f);
                        }
                    }
                }
                else if (this.state == "grab")
                {
                    if (((base.GetComponent<Animation>()["ft_attack_grab_" + this.attackAnimation].normalizedTime >= this.attackCheckTimeA) && (base.GetComponent<Animation>()["ft_attack_grab_" + this.attackAnimation].normalizedTime <= this.attackCheckTimeB)) && (this.grabbedTarget == null))
                    {
                        GameObject grabTarget = this.checkIfHitHand(this.currentGrabHand);
                        if (grabTarget != null)
                        {
                            if (this.isGrabHandLeft)
                            {
                                this.eatSetL(grabTarget);
                                this.grabbedTarget = grabTarget;
                            }
                            else
                            {
                                this.eatSet(grabTarget);
                                this.grabbedTarget = grabTarget;
                            }
                        }
                    }
                    if ((base.GetComponent<Animation>()["ft_attack_grab_" + this.attackAnimation].normalizedTime > this.attackCheckTime) && (this.grabbedTarget != null))
                    {
                        this.justEatHero(this.grabbedTarget, this.currentGrabHand);
                        this.grabbedTarget = null;
                    }
                    if (base.GetComponent<Animation>()["ft_attack_grab_" + this.attackAnimation].normalizedTime >= 1f)
                    {
                        this.idle(0f);
                    }
                }
                else if (this.state == "turn")
                {
                    base.gameObject.transform.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, Quaternion.Euler(0f, this.desDeg, 0f), (Time.deltaTime * Mathf.Abs(this.turnDeg)) * 0.1f);
                    if (base.GetComponent<Animation>()[this.turnAnimation].normalizedTime >= 1f)
                    {
                        this.idle(0f);
                    }
                }
                else if (this.state == "chase")
                {
                    if (((((this.eren == null) || (this.myDistance >= 35f)) || !this.attackTarget(this.myHero)) && (((this.getNearestHeroDistance() >= 50f) || (UnityEngine.Random.Range(0, 100) >= 20)) || !this.attackTarget(this.getNearestHero()))) && (this.myDistance < (this.attackDistance - 15f)))
                    {
                        this.idle(UnityEngine.Random.Range((float) 0.05f, (float) 0.2f));
                    }
                }
                else if (this.state == "turn180")
                {
                    if (base.GetComponent<Animation>()[this.turnAnimation].normalizedTime >= 1f)
                    {
                        base.gameObject.transform.rotation = Quaternion.Euler(base.gameObject.transform.rotation.eulerAngles.x, base.gameObject.transform.rotation.eulerAngles.y + 180f, base.gameObject.transform.rotation.eulerAngles.z);
                        this.idle(0f);
                        this.playAnimation("ft_idle");
                    }
                }
                else if (this.state == "anklehurt")
                {
                    if (base.GetComponent<Animation>()["ft_legHurt"].normalizedTime >= 1f)
                    {
                        this.crossFade("ft_legHurt_loop", 0.2f);
                    }
                    if (base.GetComponent<Animation>()["ft_legHurt_loop"].normalizedTime >= 3f)
                    {
                        this.crossFade("ft_legHurt_getup", 0.2f);
                    }
                    if (base.GetComponent<Animation>()["ft_legHurt_getup"].normalizedTime >= 1f)
                    {
                        this.idle(0f);
                        this.playAnimation("ft_idle");
                    }
                }
            }
        }
    }

    public void updateLabel()
    {
        if ((this.healthLabel != null)) //&& this.healthLabel.GetComponent<UILabel>().isVisible)
        {
            this.healthLabel.transform.LookAt(((Vector3) (2f * this.healthLabel.transform.position)) - Camera.main.transform.position);
        }
    }
}