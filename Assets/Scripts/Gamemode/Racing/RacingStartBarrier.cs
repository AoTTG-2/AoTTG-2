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
            yield return new WaitUntil(() => GameSettings.Gamemode != null);
            if (IsRacingOnly && GameSettings.Gamemode.GamemodeType != GamemodeType.Racing)
            {
                Destroy(gameObject);
                yield break;
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
