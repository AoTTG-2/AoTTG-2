public class RacingGamemode : GamemodeBase
{
    public RacingGamemode()
    {
        GamemodeType = GamemodeType.Racing;
        PlayerTitanShifters = false;
        Pvp = false;
        Supply = false;
        RespawnMode = RespawnMode.NEVER;
        Titans = 0;
    }

    public string localRacingResult = string.Empty;

    public override void OnGameWon()
    {
        FengGameManagerMKII.instance.gameEndCD = RCSettings.racingStatic == 1
            ? 1000f
            : 20f;

        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
            var parameters = new object[] { 0 };
            FengGameManagerMKII.instance.photonView.RPC("netGameWin", PhotonTargets.Others, parameters);
            if (((int)FengGameManagerMKII.settings[0xf4]) == 1)
            {
                //this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
            }
        }
    }

    public override string GetVictoryMessage(float timeUntilRestart)
    {
        return $"{localRacingResult}\n\nGame Restart in {(int) timeUntilRestart}";
    }

    public override void OnNetGameWon(int score)
    {
        FengGameManagerMKII.instance.gameEndCD = RCSettings.racingStatic == 1
            ? 1000f
            : 20f;
    }
}