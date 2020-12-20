using Assets.Scripts.Services.Interface;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Assets.Scripts.Services
{
    public class Service : MonoBehaviour
    {
        public static readonly IEntityService Entity = new EntityService();
        public static readonly IPlayerService Player = new PlayerService();

        public static AuthenticationService Authentication { get; private set; }
        public static IFactionService Faction { get; private set; }
        public static IPauseService Pause { get; private set; }
        public static ISettingsService Settings { get; private set; }
        public static ISpawnService Spawn { get; private set; }
        public static ITimeService Time { get; private set; }
        public static IUiService Ui { get; private set; }
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Authentication = gameObject.GetComponent<AuthenticationService>();
            Faction = gameObject.AddComponent<FactionService>();
            Pause = gameObject.AddComponent<PauseService>();
            Settings = gameObject.AddComponent<SettingsService>();
            Spawn = gameObject.AddComponent<SpawnService>();
            Time = gameObject.AddComponent<TimeService>();
            Ui = gameObject.GetComponent<UiService>();

            gameObject.AddComponent<ScreenshotService>();
        }
    }
}
