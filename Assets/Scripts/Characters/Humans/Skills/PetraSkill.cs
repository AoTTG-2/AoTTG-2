using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Skills
{
    public class PetraSkill : Skill
    {
        private const float CooldownLimit = 3.5f;
        private bool UsePhysics { get; set; }
        private const string SkillAnimation = "special_petra";

        public PetraSkill(Hero hero) : base(hero)
        {
            Cooldown = CooldownLimit;
        }

        public override bool Use()
        {
            if (Hero.HeroState != HERO_STATE.Idle) return false;

            RaycastHit hit;
            Hero.attackAnimation = "special_petra";
            Hero.PlayAnimation("special_petra");
            Hero.Rigidbody.velocity += (Vector3) (Vector3.up * 5f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = mask2 | mask;
            if (Physics.Raycast(ray, out hit, 1E+07f, mask3.value))
            {
                if (Hero.bulletRight != null)
                {
                    Hero.bulletRight.GetComponent<Bullet>().disable();
                    Hero.ReleaseIfIHookSb();
                }
                if (Hero.bulletLeft != null)
                {
                    Hero.bulletLeft.GetComponent<Bullet>().disable();
                    Hero.ReleaseIfIHookSb();
                }
                Hero.dashDirection = hit.point - Hero.transform.position;
                Hero.LaunchLeftRope(hit.distance, hit.point, true);
                Hero.LaunchRightRope(hit.distance, hit.point, true);
                Hero.audioSystem.rope.Play();
            }
            Hero.facingDirection = Mathf.Atan2(Hero.dashDirection.x, Hero.dashDirection.z) * 57.29578f;
            Hero.targetRotation = Quaternion.Euler(0f, Hero.facingDirection, 0f);
            Hero.attackLoop = 3;
            IsActive = true;
            UsePhysics = true;
            return true;
        }

        public override void OnUpdate()
        {
            if (Hero.Animation.IsPlaying("special_petra")) return;
            IsActive = false;
        }



        public override void OnFixedUpdate()
        {
            if (UsePhysics)
            {
                AddUseForce();
            }

            if (Hero.grounded && Hero.HeroState == HERO_STATE.Attack)
            {
                if (Hero.Animation[SkillAnimation].normalizedTime > 0.35f &&
                    Hero.Animation[SkillAnimation].normalizedSpeed < 0.48f)
                {
                    Hero.Rigidbody.AddForce(Hero.gameObject.transform.forward * 200f);
                }
            }
        }

        private void AddUseForce()
        {
            if (Hero.HeroState != HERO_STATE.Attack || Hero.attackAnimation != "special_petra" ||
                Hero.Animation["special_petra"].normalizedTime <= 0.4f) return;

            if (Hero.launchPointRight.magnitude > 0f)
            {
                Vector3 vector19 = Hero.launchPointRight - Hero.transform.position;
                vector19.Normalize();
                vector19 = (Vector3) (vector19 * 13f);
                Hero.Rigidbody.AddForce(vector19, ForceMode.Impulse);
            }

            if (Hero.launchPointLeft.magnitude > 0f)
            {
                Vector3 vector20 = Hero.launchPointLeft - Hero.transform.position;
                vector20.Normalize();
                vector20 = (Vector3) (vector20 * 13f);
                Hero.Rigidbody.AddForce(vector20, ForceMode.Impulse);
                if (Hero.bulletRight != null)
                {
                    Hero.bulletRight.GetComponent<Bullet>().disable();
                    Hero.ReleaseIfIHookSb();
                }
                if (Hero.bulletLeft != null)
                {
                    Hero.bulletLeft.GetComponent<Bullet>().disable();
                    Hero.ReleaseIfIHookSb();
                }
            }

            Hero.Rigidbody.AddForce(Vector3.up * 2f, ForceMode.Impulse);
            UsePhysics = false;
        }
    }
}
