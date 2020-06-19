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
            var damageLabels = Damage.GetComponentsInChildren<Text>();
            foreach (var label in damageLabels)
            {
                label.text = damage.ToString();
            }

            ShowDamage();
        }

        private void ShowDamage()
        {
            Damage.GetComponent<Animator>().SetTrigger("ShowDamage");
        }
    }
}
