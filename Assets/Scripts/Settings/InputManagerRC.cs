using System;
using UnityEngine;

//[Obsolete("Use InputManager instead", true)]
public class InputManagerRC
{
    public KeyCode[] humanKeys;
    public int[] humanWheel;

    public InputManagerRC()
    {
        int num;
        this.humanWheel = new int[8];
        this.humanKeys = new KeyCode[8];
        for (num = 0; num < this.humanWheel.Length; num++)
        {
            this.humanWheel[num] = 0;
            this.humanKeys[num] = KeyCode.None;
        }
    }

    public bool isInputHuman(int code)
    {
        if (this.humanWheel[code] != 0)
        {
            return ((Input.GetAxis("Mouse ScrollWheel") * this.humanWheel[code]) > 0f);
        }
        return Input.GetKey(this.humanKeys[code]);
    }

    public bool isInputHumanDown(int code)
    {
        if (this.humanWheel[code] != 0)
        {
            return ((Input.GetAxis("Mouse ScrollWheel") * this.humanWheel[code]) > 0f);
        }
        return Input.GetKeyDown(this.humanKeys[code]);
    }

    public void setInputHuman(int code, string setting)
    {
        this.humanKeys[code] = KeyCode.None;
        this.humanWheel[code] = 0;
        if (setting == "Scroll Up")
        {
            this.humanWheel[code] = 1;
        }
        else if (setting == "Scroll Down")
        {
            this.humanWheel[code] = -1;
        }
        else if (Enum.IsDefined(typeof(KeyCode), setting))
        {
            this.humanKeys[code] = (KeyCode) Enum.Parse(typeof(KeyCode), setting);
        }
    }
}
