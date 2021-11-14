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
        private bool UsePhysics { get; set; }

        private Ray ray;
        private Vector3 velocity;
        private bool bladeThrown;

        public BladeThrowSkill(Hero hero) : base(hero)
        {
            Cooldown = CooldownLimit;
        }

        public override bool Use()
        {
            if (Hero._state != HumanState.Idle || Hero.currentBladeSta <= 0f) return false;

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

        private void ThrowBlade()
        {
            if (!Hero.Equipment.Weapon.WeaponLeft.GetActive() && !Hero.Equipment.Weapon.WeaponLeft.GetActive()) return;
            bladeThrown = true;
            Hero.Equipment.Weapon.WeaponLeft.SetActive(false);
            Hero.Equipment.Weapon.WeaponRight.SetActive(false);
            Hero.Equipment.Weapon.AmountLeft--;
            Hero.Equipment.Weapon.AmountRight--;

            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
            GameObject leftBlade = PhotonNetwork.Instantiate("fx/thrownBladeL", leftTransform.position, leftTransform.rotation, 0);
            GameObject rightBlade = PhotonNetwork.Instantiate("fx/thrownBladeR", rightTransform.position, rightTransform.rotation, 0);
            if (PhotonNetwork.connected && Hero.photonView.isMine)
            {
                object[] objArray7 = new object[] { Hero.photonView.viewID, leftBlade.transform.position, velocity, Hero.myTeam };
                leftBlade.GetPhotonView().RPC(nameof(ThrownBlade.InitRPC), PhotonTargets.Others, objArray7);
                objArray7 = new object[] { Hero.photonView.viewID, rightBlade.transform.position, velocity, Hero.myTeam };
                rightBlade.GetPhotonView().RPC(nameof(ThrownBlade.InitRPC), PhotonTargets.Others, objArray7);
            }
            float scoreMulti = Hero.currentBladeSta / Hero.totalBladeSta * 0.4f + 0.1f;
            Hero.currentBladeSta = 0f;
            float bodyVel = Hero.GetComponent<Rigidbody>().velocity.magnitude;
            leftBlade.GetComponent<ThrownBlade>().Initialize(Hero, scoreMulti, bodyVel, velocity);
            rightBlade.GetComponent<ThrownBlade>().Initialize(Hero, scoreMulti, bodyVel, velocity);
        }

    }
}
