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
            CancelInvoke("HideDamage");
            Damage.GetComponent<Animator>().SetFloat("Direction", 1);
            Damage.GetComponent<Animator>().SetTrigger("Show");
            Damage.SetActive(true);
            Invoke("HideDamage", 2f);
        }

        private void HideDamage()
        {
            Damage.GetComponent<Animator>().SetFloat("Direction", -1);
        }
    }
}
