using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings.Game.Gamemodes;
using Assets.Scripts.Settings.Types;
using System;
using UnityEngine;

namespace Assets.Scripts.Settings.Game
{
    [CreateAssetMenu(fileName = "Custom RuleSet", menuName = "Settings/RuleSet", order = 1)]
    public class RuleSet : GameSettings
    {

        [Header("Override Default Gamemode Settings")]
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

        public void TryOverrideGamemode(GamemodeSetting setting)
        {
            GamemodeSetting currentGamemode = setting switch
            {
                CatchGamemodeSetting _ => gamemodes.Catch,
                CaptureGamemodeSetting _ => gamemodes.Capture,
                InfectionGamemodeSetting _ => gamemodes.Infection,
                KillTitansGamemodeSetting _ => gamemodes.KillTitans,
                RacingGamemodeSetting _ => gamemodes.Racing,
                RushGamemodeSetting _ => gamemodes.Rush,
                TrostGamemodeSetting _ => gamemodes.Trost,
                WaveGamemodeSetting _ => gamemodes.Wave,
                CreditsGamemodeSetting _ => null, // Credits is a special gamemode
                _ => throw new NotImplementedException($"Gamemode: {setting.GetType()} is not implemented")
            };

            setting.Override(currentGamemode);
        }
    }
}
