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
        
        /// <summary>
        /// InGame HUD display
        /// </summary>
        /// <param name="HUDPosition">Define which label has to be shown</param>
        /// <param name="content">the contenet to be either shown or added to the lbl targetted</param>
        /// <param name="add">If true doesn't overwrite the prev content of the lbl</param>
        public void ShowHUDInfo(LabelPosition HUDPosition, string content, bool add = false)
        {
            Text lblTarget;
            switch(HUDPosition)
            {
                case LabelPosition.TopRight:
                    lblTarget = Labels.TopRight;
                    break;
                case LabelPosition.TopLeft:
                    lblTarget = Labels.TopLeft;
                    break;
                case LabelPosition.TopCenter:
                    lblTarget = Labels.Top;
                    break;
                default:
                    lblTarget = Labels.Center;
                    break;
            }

            if (add)
                lblTarget.text += content;
            else
                lblTarget.text = content;
        }
    }
}
