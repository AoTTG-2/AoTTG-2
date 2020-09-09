using Assets.Scripts.Services.Interface;

namespace Assets.Scripts.Services
{
    public static class Service
    {
        public static readonly IRespawnService Respawn = new RespawnService();
    }
}
