using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Newtonsoft.Json;

public class FPSLimiter : MonoBehaviour {

	
	[SerializeField] public InputField fpsLimiter;
	[SerializeField] public Dropdown fpsDropdown;
	

	public Dropdown FPSDropdown
	{
		get { return fpsDropdown; }
		set { fpsDropdown = value; }
	}
	public InputField FPSLimit
	{
		get { return fpsLimiter; }
		set { fpsLimiter = value; }
	}
	

	private void Start() {
		SetFPSLimit();
	}

	public void SetFPSLimit()
	{
		if(FPSDropdown.value == 5)
		{
			FPSLimit.interactable = true;
			var text = FPSLimit.text;
			if (FPSLimit.contentType.Equals(InputField.ContentType.IntegerNumber))
			{
				try
				{
					Application.targetFrameRate = int.Parse(text.ToString());
				}
				catch (FormatException ex)
				{
					Debug.LogWarning("Inputed value is not a number!");
				}
			}
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

	[Serializable]
	public struct FPSData
	{
		public int dropdown;
		public string field;

		public FPSData(FPSLimiter toCopy)
		{
			this.dropdown = toCopy.FPSDropdown.value;
			this.field = toCopy.FPSLimit.text;
		}
	}


}
