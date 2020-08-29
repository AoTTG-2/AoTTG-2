using Assets.Scripts.Characters.Titan;
using System.Collections.Generic;
using Assets.Scripts.Settings.Titans;

namespace Assets.Scripts.Gamemode.Settings
{
    public class TrostSettings : GamemodeSettings
    {
        public TrostSettings()
        {
            Titan = new SettingsTitan
            {
                Start = 2
            };
            GamemodeType = GamemodeType.Trost;
            PlayerShifters = false;
            DisabledTitans = new List<MindlessTitanType> {MindlessTitanType.Punk};
        }
    }
}
