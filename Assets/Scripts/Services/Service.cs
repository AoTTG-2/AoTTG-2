using Assets.Scripts.Services.Interface;
using Photon;

namespace Assets.Scripts.Services
{
    public class Service : MonoBehaviour
    {
        public static readonly IEntityService Entity = new EntityService();
        public static readonly IFactionService Faction = new FactionService();
        public static readonly IPlayerService Player = new PlayerService();

        public static IPauseService Pause { get; private set; }
        public static ISpawnService Spawn { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Spawn = gameObject.AddComponent<SpawnService>();
            Pause = gameObject.AddComponent<PauseService>();
        }
    }
}
