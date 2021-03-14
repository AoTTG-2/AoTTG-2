using System;
using Assets.Scripts.Characters.Humans;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class ComboAttack : Attack<MindlessTitan>
    {
        public ComboAttack(bool isPunk = false)
        {
            BodyParts = new[] { BodyPart.HandRight, BodyPart.HandLeft };
            Damage = 100;
            this.isPunk = isPunk;
        }

        public override Type[] TargetTypes { get; } = { typeof(Human), typeof(TitanBase) };

        private readonly bool isPunk;
        private const string AnimationPunchRight = "attack_combo_1";
        private const string AnimationPunchLeft  = "attack_combo_2";
        private const string AnimationSlam = "attack_combo_3";
        private const string BoomEffect = "FX/boom1";

        private float attackCheckTimeA;
        private float attackCheckTimeB;

        private BodyPart Hand { get; set; }
        private bool HasExploded { get; set; }

        public override bool CanAttack()
        {
            if (!base.CanAttack()) return false;

            if (Titan.TargetDistance >= Titan.AttackDistance * 2) return false;
            Vector3 vector18 = Titan.Target.transform.position - Titan.transform.position;
            var angle = -Mathf.Atan2(vector18.z, vector18.x) * Mathf.Rad2Deg;
            var between = -Mathf.DeltaAngle(angle, Titan.gameObject.transform.rotation.eulerAngles.y - 90f);
            if (Mathf.Abs(between) < 90f)
            {
                if (Titan.TargetDistance * 0.75f < Titan.AttackDistance * 0.75f)
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
                Vector3 vector12 = Titan.transform.forward * Titan.Speed * 0.40f;
                Vector3 vector14 = vector12 - Titan.Rigidbody.velocity;
                vector14.x = Mathf.Clamp(vector14.x, -10f, 10f);
                vector14.z = Mathf.Clamp(vector14.z, -10f, 10f);
                vector14.y = 0f;
                //Titan.Rigidbody.AddForce(vector14, ForceMode.VelocityChange);
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

            if (AttackAnimation == AnimationSlam)
            {
                if (!HasExploded && Titan.Animation[AttackAnimation].normalizedTime >= 0.21f)
                {
                    HasExploded = true;
                    GameObject obj9;
                    var rotation = Quaternion.Euler(270f, 0f, 0f);
                    if (Titan.photonView.isMine)
                    {
                        obj9 = PhotonNetwork.Instantiate(BoomEffect, Titan.Body.AttackFrontGround.position, rotation, 0);
                    }
                    else
                    {
                        return;
                    }
                    obj9.transform.localScale = Titan.transform.localScale;
                    if (obj9.GetComponent<EnemyfxIDcontainer>() != null)
                    {
                        obj9.GetComponent<EnemyfxIDcontainer>().titanName = Titan.name;
                    }
                }
            }
        }
    }
}
