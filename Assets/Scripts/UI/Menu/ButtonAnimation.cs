using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Menu
{
    class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private float sizeIncrease, scaleTime, soundDelay;
        [SerializeField]
        private AudioSource enterSound;


        private Vector3 normalScale;
        private LTDescr delayedSound;

        void Awake()
        {
            this.normalScale = this.transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            LeanTween.scale(this.gameObject, this.normalScale * this.sizeIncrease, scaleTime);
            delayedSound = LeanTween.delayedCall(soundDelay, enterSound.Play);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            LeanTween.scale(this.gameObject, this.normalScale, scaleTime);
            if (delayedSound != null)
            {
                LeanTween.cancel(delayedSound.id);
                delayedSound = null;
            }
        }
    }
}
