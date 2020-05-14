using Assets.Scripts.Characters.Titan;
using System.Collections.Generic;

namespace Assets.Scripts.Gamemode.Settings
{
    public class TrostSettings : GamemodeSettings
    {
        public TrostSettings()
        {
            GamemodeType = GamemodeType.Trost;
            PlayerTitanShifters = false;
            Titans = 2;
            DisabledTitans = new List<MindlessTitanType> {MindlessTitanType.Punk};
        }
    }
}
