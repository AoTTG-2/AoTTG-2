using Assets.Scripts.Characters.Humans;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Inventory.Items.Data;

namespace Assets.Scripts.Inventory.Items
{
    public class Lantern : Item
    {
        public override void Use(Hero hero)
        {

            Debug.Log("Used a lantern");
            //TO DO: Create a light source in front of hero which everyone can see

        }

        public Lantern(LanternData data) : base(data)
        {



        }

    }

}
