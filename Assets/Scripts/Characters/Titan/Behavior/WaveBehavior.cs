using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Behavior
{
    public class WaveBehavior : TitanBehavior
    {
        protected override bool OnChasingUpdateEverySecond(int seconds)
        {
            if (seconds % 2 != 0) return false;

            GameObject obj2 = null;
            float positiveInfinity = float.PositiveInfinity;
            Vector3 position = Titan.transform.position;
            foreach (Hero hero in FengGameManagerMKII.instance.getPlayers())
            {
                GameObject gameObject = hero.gameObject;
                float num2 = Vector3.Distance(gameObject.transform.position, position);
                if (num2 < positiveInfinity)
                {
                    obj2 = gameObject;
                    positiveInfinity = num2;
                }
            }

            if (obj2 != null)
            {
                Titan.OnTargetDetected(obj2);
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
                Titan.ChangeState(MindlessTitanState.Chase);
                return true;
            }
            return false;
        }
    }
}
