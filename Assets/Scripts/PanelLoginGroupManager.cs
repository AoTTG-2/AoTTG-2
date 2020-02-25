using System;
using UnityEngine;

public class PanelLoginGroupManager : MonoBehaviour
{
    private string _loginName = string.Empty;
    private string _loginPassword = string.Empty;
    public LoginFengKAI logincomponent;
    public GameObject[] panels;
    public PanelGroupManager pgm;

    public void SignIn()
    {
        this.logincomponent.login(this._loginName, this._loginPassword);
    }

    private void Start()
    {
        this.pgm = new PanelGroupManager();
        this.pgm.panelGroup = this.panels;
    }

    public void toChangeGuildNamePanel()
    {
        this.pgm.ActivePanel(4);
    }

    public void toForgetPasswordPanel()
    {
        this.pgm.ActivePanel(3);
    }

    public void toLoginPanel()
    {
        this.pgm.ActivePanel(0);
    }

    public void toNewPasswordPanel()
    {
        this.pgm.ActivePanel(2);
    }

    public void toSignUpPanel()
    {
        this.pgm.ActivePanel(1);
    }

    public void toStatusPanel()
    {
        this.pgm.ActivePanel(5);
    }

    public string loginName
    {
        set
        {
            this._loginName = value;
        }
    }

    public string loginPassword
    {
        set
        {
            this._loginPassword = value;
        }
    }
}

