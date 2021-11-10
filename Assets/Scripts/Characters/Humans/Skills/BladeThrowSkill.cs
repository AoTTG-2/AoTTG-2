using Assets.Scripts.Characters.Humans.Constants;
using Assets.Scripts.Constants;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Skills
{
    public class BladeThrowSkill : Skill
    {
        private const float CooldownLimit = 3.5f;
        private const float BladeSpeed = 150f;
        private const float BladeRotationSpeed = 1000f; // In degrees
        private bool UsePhysics { get; set; }

        private Ray ray;
        private Vector3 velocity;
        private bool bladeThrown;
        private List<GameObject> leftBlades = new List<GameObject>();
        private List<GameObject> rightBlades = new List<GameObject>();

        public BladeThrowSkill(Hero hero) : base(hero)
        {
            Cooldown = CooldownLimit;
        }

        public override bool Use()
        {
            if (Hero._state != HumanState.Idle || Hero.totalBladeSta <= 0f) return false;

            Hero.attackAnimation = HeroAnim.ATTACK1;
            Hero.PlayAnimation(HeroAnim.ATTACK1);
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 facingDir = ray.direction;
            Hero.facingDirection = Mathf.Atan2(facingDir.x, facingDir.z) * Mathf.Rad2Deg;
            Hero.targetRotation = Quaternion.Euler(0f, Hero.facingDirection, 0f);
            IsActive = true;
            UsePhysics = true;
            bladeThrown = false;
            return true;
        }

        public override void OnUpdate()
        {
            if (!bladeThrown && (Hero._state != HumanState.Attack || Hero.attackAnimation != HeroAnim.ATTACK1 ||
                Hero.Animation[HeroAnim.ATTACK1].normalizedTime >= 0.4f))
            {
                ThrowBlade();
            }
            if (Hero.Animation.IsPlaying(HeroAnim.ATTACK1)) return;
            // Reload blade after throwing
            if (Hero.Equipment.Weapon.AmountLeft > 0 || Hero.Equipment.Weapon.AmountRight > 0)
            {
                Hero.ChangeBlade();
            }
            Hero.Equipment.Weapon.PlayReloadAnimation();
            IsActive = false;
        }

        public override void OnFixedUpdate()
        {
            if (!UsePhysics) return;

            if (Hero._state != HumanState.Attack || Hero.attackAnimation != HeroAnim.ATTACK1 ||
                Hero.Animation[HeroAnim.ATTACK1].normalizedTime <= 0.4f) return;

            UsePhysics = false;
        }

        public override void OnAlwaysUpdate()
        {
            ThrownBladesUpdate();
        }

        private void ThrownBladesUpdate()
        {
            List<GameObject> removing = new List<GameObject>();
            for (int i = 0; i < leftBlades.Count; i++)
            {
                leftBlades[i].GetComponent<Rigidbody>().transform.position += velocity * Time.deltaTime;
                rightBlades[i].GetComponent<Rigidbody>().transform.position += velocity * Time.deltaTime;
                leftBlades[i].GetComponent<Rigidbody>().transform.Rotate(new Vector3(0, 0, 1), BladeRotationSpeed * Time.deltaTime);
                rightBlades[i].GetComponent<Rigidbody>().transform.Rotate(new Vector3(0, 0, 1), BladeRotationSpeed * Time.deltaTime);
                if (BladeHit(leftBlades[i]) || BladeHit(rightBlades[i]))
                {
                    removing.Add(leftBlades[i]);
                    removing.Add(rightBlades[i]);
                }
            }
            for (int i = 0; i < removing.Count; i++)
            {
                leftBlades.Remove(removing[i]);
                leftBlades.Remove(removing[i]);
            }
        }

        private bool BladeHit(GameObject blade)
        {
            return false;
        }

        private void ThrowBlade()
        {
            if (!Hero.Equipment.Weapon.WeaponLeft.GetActive() && !Hero.Equipment.Weapon.WeaponLeft.GetActive()) return;
            bladeThrown = true;
            Hero.Equipment.Weapon.WeaponLeft.SetActive(false);
            Hero.Equipment.Weapon.WeaponRight.SetActive(false);
            Hero.currentBladeSta = 0f;
            //Hero.Equipment.Weapon.AmountLeft--;
            //Hero.Equipment.Weapon.AmountRight--;

            RaycastHit hit;
            LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer();
            if (Physics.Raycast(ray, out hit, float.MaxValue, mask.value))
            {
                velocity = Vector3.Normalize(hit.point - Hero.transform.position) * BladeSpeed;
            }
            else
            {
                velocity = Vector3.Normalize(ray.direction) * BladeSpeed;
            }
            Transform leftTransform = Hero.Equipment.Weapon.WeaponLeft.transform;
            Transform rightTransform = Hero.Equipment.Weapon.WeaponRight.transform;
            GameObject leftBlade = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_blade_l"), leftTransform.position, leftTransform.rotation);
            GameObject rightBlade = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_blade_r"), rightTransform.position, rightTransform.rotation);
            leftBlade.GetComponent<Rigidbody>().isKinematic = true;
            rightBlade.GetComponent<Rigidbody>().isKinematic = true;
            leftBlades.Add(leftBlade);
            rightBlades.Add(rightBlade);
        }

    }
}
