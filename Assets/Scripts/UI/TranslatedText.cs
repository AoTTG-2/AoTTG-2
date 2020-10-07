using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Language;

namespace Assets.Scripts.UI
{
    class TranslatedText:UnityEngine.UI.Text
    {
        string translationKey = string.Empty;

        private void tryTranslate()
        {
            if (!string.IsNullOrEmpty(translationKey))
                base.text = translationKey.translated();
        }

        protected override void Start()
        {
            if (base.text != "Text")
                translationKey = base.text;
            this.tryTranslate();

            base.Start();
        }

        public override string text { 
            get => base.text; 
            set
            {
                if (translationKey != value)
                    translationKey = value;
                this.tryTranslate();
            }
        }
    }
}
