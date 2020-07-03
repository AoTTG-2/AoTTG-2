using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class JumpAttack : Attack
    {
        public JumpAttack(bool isCrawler = false)
        {
            BodyParts = new []{BodyPart.LegLeft, BodyPart.LegRight};
            if (isCrawler)
            {
                this.isCrawler = true;
                AnimationJump = "attack_crawler_jump_0";
                AnimationFall = "attack_crawler_jump_1";
                AnimationLand = "attack_crawler_jump_2";
            }
        }

        private readonly bool isCrawler;
        private string AttackAnimation { get; set; }
        private string AnimationJump { get; set; } = "attack_jumper_0";
        private string AnimationFall { get; set; } = "attack_jumper_1";
        private string AnimationLand { get; set; } = "attack_jumper_2";
        private const float Gravity = 120f;
        private Vector3 JumpPosition { get; set; }
        private bool AddJumpForce { get; set; }

        public override bool CanAttack(MindlessTitan titan)
        {
            if (IsDisabled(titan)) return false;

            if (isCrawler)
            {
                Vector3 vector14 = titan.Target.transform.position - titan.transform.position;
                float current = -Mathf.Atan2(vector14.z, vector14.x) * 57.29578f;
                float f = -Mathf.DeltaAngle(current, titan.gameObject.transform.rotation.eulerAngles.y - 90f);
                if (titan.TargetDistance < titan.AttackDistance * 3f 
                    && Mathf.Abs(f) < 90f 
                    && titan.Target.transform.position.y < titan.TitanBody.Neck.position.y + 30f * titan.Size 
                    && titan.Target.transform.position.y > titan.TitanBody.Neck.position.y + 10f * titan.Size)
                {
                    AttackAnimation = AnimationJump;
                    AddJumpForce = false;
                    return true;
                }
            }
            else
            {
                if (titan.TargetDistance > titan.AttackDistance
                    && titan.Target.transform.position.y > titan.TitanBody.Head.position.y + 4f * titan.Size
                    && Vector3.Distance(titan.transform.position, titan.Target.transform.position) < 1.5f * titan.Target.transform.position.y)
                {
                    AttackAnimation = AnimationJump;
                    AddJumpForce = false;
                    return true;
                }
            }
            return false;
        }
        
        public override void Execute(MindlessTitan titan)
        {
            if (IsFinished) return;
            if (!titan.Animation.IsPlaying(AttackAnimation))
            {
                titan.CrossFade(AttackAnimation, 0.1f);
                return;
            }

            if (AttackAnimation == AnimationJump)
            {
                if (!AddJumpForce)
                {
                    if (titan.Animation[AttackAnimation].normalizedTime >= 0.68f)
                    {
                        AddJumpForce = true;

                        if (titan.Target != null)
                        {
                            float num18;
                            float num9 = titan.Target.GetComponent<Rigidbody>().velocity.y;
                            float num10 = -20f;
                            float num12 = titan.TitanBody.Neck.position.y;
                            float num13 = (num10 - Gravity) * 0.5f;
                            float num14 = num9;
                            float num15 = titan.Target.transform.position.y - num12;
                            float num16 = Mathf.Abs((float)((Mathf.Sqrt((num14 * num14) - ((4f * num13) * num15)) - num14) / (2f * num13)));
                            Vector3 vector8 = (Vector3)((titan.Target.transform.position + (titan.Target.GetComponent<Rigidbody>().velocity * num16)) + ((((Vector3.up * 0.5f) * num10) * num16) * num16));
                            float num17 = vector8.y;
                            if ((num15 < 0f) || ((num17 - num12) < 0f))
                            {
                                num18 = 60f;
                                float num19 = titan.Speed * 2.5f;
                                num19 = Mathf.Min(num19, 100f);
                                Vector3 vector9 = (Vector3)((titan.transform.forward * num19) + (Vector3.up * num18));
                                titan.Rigidbody.velocity = vector9;
                                return;
                            }
                            float num20 = num17 - num12;
                            float num21 = Mathf.Sqrt((2f * num20) / Gravity);
                            num18 = Gravity * num21;
                            num18 = Mathf.Max(30f, num18);
                            Vector3 vector10 = (Vector3)((vector8 - titan.transform.position) / num16);
                            JumpPosition = new Vector3(vector10.x, 0f, vector10.z);
                            Vector3 velocity = titan.Rigidbody.velocity;
                            Vector3 force = new Vector3(JumpPosition.x, velocity.y, JumpPosition.z) - velocity;
                            titan.Rigidbody.AddForce(force, ForceMode.VelocityChange);
                            titan.Rigidbody.AddForce((Vector3)(Vector3.up * num18), ForceMode.VelocityChange);
                            float num22 = Vector2.Angle(new Vector2(titan.transform.position.x, titan.transform.position.z), new Vector2(titan.Target.transform.position.x, titan.Target.transform.position.z));
                            num22 = Mathf.Atan2(titan.Target.transform.position.x - titan.transform.position.x, titan.Target.transform.position.z - titan.transform.position.z) * 57.29578f;
                            titan.gameObject.transform.rotation = Quaternion.Euler(0f, num22, 0f);
                        }
                    }
                    else
                    {
                        titan.Rigidbody.velocity = Vector3.zero;
                    }
                }

                if (titan.Animation[AttackAnimation].normalizedTime >= 1f)
                {
                    var hero = GetPlayerHitHead(titan.TitanBody.Head, 3f, titan.Size);
                    if (hero != null)
                    {
                        Vector3 vector13 = titan.TitanBody.Chest.position;
                        /*if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                        {
                            hero.die((hero.transform.position - vector13) * 15f * titan.Size, false);
                        }*/
                        if (titan.photonView.isMine || !hero.HasDied())
                        {
                            hero.markDie();
                            object[] objArray8 = { (hero.transform.position - vector13) * 15f * titan.Size, true, titan.photonView.viewID, titan.name, true };
                            hero.photonView.RPC("netDie", PhotonTargets.All, objArray8);
                        }

                        AttackAnimation = AnimationFall;
                        titan.CrossFade(AttackAnimation, 0.0f);
                    }

                    if (Mathf.Abs(titan.Rigidbody.velocity.y) < 0.5f || titan.Rigidbody.velocity.y < 0f || IsGrounded(titan))
                    {
                        AttackAnimation = AnimationFall;
                        titan.CrossFade(AttackAnimation, 0.0f);
                    }
                }

            }

            if (AttackAnimation == AnimationFall)
            {
                if (titan.Animation[AttackAnimation].normalizedTime >= 1f && IsGrounded(titan))
                {
                    GameObject obj11;
                    AttackAnimation = AnimationLand;
                    titan.CrossFade(AttackAnimation, 0.1f);

                    var fxPosition = titan.transform.position;
                    if (titan.photonView.isMine)
                    {
                        obj11 = PhotonNetwork.Instantiate("FX/boom2", fxPosition, Quaternion.Euler(270f, 0f, 0f), 0);
                        obj11.transform.localScale = (Vector3)(titan.transform.localScale * 1.6f);
                        //float num23 = 1f - (Vector3.Distance(this.currentCamera.transform.position, obj11.transform.position) * 0.05f);
                        //num23 = Mathf.Min(1f, num23);
                        //this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(num23, num23, 0.95f);
                    }
                }
            }

            if (AttackAnimation == AnimationLand)
            {
                if (titan.Animation[AttackAnimation].normalizedTime >= 1f)
                {
                    IsFinished = true;
                }
            }
        }

        private static Hero GetPlayerHitHead(Transform head, float rad, float titanSize)
        {
            var num = rad * titanSize;
            foreach (Hero hero in FengGameManagerMKII.instance.getPlayers())
            {
                if (hero.isInvincible()) continue;
                var num3 = hero.GetComponent<CapsuleCollider>().height * 0.5f;
                if (Vector3.Distance(hero.transform.position + Vector3.up * num3, head.position + Vector3.up * 1.5f * titanSize) < (num + num3))
                {
                    return hero;
                }
            }
            return null;
        }

        private static bool IsGrounded(MindlessTitan titan)
        {
            LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyAABB");
            LayerMask mask3 = mask2 | mask;
            return Physics.Raycast(titan.gameObject.transform.position + Vector3.up * 0.1f, -Vector3.up, 0.3f, mask3.value);
        }
    }
}

//if ((this.attackAnimation == "jumper_0") || (this.attackAnimation == "crawler_jump_0"))
//{
//    if (!this.attacked)
//    {
//        if (this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 0.68f)
//        {
//            this.attacked = true;
//            if ((this.myHero == null) || this.nonAI)
//            {
//                float num7 = 120f;
//                Vector3 vector7 = (Vector3)((this.baseTransform.forward * this.speed) + (Vector3.up * num7));
//                if (this.nonAI && (this.TitanType == TitanType.TYPE_CRAWLER))
//                {
//                    num7 = 100f;
//                    float a = this.speed * 2.5f;
//                    a = Mathf.Min(a, 100f);
//                    vector7 = (Vector3)((this.baseTransform.forward * a) + (Vector3.up * num7));
//                }
//                this.baseRigidBody.velocity = vector7;
//            }