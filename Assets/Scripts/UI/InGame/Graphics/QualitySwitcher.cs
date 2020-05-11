using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
	public class QualitySwitcher : MonoBehaviour {
		public Text Label;
		public Slider Slider;

		private void Update() {
			Label.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
			// this was causing the bug // gameObject.GetComponentInChildren<Slider>().value = QualitySettings.GetQualityLevel();
		}

		public void UpdateQualitySliderValue()
		{
			
			int sValue = (int)Slider.value;
			QualitySettings.SetQualityLevel(sValue, true);
			
		}

		public void LoadPlayerPrefs()
		{
			Slider.value = PlayerPrefs.GetInt("QualitySlider");
		}

		[Serializable]
		public struct Data
		{
			public int Slider;

			public Data(int value)
			{
				this.Slider = value;
			}
		}
	}
}