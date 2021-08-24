using Assets.Scripts.Settings.New.Game.Gamemodes;
using System;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Game
{
    [CreateAssetMenu(fileName = "Custom", menuName = "Settings/Game", order = 1)]
    public class GameSettingsNew : BaseSettings
    {
        [SerializeField] private GamemodeConfiguration gamemodes;

        public PvPSettings PvP;
        public TitanSettings Titan;
        public HorseSettings Horse;
        public RespawnSettings Respawn;
        public TimeSettings Time;

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
            setting.gamemodes.Wave = setting.gamemodes.Wave.Copy() as WaveGamemodeSetting;
            setting.gamemodes.KillTitans = setting.gamemodes.KillTitans.Copy() as KillTitansGamemodeSetting;
            return setting;
        }
    }

    [Serializable]
    public struct GamemodeConfiguration
    {
        public WaveGamemodeSetting Wave;
        public KillTitansGamemodeSetting KillTitans;
    }
}
