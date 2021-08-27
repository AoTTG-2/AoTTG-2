using Assets.Scripts.Settings;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gamemode.Racing
{
    public class RacingStartBarrier : MonoBehaviour
    {
        public bool IsRacingOnly;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => Setting.Gamemode != null);
            if (IsRacingOnly && Setting.Gamemode.GamemodeType != GamemodeType.Racing)
            {
                Destroy(gameObject);
            }
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
