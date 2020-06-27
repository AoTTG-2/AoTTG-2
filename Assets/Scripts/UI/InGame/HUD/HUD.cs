using System;
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
                label.fontSize = ScaleDamageText(damage);
                label.text = damage.ToString();
            }

            ShowDamage();
        }

        private void ShowDamage()
        {
            Damage.GetComponent<Animator>().SetTrigger("ShowDamage");
        }
        private int ScaleDamageText(int damage)
        {
            var rateOfChange = 0.214;
            var fontSize = 150;
            var scale = (damage * rateOfChange) + fontSize;
            var maxScaling = (1400 * rateOfChange) + fontSize;

            if (damage < 1400)
            {
                return (int) Math.Round(scale);
            }
            else
            {
                return (int) Math.Round(maxScaling);
            }
        }
    }
}
