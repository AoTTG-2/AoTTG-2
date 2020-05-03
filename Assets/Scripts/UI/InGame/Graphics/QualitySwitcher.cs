using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualitySwitcher : MonoBehaviour {
	public Text label;

	private void Update() {
		label.text = QualitySettings.currentLevel.ToString();
		gameObject.GetComponentInChildren<Slider>().value = QualitySettings.GetQualityLevel();
	}

	public void UpdateQualitySliderValue()
	{
		
		int sValue = (int)gameObject.GetComponentInChildren<Slider>().value;
		QualitySettings.SetQualityLevel(sValue, true);

		
	}

	public void LoadPlayerPrefs(int value)
	{
		gameObject.GetComponentInChildren<Slider>().value = value;
	}
}
