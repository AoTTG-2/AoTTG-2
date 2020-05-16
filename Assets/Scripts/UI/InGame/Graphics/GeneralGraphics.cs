using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Newtonsoft.Json;
namespace Assets.Scripts.UI.InGame
{
	public class GeneralGraphics : MonoBehaviour {

		[SerializeField] public Dropdown textureQuality;
		[SerializeField] public Dropdown shadowRes;
		[SerializeField] public Dropdown antiAliasing;
		[SerializeField] public Dropdown shadows;
		
		[SerializeField] public Toggle vSync;
		[SerializeField] public Toggle softParticles;
		[SerializeField] public Toggle customSettings;
		

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
		
		

		public enum Textures
		{
			Potatoe,
			Low,
			Medium,
			High
		}

		private void Update() {
			
			if(!CustomSettings.isOn)
			{
				// shadow res
				ShadowRes.value = (int)QualitySettings.shadowResolution;
			
				// shadows
				Shadows.value = (int)QualitySettings.shadows;

				// texture quality
				if(QualitySettings.masterTextureLimit == (int)Textures.High)
				{
					TextureQuality.value = 0;
				}
				if(QualitySettings.masterTextureLimit == (int)Textures.Medium)
				{
					TextureQuality.value = 1;
				}
				if(QualitySettings.masterTextureLimit == (int)Textures.Low)
				{
					TextureQuality.value = 2;
				}
				if(QualitySettings.masterTextureLimit == (int)Textures.Potatoe)
				{
					TextureQuality.value = 3;
				}

				// anti aliasing
				AntiAliasing.value = (int)QualitySettings.antiAliasing;

				// soft particles
				SoftParticles.isOn = (bool)QualitySettings.softParticles;

				// vsync
				if(QualitySettings.vSyncCount == 0)
				{
					VSync.isOn = false;
				}
				else
				{
					VSync.isOn = true;
				}

			}
		}
		public void ChangeShadows()
		{
			QualitySettings.shadows = (ShadowQuality) Shadows.value;
		}
		public void ChangeTextureQuality()
		{
			var selected = TextureQuality.value;
			switch(selected)
			{
				case 0:
					QualitySettings.masterTextureLimit = 3;
					break;
				case 1:
					QualitySettings.masterTextureLimit = 2;
					break;
				case 2:
					QualitySettings.masterTextureLimit = 1;
					break;
				case 3:
					QualitySettings.masterTextureLimit = 0;
					break;
			}
		}

		public void ChangeAntiAliasing()
		{
			QualitySettings.antiAliasing = AntiAliasing.value;
		}

		public void ChangeShadowResolution()
		{
			QualitySettings.shadowResolution = (ShadowResolution) ShadowRes.value;
		}

		public void ChangeSoftParticles()
		{
			QualitySettings.softParticles = SoftParticles.isOn;
		}

		public void ChangeVSync()
		{
			var selected = VSync.isOn;
			if(selected)
			{
				QualitySettings.vSyncCount = 1;
			}
			else
			{
				QualitySettings.vSyncCount = 0;
			}
		}
		

		public void UpdateEverything()
		{
			ChangeShadowResolution();
			ChangeShadows();
			ChangeSoftParticles();
			ChangeTextureQuality();
			ChangeVSync();
			ChangeAntiAliasing();
			
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
	}
}
