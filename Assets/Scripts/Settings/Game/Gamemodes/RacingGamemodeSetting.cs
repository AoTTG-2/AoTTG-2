using Assets.Scripts.Settings.Types;
using UnityEngine;

namespace Assets.Scripts.Settings.Game.Gamemodes
{
    [CreateAssetMenu(fileName = "Racing Gamemode Setting", menuName = "Settings/Gamemodes/Racing", order = 2)]
    public class RacingGamemodeSetting : GamemodeSetting
    {
        public BoolSetting RestartOnFinish;

        public override void Override(BaseSettings settings)
        {
            base.Override(settings);
            if (!(settings is RacingGamemodeSetting gamemode)) return;
            if (gamemode.RestartOnFinish.HasValue) RestartOnFinish = gamemode.RestartOnFinish;
        }
    }
}
