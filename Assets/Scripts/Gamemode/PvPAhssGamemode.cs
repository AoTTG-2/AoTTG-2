public class PvPAhssGamemode : GamemodeBase
{
    public PvPAhssGamemode()
    {
        GamemodeType = GamemodeType.PvpAhss;
    }

    public override string GetGamemodeStatusTopRight(int time = 0, int totalRoomTime = 0)
    {
        var content = "";
        //for (int j = 0; j < this.teamScores.Length; j++)
        //{
        //    string str3 = content;
        //    content = string.Concat(new object[] { str3, (j == 0) ? string.Empty : " : ", "Team", j + 1, " ", this.teamScores[j], string.Empty });
        //}
        content += content + "\nTime : " + (totalRoomTime - time);
        return content;
    }
}
