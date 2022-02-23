using Assets.Scripts.Services.Interface;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Assets.Scripts.Services
{
    public class Service : MonoBehaviour
    {
        public static readonly IEntityService Entity = new EntityService();
        public static readonly IPlayerService Player = new PlayerService();
        public static readonly ILevelService Level = new LevelService();
        public static readonly IMusicService Music = new MusicService();

        public static AuthenticationService Authentication { get; private set; }
        public static IFactionService Faction { get; private set; }
        public static IInventoryService Inventory { get; private set; }
        public static ICustomMapService Map { get; private set; }
        public static IMessageService Message { get; private set; }
        public static IPauseService Pause { get; private set; }
        public static ISettingsService Settings { get; private set; }
        public static ISpawnService Spawn { get; private set; }
        public static ITimeService Time { get; private set; }
        public static IUiService Ui { get; private set; }
        
        public static IPhotonService Photon { get; private set; }
        public static IDiscordService Discord { get; private set; }
        public static Localization Localization { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Authentication = gameObject.GetComponent<AuthenticationService>();
            Faction = gameObject.AddComponent<FactionService>();
            Inventory = gameObject.GetComponent<IInventoryService>();
            Map = gameObject.GetComponent<ICustomMapService>();
            Message = gameObject.GetComponent<IMessageService>();
            Pause = gameObject.AddComponent<PauseService>();
            Settings = gameObject.AddComponent<SettingsService>();
            Spawn = gameObject.AddComponent<SpawnService>();
            Time = gameObject.AddComponent<TimeService>();
            Ui = gameObject.GetComponent<UiService>();
            Localization = gameObject.GetComponent<Localization>();
#if UNITY_INCLUDE_TESTS
            Discord = gameObject.AddComponent<DiscordTestService>();
#else
            Discord = gameObject.AddComponent<DiscordService>();
#endif

            Photon = gameObject.GetComponent<IPhotonService>();
            gameObject.AddComponent<ScreenshotService>();
        }
    }
}