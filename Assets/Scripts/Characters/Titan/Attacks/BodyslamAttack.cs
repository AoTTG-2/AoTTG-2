using System;
using Assets.Scripts.Characters.Humans;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class BodySlamAttack : Attack<MindlessTitan>
    {
        private readonly string _startAnimation = "attack_abnormal_jump";
        private readonly string _endAnimation = "attack_abnormal_getup";
        private bool HasExploded { get; set; }
        private float WaitTime { get; set; }

        public override Type[] TargetTypes { get; } = { typeof(Human) };


        public override bool CanAttack()
        {
            if (Titan.TargetDistance >= Titan.AttackDistance * 2) return false;
            if (IsDisabled()) return false;
            Vector3 vector18 = Titan.Target.transform.position - Titan.transform.position;
            var angle = -Mathf.Atan2(vector18.z, vector18.x) * Mathf.Rad2Deg;
            var between = -Mathf.DeltaAngle(angle, Titan.gameObject.transform.rotation.eulerAngles.y - 90f);
            return Mathf.Abs(between) < 90f && between > 0 &&
                   Titan.TargetDistance < Titan.AttackDistance * 0.5f;
        }

        public override void Execute()
        {
            if (IsFinished) return;
            if (!Titan.Animation.IsPlaying(_startAnimation) && !Titan.Animation.IsPlaying(_endAnimation))
            {
                Titan.CrossFade(_startAnimation, 0.1f);
                HasExploded = false;
                //(this.myDifficulty <= 0) ? UnityEngine.Random.Range((float)1f, (float)4f) : UnityEngine.Random.Range((float)0f, (float)1f);
                WaitTime = Random.Range(0f, 1f);
                return;
            }

            if (!HasExploded && Titan.Animation[_startAnimation].normalizedTime >= 0.75f)
            {
                HasExploded = true;
                GameObject obj9;
                var rotation = Quaternion.Euler(270f, Titan.transform.rotation.eulerAngles.y, 0f);
                if (Titan.photonView.isMine)
                {
                    obj9 = PhotonNetwork.Instantiate("FX/boom4", Titan.Body.AttackAbnormalJump.position, rotation, 0);
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

            if (Titan.Animation[_startAnimation].normalizedTime >= 1f + WaitTime)
            {
                Titan.Animation.CrossFade(_endAnimation, 0.1f);
            }

            if (Titan.Animation[_endAnimation].normalizedTime >= 1f)
            {
                IsFinished = true;
            }
        }
    }
}
