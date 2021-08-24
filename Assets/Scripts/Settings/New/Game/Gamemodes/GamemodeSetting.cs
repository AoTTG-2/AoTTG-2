using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings.New.Types;
using Assets.Scripts.Settings.New.Validation;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Game.Gamemodes
{
    public abstract class GamemodeSetting : BaseSettings
    {
        public GamemodeType GamemodeType { get; protected set; }


        public StringSetting Description;
        public StringSetting MOTD;
        public BoolSetting LavaMode;
        public EnumSetting<TeamMode> TeamMode;
        public BoolSetting SaveKDROnDisconnect;
        public IntSetting PointMode;
        public IntSetting ImpactForce;
        public BoolSetting Supply;
        public BoolSetting IsPlayerTitanEnabled;
        public BoolSetting PlayerShifers;
        [Tooltip("Determines whether or not the round should automatically restart if a win/lose condition has occurred. Setting this to false will only allow a manual restart by the MC.")]
        public BoolSetting RestartOnCompleted;

        [Header("Gamemode Override (optional)")]
        public PvPSettings PvP;
        public TitanSettings Titan;
        public HorseSettings Horse;
        public RespawnSettings Respawn;
        public TimeSettings Time;

        public override void Override(BaseSettings settings)
        {
            if (!(settings is GamemodeSetting gamemodeSetting)) return;
        }

        public override BaseSettings Copy()
        {
            var setting = Instantiate(this);
            if (PvP != null) setting.PvP = PvP.Copy() as PvPSettings;
            if (Titan != null) setting.Titan = Titan.Copy() as TitanSettings;
            if (Horse != null) setting.Horse = Horse.Copy() as HorseSettings;
            if (Respawn != null) setting.Respawn = Respawn.Copy() as RespawnSettings;
            if (Time != null) setting.Time = Time.Copy() as TimeSettings;
            return setting;
        }
    }
}
