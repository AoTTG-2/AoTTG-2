using Assets.Scripts.Settings;
using UnityEngine;

namespace Assets.Scripts.Gamemode.Racing
{
    public class RacingStartBarrier : MonoBehaviour
    {
        public bool IsRacingOnly;

        private void Start()
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
            if (FengGameManagerMKII.Gamemode is RacingGamemode racingGamemode)
            {
                racingGamemode.StartBarriers.Remove(this);
            }
        }
    }
}
