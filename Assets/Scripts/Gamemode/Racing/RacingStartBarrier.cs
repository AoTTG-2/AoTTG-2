using UnityEngine;

namespace Assets.Scripts.Gamemode.Racing
{
    public class RacingStartBarrier : MonoBehaviour
    {

        private void Awake()
        {
            FengGameManagerMKII.instance.racingDoors.Add(gameObject);
        }

        private void OnDestroy()
        {
            FengGameManagerMKII.instance.racingDoors.Remove(gameObject);
        }
    }
}
