using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Gamemode
{
    public class RacingGamemode : GamemodeBase
    {
        public RacingGamemode()
        {
            GamemodeType = GamemodeType.Racing;
            PlayerTitanShifters = false;
            Pvp = PvpMode.Disabled;
            Supply = false;
            RespawnMode = RespawnMode.NEVER;
            Titans = 0;
            TitansEnabled = false;
        }

        [UiElement("Restart on Finish", "Should the game restart in 10s upon someone finishing?")]
        public bool RestartOnFinish { get; set; } = true;

        public string localRacingResult = string.Empty;

        public override void OnGameWon()
        {
            FengGameManagerMKII.instance.gameEndCD = RestartOnFinish
                ? 20f
                : 9999f;

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

        public override string GetVictoryMessage(float timeUntilRestart, float totalServerTime = 0f)
        {
            if (IsSinglePlayer)
            {
                var num = (((int)(totalServerTime * 10f)) * 0.1f) - 5f;
                return $"{num}s !!\n Press {FengGameManagerMKII.instance.inputManager.inputString[InputCode.restart]}  to Restart.\n\n\n";
            }
            return $"{localRacingResult}\n\nGame Restart in {(int) timeUntilRestart}";
        }

        public override void OnNetGameWon(int score)
        {
            FengGameManagerMKII.instance.gameEndCD = RestartOnFinish
                ? 20f
                : 9999f;
        }
    }
}