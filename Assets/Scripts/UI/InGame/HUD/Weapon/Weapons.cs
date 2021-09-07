using Assets.Scripts.Characters.Humans.Equipment;
using Assets.Scripts.UI.InGame.Weapon;
using System;
using UnityEngine;

namespace Assets.Scripts.UI.InGame.HUD.Weapon
{
    public class Weapons : MonoBehaviour
    {
        public Blades Blades;
        public AHSS AHSS;

        public void SetWeapon(EquipmentType type)
        {
            switch (type)
            {
                case EquipmentType.Blades:
                    Blades.gameObject.SetActive(true);
                    AHSS.gameObject.SetActive(false);
                    break;
                case EquipmentType.Ahss:
                    Blades.gameObject.SetActive(false);
                    AHSS.gameObject.SetActive(true);
                    break;
                case EquipmentType.ThunderSpear:
                    Blades.gameObject.SetActive(true);
                    AHSS.gameObject.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
