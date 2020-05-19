using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class ComboAttack : Attack
    {
        public ComboAttack(bool isPunk = false)
        {
            BodyParts = new[] { BodyPart.HandRight, BodyPart.HandLeft };
            this.isPunk = isPunk;
        }

        private readonly bool isPunk;
        private string AttackAnimation { get; set; }
        private const string AnimationPunchRight = "attack_combo_1";
        private const string AnimationPunchLeft  = "attack_combo_2";
        private const string AnimationSlam = "attack_combo_3";
        private const string BoomEffect = "FX/boom1";

        private float attackCheckTimeA;
        private float attackCheckTimeB;

        private BodyPart Hand { get; set; }
        private bool HasExploded { get; set; }

        public override bool CanAttack(MindlessTitan titan)
        {
            if (IsDisabled(titan)) return false;
            Vector3 vector18 = titan.Target.transform.position - titan.transform.position;
            var angle = -Mathf.Atan2(vector18.z, vector18.x) * 57.29578f;
            var between = -Mathf.DeltaAngle(angle, titan.gameObject.transform.rotation.eulerAngles.y - 90f);
            if (Mathf.Abs(between) < 90f)
            {
                if (titan.TargetDistance < titan.AttackDistance * 0.75f)
                {
                    IsFinished = false;
                    HasExploded = false;
                    AttackAnimation = AnimationPunchRight;
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
                    attackCheckTimeA = 0.54f;
                    attackCheckTimeB = 0.76f;
                    Hand = BodyPart.HandRight;
                    break;
                case AnimationPunchLeft:
                    attackCheckTimeA = 0.37f;
                    attackCheckTimeB = 0.57f;
                    Hand = BodyPart.HandLeft;
                    break;
            }
        }

        private Transform GetHand(MindlessTitan titan)
        {
            return Hand == BodyPart.HandRight 
                ? titan.TitanBody.HandRight 
                : titan.TitanBody.HandLeft;
        }

        public override void Execute(MindlessTitan titan)
        {
            if (IsFinished) return;
            if (!titan.Animation.IsPlaying(AttackAnimation))
            {
                titan.CrossFade(AttackAnimation, 0.1f);
                return;
            }


            if (titan.Animation[AttackAnimation].normalizedTime > 1f)
            {
                if (AttackAnimation == AnimationPunchRight)
                {
                    AttackAnimation = AnimationPunchLeft;
                    SetCheckTimers();
                    return;
                }

                if (AttackAnimation == AnimationPunchLeft)
                {
                    if (isPunk)
                    {
                        IsFinished = true;
                        return;
                    }

                    AttackAnimation = AnimationSlam;
                    SetCheckTimers();
                    return;
                }

                if (AttackAnimation == AnimationSlam)
                {
                    IsFinished = true;
                }
            }

            if (AttackAnimation == AnimationPunchRight
                || AttackAnimation == AnimationPunchLeft)
            {
                Vector3 vector12 = titan.transform.forward * titan.Speed * 0.40f;
                Vector3 vector14 = vector12 - titan.Rigidbody.velocity;
                vector14.x = Mathf.Clamp(vector14.x, -10f, 10f);
                vector14.z = Mathf.Clamp(vector14.z, -10f, 10f);
                vector14.y = 0f;
                titan.Rigidbody.AddForce(vector14, ForceMode.VelocityChange);
                if (titan.Animation[AttackAnimation].normalizedTime >= attackCheckTimeA &&
                    titan.Animation[AttackAnimation].normalizedTime <= attackCheckTimeB)
                {
                    var target = checkIfHitHand(GetHand(titan), titan.Size);
                    if (target != null)
                    {
                        Vector3 position = titan.TitanBody.Chest.position;
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                        {
                            target.GetComponent<Hero>().die((Vector3)(((target.transform.position - position) * 15f) * titan.Size), false);
                        }
                        else if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER || titan.photonView.isMine || !target.GetComponent<Hero>().HasDied()))
                        {
                            target.GetComponent<Hero>().markDie();
                            object[] objArray3 = { (Vector3)((target.transform.position - position) * 15f * titan.Size), false, titan.photonView.viewID, titan.name, true };
                            target.GetComponent<Hero>().photonView.RPC("netDie", PhotonTargets.All, objArray3);
                        }
                    }
                }
            }

            if (AttackAnimation == AnimationSlam)
            {
                if (!HasExploded && titan.Animation[AttackAnimation].normalizedTime >= 0.21f)
                {
                    HasExploded = true;
                    GameObject obj9;
                    var rotation = Quaternion.Euler(270f, 0f, 0f);
                    if (titan.photonView.isMine)
                    {
                        obj9 = PhotonNetwork.Instantiate(BoomEffect, titan.TitanBody.AttackFrontGround.position, rotation, 0);
                    }
                    else
                    {
                        return;
                    }
                    obj9.transform.localScale = titan.transform.localScale;
                    if (obj9.GetComponent<EnemyfxIDcontainer>() != null)
                    {
                        obj9.GetComponent<EnemyfxIDcontainer>().titanName = titan.name;
                    }
                }
            }
        }
    }
}
