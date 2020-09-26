using System;

public class RacingResult
{
    public int ID;
    public string name;
    public float time;

    public RacingResult(int playerID, string playerName, float timeResult)
    {
        ID = playerID;
        name = playerName;
        time = timeResult;
    }

    public override string ToString()
    {
        return "["+ID+"]"+ name + " " + time.ToString("000.00")+" s\n";
    }
}

