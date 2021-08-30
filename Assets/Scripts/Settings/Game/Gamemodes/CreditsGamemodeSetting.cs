using Assets.Scripts.Gamemode.Credits;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Settings.Game.Gamemodes
{
    [CreateAssetMenu(fileName = "Credits", menuName = "Settings/Gamemodes/Credits", order = 2)]
    public class CreditsGamemodeSetting : GamemodeSetting
    {
        [Tooltip("A list of all the awesome people who developed AoTTG2 & noticeable people from AoTTG1")]
        public List<Contributor> Contributors;


        public override void Override(BaseSettings settings)
        {
            base.Override(settings);
            if (!(settings is CreditsGamemodeSetting gamemode)) return;
            Contributors = gamemode.Contributors;
        }
    }
}
