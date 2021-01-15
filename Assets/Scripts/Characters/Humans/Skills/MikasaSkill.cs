using System;
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
            if (Hero.HeroState != HERO_STATE.Idle) return false;

            Hero.attackAnimation = "attack3_1";
            Hero.PlayAnimation("attack3_1");
            Hero.Rigidbody.velocity = Vector3.up * 10f;
            IsActive = true;
            return true;
        }

        public override void OnUpdate()
        {
            //throw new NotImplementedException();
        }

        public override void OnFixedUpdate()
        {
            if (!Hero.grounded) return;

            if (Hero.HeroState == HERO_STATE.Attack && Hero.attackAnimation == "attack3_1" &&
                Hero.Animation[Hero.attackAnimation].normalizedTime >= 1f)
            {
                Hero.PlayAnimation("attack3_2");
                Hero.ResetAnimationSpeed();
                Hero.Rigidbody.velocity = Vector3.zero;
                Hero.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.2f, 0.3f, 0.95f);
                IsActive = false;
            }
        }
    }
}
