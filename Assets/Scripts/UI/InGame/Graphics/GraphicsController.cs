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

        protected void Start()
		{
            if (PlayerPrefs.HasKey(GDATA) && PlayerPrefs.HasKey(QDATA) && PlayerPrefs.HasKey(FDATA))
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

                var fpsData = new GeneralGraphics.FpsData(GeneralGraphic);
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
				var graphicsData = JsonUtility.FromJson<GeneralGraphics.GraphicsData>(PlayerPrefs.GetString(GDATA));
				var qualityData = JsonUtility.FromJson<QualitySwitcher.QualityData>(PlayerPrefs.GetString(QDATA));
                var fpsData = JsonUtility.FromJson<GeneralGraphics.FpsData>(PlayerPrefs.GetString(FDATA));

                GeneralGraphic.FPSLimit.text = fpsData.Limit;

                GeneralGraphic.QualitySwitcher.Slider.value = qualityData.Slider;

				if (graphicsData.CustomSettings)
				{
                    GeneralGraphic.Update(graphicsData);
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
            PlayerPrefs.DeleteKey(FDATA);
		}
	}
}