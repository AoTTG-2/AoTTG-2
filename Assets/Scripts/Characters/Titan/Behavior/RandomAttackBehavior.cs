using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Behavior
{
    public class RandomAttackBehavior : TitanBehavior
    {
        private float nextUpdate = 0.1f;
        protected override bool OnWandering(MindlessTitan titan)
        {
            if (Time.time >= nextUpdate)
            {
                nextUpdate = Mathf.FloorToInt(Time.time) + 0.1f;
                if (Random.Range(0, 100f) > 98.5f)
                {
                    titan.ChangeState(MindlessTitanState.Attacking);
                    titan.CurrentAttack = titan.Attacks[Random.Range(0, titan.Attacks.Count)];
                    return true;
                }
            }

            return false;
        }

        protected override bool OnChase(MindlessTitan titan)
        {
            return OnWandering(titan);
        }
    }
}
