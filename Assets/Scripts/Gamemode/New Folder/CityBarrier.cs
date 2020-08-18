using UnityEngine;

namespace Assets.Scripts.Gamemode.Racing
{
    public class CityBarrier : MonoBehaviour
    {

        private void Awake()
        {
            FengGameManagerMKII.instance.CityBarrier.Add(gameObject);
        }

        private void OnDestroy()
        {
            FengGameManagerMKII.instance.CityBarrier.Remove(gameObject);
        }
    }
}
