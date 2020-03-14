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

    public override void OnGameWon()
    {
        //FengGameManagerMKII.instance.gameEndCD = FengGameManagerMKII.instance.gameEndTotalCDtime;
        //if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        //{
        //    object[] objArray3 = new object[] { FengGameManagerMKII.instance.teamWinner };
        //    FengGameManagerMKII.instance.photonView.RPC("netGameWin", PhotonTargets.Others, objArray3);
        //    if (((int)FengGameManagerMKII.settings[0xf4]) == 1)
        //    {
        //        this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
        //    }
        //}
        //this.teamScores[this.teamWinner - 1]++;
    }

    public override void OnNetGameWon()
    {
        //this.teamWinner = score;
        //this.teamScores[this.teamWinner - 1]++;
        //this.gameEndCD = this.gameEndTotalCDtime;
    }
}
