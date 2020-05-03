using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSwitcher : MonoBehaviour {

	public Dropdown dropdown;
	public Toggle toggle;
	public Text DropDownLabel;

	public string Resolution { get; set; }
	public int ScreenMode { get; set; }
	private void OnEnable() {
		
		foreach(Resolution resolution in Screen.resolutions)
		{
			string resolutionText = resolution.ToString().Split('@')[0];
			Dropdown.OptionData op = new Dropdown.OptionData(resolutionText);
			dropdown.options.Add(op);
		}
	}

	public void ChangeResolution()
	{
		string res = DropDownLabel.text;
		for(int i = 0; i < Screen.resolutions.Length; i++)
		{
			Resolution temp = Screen.resolutions[i];
			if(temp.ToString().Split('@')[0].Equals(res))
			{
				Screen.SetResolution(temp.width, temp.height, toggle.GetComponent<Toggle>().isOn, temp.refreshRate);
				Resolution = temp.ToString();
			}
		}
	}

	public void ChangeScreenMode() 
	{
		if(toggle.GetComponent<Toggle>().isOn)
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

	public void LoadPlayerPrefs(string resolution, int screen_mode)
	{
		DropDownLabel.text = resolution.Split('@')[0];
		if(screen_mode == 1)
		{
			toggle.isOn = true;
		}
		else
		{
			toggle.isOn = false;
		}
	}
}
