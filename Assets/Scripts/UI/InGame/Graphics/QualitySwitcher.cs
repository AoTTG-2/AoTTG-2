using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
	public class QualitySwitcher : UiElement {
		public Text Label;
		public Slider Slider;

        private void Start()
        {
            Slider.onValueChanged.AddListener(delegate
            {
                UpdateQuality(this);
            });
        }

        public void UpdateQuality(QualitySwitcher qualitySwitcher)
		{
			int sValue = (int) qualitySwitcher.Slider.value;
			QualitySettings.SetQualityLevel(sValue, true);
            Debug.Log($"QualityLevel set to {QualitySettings.GetQualityLevel()}");
            qualitySwitcher.Label.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
			FindObjectOfType<GeneralGraphics>().UpdateUi();
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