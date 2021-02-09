using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class HUD : UiElement
    {
        public GameObject Damage;
        public Labels Labels;
        public GameObject BloodSmear;

        public void SetDamage(int damage)
        {
            var damageLabels = Damage.GetComponentsInChildren<TMP_Text>();
            foreach (var label in damageLabels)
            {
                label.fontSize = ScaleDamageText(damage);
                label.text = damage.ToString();
            }

            ShowDamage();
            ShowBloodSmear(damage);

        }

        public void ClearDamage()
        {
            var damageLabels = Damage.GetComponentsInChildren<TMP_Text>();
            foreach (var label in damageLabels)
            {
                label.text = string.Empty;
            }
        }

        private void ShowDamage()
        {
            Damage.GetComponent<Animator>().SetTrigger("ShowDamage");
        }

        private void ShowBloodSmear(int damage)
        {
            // if(damage >= 1000)
            // {
                BloodSmear.GetComponent<Animator>().SetTrigger("ShowBloodSmear");
            // }
        }

        private int ScaleDamageText(int damage)
        {
            var baseFontSize = 150;
            var lowestDamageScaling = 10;
            var highestDamageScaling = 1400;
            var rateOfChange = 0.216;

            var damageOffset = rateOfChange * lowestDamageScaling;
            var scale = (damage * rateOfChange) - damageOffset + baseFontSize;
            var maxScaling = (highestDamageScaling * rateOfChange) - damageOffset + baseFontSize;

            if (damage < highestDamageScaling)
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
