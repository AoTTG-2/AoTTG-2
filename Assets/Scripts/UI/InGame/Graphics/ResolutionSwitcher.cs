using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
	public class ResolutionSwitcher : MonoBehaviour {

		public Dropdown Dropdown;
		public Dropdown ARDropdown;
		public Dropdown MDropdown;
		public Toggle Toggle;
		public Text DropDownLabel;

		public string Resolution { get; set; }
		public int ScreenMode { get; set; }

		private void OnEnable() {
			Toggle.isOn = Screen.fullScreen;
			string[] aspectRatios = new string[Screen.resolutions.Length];
			var temp = "";
			bool check = false;

			for(int i = 0; i < Display.displays.Length; i++)
			{
				Dropdown.OptionData op = new Dropdown.OptionData(i.ToString());
			}

			foreach (Resolution resolution in Screen.resolutions)
			{
				var resolutionInfo = resolution.ToString().Split('@');
				
				if (temp != resolutionInfo[1] &&  temp != "")
				{
					Debug.Log(resolution);
					check = true;
					Dropdown.OptionData _op = new Dropdown.OptionData(resolutionInfo[1]);
					ARDropdown.options.Add(_op);
				}
				if(!check)
				{
					Dropdown.OptionData op = new Dropdown.OptionData(resolutionInfo[0]);
					Dropdown.options.Add(op);
					if (resolution.width == Screen.width && resolution.height == Screen.height)
					{
						Dropdown.value = Dropdown.options.IndexOf(op);
					}
				}
				temp = resolutionInfo[1];
			}
		}

		public void ChangeMonitor()
		{
			Display.displays[MDropdown.value].Activate();
		}

		public void ChangeResolution()
		{
			string res = DropDownLabel.text;
			for(int i = 0; i < Screen.resolutions.Length; i++)
			{
				Resolution temp = Screen.resolutions[i];
				if(temp.ToString().Equals(res))
				{
					Screen.SetResolution(temp.width, temp.height, Toggle.GetComponent<Toggle>().isOn, temp.refreshRate);
					Resolution = temp.ToString();
				}
			}
		}

		public void ChangeScreenMode() 
		{
			if(Toggle.isOn)
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