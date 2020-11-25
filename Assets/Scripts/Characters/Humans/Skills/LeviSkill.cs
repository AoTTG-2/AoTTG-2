using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Skills
{
    public class LeviSkill : Skill
    {
        private const float CooldownLimit = 3.5f;
        private bool UsePhysics { get; set; }

        public LeviSkill(Hero hero) : base(hero)
        {
            Cooldown = CooldownLimit;
        }

        public override bool Use()
        {
            if (Hero._state != HERO_STATE.Idle) return false;

            RaycastHit hit;
            Hero.attackAnimation = "attack5";
            Hero.playAnimation("attack5");
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
                    Hero.releaseIfIHookSb();
                }
                Hero.dashDirection = hit.point - Hero.transform.position;
                Hero.LaunchRightRope(hit.distance, hit.point, true, 1);
                Hero.rope.Play();
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
            if (Hero.Animation.IsPlaying("attack5")) return;
            IsActive = false;
        }



        public override void OnFixedUpdate()
        {
            if (!UsePhysics) return;

            if (Hero._state != HERO_STATE.Attack || Hero.attackAnimation != "attack5" ||
                Hero.Animation["attack5"].normalizedTime <= 0.4f) return;

            if (Hero.launchPointRight.magnitude > 0f)
            {
                Vector3 vector19 = Hero.launchPointRight - Hero.transform.position;
                vector19.Normalize();
                vector19 = (Vector3) (vector19 * 13f);
                Hero.Rigidbody.AddForce(vector19, ForceMode.Impulse);
            }
            Hero.Rigidbody.AddForce(Vector3.up * 2f, ForceMode.Impulse);
            UsePhysics = false;
        }
    }
}
