using Assets.Scripts.UI.Elements;
using Assets.Scripts.Gamemode.Options;

namespace Assets.Scripts.Gamemode.Settings
{
    public class StandoffGamemodeSettings : GamemodeSettings
    {
        public StandoffGamemodeSettings()
        {
            GamemodeType = GamemodeType.Standoff;
            PlayerShifters = false;
            Titans = 0;
            TitanLimit = 50;
            TitanChaseDistance = 60f;
            RespawnMode = RespawnMode.NEWROUND;

        }


    }
}
