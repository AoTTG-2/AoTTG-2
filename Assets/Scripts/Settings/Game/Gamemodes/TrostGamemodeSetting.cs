using UnityEngine;

namespace Assets.Scripts.Settings.Game.Gamemodes
{
    [CreateAssetMenu(fileName = "Trost Gamemode Setting", menuName = "Settings/Gamemodes/Trost", order = 2)]
    public class TrostGamemodeSetting : GamemodeSetting
    {
        public override void Override(BaseSettings settings)
        {
            base.Override(settings);
            if (!(settings is TrostGamemodeSetting gamemode)) return;
        }
    }
}