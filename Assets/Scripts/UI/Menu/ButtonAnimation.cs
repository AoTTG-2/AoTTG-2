using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Menu
{
    class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        protected float sizeIncrease, scaleTime, soundDelay;
        [SerializeField]
        protected AudioSource enterSound;

        protected Vector3 normalScale;
        protected LTDescr delayedSound, sizeScaler;

        protected void Awake()
        {
            this.normalScale = this.transform.localScale;
        }

        protected virtual void OnHover(PointerEventData eventData)
        {            
            this.sizeScaler = LeanTween.scale(this.gameObject, this.normalScale * this.sizeIncrease, scaleTime);
            this.delayedSound = LeanTween.delayedCall(soundDelay, enterSound.Play);
        }

        protected virtual void OnExit(PointerEventData eventData)
        {
            if (this.sizeScaler != null)
            {
                LeanTween.cancel(this.sizeScaler.id);
            }
            if (this.delayedSound != null)
            {
                LeanTween.cancel(this.delayedSound.id);
            }
            this.sizeScaler = LeanTween.scale(this.gameObject, this.normalScale, scaleTime);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.OnHover(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.OnExit(eventData);
        }
    }
}
