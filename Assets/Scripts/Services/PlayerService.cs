using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using Assets.Scripts.Events.Args;
using Assets.Scripts.Services.Interface;

namespace Assets.Scripts.Services
{
    /// <inheritdoc/>
    public class PlayerService : IPlayerService
    {
        public event OnTitanDamaged OnTitanDamaged;
        public event OnTitanHit OnTitanHit;
        public event OnHeroHit OnHeroHit;
        public event OnHeroKill OnHeroKill;

        /// <inheritdoc/>
        public void TitanDamaged(TitanDamagedEvent titanDamagedEvent)
        {
            OnTitanDamaged?.Invoke(titanDamagedEvent);
        }

        /// <inheritdoc/>
        public void TitanHit(TitanHitEvent titanHitEvent)
        {
            OnTitanHit?.Invoke(titanHitEvent);
        }

        /// <inheritdoc/>
        public void HeroHit(HeroHitEvent heroHitEvent)
        {
            OnHeroHit?.Invoke(heroHitEvent);
        }

        /// <inheritdoc/>
        public void HeroKill(HeroKillEvent heroKillEvent)
        {
            OnHeroKill?.Invoke(heroKillEvent);
        }

        public void OnRestart()
        {
        }

        public Entity Self { get; set; }
    }
}
