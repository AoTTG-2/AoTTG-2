using Assets.Scripts.Services;
using UnityEngine;
#if UNITY_EDITOR
#else
#endif

namespace Assets.Scripts.UI.Menu
{
    /// <summary>
    /// UI Container class which is used to navigate to all the Main menu UI elements
    /// </summary>
    public class MainMenu : UiNavigationElement
    {
        private static bool isFirstLaunch = true;

        [SerializeField]
        RectTransform rightPanel,accountPanel;

        [SerializeField]
        private Vector3 rightPanelEndPosition, accountPanelEndPosition;
        [SerializeField]
        private float initialDelay,panelEnterAnimationTime;

        private void Awake()
        {
            if (isFirstLaunch)
            {
                LeanTween.delayedCall(initialDelay, () =>
                {
                    LeanTween.move(rightPanel, rightPanelEndPosition, panelEnterAnimationTime);
                    rightPanel.GetComponent<AudioSource>().Play();
                    LeanTween.move(accountPanel, accountPanelEndPosition, panelEnterAnimationTime);
                });

                isFirstLaunch = false;
            }
            else
            {
                rightPanel.anchoredPosition = rightPanelEndPosition;
                accountPanel.anchoredPosition = accountPanelEndPosition;
            }
        }

        public void Singleplayer()
        {
            Navigate(typeof(Singleplayer));
        }

        public void Multiplayer()
        {
            Navigate(typeof(ServerSelector));
        }

        public void MultiplayerLobby()
        {
            Service.Photon.Connect();
            Navigate(typeof(Lobby));
        }

        public void Credits()
        {
            Navigate(typeof(Credits));
        }

        public void MapEditor()
        {
            //TODO: Switch to MapEditor scene
            Navigate(typeof(MapConverter));
        }

        public void Quit()
        {

#if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
