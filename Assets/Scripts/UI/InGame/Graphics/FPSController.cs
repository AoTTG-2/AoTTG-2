using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Newtonsoft.Json;

public class FPSController : MonoBehaviour {

	[SerializeField] public Slider fpsLimiter;
	[SerializeField] public Dropdown fpsDropdown;
	public Dropdown FPSDropdown
	{
		get { return fpsDropdown; }
		set { fpsDropdown = value; }
	}
	public Slider FPSLimiter
	{
		get { return fpsLimiter; }
		set { fpsLimiter = value; }
	}
	public void SetFPSLimit()
	{
		if(fpsDropdown.itemText.text == "No Limit")
		{
			fpsLimiter.interactable = true;
			Application.targetFrameRate = (int)fpsLimiter.value;
		}
		else
		{
			fpsLimiter.interactable = false;
			if(fpsDropdown.value == 0)
			{
				Application.targetFrameRate = 30;
			}
			else if(fpsDropdown.value == 1)
			{
				Application.targetFrameRate = 60;
			}
			else if(fpsDropdown.value == 2)
			{
				Application.targetFrameRate = 120;
			}
			else if(fpsDropdown.value == 3)
			{
				Application.targetFrameRate = 144;
			}
			else if(fpsDropdown.value == 4)
			{
					Application.targetFrameRate = 240;
			}
		}	
	}
}
