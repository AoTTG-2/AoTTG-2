using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Settings.Types;
using Assets.Scripts.Settings.Validation;
using UnityEngine;

namespace Assets.Scripts.Settings.Game.Gamemodes
{
    [CreateAssetMenu(fileName = "Waves Gamemode Setting", menuName = "Settings/Gamemodes/Waves", order = 1)]
    public class WaveGamemodeSetting : GamemodeSetting
    {
        [Header("Gamemode Specific")]
        [IntValidation(1, 998, 1)]
        public IntSetting StartWave;
        [IntValidation(1, 999, 20)]
        public IntSetting MaxWave;
        public IntSetting WaveIncrement;
        public IntSetting BossWave;
        public EnumSetting<MindlessTitanType> BossType;

        public override void Override(BaseSettings settings)
        {
            base.Override(settings);
            if (!(settings is WaveGamemodeSetting gamemode)) return;
            if (gamemode.StartWave.HasValue) StartWave.Value = gamemode.StartWave.Value;
            if (gamemode.MaxWave.HasValue) MaxWave.Value = gamemode.MaxWave.Value;
            if (gamemode.WaveIncrement.HasValue) WaveIncrement.Value = gamemode.WaveIncrement.Value;
            if (gamemode.BossWave.HasValue) BossWave.Value = gamemode.BossWave.Value;
            if (gamemode.BossType.HasValue) BossType.Value = gamemode.BossType.Value;
        }
    }
}
