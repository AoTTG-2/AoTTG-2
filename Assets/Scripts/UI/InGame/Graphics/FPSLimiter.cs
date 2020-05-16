using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Newtonsoft.Json;

public class FPSLimiter : MonoBehaviour {

	
	[SerializeField] public Slider fpsLimiter;
	[SerializeField] public Dropdown fpsDropdown;
	

	public Dropdown FPSDropdown
	{
		get { return fpsDropdown; }
		set { fpsDropdown = value; }
	}
	public Slider FPSLimit
	{
		get { return fpsLimiter; }
		set { fpsLimiter = value; }
	}
	

	private void Start() {
		FPSDropdown.value = 1;
		SetFPSLimit();
	}

	public void SetFPSLimit()
	{
		if(FPSDropdown.value == 6)
		{
			FPSLimit.interactable = true;
			Application.targetFrameRate = (int)fpsLimiter.value;
		}
		else
		{
			FPSLimit.interactable = false;
			if(FPSDropdown.value == 0)
			{
				Application.targetFrameRate = 30;
			}
			else if(FPSDropdown.value == 1)
			{
				Application.targetFrameRate = 60;
			}
			else if(FPSDropdown.value == 2)
			{
				Application.targetFrameRate = 120;
			}
			else if(FPSDropdown.value == 3)
			{
					Application.targetFrameRate = 240;
			}
			else
			{
				Application.targetFrameRate = -1;
			}
		}	
	}

	
}
