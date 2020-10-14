using Assets.Scripts.Characters;
using Assets.Scripts.Services.Interface;

namespace Assets.Scripts.Services
{
    public class PlayerService : IPlayerService
    {
        public void OnRestart()
        {
        }

        public Entity Self { get; set; }
    }
}
