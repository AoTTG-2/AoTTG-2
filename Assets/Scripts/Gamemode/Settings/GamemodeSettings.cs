using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings;
using Assets.Scripts.UI.Elements;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Gamemode.Settings
{
    public abstract class GamemodeSettings
    {
        public GamemodeType GamemodeType;

        private string name;
        public string Name
        {
            get { return name ?? GamemodeType.ToString(); }
            set { name = value; }
        }

        public string Description;

        [UiElement("MOTD", "Message of the Day")]
        public string Motd { get; set; } = string.Empty;

        [UiElement("Start Titans", "The amount of titans that will spawn at the start", SettingCategory.Titans)]
        public int Titans { get; set; } = 25;

        [UiElement("Titan Limit", "The max amount of titans", SettingCategory.Titans)]
        public int TitanLimit { get; set; } = 30;

        [UiElement("Min Size", "Minimal titan size", SettingCategory.Titans)]
        public float TitanMinimumSize { get; set; } = 0.7f;

        [UiElement("Max size", "Maximun titan size", SettingCategory.Titans)]
        public float TitanMaximumSize { get; set; } = 3.0f;

        [UiElement("Custom Size", "Enable custom titan sizes", SettingCategory.Titans)]
        public bool TitanCustomSize { get; set; } = false;

        [UiElement("Titan Chase Distance", "", SettingCategory.Titans)]
        public float TitanChaseDistance { get; set; } = 100f;

        [UiElement("Enable Titan Chase Distance", "", SettingCategory.Titans)]
        public bool TitanChaseDistanceEnabled { get; set; } = true;

        [UiElement("Titan Health Mode", "", SettingCategory.Titans)]
        public TitanHealthMode TitanHealthMode { get; set; } = TitanHealthMode.Disabled;

        [UiElement("Titan Minimum Health", "", SettingCategory.Titans)]
        public int TitanHealthMinimum { get; set; } = 200;

        [UiElement("Titan Maximum Health", "", SettingCategory.Titans)]
        public int TitanHealthMaximum { get; set; } = 500;

        [UiElement("Punk rock throwing", "", SettingCategory.Titans)]
        public bool PunkRockThrow { get; set; } = true;

        [UiElement("Custom Titans", "Should custom titan rates be used?", SettingCategory.Titans)]
        public bool CustomTitanRatio { get; set; } = true;

        [UiElement("Damage Mode", "Minimum damage you need to do", SettingCategory.Titans)]
        public int DamageMode { get; set; }

        //If the explode mode <= 0, then it's disabled, 0 > then it's enabled.
        [UiElement("Explode mode", "", SettingCategory.Titans)]
        public int TitanExplodeMode { get; set; } = 0;

        [UiElement("Allow Titan Shifters", "")]
        public bool TitanShifters { get; set; } = true;

        public bool TitansEnabled { get; set; } = true;

        [UiElement("Spawn Titans on FT Defeat", "Should titans spawn when the Female Titan is killed?", SettingCategory.Advanced)]
        public bool SpawnTitansOnFemaleTitanDefeat { get; set; } = true;

        [UiElement("Female Titan Despawn Time", "How long (in seconds), will the FT be on the map after dying?", SettingCategory.Advanced)]
        public float FemaleTitanDespawnTimer { get; set; } = 5f;

        [UiElement("PvP Cannons", "Can cannons kill humans?", SettingCategory.Pvp)]
        public bool PvpCannons { get; set; }

        public float FemaleTitanHealthModifier = 1f;

        //LevelInfo attributes
        public bool Hint;

        [UiElement("Horses", "Enables/Disables horses in the game")]
        public bool Horse { get; set; }

        [UiElement("Lava mode", "The floor is lava! Touching the floor means that you will die...")]
        public bool LavaMode { get; set; }

        [UiElement("PvP", "Can players kill each other?", SettingCategory.Pvp)]
        public PvpMode Pvp { get; set; } = PvpMode.Disabled;

        [UiElement("PvP win on enemies killed", "Does the round end if all PvP enemies are dead?", SettingCategory.Pvp)]
        public bool PvPWinOnEnemiesDead { get; set; } = false;

        [UiElement("Bomb PvP", "", SettingCategory.Pvp)]
        public bool PvPBomb { get; set; }

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

        public float RespawnTime = 5f;
        [UiElement("Ahss Air Reload", "Can AHSS reload in mid air?", SettingCategory.Pvp)]
        public bool AhssAirReload { get; set; } = true;
        public bool PlayerShifters = true;

        public bool RestartOnTitansKilled = true;

        public int Difficulty = 1;

        public bool IsSinglePlayer = IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE;

        //TODO: Be able to change these via UI
        public Dictionary<MindlessTitanType, float> TitanTypeRatio = new Dictionary<MindlessTitanType, float>
        {
            {MindlessTitanType.Normal, 80f},
            {MindlessTitanType.Abberant, 40f},
            {MindlessTitanType.Jumper, 25f},
            {MindlessTitanType.Punk, 15f},
            {MindlessTitanType.Crawler, 5f},
            {MindlessTitanType.Burster, 0f},
            {MindlessTitanType.Stalker, 0f},
            { MindlessTitanType.Abnormal, 5f }
        };

        public List<MindlessTitanType> DisabledTitans { get; set; } = new List<MindlessTitanType>();

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

            return null;
        }
    }
}
