using UnityEngine;

namespace Assets.Scripts.UI.InGame.Weapon
{
    public class Blades : MonoBehaviour
    {
        public GameObject BladeLeft;
        public GameObject BladeRight;
        public GameObject currentLeftBlade;
        public GameObject currentRightBlade;

        public GameObject BladeLeftSprite;
        public GameObject BladeRightSprite;
        public GameObject currentLeftBladeSprite;
        public GameObject currentRightBladeSprite;
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

            foreach (Transform child in currentLeftBlade.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in currentRightBlade.transform)
            {
                Destroy(child.gameObject);
            }

            var previousLeftBlade = BladeLeft;
            var previousRightBlade = BladeRight;
            for (var i = 0; i < blades; i++)
            {

                if(i == 0)
                {
                    previousLeftBlade = Instantiate(currentLeftBladeSprite, currentLeftBlade.transform);

                    previousRightBlade = Instantiate(currentRightBladeSprite, currentRightBlade.transform);
                }
                else
                {
                    previousLeftBlade = Instantiate(BladeLeftSprite, BladeLeft.transform);

                    previousRightBlade = Instantiate(BladeRightSprite, BladeRight.transform);
                }
                
            }

            

        }
    }
}
