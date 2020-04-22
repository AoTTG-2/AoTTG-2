using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class SlapAttack : Attack
    {
        private enum TitanHand
        {
            Left,
            Right
        }

        private string AttackAnimation { get; set; }
        private TitanHand Hand { get; set; }
        public override bool CanAttack(MindlessTitan titan)
        {
            Vector3 line = (Vector3)((titan.Target.GetComponent<Rigidbody>().velocity * Time.deltaTime) * 30f);
            if (line.sqrMagnitude <= 10f) return false;
            if (this.simpleHitTestLineAndBall(line, titan.TitanBody.checkAeLeft.position - titan.Target.transform.position, 5f * titan.Size))
            {
                AttackAnimation = "attack_anti_AE_l";
                Hand = TitanHand.Left;
                return true;
            }
            if (this.simpleHitTestLineAndBall(line, titan.TitanBody.checkAeLLeft.position - titan.Target.transform.position, 5f * titan.Size))
            {
                AttackAnimation = "attack_anti_AE_low_l";
                Hand = TitanHand.Left;
                return true;
            }
            if (this.simpleHitTestLineAndBall(line, titan.TitanBody.checkAeRight.position - titan.Target.transform.position, 5f * titan.Size))
            {
                AttackAnimation = "attack_anti_AE_r";
                Hand = TitanHand.Right;
                return true;
            }
            if (this.simpleHitTestLineAndBall(line, titan.TitanBody.checkAeLRight.position - titan.Target.transform.position, 5f * titan.Size))
            {
                AttackAnimation = "attack_anti_AE_low_r";
                Hand = TitanHand.Right;
                return true;
            }
            return false;
        }

        private void HandleHit(MindlessTitan titan)
        {
            var hand = Hand == TitanHand.Left
                ? titan.TitanBody.HandLeft
                : titan.TitanBody.HandRight;

            GameObject obj7 = this.checkIfHitHand(hand, titan.Size);
            if (obj7 != null)
            {
                Vector3 vector4 = titan.TitanBody.Chest.position;
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                {
                    obj7.GetComponent<Hero>().die((Vector3)(((obj7.transform.position - vector4) * 15f) * titan.Size), false);
                }
                else if (!(((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER) || !titan.photonView.isMine) || obj7.GetComponent<Hero>().HasDied()))
                {
                    obj7.GetComponent<Hero>().markDie();
                    object[] objArray5 = new object[] { (Vector3)(((obj7.transform.position - vector4) * 15f) * titan.Size), false, titan.photonView.viewID, titan.name, true };
                    obj7.GetComponent<Hero>().photonView.RPC("netDie", PhotonTargets.All, objArray5);
                }
            }
        }

        public override void Execute(MindlessTitan titan)
        {
            if (IsFinished) return;
            if (!titan.Animation.IsPlaying(AttackAnimation))
            {
                titan.Animation.CrossFade(AttackAnimation);
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
