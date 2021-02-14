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
        public bool inEditMode;
        public bool isActive = true;
        public float damageTimer = 2.25f;
        public bool inDamageWindow;

        void Update()
        {
            if(damageTimer > 0)
            {
                damageTimer -= Time.deltaTime;
                inDamageWindow = true;
            }
            
            if(damageTimer <= 0)
            {
                inDamageWindow = false;
            }
            
        }

        public void SetDamage(int damage)
        {
            var damageLabels = Damage.GetComponentsInChildren<TMP_Text>();
            foreach (var label in damageLabels)
            {
                label.fontSize = ScaleDamageText(damage);
                if(inDamageWindow)
                {
                    label.text += " " + damage.ToString(); //Do some effects with double damage, maybe a certain animations
                    // label.color = Color.red;
                }
                else
                {
                    label.text = damage.ToString();
                }
            }

            damageTimer = 2.25f;

            ShowDamage();
            ShowBloodSmear(damage);
        }


        public void ClearDamage()
        {
            var damageLabels = Damage.GetComponentsInChildren<TMP_Text>();
            foreach (var label in damageLabels)
            {
                label.text = "";
            }
            Damage.transform.localScale = new Vector3(0,0,1f);
        }

        private void ShowDamage()
        {
            Damage.GetComponent<Animator>().SetTrigger("ShowDamage");
        }

        private void ShowBloodSmear(int damage)
        {
            if(damage >= 1000)
            {
                BloodSmear.GetComponent<Animator>().SetTrigger("ShowBloodSmear");
            }
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
