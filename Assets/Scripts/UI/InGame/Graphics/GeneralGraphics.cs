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

		private void Update() {
			
			if(!CustomSettings.isOn)
			{
				// shadow res
				if(QualitySettings.shadowResolution == ShadowResolution.Low)
				{
					ShadowRes.value = 0;
				}
				else if(QualitySettings.shadowResolution == ShadowResolution.Medium)
				{
					ShadowRes.value = 1;
				}
				else if(QualitySettings.shadowResolution == ShadowResolution.High)
				{
					ShadowRes.value = 2;
				}
				else if(QualitySettings.shadowResolution == ShadowResolution.VeryHigh)
				{
					ShadowRes.value = 3;
				}

				// shadows
				if(QualitySettings.shadows == ShadowQuality.Disable)
				{
					Shadows.value = 0;
				}
				else if(QualitySettings.shadows == ShadowQuality.HardOnly)
				{
					Shadows.value = 1;
				}
				else if(QualitySettings.shadows == ShadowQuality.All)
				{
					Shadows.value = 2;
				}

				// texture quality
				if(QualitySettings.masterTextureLimit == 3)
				{
					TextureQuality.value = 0;
				}
				else if(QualitySettings.masterTextureLimit == 2)
				{
					TextureQuality.value = 1;
				}
				else if(QualitySettings.masterTextureLimit == 1)
				{
					TextureQuality.value = 2;
				}
				else if(QualitySettings.masterTextureLimit == 0)
				{
					TextureQuality.value = 3;
				}
				
				// anti aliasing
				if(QualitySettings.antiAliasing == 0)
				{
					AntiAliasing.value = 0;
				}
				else if(QualitySettings.antiAliasing == 2)
				{
					AntiAliasing.value = 1;
				}
				else if(QualitySettings.antiAliasing == 4)
				{
					AntiAliasing.value = 2;
				}
				else if(QualitySettings.antiAliasing == 8)
				{
					AntiAliasing.value = 3;
				}

				// soft particles
				if(QualitySettings.softParticles)
				{
					SoftParticles.isOn = true;
				}
				else
				{
					SoftParticles.isOn = false;
				}

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
			var selected = Shadows.value;
			switch(selected)
			{
				case 0:
					QualitySettings.shadows = ShadowQuality.Disable;
					break;
				case 1:
					QualitySettings.shadows = ShadowQuality.HardOnly;	
					break;
				case 2:
					QualitySettings.shadows = ShadowQuality.All;
					break;	
			}
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
			var selected = AntiAliasing.value;
			switch(selected)
			{
				case 0:
					QualitySettings.antiAliasing = 0;
					break;
				case 1:
					QualitySettings.antiAliasing = 2;
					break;
				case 2:
					QualitySettings.antiAliasing = 4;
					break;
				case 3:
					QualitySettings.antiAliasing = 8;
					break;				
			}
		}

		public void ChangeShadowResolution()
		{
			var selected = ShadowRes.value;
			switch(selected)
			{
				case 0:
					QualitySettings.shadowResolution = ShadowResolution.Low;
					break;
				case 1:
					QualitySettings.shadowResolution = ShadowResolution.Medium;
					break;
				case 2:
					QualitySettings.shadowResolution = ShadowResolution.High;
					break;
				case 3:
					QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
					break;				
			}
		}

		public void ChangeSoftParticles()
		{
			var selected = SoftParticles.isOn;
			if(selected)
			{
				QualitySettings.softParticles = true;
			}
			else
			{
				QualitySettings.softParticles = false;
			}
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
		public struct Data
		{
			public int TextureQuality;
			public int ShadowRes;
			public int AntiAliasing;
			public int Shadows;
			public bool VSync;
			public bool SoftParticles;
			public bool CustomSettings;

			public Data(int textureQuality, int shadowRes, int antiAliasing, int shadows, bool vSync, bool softParticles, bool customSettings)
			{
				this.TextureQuality = textureQuality;
				this.ShadowRes = shadowRes;
				this.AntiAliasing = antiAliasing;
				this.Shadows = shadows;
				this.VSync = vSync;
				this.SoftParticles = softParticles;
				this.CustomSettings = customSettings;
			}
		}
	}
}
