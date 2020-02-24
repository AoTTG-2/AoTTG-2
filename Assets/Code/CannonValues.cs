using System;

public class CannonValues
{
    public string settings = string.Empty;
    public int viewID = -1;

    public CannonValues(int id, string str)
    {
        this.viewID = id;
        this.settings = str;
    }
}

