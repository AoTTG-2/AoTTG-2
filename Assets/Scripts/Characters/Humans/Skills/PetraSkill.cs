using Assets.Scripts.Characters.Humans.Constants;
using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Skills
{
    public class PetraSkill : Skill
    {
        private const float CooldownLimit = 3.5f;
        private bool UsePhysics { get; set; }

        public PetraSkill(Hero hero) : base(hero)
        {
            Cooldown = CooldownLimit;
        }

        public override bool Use()
        {
            if (Hero._state != HumanState.Idle) return false;

            RaycastHit hit;
            Hero.attackAnimation = HeroAnim.SPECIAL_PETRA;
            Hero.PlayAnimation(HeroAnim.SPECIAL_PETRA);
            Hero.Rigidbody.velocity += Vector3.up * 5f;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer();
            if (Physics.Raycast(ray, out hit, float.MaxValue, mask.value))
            {
                if (Hero.hookRight != null)
                {
                    Hero.hookRight.disable();
                    Hero.ReleaseIfIHookSb();
                }
                if (Hero.hookLeft != null)
                {
                    Hero.hookLeft.disable();
                    Hero.ReleaseIfIHookSb();
                }
                Hero.dashDirection = hit.point - Hero.transform.position;
                Hero.LaunchLeftRope(hit.distance, hit.point, true);
                Hero.LaunchRightRope(hit.distance, hit.point, true);
                Hero.rope.Play();
            }
            Hero.facingDirection = Mathf.Atan2(Hero.dashDirection.x, Hero.dashDirection.z) * Mathf.Rad2Deg;
            Hero.targetRotation = Quaternion.Euler(0f, Hero.facingDirection, 0f);
            Hero.attackLoop = 3;
            IsActive = true;
            UsePhysics = true;
            return true;
        }

        public override void OnUpdate()
        {
            if (Hero.Animation.IsPlaying(HeroAnim.SPECIAL_PETRA)) return;
            IsActive = false;
        }



        public override void OnFixedUpdate()
        {
            if (UsePhysics)
            {
                AddUseForce();
            }

            if (Hero.grounded && Hero._state == HumanState.Attack)
            {
                if (Hero.Animation[HeroAnim.SPECIAL_PETRA].normalizedTime > 0.35f &&
                    Hero.Animation[HeroAnim.SPECIAL_PETRA].normalizedSpeed < 0.48f)
                {
                    Hero.Rigidbody.AddForce(Hero.gameObject.transform.forward * 200f);
                }
            }
        }

        private void AddUseForce()
        {
            if (Hero._state != HumanState.Attack || Hero.attackAnimation != HeroAnim.SPECIAL_PETRA ||
                Hero.Animation[HeroAnim.SPECIAL_PETRA].normalizedTime <= 0.4f) return;

            if (Hero.launchPointRight.magnitude > 0f)
            {
                Vector3 vector19 = Hero.launchPointRight - Hero.transform.position;
                vector19.Normalize();
                Hero.Rigidbody.AddForce(vector19 * 13f, ForceMode.Impulse);
            }

            if (Hero.launchPointLeft.magnitude > 0f)
            {
                Vector3 vector20 = Hero.launchPointLeft - Hero.transform.position;
                vector20.Normalize();
                Hero.Rigidbody.AddForce(vector20 * 13f, ForceMode.Impulse);
                if (Hero.hookRight != null)
                {
                    Hero.hookRight.disable();
                    Hero.ReleaseIfIHookSb();
                }
                if (Hero.hookLeft != null)
                {
                    Hero.hookLeft.disable();
                    Hero.ReleaseIfIHookSb();
                }
            }

            Hero.Rigidbody.AddForce(Vector3.up * 2f, ForceMode.Impulse);
            UsePhysics = false;
        }
    }
}
