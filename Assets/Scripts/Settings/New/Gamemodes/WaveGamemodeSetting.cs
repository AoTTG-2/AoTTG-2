using Assets.Scripts.Settings.New.Types;
using Assets.Scripts.Settings.New.Validation;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Gamemodes
{
    [CreateAssetMenu(fileName = "Waves Gamemode Setting", menuName = "Settings/Gamemodes/Waves", order = 1)]
    public class WaveGamemodeSetting : GamemodeSetting
    {
        [IntValidation(1, 998, 1)]
        public IntSetting StartWave;
        [IntValidation(1, 999, 20)]
        public IntSetting MaxWave;
    }
}
