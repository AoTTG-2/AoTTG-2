using UnityEngine;

namespace Assets.Scripts.Utility
{

    public static class SpectatorMode
    {

        private static SpectatorMovement _specMovement;

        public static void Initialize()
        {
            _specMovement = GameObject.Find("MainCamera").GetComponent<SpectatorMovement>();
        }

        public static SpectatorMovement Get()
        {
            return _specMovement;
        }

        public static bool IsDisable()
        {
            return _specMovement.disable;
        }

        public static void Toggle()
        {
            _specMovement.disable = !_specMovement.disable;
        }

        public static void Disable()
        {
            _specMovement.disable = true;
        }

        public static void Enable()
        {
            _specMovement.disable = false;
        }

        public static void SetState(bool value)
        {
            _specMovement.disable = !value;
        }
    }

}