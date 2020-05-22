using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class BodySlamAttack : Attack
    {
        private readonly string _startAnimation = "attack_abnormal_jump";
        private readonly string _endAnimation = "attack_abnormal_getup";
        private bool HasExploded { get; set; }
        private float WaitTime { get; set; }

        public override bool CanAttack(MindlessTitan titan)
        {
            if (titan.TargetDistance >= titan.AttackDistance) return false;
            if (IsDisabled(titan)) return false;
            Vector3 vector18 = titan.Target.transform.position - titan.transform.position;
            var angle = -Mathf.Atan2(vector18.z, vector18.x) * 57.29578f;
            var between = -Mathf.DeltaAngle(angle, titan.gameObject.transform.rotation.eulerAngles.y - 90f);
            return Mathf.Abs(between) < 90f && between > 0 &&
                   titan.TargetDistance < titan.AttackDistance * 0.5f;
        }

        public override void Execute(MindlessTitan titan)
        {
            if (IsFinished) return;
            if (!titan.Animation.IsPlaying(_startAnimation) && !titan.Animation.IsPlaying(_endAnimation))
            {
                titan.CrossFade(_startAnimation, 0.1f);
                HasExploded = false;
                //(this.myDifficulty <= 0) ? UnityEngine.Random.Range((float)1f, (float)4f) : UnityEngine.Random.Range((float)0f, (float)1f);
                WaitTime = Random.Range(0f, 1f);
                return;
            }

            if (!HasExploded && titan.Animation[_startAnimation].normalizedTime >= 0.75f)
            {
                HasExploded = true;
                GameObject obj9;
                var rotation = Quaternion.Euler(270f, titan.transform.rotation.eulerAngles.y, 0f);
                if (titan.photonView.isMine)
                {
                    obj9 = PhotonNetwork.Instantiate("FX/boom4", titan.TitanBody.AttackAbnormalJump.position, rotation, 0);
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

            if (titan.Animation[_startAnimation].normalizedTime >= 1f + WaitTime)
            {
                titan.Animation.CrossFade(_endAnimation, 0.1f);
            }

            if (titan.Animation[_endAnimation].normalizedTime >= 1f)
            {
                IsFinished = true;
            }
        }
    }
}
