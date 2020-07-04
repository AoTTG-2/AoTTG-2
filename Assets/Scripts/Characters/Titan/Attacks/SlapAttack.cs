using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class SlapAttack : Attack
    {
        public SlapAttack()
        {
            BodyParts = new[] { BodyPart.HandLeft, BodyPart.HandRight };
        }

        private string AttackAnimation { get; set; }
        private BodyPart Hand { get; set; }
        public override bool CanAttack(MindlessTitan titan)
        {
            if (IsDisabled(titan)) return false;
            Vector3 line = (Vector3)((titan.Target.GetComponent<Rigidbody>().velocity * Time.deltaTime) * 30f);
            if (line.sqrMagnitude <= 10f) return false;
            if (this.simpleHitTestLineAndBall(line, titan.TitanBody.checkAeLeft.position - titan.Target.transform.position, 5f * titan.Size))
            {
                AttackAnimation = "attack_anti_AE_l";
                Hand = BodyPart.HandLeft;
                if (IsDisabled(titan, Hand)) return false;
                return true;
            }
            if (this.simpleHitTestLineAndBall(line, titan.TitanBody.checkAeLLeft.position - titan.Target.transform.position, 5f * titan.Size))
            {
                AttackAnimation = "attack_anti_AE_low_l";
                Hand = BodyPart.HandLeft;
                if (IsDisabled(titan, Hand)) return false;
                return true;
            }
            if (this.simpleHitTestLineAndBall(line, titan.TitanBody.checkAeRight.position - titan.Target.transform.position, 5f * titan.Size))
            {
                AttackAnimation = "attack_anti_AE_r";
                Hand = BodyPart.HandRight;
                if (IsDisabled(titan, Hand)) return false;
                return true;
            }
            if (this.simpleHitTestLineAndBall(line, titan.TitanBody.checkAeLRight.position - titan.Target.transform.position, 5f * titan.Size))
            {
                AttackAnimation = "attack_anti_AE_low_r";
                Hand = BodyPart.HandRight;
                if (IsDisabled(titan, Hand)) return false;
                return true;
            }
            return false;
        }

        public bool CanAttack(PlayerTitan titan, bool isLeftHand)
        {
            Hand = isLeftHand
                ? BodyPart.HandLeft
                : BodyPart.HandRight;
            if (titan.IsDisabled(Hand)) return false;
            AttackAnimation = isLeftHand
                ? "attack_anti_AE_l"
                : "attack_anti_AE_r";
            return true;
        }

        private void HandleHit(MindlessTitan titan)
        {
            var hand = Hand == BodyPart.HandLeft
                ? titan.TitanBody.HandLeft
                : titan.TitanBody.HandRight;

            GameObject slappedGobj = this.checkIfHitHand(hand, titan.Size);
            if (slappedGobj != null)
            {
                Vector3 vector4 = titan.TitanBody.Chest.position;
                var slappedHero = slappedGobj.GetComponent<Hero>();
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                {
                    slappedHero.die((Vector3) (((slappedGobj.transform.position - vector4) * 15f) * titan.Size), false);
                }
                else if (!(((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER) || !titan.photonView.isMine) || slappedHero.HasDied()))
                {
                    slappedHero.markDie();
                    slappedHero.photonView.RPC(
                        nameof(slappedHero.netDie),
                        PhotonTargets.All,
                        (slappedGobj.transform.position - vector4) * 15f * titan.Size,
                        false,
                        titan.photonView.viewID,
                        titan.name,
                        true);
                }
            }
        }

        public override void Execute(MindlessTitan titan)
        {
            if (IsFinished) return;
            if (!titan.Animation.IsPlaying(AttackAnimation))
            {
                titan.CrossFade(AttackAnimation, 0.1f);
                return;
            }

            if (IsDisabled(titan, Hand))
            {
                IsFinished = true;
                return;
            }

            if (titan.Animation[AttackAnimation].normalizedTime >= 0.31f &&
                titan.Animation[AttackAnimation].normalizedTime <= 0.4f)
            {
                HandleHit(titan);
            }

            if (titan.Animation[AttackAnimation].normalizedTime >= 1f)
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
