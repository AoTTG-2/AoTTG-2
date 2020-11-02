using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class SmashAttack : BoomAttack
    {
        public SmashAttack()
        {
            BodyParts = new[] { BodyPart.ArmRight, BodyPart.ArmLeft };
        }
        protected override string Effect { get; set; } = "FX/boom1";
        protected override float BoomTimer { get; set; } = 0.45f;
        protected override string AttackAnimation { get; set; } = "attack_front_ground";

        public override bool CanAttack(MindlessTitan titan)
        {
            if (titan.TargetDistance >= titan.AttackDistance * 2) return false;
            if (IsDisabled(titan)) return false;
            Vector3 vector18 = titan.Target.transform.position - titan.transform.position;
            var angle = -Mathf.Atan2(vector18.z, vector18.x) * 57.29578f;
            var between = -Mathf.DeltaAngle(angle, titan.gameObject.transform.rotation.eulerAngles.y - 90f);
            if (Mathf.Abs(between) < 90f && between > 0f && titan.TargetDistance < titan.AttackDistance * 0.25f)
            {
                TitanBodyPart = titan.TitanBody.AttackFrontGround;
                return true;
            }
            return false;
        }

        public override bool CanAttack(PlayerTitan titan)
        {
            if (IsDisabled(titan)) return false;
            TitanBodyPart = titan.TitanBody.AttackFrontGround;
            return true;
        }
    }
}
