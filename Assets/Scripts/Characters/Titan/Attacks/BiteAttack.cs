using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class BiteAttack : BoomAttack
    {
        protected override string Effect { get; set; } = "fx/bite";

        public override bool CanAttack(MindlessTitan titan)
        {
            if (titan.TargetDistance >= titan.AttackDistance * 2) return false;
            if (titan.Target.transform.position.y <= titan.TitanBody.Neck.position.y - 3f * titan.Size
                || titan.TargetDistance >= titan.AttackDistance * 0.5f) return false;

            Vector3 vector18 = titan.Target.transform.position - titan.transform.position;
            var angle = -Mathf.Atan2(vector18.z, vector18.x) * 57.29578f;
            var between = -Mathf.DeltaAngle(angle, titan.gameObject.transform.rotation.eulerAngles.y - 90f);

            if (Mathf.Abs(between) >= 90f) return false;
            if (Mathf.Abs(between) < 30f)
            {
                if (Vector3.Distance(titan.Target.transform.position, titan.TitanBody.CheckFront.position) < (2.5f * titan.Size))
                {
                    AttackAnimation = "attack_bite";
                    BoomTimer = 0.6f;
                    TitanBodyPart = titan.TitanBody.AttackBite;
                    return true;
                }
            }
            else if (between > 0f)
            {
                if (Vector3.Distance(titan.Target.transform.position, titan.TitanBody.CheckFrontRight.position) < (2.5f * titan.Size))
                {
                    AttackAnimation = "attack_bite_r";
                    BoomTimer = 0.4f;
                    TitanBodyPart = titan.TitanBody.AttackBiteRight;
                    return true;
                }
            }
            if (Vector3.Distance(titan.Target.transform.position, titan.TitanBody.CheckFrontLeft.position) < (2.5f * titan.Size))
            {
                AttackAnimation = "attack_bite_l";
                BoomTimer = 0.4f;
                TitanBodyPart = titan.TitanBody.AttackBiteLeft;
                return true;
            }
            return false;
        }

        public bool CanAttack(PlayerTitan titan, float rotation)
        {
            if (rotation > 7.5f)
            {
                AttackAnimation = "attack_bite_r";
                BoomTimer = 0.4f;
                TitanBodyPart = titan.TitanBody.AttackBiteRight;
                return true;
            }

            if (rotation < -7.5f)
            {
                AttackAnimation = "attack_bite_l";
                BoomTimer = 0.4f;
                TitanBodyPart = titan.TitanBody.AttackBiteLeft;
                return true;
            }

            if (rotation >= -7.5f && rotation <= 7.5f)
            {
                AttackAnimation = "attack_bite";
                BoomTimer = 0.6f;
                TitanBodyPart = titan.TitanBody.AttackBite;
                return true;
            }

            return false;
        }
    }
}
