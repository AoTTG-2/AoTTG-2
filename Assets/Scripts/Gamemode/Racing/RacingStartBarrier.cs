using Assets.Scripts.Settings;
using UnityEngine;

namespace Assets.Scripts.Gamemode.Racing
{
    public class RacingStartBarrier : MonoBehaviour
    {
        public bool IsRacingOnly;

        private void Awake()
        {
            if (IsRacingOnly && GameSettings.Gamemode.GamemodeType != GamemodeType.Racing)
            {
                Destroy(gameObject);
                return;
            }

            var racingGamemode = (RacingGamemode) FengGameManagerMKII.Gamemode;
            racingGamemode.StartBarriers.Add(this);
        }

        private void OnDestroy()
        {
            var racingGamemode = (RacingGamemode) FengGameManagerMKII.Gamemode;
            racingGamemode.StartBarriers.Remove(this);
        }
    }
}
