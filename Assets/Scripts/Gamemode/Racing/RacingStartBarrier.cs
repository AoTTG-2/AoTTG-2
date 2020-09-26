using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gamemode.Racing
{
    public class RacingStartBarrier : RacingGameComponent
    {
        public bool IsRacingOnly;

        protected override void Awake()
        {
            if (IsRacingOnly)
                base.Awake();
        }
    }
}
