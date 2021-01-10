using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Behavior
{
    public class RandomAttackBehavior : TitanBehavior
    {
        private float nextUpdate = 0.1f;
        protected override bool OnWandering()
        {
            //if (Time.time >= nextUpdate)
            //{
            //    nextUpdate = Mathf.FloorToInt(Time.time) + 0.1f;
            //    if (Random.Range(0, 100f) > 98.5f)
            //    {
            //        Titan.SetState(TitanState.Attacking);
            //        Titan.CurrentAttack = Titan.Attacks[Random.Range(0, Titan.Attacks.Length)];
            //        return true;
            //    }
            //}

            return false;
        }

        protected override bool OnChase()
        {
            return OnWandering();
        }
    }
}
