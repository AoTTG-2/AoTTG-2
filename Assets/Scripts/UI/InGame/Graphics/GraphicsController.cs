using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsController : MonoBehaviour {

	public GeneralGraphics GeneralGraphic;
	public QualitySwitcher QualitySwitcher;
	public ResolutionSwitcher ResolutionSwitcher;
	public Text label;
	public Toggle CustomSettings;

	private void Update() {
		if(!CustomSettings.isOn)
		{
			GeneralGraphic.TextureQuality.interactable = false;
			GeneralGraphic.shadowRes.interactable = false;
			GeneralGraphic.antiAliasing.interactable = false;
			GeneralGraphic.shadows.interactable = false;
			GeneralGraphic.VSync.interactable = false;
			GeneralGraphic.SoftParticles.interactable = false;

			QualitySwitcher.GetComponentInChildren<Slider>().interactable = true;
		}
	}
	public void SaveGraphicPlayerPrefs()
	{
		PlayerPrefs.SetInt("Quality", QualitySettings.GetQualityLevel());
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
		if(CustomSettings.isOn)
		{
			PlayerPrefs.SetInt("CustomSettings", 1);
		}
		else
		{
			PlayerPrefs.SetInt("CustomSettings", 0);
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
		if(PlayerPrefs.HasKey("Quality"))
		{
			QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"), true);
		}
		if(PlayerPrefs.HasKey("CustomSettings"))
		{
			if(PlayerPrefs.GetInt("CustomSettings") == 1)
			{
				CustomSettings.isOn = true;
			}
			else
			{
				CustomSettings.isOn = false;
			}
		}
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

	public void AdvancedOptions()
	{
		var selected = CustomSettings.isOn;
		var temp = QualitySettings.GetQualityLevel();
		
		if(!selected)
		{
			QualitySettings.SetQualityLevel(temp);
		}
		if(selected)
		{
			QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1);
			GeneralGraphic.TextureQuality.interactable = true;
			GeneralGraphic.shadowRes.interactable = true;
			GeneralGraphic.antiAliasing.interactable = true;
			GeneralGraphic.shadows.interactable = true;
			GeneralGraphic.VSync.interactable = true;
			GeneralGraphic.SoftParticles.interactable = true;

			QualitySwitcher.GetComponentInChildren<Slider>().interactable = false;
		}
	}
}
