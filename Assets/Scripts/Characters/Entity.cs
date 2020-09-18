using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Photon;
using System;

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

        protected static void Initialize<T>(T entityConfiguration) where T : EntityConfiguration
        {

        }
    }
}
