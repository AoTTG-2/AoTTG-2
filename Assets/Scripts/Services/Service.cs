using Assets.Scripts.Services.Interface;
using Photon;

namespace Assets.Scripts.Services
{
    public class Service : MonoBehaviour
    {
        public static readonly IEntityService Entity = new EntityService();
        public static readonly IPlayerService Player = new PlayerService();

        public static IFactionService Faction { get; private set; }
        public static IPauseService Pause { get; private set; }
        public static ISpawnService Spawn { get; private set; }
        public static ITimeService Time { get; private set; }
        public static IUiService Ui { get; private set; }
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Faction = gameObject.AddComponent<FactionService>();
            Pause = gameObject.AddComponent<PauseService>();
            Spawn = gameObject.AddComponent<SpawnService>();
            Time = gameObject.AddComponent<TimeService>();
            Ui = gameObject.GetComponent<IUiService>();
        }
    }
}
