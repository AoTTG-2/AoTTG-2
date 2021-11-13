using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Equipment.Weapon
{
    public class Rifle : Weapon
    {

        private const int MaxAmmo = 5;
        private int currentBullets;

        public Rifle()
        {

            //USING BLADE ANIMATIONS FOR TESTING, CANNOT USE 3DMG WHILE USING THIS ITEM
            HookForwardLeft = "";
            HookForwardRight = "";
            HookForward = "";

            currentBullets = MaxAmmo;

        }

        public override bool CanReload => currentBullets <= 0;

        public override EquipmentType ThisType => EquipmentType.Rifle;

        public override void PlayReloadAnimation()
        {
            Debug.Log("No Rifle Reload Animation Yet");
        }

        public override void Reload()
        {
            PlayReloadAnimation();
            currentBullets = MaxAmmo;
        }

        public override void Resupply()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateSupplyUi(GameObject inGameUi)
        {
            
        }

        public override void Use(int amount = 0)
        {

            if (Hero.IsGrounded())
            {

                Debug.Log("Fired a bullet!");
                currentBullets -= 1;

            }

        }
    }
}