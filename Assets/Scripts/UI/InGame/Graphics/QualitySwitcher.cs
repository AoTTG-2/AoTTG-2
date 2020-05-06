using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualitySwitcher : MonoBehaviour {
	public Text label;

	private void Update() {
		label.text = QualitySettings.currentLevel.ToString();
		gameObject.GetComponentInChildren<Slider>().value = QualitySettings.GetQualityLevel();
		Debug.Log(gameObject.GetComponentInChildren<Slider>().value);


	}

	public void UpdateQualitySliderValue()
	{
		
		int sValue = (int)gameObject.GetComponentInChildren<Slider>().value;
		QualitySettings.SetQualityLevel(sValue, true);

		
	}

	public void LoadPlayerPrefs()
	{
		gameObject.GetComponentInChildren<Slider>().value = PlayerPrefs.GetInt("QualitySlider");
	}
}
