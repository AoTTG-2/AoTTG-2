using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSwitcher : MonoBehaviour {

	public Dropdown dropdown;
	public Toggle toggle;
	private void OnEnable() {
		foreach(Resolution resolution in Screen.resolutions)
		{
			string resolutionText = resolution.ToString().Split('@')[0];
			Dropdown.OptionData op = new Dropdown.OptionData(resolutionText);
			dropdown.options.Add(op);
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
	public void ChangeRes()
	{
		
	}
}
