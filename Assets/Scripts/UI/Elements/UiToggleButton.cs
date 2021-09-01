using Assets.Scripts.Extensions;
using Assets.Scripts.UI.Tooltip;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements
{
    public class UiToggleButton : UiElement
    {
        public Color EnabledColor;
        public Color DisabledColor;
        public Button Button;
        public TextMeshProUGUI Text;
        public TooltipTrigger TooltipTrigger;

        public void Localize(StringTable table, string header, string content)
        {
            var localizedHeader = table.GetLocalizedString(header);
            var localizedContent = table.GetLocalizedString(content);
            Text.text = localizedHeader;
            TooltipTrigger.Header = localizedHeader;
            TooltipTrigger.Content = localizedContent;
        }

        [SerializeField]
        private bool value;
        public bool Value
        {
            get => value;
            set
            {
                this.value = value;
                Button.image.color = value ? EnabledColor : DisabledColor;
            }
        }

        private void OnDisable()
        {
            TooltipTrigger.OnPointerExit(null);
        }
    }
}
