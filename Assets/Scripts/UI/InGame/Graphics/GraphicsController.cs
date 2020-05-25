using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System;

namespace Assets.Scripts.UI.InGame
{
	public class GraphicsController : MonoBehaviour {

		public GeneralGraphics GeneralGraphic;
		public QualitySwitcher QualitySwitcher;
		public ResolutionSwitcher ResolutionSwitcher;
		public FPSLimiter FPSLimiter;
		public Text label;
		public Toggle CustomSettings;

		// Index of the custom quality level
		public const int CUSTOM = 6;


		private void Start()
		{
			if(PlayerPrefs.HasKey("GraphicsData") && PlayerPrefs.HasKey("QualityProfile") && PlayerPrefs.HasKey("FPSLimit"))
			{
				LoadGraphicPlayerPrefs();
				Debug.Log("Has Prefs");
			}
			else
			{
				QualitySwitcher.Slider.value = QualitySettings.GetQualityLevel();
				QualitySwitcher.UpdateQuality();
				ChangeObjectValues();
			}

			AdvancedOptions();
		}

		public void SaveGraphicPlayerPrefs()
		{
			try
			{
				var data1 = new GeneralGraphics.GraphicsData(GeneralGraphic);
				string json1 = JsonUtility.ToJson(data1);
				PlayerPrefs.SetString("GraphicsData", json1);

				// quality profile
				var data2 = new QualitySwitcher.QualityData(QualitySwitcher);
				string json2 = JsonUtility.ToJson(data2);
				PlayerPrefs.SetString("QualityProfile", json2);

				// fps limit
				var data3 = new FPSLimiter.FPSData(FPSLimiter);
				string json3 = JsonUtility.ToJson(data3);
				PlayerPrefs.SetString("FPSLimit", json3);

				PlayerPrefs.Save();

				label.color = Color.green;
				label.text = "saved player prefs";
			}
			catch(Exception ex)
			{
				Debug.LogError(ex.ToString());
				label.color = Color.red;
				label.text = "error saving player prefs";
			}
			// graphics
			
		}

		// NULL REFERENCE HAPPENES HERE
		public void LoadGraphicPlayerPrefs()
		{
			try
			{
				var loaded1 = JsonUtility.FromJson<GeneralGraphics.GraphicsData>(PlayerPrefs.GetString("GraphicsData"));
				var loaded2 = JsonUtility.FromJson<QualitySwitcher.QualityData>(PlayerPrefs.GetString("QualityProfile"));
				var loaded3 = JsonUtility.FromJson<FPSLimiter.FPSData>(PlayerPrefs.GetString("FPSLimit"));

				QualitySwitcher.Slider.value = loaded2.Slider;

				FPSLimiter.FPSDropdown.value = loaded3.dropdown;
				FPSLimiter.FPSLimit.text = loaded3.field;
				FPSLimiter.SetFPSLimit();

				

				GeneralGraphic.CustomSettings.isOn = loaded1.CustomSettings;
				AdvancedOptions();

				if (loaded1.CustomSettings)
				{
					GeneralGraphic.TextureQuality.value = loaded1.TextureQuality;
					GeneralGraphic.ShadowRes.value = loaded1.ShadowRes;
					GeneralGraphic.AntiAliasing.value = loaded1.AntiAliasing;
					GeneralGraphic.Shadows.value = loaded1.Shadows;
					GeneralGraphic.VSync.isOn = loaded1.VSync;
					GeneralGraphic.SoftParticles.isOn = loaded1.SoftParticles;
					GeneralGraphic.UpdateEverything();
				}

				label.color = Color.green;
				label.text = "loaded player prefs";
			}
			catch(Exception ex)
			{
				Debug.LogError(ex.ToString());

				label.color = Color.red;
				label.text = "error loading player prefs";
			}
		}

		public void AdvancedOptions()
		{
			if(CustomSettings.isOn)
			{
				QualitySettings.SetQualityLevel(CUSTOM, true);
				QualitySwitcher.Slider.interactable = false;
			}
			else
			{
				QualitySettings.SetQualityLevel((int)QualitySwitcher.Slider.value, true);
				QualitySwitcher.Slider.interactable = true;
			}
			GeneralGraphic.SetInteractable(CustomSettings.isOn);
		}

		public void DeletePrefs()
		{
			PlayerPrefs.DeleteAll();
		}

		private void ChangeObjectValues()
		{
			GeneralGraphic.TextureQuality.value = QualitySettings.masterTextureLimit;
			GeneralGraphic.AntiAliasing.value = QualitySettings.antiAliasing;
			GeneralGraphic.ShadowRes.value = (int)QualitySettings.shadowResolution;
			GeneralGraphic.Shadows.value = (int)QualitySettings.shadows;
			GeneralGraphic.SoftParticles.isOn = QualitySettings.softParticles;
			if (QualitySettings.vSyncCount == 0)
			{
				GeneralGraphic.VSync.isOn = false;
			}
			else
			{
				GeneralGraphic.VSync.isOn = true;
			}
		}
	}
}