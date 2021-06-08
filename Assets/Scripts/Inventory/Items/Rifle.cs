using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Inventory.Items.Data;
using UnityEngine;

namespace Assets.Scripts.Inventory.Items
{
    public class Rifle : Item
    {

        public int MaxRounds;
        public int CurrentRounds;
        public float ReloadTime;
        public float DamagePerRound;

        public override void Use(Hero hero)
        {
            Debug.Log("Used rifle");
            //TO DO: have the rifle become equipped on hero and set their limiations while it is equipped
        }

        public Rifle(RifleData data) : base(data)
        {

        }


    }

}
