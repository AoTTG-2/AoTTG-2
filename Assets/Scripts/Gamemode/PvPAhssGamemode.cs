using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Gamemode.Settings;

namespace Assets.Scripts.Gamemode
{
    public class PvPAhssGamemode : GamemodeBase
    {
        public sealed override GamemodeSettings Settings { get; set; }
        private PvPAhssSettings GamemodeSettings => Settings as PvPAhssSettings;

        private int teamWinner;
        private readonly int[] teamScores = new int[2];

        public override string GetGamemodeStatusTopRight()
        {
            var content = "";
            //for (int j = 0; j < this.teamScores.Length; j++)
            //{
            //    string str3 = content;
            //    content = string.Concat(new object[] { str3, (j == 0) ? string.Empty : " : ", "Team", j + 1, " ", this.teamScores[j], string.Empty });
            //}
            content += content + "\nTime : " + (FengGameManagerMKII.instance.time - FengGameManagerMKII.instance.timeTotalServer).ToString("f1");
            return content;
        }

        public override string GetRoundEndedMessage()
        {
            var result = string.Empty;
            for (int k = 0; k < this.teamScores.Length; k++)
            {
                result += ((k == 0) ? string.Concat(new object[] { "Team", k + 1, " ", this.teamScores[k], " " }) : " : ");
            }
            return result;
        }

        public override void OnPlayerKilled(int id)
        {
            if (Settings.Pvp != PvpMode.Disabled || Settings.PvPBomb) return;
            if (IsAllPlayersDead())
            {
                FengGameManagerMKII.Gamemode.GameLose();
                teamWinner = 0;
            }
            if (IsTeamAllDead(1))
            {
                teamWinner = 2;
                FengGameManagerMKII.Gamemode.GameWin();
            }
            if (IsTeamAllDead(2))
            {
                teamWinner = 1;
                FengGameManagerMKII.Gamemode.GameWin();
            }
        }

        public override string GetVictoryMessage(float timeUntilRestart, float totalServerTime = 0f)
        {
            if (Settings.Pvp == PvpMode.Disabled && !Settings.PvPBomb)
            {
                return $"Team {teamWinner}, Win!\nGame Restart in {(int)timeUntilRestart}s\n\n";
            }
            return $"Round Ended!\nGame Restart in {(int)timeUntilRestart}s\n\n";
        }

        private static bool IsTeamAllDead(int team)
        {
            var num = 0;
            var num2 = 0;
            foreach (var player in PhotonNetwork.playerList)
            {
                if (player.CustomProperties.SafeCompare(PhotonPlayerProperty.isTitan,1) && player.CustomProperties.SafeCompare(PhotonPlayerProperty.team,team))
                {
                    num++;
                    if (RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead]))
                    {
                        num2++;
                    }
                }
            }
            return (num == num2);
        }

        public override void OnGameWon()
        {
            FengGameManagerMKII.instance.gameEndCD = FengGameManagerMKII.instance.gameEndTotalCDtime;
            var parameters = new object[] { teamWinner };
            FengGameManagerMKII.instance.photonView.RPC("netGameWin", PhotonTargets.Others, parameters);
            if (Settings.ChatFeed)
            {
                //this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
            }
            this.teamScores[this.teamWinner - 1]++;
        }

        public override void OnNetGameWon(int score)
        {
            base.OnNetGameWon(score);
            this.teamWinner = score;
            this.teamScores[this.teamWinner - 1]++;
        }
    }
}
