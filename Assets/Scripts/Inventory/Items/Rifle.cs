using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Humans.Equipment.Weapon;
using Assets.Scripts.Inventory.Items.Data;
using UnityEngine;

namespace Assets.Scripts.Inventory.Items
{
    public class Rifle : Item
    {

        private Weapon thisWeapon;

        public override void Use(Hero hero)
        {

            if(thisWeapon == null) thisWeapon = new Characters.Humans.Equipment.Weapon.Rifle();
            var equipment = hero.EquipmentType;
            hero.Equipment.ChangeWeapon(equipment, thisWeapon);
            //TO DO: have the rifle become equipped on hero and set their limiations while it is equipped
        }

        public Rifle(RifleData data) : base(data)
        {

        }


    }

}
