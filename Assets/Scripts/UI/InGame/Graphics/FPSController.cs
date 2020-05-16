using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Newtonsoft.Json;

public class FPSController : MonoBehaviour {

	private float time;
	[SerializeField] public Slider fpsLimiter;
	[SerializeField] public Dropdown fpsDropdown;
	[SerializeField] public Text fpsCounter;

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
	public Text FPSCounter
	{
		get { return fpsCounter; }
		set { fpsCounter = value; }
	}

	private void Start() {
		FPSDropdown.value = 5;
		SetFPSLimit();
		time = 0f;
	}

	private void Update() {
		Counter();
	}

	public void SetFPSLimit()
	{
		if(FPSDropdown.value == 6)
		{
			FPSLimiter.interactable = true;
			Application.targetFrameRate = (int)fpsLimiter.value;
		}
		else
		{
			FPSLimiter.interactable = false;
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
				Application.targetFrameRate = 144;
			}
			else if(FPSDropdown.value == 4)
			{
					Application.targetFrameRate = 240;
			}
			else
			{
				Application.targetFrameRate = -1;
			}
		}	
	}

	public void Counter()
	{
		time += Time.deltaTime;
		if(time >= 1.0f)
		{
			var fps = 1.0f/Time.deltaTime;
			FPSCounter.text = Convert.ToInt64(fps).ToString();
			time = 0f;
		}
		
	}
}
