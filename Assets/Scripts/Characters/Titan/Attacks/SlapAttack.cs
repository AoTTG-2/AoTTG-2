using System;
using Assets.Scripts.Characters.Humans;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class SlapAttack : Attack<MindlessTitan>
    {
        public SlapAttack()
        {
            BodyParts = new[] { BodyPart.HandLeft, BodyPart.HandRight };
        }

        public override Type[] TargetTypes { get; } = { typeof(Human) };

        private BodyPart Hand { get; set; }
        public override bool CanAttack()
        {
            if (IsDisabled()) return false;
            Vector3 line = (Titan.Target.GetComponent<Rigidbody>().velocity * Time.deltaTime) * 30f;
            if (line.sqrMagnitude <= 10f) return false;
            if (this.simpleHitTestLineAndBall(line, Titan.Body.checkAeLeft.position - Titan.Target.transform.position, 5f * Titan.Size))
            {
                AttackAnimation = "attack_anti_AE_l";
                Hand = BodyPart.HandLeft;
                if (IsDisabled(Hand)) return false;
                return true;
            }
            if (this.simpleHitTestLineAndBall(line, Titan.Body.checkAeLLeft.position - Titan.Target.transform.position, 5f * Titan.Size))
            {
                AttackAnimation = "attack_anti_AE_low_l";
                Hand = BodyPart.HandLeft;
                if (IsDisabled(Hand)) return false;
                return true;
            }
            if (this.simpleHitTestLineAndBall(line, Titan.Body.checkAeRight.position - Titan.Target.transform.position, 5f * Titan.Size))
            {
                AttackAnimation = "attack_anti_AE_r";
                Hand = BodyPart.HandRight;
                if (IsDisabled(Hand)) return false;
                return true;
            }
            if (this.simpleHitTestLineAndBall(line, Titan.Body.checkAeLRight.position - Titan.Target.transform.position, 5f * Titan.Size))
            {
                AttackAnimation = "attack_anti_AE_low_r";
                Hand = BodyPart.HandRight;
                if (IsDisabled(Hand)) return false;
                return true;
            }
            return false;
        }

        public bool CanAttack(PlayerTitan titan, bool isLeftHand)
        {
            Hand = isLeftHand
                ? BodyPart.HandLeft
                : BodyPart.HandRight;
            if (titan.Body.IsDisabled(Hand)) return false;
            AttackAnimation = isLeftHand
                ? "attack_anti_AE_l"
                : "attack_anti_AE_r";
            return true;
        }

        private void HandleHit()
        {
            var hand = Hand == BodyPart.HandLeft
                ? Titan.Body.HandLeft
                : Titan.Body.HandRight;

            GameObject obj7 = this.checkIfHitHand(hand, Titan.Size);
            if (obj7 != null)
            {
                Vector3 vector4 = Titan.Body.Chest.position;
                if (!((!Titan.photonView.isMine) || obj7.GetComponent<Hero>().HasDied()))
                {
                    obj7.GetComponent<Hero>().MarkDie();
                    object[] objArray5 = new object[] { (Vector3)(((obj7.transform.position - vector4) * 15f) * Titan.Size), false, Titan.photonView.viewID, Titan.name, true };
                    obj7.GetComponent<Hero>().photonView.RPC(nameof(Hero.NetDie), PhotonTargets.All, objArray5);
                }
            }
        }

        public override void Execute()
        {
            if (IsFinished) return;
            if (!Titan.Animation.IsPlaying(AttackAnimation))
            {
                Titan.CrossFade(AttackAnimation, 0.1f);
                return;
            }

            if (IsDisabled(Hand))
            {
                IsFinished = true;
                return;
            }

            if (Titan.Animation[AttackAnimation].normalizedTime >= 0.31f &&
                Titan.Animation[AttackAnimation].normalizedTime <= 0.4f)
            {
                HandleHit();
            }

            if (Titan.Animation[AttackAnimation].normalizedTime >= 1f)
            {
                IsFinished = true;
            }
        }

        private bool simpleHitTestLineAndBall(Vector3 line, Vector3 ball, float R)
        {
            Vector3 rhs = Vector3.Project(ball, line);
            Vector3 vector2 = ball - rhs;
            if (vector2.magnitude > R)
            {
                return false;
            }
            if (Vector3.Dot(line, rhs) < 0f)
            {
                return false;
            }
            if (rhs.sqrMagnitude > line.sqrMagnitude)
            {
                return false;
            }
            return true;
        }
    }
}
