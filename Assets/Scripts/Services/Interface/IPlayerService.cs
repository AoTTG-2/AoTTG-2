using Assets.Scripts.Characters;

namespace Assets.Scripts.Services.Interface
{
    public interface IPlayerService : IService
    {
        Entity Self { get; set; }
    }
}
