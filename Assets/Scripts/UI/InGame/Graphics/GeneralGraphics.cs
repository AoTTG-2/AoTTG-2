using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Graphics;

namespace Assets.Scripts.UI.InGame
{
    public class GeneralGraphics : MonoBehaviour
    {
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

        private void Start()
        {
            textureQuality.onValueChanged.AddListener(delegate
            {
                ChangeTextureQuality(textureQuality);
            });
            shadowRes.onValueChanged.AddListener(delegate
            {
                ChangeShadowResolution(shadowRes);
            });
            antiAliasing.onValueChanged.AddListener(delegate
            {
                ChangeAntiAliasing(antiAliasing);
            });
            shadows.onValueChanged.AddListener(delegate
            {
                ChangeShadows(shadows);
            });
            vSync.onValueChanged.AddListener(delegate
            {
                ChangeVSync(vSync);
            });
            softParticles.onValueChanged.AddListener(delegate
            {
                ChangeSoftParticles(softParticles);
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
            textureQuality.value = data.TextureQuality;
            antiAliasing.value = data.AntiAliasing;
            shadowRes.value = data.ShadowRes;
            shadows.value = data.Shadows;
            softParticles.isOn = data.SoftParticles;
            vSync.isOn = data.VSync;
        }

        private void ChangeShadows(Dropdown dropdown)
        {
            QualitySettings.shadows = (ShadowQuality) dropdown.value;
        }
        private void ChangeTextureQuality(Dropdown dropdown)
        {
            QualitySettings.masterTextureLimit = dropdown.value;
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
        }

        private void ChangeShadowResolution(Dropdown dropdown)
        {
            QualitySettings.shadowResolution = (ShadowResolution) dropdown.value;
        }

        private void ChangeSoftParticles(Toggle toggle)
        {
            QualitySettings.softParticles = toggle.isOn;
        }

        private void ChangeVSync(Toggle toggle)
        {
            QualitySettings.vSyncCount = toggle.isOn ? 1 : 0;
            if (toggle.isOn)
            {
                FramerateController.LockFramerateToRefreshRate();
            }
            else
            {
                //Unlocks the framerate before trying to set it according to the inputfield (in case the input is invalid)
                FramerateController.UnlockFramerate();
                FramerateController.SetFramerateLimit(fpsLimit.text);
            }

        }

        private void ChangeFrameRate(string limit)
        {
            if (!vSync.isOn)
            {
                FramerateController.SetFramerateLimit(limit);
            }
        }

        public void SetInteractable(bool value)
        {
            qualitySwitcher.Slider.interactable = !value;
            TextureQuality.interactable = value;
            AntiAliasing.interactable = value;
            ShadowRes.interactable = value;
            Shadows.interactable = value;
            VSync.interactable = value;
            SoftParticles.interactable = value;
        }

        public void UpdateQualitySettings()
        {
            ChangeCustomQuality(qualitySwitcher);
            ChangeShadowResolution(shadowRes);
            ChangeShadows(shadows);
            ChangeSoftParticles(softParticles);
            ChangeTextureQuality(textureQuality);
            ChangeVSync(vSync);
            ChangeAntiAliasing(antiAliasing);
            ChangeFrameRate(fpsLimit.text);
        }

        private void ChangeCustomQuality(QualitySwitcher qualitySwitcher)
        {
            if (customSettings.isOn)
            {
                QualitySettings.SetQualityLevel((int) QualitySwitcher.Slider.value, true);
            }
        }

        public void Update()
        {
            SetInteractable(customSettings.isOn);
            UpdateUi();
            UpdateQualitySettings();
        }

        public void Update(GraphicsData data)
        {
            SetInteractable(customSettings.isOn);
            UpdateUi(data);
            UpdateQualitySettings();
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
            public string FpsLimit;

            public GraphicsData(int textureQuality, int shadowRes, int antiAliasing, int shadows, bool vSync, bool softParticles, bool customSettings, string fpsLimit)
            {
                this.TextureQuality = textureQuality;
                this.ShadowRes = shadowRes;
                this.AntiAliasing = antiAliasing;
                this.Shadows = shadows;
                this.VSync = vSync;
                this.SoftParticles = softParticles;
                this.CustomSettings = customSettings;
                this.FpsLimit = fpsLimit;
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
                this.FpsLimit = toCopy.fpsLimit.text;
            }
        }
    }
}
