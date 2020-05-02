using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
	public class GraphicSettingMenu : MonoBehaviour {

		public GraphicsPanel GraphicPanel;
		public InGameMenu Menu;

		private void OnEnable() {
			GraphicPanel.gameObject.SetActive(true);
			Menu.gameObject.SetActive(false);
		}
	}
}
