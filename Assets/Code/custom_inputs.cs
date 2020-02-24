using System;
using UnityEngine;

public class custom_inputs : MonoBehaviour
{
    public bool allowDuplicates;
    public KeyCode[] alt_default_inputKeys;
    private float AltInputBox_X = 120f;
    private bool altInputson;
    [HideInInspector]
    public float analogFeel_down;
    public float analogFeel_gravity = 0.2f;
    [HideInInspector]
    public float analogFeel_jump;
    [HideInInspector]
    public float analogFeel_left;
    [HideInInspector]
    public float analogFeel_right;
    public float analogFeel_sensitivity = 0.8f;
    [HideInInspector]
    public float analogFeel_up;
    public float Boxes_Y = 300f;
    public float BoxesMargin_Y = 30f;
    private float buttonHeight = 20f;
    public int buttonSize = 200;
    public KeyCode[] default_inputKeys;
    private float DescBox_X = -320f;
    private float DescriptionBox_X;
    public int DescriptionSize = 200;
    public string[] DescriptionString;
    private bool[] inputBool;
    private bool[] inputBool2;
    private float InputBox_X = -100f;
    private float InputBox1_X;
    private float InputBox2_X;
    private KeyCode[] inputKey;
    private KeyCode[] inputKey2;
    private string[] inputString;
    private string[] inputString2;
    [HideInInspector]
    public bool[] isInput;
    [HideInInspector]
    public bool[] isInputDown;
    [HideInInspector]
    public bool[] isInputUp;
    [HideInInspector]
    public bool[] joystickActive;
    [HideInInspector]
    public bool[] joystickActive2;
    [HideInInspector]
    public string[] joystickString;
    [HideInInspector]
    public string[] joystickString2;
    private float lastInterval;
    public bool menuOn;
    public bool mouseAxisOn;
    public bool mouseButtonsOn = true;
    public GUISkin OurSkin;
    private float resetbuttonLocX = -100f;
    public float resetbuttonLocY = 600f;
    public string resetbuttonText = "Reset to defaults";
    private float resetbuttonX;
    private bool tempbool;
    private bool[] tempjoy1;
    private bool[] tempjoy2;
    public string tempkeyPressed;
    private int tempLength;

    private void checDoubleAxis(string testAxisString, int o, int p)
    {
        if (!this.allowDuplicates)
        {
            for (int i = 0; i < this.DescriptionString.Length; i++)
            {
                if ((testAxisString == this.joystickString[i]) && ((i != o) || (p == 2)))
                {
                    this.inputKey[i] = KeyCode.None;
                    this.inputBool[i] = false;
                    this.inputString[i] = this.inputKey[i].ToString();
                    this.joystickActive[i] = false;
                    this.joystickString[i] = "#";
                    this.saveInputs();
                }
                if ((testAxisString == this.joystickString2[i]) && ((i != o) || (p == 1)))
                {
                    this.inputKey2[i] = KeyCode.None;
                    this.inputBool2[i] = false;
                    this.inputString2[i] = this.inputKey2[i].ToString();
                    this.joystickActive2[i] = false;
                    this.joystickString2[i] = "#";
                    this.saveInputs();
                }
            }
        }
    }

    private void checDoubles(KeyCode testkey, int o, int p)
    {
        if (!this.allowDuplicates)
        {
            for (int i = 0; i < this.DescriptionString.Length; i++)
            {
                if ((testkey == this.inputKey[i]) && ((i != o) || (p == 2)))
                {
                    this.inputKey[i] = KeyCode.None;
                    this.inputBool[i] = false;
                    this.inputString[i] = this.inputKey[i].ToString();
                    this.joystickActive[i] = false;
                    this.joystickString[i] = "#";
                    this.saveInputs();
                }
                if ((testkey == this.inputKey2[i]) && ((i != o) || (p == 1)))
                {
                    this.inputKey2[i] = KeyCode.None;
                    this.inputBool2[i] = false;
                    this.inputString2[i] = this.inputKey2[i].ToString();
                    this.joystickActive2[i] = false;
                    this.joystickString2[i] = "#";
                    this.saveInputs();
                }
            }
        }
    }

