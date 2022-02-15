using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Graphics;

namespace Assets.Scripts.UI.InGame
{
    public class GeneralGraphics : MonoBehaviour
    {
        private const int CUSTOM = 6;

        [SerializeField] private Dropdown textureQuality;
        [SerializeField] private Dropdown shadowRes;
        [SerializeField] private Dropdown antiAliasing;
        [SerializeField] private Dropdown shadows;
        [SerializeField] private Toggle vSync;
        [SerializeField] private Toggle softParticles;
        [SerializeField] private Toggle customSettings;
        [SerializeField] private QualitySwitcher qualitySwitcher;
        [SerializeField] private InputField fpsLimit;

        public InputField FPSLimit
        {
            get { return fpsLimit; }
            set { fpsLimit = value; }
        }

        public QualitySwitcher QualitySwitcher
        {
            get { return qualitySwitcher; }
            set { qualitySwitcher = value; }
        }

        public Dropdown TextureQuality
        {
            get { return textureQuality; }
            set { textureQuality = value; }
        }
        public Dropdown ShadowRes
        {
            get { return shadowRes; }
            set { shadowRes = value; }
        }
        public Dropdown AntiAliasing
        {
            get { return antiAliasing; }
            set { antiAliasing = value; }
        }
        public Dropdown Shadows
        {
            get { return shadows; }
            set { shadows = value; }
        }
        public Toggle VSync
        {
            get { return vSync; }
            set { vSync = value; }
        }
        public Toggle SoftParticles
        {
            get { return softParticles; }
            set { softParticles = value; }
        }

        public Toggle CustomSettings
        {
            get { return customSettings; }
            set { customSettings = value; }
        }

        protected void Start()
        {
            textureQuality.onValueChanged.AddListener(delegate
            {
                ChangeTextureQuality(textureQuality);
            });
            vSync.onValueChanged.AddListener(delegate
            {
                ChangeVSync(vSync);
            });
            fpsLimit.onEndEdit.AddListener(ChangeFrameRate);
            customSettings.onValueChanged.AddListener(SetInteractable);
        }

        public void UpdateUi()
        {
            textureQuality.value = QualitySettings.masterTextureLimit;
            shadowRes.value = (int) QualitySettings.shadowResolution;
            shadows.value = (int) QualitySettings.shadows;
            softParticles.isOn = QualitySettings.softParticles;
            antiAliasing.value = QualitySettings.antiAliasing switch
            {
                (int) GraphicsEnums.AntiAliasing._8xMultiSampling => 3,
                (int) GraphicsEnums.AntiAliasing._4xMultiSampling => 2,
                (int) GraphicsEnums.AntiAliasing._2xMultiSampling => 1,
                _ => 0,
            };
            vSync.isOn = QualitySettings.vSyncCount != 0;
        }

        private void UpdateUi(GraphicsData data)
        {
            customSettings.isOn = data.CustomSettings;
            textureQuality.value = data.TextureQuality;
            antiAliasing.value = data.AntiAliasing;
            shadowRes.value = data.ShadowRes;
            shadows.value = data.Shadows;
            softParticles.isOn = data.SoftParticles;
            vSync.isOn = data.VSync;
        }

        private void ChangeShadows(Dropdown dropdown)
        {
            QualitySettings.shadows = (ShadowQuality)dropdown.value;
            Debug.Log($"Shadows set to {QualitySettings.shadows}");
        }
        private void ChangeTextureQuality(Dropdown dropdown)
        {
            QualitySettings.masterTextureLimit = dropdown.value;
            Debug.Log($"Textures set to {QualitySettings.masterTextureLimit}");
        }

        private void ChangeAntiAliasing(Dropdown dropdown)
        {
            QualitySettings.antiAliasing = dropdown.value switch
            {
                3 => (int) GraphicsEnums.AntiAliasing._8xMultiSampling,
                2 => (int) GraphicsEnums.AntiAliasing._4xMultiSampling,
                1 => (int) GraphicsEnums.AntiAliasing._2xMultiSampling,
                _ => (int) GraphicsEnums.AntiAliasing.Disabled,
            };
            Debug.Log($"AntiAliasing set to {QualitySettings.antiAliasing}");
        }

        private void ChangeShadowResolution(Dropdown dropdown)
        {
            QualitySettings.shadowResolution = (ShadowResolution)dropdown.value;
            Debug.Log($"ShadowResolution set to {QualitySettings.shadowResolution}");
        }

        private void ChangeSoftParticles(Toggle toggle)
        {
            QualitySettings.softParticles = toggle.isOn;
            Debug.Log($"SoftParticles set to {QualitySettings.softParticles}");
        }

        private void ChangeVSync(Toggle toggle)
        {
            //Vsync overrides Application.targetFramerate
            QualitySettings.vSyncCount = toggle.isOn ? 1 : 0;
            Debug.Log($"Vsync set to {QualitySettings.vSyncCount}");
        }

        private void ChangeFrameRate(string limit)
        {
            var parsed = int.TryParse(limit, out int frameRate);

            if (parsed)
            {
                FramerateController.SetFramerateLimit(frameRate);
            }
            else
            {
                FramerateController.UnlockFramerate();
            }
        }

        public void SetInteractable(bool value)
        {
            //hardcoded false are due to some settings not being decoupled from the set QualityLevel
            //https://forum.unity.com/threads/change-shadow-resolution-from-script.784793/page-2
            qualitySwitcher.Slider.interactable = true;
            textureQuality.interactable = value;
            antiAliasing.interactable = false;
            shadowRes.interactable = false;
            shadows.interactable = false;
            vSync.interactable = value;
            softParticles.interactable = false;
        }

        public void UpdateGraphicSettings()
        {
            UpdateUi();
            SetInteractable(customSettings.isOn);
        }

        public void UpdateGraphicSettings(GraphicsData data)
        {
            UpdateUi(data);
            SetInteractable(customSettings.isOn);
        }

        // This helped with showing the objects in the inspector https://forum.unity.com/threads/c-nested-class-and-inspector.18582/
        // Good for understanding how to setup data for JSON Serialization https://medium.com/@antifreemium/extending-playerprefs-with-json-3c227a5876a5
        [Serializable]
        public struct GraphicsData
        {
            public int TextureQuality;
            public int ShadowRes;
            public int AntiAliasing;
            public int Shadows;
            public bool VSync;
            public bool SoftParticles;
            public bool CustomSettings;

            public GraphicsData(int textureQuality, int shadowRes, int antiAliasing, int shadows, bool vSync, bool softParticles, bool customSettings)
            {
                this.TextureQuality = textureQuality;
                this.ShadowRes = shadowRes;
                this.AntiAliasing = antiAliasing;
                this.Shadows = shadows;
                this.VSync = vSync;
                this.SoftParticles = softParticles;
                this.CustomSettings = customSettings;
            }
            public GraphicsData(GeneralGraphics toCopy)
            {
                this.TextureQuality = toCopy.textureQuality.value;
                this.ShadowRes = toCopy.shadowRes.value;
                this.AntiAliasing = toCopy.antiAliasing.value;
                this.Shadows = toCopy.shadows.value;
                this.VSync = toCopy.vSync.isOn;
                this.SoftParticles = toCopy.softParticles.isOn;
                this.CustomSettings = toCopy.customSettings.isOn;
            }
        }

        public struct FpsData
        {
            public string Limit;

            public FpsData(GeneralGraphics toCopy)
            {
                this.Limit = toCopy.FPSLimit.text;
            }
        }
    }
}
