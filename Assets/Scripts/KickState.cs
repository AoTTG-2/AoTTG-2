using System;
using System.Collections;

public class KickState
{
    public int id;
    private int kickCount;
    private string kickers;
    public ArrayList kickers2;
    public string name;

    public void addKicker(string n)
    {
        if (!this.kickers.Contains(n))
        {
            this.kickers = this.kickers + n;
            this.kickCount++;
        }
    }

    public int getKickCount()
    {
        return this.kickCount;
    }

    public void init(string n)
    {
        this.name = n;
        this.kickers = string.Empty;
        this.kickCount = 0;
    }
}

