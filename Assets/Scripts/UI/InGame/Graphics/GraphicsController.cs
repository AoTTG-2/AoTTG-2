using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class GraphicsController : MonoBehaviour {

        public GeneralGraphics GeneralGraphic;
		public ResolutionSwitcher ResolutionSwitcher;
		public Text label;

        public const string GDATA = "GraphicsData";
		public const string QDATA = "QualityProfile";
        public const string FDATA = "FPSLimit";

        private void Start()
		{
            if (PlayerPrefs.HasKey(GDATA) && PlayerPrefs.HasKey(QDATA))
			{
				LoadGraphicPlayerPrefs();
			}

			GeneralGraphic.Update();
			
		}

		public void SaveGraphicPlayerPrefs()
		{
			try
			{
				var graphicsData = new GeneralGraphics.GraphicsData(GeneralGraphic);
				var serializedGraphicsData = JsonUtility.ToJson(graphicsData);
				PlayerPrefs.SetString(GDATA, serializedGraphicsData);

				var qualityData = new QualitySwitcher.QualityData(GeneralGraphic.QualitySwitcher);
				string serializedQualityData = JsonUtility.ToJson(qualityData);
				PlayerPrefs.SetString(QDATA, serializedQualityData);

                var fpsData = new GeneralGraphics.FPSData(GeneralGraphic);
                var serializedFpsData = JsonUtility.ToJson(fpsData);
                PlayerPrefs.SetString(FDATA, serializedFpsData);

                PlayerPrefs.Save();

				label.color = Color.green;
				label.text = "saved player prefs";
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.ToString());
				label.color = Color.red;
				label.text = "error saving player prefs";
			}
		}

		public void LoadGraphicPlayerPrefs()
		{
			try
			{
				var loaded1 = JsonUtility.FromJson<GeneralGraphics.GraphicsData>(PlayerPrefs.GetString(GDATA));
				var loaded2 = JsonUtility.FromJson<QualitySwitcher.QualityData>(PlayerPrefs.GetString(QDATA));

                GeneralGraphic.QualitySwitcher.Slider.value = loaded2.Slider;

				if (loaded1.CustomSettings)
				{
                    GeneralGraphic.Update(loaded1);
                }
                else
                {
                    GeneralGraphic.Update();
                }

                label.color = Color.green;
				label.text = "loaded player prefs";
			}
			catch(NullReferenceException ex)
			{
				Debug.LogError("Error loading player prefs");

				label.color = Color.red;
				label.text = "error loading player prefs";
			}
		}

		public void DeletePrefs()
		{
			PlayerPrefs.DeleteKey(GDATA);
			PlayerPrefs.DeleteKey(QDATA);
		}
	}
}