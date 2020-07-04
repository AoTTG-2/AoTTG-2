using Assets.Scripts.Gamemode.Settings;
using Assets.Scripts.UI.Input;

namespace Assets.Scripts.Gamemode
{
    public class RacingGamemode : GamemodeBase
    {
        public string localRacingResult = string.Empty;

        public sealed override GamemodeSettings Settings { get; set; }
        private RacingSettings GamemodeSettings => Settings as RacingSettings;

        public override void OnGameWon()
        {
            FengGameManagerMKII.instance.gameEndCD = GamemodeSettings.RestartOnFinish
                ? 20f
                : 9999f;

            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            {
                FengGameManagerMKII.instance.photonView.RPC(
                    nameof(FengGameManagerMKII.instance.netGameWin),
                    PhotonTargets.Others,
                    0);
                if (((int)FengGameManagerMKII.settings[0xf4]) == 1)
                {
                    //this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
                }
            }
        }

        public override string GetVictoryMessage(float timeUntilRestart, float totalServerTime = 0f)
        {
            if (GamemodeSettings.IsSinglePlayer)
            {
                var num = (((int)(totalServerTime * 10f)) * 0.1f) - 5f;
                return $"{num}s !!\n Press {InputManager.GetKey(InputUi.Restart)} to Restart.\n\n\n";
            }
            return $"{localRacingResult}\n\nGame Restart in {(int) timeUntilRestart}";
        }

        public override void OnNetGameWon(int score)
        {
            FengGameManagerMKII.instance.gameEndCD = GamemodeSettings.RestartOnFinish
                ? 20f
                : 9999f;
        }
    }
}