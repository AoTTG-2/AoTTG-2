using System;
using UnityEngine;

public class FengCustomInputs : MonoBehaviour
{
    public bool allowDuplicates;
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
    public KeyCode[] default_inputKeys;
    public string[] DescriptionString;
    private bool[] inputBool;
    public KeyCode[] inputKey;
    public string[] inputString;
    [HideInInspector]
    public bool[] isInput;
    [HideInInspector]
    public bool[] isInputDown;
    [HideInInspector]
    public bool[] isInputUp;
    [HideInInspector]
    public bool[] joystickActive;
    [HideInInspector]
    public string[] joystickString;
    private float lastInterval;
    public bool menuOn = true;
    public bool mouseAxisOn;
    public bool mouseButtonsOn = true;
    public GUISkin OurSkin;
    public bool tempbool;
    private bool[] tempjoy1;
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
            }
        }
    }

    private void drawButtons1()
    {
        bool flag = false;
        for (int i = 0; i < this.DescriptionString.Length; i++)
        {
            if (!this.joystickActive[i] && (this.inputKey[i] == KeyCode.None))
            {
                this.joystickString[i] = "#";
            }
            bool flag2 = this.inputBool[i];
            if ((Event.current.type == EventType.KeyDown) && this.inputBool[i])
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
                int num2 = 0x143;
                for (int k = 0; k < 6; k++)
                {
                    if (Input.GetMouseButton(k) && this.inputBool[i])
                    {
                        num2 += k;
                        this.inputKey[i] = (KeyCode) num2;
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
                if (Input.GetKey((KeyCode) j) && this.inputBool[i])
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
                if ((Input.GetAxis("MouseUp") == 1f) && this.inputBool[i])
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
                if ((Input.GetAxis("MouseDown") == 1f) && this.inputBool[i])
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
                if ((Input.GetAxis("MouseLeft") == 1f) && this.inputBool[i])
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
                if ((Input.GetAxis("MouseRight") == 1f) && this.inputBool[i])
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
                if ((Input.GetAxis("MouseScrollUp") > 0f) && this.inputBool[i])
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
                if ((Input.GetAxis("MouseScrollDown") > 0f) && this.inputBool[i])
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
            if ((Input.GetAxis("JoystickUp") > 0.5f) && this.inputBool[i])
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
            if ((Input.GetAxis("JoystickDown") > 0.5f) && this.inputBool[i])
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
            if ((Input.GetAxis("JoystickLeft") > 0.5f) && this.inputBool[i])
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
            if ((Input.GetAxis("JoystickRight") > 0.5f) && this.inputBool[i])
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
            if ((Input.GetAxis("Joystick_3a") > 0.8f) && this.inputBool[i])
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
            if ((Input.GetAxis("Joystick_3b") > 0.8f) && this.inputBool[i])
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
            if ((Input.GetAxis("Joystick_4a") > 0.8f) && this.inputBool[i])
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
            if ((Input.GetAxis("Joystick_4b") > 0.8f) && this.inputBool[i])
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
            if ((Input.GetAxis("Joystick_5b") > 0.8f) && this.inputBool[i])
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
            if ((Input.GetAxis("Joystick_6b") > 0.8f) && this.inputBool[i])
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
            if ((Input.GetAxis("Joystick_7a") > 0.8f) && this.inputBool[i])
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
            if ((Input.GetAxis("Joystick_7b") > 0.8f) && this.inputBool[i])
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
            if ((Input.GetAxis("Joystick_8a") > 0.8f) && this.inputBool[i])
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
            if ((Input.GetAxis("Joystick_8b") > 0.8f) && this.inputBool[i])
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
            if (flag2 != this.inputBool[i])
            {
                flag = true;
            }
        }
        if (flag)
        {
            this.showKeyMap();
        }
    }

    public string getKeyRC(int i)
    {
        return this.inputString[i];
    }

    private void inputSetBools()
    {
        for (int i = 0; i < this.DescriptionString.Length; i++)
        {
            if (!Input.GetKey(this.inputKey[i]) && (!this.joystickActive[i] || (Input.GetAxis(this.joystickString[i]) <= 0.95f)))
            {
                this.isInput[i] = false;
            }
            else
            {
                this.isInput[i] = true;
            }
            if (Input.GetKeyDown(this.inputKey[i]))
            {
                this.isInputDown[i] = true;
            }
            else
            {
                this.isInputDown[i] = false;
            }
            if (this.joystickActive[i] && (Input.GetAxis(this.joystickString[i]) > 0.95f))
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
            if ((!this.tempjoy1[i] && this.joystickActive[i]) && (Input.GetAxis(this.joystickString[i]) < 0.1f))
            {
                this.isInputDown[i] = false;
                this.tempjoy1[i] = true;
            }
            if (Input.GetKeyUp(this.inputKey[i]))
            {
                this.isInputUp[i] = true;
            }
            else
            {
                this.isInputUp[i] = false;
            }
        }
    }

    public void justUPDATEME()
    {
        this.Update();
    }

    private void loadConfig()
    {
        string str = PlayerPrefs.GetString("KeyCodes");
        string str2 = PlayerPrefs.GetString("Joystick_input");
        string str3 = PlayerPrefs.GetString("Names_input");
        char[] separator = new char[] { '*' };
        string[] strArray = str.Split(separator);
        char[] chArray2 = new char[] { '*' };
        this.joystickString = str2.Split(chArray2);
        char[] chArray3 = new char[] { '*' };
        this.inputString = str3.Split(chArray3);
        for (int i = 0; i < this.DescriptionString.Length; i++)
        {
            int num2;
            int.TryParse(strArray[i], out num2);
            this.inputKey[i] = (KeyCode) num2;
            if (this.joystickString[i] == "#")
            {
                this.joystickActive[i] = false;
            }
            else
            {
                this.joystickActive[i] = true;
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
        }
    }

    private void reset2defaults()
    {
        if (this.default_inputKeys.Length != this.DescriptionString.Length)
        {
            this.default_inputKeys = new KeyCode[this.DescriptionString.Length];
        }
        string str = string.Empty;
        string str2 = string.Empty;
        string str3 = string.Empty;
        for (int i = this.DescriptionString.Length - 1; i > -1; i--)
        {
            str = ((int) this.default_inputKeys[i]) + "*" + str;
            str2 = str2 + "#*";
            str3 = this.default_inputKeys[i].ToString() + "*" + str3;
            PlayerPrefs.SetString("KeyCodes", str);
            PlayerPrefs.SetString("Joystick_input", str2);
            PlayerPrefs.SetString("Names_input", str3);
            PlayerPrefs.SetInt("KeyLength", this.DescriptionString.Length);
        }
    }

    private void saveInputs()
    {
        string str = string.Empty;
        string str2 = string.Empty;
        string str3 = string.Empty;
        for (int i = this.DescriptionString.Length - 1; i > -1; i--)
        {
            str = ((int) this.inputKey[i]) + "*" + str;
            str2 = this.joystickString[i] + "*" + str2;
            str3 = this.inputString[i] + "*" + str3;
        }
        PlayerPrefs.SetString("KeyCodes", str);
        PlayerPrefs.SetString("Joystick_input", str2);
        PlayerPrefs.SetString("Names_input", str3);
        PlayerPrefs.SetInt("KeyLength", this.DescriptionString.Length);
    }

    public void setKeyRC(int i, string setting)
    {
        if ((setting == "Scroll Up") || (setting == "Scroll Down"))
        {
            if (setting == "Scroll Up")
            {
                this.joystickString[i] = "MouseScrollUp";
                this.inputString[i] = "Mouse scroll Up";
            }
            else if (setting == "Scroll Down")
            {
                this.joystickString[i] = "MouseScrollDown";
                this.inputString[i] = "Mouse scroll Down";
            }
            this.inputKey[i] = KeyCode.None;
            this.inputBool[i] = false;
            this.joystickActive[i] = true;
            this.inputBool[i] = false;
            this.tempbool = false;
            this.saveInputs();
            this.checDoubleAxis(this.joystickString[i], i, 1);
        }
        else
        {
            this.inputKey[i] = (KeyCode) Enum.Parse(typeof(KeyCode), setting);
            this.inputBool[i] = false;
            this.inputString[i] = this.inputKey[i].ToString();
            this.tempbool = false;
            this.joystickActive[i] = false;
            this.joystickString[i] = "#";
            this.saveInputs();
            this.checDoubles(this.inputKey[i], i, 1);
        }
    }

    public void setNameRC(int i, string str)
    {
        this.inputString[i] = str;
    }

    public void setToDefault()
    {
        this.reset2defaults();
        this.loadConfig();
        this.saveInputs();
        PlayerPrefs.SetFloat("MouseSensitivity", 0.5f);
        PlayerPrefs.SetString("version", UIMainReferences.version);
        PlayerPrefs.SetInt("invertMouseY", 1);
        PlayerPrefs.SetInt("cameraTilt", 1);
        PlayerPrefs.SetFloat("GameQuality", 0.9f);
    }

    public void showKeyMap()
    {
        for (int i = 0; i < this.DescriptionString.Length; i++)
        {
            if (GameObject.Find("CInput" + i) != null)
            {
                GameObject.Find("CInput" + i).transform.Find("Label").gameObject.GetComponent<UILabel>().text = this.inputString[i];
            }
        }
        if (GameObject.Find("ChangeQuality") != null)
        {
            GameObject.Find("ChangeQuality").GetComponent<UISlider>().sliderValue = PlayerPrefs.GetFloat("GameQuality");
        }
        if (GameObject.Find("MouseSensitivity") != null)
        {
            GameObject.Find("MouseSensitivity").GetComponent<UISlider>().sliderValue = PlayerPrefs.GetFloat("MouseSensitivity");
        }
        if (GameObject.Find("CheckboxSettings") != null)
        {
            GameObject.Find("CheckboxSettings").GetComponent<UICheckbox>().isChecked = PlayerPrefs.GetInt("invertMouseY") != 1;
        }
        if (GameObject.Find("CheckboxCameraTilt") != null)
        {
            GameObject.Find("CheckboxCameraTilt").GetComponent<UICheckbox>().isChecked = PlayerPrefs.GetInt("cameraTilt") == 1;
        }
    }

    private void Start()
    {
        this.inputBool = new bool[this.DescriptionString.Length];
        this.inputString = new string[this.DescriptionString.Length];
        this.inputKey = new KeyCode[this.DescriptionString.Length];
        this.joystickActive = new bool[this.DescriptionString.Length];
        this.joystickString = new string[this.DescriptionString.Length];
        this.isInput = new bool[this.DescriptionString.Length];
        this.isInputDown = new bool[this.DescriptionString.Length];
        this.isInputUp = new bool[this.DescriptionString.Length];
        this.tempLength = PlayerPrefs.GetInt("KeyLength");
        this.tempjoy1 = new bool[this.DescriptionString.Length];
        if (!PlayerPrefs.HasKey("version"))
        {
            this.setToDefault();
        }
        this.tempLength = PlayerPrefs.GetInt("KeyLength");
        if (PlayerPrefs.HasKey("KeyCodes") && (this.tempLength == this.DescriptionString.Length))
        {
            this.loadConfig();
        }
        else
        {
            this.setToDefault();
        }
        for (int i = 0; i < this.DescriptionString.Length; i++)
        {
            this.isInput[i] = false;
            this.isInputDown[i] = false;
            this.isInputUp[i] = false;
            this.tempjoy1[i] = true;
        }
    }

    public void startListening(int n)
    {
        this.tempbool = true;
        this.inputBool[n] = true;
        this.lastInterval = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        if (!this.menuOn)
        {
            this.inputSetBools();
        }
    }
}

