using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class StompAttack : BoomAttack
    {
        public StompAttack()
        {
            BodyParts = new[] {BodyPart.LegLeft};
            AttackAnimation = "attack_stomp";
        }
        protected override string Effect { get; set; } = "FX/boom2";
        protected override float BoomTimer { get; set; } = 0.42f;

        public override bool CanAttack()
        {
            if (Titan.TargetDistance >= Titan.AttackDistance * 2) return false;
            if (IsDisabled()) return false;
            Vector3 vector18 = Titan.Target.transform.position - Titan.transform.position;
            var angle = -Mathf.Atan2(vector18.z, vector18.x) * Mathf.Rad2Deg;
            var between = -Mathf.DeltaAngle(angle, Titan.gameObject.transform.rotation.eulerAngles.y - 90f);
            if (Mathf.Abs(between) < 90f && between > 0f && Titan.TargetDistance < Titan.AttackDistance * 0.25)
            {
                TitanBodyPart = Titan.Body.AttackStomp;
                return true;
            }
            return false;
        }
    }
}