    private void drawButtons1()
    {
        float top = this.Boxes_Y;
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;
        Vector3 point = GUI.matrix.inverse.MultiplyPoint3x4(new Vector3(x, Screen.height - y, 1f));
        GUI.skin = this.OurSkin;
        GUI.Box(new Rect(0f, 0f, (float) Screen.width, (float) Screen.height), string.Empty);
        GUI.Box(new Rect(60f, 60f, (float) (Screen.width - 120), (float) (Screen.height - 120)), string.Empty, "window");
        GUI.Label(new Rect(this.DescriptionBox_X, top - 10f, (float) this.DescriptionSize, this.buttonHeight), "name", "textfield");
        GUI.Label(new Rect(this.InputBox1_X, top - 10f, (float) this.DescriptionSize, this.buttonHeight), "input", "textfield");
        GUI.Label(new Rect(this.InputBox2_X, top - 10f, (float) this.DescriptionSize, this.buttonHeight), "alt input", "textfield");
        for (int i = 0; i < this.DescriptionString.Length; i++)
        {
            top += this.BoxesMargin_Y;
            GUI.Label(new Rect(this.DescriptionBox_X, top, (float) this.DescriptionSize, this.buttonHeight), this.DescriptionString[i], "box");
            Rect position = new Rect(this.InputBox1_X, top, (float) this.buttonSize, this.buttonHeight);
            GUI.Button(position, this.inputString[i]);
            if (!this.joystickActive[i] && (this.inputKey[i] == KeyCode.None))
            {
                this.joystickString[i] = "#";
            }
            if (this.inputBool[i])
            {
                GUI.Toggle(position, true, string.Empty, this.OurSkin.button);
            }
            if ((position.Contains(point) && Input.GetMouseButtonUp(0)) && !this.tempbool)
            {
                this.tempbool = true;
                this.inputBool[i] = true;
                this.lastInterval = Time.realtimeSinceStartup;
            }
            if (GUI.Button(new Rect(this.resetbuttonX, this.resetbuttonLocY, (float) this.buttonSize, this.buttonHeight), this.resetbuttonText) && Input.GetMouseButtonUp(0))
            {
                PlayerPrefs.DeleteAll();
                this.reset2defaults();
                this.loadConfig();
                this.saveInputs();
            }
            if (((Event.current.type == EventType.KeyDown) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey[i] = Event.current.keyCode;
                this.inputBool[i] = false;
                this.inputString[i] = this.inputKey[i].ToString();
                this.tempbool = false;
                this.joystickActive[i] = false;
                this.joystickString[i] = "#";
                this.saveInputs();
                this.checDoubles(this.inputKey[i], i, 1);
            }
            if (this.mouseButtonsOn)
            {
                int num5 = 0x143;
                for (int k = 0; k < 6; k++)
                {
                    if ((Input.GetMouseButton(k) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
                    {
                        num5 += k;
                        this.inputKey[i] = (KeyCode) num5;
                        this.inputBool[i] = false;
                        this.inputString[i] = this.inputKey[i].ToString();
                        this.joystickActive[i] = false;
                        this.joystickString[i] = "#";
                        this.saveInputs();
                        this.checDoubles(this.inputKey[i], i, 1);
                    }
                }
            }
            for (int j = 350; j < 0x199; j++)
            {
                if ((Input.GetKey((KeyCode) j) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
                {
                    this.inputKey[i] = (KeyCode) j;
                    this.inputBool[i] = false;
                    this.inputString[i] = this.inputKey[i].ToString();
                    this.tempbool = false;
                    this.joystickActive[i] = false;
                    this.joystickString[i] = "#";
                    this.saveInputs();
                    this.checDoubles(this.inputKey[i], i, 1);
                }
            }
            if (this.mouseAxisOn)
            {
                if (((Input.GetAxis("MouseUp") == 1f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
                {
                    this.inputKey[i] = KeyCode.None;
                    this.inputBool[i] = false;
                    this.joystickActive[i] = true;
                    this.joystickString[i] = "MouseUp";
                    this.inputString[i] = "Mouse Up";
                    this.tempbool = false;
                    this.saveInputs();
                    this.checDoubleAxis(this.joystickString[i], i, 1);
                }
                if (((Input.GetAxis("MouseDown") == 1f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
                {
                    this.inputKey[i] = KeyCode.None;
                    this.inputBool[i] = false;
                    this.joystickActive[i] = true;
                    this.joystickString[i] = "MouseDown";
                    this.inputString[i] = "Mouse Down";
                    this.tempbool = false;
                    this.saveInputs();
                    this.checDoubleAxis(this.joystickString[i], i, 1);
                }
                if (((Input.GetAxis("MouseLeft") == 1f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
                {
                    this.inputKey[i] = KeyCode.None;
                    this.inputBool[i] = false;
                    this.joystickActive[i] = true;
                    this.joystickString[i] = "MouseLeft";
                    this.inputBool[i] = false;
                    this.inputString[i] = "Mouse Left";
                    this.tempbool = false;
                    this.saveInputs();
                    this.checDoubleAxis(this.joystickString[i], i, 1);
                }
                if (((Input.GetAxis("MouseRight") == 1f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
                {
                    this.inputKey[i] = KeyCode.None;
                    this.inputBool[i] = false;
                    this.joystickActive[i] = true;
                    this.joystickString[i] = "MouseRight";
                    this.inputString[i] = "Mouse Right";
                    this.tempbool = false;
                    this.saveInputs();
                    this.checDoubleAxis(this.joystickString[i], i, 1);
                }
            }
            if (this.mouseButtonsOn)
            {
                if (((Input.GetAxis("MouseScrollUp") > 0f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
                {
                    this.inputKey[i] = KeyCode.None;
                    this.inputBool[i] = false;
                    this.joystickActive[i] = true;
                    this.joystickString[i] = "MouseScrollUp";
                    this.inputBool[i] = false;
                    this.inputString[i] = "Mouse scroll Up";
                    this.tempbool = false;
                    this.saveInputs();
                    this.checDoubleAxis(this.joystickString[i], i, 1);
                }
                if (((Input.GetAxis("MouseScrollDown") > 0f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
                {
                    this.inputKey[i] = KeyCode.None;
                    this.inputBool[i] = false;
                    this.joystickActive[i] = true;
                    this.joystickString[i] = "MouseScrollDown";
                    this.inputBool[i] = false;
                    this.inputString[i] = "Mouse scroll Down";
                    this.tempbool = false;
                    this.saveInputs();
                    this.checDoubleAxis(this.joystickString[i], i, 1);
                }
            }
            if (((Input.GetAxis("JoystickUp") > 0.5f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey[i] = KeyCode.None;
                this.inputBool[i] = false;
                this.joystickActive[i] = true;
                this.joystickString[i] = "JoystickUp";
                this.inputString[i] = "Joystick Up";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString[i], i, 1);
            }
            if (((Input.GetAxis("JoystickDown") > 0.5f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey[i] = KeyCode.None;
                this.inputBool[i] = false;
                this.joystickActive[i] = true;
                this.joystickString[i] = "JoystickDown";
                this.inputString[i] = "Joystick Down";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString[i], i, 1);
            }
            if (((Input.GetAxis("JoystickLeft") > 0.5f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey[i] = KeyCode.None;
                this.inputBool[i] = false;
                this.joystickActive[i] = true;
                this.joystickString[i] = "JoystickLeft";
                this.inputString[i] = "Joystick Left";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString[i], i, 1);
            }
            if (((Input.GetAxis("JoystickRight") > 0.5f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey[i] = KeyCode.None;
                this.inputBool[i] = false;
                this.joystickActive[i] = true;
                this.joystickString[i] = "JoystickRight";
                this.inputString[i] = "Joystick Right";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString[i], i, 1);
            }
            if (((Input.GetAxis("Joystick_3a") > 0.8f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey[i] = KeyCode.None;
                this.inputBool[i] = false;
                this.joystickActive[i] = true;
                this.joystickString[i] = "Joystick_3a";
                this.inputString[i] = "Joystick Axis 3 +";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString[i], i, 1);
            }
            if (((Input.GetAxis("Joystick_3b") > 0.8f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey[i] = KeyCode.None;
                this.inputBool[i] = false;
                this.joystickActive[i] = true;
                this.joystickString[i] = "Joystick_3b";
                this.inputString[i] = "Joystick Axis 3 -";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString[i], i, 1);
            }
            if (((Input.GetAxis("Joystick_4a") > 0.8f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey[i] = KeyCode.None;
                this.inputBool[i] = false;
                this.joystickActive[i] = true;
                this.joystickString[i] = "Joystick_4a";
                this.inputString[i] = "Joystick Axis 4 +";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString[i], i, 1);
            }
            if (((Input.GetAxis("Joystick_4b") > 0.8f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey[i] = KeyCode.None;
                this.inputBool[i] = false;
                this.joystickActive[i] = true;
                this.joystickString[i] = "Joystick_4b";
                this.inputString[i] = "Joystick Axis 4 -";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString[i], i, 1);
            }
            if (((Input.GetAxis("Joystick_5b") > 0.8f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey[i] = KeyCode.None;
                this.inputBool[i] = false;
                this.joystickActive[i] = true;
                this.joystickString[i] = "Joystick_5b";
                this.inputString[i] = "Joystick Axis 5 -";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString[i], i, 1);
            }
            if (((Input.GetAxis("Joystick_6b") > 0.8f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey[i] = KeyCode.None;
                this.inputBool[i] = false;
                this.joystickActive[i] = true;
                this.joystickString[i] = "Joystick_6b";
                this.inputString[i] = "Joystick Axis 6 -";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString[i], i, 1);
            }
            if (((Input.GetAxis("Joystick_7a") > 0.8f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey[i] = KeyCode.None;
                this.inputBool[i] = false;
                this.joystickActive[i] = true;
                this.joystickString[i] = "Joystick_7a";
                this.inputString[i] = "Joystick Axis 7 +";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString[i], i, 1);
            }
            if (((Input.GetAxis("Joystick_7b") > 0.8f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey[i] = KeyCode.None;
                this.inputBool[i] = false;
                this.joystickActive[i] = true;
                this.joystickString[i] = "Joystick_7b";
                this.inputString[i] = "Joystick Axis 7 -";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString[i], i, 1);
            }
            if (((Input.GetAxis("Joystick_8a") > 0.8f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey[i] = KeyCode.None;
                this.inputBool[i] = false;
                this.joystickActive[i] = true;
                this.joystickString[i] = "Joystick_8a";
                this.inputString[i] = "Joystick Axis 8 +";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString[i], i, 1);
            }
            if (((Input.GetAxis("Joystick_8b") > 0.8f) && this.inputBool[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey[i] = KeyCode.None;
                this.inputBool[i] = false;
                this.joystickActive[i] = true;
                this.joystickString[i] = "Joystick_8b";
                this.inputString[i] = "Joystick Axis 8 -";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString[i], i, 1);
            }
        }
    }

    private void drawButtons2()
    {
        float top = this.Boxes_Y;
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;
        Vector3 point = GUI.matrix.inverse.MultiplyPoint3x4(new Vector3(x, Screen.height - y, 1f));
        GUI.skin = this.OurSkin;
        for (int i = 0; i < this.DescriptionString.Length; i++)
        {
            top += this.BoxesMargin_Y;
            Rect position = new Rect(this.InputBox2_X, top, (float) this.buttonSize, this.buttonHeight);
            GUI.Button(position, this.inputString2[i]);
            if (!this.joystickActive2[i] && (this.inputKey2[i] == KeyCode.None))
            {
                this.joystickString2[i] = "#";
            }
            if (this.inputBool2[i])
            {
                GUI.Toggle(position, true, string.Empty, this.OurSkin.button);
            }
            if ((position.Contains(point) && Input.GetMouseButtonUp(0)) && !this.tempbool)
            {
                this.tempbool = true;
                this.inputBool2[i] = true;
                this.lastInterval = Time.realtimeSinceStartup;
            }
            if (((Event.current.type == EventType.KeyDown) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey2[i] = Event.current.keyCode;
                this.inputBool2[i] = false;
                this.inputString2[i] = this.inputKey2[i].ToString();
                this.tempbool = false;
                this.joystickActive2[i] = false;
                this.joystickString2[i] = "#";
                this.saveInputs();
                this.checDoubles(this.inputKey2[i], i, 2);
            }
            if (this.mouseButtonsOn)
            {
                int num5 = 0x143;
                for (int k = 0; k < 6; k++)
                {
                    if ((Input.GetMouseButton(k) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
                    {
                        num5 += k;
                        this.inputKey2[i] = (KeyCode) num5;
                        this.inputBool2[i] = false;
                        this.inputString2[i] = this.inputKey2[i].ToString();
                        this.joystickActive2[i] = false;
                        this.joystickString2[i] = "#";
                        this.saveInputs();
                        this.checDoubles(this.inputKey2[i], i, 2);
                    }
                }
            }
            for (int j = 350; j < 0x199; j++)
            {
                if ((Input.GetKey((KeyCode) j) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
                {
                    this.inputKey2[i] = (KeyCode) j;
                    this.inputBool2[i] = false;
                    this.inputString2[i] = this.inputKey2[i].ToString();
                    this.tempbool = false;
                    this.joystickActive2[i] = false;
                    this.joystickString2[i] = "#";
                    this.saveInputs();
                    this.checDoubles(this.inputKey2[i], i, 2);
                }
            }
            if (this.mouseAxisOn)
            {
                if (((Input.GetAxis("MouseUp") == 1f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
                {
                    this.inputKey2[i] = KeyCode.None;
                    this.inputBool2[i] = false;
                    this.joystickActive2[i] = true;
                    this.joystickString2[i] = "MouseUp";
                    this.inputString2[i] = "Mouse Up";
                    this.tempbool = false;
                    this.saveInputs();
                    this.checDoubleAxis(this.joystickString2[i], i, 2);
                }
                if (((Input.GetAxis("MouseDown") == 1f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
                {
                    this.inputKey2[i] = KeyCode.None;
                    this.inputBool2[i] = false;
                    this.joystickActive2[i] = true;
                    this.joystickString2[i] = "MouseDown";
                    this.inputString2[i] = "Mouse Down";
                    this.tempbool = false;
                    this.saveInputs();
                    this.checDoubleAxis(this.joystickString2[i], i, 2);
                }
                if (((Input.GetAxis("MouseLeft") == 1f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
                {
                    this.inputKey2[i] = KeyCode.None;
                    this.inputBool2[i] = false;
                    this.joystickActive2[i] = true;
                    this.joystickString2[i] = "MouseLeft";
                    this.inputBool2[i] = false;
                    this.inputString2[i] = "Mouse Left";
                    this.tempbool = false;
                    this.saveInputs();
                    this.checDoubleAxis(this.joystickString2[i], i, 2);
                }
                if (((Input.GetAxis("MouseRight") == 1f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
                {
                    this.inputKey2[i] = KeyCode.None;
                    this.inputBool2[i] = false;
                    this.joystickActive2[i] = true;
                    this.joystickString2[i] = "MouseRight";
                    this.inputString2[i] = "Mouse Right";
                    this.tempbool = false;
                    this.saveInputs();
                    this.checDoubleAxis(this.joystickString2[i], i, 2);
                }
            }
            if (this.mouseButtonsOn)
            {
                if (((Input.GetAxis("MouseScrollUp") > 0f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
                {
                    this.inputKey2[i] = KeyCode.None;
                    this.inputBool2[i] = false;
                    this.joystickActive2[i] = true;
                    this.joystickString2[i] = "MouseScrollUp";
                    this.inputBool2[i] = false;
                    this.inputString2[i] = "Mouse scroll Up";
                    this.tempbool = false;
                    this.saveInputs();
                    this.checDoubleAxis(this.joystickString2[i], i, 2);
                }
                if (((Input.GetAxis("MouseScrollDown") > 0f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
                {
                    this.inputKey2[i] = KeyCode.None;
                    this.inputBool2[i] = false;
                    this.joystickActive2[i] = true;
                    this.joystickString2[i] = "MouseScrollDown";
                    this.inputBool2[i] = false;
                    this.inputString2[i] = "Mouse scroll Down";
                    this.tempbool = false;
                    this.saveInputs();
                    this.checDoubleAxis(this.joystickString2[i], i, 2);
                }
            }
            if (((Input.GetAxis("JoystickUp") > 0.5f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey2[i] = KeyCode.None;
                this.inputBool2[i] = false;
                this.joystickActive2[i] = true;
                this.joystickString2[i] = "JoystickUp";
                this.inputString2[i] = "Joystick Up";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString2[i], i, 2);
            }
            if (((Input.GetAxis("JoystickDown") > 0.5f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey2[i] = KeyCode.None;
                this.inputBool2[i] = false;
                this.joystickActive2[i] = true;
                this.joystickString2[i] = "JoystickDown";
                this.inputString2[i] = "Joystick Down";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString2[i], i, 2);
            }
            if (((Input.GetAxis("JoystickLeft") > 0.5f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey2[i] = KeyCode.None;
                this.inputBool2[i] = false;
                this.joystickActive2[i] = true;
                this.joystickString2[i] = "JoystickLeft";
                this.inputBool2[i] = false;
                this.inputString2[i] = "Joystick Left";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString2[i], i, 2);
            }
            if (((Input.GetAxis("JoystickRight") > 0.5f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey2[i] = KeyCode.None;
                this.inputBool2[i] = false;
                this.joystickActive2[i] = true;
                this.joystickString2[i] = "JoystickRight";
                this.inputString2[i] = "Joystick Right";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString2[i], i, 2);
            }
            if (((Input.GetAxis("Joystick_3a") > 0.8f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey2[i] = KeyCode.None;
                this.inputBool2[i] = false;
                this.joystickActive2[i] = true;
                this.joystickString2[i] = "Joystick_3a";
                this.inputString2[i] = "Joystick Axis 3 +";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString2[i], i, 2);
            }
            if (((Input.GetAxis("Joystick_3b") > 0.8f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey2[i] = KeyCode.None;
                this.inputBool2[i] = false;
                this.joystickActive2[i] = true;
                this.joystickString2[i] = "Joystick_3b";
                this.inputString2[i] = "Joystick Axis 3 -";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString2[i], i, 2);
            }
            if (((Input.GetAxis("Joystick_4a") > 0.8f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey2[i] = KeyCode.None;
                this.inputBool2[i] = false;
                this.joystickActive2[i] = true;
                this.joystickString2[i] = "Joystick_4a";
                this.inputString2[i] = "Joystick Axis 4 +";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString2[i], i, 2);
            }
            if (((Input.GetAxis("Joystick_4b") > 0.8f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey2[i] = KeyCode.None;
                this.inputBool2[i] = false;
                this.joystickActive2[i] = true;
                this.joystickString2[i] = "Joystick_4b";
                this.inputString2[i] = "Joystick Axis 4 -";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString2[i], i, 2);
            }
            if (((Input.GetAxis("Joystick_5b") > 0.8f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey2[i] = KeyCode.None;
                this.inputBool2[i] = false;
                this.joystickActive2[i] = true;
                this.joystickString2[i] = "Joystick_5b";
                this.inputString2[i] = "Joystick Axis 5 -";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString2[i], i, 2);
            }
            if (((Input.GetAxis("Joystick_6b") > 0.8f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey2[i] = KeyCode.None;
                this.inputBool2[i] = false;
                this.joystickActive2[i] = true;
                this.joystickString2[i] = "Joystick_6b";
                this.inputString2[i] = "Joystick Axis 6 -";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString2[i], i, 2);
            }
            if (((Input.GetAxis("Joystick_7a") > 0.8f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey2[i] = KeyCode.None;
                this.inputBool2[i] = false;
                this.joystickActive2[i] = true;
                this.joystickString2[i] = "Joystick_7a";
                this.inputString2[i] = "Joystick Axis 7 +";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString2[i], i, 2);
            }
            if (((Input.GetAxis("Joystick_7b") > 0.8f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey2[i] = KeyCode.None;
                this.inputBool2[i] = false;
                this.joystickActive2[i] = true;
                this.joystickString2[i] = "Joystick_7b";
                this.inputString2[i] = "Joystick Axis 7 -";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString2[i], i, 2);
            }
            if (((Input.GetAxis("Joystick_8a") > 0.8f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey2[i] = KeyCode.None;
                this.inputBool2[i] = false;
                this.joystickActive2[i] = true;
                this.joystickString2[i] = "Joystick_8a";
                this.inputString2[i] = "Joystick Axis 8 +";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString2[i], i, 2);
            }
            if (((Input.GetAxis("Joystick_8b") > 0.8f) && this.inputBool2[i]) && (Event.current.keyCode != KeyCode.Escape))
            {
                this.inputKey2[i] = KeyCode.None;
                this.inputBool2[i] = false;
                this.joystickActive2[i] = true;
                this.joystickString2[i] = "Joystick_8b";
                this.inputString2[i] = "Joystick Axis 8 -";
                this.tempbool = false;
                this.saveInputs();
                this.checDoubleAxis(this.joystickString2[i], i, 2);
            }
        }
    }

    private void inputSetBools()
    {
        for (int i = 0; i < this.DescriptionString.Length; i++)
        {
            if ((!Input.GetKey(this.inputKey[i]) && (!this.joystickActive[i] || (Input.GetAxis(this.joystickString[i]) <= 0.95f))) && (!Input.GetKey(this.inputKey2[i]) && (!this.joystickActive2[i] || (Input.GetAxis(this.joystickString2[i]) <= 0.95f))))
            {
                this.isInput[i] = false;
            }
            else
            {
                this.isInput[i] = true;
            }
            if (!Input.GetKeyDown(this.inputKey[i]) && !Input.GetKeyDown(this.inputKey2[i]))
            {
                this.isInputDown[i] = false;
            }
            else
            {
                this.isInputDown[i] = true;
            }
            if ((this.joystickActive[i] && (Input.GetAxis(this.joystickString[i]) > 0.95f)) || (this.joystickActive2[i] && (Input.GetAxis(this.joystickString2[i]) > 0.95f)))
            {
                if (!this.tempjoy1[i])
                {
                    this.isInputDown[i] = false;
                }
                if (this.tempjoy1[i])
                {
                    this.isInputDown[i] = true;
                    this.tempjoy1[i] = false;
                }
            }
            if (((!this.tempjoy1[i] && this.joystickActive[i]) && ((Input.GetAxis(this.joystickString[i]) < 0.1f) && this.joystickActive2[i])) && (Input.GetAxis(this.joystickString2[i]) < 0.1f))
            {
                this.isInputDown[i] = false;
                this.tempjoy1[i] = true;
            }
            if ((!this.tempjoy1[i] && !this.joystickActive[i]) && (this.joystickActive2[i] && (Input.GetAxis(this.joystickString2[i]) < 0.1f)))
            {
                this.isInputDown[i] = false;
                this.tempjoy1[i] = true;
            }
            if ((!this.tempjoy1[i] && !this.joystickActive2[i]) && (this.joystickActive[i] && (Input.GetAxis(this.joystickString[i]) < 0.1f)))
            {
                this.isInputDown[i] = false;
                this.tempjoy1[i] = true;
            }
            if (!Input.GetKeyUp(this.inputKey[i]) && !Input.GetKeyUp(this.inputKey2[i]))
            {
                this.isInputUp[i] = false;
            }
            else
            {
                this.isInputUp[i] = true;
            }
            if ((this.joystickActive[i] && (Input.GetAxis(this.joystickString[i]) > 0.95f)) || (this.joystickActive2[i] && (Input.GetAxis(this.joystickString2[i]) > 0.95f)))
            {
                if (this.tempjoy2[i])
                {
                    this.isInputUp[i] = false;
                }
                if (!this.tempjoy2[i])
                {
                    this.isInputUp[i] = false;
                    this.tempjoy2[i] = true;
                }
            }
            if (((this.tempjoy2[i] && this.joystickActive[i]) && ((Input.GetAxis(this.joystickString[i]) < 0.1f) && this.joystickActive2[i])) && (Input.GetAxis(this.joystickString2[i]) < 0.1f))
            {
                this.isInputUp[i] = true;
                this.tempjoy2[i] = false;
            }
            if ((this.tempjoy2[i] && !this.joystickActive[i]) && (this.joystickActive2[i] && (Input.GetAxis(this.joystickString2[i]) < 0.1f)))
            {
                this.isInputUp[i] = true;
                this.tempjoy2[i] = false;
            }
            if ((this.tempjoy2[i] && !this.joystickActive2[i]) && (this.joystickActive[i] && (Input.GetAxis(this.joystickString[i]) < 0.1f)))
            {
                this.isInputUp[i] = true;
                this.tempjoy2[i] = false;
            }
        }
    }

    private void loadConfig()
    {
        string str = PlayerPrefs.GetString("KeyCodes");
        string str2 = PlayerPrefs.GetString("Joystick_input");
        string str3 = PlayerPrefs.GetString("Names_input");
        string str4 = PlayerPrefs.GetString("KeyCodes2");
        string str5 = PlayerPrefs.GetString("Joystick_input2");
        string str6 = PlayerPrefs.GetString("Names_input2");
        char[] separator = new char[] { '*' };
        string[] strArray = str.Split(separator);
        char[] chArray2 = new char[] { '*' };
        this.joystickString = str2.Split(chArray2);
        char[] chArray3 = new char[] { '*' };
        this.inputString = str3.Split(chArray3);
        char[] chArray4 = new char[] { '*' };
        string[] strArray2 = str4.Split(chArray4);
        char[] chArray5 = new char[] { '*' };
        this.joystickString2 = str5.Split(chArray5);
        char[] chArray6 = new char[] { '*' };
        this.inputString2 = str6.Split(chArray6);
        for (int i = 0; i < this.DescriptionString.Length; i++)
        {
            int num2;
            int num3;
            int.TryParse(strArray[i], out num2);
            this.inputKey[i] = (KeyCode) num2;
            int.TryParse(strArray2[i], out num3);
            this.inputKey2[i] = (KeyCode) num3;
            if (this.joystickString[i] == "#")
            {
                this.joystickActive[i] = false;
            }
            else
            {
                this.joystickActive[i] = true;
            }
            if (this.joystickString2[i] == "#")
            {
                this.joystickActive2[i] = false;
            }
            else
            {
                this.joystickActive2[i] = true;
            }
        }
    }

    private void OnGUI()
    {
        if (Time.realtimeSinceStartup > (this.lastInterval + 3f))
        {
            this.tempbool = false;
        }
        if (this.menuOn)
        {
            this.drawButtons1();
            if (this.altInputson)
            {
                this.drawButtons2();
            }
        }
    }

    private void reset2defaults()
    {
        if (this.default_inputKeys.Length != this.DescriptionString.Length)
        {
            this.default_inputKeys = new KeyCode[this.DescriptionString.Length];
        }
        if (this.alt_default_inputKeys.Length != this.default_inputKeys.Length)
        {
            this.alt_default_inputKeys = new KeyCode[this.default_inputKeys.Length];
        }
        string str = string.Empty;
        string str2 = string.Empty;
        string str3 = string.Empty;
        string str4 = string.Empty;
        string str5 = string.Empty;
        string str6 = string.Empty;
        for (int i = this.DescriptionString.Length - 1; i > -1; i--)
        {
            str = ((int) this.default_inputKeys[i]) + "*" + str;
            str2 = str2 + "#*";
            str3 = this.default_inputKeys[i].ToString() + "*" + str3;
            PlayerPrefs.SetString("KeyCodes", str);
            PlayerPrefs.SetString("Joystick_input", str2);
            PlayerPrefs.SetString("Names_input", str3);
            str4 = ((int) this.alt_default_inputKeys[i]) + "*" + str4;
            str5 = str5 + "#*";
            str6 = this.alt_default_inputKeys[i].ToString() + "*" + str6;
            PlayerPrefs.SetString("KeyCodes2", str4);
            PlayerPrefs.SetString("Joystick_input2", str5);
            PlayerPrefs.SetString("Names_input2", str6);
            PlayerPrefs.SetInt("KeyLength", this.DescriptionString.Length);
        }
    }

    private void saveInputs()
    {
        string str = string.Empty;
        string str2 = string.Empty;
        string str3 = string.Empty;
        string str4 = string.Empty;
        string str5 = string.Empty;
        string str6 = string.Empty;
        for (int i = this.DescriptionString.Length - 1; i > -1; i--)
        {
            str = ((int) this.inputKey[i]) + "*" + str;
            str2 = this.joystickString[i] + "*" + str2;
            str3 = this.inputString[i] + "*" + str3;
            str4 = ((int) this.inputKey2[i]) + "*" + str4;
            str5 = this.joystickString2[i] + "*" + str5;
            str6 = this.inputString2[i] + "*" + str6;
        }
        PlayerPrefs.SetString("KeyCodes", str);
        PlayerPrefs.SetString("Joystick_input", str2);
        PlayerPrefs.SetString("Names_input", str3);
        PlayerPrefs.SetString("KeyCodes2", str4);
        PlayerPrefs.SetString("Joystick_input2", str5);
        PlayerPrefs.SetString("Names_input2", str6);
        PlayerPrefs.SetInt("KeyLength", this.DescriptionString.Length);
    }

    private void Start()
    {
        if (this.alt_default_inputKeys.Length == this.default_inputKeys.Length)
        {
            this.altInputson = true;
        }
        this.inputBool = new bool[this.DescriptionString.Length];
        this.inputString = new string[this.DescriptionString.Length];
        this.inputKey = new KeyCode[this.DescriptionString.Length];
        this.joystickActive = new bool[this.DescriptionString.Length];
        this.joystickString = new string[this.DescriptionString.Length];
        this.inputBool2 = new bool[this.DescriptionString.Length];
        this.inputString2 = new string[this.DescriptionString.Length];
        this.inputKey2 = new KeyCode[this.DescriptionString.Length];
        this.joystickActive2 = new bool[this.DescriptionString.Length];
        this.joystickString2 = new string[this.DescriptionString.Length];
        this.isInput = new bool[this.DescriptionString.Length];
        this.isInputDown = new bool[this.DescriptionString.Length];
        this.isInputUp = new bool[this.DescriptionString.Length];
        this.tempLength = PlayerPrefs.GetInt("KeyLength");
        this.tempjoy1 = new bool[this.DescriptionString.Length];
        this.tempjoy2 = new bool[this.DescriptionString.Length];
        if (!PlayerPrefs.HasKey("KeyCodes") || !PlayerPrefs.HasKey("KeyCodes2"))
        {
            this.reset2defaults();
        }
        this.tempLength = PlayerPrefs.GetInt("KeyLength");
        if (PlayerPrefs.HasKey("KeyCodes") && (this.tempLength == this.DescriptionString.Length))
        {
            this.loadConfig();
        }
        else
        {
            PlayerPrefs.DeleteAll();
            this.reset2defaults();
            this.loadConfig();
            this.saveInputs();
        }
        for (int i = 0; i < this.DescriptionString.Length; i++)
        {
            this.isInput[i] = false;
            this.isInputDown[i] = false;
            this.isInputUp[i] = false;
            this.tempjoy1[i] = true;
            this.tempjoy2[i] = false;
        }
    }

    private void Update()
    {
        this.DescriptionBox_X = (Screen.width / 2) + this.DescBox_X;
        this.InputBox1_X = (Screen.width / 2) + this.InputBox_X;
        this.InputBox2_X = (Screen.width / 2) + this.AltInputBox_X;
        this.resetbuttonX = (Screen.width / 2) + this.resetbuttonLocX;
        if (!this.menuOn)
        {
            this.inputSetBools();
        }
        if (Input.GetKeyDown("escape"))
        {
            if (this.menuOn)
            {
                Time.timeScale = 1f;
                this.tempbool = false;
                this.menuOn = false;
                this.saveInputs();
            }
            else
            {
                Time.timeScale = 0f;
                this.menuOn = true;
                Cursor.visible = true;
                Screen.lockCursor = false;
            }
        }
    }
}

