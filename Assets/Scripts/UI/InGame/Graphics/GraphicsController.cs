using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsController : MonoBehaviour {

	public GeneralGraphics GeneralGraphic;
	public QualitySwitcher QualitySwitcher;
	public ResolutionSwitcher ResolutionSwitcher;
	public Text label;
	
	public void SaveGraphicPlayerPrefs()
	{
		PlayerPrefs.SetInt("QualitySlider", (int)QualitySwitcher.GetComponentInChildren<Slider>().value);
		PlayerPrefs.SetString("Resolution", ResolutionSwitcher.GetComponentInChildren<ResolutionSwitcher>().Resolution);
		PlayerPrefs.SetInt("ScreenMode", ResolutionSwitcher.GetComponentInChildren<ResolutionSwitcher>().ScreenMode);
		PlayerPrefs.Save();
		if(PlayerPrefs.HasKey("QualitySlider") && PlayerPrefs.HasKey("Resolution") && PlayerPrefs.HasKey("ScreenMode"))
		{
			label.color = Color.green;
			label.text = "saved to player prefs";
		}
		else
		{
			label.color = Color.red;
			label.text = "error saving player prefs";
		}
	}

	public void LoadGraphicPlayerPrefs()
	{
		// Call load for each graphics option
		ResolutionSwitcher.LoadPlayerPrefs(PlayerPrefs.GetString("Resolution"), PlayerPrefs.GetInt("ScreenMode"));
		QualitySwitcher.LoadPlayerPrefs(PlayerPrefs.GetInt("QualitySlider"));
		label.color = Color.green;
		label.text = "loaded player prefs";
	}

}
