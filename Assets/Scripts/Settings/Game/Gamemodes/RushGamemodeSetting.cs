using Assets.Scripts.Settings.Types;
using Assets.Scripts.UI.Elements;
using UnityEngine;

namespace Assets.Scripts.Settings.Game.Gamemodes
{
    [CreateAssetMenu(fileName = "Rush Gamemode Setting", menuName = "Settings/Gamemodes/Rush", order = 2)]
    public class RushGamemodeSetting : GamemodeSetting
    {
        [UiElement("Titan frequency", "1 titan will spawn per Interval")]
        public IntSetting TitanInterval;
        [UiElement("Titan Group Internval", "X titans will spawn per Interval")]
        public IntSetting TitanGroupInterval;
        [UiElement("Titan Group Size", "X titans will spawn per Interval")]
        public IntSetting TitanGroupSize;

        public override void Override(BaseSettings settings)
        {
            base.Override(settings);
            if (!(settings is RushGamemodeSetting gamemode)) return;
            if (gamemode.TitanInterval.HasValue) TitanInterval = gamemode.TitanInterval;
            if (gamemode.TitanGroupInterval.HasValue) TitanGroupInterval = gamemode.TitanGroupInterval;
            if (gamemode.TitanGroupSize.HasValue) TitanGroupSize = gamemode.TitanGroupSize;
        }
    }
}