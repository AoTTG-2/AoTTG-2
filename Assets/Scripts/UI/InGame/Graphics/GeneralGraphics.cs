using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Newtonsoft.Json;

	public class GeneralGraphics : MonoBehaviour {

		[SerializeField] public Dropdown textureQuality;
		[SerializeField] public Dropdown shadowRes;
		[SerializeField] public Dropdown antiAliasing;
		[SerializeField] public Dropdown shadows;
		[SerializeField] public Toggle vSync;
		[SerializeField] public Toggle softParticles;

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

		private void Update() {
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
		public void ChangeShadows()
		{
			var selected = Shadows.value;
			Debug.Log(selected);
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

		public void LoadPlayerPrefs()
		{
			TextureQuality.value = PlayerPrefs.GetInt("TextureQuality");
			ShadowRes.value = PlayerPrefs.GetInt("ShadowResolution");
			AntiAliasing.value = PlayerPrefs.GetInt("AntiAliasing");
			Shadows.value = PlayerPrefs.GetInt("Shadow");
			if(PlayerPrefs.GetInt("VSync") == 1)
			{
				VSync.isOn = true;
			}
			else
			{
				VSync.isOn = false;
			}
			if(PlayerPrefs.GetInt("SoftParticles") == 1)
			{
				SoftParticles.isOn = true;
			}
			else
			{
				SoftParticles.isOn = false;
			}
		}
	}
