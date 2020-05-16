using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace Assets.Scripts.UI.InGame
{
	public class GraphicsController : MonoBehaviour {

		public GeneralGraphics GeneralGraphic;
		public QualitySwitcher QualitySwitcher;
		public ResolutionSwitcher ResolutionSwitcher;
		public Text label;
		private Toggle CustomSettings;

		private void Start() {
			CustomSettings = GeneralGraphic.CustomSettings;
			label = GeneralGraphic.CustomSettings.GetComponentInChildren<Text>();
			AdvancedOptions();
			QualitySwitcher.Slider.value = QualitySettings.GetQualityLevel();
		}

		public void SaveGraphicPlayerPrefs()
		{
			// graphics
			var data = new GeneralGraphics.GraphicsData(GeneralGraphic);
			string json = JsonUtility.ToJson(data);
			PlayerPrefs.SetString("GraphicsData", json);

			// quality profile
			var _data = new QualitySwitcher.QualityData(QualitySwitcher);
			string _json = JsonUtility.ToJson(_data);
			PlayerPrefs.SetString("QualityProfile", _json);

			PlayerPrefs.Save();	

			label.color = Color.green;
			label.text = "saved player prefs";
		}
		public void LoadGraphicPlayerPrefs()
		{
			
			var loaded = JsonUtility.FromJson<GeneralGraphics.GraphicsData>(PlayerPrefs.GetString("GraphicsData"));

			GeneralGraphic.CustomSettings.isOn = loaded.CustomSettings;
			GeneralGraphic.TextureQuality.value = loaded.TextureQuality;
			GeneralGraphic.ShadowRes.value = loaded.ShadowRes;
			GeneralGraphic.AntiAliasing.value = loaded.AntiAliasing;
			GeneralGraphic.Shadows.value = loaded.Shadows;
			GeneralGraphic.VSync.isOn = loaded.VSync;
			GeneralGraphic.SoftParticles.isOn = loaded.SoftParticles;
			

			GeneralGraphic.UpdateEverything();

			var _loaded = JsonUtility.FromJson<QualitySwitcher.QualityData>(PlayerPrefs.GetString("QualityProfile"));
			QualitySwitcher.Slider.value = _loaded.Slider;
			
			label.color = Color.green;
			label.text = "loaded player prefs";
		}

		public void AdvancedOptions()
		{
			var selected = CustomSettings.isOn;
			
			if(!selected)
			{
				GeneralGraphic.TextureQuality.interactable = false;
				GeneralGraphic.ShadowRes.interactable = false;
				GeneralGraphic.AntiAliasing.interactable = false;
				GeneralGraphic.Shadows.interactable = false;
				GeneralGraphic.VSync.interactable = false;
				GeneralGraphic.SoftParticles.interactable = false;

				QualitySwitcher.Slider.interactable = true;
				QualitySwitcher.Label.color = Color.white;
				QualitySwitcher.UpdateQuality();
				
			}
			if(selected)
			{
				GeneralGraphic.TextureQuality.interactable = true;
				GeneralGraphic.ShadowRes.interactable = true;
				GeneralGraphic.AntiAliasing.interactable = true;
				GeneralGraphic.Shadows.interactable = true;
				GeneralGraphic.VSync.interactable = true;
				GeneralGraphic.SoftParticles.interactable = true;

				QualitySwitcher.Slider.interactable = false;

				QualitySwitcher.Label.color = Color.gray;
				QualitySettings.SetQualityLevel(6, true);

				GeneralGraphic.UpdateEverything();
				
			}
		}
	}
}