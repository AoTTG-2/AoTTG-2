using System;
using UnityEngine;

public class BTN_SET_GUILD_NAME : MonoBehaviour
{
    public GameObject guildname;
    public GameObject logincomponent;
    public GameObject output;

    private void OnClick()
    {
        if (this.guildname.GetComponent<UIInput>().text.Length < 3)
        {
            this.output.GetComponent<UILabel>().text = "Guild name too short.";
        }
        else
        {
            this.output.GetComponent<UILabel>().text = "please wait...";
            this.logincomponent.GetComponent<LoginFengKAI>().cGuild(this.guildname.GetComponent<UIInput>().text);
        }
    }
}

