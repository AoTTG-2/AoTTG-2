using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class DestroySelf : MonoBehaviour
    {
        public float countdown = 5f;

        void Update()
        {
            if(countdown > 0)
            {
                countdown -= Time.deltaTime;
            }

            if(countdown <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
