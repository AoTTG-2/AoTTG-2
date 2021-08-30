using Assets.Scripts.Settings.Game.Gamemodes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Settings.Game
{
    [CreateAssetMenu(fileName = "Custom", menuName = "Settings/Game", order = 1)]
    public class GameSettings : BaseSettings
    {
        [SerializeField] private GamemodeConfiguration gamemodes;

        public GamemodeSetting CurrentGamemode { get; private set; }
        public PvPSettings PvP;
        public TitanSettings Titan;
        public HorseSettings Horse;
        public RespawnSettings Respawn;
        public TimeSettings Time;
        public GlobalSettings Global;

        public override void Override(BaseSettings settings)
        {

        }

        public override BaseSettings Copy()
        {
            var setting = Instantiate(this);
            setting.PvP = PvP.Copy() as PvPSettings;
            setting.Titan = Titan.Copy() as TitanSettings;
            setting.Horse = Horse.Copy() as HorseSettings;
            setting.Respawn = Respawn.Copy() as RespawnSettings;
            setting.Time = Time.Copy() as TimeSettings;
            setting.Global = Global.Copy() as GlobalSettings;
            setting.gamemodes.Wave = setting.gamemodes.Wave.Copy() as WaveGamemodeSetting;
            setting.gamemodes.KillTitans = setting.gamemodes.KillTitans.Copy() as KillTitansGamemodeSetting;
            return setting;
        }

        public GamemodeSetting Setup(GamemodeSetting levelSetting, List<RuleSet> ruleSets)
        {
            GamemodeSetting currentGamemode = levelSetting switch
            {
                WaveGamemodeSetting _ => gamemodes.Wave,
                KillTitansGamemodeSetting _ => gamemodes.KillTitans,
                CaptureGamemodeSetting _ => gamemodes.Capture,
                CreditsGamemodeSetting _ => null,
                _ => throw new NotImplementedException($"Gamemode: {levelSetting.GetType()} is not implemented")
            };

            // 1. Create a Gamemode Setting based on the GameSettings
            var currentSetting = Instantiate(currentGamemode ?? levelSetting);
            currentSetting.PvP = PvP;
            currentSetting.Horse = Horse;
            currentSetting.Global = Global;
            currentSetting.Titan = Titan;
            currentSetting.Time = Time;
            currentSetting.Respawn = Respawn;

            // 2. Override the Settings with the gamemode specific settings
            if (currentGamemode != null) currentSetting.Override(currentGamemode);

            // 3. Override the Settings with RuleSets
            foreach (var ruleSet in ruleSets)
            {
                currentSetting.Override(ruleSet);
            }

            // 4. Override the Settings with the Level Settings
            currentSetting.Override(levelSetting);
            return currentSetting;
        }
    }

    [Serializable]
    public struct GamemodeConfiguration
    {
        public WaveGamemodeSetting Wave;
        public KillTitansGamemodeSetting KillTitans;
        public CaptureGamemodeSetting Capture;
    }
}
