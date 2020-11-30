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

        public abstract void PlayReloadAnimation();
        public abstract void Reload();
        public abstract void UpdateSupplyUi(GameObject inGameUi);
        public abstract void Use(int amount = 0);
    }
}
