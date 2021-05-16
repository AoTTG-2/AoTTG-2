using UnityEngine;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class GasNotification : MonoBehaviour
    {
        public Assets.Scripts.UI.InGame.Weapon.Blades blades;
        public GameObject[] gasNotifications;

        void Update()
        {
            foreach(GameObject gas in gasNotifications)
            {
                gas.SetActive(blades.curGas <= 20);
            }
        }
    }
}