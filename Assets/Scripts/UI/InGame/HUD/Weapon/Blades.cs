using UnityEngine;

namespace Assets.Scripts.UI.InGame.Weapon
{
    public class Blades : MonoBehaviour
    {
        public GameObject BladeLeft;
        public GameObject BladeRight;

        public GameObject BladeLeftSprite;
        public GameObject BladeRightSprite;
        private int distance = 18;
        private int previousBlades;

        public void SetBlades(int blades)
        {
            if (blades == previousBlades) return;
            previousBlades = blades;

            foreach (Transform child in BladeLeft.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in BladeRight.transform)
            {
                Destroy(child.gameObject);
            }

            var previousLeftBlade = BladeLeft;
            var previousRightBlade = BladeRight;
            for (var i = 0; i < blades; i++)
            {
                var cordsLeft = previousLeftBlade.transform.position;
                cordsLeft.x -= distance;
                previousLeftBlade = Instantiate(BladeLeftSprite, cordsLeft, previousLeftBlade.transform.rotation, BladeLeft.transform);
                previousLeftBlade.transform.position = cordsLeft;

                var cordsRight = previousRightBlade.transform.position;
                cordsRight.x += distance;
                previousRightBlade = Instantiate(BladeRightSprite, cordsRight, previousRightBlade.transform.rotation, BladeRight.transform);
                previousRightBlade.transform.position = cordsRight;
            }
        }
    }
}
