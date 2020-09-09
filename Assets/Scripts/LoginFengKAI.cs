using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[Obsolete("LoginFengKAI is the previous aotskins login method. This will be replaced with a new login system.")]
public class LoginFengKAI : MonoBehaviour
{
    private string ChangeGuildURL = "http://aotskins.com/version/guild.php";
    private string ChangePasswordURL = "http://fenglee.com/game/aog/change_password.php";
    private string CheckUserURL = "http://aotskins.com/version/login.php";
    private string ForgetPasswordURL = "http://fenglee.com/game/aog/forget_password.php";
    public string formText = string.Empty;
    private string GetInfoURL = "http://aotskins.com/version/getinfo.php";
    public GameObject output;
    public GameObject output2;
    public GameObject panelChangeGUILDNAME;
    public GameObject panelChangePassword;
    public GameObject panelForget;
    public GameObject panelLogin;
    public GameObject panelRegister;
    public GameObject panelStatus;
    public static PlayerInfoPHOTON player;
    private static string playerGUILDName = string.Empty;
    private static string playerName = string.Empty;
    private static string playerPassword = string.Empty;
    private string RegisterURL = "http://fenglee.com/game/aog/signup_check.php";

    [DebuggerHidden]
    private IEnumerator changeGuild(string name)
    {
        return new changeGuildc__Iterator5 { name = name, f__this = this };
    }

    private void clearCOOKIE()
    {
        playerName = string.Empty;
        playerPassword = string.Empty;
    }

    [DebuggerHidden]
    private IEnumerator ForgetPassword(string email)
    {
        return new ForgetPasswordc__Iterator6 { email = email, f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator getInfo()
    {
        return new getInfoc__Iterator2 { f__this = this };
    }

    public void login(string name, string password)
    {
        base.StartCoroutine(this.Login(name, password));
    }

    [DebuggerHidden]
    private IEnumerator Login(string name, string password)
    {
        return new Loginc__Iterator1 { name = name, password = password, f__this = this };
    }

    public void logout()
    {
        this.clearCOOKIE();
        player = new PlayerInfoPHOTON();
        player.initAsGuest();
    }

    [DebuggerHidden]
    private IEnumerator Register(string name, string password, string password2, string email)
    {
        return new Registerc__Iterator3 { name = name, password = password, password2 = password2, email = email, f__this = this };
    }

    public void resetPassword(string email)
    {
        base.StartCoroutine(this.ForgetPassword(email));
    }

    public void signup(string name, string password, string password2, string email)
    {
        base.StartCoroutine(this.Register(name, password, password2, email));
    }

    private void Start()
    {
        if (player == null)
        {
            player = new PlayerInfoPHOTON();
            player.initAsGuest();
        }
        //if (playerName != string.Empty)
        //{
        //    NGUITools.SetActive(this.panelLogin, false);
        //    NGUITools.SetActive(this.panelStatus, true);
        //    base.StartCoroutine(this.getInfo());
        //}
        //else
        //{
        //    this.output.GetComponent<UILabel>().text = "Welcome," + player.name;
        //}
    }

    [CompilerGenerated]
    private sealed class changeGuildc__Iterator5 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object Scurrent;
        internal int SPC;
        internal LoginFengKAI f__this;
        internal WWWForm form__0;
        internal WWW w__1;
        internal string name;

        [DebuggerHidden]
        public void Dispose()
        {
            this.SPC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint)this.SPC;
            this.SPC = -1;
            switch (num)
            {
                case 0:
                    this.form__0 = new WWWForm();
                    this.form__0.AddField("thisName", LoginFengKAI.playerName);
                    this.form__0.AddField("guildname", this.name);
                    this.w__1 = new WWW(this.f__this.ChangeGuildURL, this.form__0);
                    this.Scurrent = this.w__1;
                    this.SPC = 1;
                    return true;

                case 1:
                    if (this.w__1.error == null)
                    {
                        //this.f__this.output.GetComponent<UILabel>().text = this.w__1.text;
                        if (this.w__1.text.Contains("Guild thisName set."))
                        {
                            //SetActive(this.f__this.panelChangeGUILDNAME, false);
                            //SetActive(this.f__this.panelStatus, true);
                            this.f__this.StartCoroutine(this.f__this.getInfo());
                        }
                        this.w__1.Dispose();
                        break;
                    }
                    MonoBehaviour.print(this.w__1.error);
                    break;

                default:
                    goto Label_0135;
            }
            this.SPC = -1;
        Label_0135:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }
    }

