using Assets.Scripts.Settings.Types;
using Assets.Scripts.Settings.Validation;
using Assets.Scripts.UI.Elements;
using UnityEngine;

namespace Assets.Scripts.Settings.Game.Gamemodes
{
    [CreateAssetMenu(fileName = "Catch", menuName = "Settings/Gamemodes/Catch", order = 2)]
    public class CatchGamemodeSetting : GamemodeSetting
    {
        [UiElement("CATCH_TOTAL_BALLS", "CATCH_TOTAL_BALLS_DESC")]
        [IntValidation(1, 10, 1)]
        public IntSetting TotalBalls;
        [FloatValidation(25f, 500f, 150f)]
        public FloatSetting BallSpeed;
        [FloatValidation(1.0f, 100f, 25f)]
        public FloatSetting BallSize;
        [IntValidation(0, 100, 5)]
        public IntSetting PointLimit;

        public override void Override(BaseSettings settings)
        {
            base.Override(settings);
            if (!(settings is CatchGamemodeSetting gamemode)) return;
            if (gamemode.TotalBalls.HasValue) TotalBalls.Value = gamemode.TotalBalls.Value;
            if (gamemode.BallSpeed.HasValue) BallSpeed.Value = gamemode.BallSpeed.Value;
            if (gamemode.BallSize.HasValue) BallSize.Value = gamemode.BallSize.Value;
            if (gamemode.PointLimit.HasValue) PointLimit.Value = gamemode.PointLimit.Value;
        }
    }
}