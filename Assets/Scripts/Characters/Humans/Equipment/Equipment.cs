using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    private Hero Hero;

    public Weapon Weapon { get; set; }

    private void Awake()
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

        Weapon.Hero = Hero;
    }


}
