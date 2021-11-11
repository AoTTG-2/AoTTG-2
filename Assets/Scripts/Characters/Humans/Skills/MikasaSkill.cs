using Assets.Scripts.Characters.Humans.Constants;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Skills
{
    public class MikasaSkill : Skill
    {
        public MikasaSkill(Hero hero) : base(hero)
        {
        }

        public override bool Use()
        {
            if (Hero._state != HumanState.Idle) return false;

            Hero.attackAnimation = HeroAnim.ATTACK3_1;
            Hero.PlayAnimation(HeroAnim.ATTACK3_1);
            Hero.Rigidbody.velocity = Vector3.up * 10f;
            IsActive = true;
            return true;
        }

        public override void OnUpdate()
        {
            // Ignored
        }

        public override void OnFixedUpdate()
        {

            // Used to determine if the hero state changes at any time during the skill. If so then the skill is no longer active
            if (IsActive && Hero._state != HumanState.Attack)
            {
                IsActive = false;
            }

            if (!Hero.grounded) return;

            if (Hero._state == HumanState.Attack && Hero.attackAnimation == HeroAnim.ATTACK3_1 &&
                Hero.Animation[Hero.attackAnimation].normalizedTime >= 1f)
            {
                Hero.PlayAnimation(HeroAnim.ATTACK3_2);
                Hero.ResetAnimationSpeed();
                Hero.Rigidbody.velocity = Vector3.zero;
                Hero.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().StartShake(0.2f, 0.3f, 0.95f);
                IsActive = false;
            }

            
        }
    }
}
