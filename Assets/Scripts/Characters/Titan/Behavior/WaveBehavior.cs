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
            float current;
            float chaseDistance = 999999f;
            float f;
            foreach (Hero hero in FengGameManagerMKII.instance.getPlayers())
            {
                if (hero.HasDied()) continue;
                GameObject gameObject = hero.gameObject;
                float num5 = Vector3.Distance(gameObject.transform.position, position);
                if (num5 < positiveInfinity)
                {
                    Vector3 vector2 = gameObject.transform.position - Titan.transform.position;
                    current = -Mathf.Atan2(vector2.z, vector2.x) * 57.29578f;
                    f = -Mathf.DeltaAngle(current, Titan.gameObject.transform.rotation.eulerAngles.y - 90f);
                    if (Mathf.Abs(f) < chaseDistance)
                    {
                        obj2 = gameObject;
                        positiveInfinity = num5;
                    }
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
    }
}
