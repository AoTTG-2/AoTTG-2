using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsController : MonoBehaviour {

	public GeneralGraphics GeneralGraphic;
	public QualitySwitcher QualitySwitcher;
	public ResolutionSwitcher ResolutionSwitcher;
	public Text label;
	
	public void SaveGraphicPlayerPrefs()
	{
		PlayerPrefs.SetInt("QualitySlider", (int)QualitySwitcher.GetComponentInChildren<Slider>().value);
		PlayerPrefs.SetString("Resolution", ResolutionSwitcher.GetComponentInChildren<ResolutionSwitcher>().Resolution);
		PlayerPrefs.SetInt("ScreenMode", ResolutionSwitcher.GetComponentInChildren<ResolutionSwitcher>().ScreenMode);
		PlayerPrefs.SetInt("Shadow", GeneralGraphic.Shadows.value);
		PlayerPrefs.SetInt("TextureQuality", GeneralGraphic.TextureQuality.value);
		PlayerPrefs.SetInt("AntiAliasing", GeneralGraphic.AntiAliasing.value);
		PlayerPrefs.SetInt("ShadowResolution", GeneralGraphic.ShadowRes.value);
		if(GeneralGraphic.SoftParticles.isOn)
		{
			PlayerPrefs.SetInt("SoftParticles", 1);
		}
		else
		{
			PlayerPrefs.SetInt("SoftParticles", 0);
		}
		if(GeneralGraphic.VSync.isOn)
		{
			PlayerPrefs.SetInt("VSync", 1);
		}
		else
		{
			PlayerPrefs.SetInt("VSync", 0);
		}
		
		PlayerPrefs.Save();




		if(PlayerPrefs.HasKey("QualitySlider") && PlayerPrefs.HasKey("Resolution") && PlayerPrefs.HasKey("ScreenMode"))
		{
			label.color = Color.green;
			label.text = "saved to player prefs";
		}
		else
		{
			label.color = Color.red;
			label.text = "error saving player prefs";
		}
	}

	public void LoadGraphicPlayerPrefs()
	{
		
		// Call load for each graphics option
		if(PlayerPrefs.HasKey("QualitySlider"))
		{
			QualitySwitcher.LoadPlayerPrefs();
		}
		if(PlayerPrefs.HasKey("Resolution") && PlayerPrefs.HasKey("ScreenMode"))
		{
			ResolutionSwitcher.LoadPlayerPrefs();
		}
		if(PlayerPrefs.HasKey("Shadow") && PlayerPrefs.HasKey("TextureQuality") && PlayerPrefs.HasKey("AntiAliasing") && PlayerPrefs.HasKey("ShadowResolution") && PlayerPrefs.HasKey("SoftParticles") && PlayerPrefs.HasKey("VSync"))
		{
			GeneralGraphic.LoadPlayerPrefs();
		}
		
		
		label.color = Color.green;
		label.text = "loaded player prefs";
	}

	public void RealTimeUpdateSettings()
	{
		
	}

}
