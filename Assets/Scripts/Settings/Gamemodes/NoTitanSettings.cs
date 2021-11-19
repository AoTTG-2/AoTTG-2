using Assets.Scripts.Gamemode;
using System;

namespace Assets.Scripts.Settings.Gamemodes
{
    public class NoTitanSettings : GamemodeSettings
    {
        public NoTitanSettings()
        {
            GamemodeType = GamemodeType.NoTitan;
        }
        public NoTitanSettings(Difficulty difficulty) : base(difficulty)
        {
            GamemodeType = GamemodeType.NoTitan;
        }
    }
}

