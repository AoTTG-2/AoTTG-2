using Assets.Scripts.Events.Args;

namespace Assets.Scripts.Events
{
    public delegate void OnHeroHit(HeroHitEvent heroKillEvent);

    public delegate void OnHeroKill(HeroKillEvent heroKillEvent);

    public delegate void OnTitanDamaged(TitanDamagedEvent titanDamagedEvent);

    public delegate void OnTitanHit(TitanHitEvent titanHitEvent);
}
