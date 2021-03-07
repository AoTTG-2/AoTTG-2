using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class KillInfo : UiElement
    {
        private float alpha = 1f;
        private int col;
        //public Text KillerLabel;
        //public Text DamageLabel;
        //public Text VictimLabel;
        public TMP_Text KillerLabel;
        public TMP_Text DamageLabel;
        public TMP_Text VictimLabel;
        private float lifeTime = 8f;
        private float maxScale = 1.5f;
        private int offset = 24;
        private bool start;
        private float timeElapsed;
        private float startPosition;
        private float startPositionRatio = 0.85f; // this is the proportion of the total screen that is below the kill feed.

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

        private void setAlpha(float alpha)
        {

        }

        public void Show(bool isTitan1, string name1, bool isTitan2, string name2, int dmg = 0)
        {

            //if (!isTitan1)
            //{
            //    Transform transform = KillerLabel.transform;
            //    transform.position += new Vector3(18f, 0f, 0f);
            //}
            //else
            //{
            //    Transform transform3 = VictimLabel.transform;
            //    transform3.position -= new Vector3(18f, 0f, 0f);
            //}

            KillerLabel.text = name1;
            VictimLabel.text = name2;

            if (dmg == 0)
            {
                DamageLabel.text = "";
            }
            else
            {
                Color32 green = new Color32(110,247,59, 255);
                Color32 yellow = new Color32(246,255,77, 255);
                Color32 orange = new Color32(240,99,53, 255);
                Color32 purple = new Color32(134,50,194, 255);
                Color32 red = new Color32(186,43,43, 255);
                DamageLabel.text = dmg.ToString();
                if (dmg >= 500 && dmg < 1000)
                {
                    DamageLabel.color = green;
                } else 
                if (dmg >= 1000 && dmg < 2000)
                {
                    DamageLabel.color = yellow;
                } else 
                if (dmg >= 2000 && dmg < 3000)
                {
                    DamageLabel.color = orange;
                } else 
                if (dmg >= 3000 && dmg < 4000)
                {
                    DamageLabel.color = purple;
                } else 
                if (dmg >= 4000)
                {
                    DamageLabel.color = red;
                }
            }
        }

        private void Start()
        {
            this.start = true;
            this.transform.localScale = new Vector3(1,1,1);
            // base.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
            // startPosition = (Screen.height * startPositionRatio);
            // base.transform.position = new Vector3(Screen.width * 0.5f, startPosition, 0f);
        }

        private void Update()
        {
            if (this.start)
            {
                this.timeElapsed += Time.deltaTime;
                // if (this.timeElapsed < 0.2f)
                // {
                //     base.transform.localScale = Vector3.Lerp(base.transform.localScale, (Vector3)(Vector3.one * this.maxScale), Time.deltaTime * 10f);
                // }
                // else if (this.timeElapsed < 1f)
                // {
                //     base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.one, Time.deltaTime * 10f);
                // }
                // if (this.timeElapsed > this.lifeTime)
                // {
                //     base.transform.position += new Vector3(0f, Time.deltaTime * 0.15f, 0f);
                //     this.alpha = ((1f - (Time.deltaTime * 45f)) + this.lifeTime) - this.timeElapsed;
                //     this.setAlpha(this.alpha);
                // }
                // else
                // {
                //     float num = ((int)(-startPosition)) + (this.col * this.offset);
                //     base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(Screen.width * 0.5f, -num, 0f), Time.deltaTime * 10f);
                // }
                if (this.timeElapsed > (this.lifeTime + 0.5f))
                {
                    UnityEngine.Object.Destroy(base.gameObject);
                }
            }
        }
    }


}
