using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Input Validator"), RequireComponent(typeof(UIInput))]
public class UIInputValidator : MonoBehaviour
{
    public Validation logic;

    private void Start()
    {
        base.GetComponent<UIInput>().validator = new UIInput.Validator(this.Validate);
    }

    private char Validate(string text, char ch)
    {
        if ((this.logic == Validation.None) || !base.enabled)
        {
            return ch;
        }
        if (this.logic == Validation.Integer)
        {
            if ((ch >= '0') && (ch <= '9'))
            {
                return ch;
            }
            if ((ch == '-') && (text.Length == 0))
            {
                return ch;
            }
        }
        else if (this.logic == Validation.Float)
        {
            if ((ch >= '0') && (ch <= '9'))
            {
                return ch;
            }
            if ((ch == '-') && (text.Length == 0))
            {
                return ch;
            }
            if ((ch == '.') && !text.Contains("."))
            {
                return ch;
            }
        }
        else if (this.logic == Validation.Alphanumeric)
        {
            if ((ch >= 'A') && (ch <= 'Z'))
            {
                return ch;
            }
            if ((ch >= 'a') && (ch <= 'z'))
            {
                return ch;
            }
            if ((ch >= '0') && (ch <= '9'))
            {
                return ch;
            }
        }
        else if (this.logic == Validation.Username)
        {
            if ((ch >= 'A') && (ch <= 'Z'))
            {
                return (char) ((ch - 'A') + 0x61);
            }
            if ((ch >= 'a') && (ch <= 'z'))
            {
                return ch;
            }
            if ((ch >= '0') && (ch <= '9'))
            {
                return ch;
            }
        }
        else if (this.logic == Validation.Name)
        {
            char ch2 = (text.Length <= 0) ? ' ' : text[text.Length - 1];
            if ((ch >= 'a') && (ch <= 'z'))
            {
                if (ch2 == ' ')
                {
                    return (char) ((ch - 'a') + 0x41);
                }
                return ch;
            }
            if ((ch >= 'A') && (ch <= 'Z'))
            {
                if ((ch2 != ' ') && (ch2 != '\''))
                {
                    return (char) ((ch - 'A') + 0x61);
                }
                return ch;
            }
            if (ch == '\'')
            {
                if (((ch2 != ' ') && (ch2 != '\'')) && !text.Contains("'"))
                {
                    return ch;
                }
            }
            else if (((ch == ' ') && (ch2 != ' ')) && (ch2 != '\''))
            {
                return ch;
            }
        }
        return '\0';
    }

    public enum Validation
    {
        None,
        Integer,
        Float,
        Alphanumeric,
        Username,
        Name
    }
}

