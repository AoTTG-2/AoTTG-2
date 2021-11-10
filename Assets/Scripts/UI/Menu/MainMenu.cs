using Assets.Scripts.Services;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UCamera = UnityEngine.Camera;

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

        private Volume postProcess;
        private RenderTexture sceneRender;
        private UCamera blenderCam;

        private void Awake()
        {
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

            this.blenderCam = GameObject.Find("Camera").GetComponent<UCamera>();
            this.postProcess = GameObject.FindObjectOfType<Volume>();
            this.setCameraResolution();
        }

        private void recalculatePostRenderEffects()
        {
            if(this.postProcess.profile.TryGet<DepthOfField>(out var depthEffect))
            {
                depthEffect.focalLength.value = 75+Mathf.Log(Screen.height / 1080f, 2)*20;
            }
        }

        private void recalculateSceneRenderer()
        {
            if (this.sceneRender == null)
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
        }

        private void setCameraResolution()
        {
            this.recalculateSceneRenderer();
            var aspectRatio = (float)sceneRender.width / sceneRender.height;
            blenderCam.aspect = aspectRatio;
            
            //super ultra wide monitor ratio goes up to ~3.5 (32/9)
            //and the default considered is the ~1.7 (16/9) standard
            float capped_normalized_ratio = (Mathf.Max(Mathf.Min(aspectRatio, 3.5f), 1.5f)-1.5f)/2f;
            blenderCam.focalLength = Mathf.Lerp(90, 40, capped_normalized_ratio);
            this.recalculatePostRenderEffects();

#if UNITY_EDITOR
            Debug.Log("RESETTING RENDER TO " + this.sceneRender.width + "x" + this.sceneRender.height);
#endif
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
