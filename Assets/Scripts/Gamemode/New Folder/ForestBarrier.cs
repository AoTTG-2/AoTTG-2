using UnityEngine;

namespace Assets.Scripts.Gamemode.Racing
{
    public class ForestBarrier : MonoBehaviour
    {

        private void Awake()
        {
            FengGameManagerMKII.instance.ForestBarrier.Add(gameObject);
        }

        private void OnDestroy()
        {
            FengGameManagerMKII.instance.ForestBarrier.Remove(gameObject);
        }
    }
}
