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
        }

        protected virtual void OnDestroy()
        {
            EntityService.UnRegister(this);
        }

        public abstract void OnHit(Entity attacker, int damage);

        protected static void Initialize<T>(T entityConfiguration) where T : EntityConfiguration
        {

        }
    }
}
