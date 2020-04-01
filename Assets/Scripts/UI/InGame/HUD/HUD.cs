using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class HUD : MonoBehaviour
    {
        public GameObject Damage;
        public Labels Labels;

        public void SetDamage(int damage)
        {
            CancelInvoke("HideDamage");
            Damage.SetActive(true);
            var texts = Damage.GetComponentsInChildren<Text>();
            foreach (var text in texts)
            {
                text.text = damage.ToString();
            }
            Invoke("HideDamage", 3f);
        }

        private void HideDamage()
        {
            Damage.SetActive(false);
        }
    }
}
