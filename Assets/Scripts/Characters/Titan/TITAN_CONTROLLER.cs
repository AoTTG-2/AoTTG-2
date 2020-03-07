using System;
using UnityEngine;

public class TITAN_CONTROLLER : MonoBehaviour
{
    public bool bite;
    public bool bitel;
    public bool biter;
    public bool chopl;
    public bool chopr;
    public bool choptl;
    public bool choptr;
    public bool cover;
    public Camera currentCamera;
    public float currentDirection;
    public bool grabbackl;
    public bool grabbackr;
    public bool grabfrontl;
    public bool grabfrontr;
    public bool grabnapel;
    public bool grabnaper;
    public FengCustomInputs inputManager;
    public bool isAttackDown;
    public bool isAttackIIDown;
    public bool isHorse;
    public bool isJumpDown;
    public bool isSuicide;
    public bool isWALKDown;
    public bool sit;
    public float targetDirection;

    private void Start()
    {
        this.inputManager = GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();
        this.currentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            base.enabled = false;
        }
    }

    private void Update()
    {
        int num;
        int num2;
        float y;
        float num4;
        float num5;
        float num6;
        if (this.isHorse)
        {
            if (FengGameManagerMKII.inputRC.isInputHorse(InputCodeRC.horseForward))
            {
                num = 1;
            }
            else if (FengGameManagerMKII.inputRC.isInputHorse(InputCodeRC.horseBack))
            {
                num = -1;
            }
            else
            {
                num = 0;
            }
            if (FengGameManagerMKII.inputRC.isInputHorse(InputCodeRC.horseLeft))
            {
                num2 = -1;
            }
            else if (FengGameManagerMKII.inputRC.isInputHorse(InputCodeRC.horseRight))
            {
                num2 = 1;
            }
            else
            {
                num2 = 0;
            }
            if ((num2 != 0) || (num != 0))
            {
                y = this.currentCamera.transform.rotation.eulerAngles.y;
                num4 = Mathf.Atan2((float) num, (float) num2) * 57.29578f;
                num4 = -num4 + 90f;
                num5 = y + num4;
                this.targetDirection = num5;
            }
            else
            {
                this.targetDirection = -874f;
            }
            this.isAttackDown = false;
            this.isAttackIIDown = false;
            if (this.targetDirection != -874f)
            {
                this.currentDirection = this.targetDirection;
            }
            num6 = this.currentCamera.transform.rotation.eulerAngles.y - this.currentDirection;
            if (num6 >= 180f)
            {
                num6 -= 360f;
            }
            if (FengGameManagerMKII.inputRC.isInputHorse(InputCodeRC.horseJump))
            {
                this.isAttackDown = true;
            }
            this.isWALKDown = FengGameManagerMKII.inputRC.isInputHorse(InputCodeRC.horseWalk);
        }
        else
        {
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanForward))
            {
                num = 1;
            }
            else if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanBack))
            {
                num = -1;
            }
            else
            {
                num = 0;
            }
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanLeft))
            {
                num2 = -1;
            }
            else if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanRight))
            {
                num2 = 1;
            }
            else
            {
                num2 = 0;
            }
            if ((num2 != 0) || (num != 0))
            {
                y = this.currentCamera.transform.rotation.eulerAngles.y;
                num4 = Mathf.Atan2((float) num, (float) num2) * 57.29578f;
                num4 = -num4 + 90f;
                num5 = y + num4;
                this.targetDirection = num5;
            }
            else
            {
                this.targetDirection = -874f;
            }
            this.isAttackDown = false;
            this.isJumpDown = false;
            this.isAttackIIDown = false;
            this.isSuicide = false;
            this.grabbackl = false;
            this.grabbackr = false;
            this.grabfrontl = false;
            this.grabfrontr = false;
            this.grabnapel = false;
            this.grabnaper = false;
            this.choptl = false;
            this.chopr = false;
            this.chopl = false;
            this.choptr = false;
            this.bite = false;
            this.bitel = false;
            this.biter = false;
            this.cover = false;
            this.sit = false;
            if (this.targetDirection != -874f)
            {
                this.currentDirection = this.targetDirection;
            }
            num6 = this.currentCamera.transform.rotation.eulerAngles.y - this.currentDirection;
            if (num6 >= 180f)
            {
                num6 -= 360f;
            }
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanPunch))
            {
                this.isAttackDown = true;
            }
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanSlam))
            {
                this.isAttackIIDown = true;
            }
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanJump))
            {
                this.isJumpDown = true;
            }
            if (this.inputManager.GetComponent<FengCustomInputs>().isInputDown[InputCode.restart])
            {
                this.isSuicide = true;
            }
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanCover))
            {
                this.cover = true;
            }
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanSit))
            {
                this.sit = true;
            }
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanGrabFront) && (num6 >= 0f))
            {
                this.grabfrontr = true;
            }
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanGrabFront) && (num6 < 0f))
            {
                this.grabfrontl = true;
            }
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanGrabBack) && (num6 >= 0f))
            {
                this.grabbackr = true;
            }
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanGrabBack) && (num6 < 0f))
            {
                this.grabbackl = true;
            }
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanGrabNape) && (num6 >= 0f))
            {
                this.grabnaper = true;
            }
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanGrabNape) && (num6 < 0f))
            {
                this.grabnapel = true;
            }
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanAntiAE) && (num6 >= 0f))
            {
                this.choptr = true;
            }
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanAntiAE) && (num6 < 0f))
            {
                this.choptl = true;
            }
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanBite) && (num6 > 7.5f))
            {
                this.biter = true;
            }
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanBite) && (num6 < -7.5f))
            {
                this.bitel = true;
            }
            if ((FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanBite) && (num6 >= -7.5f)) && (num6 <= 7.5f))
            {
                this.bite = true;
            }
            this.isWALKDown = FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanWalk);
        }
    }
}

