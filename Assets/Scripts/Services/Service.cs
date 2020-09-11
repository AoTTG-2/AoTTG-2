using Assets.Scripts.Services.Interface;

namespace Assets.Scripts.Services
{
    public static class Service
    {
        public static readonly IEntityService Entity = new EntityService();
        public static readonly IFactionService Faction = new FactionService();
        public static readonly IPlayerService Player = new PlayerService();
        public static readonly ISpawnService Spawn = new SpawnService();
    }
}
