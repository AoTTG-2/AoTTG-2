using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class ResolutionSwitcher : UiElement
    {

        public Dropdown Dropdown;
        public Toggle Toggle;
        public Text DropDownLabel;

        public string Resolution { get; set; }
        public int ScreenMode { get; set; }

        private void OnEnable()
        {
            Toggle.isOn = Screen.fullScreen;

            foreach (Resolution resolution in Screen.resolutions)
            {
                string resolutionText = resolution.ToString();
                Dropdown.OptionData op = new Dropdown.OptionData(resolutionText);
                Dropdown.options.Add(op);
                if (resolution.width == Screen.width && resolution.height == Screen.height)
                {
                    Dropdown.value = Dropdown.options.IndexOf(op);
                }

            }
        }

        public void ChangeResolution()
        {
            string res = DropDownLabel.text;
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                Resolution temp = Screen.resolutions[i];
                if (temp.ToString().Equals(res))
                {
                    Screen.SetResolution(temp.width, temp.height, Toggle.GetComponent<Toggle>().isOn, temp.refreshRate);
                    Resolution = temp.ToString();
                }
            }
        }

        public void ChangeScreenMode()
        {
            if (Toggle.isOn)
            {
                Screen.fullScreen = true;
                ScreenMode = 1;
            }
            else
            {
                Screen.fullScreen = false;
                ScreenMode = 0;
            }
        }

        public void LoadPlayerPrefs()
        {
            string resolution = PlayerPrefs.GetString("Resolution");
            int screen_mode = PlayerPrefs.GetInt("ScreenMode");
            DropDownLabel.text = resolution;
            Toggle.isOn = screen_mode == 1;
        }
    }
}