using Assets.Scripts.Characters.Humans.Equipment.Weapon;
using System.Linq;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Equipment
{
    public class Equipment : MonoBehaviour
    {
        private Hero Hero;

        public Weapon.Weapon Weapon { get; set; }

        public void Initialize()
        {
            Hero = gameObject.GetComponent<Hero>();
            var equipment = Hero.Prefabs.Equipment.Single(x => x.EquipmentType == Hero.EquipmentType);
            switch (Hero.EquipmentType)
            {
                case EquipmentType.Blades:
                    Weapon = new Blades();
                    Weapon.WeaponLeft = Hero.Body.hand_L.Find("character_blade_l(Clone)").gameObject;
                    Weapon.WeaponRight = Hero.Body.hand_R.Find("character_blade_r(Clone)").gameObject;
                    Weapon.WeaponLeftPrefab = equipment.WeaponLeft;
                    Weapon.WeaponRightPrefab = equipment.WeaponRight;
                    Hero.rightweapontrail = Weapon.WeaponRight.GetComponentInChildren<MeleeWeaponTrail>();
                    Hero.leftweapontrail = Weapon.WeaponLeft.GetComponentInChildren<MeleeWeaponTrail>();
                    break;
                case EquipmentType.Ahss:
                    Weapon = new Ahss();
                    Weapon.WeaponLeft = Hero.Body.hand_L.Find("character_gun_l(Clone)").gameObject;
                    Weapon.WeaponRight = Hero.Body.hand_R.Find("character_gun_r(Clone)").gameObject;
                    Weapon.WeaponLeftPrefab = equipment.WeaponLeft;
                    Weapon.WeaponRightPrefab = equipment.WeaponRight;
                    break;
                default:
                    Weapon = new Blades();
                    break;
            }

            Weapon.Hero = Hero;
            Service.Ui.GetUiHandler().InGameUi.HUD.Weapons.SetWeapon(Hero.EquipmentType);
        }


    }
}
