using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Events;
using Assets.Scripts.Events.Args;

namespace Assets.Scripts.Services.Interface
{
    public interface IPlayerService : IService
    {
        Entity Self { get; set; }

        /// <summary>
        /// The team/faction this player is on. If this is null you can assume that the player has not selected a team yet.
        /// Replaces FengGameManager.needChooseSide
        /// </summary>
        Faction Faction { get; }

        /// <summary>
        /// Sets the faction of this player and also sets the faction of the entity Self.
        /// </summary>
        /// <param name="faction"></param>
        void SetFaction(Faction faction);

        event OnTitanDamaged OnTitanDamaged;
        event OnTitanHit OnTitanHit;
        event OnHeroHit OnHeroHit;
        event OnHeroKill OnHeroKill;

        /// <summary>
        /// Triggers when damaging a <see cref="TitanBase">Titan</see>.
        /// </summary>
        /// <param name="titanDamagedEvent"></param>
        void TitanDamaged(TitanDamagedEvent titanDamagedEvent);

        /// <summary>
        /// Triggers when hitting a <see cref="TitanBase">Titan</see>.
        /// </summary>
        /// <param name="titanHitEvent"></param>
        void TitanHit(TitanHitEvent titanHitEvent);

        /// <summary>
        /// Triggers when a Hero hits another <see cref="Hero"/>.
        /// </summary>
        /// <param name="heroKillEvent"></param>
        void HeroHit(HeroHitEvent heroHitEvent);

        /// <summary>
        /// Triggers when a Hero kills another <see cref="Hero"/>.
        /// </summary>
        /// <param name="heroKillEvent"></param>
        void HeroKill(HeroKillEvent heroKillEvent);

        void OnJoinedRoom();
    }
}
