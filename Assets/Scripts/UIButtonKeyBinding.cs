using System;
using UnityEngine;

[AddComponentMenu("Game/UI/Button Key Binding")]
public class UIButtonKeyBinding : MonoBehaviour
{
    public KeyCode keyCode;

    private void Update()
    {
        if (!UICamera.inputHasFocus && (this.keyCode != KeyCode.None))
        {
            if (Input.GetKeyDown(this.keyCode))
            {
                base.SendMessage("OnPress", true, SendMessageOptions.DontRequireReceiver);
            }
            if (Input.GetKeyUp(this.keyCode))
            {
                base.SendMessage("OnPress", false, SendMessageOptions.DontRequireReceiver);
                base.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}

