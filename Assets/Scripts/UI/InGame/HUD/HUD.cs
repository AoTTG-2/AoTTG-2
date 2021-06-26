using System;
using Assets.Scripts.UI.InGame.HUD.Weapon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class HUD : UiElement
    {
        public GameObject Damage;
        public Labels Labels;
        public GameObject BloodSmear;
        public UnityEngine.Sprite[] BloodSmearSprite;
        public bool inEditMode;
        public bool isActive = true;
        public Crosshair Crosshair;
        public Weapons Weapons;

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
                GameObject BloodOverlay = Instantiate(BloodSmear, transform);
                BloodOverlay.GetComponent<Image>().sprite = BloodSmearSprite[UnityEngine.Random.Range(0,BloodSmearSprite.Length)];
                Destroy(BloodOverlay, 5f);
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
                return (int) Math.Floor(scale);
            }
            else
            {
                return (int) Math.Floor(maxScaling);
            }
        }
    }
}
