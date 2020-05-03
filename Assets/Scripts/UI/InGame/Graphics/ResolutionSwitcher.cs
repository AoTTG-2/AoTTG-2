using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSwitcher : MonoBehaviour {

	public Dropdown dropdown;
	public Toggle toggle;
	public Text label;
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
		string res = label.text;
		for(int i = 0; i < Screen.resolutions.Length; i++)
		{
			Resolution temp = Screen.resolutions[i];
			if(temp.ToString().Split('@')[0].Equals(res))
			{
				Screen.SetResolution(temp.width, temp.height, toggle.GetComponent<Toggle>().isOn, temp.refreshRate);
			}
		}
	}

	public void ChangeScreenMode() 
	{
		if(toggle.GetComponent<Toggle>().isOn)
		{
			Screen.fullScreen = true;
		}
		else
		{
			Screen.fullScreen = false;
		}
	}
}
