using System;
using UnityEngine;

public class BTN_LOGIN : MonoBehaviour
{
    public GameObject logincomponent;
    public GameObject output;
    public GameObject password;
    public GameObject username;

    private void OnClick()
    {
        this.logincomponent.GetComponent<LoginFengKAI>().login(this.username.GetComponent<UIInput>().text, this.password.GetComponent<UIInput>().text);
        this.output.GetComponent<UILabel>().text = "please wait...";
    }
}

