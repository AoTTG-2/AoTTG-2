using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
	public class GraphicSettingMenu : MonoBehaviour {

		public GraphicsController GraphicController;
		public InGameMenu Menu;
		public GameObject QualityMenu;
		public GameObject ResolutionMenu;

		private void OnDisable() {
			GraphicController.label.text = "";
		}

		public void ShowQualityMenu()
		{
			if(QualityMenu.GetActive())
			{
				QualityMenu.SetActive(true);
				if(ResolutionMenu.GetActive())
				{
					ResolutionMenu.SetActive(false);
				}
			}
		}

		public void ShowResolutionMenu()
		{
			if(!ResolutionMenu.GetActive() == true)
			{
				ResolutionMenu.SetActive(true);
				if(QualityMenu.GetActive())
				{
					QualityMenu.SetActive(false);
				}
			}
		}
	}
}
