using Assets.Scripts.Services;
using TMPro;

namespace Assets.Scripts.UI.Menu
{
    /// <summary>
    /// UI Class for the "Map Converter" page
    /// </summary>
    public class MapConverter : UiNavigationElement
    {
        public TMP_InputField InputLegacyMap;
        public TMP_InputField Output;

        public void Convert()
        {
            var convertedMap = Service.Map.ConvertLegacyMap(InputLegacyMap.text);
            Output.text = convertedMap;
        }

        public override void Back()
        {
            InputLegacyMap.text = string.Empty;
            Output.text = string.Empty;
            base.Back();
        }
    }
}
