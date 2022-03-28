using Assets.Scripts.Settings.Types;
using Assets.Scripts.UI.Elements;
using UnityEngine;

namespace Assets.Scripts.Settings.Game.Gamemodes
{
    [CreateAssetMenu(fileName = "Infection Gamemode Setting", menuName = "Settings/Gamemodes/Infection", order = 2)]
    public class InfectionGamemodeSetting : GamemodeSetting
    {
        [UiElement("Start Infected", "The amount of players that start as an infected")]
        public IntSetting Infected;
        public override void Override(BaseSettings settings)
        {
            base.Override(settings);
            if (!(settings is InfectionGamemodeSetting gamemode)) return;
            if (gamemode.Infected.HasValue) Infected = gamemode.Infected;
        }
    }
}