using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
	public class QualitySwitcher : MonoBehaviour {
		public Text Label;
		public Slider Slider;

		private void Update() {
			
		}

		public void UpdateQualitySliderValue()
		{
			Label.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
			int sValue = (int)Slider.value;
			QualitySettings.SetQualityLevel(sValue, true);
			
		}

		public void LoadPlayerPrefs()
		{
			Slider.value = PlayerPrefs.GetInt("QualitySlider");
			Label.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
		}

		[Serializable]
		public struct QualityData
		{
			public int Slider;

			public QualityData(QualitySwitcher toCopy)
			{
				this.Slider = (int)toCopy.Slider.value;
			}
		}
	}
}