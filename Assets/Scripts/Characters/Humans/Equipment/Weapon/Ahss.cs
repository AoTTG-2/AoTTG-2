using Assets.Scripts.UI.InGame.Weapon;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Equipment.Weapon
{
    public class Ahss : Weapon 
    {
        private const int MaxAmmo = 7;

        public Ahss()
        {
            HookForwardLeft = "AHSS_hook_forward_l";
            HookForwardRight = "AHSS_hook_forward_r";
            HookForward = "AHSS_hook_forward_both";
            AmountLeft = AmountRight = MaxAmmo;
        }

        public override bool CanReload => AmountLeft < 7 || AmountRight < 7;

        public override void EnableWeapons()
        {
            throw new System.NotImplementedException();
        }

        public override void PlayReloadAnimation()
        {
            if (!Hero.leftGunHasBullet && !Hero.rightGunHasBullet)
            {
                if (Hero.grounded)
                {
                    Hero.reloadAnimation = "AHSS_gun_reload_both";
                }
                else
                {
                    Hero.reloadAnimation = "AHSS_gun_reload_both_air";
                }
            }
            else if (!Hero.leftGunHasBullet)
            {
                if (Hero.grounded)
                {
                    Hero.reloadAnimation = "AHSS_gun_reload_l";
                }
                else
                {
                    Hero.reloadAnimation = "AHSS_gun_reload_l_air";
                }
            }
            else if (!Hero.rightGunHasBullet)
            {
                if (Hero.grounded)
                {
                    Hero.reloadAnimation = "AHSS_gun_reload_r";
                }
                else
                {
                    Hero.reloadAnimation = "AHSS_gun_reload_r_air";
                }
            }
            else
            {
                if (Hero.grounded)
                {
                    Hero.reloadAnimation = "AHSS_gun_reload_both";
                }
                else
                {
                    Hero.reloadAnimation = "AHSS_gun_reload_both_air";
                }
                Hero.rightGunHasBullet = false;
                Hero.leftGunHasBullet = false;
            }
            Hero.CrossFade(Hero.reloadAnimation, 0.05f);
        }

        public override void Reload()
        {
            if (Hero.Animation[Hero.reloadAnimation].normalizedTime > 0.22f)
            {
                if (!(Hero.leftGunHasBullet || !WeaponLeft.activeSelf))
                {
                    WeaponLeft.SetActive(false);
                    Transform transform = WeaponLeft.transform;
                    GameObject obj5 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_l"), transform.position, transform.rotation);
                    //obj5.GetComponent<Renderer>().material = CharacterMaterials.materials[Hero.setup.myCostume._3dmg_texture];
                    Vector3 force = (-Hero.transform.forward * 10f) + (Hero.transform.up * 5f) - Hero.transform.right;
                    obj5.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                    Vector3 torque = new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100));
                    obj5.GetComponent<Rigidbody>().AddTorque(torque, ForceMode.Acceleration);
                }
                if (!(Hero.rightGunHasBullet || !WeaponRight.activeSelf))
                {
                    WeaponRight.SetActive(false);
                    Transform transform5 = WeaponRight.transform;
                    GameObject obj6 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_r"), transform5.position, transform5.rotation);
                    //obj6.GetComponent<Renderer>().material = CharacterMaterials.materials[Hero.setup.myCostume._3dmg_texture];
                    Vector3 vector3 = (-Hero.transform.forward * 10f) + (Hero.transform.up * 5f) + Hero.transform.right;
                    obj6.GetComponent<Rigidbody>().AddForce(vector3, ForceMode.Impulse);
                    Vector3 vector4 = new Vector3(UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-300, 300));
                    obj6.GetComponent<Rigidbody>().AddTorque(vector4, ForceMode.Acceleration);
                }
            }
            if ((Hero.Animation[Hero.reloadAnimation].normalizedTime > 0.62f) && !Hero.throwedBlades)
            {
                Hero.throwedBlades = true;
                if (!((Hero.leftBulletLeft <= 0) || Hero.leftGunHasBullet))
                {
                    AmountLeft--;
                    Hero.leftBulletLeft--;
                    WeaponLeft.SetActive(true);
                    Hero.leftGunHasBullet = true;
                }
                if (!((Hero.rightBulletLeft <= 0) || Hero.rightGunHasBullet))
                {
                    WeaponRight.SetActive(true);
                    AmountRight--;
                    Hero.rightBulletLeft--;
                    Hero.rightGunHasBullet = true;
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
            var bladesUi = inGameUi.GetComponentInChildren<AHSS>();
            bladesUi?.SetAHSS(AmountLeft, AmountRight);
        }

        public override void Use(int amount = 0)
        {

        }
    }
}
