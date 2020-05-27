using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
	public class GraphicSettingMenu : MonoBehaviour {

		public GraphicsController GraphicController;
		public InGameMenu Menu;

		private void OnDisable() {
			GraphicController.label.text = "";
		}
	}
}
