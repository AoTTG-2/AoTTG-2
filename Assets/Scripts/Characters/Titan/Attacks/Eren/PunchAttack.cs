using System;
using Assets.Scripts.Characters.Humans;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks.Eren
{
    public class PunchAttack : Attack<TitanBase>
    {
        public PunchAttack()
        {
            BodyParts = new[] { BodyPart.HandRight, BodyPart.HandLeft };
        }

        public override Type[] TargetTypes { get; } = { typeof(Human), typeof(TitanBase) };

        private const string AnimationPunchRight = "attack_combo_001";
        private const string AnimationPunchLeft = "attack_combo_002";
        private const string AnimationHeavy = "attack_combo_003";

        private float attackCheckTimeA;
        private float attackCheckTimeB;

        private BodyPart Hand { get; set; }

        public override bool CanAttack()
        {
            if (!base.CanAttack()) return false;

            if (Titan.TargetDistance >= Titan.AttackDistance * 2) return false;
            Vector3 vector18 = Titan.Target.transform.position - Titan.transform.position;
            var angle = -Mathf.Atan2(vector18.z, vector18.x) * Mathf.Rad2Deg;
            var between = -Mathf.DeltaAngle(angle, Titan.gameObject.transform.rotation.eulerAngles.y - 90f);
            if (Mathf.Abs(between) < 90f)
            {
                if (Titan.TargetDistance < Titan.AttackDistance)
                {
                    IsFinished = false;
                    AttackAnimation = AnimationPunchRight;
                    Damage = 250;
                    SetCheckTimers();
                    return true;
                }
            }
            return false;
        }

        private void SetCheckTimers()
        {
            switch (AttackAnimation)
            {
                case AnimationPunchRight:
                    attackCheckTimeA = 0.4f;
                    attackCheckTimeB = 0.55f;
                    Hand = BodyPart.HandRight;
                    break;
                case AnimationPunchLeft:
                    attackCheckTimeA = 0.13f;
                    attackCheckTimeB = 0.36f;
                    Hand = BodyPart.HandLeft;
                    break;
                case AnimationHeavy:
                    attackCheckTimeA = 0.30f;
                    attackCheckTimeB = 0.38f;
                    Hand = BodyPart.HandRight;
                    break;
            }
        }

        private Transform GetHand()
        {
            return Hand == BodyPart.HandRight
                ? Titan.Body.HandRight
                : Titan.Body.HandLeft;
        }

        public override void Execute()
        {
            if (IsFinished) return;
            if (!Titan.Animation.IsPlaying(AttackAnimation))
            {
                Titan.CrossFade(AttackAnimation, 0.1f);
                return;
            }


            if (Titan.Animation[AttackAnimation].normalizedTime > 1f)
            {
                if (AttackAnimation == AnimationPunchRight)
                {
                    AttackAnimation = AnimationPunchLeft;
                    Damage = 250;
                    SetCheckTimers();
                    return;
                }

                if (AttackAnimation == AnimationPunchLeft)
                {
                    AttackAnimation = AnimationHeavy;
                    Damage = 1000;
                    SetCheckTimers();
                    return;
                }

                if (AttackAnimation == AnimationHeavy)
                {
                    Damage = 250;
                    IsFinished = true;
                    return;
                }
            }

            if (Titan.Animation[AttackAnimation].normalizedTime >= attackCheckTimeA &&
                Titan.Animation[AttackAnimation].normalizedTime <= attackCheckTimeB)
            {
                if (IsEntityHit(GetHand(), out var targets))
                {
                    foreach (var target in targets)
                    {
                        HitEntity(target);
                    }
                }
            }
        }
    }
}
