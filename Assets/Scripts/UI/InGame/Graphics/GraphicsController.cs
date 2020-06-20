using UnityEngine;
using UnityEngine.UI;
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
		public const string GDATA = "GraphicsData";
		public const string QDATA = "QualityProfile";
		public const string FDATA = "FPSLimit";

		private void Start()
		{
			if(PlayerPrefs.HasKey(GDATA) && PlayerPrefs.HasKey(QDATA) && PlayerPrefs.HasKey(FDATA))
			{
				LoadGraphicPlayerPrefs();
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
				PlayerPrefs.SetString(GDATA, json1);

				// quality profile
				var data2 = new QualitySwitcher.QualityData(QualitySwitcher);
				string json2 = JsonUtility.ToJson(data2);
				PlayerPrefs.SetString(QDATA, json2);

				// fps limit
				var data3 = new FPSLimiter.FPSData(FPSLimiter);
				string json3 = JsonUtility.ToJson(data3);
				PlayerPrefs.SetString(FDATA, json3);

				PlayerPrefs.Save();

				label.color = Color.green;
				label.text = "saved player prefs";
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.ToString());
				label.color = Color.red;
				label.text = "error saving player prefs";
			}
			
		}

		public void LoadGraphicPlayerPrefs()
		{
			try
			{
				var loaded1 = JsonUtility.FromJson<GeneralGraphics.GraphicsData>(PlayerPrefs.GetString(GDATA));
				var loaded2 = JsonUtility.FromJson<QualitySwitcher.QualityData>(PlayerPrefs.GetString(QDATA));
				var loaded3 = JsonUtility.FromJson<FPSLimiter.FPSData>(PlayerPrefs.GetString(FDATA));

				QualitySwitcher.Slider.value = loaded2.Slider;

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

				GeneralGraphic.UpdateObjects();

				label.color = Color.green;
				label.text = "loaded player prefs";
			}
			catch(NullReferenceException ex)
			{
				Debug.LogError("Error loading player prefs");

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
			GeneralGraphic.UpdateObjects();
			GeneralGraphic.UpdateEverything();
		}

		public void DeletePrefs()
		{
			PlayerPrefs.DeleteKey(GDATA);
			PlayerPrefs.DeleteKey(QDATA);
			PlayerPrefs.DeleteKey(FDATA);
		}

		private void ChangeObjectValues()
		{
			GeneralGraphic.TextureQuality.value = QualitySettings.masterTextureLimit;
			GeneralGraphic.AntiAliasing.value = (int)QualitySettings.antiAliasing;
			GeneralGraphic.ShadowRes.value = (int)QualitySettings.shadowResolution;
			GeneralGraphic.Shadows.value = (int)QualitySettings.shadows;
			GeneralGraphic.SoftParticles.isOn = QualitySettings.softParticles;

			if(QualitySettings.antiAliasing == (int)GraphicsEnums.AntiAliasing._8xMultiSampling)
			{
				GeneralGraphic.AntiAliasing.value = 3;
			}
			else if (QualitySettings.antiAliasing == (int)GraphicsEnums.AntiAliasing._4xMultiSampling)
			{
				GeneralGraphic.AntiAliasing.value = 2;
			}
			else if (QualitySettings.antiAliasing == (int)GraphicsEnums.AntiAliasing._2xMultiSampling)
			{
				GeneralGraphic.AntiAliasing.value = 1;
			}
			else
			{
				GeneralGraphic.AntiAliasing.value = 0;
			}

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