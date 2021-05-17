using UnityEngine;
using TMPro;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class KillInfo : UiElement
    {
        private int col;
        public TMP_Text KillerLabel;
        public TMP_Text DamageLabel;
        public TMP_Text VictimLabel;
        private float lifeTime = 8f;
        private float maxScale = 1.5f;
        private int offset = 24;
        private bool start;
        private float timeElapsed;

        [Header("Colors")] //Customize in the inspector
        public Color fiveHundred;
        public Color oneThousand;
        public Color twoThousand;
        public Color threeThousand;
        public Color fourThousand;

        public void Destroy()
        {
            this.timeElapsed = this.lifeTime;
        }

        // Increments the column counter so that kills don't overlap. 
        public void MoveOn()
        {
            this.col++;
            if (this.col > 4)
            {
                this.timeElapsed = this.lifeTime;
            }
        }

        public void Show(bool isTitan1, string name1, bool isTitan2, string name2, int dmg = 0)
        {

            KillerLabel.text = name1;
            VictimLabel.text = name2;

            if (dmg == 0)
            {
                DamageLabel.text = "";
            } else
            {
                DamageLabel.text = dmg.ToString();
                if (dmg >= 500 && dmg < 1000)
                    DamageLabel.color = fiveHundred;
                else if (dmg >= 1000 && dmg < 2000)
                    DamageLabel.color = oneThousand;
                else if (dmg >= 2000 && dmg < 3000)
                    DamageLabel.color = twoThousand;
                else if (dmg >= 3000 && dmg < 4000)
                    DamageLabel.color = threeThousand;
                else if (dmg >= 4000)
                    DamageLabel.color = fourThousand;
            }
        }

        private void Start()
        {
            this.start = true;
            this.transform.localScale = new Vector3(1,1,1);
        }

        private void Update()
        {
            if (this.start)
            {
                this.timeElapsed += Time.deltaTime;
                if (this.timeElapsed > (this.lifeTime + 0.5f))
                {
                    UnityEngine.Object.Destroy(base.gameObject);
                }
            }
        }
    }


}
