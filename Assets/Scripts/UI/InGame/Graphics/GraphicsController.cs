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
		public Toggle CustomSettings;


		private void Update() {
			
			if(!CustomSettings.isOn)
			{
				GeneralGraphic.TextureQuality.interactable = false;
				GeneralGraphic.ShadowRes.interactable = false;
				GeneralGraphic.AntiAliasing.interactable = false;
				GeneralGraphic.Shadows.interactable = false;
				GeneralGraphic.VSync.interactable = false;
				GeneralGraphic.SoftParticles.interactable = false;

				QualitySwitcher.Slider.interactable = true;
				
				
			}
			else
			{
				QualitySwitcher.Label.text = "Custom";
			}

		}

		public void SaveGraphicPlayerPrefs()
		{
			// graphics
			var data = new GeneralGraphics.Data(GeneralGraphic.TextureQuality.value, GeneralGraphic.ShadowRes.value, GeneralGraphic.AntiAliasing.value, GeneralGraphic.Shadows.value, GeneralGraphic.VSync.isOn, GeneralGraphic.SoftParticles.isOn, CustomSettings.isOn);
			string json = JsonUtility.ToJson(data);
			PlayerPrefs.SetString("GraphicsData", json);

			// quality profile
			var _data = new QualitySwitcher.Data((int)QualitySwitcher.Slider.value);
			string _json = JsonUtility.ToJson(_data);
			PlayerPrefs.SetString("QualityProfile", _json);

			PlayerPrefs.Save();	

			label.color = Color.green;
			label.text = "saved player prefs";
		}
		public void LoadGraphicPlayerPrefs()
		{
			
			var loaded = JsonUtility.FromJson<GeneralGraphics.Data>(PlayerPrefs.GetString("GraphicsData"));
				
			GeneralGraphic.TextureQuality.value = loaded.TextureQuality;
			GeneralGraphic.ShadowRes.value = loaded.ShadowRes;
			GeneralGraphic.AntiAliasing.value = loaded.AntiAliasing;
			GeneralGraphic.Shadows.value = loaded.Shadows;
			GeneralGraphic.VSync.isOn = loaded.VSync;
			GeneralGraphic.SoftParticles.isOn = loaded.SoftParticles;
			GeneralGraphic.CustomSettings.isOn = loaded.CustomSettings;

			GeneralGraphic.UpdateEverything();

			var _loaded = JsonUtility.FromJson<QualitySwitcher.Data>(PlayerPrefs.GetString("QualityProfile"));
			QualitySwitcher.Slider.value = _loaded.Slider;
			
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
				GeneralGraphic.ShadowRes.interactable = true;
				GeneralGraphic.AntiAliasing.interactable = true;
				GeneralGraphic.Shadows.interactable = true;
				GeneralGraphic.VSync.interactable = true;
				GeneralGraphic.SoftParticles.interactable = true;

				QualitySwitcher.GetComponentInChildren<Slider>().interactable = false;
			}
		}
	}
}