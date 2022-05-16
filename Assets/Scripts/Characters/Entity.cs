using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Photon;

namespace Assets.Scripts.Characters
{
    /// <summary>
    /// An Entity is either a Human, Titan or Horse. It are living things which roam around the world
    /// </summary>
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
            if (photonView.isMine && PhotonNetwork.connected)
                PhotonNetwork.RemoveRPCs(photonView);
        }

        public abstract void OnHit(Entity attacker, int damage);
    }
}
