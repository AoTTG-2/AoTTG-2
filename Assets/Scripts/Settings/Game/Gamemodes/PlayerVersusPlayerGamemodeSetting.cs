using UnityEngine;

namespace Assets.Scripts.Settings.Game.Gamemodes
{
    [CreateAssetMenu(fileName = "PvP Gamemode Setting", menuName = "Settings/Gamemodes/PvP", order = 2)]
    public class PlayerVersusPlayerGamemodeSetting : GamemodeSetting
    {
        public override void Override(BaseSettings settings)
        {
            base.Override(settings);
            if (!(settings is PlayerVersusPlayerGamemodeSetting gamemode)) return;
        }
    }
}