using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public abstract class BoomAttack : Attack
    {
        protected bool HasExploded { get; set; }
        protected virtual string AttackAnimation { get; set; }
        protected virtual float BoomTimer { get; set; }
        protected virtual string Effect { get; set; }
        protected virtual Transform TitanBodyPart { get; set; }

        public override void Execute(MindlessTitan titan)
        {
            ExecuteBoomAttack(titan);
        }

        private void ExecuteBoomAttack(MindlessTitan titan)
        {
            if (IsFinished) return;
            if (!titan.Animation.IsPlaying(AttackAnimation))
            {
                titan.Animation.CrossFade(AttackAnimation, 0.1f);
                HasExploded = false;
                return;
            }

            if (!HasExploded && titan.Animation[AttackAnimation].normalizedTime >= BoomTimer)
            {
                HasExploded = true;
                GameObject obj9;
                var rotation = Quaternion.Euler(270f, 0f, 0f);
                if (titan.photonView.isMine)
                {
                    obj9 = PhotonNetwork.Instantiate(Effect, TitanBodyPart.position, rotation, 0);
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

            if (titan.Animation[AttackAnimation].normalizedTime >= 1f)
            {
                IsFinished = true;
            }
        }
    }
}
