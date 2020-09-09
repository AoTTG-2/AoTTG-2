using Assets.Scripts.Room;
using System.Collections.Generic;

namespace Assets.Scripts.Services.Interface
{
    public interface IRespawnService
    {
        void Add(PlayerSpawner playerSpawner);
        void Remove(PlayerSpawner playerSpawner);
        List<PlayerSpawner> GetAll();
        PlayerSpawner GetRandom();
        List<PlayerSpawner> GetByType(PlayerSpawnType type);
    }
}
