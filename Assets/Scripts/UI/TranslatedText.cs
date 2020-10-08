using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Language;

namespace Assets.Scripts.UI
{
    class TranslatedText:UnityEngine.UI.Text
    {
        string translationKey = string.Empty;

        protected override void OnEnable()
        {
            if (!string.IsNullOrEmpty(translationKey))
                Internationalization.OnTranslationSet += () => text = translationKey;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            Internationalization.OnTranslationSet -= () => text = translationKey;
            base.OnDisable();
        }

        private void TryTranslate()
        {
            if (!string.IsNullOrEmpty(translationKey))
                base.text = translationKey.Translated();
        }

        protected override void Start()
        {
            if (base.text != "Text")
                text = base.text;
            this.TryTranslate();

            base.Start();
        }

        public override string text { 
            get => base.text; 
            set
            {
                if (translationKey != value)
                {
                    translationKey = value;
                    Internationalization.OnTranslationSet += () => text = translationKey;
                }
                this.TryTranslate();
            }
        }
    }
}
