using UnityEngine;

namespace Assets.Scripts.UI.InGame.Weapon
{
    public class AHSS : MonoBehaviour
    {
        public GameObject AHSSLeft;
        public GameObject AHSSRight;

        public GameObject AHSSSprite;
        private int distance = 18;
        private int previousLeft;
        private int previousRight;
        
        public void SetAHSS(int ammoLeft, int ammoRight)
        {
            if (ammoLeft == previousLeft && ammoRight == previousRight) return;
            previousLeft = ammoLeft;
            previousRight = ammoRight;

            foreach (Transform child in AHSSLeft.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in AHSSRight.transform)
            {
                Destroy(child.gameObject);
            }

            var previousLeftBlade = AHSSLeft;
            var previousRightBlade = AHSSRight;
            for (var i = 0; i < ammoLeft; i++)
            {
                var cordsLeft = previousLeftBlade.transform.position;
                cordsLeft.x -= distance;
                previousLeftBlade = Instantiate(AHSSSprite, cordsLeft, previousLeftBlade.transform.rotation, AHSSLeft.transform);
                previousLeftBlade.transform.position = cordsLeft;
            }

            for (var i = 0; i < ammoRight; i++)
            {
                var cordsRight = previousRightBlade.transform.position;
                cordsRight.x += distance;
                previousRightBlade = Instantiate(AHSSSprite, cordsRight, previousRightBlade.transform.rotation, AHSSRight.transform);
                previousRightBlade.transform.position = cordsRight;
            }
        }
    }
}