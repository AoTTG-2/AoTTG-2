using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Equipment.Weapon
{
    public class Blades : Weapon
    {
        public override bool CanReload => AmountLeft < 5 || AmountRight < 5;

        public bool bladesThrown;       //Have the blades been thrown (in order to prepare for reloading, has nothing to do with the attack throwing blades)
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

        public override void EnableWeapons()
        {
            if (TotalBlades > 0)
            {
                WeaponLeft.SetActive(true);
                WeaponRight.SetActive(true);
                Hero.currentBladeSta = Hero.totalBladeSta;
                bladesThrown = false;
                Hero.weaponDisabledOnReloading = false;
            }
        }

        public override void Reload()
        {
            if (!Hero.grounded)
            {
                float reloadAnimBladeThrowTimeNonGrounded = 0.2f;     //How far along does the reload animation need to go before the blades are thrown in normalized time.
                float reloadAnimBladeEnableTimeNonGrounded = 0.56f;   //How far along does the reload animation need to go before blades are reenabled in normalized time.
                if (!(Hero.Animation[Hero.reloadAnimation].normalizedTime < reloadAnimBladeThrowTimeNonGrounded || Hero.throwedBlades))
                {
                    Hero.throwedBlades = true;
                    if (WeaponLeft.activeSelf)
                    {
                        DropBladesAndReload();
                    }
                }
                //Checks for how finished the "reload" animation is for blades. If it's more than the float % finished, it will restock the blades
                if (Hero.Animation[Hero.reloadAnimation].normalizedTime >= reloadAnimBladeEnableTimeNonGrounded && TotalBlades > 0 || Hero.Animation != Hero.Animation[Hero.reloadAnimation])
                {
                    EnableWeapons();
                }
            }
            else
            {
                float reloadAnimBladeThrowTimeGrounded = 0.13f;     //How far along does the reload animation need to go before the blades are thrown in normalized time.
                float reloadAnimBladeEnableTimeGrounded = 0.37f;    //How far along does the reload animation need to go before blades are reenabled in normalized time.
                if (!(Hero.Animation[Hero.reloadAnimation].normalizedTime < reloadAnimBladeThrowTimeGrounded || Hero.throwedBlades))
                {
                    Hero.throwedBlades = true;
                    if (WeaponLeft.activeSelf)
                    {
                        DropBladesAndReload();
                    }
                }
                //Checks for how finished the "reload" animation is for blades. If it's more than the float % finished, it will restock the blades
                if (Hero.Animation[Hero.reloadAnimation].normalizedTime >= reloadAnimBladeEnableTimeGrounded && TotalBlades > 0)
                {
                    EnableWeapons();
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

        private void DropBladesAndReload()
        {
            Hero.weaponDisabledOnReloading = true;
            DropBlades();
        }

        private void DropBlades()
        {
            bladesThrown = true;
            var leftTransform = WeaponLeft.transform;
            var rightTransform = WeaponRight.transform;
            var leftBlade = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_blade_l"), leftTransform.position, leftTransform.rotation);
            var rightBlade = (GameObject) Object.Instantiate(Resources.Load("Character_parts/character_blade_r"), rightTransform.position, rightTransform.rotation);
            //obj2.GetComponent<Renderer>().material = CharacterMaterials.materials[Hero.setup.myCostume._3dmg_texture];
            //obj3.GetComponent<Renderer>().material = CharacterMaterials.materials[Hero.setup.myCostume._3dmg_texture];
            Vector3 force = (Hero.transform.forward + Hero.transform.up * 2f) - Hero.transform.right;
            leftBlade.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
            Vector3 vector2 = (Hero.transform.forward + Hero.transform.up * 2f) + Hero.transform.right;
            rightBlade.GetComponent<Rigidbody>().AddForce(vector2, ForceMode.Impulse);
            Vector3 torque = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
            torque.Normalize();
            leftBlade.GetComponent<Rigidbody>().AddTorque(torque);
            torque = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
            torque.Normalize();
            rightBlade.GetComponent<Rigidbody>().AddTorque(torque);
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
                    DropBlades();
                }
            }
        }
    }
}
