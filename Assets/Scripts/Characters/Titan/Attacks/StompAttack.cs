using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class StompAttack : BoomAttack
    {
        protected override string Effect { get; set; } = "FX/boom2";
        protected override float BoomTimer { get; set; } = 0.42f;
        protected override string AttackAnimation { get; set; } = "attack_stomp";

        public override bool CanAttack(MindlessTitan titan)
        {
            Vector3 vector18 = titan.Target.transform.position - titan.transform.position;
            var angle = -Mathf.Atan2(vector18.z, vector18.x) * 57.29578f;
            var between = -Mathf.DeltaAngle(angle, titan.gameObject.transform.rotation.eulerAngles.y - 90f);
            if (Mathf.Abs(between) < 90f && between > 0f && titan.TargetDistance < titan.AttackDistance * 0.25)
            {
                TitanBodyPart = titan.TitanBody.AttackStomp;
                return true;
            }
            return false;
        }
    }
}
