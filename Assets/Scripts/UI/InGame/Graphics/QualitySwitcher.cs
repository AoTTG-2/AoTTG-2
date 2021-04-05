using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
	public class QualitySwitcher : MonoBehaviour {
		public Text Label;
		public Slider Slider;

		public void UpdateQuality()
		{
			int sValue = (int)Slider.value;
			QualitySettings.SetQualityLevel(sValue, true);
			Label.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
			FindObjectOfType<GeneralGraphics>().UpdateObjects();
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