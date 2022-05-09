using Assets.Scripts.Gamemode;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings.Converter;
using Assets.Scripts.Settings.Titans;
using Assets.Scripts.UI.Elements;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Assets.Scripts.Gamemode.Catch;

namespace Assets.Scripts.Settings.Gamemodes
{
    [JsonConverter(typeof(GamemodeConverter))]
    public class GamemodeSettings
    {
        private string name;
        public string Name
        {
            get { return name ?? GamemodeType.ToString(); }
            set { name = value; }
        }

        public GamemodeType GamemodeType { get; set; }

        public string Description { get; set; }

        [UiElement("MOTD", "Message of the Day")]
        public string Motd { get; set; }
        
        [UiElement("Lava mode", "The floor is lava! Touching the floor means that you will die...")]
        public bool? LavaMode { get; set; }

        [UiElement("Team mode", "Enable teams")]
        public TeamMode TeamMode { get; set; }

        [UiElement("Save KDR on DC", "When a player disconnects, should their KDR be saved?")]
        public bool? SaveKDROnDisconnect { get; set; } = true;

        [UiElement("Point mode", "")]
        public int? PointMode { get; set; }

        [UiElement("ImpactForce", "")]
        public int? ImpactForce { get; set; }

        public bool? Supply { get; set; }
        public bool? IsPlayerTitanEnabled { get; set; }
        public bool? PlayerShifters { get; set; }

        public bool? RestartOnTitansKilled { get; set; }
        public PvPSettings Pvp { get; set; }
        public SettingsTitan Titan { get; set; }
        public HorseSettings Horse { get; set; }
        public RespawnSettings Respawn { get; set; }
        public TimeSettings Time { get; set; }

        public GamemodeSettings() { }

        public GamemodeSettings(Difficulty difficulty)
        {
            Pvp = new PvPSettings();
            Titan = new SettingsTitan
            {
                Colossal = new TitanSettings(),
                Eren = new TitanSettings(),
                Female = new FemaleTitanSettings(),
                Mindless = new MindlessTitanSettings()
            };
            Horse = new HorseSettings();
            Horse.Enabled = false;
            Respawn = new RespawnSettings();
            Time = new TimeSettings();
            TeamMode = TeamMode.Disabled;
            SaveKDROnDisconnect = true;
            PointMode = 0;
            ImpactForce = 0;
            Supply = true;
            IsPlayerTitanEnabled = true;
            LavaMode = false;
            RestartOnTitansKilled = true;
            PlayerShifters = true;
            PlayerShifters = true;
            RestartOnTitansKilled = true;
            switch (difficulty)
            {
                case Difficulty.Easy:
                case Difficulty.Normal:
                case Difficulty.Hard:
                case Difficulty.Abnormal:
                    break;
                case Difficulty.Realism:
                    ImpactForce = 1500;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }

        public static List<GamemodeSettings> GetAll(Difficulty difficulty)
        {
            return new List<GamemodeSettings>
            {
                new CaptureGamemodeSettings(difficulty),
                new EndlessSettings(difficulty),
                new InfectionGamemodeSettings(difficulty),
                new KillTitansSettings(difficulty),
                new PvPAhssSettings(difficulty),
                new RacingSettings(difficulty),
                new RushSettings(difficulty),
                new TrostSettings(difficulty),
                new WaveGamemodeSettings(difficulty),
                new CatchGamemodeSettings(difficulty)
            };
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
                case GamemodeType.Catch:
                    return typeof(CatchGamemode);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
