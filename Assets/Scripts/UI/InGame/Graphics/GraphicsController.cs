using UnityEngine;
using UnityEngine.UI;
using System;

namespace Assets.Scripts.UI.InGame
{
	public class GraphicsController : MonoBehaviour {

        public GeneralGraphics GeneralGraphic;
		public ResolutionSwitcher ResolutionSwitcher;
		public Text label;

        public const string GDATA = "GraphicsData";
		public const string QDATA = "QualityProfile";

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
				var data1 = new GeneralGraphics.GraphicsData(GeneralGraphic);
				string json1 = JsonUtility.ToJson(data1);
				PlayerPrefs.SetString(GDATA, json1);

				// quality profile
				var data2 = new QualitySwitcher.QualityData(GeneralGraphic.QualitySwitcher);
				string json2 = JsonUtility.ToJson(data2);
				PlayerPrefs.SetString(QDATA, json2);

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