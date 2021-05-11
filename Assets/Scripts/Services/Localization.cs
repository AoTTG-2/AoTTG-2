using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace Assets.Scripts.Services
{
    public class Localization : MonoBehaviour
    {
        [SerializeField] private LocalizedStringTable commonTable;

        [Header("Gamemodes")]
        [SerializeField] private LocalizedStringTable catchGamemodeTable;
        [SerializeField] private LocalizedStringTable racingGamemodeTable;
        [SerializeField] private LocalizedStringTable waveGamemodeTable;

        public static StringTable Common { get; set; }
        public static GamemodeLocalization Gamemode { get; set; } = new GamemodeLocalization();

        private void Start()
        {
            commonTable.GetTable().Completed += (op) => { Common = op.Result; };
            catchGamemodeTable.GetTable().Completed += (op) => { Gamemode.CatchGamemode = op.Result; };
            racingGamemodeTable.GetTable().Completed += (op) => { Gamemode.RacingGamemode = op.Result; };
            waveGamemodeTable.GetTable().Completed += (op) => { Gamemode.WaveGamemode = op.Result; };
        }

        public void ReloadLocalization()
        {
            Start();
        }
    }

    public class GamemodeLocalization
    {
        public StringTable CatchGamemode { get; set; }
        public StringTable RacingGamemode { get; set; }
        public StringTable WaveGamemode { get; set; }
    }
}
