using Assets.Scripts.Characters.Humans.Constants;
using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Skills
{
    public class BladeThrowSkill : Skill
    {
        private const float CooldownLimit = 3.5f;
        private const float BladeSpeed = 5f;
        private bool UsePhysics { get; set; }

        public BladeThrowSkill(Hero hero) : base(hero)
        {
            Cooldown = CooldownLimit;
        }

        public override bool Use()
        {
            if (Hero._state != HumanState.Idle) return false;

            Hero.attackAnimation = HeroAnim.ATTACK5;
            Hero.PlayAnimation(HeroAnim.ATTACK5);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 velocity = Vector3.Normalize(ray.direction) * BladeSpeed;
            LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer();
            Hero.facingDirection = Mathf.Atan2(Hero.dashDirection.x, Hero.dashDirection.z) * Mathf.Rad2Deg;
            Hero.targetRotation = Quaternion.Euler(0f, Hero.facingDirection, 0f);
            Hero.attackLoop = 3;
            IsActive = true;
            UsePhysics = true;
            return true;
        }

        public override void OnUpdate()
        {
            if (Hero.Animation.IsPlaying(HeroAnim.ATTACK5)) return;
            IsActive = false;
        }

        public override void OnFixedUpdate()
        {
            if (!UsePhysics) return;

            if (Hero._state != HumanState.Attack || Hero.attackAnimation != HeroAnim.ATTACK5 ||
                Hero.Animation[HeroAnim.ATTACK5].normalizedTime <= 0.4f) return;

            UsePhysics = false;
        }
    }
}
