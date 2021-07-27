using Assets.Scripts.Services.Interface;
using Photon;

namespace Assets.Scripts.Services
{
    /// <summary>
    /// A mock of the <see cref="IDiscordService"/> used to assure the Play Tests can pass
    /// </summary>
    public class DiscordTestService : PunBehaviour, IDiscordService
    {
        /// <inheritdoc/>
        public void UpdateDiscordActivity(global::Room room)
        {
            // Ignored, as Discord won't work in CI
        }
    }
}
