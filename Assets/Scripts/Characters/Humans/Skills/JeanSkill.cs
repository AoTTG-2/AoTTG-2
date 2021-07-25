using Assets.Scripts.Characters.Humans.Constants;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Skills
{
    public class JeanSkill : Skill
    {
        public JeanSkill(Hero hero) : base(hero)
        {
        }

        public int TimesUsed { get; set; }

        private const int TimesAllowed = 1;
        
        public override bool Use()
        {
            if (Hero._state != HumanState.Grab || IsActive) return false;

            if (TimesUsed < TimesAllowed && !Hero.Animation.IsPlaying(HeroAnim.GRABBED_JEAN))
            {
                Hero.PlayAnimation(HeroAnim.GRABBED_JEAN);
                TimesUsed++;
                IsActive = true;
                return true;
            }

            return false;
        }

        public override void OnUpdate()
        {
            if (Hero.Animation.IsPlaying(HeroAnim.GRABBED_JEAN) && Hero.Animation[HeroAnim.GRABBED_JEAN].normalizedTime > 0.64f)
            {
                Hero.Ungrabbed();
                Hero.Rigidbody.velocity = Vector3.up * 30f;
                IsActive = false;
            }
            else
            {
                IsActive = true;
            }

        }
    }
}
