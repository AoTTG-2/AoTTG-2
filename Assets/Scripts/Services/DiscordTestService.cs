using Assets.Scripts.Services.Interface;
using Photon;

namespace Assets.Scripts.Services
{
    public class DiscordTestService : PunBehaviour, IDiscordService
    {
        public void UpdateDiscordActivity(global::Room room)
        {
            // Ignored, as Discord won't work in CI
        }
    }
}
