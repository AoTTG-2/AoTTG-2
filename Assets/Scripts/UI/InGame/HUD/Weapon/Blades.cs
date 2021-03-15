using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame.Weapon
{
    public class Blades : MonoBehaviour
    {
        [HideInInspector]public float bladeSta = 100f;
        [HideInInspector]public float curGas = 100f;

        public GameObject BladeLeft;
        public GameObject BladeRight;
        public GameObject currentLeftBlade;
        public GameObject currentRightBlade;

        public GameObject BladeLeftSprite;
        public GameObject BladeRightSprite;
        public GameObject currentLeftBladeSprite;
        public GameObject currentRightBladeSprite;

        public GameObject bloodPrefab;

        public UnityEngine.Sprite[] BladesSprite;

        public Image leftImage;
        public Image rightImage;
        
        private GameObject curLeft;
        private GameObject curRight;

        private int previousBlades;

        private void Update()
        {   
            if(leftImage != null && rightImage != null)
            {
                if(bladeSta > 75)
                {
                    leftImage.sprite = rightImage.sprite = BladesSprite[0];
                } else if (bladeSta > 50 && bladeSta <= 75)
                {
                    leftImage.sprite = rightImage.sprite = BladesSprite[1];
                } else if (bladeSta > 25 && bladeSta <= 50)
                {
                    leftImage.sprite = rightImage.sprite = BladesSprite[2];
                } else if (bladeSta > 0 && bladeSta <= 25)
                {
                    leftImage.sprite = rightImage.sprite = BladesSprite[3];
                } else if (bladeSta <= 0)
                {
                    leftImage.sprite = rightImage.sprite = BladesSprite[4];
                }
            }   
        }
        
        public void ShakeBlades()
        {
            if(curLeft.transform.childCount < 5) Instantiate(bloodPrefab, curLeft.transform); //To prevent overpopulating blood prefabs
            if(curRight.transform.childCount < 5) Instantiate(bloodPrefab, curRight.transform);
            currentLeftBlade.GetComponent<Animator>().SetTrigger("Shake");
            currentRightBlade.GetComponent<Animator>().SetTrigger("Shake");
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

                    leftImage = previousLeftBlade.GetComponent<Image>();
                    rightImage = previousRightBlade.GetComponent<Image>();

                    curLeft = previousLeftBlade;
                    curRight = previousRightBlade;
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
