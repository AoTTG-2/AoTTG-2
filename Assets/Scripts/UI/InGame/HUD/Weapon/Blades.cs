using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame.Weapon
{
    public class Blades : MonoBehaviour
    {
        public float bladeSta = 100f;

        public GameObject BladeLeft;
        public GameObject BladeRight;
        public GameObject currentLeftBlade;
        public GameObject currentRightBlade;

        public GameObject BladeLeftSprite;
        public GameObject BladeRightSprite;
        public GameObject currentLeftBladeSprite;
        public GameObject currentRightBladeSprite;

        public UnityEngine.Sprite[] LeftBlades;
        public UnityEngine.Sprite[] RightBlades;

        private Image leftImage;
        private Image rightImage;
        
        private int previousBlades;

        private void Start()
        {
            leftImage = currentLeftBladeSprite.GetComponent<Image>();
            rightImage = currentRightBladeSprite.GetComponent<Image>();
        }

        private void Update()
        {
            if(bladeSta > 75)
            {
                leftImage.sprite = LeftBlades[0];
                rightImage.sprite = LeftBlades[0];
            } else if (bladeSta > 50 && bladeSta <= 75)
            {
                leftImage.sprite = LeftBlades[1];
                rightImage.sprite = LeftBlades[1];
            } else if (bladeSta > 25 && bladeSta <= 50)
            {
                leftImage.sprite = LeftBlades[2];
                rightImage.sprite = LeftBlades[2];
            } else if (bladeSta > 0 && bladeSta <= 25)
            {
                leftImage.sprite = LeftBlades[3];
                rightImage.sprite = LeftBlades[3];
            } else if (bladeSta <= 0)
            {
                leftImage.sprite = LeftBlades[0];
                rightImage.sprite = LeftBlades[0];
            }
        }

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