    [CompilerGenerated]
    private sealed class changePasswordc__Iterator4 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object Scurrent;
        internal int SPC;
        internal LoginFengKAI f__this;
        internal WWWForm form__0;
        internal WWW w__1;
        internal string oldpassword;
        internal string password;
        internal string password2;

        [DebuggerHidden]
        public void Dispose()
        {
            this.SPC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint)this.SPC;
            this.SPC = -1;
            switch (num)
            {
                case 0:
                    this.form__0 = new WWWForm();
                    this.form__0.AddField("userid", LoginFengKAI.playerName);
                    this.form__0.AddField("old_password", this.oldpassword);
                    this.form__0.AddField("password", this.password);
                    this.form__0.AddField("password2", this.password2);
                    this.w__1 = new WWW(this.f__this.ChangePasswordURL, this.form__0);
                    this.Scurrent = this.w__1;
                    this.SPC = 1;
                    return true;

                case 1:
                    if (this.w__1.error == null)
                    {
                        //this.f__this.output.GetComponent<UILabel>().text = this.w__1.text;
                        if (this.w__1.text.Contains("Thanks, Your password changed successfully"))
                        {
                            //SetActive(this.f__this.panelChangePassword, false);
                            //SetActive(this.f__this.panelLogin, true);
                        }
                        this.w__1.Dispose();
                        break;
                    }
                    MonoBehaviour.print(this.w__1.error);
                    break;

                default:
                    goto Label_014A;
            }
            this.SPC = -1;
        Label_014A:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }
    }

    [CompilerGenerated]
    private sealed class ForgetPasswordc__Iterator6 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object Scurrent;
        internal int SPC;
        internal LoginFengKAI f__this;
        internal WWWForm form__0;
        internal WWW w__1;
        internal string email;

        [DebuggerHidden]
        public void Dispose()
        {
            this.SPC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint)this.SPC;
            this.SPC = -1;
            switch (num)
            {
                case 0:
                    this.form__0 = new WWWForm();
                    this.form__0.AddField("email", this.email);
                    this.w__1 = new WWW(this.f__this.ForgetPasswordURL, this.form__0);
                    this.Scurrent = this.w__1;
                    this.SPC = 1;
                    return true;

                case 1:
                    if (this.w__1.error == null)
                    {
                        //this.f__this.output.GetComponent<UILabel>().text = this.w__1.text;
                        this.w__1.Dispose();
                        //SetActive(this.f__this.panelForget, false);
                        //SetActive(this.f__this.panelLogin, true);
                        break;
                    }
                    MonoBehaviour.print(this.w__1.error);
                    break;

                default:
                    goto Label_00FA;
            }
            this.f__this.clearCOOKIE();
            this.SPC = -1;
        Label_00FA:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }
    }

    [CompilerGenerated]
    private sealed class getInfoc__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object Scurrent;
        internal int SPC;
        internal LoginFengKAI f__this;
        internal WWWForm form__0;
        internal string[] result__2;
        internal WWW w__1;

        [DebuggerHidden]
        public void Dispose()
        {
            this.SPC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint)this.SPC;
            this.SPC = -1;
            switch (num)
            {
                case 0:
                    this.form__0 = new WWWForm();
                    this.form__0.AddField("userid", LoginFengKAI.playerName);
                    this.form__0.AddField("password", LoginFengKAI.playerPassword);
                    this.w__1 = new WWW(this.f__this.GetInfoURL, this.form__0);
                    this.Scurrent = this.w__1;
                    this.SPC = 1;
                    return true;

                case 1:
                    if (this.w__1.error == null)
                    {
                        if (this.w__1.text.Contains("Error,please sign in again."))
                        {
                            //SetActive(this.f__this.panelLogin, true);
                            //SetActive(this.f__this.panelStatus, false);
                            //this.f__this.output.GetComponent<UILabel>().text = this.w__1.text;
                            LoginFengKAI.playerName = string.Empty;
                            LoginFengKAI.playerPassword = string.Empty;
                        }
                        else
                        {
                            char[] separator = new char[] { '|' };
                            this.result__2 = this.w__1.text.Split(separator);
                            LoginFengKAI.playerGUILDName = this.result__2[0];
                            //this.f__this.output2.GetComponent<UILabel>().text = this.result__2[1];
                            LoginFengKAI.player.name = LoginFengKAI.playerName;
                            LoginFengKAI.player.guildname = LoginFengKAI.playerGUILDName;
                        }
                        this.w__1.Dispose();
                        break;
                    }
                    MonoBehaviour.print(this.w__1.error);
                    break;

                default:
                    goto Label_01A7;
            }
            this.SPC = -1;
        Label_01A7:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }
    }

    [CompilerGenerated]
    private sealed class Loginc__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object Scurrent;
        internal int SPC;
        internal LoginFengKAI f__this;
        internal WWWForm form__0;
        internal WWW w__1;
        internal string name;
        internal string password;

        [DebuggerHidden]
        public void Dispose()
        {
            this.SPC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint)this.SPC;
            this.SPC = -1;
            switch (num)
            {
                case 0:
                    this.form__0 = new WWWForm();
                    this.form__0.AddField("userid", this.name);
                    this.form__0.AddField("password", this.password);
                    //this.form__0.AddField("version", UIMainReferences.version);
                    this.w__1 = new WWW(this.f__this.CheckUserURL, this.form__0);
                    this.Scurrent = this.w__1;
                    this.SPC = 1;
                    return true;

                case 1:
                    this.f__this.clearCOOKIE();
                    if (this.w__1.error == null)
                    {
                        //this.f__this.output.GetComponent<UILabel>().text = this.w__1.text;
                        this.f__this.formText = this.w__1.text;
                        this.w__1.Dispose();
                        if (this.f__this.formText.Contains("Welcome back") && this.f__this.formText.Contains("(^o^)/~"))
                        {
                            //SetActive(this.f__this.panelLogin, false);
                            //SetActive(this.f__this.panelStatus, true);
                            LoginFengKAI.playerName = this.name;
                            LoginFengKAI.playerPassword = this.password;
                            this.f__this.StartCoroutine(this.f__this.getInfo());
                        }
                        break;
                    }
                    MonoBehaviour.print(this.w__1.error);
                    break;

                default:
                    goto Label_019C;
            }
            this.SPC = -1;
        Label_019C:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }
    }

    [CompilerGenerated]
    private sealed class Registerc__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object Scurrent;
        internal int SPC;
        internal LoginFengKAI f__this;
        internal WWWForm form__0;
        internal WWW w__1;
        internal string email;
        internal string name;
        internal string password;
        internal string password2;

        [DebuggerHidden]
        public void Dispose()
        {
            this.SPC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint)this.SPC;
            this.SPC = -1;
            switch (num)
            {
                case 0:
                    this.form__0 = new WWWForm();
                    this.form__0.AddField("userid", this.name);
                    this.form__0.AddField("password", this.password);
                    this.form__0.AddField("password2", this.password2);
                    this.form__0.AddField("email", this.email);
                    this.w__1 = new WWW(this.f__this.RegisterURL, this.form__0);
                    this.Scurrent = this.w__1;
                    this.SPC = 1;
                    return true;

                case 1:
                    if (this.w__1.error == null)
                    {
                        //this.f__this.output.GetComponent<UILabel>().text = this.w__1.text;
                        if (this.w__1.text.Contains("Final step,to activate your account, please click the link in the activation email"))
                        {
                            //SetActive(this.f__this.panelRegister, false);
                            //SetActive(this.f__this.panelLogin, true);
                        }
                        this.w__1.Dispose();
                        break;
                    }
                    MonoBehaviour.print(this.w__1.error);
                    break;

                default:
                    goto Label_0156;
            }
            this.f__this.clearCOOKIE();
            this.SPC = -1;
        Label_0156:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.Scurrent;
            }
        }
    }
}

