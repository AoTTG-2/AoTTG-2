using System;
using Assets.Scripts.Characters.Humans;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public abstract class BoomAttack : Attack<MindlessTitan>
    {
        protected bool HasExploded { get; set; }
        protected virtual float BoomTimer { get; set; }
        protected virtual string Effect { get; set; }
        protected virtual Transform TitanBodyPart { get; set; }

        public override Type[] TargetTypes { get; } = { typeof(Human) };

        public override void Execute()
        {
            ExecuteBoomAttack();
        }
        
        private void ExecuteBoomAttack()
        {
            if (IsFinished) return;
            if (!Titan.Animation.IsPlaying(AttackAnimation))
            {
                Titan.CrossFade(AttackAnimation, 0.1f);
                HasExploded = false;
                return;
            }

            if (IsDisabled())
            {
                IsFinished = true;
                return;
            }

            if (!HasExploded && Titan.Animation[AttackAnimation].normalizedTime >= BoomTimer)
            {
                HasExploded = true;
                GameObject obj9;
                var rotation = Quaternion.Euler(270f, 0f, 0f);
                if (Titan.photonView.isMine)
                {
                    obj9 = PhotonNetwork.Instantiate(Effect, TitanBodyPart.position, rotation, 0);
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

            if (Titan.Animation[AttackAnimation].normalizedTime >= 1f)
            {
                IsFinished = true;
            }
        }
    }
}
