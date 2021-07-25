using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class BiteAttack : BoomAttack
    {
        protected override string Effect { get; set; } = "fx/bite";

        public override bool CanAttack()
        {
            if (Titan.TargetDistance >= Titan.AttackDistance * 2) return false;
            if (Titan.Target.transform.position.y <= Titan.Body.Neck.position.y - 3f * Titan.Size
                || Titan.TargetDistance >= Titan.AttackDistance * 0.5f) return false;

            Vector3 vector18 = Titan.Target.transform.position - Titan.transform.position;
            var angle = -Mathf.Atan2(vector18.z, vector18.x) * Mathf.Rad2Deg;
            var between = -Mathf.DeltaAngle(angle, Titan.gameObject.transform.rotation.eulerAngles.y - 90f);

            if (Mathf.Abs(between) >= 90f) return false;
            if (Mathf.Abs(between) < 30f)
            {
                if (Vector3.Distance(Titan.Target.transform.position, Titan.Body.CheckFront.position) < (2.5f * Titan.Size))
                {
                    AttackAnimation = "attack_bite";
                    BoomTimer = 0.6f;
                    TitanBodyPart = Titan.Body.AttackBite;
                    return true;
                }
            }
            else if (between > 0f)
            {
                if (Vector3.Distance(Titan.Target.transform.position, Titan.Body.CheckFrontRight.position) < (2.5f * Titan.Size))
                {
                    AttackAnimation = "attack_bite_r";
                    BoomTimer = 0.4f;
                    TitanBodyPart = Titan.Body.AttackBiteRight;
                    return true;
                }
            }
            if (Vector3.Distance(Titan.Target.transform.position, Titan.Body.CheckFrontLeft.position) < (2.5f * Titan.Size))
            {
                AttackAnimation = "attack_bite_l";
                BoomTimer = 0.4f;
                TitanBodyPart = Titan.Body.AttackBiteLeft;
                return true;
            }
            return false;
        }

        public bool CanAttack(PlayerTitan Titan, float rotation)
        {
            if (rotation > 7.5f)
            {
                AttackAnimation = "attack_bite_r";
                BoomTimer = 0.4f;
                TitanBodyPart = Titan.Body.AttackBiteRight;
                return true;
            }

            if (rotation < -7.5f)
            {
                AttackAnimation = "attack_bite_l";
                BoomTimer = 0.4f;
                TitanBodyPart = Titan.Body.AttackBiteLeft;
                return true;
            }

            if (rotation >= -7.5f && rotation <= 7.5f)
            {
                AttackAnimation = "attack_bite";
                BoomTimer = 0.6f;
                TitanBodyPart = Titan.Body.AttackBite;
                return true;
            }

            return false;
        }
    }
}
