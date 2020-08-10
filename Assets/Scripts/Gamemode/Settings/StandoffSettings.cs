using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Gamemode.Settings
{
    public class StandoffSettings : GamemodeSettings
    {
        public StandoffGamemodeSettings()
        {
            GamemodeType = GamemodeType.Standoff;
            PlayerShifters = false;
            Titans = 0;
            TitanLimit = 40;
            TitanChaseDistance = 60f;
            RespawnMode = RespawnMode.NEWROUND;
        }
        [UiElement("Alpha team points", "Once this reaches 0, the team wins")]
        public int AlphaScore { get; set; } = 10;
        [UiElement("Beta team points", "Once this reaches 0, the team wins")]
        public int BetaScore { get; set; } = 10;
        [UiElement("PvP win on enemies killed", "Does the round end if all PvP enemies are dead?", SettingCategory.Pvp)]
        public bool PvPWinOnEnemiesDead { get; set; } = true;
    }
}
