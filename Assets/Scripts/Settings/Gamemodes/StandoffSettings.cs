using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Gamemode;
namespace Assets.Scripts.Settings.Gamemodes
{
    public class StandoffSettings : GamemodeSettings
    {
        public StandoffSettings()
        {
            GamemodeType = GamemodeType.Standoff;
        }
        public StandoffSettings(Difficulty difficulty) : base(difficulty)
        {
            GamemodeType = GamemodeType.Standoff;
            Titan.Start = 5;
            Respawn.Mode = RespawnMode.NewRound;

        }
    }
}