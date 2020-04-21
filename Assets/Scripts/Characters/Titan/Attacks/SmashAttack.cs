using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class SmashAttack : Attack
    {
        private string attackAnimation = "attack_combo_3";
        private bool hasExploded;

        public override bool CanAttack(MindlessTitan titan)
        {
            //var distance = Vector3.Distance(titan.transform.position, titan.Target.transform.position);
            float angle = Vector3.Angle(titan.transform.forward, titan.transform.position - titan.Target.transform.position);
            if (titan.TargetDistance >= 10 * titan.Size || angle < 155 && !titan.Animation.IsPlaying(attackAnimation))
            {
                return false;
            }
            return true;
        }

        public override void Execute(MindlessTitan titan)
        {
            if (IsFinished) return;
            if (!titan.Animation.IsPlaying(attackAnimation))
            {
                titan.Animation[attackAnimation].speed = 0.5f;
                titan.Animation.CrossFade(attackAnimation);
                hasExploded = false;
                return;
            }

            if (!hasExploded && titan.Animation[attackAnimation].normalizedTime >= 0.20f)
            {
                hasExploded = true;
                GameObject obj9;
                var fxPosition = titan.transform.Find("ap_combo_3").position;
                var rotation = Quaternion.Euler(270f, 0f, 0f);
                if (titan.photonView.isMine)
                {
                    obj9 = PhotonNetwork.Instantiate("FX/boom1", fxPosition, rotation, 0);
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

            if (titan.Animation[attackAnimation].normalizedTime >= 1f)
            {
                IsFinished = true;
                titan.Animation[attackAnimation].speed = 1f;
            }
        }
    }
}
