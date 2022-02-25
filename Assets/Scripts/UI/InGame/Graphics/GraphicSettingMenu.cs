using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
	public class GraphicSettingMenu : UiMenu {

		public GraphicsController GraphicController;
		public InGameMenu Menu;

        protected override void OnEnable()
        {
            base.OnEnable();
            GraphicController.gameObject.SetActive(true);
        }

        protected override void OnDisable() {
            base.OnDisable();
			GraphicController.PrefsLabel.text = "";
		}
	}
}
