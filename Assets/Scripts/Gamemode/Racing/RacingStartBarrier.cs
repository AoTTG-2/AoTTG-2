using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gamemode.Racing
{
    public class RacingStartBarrier : MonoBehaviour
    {
        public bool IsRacingOnly;

        private void Awake()
        {
            if (IsRacingOnly && FengGameManagerMKII.Gamemode.Settings.GamemodeType != GamemodeType.Racing)
            {
                Destroy(gameObject);
                return;
            }

            if (FengGameManagerMKII.instance.racingDoors == null)
            {
                FengGameManagerMKII.instance.racingDoors = new List<GameObject>();
            }
            FengGameManagerMKII.instance.racingDoors.Add(gameObject);
        }

        private void OnDestroy()
        {
            FengGameManagerMKII.instance.racingDoors?.Remove(gameObject);
        }
    }
}
