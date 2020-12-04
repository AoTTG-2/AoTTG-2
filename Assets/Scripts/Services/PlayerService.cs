using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Events;
using Assets.Scripts.Events.Args;
using Assets.Scripts.Services.Interface;

namespace Assets.Scripts.Services
{
    public class PlayerService : IPlayerService
    {
        public event OnTitanDamaged OnTitanDamaged;
        public event OnTitanHit OnTitanHit;
        public event OnHeroHit OnHeroHit;

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
        public void HeroHit(HeroKillEvent heroKillEvent)
        {
            OnHeroHit?.Invoke(heroKillEvent);
        }

        public void OnRestart()
        {
        }

        public Entity Self { get; set; }
    }
}
