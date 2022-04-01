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
            if (sValue <= 1)
            {
                Label.color = Color.red;
            }
            else if (sValue <= 2)
            {
                Label.color = Color.yellow;
            }
            else if (sValue>2)
            {
                Label.color = Color.green;
            }
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