using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class GraphicsController : MonoBehaviour
    {
        public Button SaveGraphicPrefs;
        public Button LoadGraphicPrefs;
        public Button DeleteGraphicPrefs;
        public Text PrefsLabel;

        public GeneralGraphics GeneralGraphic;
        public ResolutionSwitcher ResolutionSwitcher;

        public const string GDATA = "GraphicsData";
        public const string QDATA = "QualityProfile";
        public const string FDATA = "FPSLimit";

        private void Start()
        {
            SaveGraphicPrefs.onClick.AddListener(SaveGraphicPlayerPrefs);
            LoadGraphicPrefs.onClick.AddListener(LoadGraphicPlayerPrefs);
            DeleteGraphicPrefs.onClick.AddListener(DeletePrefs);

            GeneralGraphic.UpdateGraphicSettings();
            LoadGraphicPlayerPrefs();
        }

        private void SaveGraphicPlayerPrefs()
        {
            try
            {
                var graphicsData = new GeneralGraphics.GraphicsData(GeneralGraphic);
                SetPlayerPreference(GDATA, graphicsData);

                var qualityData = new QualitySwitcher.QualityData(GeneralGraphic.QualitySwitcher);
                SetPlayerPreference(QDATA, qualityData);

                var fpsData = new GeneralGraphics.FpsData(GeneralGraphic);
                SetPlayerPreference(FDATA, fpsData);

                PlayerPrefs.Save();

                SetPrefsLabelSuccess("Saved player prefs");
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                SetPrefsLabelError("Error saving player prefs");
            }
        }

        private void LoadGraphicPlayerPrefs()
        {
            try
            {
                if (!PrefsExist())
                {
                    SetPrefsLabelError("No saved player prefs exist");
                    return;
                }

                var graphicsData = JsonUtility.FromJson<GeneralGraphics.GraphicsData>(PlayerPrefs.GetString(GDATA));
                var qualityData = JsonUtility.FromJson<QualitySwitcher.QualityData>(PlayerPrefs.GetString(QDATA));
                var fpsData = JsonUtility.FromJson<GeneralGraphics.FpsData>(PlayerPrefs.GetString(FDATA));

                GeneralGraphic.FpsLimit.text = fpsData.Limit;

                GeneralGraphic.QualitySwitcher.Slider.value = qualityData.Slider;

                GeneralGraphic.UpdateGraphicSettings(graphicsData);

                SetPrefsLabelSuccess("Loaded player prefs");
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                SetPrefsLabelError("Error loading player prefs");
            }
        }

        private void DeletePrefs()
        {
            PlayerPrefs.DeleteKey(GDATA);
            PlayerPrefs.DeleteKey(QDATA);
            PlayerPrefs.DeleteKey(FDATA);

            SetPrefsLabelWarning("Deleted player prefs");
        }

        private bool PrefsExist()
        {
            return PlayerPrefs.HasKey(GDATA) && PlayerPrefs.HasKey(QDATA) && PlayerPrefs.HasKey(FDATA);
        }

        private void SetPlayerPreference<T>(string key, T obj)
        {
            var serializedQualityData = JsonUtility.ToJson(obj);
            PlayerPrefs.SetString(key, serializedQualityData);
        }

        private void SetPrefsLabelWarning(string message)
        {
            SetPrefsLabel(new Color(255, 140, 0), message);
        }

        private void SetPrefsLabelError(string message)
        {
            SetPrefsLabel(Color.red, message);
        }

        private void SetPrefsLabelSuccess(string message)
        {
            SetPrefsLabel(Color.green, message);
        }

        private void SetPrefsLabel(Color color, string message)
        {
            PrefsLabel.color = color;
            PrefsLabel.text = message;
        }
    }
}