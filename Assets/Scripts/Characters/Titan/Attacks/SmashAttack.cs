using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class SmashAttack : BoomAttack
    {
        public SmashAttack()
        {
            BodyParts = new[] { BodyPart.ArmRight, BodyPart.ArmLeft };
            AttackAnimation = "attack_front_ground";
        }
        protected override string Effect { get; set; } = "FX/boom1";
        protected override float BoomTimer { get; set; } = 0.45f;

        public override bool CanAttack()
        {
            if (Titan.TargetDistance >= Titan.AttackDistance * 2) return false;
            if (IsDisabled()) return false;
            Vector3 vector18 = Titan.Target.transform.position - Titan.transform.position;
            var angle = -Mathf.Atan2(vector18.z, vector18.x) * Mathf.Rad2Deg;
            var between = -Mathf.DeltaAngle(angle, Titan.gameObject.transform.rotation.eulerAngles.y - 90f);
            if (Mathf.Abs(between) < 90f && between > 0f && Titan.TargetDistance < Titan.AttackDistance * 0.25f)
            {
                TitanBodyPart = Titan.Body.AttackFrontGround;
                return true;
            }
            return false;
        }

        public override bool CanAttack(PlayerTitan titan)
        {
            if (IsDisabled()) return false;
            TitanBodyPart = Titan.Body.AttackFrontGround;
            return true;
        }
    }
}
