using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Equipment.Weapon
{
    public class Blades : Weapon
    {
        public override bool CanReload => AmountLeft < 5 || AmountRight < 5;

        private Assets.Scripts.UI.InGame.Weapon.Blades bladesUi;
        public int TotalBlades => AmountLeft;
        private const int MaxAmmo = 5;

        public Blades()
        {
            HookForwardLeft = "air_hook_l";
            HookForwardRight = "air_hook_r";
            HookForward = "air_hook";
            AmountLeft = AmountRight = MaxAmmo;
        }

        public override void PlayReloadAnimation()
        {
            Hero.reloadAnimation = Hero.grounded
                ? "changeBlade"
                : "changeBlade_air";
            Hero.CrossFade(Hero.reloadAnimation, 0.1f);
        }

        public override void Reload()
        {
            if (!Hero.grounded)
            {
                if (!(Hero.Animation[Hero.reloadAnimation].normalizedTime < 0.2f || Hero.throwedBlades))
                {
                    Hero.throwedBlades = true;
                    if (WeaponLeft.activeSelf)
                    {
                        ThrowBlades();
                    }
                }
                if (Hero.Animation[Hero.reloadAnimation].normalizedTime >= 0.56f && TotalBlades > 0)
                {
                    WeaponLeft.SetActive(true);
                    WeaponRight.SetActive(true);
                    Hero.currentBladeSta = Hero.totalBladeSta;
                }
            }
            else
            {
                if (!(Hero.Animation[Hero.reloadAnimation].normalizedTime < 0.13f || Hero.throwedBlades))
                {
                    Hero.throwedBlades = true;
                    if (WeaponLeft.activeSelf)
                    {
                        ThrowBlades();
                    }
                }
                if (Hero.Animation[Hero.reloadAnimation].normalizedTime >= 0.37f && TotalBlades > 0)
                {
                    WeaponLeft.SetActive(true);
                    WeaponRight.SetActive(true);
                    Hero.currentBladeSta = Hero.totalBladeSta;
                }
            }
        }

        public override void Resupply()
        {
            AmountLeft = AmountRight = MaxAmmo;
            WeaponLeft.SetActive(true);
            WeaponRight.SetActive(true);
        }

        public override void UpdateSupplyUi(GameObject inGameUi)
        {
            bladesUi = inGameUi.GetComponentInChildren<Assets.Scripts.UI.InGame.Weapon.Blades>();
            if (bladesUi == null) return;
            if(Hero.currentBladeSta > 0) 
                bladesUi.SetBlades(AmountLeft);
            //TODO: This is a temporary reference to bladeSta and gasSta of Hero;
            bladesUi.bladeSta = Hero.currentBladeSta;
            bladesUi.curGas = Hero.currentGas;
        }

        private void ThrowBlades()
        {
            var transform = WeaponLeft.transform;
            var transform2 = WeaponRight.transform;
            var obj2 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_blade_l"), transform.position, transform.rotation);
            var obj3 = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_blade_r"), transform2.position, transform2.rotation);

            //obj2.GetComponent<Renderer>().material = CharacterMaterials.materials[Hero.setup.myCostume._3dmg_texture];
            //obj3.GetComponent<Renderer>().material = CharacterMaterials.materials[Hero.setup.myCostume._3dmg_texture];
            Vector3 force = (Hero.transform.forward + Hero.transform.up * 2f) - Hero.transform.right;
            obj2.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
            Vector3 vector2 = (Hero.transform.forward + Hero.transform.up * 2f) + Hero.transform.right;
            obj3.GetComponent<Rigidbody>().AddForce(vector2, ForceMode.Impulse);
            Vector3 torque = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
            torque.Normalize();
            obj2.GetComponent<Rigidbody>().AddTorque(torque);
            torque = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
            torque.Normalize();
            obj3.GetComponent<Rigidbody>().AddTorque(torque);
            WeaponLeft.SetActive(false);
            WeaponRight.SetActive(false);

            AmountLeft--;
            AmountRight--;

            if (TotalBlades == 0)
                Hero.currentBladeSta = 0f;
        }

        public override void Use(int amount = 0)
        {
            if (amount == 0)
            {
                amount = 1;
            }
            amount *= 2;
            if (Hero.currentBladeSta > 0f)
            {
                Hero.currentBladeSta -= amount;
                bladesUi.ShakeBlades();
                if (Hero.currentBladeSta <= 0f)
                {
                    if (Hero.photonView.isMine)
                    {
                        //this.leftbladetrail.Deactivate();
                        //this.rightbladetrail.Deactivate();
                        //this.leftbladetrail2.Deactivate();
                        //this.rightbladetrail2.Deactivate();
                        Hero.checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive = false;
                        Hero.checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive = false;
                    }
                    Hero.currentBladeSta = 0f;
                    this.ThrowBlades();
                }
            }
        }
    }
}
