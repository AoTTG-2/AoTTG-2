using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode;
using System.Collections.Generic;

namespace Assets.Scripts.Settings.Gamemodes
{
    public class TrostSettings : GamemodeSettings
    {
        public TrostSettings()
        {
            GamemodeType = GamemodeType.Trost;
        }

        public TrostSettings(Difficulty difficulty) : base(difficulty)
        {
            GamemodeType = GamemodeType.Trost;
            Titan.Start = 2;
            Titan.Mindless.Disabled = new List<MindlessTitanType> {MindlessTitanType.Punk};
            PlayerShifters = false;
        }
    }
}
