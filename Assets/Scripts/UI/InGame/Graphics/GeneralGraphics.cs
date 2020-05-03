using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class GeneralGraphics : MonoBehaviour {

	public Dropdown TextureQuality;
	public Dropdown ShadowResolution;
	public Dropdown AntiAliasing;
	public Dropdown Shadows;
	public Toggle SoftParticles;

	private void OnEnable() {
		
	}

	public void ChangeShadows()
	{
		var label = Shadows.GetComponentInChildren<Text>().text;
		if(label.Equals("All"))
		{
			QualitySettings.shadows = ShadowQuality.All;
		}
		if(label.Equals("Hard Shadows"))
		{
			QualitySettings.shadows = ShadowQuality.HardOnly;
		}
		if(label.Equals("Disable"))
		{
			QualitySettings.shadows = ShadowQuality.Disable;
		}
	}
}
