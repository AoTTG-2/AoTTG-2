using Assets.Scripts.Settings.Types;
using Assets.Scripts.Settings.Validation;
using UnityEngine;

namespace Assets.Scripts.Settings.Game.Gamemodes
{
    [CreateAssetMenu(fileName = "Capture Gamemode Setting", menuName = "Settings/Gamemodes/Capture", order = 2)]
    public class CaptureGamemodeSetting : GamemodeSetting
    {
        [IntValidation(50, 50000, 0)]
        public IntSetting PvPTitanScoreLimit;
        [IntValidation(50, 50000, 0)]
        public IntSetting PvPHumanScoreLimit;
        public BoolSetting CheckpointSupplyStation;

        public override void Override(BaseSettings settings)
        {
            base.Override(settings);
            if (!(settings is CaptureGamemodeSetting gamemode)) return;
            if (gamemode.PvPTitanScoreLimit.HasValue) PvPTitanScoreLimit.Value = gamemode.PvPTitanScoreLimit.Value;
            if (gamemode.PvPHumanScoreLimit.HasValue) PvPHumanScoreLimit.Value = gamemode.PvPHumanScoreLimit.Value;
            if (gamemode.CheckpointSupplyStation.HasValue) CheckpointSupplyStation.Value = gamemode.CheckpointSupplyStation.Value;
        }
    }
}