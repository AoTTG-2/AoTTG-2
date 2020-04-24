using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class SlapFaceAttack : BoomAttack
    {
        protected override string Effect { get; set; } = "FX/boom3";
        protected override float BoomTimer { get; set; } = 0.66f;
        public override bool CanAttack(MindlessTitan titan)
        {
            if (titan.Target.transform.position.y <= titan.TitanBody.Neck.position.y - 3f * titan.Size
                || titan.TargetDistance >= titan.AttackDistance * 0.5f) return false;

            Vector3 vector18 = titan.Target.transform.position - titan.transform.position;
            var angle = -Mathf.Atan2(vector18.z, vector18.x) * 57.29578f;
            var between = -Mathf.DeltaAngle(angle, titan.gameObject.transform.rotation.eulerAngles.y - 90f);

            if (Mathf.Abs(between) < 30f)
            {
                if (Vector3.Distance(titan.Target.transform.position, titan.TitanBody.CheckFront.position) < (2.5f * titan.Size))
                {
                    AttackAnimation = "attack_slap_face";
                    TitanBodyPart = titan.TitanBody.AttackSlapFace;
                    return true;
                }
            }
            else if (between > 0f)
            {
                if (Vector3.Distance(titan.Target.transform.position, titan.TitanBody.CheckBackRight.position) < (2.8f * titan.Size))
                {
                    AttackAnimation = "attack_slap_back";
                    TitanBodyPart = titan.TitanBody.AttackSlapBack;
                    return true;
                }
            }
            else if (Vector3.Distance(titan.Target.transform.position, titan.TitanBody.CheckBackLeft.position) < (2.8f * titan.Size))
            {
                AttackAnimation = "attack_slap_back";
                TitanBodyPart = titan.TitanBody.AttackSlapBack;
                return true;
            }
            return false;
        }
    }
}
