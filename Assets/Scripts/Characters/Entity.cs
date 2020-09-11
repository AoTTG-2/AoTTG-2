using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Photon;

namespace Assets.Scripts.Characters
{
    public abstract class Entity : MonoBehaviour
    {
        protected readonly IEntityService EntityService = Service.Entity;

        public Faction Faction { get; set; }

        protected virtual void Awake()
        {
            EntityService.Register(this);
        }

        protected virtual void OnDestroy()
        {
            EntityService.UnRegister(this);
        }
    }
}
