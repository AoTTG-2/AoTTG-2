using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI.InGame.DebugWindow
{
    public class TabGroup : MonoBehaviour
    {
        public List<TabPanel> Panels;


        private TabPanel currentPanel;
        private int panelIndex;

        private void Awake()
        {
            panelIndex = 0;
            SetCurrentPanel();
        }

        public void SetCurrentPanel()
        {
            for (var i = 0; i < Panels.Count; i++)
            {
                Panels[i].gameObject.SetActive(i == panelIndex);
            }
        }

        public void OnTabSelected(int tabIndex)
        {
            panelIndex = tabIndex;
            SetCurrentPanel();
        }
    }
}
