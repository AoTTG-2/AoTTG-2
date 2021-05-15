using System;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class JumpAttack : Attack<MindlessTitan>
    {
        public override Type[] TargetTypes { get; } = { typeof(Human) };

        private const float Gravity = 120f;

        private readonly bool isCrawler;

        public JumpAttack(bool isCrawler = false)
        {
            BodyParts = new[] { BodyPart.LegLeft, BodyPart.LegRight };
            if (isCrawler)
            {
                this.isCrawler = true;
                AnimationJump = "attack_crawler_jump_0";
                AnimationFall = "attack_crawler_jump_1";
                AnimationLand = "attack_crawler_jump_2";
            }
        }

        private bool AddJumpForce { get; set; }

        private string AnimationFall { get; set; } = "attack_jumper_1";

        private string AnimationJump { get; set; } = "attack_jumper_0";

        private string AnimationLand { get; set; } = "attack_jumper_2";

        private Vector3 JumpPosition { get; set; }

        public override bool CanAttack()
        {
            if (IsDisabled()) return false;

            if (isCrawler)
            {
                var vector14 = Titan.Target.transform.position - Titan.transform.position;
                var current = -Mathf.Atan2(vector14.z, vector14.x) * Mathf.Rad2Deg;
                var f = -Mathf.DeltaAngle(current, Titan.gameObject.transform.rotation.eulerAngles.y - 90f);
                if (Titan.TargetDistance < Titan.AttackDistance * 3f
                    && Mathf.Abs(f) < 90f
                    && Titan.Target.transform.position.y < Titan.Body.Neck.position.y + 30f * Titan.Size
                    && Titan.Target.transform.position.y > Titan.Body.Neck.position.y + 10f * Titan.Size)
                {
                    AttackAnimation = AnimationJump;
                    AddJumpForce = false;
                    return true;
                }
            }
            else
            {
                if (Titan.TargetDistance > Titan.AttackDistance
                    && Titan.Target.transform.position.y > Titan.Body.Head.position.y + 4f * Titan.Size
                    && Vector3.Distance(Titan.transform.position, Titan.Target.transform.position) < 1.5f * Titan.Target.transform.position.y)
                {
                    AttackAnimation = AnimationJump;
                    AddJumpForce = false;
                    return true;
                }
            }
            return false;
        }

        public override void Execute()
        {
            if (IsFinished) return;
            if (!Titan.Animation.IsPlaying(AttackAnimation))
            {
                Titan.CrossFade(AttackAnimation, 0.1f);
                return;
            }

            if (AttackAnimation == AnimationJump)
                ExecuteJump();

            if (AttackAnimation == AnimationFall)
                ExecuteFall();

            if (AttackAnimation == AnimationLand)
                ExecuteLand();
        }

        private static Hero GetPlayerHitHead(Transform head, float rad, float titanSize)
        {
            var num = rad * titanSize;
            foreach (Hero hero in Service.Entity.GetAll<Hero>())
            {
                if (hero.IsInvincible) continue;
                var num3 = hero.GetComponent<CapsuleCollider>().height * 0.5f;
                if (Vector3.Distance(hero.transform.position + Vector3.up * num3, head.position + Vector3.up * 1.5f * titanSize) < (num + num3))
                {
                    return hero;
                }
            }
            return null;
        }

        private static bool IsGrounded(MindlessTitan Titan)
        {
            LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyAABB");
            LayerMask mask3 = mask2 | mask;
            return Physics.Raycast(Titan.gameObject.transform.position + Vector3.up * 0.1f, -Vector3.up, 0.3f, mask3.value);
        }

        private void ExecuteFall()
        {
            if (Titan.Animation[AttackAnimation].normalizedTime >= 1f && IsGrounded(Titan))
            {
                GameObject obj11;
                AttackAnimation = AnimationLand;
                Titan.CrossFade(AttackAnimation, 0.1f);

                var fxPosition = Titan.transform.position;
                if (Titan.photonView.isMine)
                {
                    obj11 = PhotonNetwork.Instantiate("FX/boom2", fxPosition, Quaternion.Euler(270f, 0f, 0f), 0);
                    obj11.transform.localScale = Titan.transform.localScale * 1.6f;

                    //float num23 = 1f - (Vector3.Distance(this.currentCamera.transform.position, obj11.transform.position) * 0.05f);
                    //num23 = Mathf.Min(1f, num23);
                    //this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(num23, num23, 0.95f);
                }
            }
        }

        private void ExecuteJump()
        {
            if (!AddJumpForce)
            {
                if (Titan.Animation[AttackAnimation].normalizedTime >= 0.68f)
                {
                    AddJumpForce = true;

                    if (Titan.Target != null)
                    {
                        float num18;
                        var yVel = Titan.Target.GetComponent<Rigidbody>().velocity.y;
                        var num10 = -20f;
                        var num12 = Titan.Body.Neck.position.y;
                        var num13 = (num10 - Gravity) * 0.5f;
                        var num15 = Titan.Target.transform.position.y - num12;
                        var num16 = Mathf.Abs((Mathf.Sqrt((yVel * yVel) - ((4f * num13) * num15)) - yVel) / (2f * num13));
                        var vector8 = (Titan.Target.transform.position + (Titan.Target.GetComponent<Rigidbody>().velocity * num16)) + Vector3.up * 0.5f * num10 * num16 * num16;
                        var num17 = vector8.y;
                        if ((num15 < 0f) || ((num17 - num12) < 0f))
                        {
                            num18 = 60f;
                            var num19 = Titan.Speed * 2.5f;
                            num19 = Mathf.Min(num19, 100f);
                            var vector9 = Titan.transform.forward * num19 + Vector3.up * num18;
                            Titan.Rigidbody.velocity = vector9;
                            return;
                        }
                        var num20 = num17 - num12;
                        var num21 = Mathf.Sqrt((2f * num20) / Gravity);
                        num18 = Gravity * num21;
                        num18 = Mathf.Max(30f, num18);
                        var vector10 = (vector8 - Titan.transform.position) / num16;
                        JumpPosition = new Vector3(vector10.x, 0f, vector10.z);
                        var velocity = Titan.Rigidbody.velocity;
                        var force = new Vector3(JumpPosition.x, velocity.y, JumpPosition.z) - velocity;
                        Titan.Rigidbody.AddForce(force, ForceMode.VelocityChange);
                        Titan.Rigidbody.AddForce(Vector3.up * num18, ForceMode.VelocityChange);
                        var num22 = Mathf.Atan2(Titan.Target.transform.position.x - Titan.transform.position.x, Titan.Target.transform.position.z - Titan.transform.position.z) * Mathf.Rad2Deg;
                        Titan.gameObject.transform.rotation = Quaternion.Euler(0f, num22, 0f);
                    }
                }
                else
                {
                    Titan.Rigidbody.velocity = Vector3.zero;
                }
            }

            if (Titan.Animation[AttackAnimation].normalizedTime >= 1f)
            {
                var hero = GetPlayerHitHead(Titan.Body.Head, 3f, Titan.Size);
                if (hero != null)
                {
                    var vector13 = Titan.Body.Chest.position;
                    if (Titan.photonView.isMine || !hero.HasDied())
                    {
                        hero.MarkDie();
                        object[] objArray8 = { (hero.transform.position - vector13) * 15f * Titan.Size, true, Titan.photonView.viewID, Titan.name, true };
                        hero.photonView.RPC(nameof(Hero.NetDie), PhotonTargets.All, objArray8);
                    }

                    AttackAnimation = AnimationFall;
                    Titan.CrossFade(AttackAnimation, 0.0f);
                }

                if (Mathf.Abs(Titan.Rigidbody.velocity.y) < 0.5f || Titan.Rigidbody.velocity.y < 0f || IsGrounded(Titan))
                {
                    AttackAnimation = AnimationFall;
                    Titan.CrossFade(AttackAnimation, 0.0f);
                }
            }
        }

        private void ExecuteLand()
        {
            if (Titan.Animation[AttackAnimation].normalizedTime >= 1f)
            {
                IsFinished = true;
            }
        }
    }
}