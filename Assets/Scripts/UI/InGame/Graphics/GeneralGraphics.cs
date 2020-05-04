using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class GeneralGraphics : MonoBehaviour {

	public Dropdown TextureQuality;
	public Dropdown ShadowRes;
	public Dropdown AntiAliasing;
	public Dropdown Shadows;
	public Toggle SoftParticles;

	private void Update() {
		if(QualitySettings.shadowResolution == ShadowResolution.Low)
		{
			ShadowRes.value = 0;
		}
		else if(QualitySettings.shadowResolution == ShadowResolution.Medium)
		{
			ShadowRes.value = 1;
		}
		else if(QualitySettings.shadowResolution == ShadowResolution.High)
		{
			ShadowRes.value = 2;
		}
		else if(QualitySettings.shadowResolution == ShadowResolution.VeryHigh)
		{
			ShadowRes.value = 3;
		}

		if(QualitySettings.shadows == ShadowQuality.Disable)
		{
			Shadows.value = 0;
		}
		else if(QualitySettings.shadows == ShadowQuality.HardOnly)
		{
			Shadows.value = 1;
		}
		else if(QualitySettings.shadows == ShadowQuality.All)
		{
			Shadows.value = 2;
		}
		

		// AntiAliasing.value = QualitySettings.antiAliasing;

		
	}
	public void ChangeShadows()
	{
		var selected = Shadows.value;
		Debug.Log(selected);
		switch(selected)
		{
			case 0:
				QualitySettings.shadows = ShadowQuality.Disable;
				break;
			case 1:
				QualitySettings.shadows = ShadowQuality.HardOnly;	
				break;
			case 2:
				QualitySettings.shadows = ShadowQuality.All;
				break;	
		}
	}
	public void ChangeTextureQuality()
	{
		// check = true;
		//var selected = TextureQuality.value;
		// switch(selected)
		// {
		// 	case 0:
		// 		QualitySettings.
		// 		break;
		// 	case 1:
		// 		QualitySettings.shadows = ShadowQuality.HardOnly;	
		// 		break;
		// 	case 2:
		// 		QualitySettings.shadows = ShadowQuality.All;
		// 		break;	
		// }
	}

	public void ChangeAntiAliasing()
	{
		var selected = AntiAliasing.value;
		switch(selected)
		{
			case 0:
				QualitySettings.antiAliasing = 0;
				break;
			case 1:
				QualitySettings.antiAliasing = 1;
				break;
			case 2:
				QualitySettings.antiAliasing = 2;
				break;
			case 3:
				QualitySettings.antiAliasing = 3;
				break;				
		}
	}

	public void ChangeShadowResolution()
	{
		var selected = ShadowRes.value;
		switch(selected)
		{
			case 0:
				QualitySettings.shadowResolution = ShadowResolution.Low;
				break;
			case 1:
				QualitySettings.shadowResolution = ShadowResolution.Medium;
				break;
			case 2:
				QualitySettings.shadowResolution = ShadowResolution.High;
				break;
			case 3:
				QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
				break;				
		}
	}
}
