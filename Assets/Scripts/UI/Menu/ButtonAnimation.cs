using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Menu
{
    class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        protected float sizeIncrease, scaleTime, soundDelay;
        [SerializeField]
        protected AudioSource enterSound;
        [SerializeField]
        protected Color hoveredTextColor;


        private Vector3 normalScale;
        private LTDescr delayedSound, sizeScaler;

        private Color baseTextColor;
        private Image button_bg;
        private Text button_text;
        private bool hovered = false;
        private Coroutine backGroundFader;

        protected void Awake()
        {
            this.normalScale = this.transform.localScale;
            this.button_bg = this.GetComponent<Image>();
            this.button_text = this.GetComponentInChildren<Text>();
            this.baseTextColor = this.button_text.color;
        }

        protected virtual void OnHover(PointerEventData eventData)
        {
            this.hovered = true;
            this.sizeScaler = LeanTween.scale(this.gameObject, this.normalScale * this.sizeIncrease, scaleTime);
            this.delayedSound = LeanTween.delayedCall(soundDelay, enterSound.Play);
            if(this.backGroundFader==null)
                this.backGroundFader = this.StartCoroutine(bgFader());
        }

        protected virtual void OnExit(PointerEventData eventData)
        {
            this.hovered = false;
            if (this.sizeScaler != null)
            {
                LeanTween.cancel(this.sizeScaler.id);
            }
            if (this.delayedSound != null)
            {
                LeanTween.cancel(this.delayedSound.id);
            }
            this.sizeScaler = LeanTween.scale(this.gameObject, this.normalScale, scaleTime);
            if (this.backGroundFader == null)
                this.backGroundFader = this.StartCoroutine(bgFader());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.OnHover(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.OnExit(eventData);
        }

        void OnDisable()
        {
            Color color = this.button_bg.color;
            color.a = 0;
            this.button_bg.color = color;
            this.button_text.color = this.baseTextColor;
        }

        public IEnumerator bgFader()
        {
            Color color = this.button_bg.color;
            float alpha = color.a;

            while ((this.hovered?alpha<1:alpha>0))
            {
                var delta_step = Time.deltaTime / this.scaleTime;

                if (hovered)
                    alpha += delta_step;
                else
                    alpha -= delta_step;

                if (alpha > 1)
                    alpha = 1;
                else if (alpha < 0)
                    alpha = 0;

                color.a = alpha;
                this.button_bg.color = color;
                this.button_text.color = Color.Lerp(this.baseTextColor, this.hoveredTextColor, alpha);

                yield return null;
            }

            backGroundFader = null;
        }
    }
}
