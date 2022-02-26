using Assets.Scripts.Audio;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Gamemode;
using Assets.Scripts.Services;
using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class GrabAttack : Attack<MindlessTitan>
    {
        public GrabAttack()
        {
            BodyParts = new[] { BodyPart.HandLeft, BodyPart.HandRight };
        }

        public override Type[] TargetTypes { get; } = { typeof(Human) };

        private float AttackCheckTimeA { get; set; }

        private float AttackCheckTimeB { get; set; }

        private GameObject GrabbedTarget { get; set; }

        private BodyPart Hand { get; set; }

        public override bool CanAttack()
        {
            if (Titan.Target.GetType().IsAssignableFrom(typeof(Human))) return false;

            if (Titan.TargetDistance >= Titan.AttackDistance * 2) return false;
            if (IsDisabled()) return false;

            GrabbedTarget = null;
            var delta = Titan.Target.transform.position - Titan.transform.position;
            var angle = -Mathf.Atan2(delta.z, delta.x) * Mathf.Rad2Deg;
            var between = -Mathf.DeltaAngle(angle, Titan.gameObject.transform.rotation.eulerAngles.y - 90f);

            if (Titan.Target.transform.position.y > Titan.Body.Neck.position.y - 3f * Titan.Size
                && Titan.TargetDistance < Titan.AttackDistance * 0.5f)
            {
                if (Vector3.Distance(Titan.Target.transform.position, Titan.Body.CheckOverhead.position) < (3.6f * Titan.Size))
                {
                    if (between > 0f)
                    {
                        AttackAnimation = "grab_head_front_r";
                        Hand = BodyPart.HandRight;
                    }
                    else
                    {
                        AttackAnimation = "grab_head_front_l";
                        Hand = BodyPart.HandLeft;
                    }

                    if (IsDisabled(Hand)) return false;
                    AttackCheckTimeA = 0.38f;
                    AttackCheckTimeB = 0.55f;
                    return true;
                }

                if (between > 0f)
                {
                    if (Vector3.Distance(Titan.Target.transform.position, Titan.Body.CheckBackRight.position) < (2.8f * Titan.Size))
                    {
                        AttackAnimation = "grab_head_back_r";
                        Hand = BodyPart.HandLeft;
                        if (IsDisabled(Hand)) return false;
                        AttackCheckTimeA = 0.45f;
                        AttackCheckTimeB = 0.5f;
                        return true;
                    }
                }
                if (Vector3.Distance(Titan.Target.transform.position, Titan.Body.CheckBackLeft.position) < (2.8f * Titan.Size))
                {
                    AttackAnimation = "grab_head_back_l";
                    Hand = BodyPart.HandRight;
                    if (IsDisabled(Hand)) return false;
                    AttackCheckTimeA = 0.45f;
                    AttackCheckTimeB = 0.5f;
                    return true;
                }
            }

            if (Titan.Difficulty > Difficulty.Normal)
            {
                var targetHeight = Titan.Target.transform.position.y;
                var titanGrabHeight = Titan.Body.AttackFrontGround.position.y;
                Debug.Log($"Target height: {Titan.Target.transform.position.y}, hand height: {Titan.Body.AttackFrontGround.position.y}");

                if (targetHeight < titanGrabHeight - 2f || targetHeight > titanGrabHeight + 5f * Titan.Size)
                {
                    Debug.Log("Let's not grab");
                    return false;
                }
            }
            if (Mathf.Abs(between) < 90f && Titan.TargetDistance < (Titan.AttackDistance * 0.5f))
            {
                AttackAnimation = between > 0f
                    ? "grab_ground_front_r"
                    : "grab_ground_front_l";
                Hand = between > 0f
                    ? BodyPart.HandRight
                    : BodyPart.HandLeft;
                if (IsDisabled(Hand)) return false;
                AttackCheckTimeA = 0.37f;
                AttackCheckTimeB = 0.6f;
                return true;
            }

            if (Titan.TargetDistance < (Titan.AttackDistance * 0.5f))
            {
                AttackAnimation = between > 0f
                    ? "grab_ground_back_r"
                    : "grab_ground_back_l";
                Hand = between > 0f
                    ? BodyPart.HandRight
                    : BodyPart.HandLeft;
                if (IsDisabled(Hand)) return false;
                AttackCheckTimeA = 0.34f;
                AttackCheckTimeB = 0.49f;
                return true;
            }
            return false;
        }

        public bool CanAttackGroundFront(PlayerTitan titan, bool isLeftHand)
        {
            Hand = isLeftHand
                ? BodyPart.HandLeft
                : BodyPart.HandRight;
            if (titan.Body.IsDisabled(Hand)) return false;
            AttackAnimation = isLeftHand
                ? "grab_ground_front_l"
                : "grab_ground_front_r";
            AttackCheckTimeA = 0.37f;
            AttackCheckTimeB = 0.6f;
            return true;
        }

        public bool CanAttackGroundBack(PlayerTitan titan, bool isLeftHand)
        {
            Hand = isLeftHand
                ? BodyPart.HandLeft
                : BodyPart.HandRight;
            if (titan.Body.IsDisabled(Hand)) return false;
            AttackAnimation = isLeftHand
                ? "grab_ground_back_l"
                : "grab_ground_back_r";
            AttackCheckTimeA = 0.34f;
            AttackCheckTimeB = 0.49f;
            return true;
        }

        public bool CanAttackNape(PlayerTitan titan, bool isLeftHand)
        {
            Hand = isLeftHand
                ? BodyPart.HandLeft
                : BodyPart.HandRight;
            if (titan.Body.IsDisabled(Hand)) return false;
            AttackAnimation = isLeftHand
                ? "grab_head_back_l"
                : "grab_head_back_r";
            AttackCheckTimeA = 0.45f;
            AttackCheckTimeB = 0.5f;
            return true;
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

            if (Titan.Animation[AttackAnimation].normalizedTime >= AttackCheckTimeA && Titan.Animation[AttackAnimation].normalizedTime <= AttackCheckTimeB && GrabbedTarget == null)
            {
                var hand = Hand == BodyPart.HandLeft
                    ? Titan.Body.HandLeft
                    : Titan.Body.HandRight;

                var grabTarget = checkIfHitHand(hand, Titan.Size);
                if (grabTarget != null && grabTarget.GetComponent<Hero>() != null)
                {
                    var hero = grabTarget.GetComponent<Hero>();
                    EatSet(hero);
                    GrabbedTarget = grabTarget;
                    Service.Music.SetMusicState(new Events.Args.MusicStateChangedEvent(MusicState.HumanPlayerGrabbed, 6));
                }
            }
            if (Titan.Animation[AttackAnimation].normalizedTime >= 1f)
            {
                if (GrabbedTarget != null)
                {
                    Titan.OnTargetGrabbed(GrabbedTarget, Hand == BodyPart.HandLeft);
                }
                IsFinished = true;
            }
        }

        private void EatSet(Hero grabTarget)
        {
            var isLeftHand = Hand == BodyPart.HandLeft;
            if (!grabTarget.IsGrabbed)
            {
                Titan.Grab(isLeftHand);
                if (Titan.photonView.isMine)
                {
                    Titan.photonView.RPC(nameof(MindlessTitan.Grab), PhotonTargets.Others, isLeftHand);
                    var objArray2 = new object[] { Titan.photonView.viewID, isLeftHand };
                    grabTarget.photonView.RPC(nameof(Hero.NetGrabbed), PhotonTargets.All, objArray2);
                }
                else
                {
                    grabTarget.Grabbed(Titan.gameObject, isLeftHand);
                    grabTarget.GetComponent<Animation>().Play("grabbed");
                }
            }
        }
    }
}