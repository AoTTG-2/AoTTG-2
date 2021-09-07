using Assets.Scripts.Gamemode;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Behavior
{
    /// <summary>
    /// Overrides the Titans state behavior for the <see cref="WaveGamemode"/>
    /// </summary>
    public class WaveBehavior : TitanBehavior
    {
        protected override bool OnChasingUpdateEverySecond(int seconds)
        {
            if (seconds % 2 != 0) return false;

            Entity target = null;
            float positiveInfinity = float.PositiveInfinity;
            Vector3 position = Titan.transform.position;
            foreach (var hostile in FactionService.GetAllHostile(Titan))
            {
                float num2 = Vector3.Distance(hostile.gameObject.transform.position, position);
                if (num2 < positiveInfinity)
                {
                    target = hostile;
                    positiveInfinity = num2;
                }
            }

            if (target != null)
            {
                Titan.OnTargetDetected(target);
            }

            return false;
        }

        protected override bool OnWanderingUpdateEverySecond(int seconds)
        {
            return OnChasingUpdateEverySecond(seconds);
        }

        protected override bool OnWandering()
        {
            if (Titan.Target != null)
            {
                Titan.SetState(TitanState.Chase);
                return true;
            }
            return false;
        }
    }
}
