using Assets.Scripts.Gamemode;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings.Titans;
using Assets.Scripts.UI.Elements;
using System;

namespace Assets.Scripts.Settings.Gamemodes
{
    public abstract class GamemodeSettings
    {
        public PvPSettings Pvp { get; set; }
        public SettingsTitan Titan { get; set; }
        public HorseSettings Horse { get; set; }

        private string name;
        public string Name
        {
            get { return name ?? GamemodeType.ToString(); }
            set { name = value; }
        }

        public GamemodeType GamemodeType { get; set; }
        
        public string Description;

        [UiElement("MOTD", "Message of the Day")]
        public string Motd { get; set; }
        
        [UiElement("Damage Mode", "Minimum damage you need to do", SettingCategory.Titans)]
        public int DamageMode { get; set; }

        //If the explode mode <= 0, then it's disabled, 0 > then it's enabled.
        [UiElement("Explode mode", "", SettingCategory.Titans)]
        public int TitanExplodeMode { get; set; } = 0;

        [UiElement("Allow Titan Shifters", "")]
        public bool TitanShifters { get; set; } = true; // This is the anti eren kick

        public bool TitansEnabled { get; set; } = true;

        //LevelInfo attributes
        public bool Hint;

        [UiElement("Lava mode", "The floor is lava! Touching the floor means that you will die...")]
        public bool LavaMode { get; set; }

        [UiElement("Team mode", "Enable teams", SettingCategory.Pvp)]
        public TeamMode TeamMode { get; set; }

        [UiElement("Save KDR on DC", "When a player disconnects, should their KDR be saved?")]
        public bool SaveKDROnDisconnect { get; set; } = true;

        [UiElement("Endless Revive", "")]
        public int EndlessRevive { get; set; }

        [UiElement("Point mode", "", SettingCategory.Advanced)]
        public int PointMode { get; set; }

        public bool Supply { get; set; } = true;
        public bool IsPlayerTitanEnabled { get; set; }
        public RespawnMode RespawnMode { get; set; } = RespawnMode.DEATHMATCH;

        public int HumanScore = 0;
        public int TitanScore = 0;

        [UiElement("Ahss Air Reload", "Can AHSS reload in mid air?", SettingCategory.Pvp)]
        public bool AhssAirReload { get; set; } = true;
        public bool PlayerShifters = true;

        public bool RestartOnTitansKilled = true;

        public GamemodeSettings() { }

        public GamemodeSettings(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                case Difficulty.Normal:
                case Difficulty.Hard:
                case Difficulty.Abnormal:
                case Difficulty.Realism:
                    Pvp = new PvPSettings(difficulty);
                    Titan = new SettingsTitan(difficulty);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }

        public Type GetGamemodeFromSettings()
        {
            switch (GamemodeType)
            {
                case GamemodeType.Titans:
                    return typeof(KillTitansGamemode);
                case GamemodeType.Endless:
                    return typeof(EndlessGamemode);
                case GamemodeType.Capture:
                    return typeof(CaptureGamemode);
                case GamemodeType.Wave:
                    return typeof(WaveGamemode);
                case GamemodeType.Racing:
                    return typeof(RacingGamemode);
                case GamemodeType.Trost:
                    return typeof(TrostGamemode);
                case GamemodeType.TitanRush:
                    return typeof(TitanRushGamemode);
                case GamemodeType.PvpAhss:
                    return typeof(PvPAhssGamemode);
                case GamemodeType.Infection:
                    return typeof(InfectionGamemode);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
