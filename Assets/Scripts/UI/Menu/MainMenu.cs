using Assets.Scripts.Services;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI.Menu
{
    /// <summary>
    /// UI Container class which is used to navigate to all the Main menu UI elements
    /// </summary>
    public partial class MainMenu : UiNavigationElement
    {

        private static bool isFirstLaunch = true;

        [SerializeField]
        RectTransform rightPanel,accountPanel;

        [SerializeField]
        private Vector3 rightPanelEndPosition, accountPanelEndPosition;
        
        [SerializeField]
        private float initialDelay,panelEnterAnimationTime;

        [SerializeField]
        private RenderTexture sceneRender;

        private QualityAdaptator adaptator;

        private void Awake()
        {
            this.adaptator = new QualityAdaptator(this.sceneRender);

            if (isFirstLaunch)
            {
                LeanTween.delayedCall(initialDelay, () =>
                {
                    LeanTween.move(rightPanel, rightPanelEndPosition, panelEnterAnimationTime);
                    LeanTween.move(accountPanel, accountPanelEndPosition, panelEnterAnimationTime);
                    this.rightPanel.GetComponent<AudioSource>().Play();
                    MainMenu.isFirstLaunch = false;
                });
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            //hacky code to workaround unity buggy behaviour with cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.lockState = CursorLockMode.None;


            if (!isFirstLaunch)
            {
                rightPanel.anchoredPosition = rightPanelEndPosition;
                accountPanel.anchoredPosition = accountPanelEndPosition;
                GameObject.FindObjectsOfType<Animator>().First(
                    (a) => a.name.Equals("Main Menu"))?.Play("Base Layer.Idle", 0, 0f);
            }

            this.adaptator.findComponentReferences();
            this.adaptator.setCameraResolution();
            this.adaptator.useCamera(true);
        }

        private void OnDisable()
        {
            base.OnDisable();
            this.adaptator.useCamera(true);
        }

        private void Update()
        {
            this.adaptator.checkResolution();
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
