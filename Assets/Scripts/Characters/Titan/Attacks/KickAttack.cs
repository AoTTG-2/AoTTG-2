using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class KickAttack : BoomAttack
    {
        public KickAttack()
        {
            BodyParts = new[] {BodyPart.LegLeft};
        }
        protected override string Effect { get; set; } = "FX/boom5";
        protected override float BoomTimer { get; set; } = 0.43f;
        protected override string AttackAnimation { get; set; } = "attack_kick";

        public override bool CanAttack(MindlessTitan titan)
        {
            if (IsDisabled(titan)) return false;
            Vector3 vector18 = titan.Target.transform.position - titan.transform.position;
            var angle = -Mathf.Atan2(vector18.z, vector18.x) * 57.29578f;
            var between = -Mathf.DeltaAngle(angle, titan.gameObject.transform.rotation.eulerAngles.y - 90f);
            if (Mathf.Abs(between) >= 90f || between <= 0 ||
                titan.TargetDistance > titan.AttackDistance * 0.25f) return false;

            TitanBodyPart = titan.TitanBody.AttackKick;
            return true;
        }
    }
}
