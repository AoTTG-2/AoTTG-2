using Assets.Scripts.Gamemode;
using Assets.Scripts.Gamemode.Catch;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings.Types;
using System;
using UnityEngine;

namespace Assets.Scripts.Settings.Game.Gamemodes
{
    public abstract class GamemodeSetting : BaseSettings
    {
        /// <summary>
        /// Enum to define the GamemodeType. Where possible, rather use <code>Setting.Gamemode is TYPE</code> Will become obsolete once we can use C# 9.0
        /// </summary>
        public GamemodeType GamemodeType { get; protected set; }

        public StringSetting Description;
        public StringSetting MOTD;
        public EnumSetting<WorldMode> WorldMode;
        public EnumSetting<TeamMode> TeamMode;
        public BoolSetting SaveKDROnDisconnect;
        public IntSetting PointMode;
        public IntSetting ImpactForce;
        public BoolSetting Supply;
        public BoolSetting IsPlayerTitanEnabled;
        public BoolSetting PlayerShifers;
        [Tooltip("Determines whether or not the round should automatically restart if a win/lose condition has occurred. Setting this to false will only allow a manual restart by the MC.")]
        public BoolSetting Endless;

        [Header("Gamemode Override (optional)")]
        public PvPSettings PvP;
        public TitanSettings Titan;
        public HorseSettings Horse;
        public RespawnSettings Respawn;
        public TimeSettings Time;
        public GlobalSettings Global;

        public override void Override(BaseSettings settings)
        {
            switch (settings)
            {
                case RuleSet ruleSet:
                    {
                        // Global Settings
                        if (ruleSet.PvP != null) PvP.Override(ruleSet.PvP);
                        if (ruleSet.Titan != null) Titan.Override(ruleSet.Titan);
                        if (ruleSet.Horse != null) Horse.Override(ruleSet.Horse);
                        if (ruleSet.Respawn != null) Respawn.Override(ruleSet.Respawn);
                        if (ruleSet.Time != null) Time.Override(ruleSet.Time);
                        if (ruleSet.Global != null) Global.Override(ruleSet.Global);

                        // Global Gamemode specific settings
                        if (ruleSet.Description.HasValue) Description.Value = ruleSet.Description.Value;
                        if (ruleSet.MOTD.HasValue) MOTD.Value = ruleSet.MOTD.Value;
                        if (ruleSet.WorldMode.HasValue) WorldMode.Value = ruleSet.WorldMode.Value;
                        if (ruleSet.TeamMode.HasValue) TeamMode.Value = ruleSet.TeamMode.Value;
                        if (ruleSet.SaveKDROnDisconnect.HasValue) SaveKDROnDisconnect.Value = ruleSet.SaveKDROnDisconnect.Value;
                        if (ruleSet.PointMode.HasValue) PointMode.Value = ruleSet.PointMode.Value;
                        if (ruleSet.ImpactForce.HasValue) ImpactForce.Value = ruleSet.ImpactForce.Value;
                        if (ruleSet.Supply.HasValue) Supply.Value = ruleSet.Supply.Value;
                        if (ruleSet.IsPlayerTitanEnabled.HasValue) IsPlayerTitanEnabled.Value = ruleSet.IsPlayerTitanEnabled.Value;
                        if (ruleSet.PlayerShifers.HasValue) PlayerShifers.Value = ruleSet.PlayerShifers.Value;
                        if (ruleSet.Endless.HasValue) Endless.Value = ruleSet.Endless.Value;

                        // Override any gamemode specific
                        ruleSet.TryOverrideGamemode(this);
                        break;
                    }
                case GamemodeSetting gamemode:
                    {
                        if (gamemode.Description.HasValue) Description.Value = gamemode.Description.Value;
                        if (gamemode.MOTD.HasValue) MOTD.Value = gamemode.MOTD.Value;
                        if (gamemode.WorldMode.HasValue) WorldMode.Value = gamemode.WorldMode.Value;
                        if (gamemode.TeamMode.HasValue) TeamMode.Value = gamemode.TeamMode.Value;
                        if (gamemode.SaveKDROnDisconnect.HasValue) SaveKDROnDisconnect.Value = gamemode.SaveKDROnDisconnect.Value;
                        if (gamemode.PointMode.HasValue) PointMode.Value = gamemode.PointMode.Value;
                        if (gamemode.ImpactForce.HasValue) ImpactForce.Value = gamemode.ImpactForce.Value;
                        if (gamemode.Supply.HasValue) Supply.Value = gamemode.Supply.Value;
                        if (gamemode.IsPlayerTitanEnabled.HasValue) IsPlayerTitanEnabled.Value = gamemode.IsPlayerTitanEnabled.Value;
                        if (gamemode.PlayerShifers.HasValue) PlayerShifers.Value = gamemode.PlayerShifers.Value;
                        if (gamemode.Endless.HasValue) Endless.Value = gamemode.Endless.Value;

                        if (gamemode.PvP != null) PvP.Override(gamemode.PvP);
                        if (gamemode.Titan != null) Titan.Override(gamemode.Titan);
                        if (gamemode.Horse != null) Horse.Override(gamemode.Horse);
                        if (gamemode.Respawn != null) Respawn.Override(gamemode.Respawn);
                        if (gamemode.Time != null) Time.Override(gamemode.Time);
                        if (gamemode.Global != null) Global.Override(gamemode.Global);
                        break;
                    }
            }
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

        public Type GetGamemodeFromSettings()
        {
            return this switch
            {
                CaptureGamemodeSetting _ => typeof(CaptureGamemode),
                CatchGamemodeSetting _ => typeof(CatchGamemode),
                CreditsGamemodeSetting _ => typeof(CreditsGamemode),
                InfectionGamemodeSetting _ => typeof(InfectionGamemode),
                KillTitansGamemodeSetting _ => typeof(KillTitansGamemode),
                PlayerVersusPlayerGamemodeSetting _ => typeof(PvPAhssGamemode),
                RacingGamemodeSetting _ => typeof(RacingGamemode),
                RushGamemodeSetting _ => typeof(TitanRushGamemode),
                TrostGamemodeSetting _ => typeof(TrostGamemode),
                WaveGamemodeSetting _ => typeof(WaveGamemode),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
