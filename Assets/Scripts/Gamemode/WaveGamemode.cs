using UnityEngine;

public class WaveGamemode : GamemodeBase
{
    public int Wave { get; set; } = 1;

    public WaveGamemode()
    {
        GamemodeType = GamemodeType.Wave;
    }

    public override string GetGamemodeStatusTop(int totalRoomTime = 0, int timeLeft = 0)
    {
        var content = "Titan Left: ";
        object[] objArray = new object[4];
        objArray[0] = content;
        var length = GameObject.FindGameObjectsWithTag("titan").Length;
        objArray[1] = length.ToString();
        objArray[2] = " Wave : ";
        objArray[3] = Wave;
        return string.Concat(objArray);
    }
}
