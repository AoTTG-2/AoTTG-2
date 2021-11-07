using Assets.Scripts.Services;
using System.Linq;
using UnityEngine;
using UCamera = UnityEngine.Camera;
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

        [SerializeField]
        private UnityEngine.UI.RawImage renderTarget;

        private RenderTexture sceneRender;
        private UCamera blenderCam;

        private void Awake()
        {
            if (isFirstLaunch)
            {
                LeanTween.delayedCall(initialDelay, () =>
                {
                    LeanTween.move(rightPanel, rightPanelEndPosition, panelEnterAnimationTime);
                    rightPanel.GetComponent<AudioSource>().Play();
                    LeanTween.move(accountPanel, accountPanelEndPosition, panelEnterAnimationTime);
                    isFirstLaunch = false;
                });
            }
        }

        private void OnEnable()
        {
            if (!isFirstLaunch)
            {
                rightPanel.anchoredPosition = rightPanelEndPosition;
                accountPanel.anchoredPosition = accountPanelEndPosition;
                rightPanel.GetComponent<AudioSource>().Play();
                GameObject.FindObjectsOfType<Animator>().First(
                    (a) => a.name.Equals("Main Menu"))?.Play("Base Layer.Idle", 0, 0f);
            }

            this.blenderCam = GameObject.Find("Camera").GetComponent<UCamera>();
            this.setCameraResolution();
        }

        private void setCameraResolution()
        {
            if(this.sceneRender == null)
            {
                this.sceneRender = new RenderTexture(Screen.width, Screen.height, 24);
            }
            else
            {
                this.sceneRender.Release();
                this.sceneRender.width = Screen.width;
                this.sceneRender.height = Screen.height;
            }
            this.sceneRender.Create();

            blenderCam.targetTexture = this.sceneRender;
            renderTarget.texture = this.sceneRender;
            blenderCam.aspect = (float)sceneRender.width / sceneRender.height;

            Debug.Log("RESETTING RENDER TO " + this.sceneRender.width + "x" + this.sceneRender.height);

        }

        private void Update()
        {
            if (sceneRender.width != Screen.width || sceneRender.height != Screen.height)
                setCameraResolution();
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

        private void OnApplicationFocus(bool focus)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.lockState = CursorLockMode.None;
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
