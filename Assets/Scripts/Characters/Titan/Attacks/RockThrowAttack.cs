using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Titans.Attacks;
using System;
using Assets.Scripts.Characters.Humans;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class RockThrowAttack : Attack<MindlessTitan>
    {
        public RockThrowAttack()
        {
            BodyParts = new[] { BodyPart.ArmRight };
            AttackAnimation = "attack_throw";
        }

        public override Type[] TargetTypes { get; } = { typeof(Human) };

        private GameObject Rock { get; set; }
        private bool UsedRock { get; set; }

        public override bool CanAttack()
        {
            if (!GameSettings.Titan.Mindless.Attacks<RockThrowSetting>().Enabled.Value) return false;
            if (IsDisabled()) return false;
            var distance = Vector3.Distance(Titan.transform.position, Titan.Target.transform.position);
            if (distance < 100 && !Titan.Animation.IsPlaying(AttackAnimation)) return false;
            return true;
        }

        public override void Execute()
        {
            if (IsFinished)
            {
                return;
            }

            if (!Titan.Animation.IsPlaying(AttackAnimation))
            {
                Titan.CrossFade(AttackAnimation, 0.1f);
                return;
            }

            if (IsDisabled())
            {
                IsFinished = true;
                return;
            }

            if (!UsedRock && (Titan.Animation[AttackAnimation].normalizedTime >= 0.11f && Titan.Animation[AttackAnimation].normalizedTime <= 1f))
            {
                UsedRock = true;
                Transform transform = Titan.Body.HandRight;
                Rock = PhotonNetwork.Instantiate("FX/rockThrow", transform.position, transform.rotation, 0);
                Rock.transform.localScale = Titan.transform.localScale;
                Transform transform1 = Rock.transform;
                transform1.position -= (Vector3) ((Rock.transform.forward * 2.5f) * Titan.Size);
                if (Rock.GetComponent<EnemyfxIDcontainer>() != null)
                {
                    Rock.GetComponent<EnemyfxIDcontainer>().titanName = Titan.name;
                }
                Rock.transform.parent = transform;
                if (Titan.photonView.isMine)
                {
                    object[] objArray7 = new object[] { Titan.photonView.viewID, Titan.transform.localScale, Rock.transform.localPosition, Titan.Size };
                    Rock.GetPhotonView().RPC(nameof(RockThrow.initRPC), PhotonTargets.Others, objArray7);
                }
            }
            if (Titan.Animation[AttackAnimation].normalizedTime >= 0.11f && Titan.Animation[AttackAnimation].normalizedTime <= 1f)
            {
                float y = Mathf.Atan2(Titan.Target.transform.position.x - Titan.transform.position.x, Titan.Target.transform.position.z - Titan.transform.position.z) * Mathf.Rad2Deg;
                Titan.gameObject.transform.rotation = Quaternion.Euler(0f, y, 0f);
            }
            if ((Rock != null) && ((Titan.Animation[AttackAnimation].normalizedTime >= 0.62f && Titan.Animation[AttackAnimation].normalizedTime <= 1f)))
            {
                Vector3 vector6;
                float num3 = 1f;
                float num4 = -20f;
                if (Titan.Target != null)
                {
                    vector6 = (Titan.Target.transform.position - Rock.transform.position) / num3 + Titan.Target.GetComponent<Rigidbody>().velocity;
                    float num5 = Titan.Target.transform.position.y + (2f * Titan.Size);
                    float num6 = num5 - Rock.transform.position.y;
                    vector6 = new Vector3(vector6.x, (num6 / num3) - ((0.5f * num4) * num3), vector6.z);
                }
                else
                {
                    vector6 = (Titan.transform.forward * 60f) + (Vector3.up * 10f);
                }
                Rock.GetComponent<RockThrow>().launch(vector6);
                Rock.transform.parent = null;
                Rock = null;
            }

            if (Titan.Animation[AttackAnimation].normalizedTime >= 1f)
            {
                IsFinished = true;
                UsedRock = false;
            }
        }
    }
}
