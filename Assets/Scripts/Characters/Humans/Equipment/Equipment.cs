using Assets.Scripts.Characters.Humans.Equipment.Weapon;
using System.Linq;
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
            switch (Hero.EquipmentType)
            {
                case EquipmentType.Blades:
                    Weapon = new Blades();
                    break;
                case EquipmentType.Ahss:
                    Weapon = new Ahss();
                    break;
                default:
                    Weapon = new Blades();
                    break;
            }

            var equipment = Hero.Prefabs.Equipment.Single(x => x.EquipmentType == Hero.EquipmentType);

            Weapon.WeaponLeft = Hero.Body.hand_L.Find("character_blade_l(Clone)").gameObject;
            Weapon.WeaponRight = Hero.Body.hand_R.Find("character_blade_r(Clone)").gameObject;
            Weapon.WeaponLeftPrefab = equipment.WeaponLeft;
            Weapon.WeaponRightPrefab = equipment.WeaponRight;
            Weapon.Hero = Hero;
        }


    }
}
