using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class KillInfo : UiElement
    {
        private float alpha = 1f;
        private int col;
        public Text KillerLabel;
        public Text DamageLabel;
        public Text VictimLabel;
        private float lifeTime = 8f;
        private float maxScale = 1.5f;
        private int offset = 24;
        private bool start;
        private float timeElapsed;

        public void Destroy()
        {
            this.timeElapsed = this.lifeTime;
        }

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
                DamageLabel.text = dmg.ToString();
                if (dmg >= 1000)
                {
                    DamageLabel.color = Color.red;
                }
            }
        }

        private void Start()
        {
            this.start = true;
            base.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
            base.transform.localPosition = new Vector3(0f, -100f + (Screen.height * 0.5f), 0f);
        }

        private void Update()
        {
            if (this.start)
            {
                this.timeElapsed += Time.deltaTime;
                if (this.timeElapsed < 0.2f)
                {
                    base.transform.localScale = Vector3.Lerp(base.transform.localScale, (Vector3)(Vector3.one * this.maxScale), Time.deltaTime * 10f);
                }
                else if (this.timeElapsed < 1f)
                {
                    base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.one, Time.deltaTime * 10f);
                }
                if (this.timeElapsed > this.lifeTime)
                {
                    base.transform.position += new Vector3(0f, Time.deltaTime * 0.15f, 0f);
                    this.alpha = ((1f - (Time.deltaTime * 45f)) + this.lifeTime) - this.timeElapsed;
                    this.setAlpha(this.alpha);
                }
                else
                {
                    float num = ((int)(100f - (Screen.height * 0.5f))) + (this.col * this.offset);
                    base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, new Vector3(0f, -num, 0f), Time.deltaTime * 10f);
                }
                if (this.timeElapsed > (this.lifeTime + 0.5f))
                {
                    UnityEngine.Object.Destroy(base.gameObject);
                }
            }
        }
    }


}
