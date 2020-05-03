using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualitySwitcher : MonoBehaviour {

	private int sValue;

	public void UpdateQualitySliderValue()
	{
		
		sValue = (int)gameObject.GetComponentInChildren<Slider>().value;
		QualitySettings.SetQualityLevel(sValue, true);
		
	}
}
