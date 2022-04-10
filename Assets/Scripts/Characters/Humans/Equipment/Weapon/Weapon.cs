using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Equipment.Weapon
{
    public abstract class Weapon
    {
        public Hero Hero { get; set; }

        public GameObject WeaponLeftPrefab;
        public GameObject WeaponRightPrefab;
        public GameObject WeaponLeft;
        public GameObject WeaponRight;
        public int AmountLeft;
        public int AmountRight;

        //TODO: Animations should be stored somewhere else?
        public string HookForwardLeft;
        public string HookForwardRight;
        public string HookForward;

        public abstract bool CanReload { get; }

        /// <summary>
        /// Reenables weapons so they are visible in the game view. Is used only for avoiding bugs when reload animations are cancelled
        /// </summary>
        public abstract void EnableWeapons();

        public abstract void PlayReloadAnimation();
        /// <summary>
        /// Reload the weapon
        /// </summary>
        public abstract void Reload();
        /// <summary>
        /// Resupply the weapon to retrieve the max amount of ammunition
        /// </summary>
        public abstract void Resupply();
        public abstract void UpdateSupplyUi(GameObject inGameUi);
        public abstract void Use(int amount = 0);
    }
}
