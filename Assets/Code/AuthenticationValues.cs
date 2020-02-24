using System;
using System.Runtime.CompilerServices;

public class AuthenticationValues
{
    public string AuthParameters;
    public CustomAuthenticationType AuthType;
    public string Secret;

    public virtual void SetAuthParameters(string user, string token)
    {
        this.AuthParameters = "username=" + Uri.EscapeDataString(user) + "&token=" + Uri.EscapeDataString(token);
    }

    public virtual void SetAuthPostData(string stringData)
    {
        this.AuthPostData = !string.IsNullOrEmpty(stringData) ? stringData : null;
    }

    public virtual void SetAuthPostData(byte[] byteData)
    {
        this.AuthPostData = byteData;
    }

    public override string ToString()
    {
        return (this.AuthParameters + " s: " + this.Secret);
    }

    public object AuthPostData { get; private set; }
}

