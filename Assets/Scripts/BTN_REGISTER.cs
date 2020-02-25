using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

public class BTN_REGISTER : MonoBehaviour
{
    public GameObject email;
    private bool invalid;
    public GameObject logincomponent;
    public GameObject nameGO;
    public GameObject output;
    public GameObject password;
    public GameObject password2;

    private string DomainMapper(Match match)
    {
        IdnMapping mapping = new IdnMapping();
        string unicode = match.Groups[2].Value;
        try
        {
            unicode = mapping.GetAscii(unicode);
        }
        catch (ArgumentException)
        {
            this.invalid = true;
        }
        return (match.Groups[1].Value + unicode);
    }

    public bool IsValidEmail(string strIn)
    {
        this.invalid = false;
        if (string.IsNullOrEmpty(strIn))
        {
            return false;
        }
        strIn = Regex.Replace(strIn, "(@)(.+)S", new MatchEvaluator(this.DomainMapper));
        if (this.invalid)
        {
            return false;
        }
        return Regex.IsMatch(strIn, "^(?(\")(\"[^\"]+?\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\S%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9]{2,17}))S", RegexOptions.IgnoreCase);
    }

    private void OnClick()
    {
        if (this.nameGO.GetComponent<UIInput>().text.Length < 3)
        {
            this.output.GetComponent<UILabel>().text = "User name too short.";
        }
        else if (this.password.GetComponent<UIInput>().text.Length < 3)
        {
            this.output.GetComponent<UILabel>().text = "Password too short.";
        }
        else if (this.password.GetComponent<UIInput>().text != this.password2.GetComponent<UIInput>().text)
        {
            this.output.GetComponent<UILabel>().text = "Password does not match the confirm password.";
        }
        else if (!this.IsValidEmail(this.email.GetComponent<UIInput>().text))
        {
            this.output.GetComponent<UILabel>().text = "This e-mail address is not valid.";
        }
        else
        {
            this.logincomponent.GetComponent<LoginFengKAI>().signup(this.nameGO.GetComponent<UIInput>().text, this.password.GetComponent<UIInput>().text, this.password2.GetComponent<UIInput>().text, this.email.GetComponent<UIInput>().text);
            this.output.GetComponent<UILabel>().text = "please wait...";
        }
    }
}

