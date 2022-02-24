using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Graphics;

namespace Assets.Scripts.UI.InGame
{
    public class GeneralGraphics : MonoBehaviour
    {
        [SerializeField] public InputField FpsLimit;
        [SerializeField] public QualitySwitcher QualitySwitcher;
        [SerializeField] public Dropdown TextureQuality;
        [SerializeField] public Dropdown ShadowRes;
        [SerializeField] public Dropdown AntiAliasing;
        [SerializeField] public Dropdown Shadows;
        [SerializeField] public Toggle VSync;
        [SerializeField] public Toggle SoftParticles;
        [SerializeField] public Toggle CustomSettings;

        private void Start()
        {
            TextureQuality.onValueChanged.AddListener(delegate
            {
                ChangeTextureQuality(TextureQuality);
            });
            VSync.onValueChanged.AddListener(delegate
            {
                ChangeVSync(VSync);
            });
            FpsLimit.onEndEdit.AddListener(ChangeFrameRate);
            CustomSettings.onValueChanged.AddListener(SetInteractable);
        }

        public void UpdateUi()
        {
            TextureQuality.value = QualitySettings.masterTextureLimit;
            ShadowRes.value = (int) QualitySettings.shadowResolution;
            Shadows.value = (int) QualitySettings.shadows;
            SoftParticles.isOn = QualitySettings.softParticles;
            AntiAliasing.value = QualitySettings.antiAliasing switch
            {
                (int) GraphicsEnums.AntiAliasing._8xMultiSampling => 3,
                (int) GraphicsEnums.AntiAliasing._4xMultiSampling => 2,
                (int) GraphicsEnums.AntiAliasing._2xMultiSampling => 1,
                _ => 0,
            };
            VSync.isOn = QualitySettings.vSyncCount != 0;
        }

        private void UpdateUi(GraphicsData data)
        {
            CustomSettings.isOn = data.CustomSettings;
            TextureQuality.value = data.TextureQuality;
            AntiAliasing.value = data.AntiAliasing;
            ShadowRes.value = data.ShadowRes;
            Shadows.value = data.Shadows;
            SoftParticles.isOn = data.SoftParticles;
            VSync.isOn = data.VSync;
        }

        private void ChangeShadows(Dropdown dropdown)
        {
            QualitySettings.shadows = (ShadowQuality) dropdown.value;
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
            QualitySettings.shadowResolution = (ShadowResolution) dropdown.value;
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
            if (int.TryParse(limit, out var frameRate))
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
            QualitySwitcher.Slider.interactable = true;
            TextureQuality.interactable = value;
            AntiAliasing.interactable = false;
            ShadowRes.interactable = false;
            Shadows.interactable = false;
            VSync.interactable = value;
            SoftParticles.interactable = false;
        }

        public void UpdateGraphicSettings()
        {
            UpdateUi();
            SetInteractable(CustomSettings.isOn);
        }

        public void UpdateGraphicSettings(GraphicsData data)
        {
            UpdateUi(data);
            SetInteractable(CustomSettings.isOn);
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
                this.TextureQuality = toCopy.TextureQuality.value;
                this.ShadowRes = toCopy.ShadowRes.value;
                this.AntiAliasing = toCopy.AntiAliasing.value;
                this.Shadows = toCopy.Shadows.value;
                this.VSync = toCopy.VSync.isOn;
                this.SoftParticles = toCopy.SoftParticles.isOn;
                this.CustomSettings = toCopy.CustomSettings.isOn;
            }
        }

        public struct FpsData
        {
            public string Limit;

            public FpsData(GeneralGraphics toCopy)
            {
                this.Limit = toCopy.FpsLimit.text;
            }
        }
    }
}
