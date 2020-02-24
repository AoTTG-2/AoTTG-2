using System;
using UnityEngine;

public class BTN_ChangePassword : MonoBehaviour
{
    public GameObject logincomponent;
    public GameObject oldpassword;
    public GameObject output;
    public GameObject password;
    public GameObject password2;

    private void OnClick()
    {
        if (this.password.GetComponent<UIInput>().text.Length < 3)
        {
            this.output.GetComponent<UILabel>().text = "Password too short.";
        }
        else if (this.password.GetComponent<UIInput>().text != this.password2.GetComponent<UIInput>().text)
        {
            this.output.GetComponent<UILabel>().text = "Password does not match the confirm password.";
        }
        else
        {
            this.output.GetComponent<UILabel>().text = "please wait...";
            this.logincomponent.GetComponent<LoginFengKAI>().cpassword(this.oldpassword.GetComponent<UIInput>().text, this.password.GetComponent<UIInput>().text, this.password2.GetComponent<UIInput>().text);
        }
    }
}

