using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Settings.New.Types;
using Assets.Scripts.Settings.New.Validation;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Game.Gamemodes
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
    }
}
