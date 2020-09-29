using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using System;

namespace Assets.Scripts.Characters.Titan.Behavior
{
    public abstract class TitanBehavior
    {
        protected IEntityService EntityService => Service.Entity;
        protected IFactionService FactionService => Service.Faction;

        public void Initialize(MindlessTitan titan)
        {
            Titan = titan;
        }
        protected MindlessTitan Titan { get; set; }

        public bool OnUpdate()
        {
            switch (Titan.State)
            {
                case TitanState.Idle:
                    break;
                case TitanState.Dead:
                    break;
                case TitanState.Wandering:
                    return OnWandering();
                case TitanState.Turning:
                    break;
                case TitanState.Chase:
                    return OnChase();
                case TitanState.Attacking:
                    break;
                case TitanState.Recovering:
                    break;
                case TitanState.Eat:
                    break;
                case TitanState.Disabled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return false;
        }

        public bool OnFixedUpdate()
        {
            switch (Titan.State)
            {
                case TitanState.Idle:
                    break;
                case TitanState.Dead:
                    break;
                case TitanState.Wandering:
                    return OnWanderingFixedUpdate();
                case TitanState.Turning:
                    break;
                case TitanState.Chase:
                    return OnFixedUpdateChase();
                case TitanState.Attacking:
                    break;
                case TitanState.Recovering:
                    break;
                case TitanState.Eat:
                    break;
                case TitanState.Disabled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return false;
        }

        public bool OnUpdateEverySecond(int seconds)
        {
            switch (Titan.State)
            {
                case TitanState.Idle:
                    break;
                case TitanState.Dead:
                    break;
                case TitanState.Wandering:
                    return OnWanderingUpdateEverySecond(seconds);
                case TitanState.Turning:
                    break;
                case TitanState.Chase:
                    break;
                case TitanState.Attacking:
                    break;
                case TitanState.Recovering:
                    break;
                case TitanState.Eat:
                    break;
                case TitanState.Disabled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return false;
        }
        
        protected virtual bool OnChase()
        {
            return false;
        }

        protected virtual bool OnChasingUpdateEverySecond(int seconds)
        {
            return false;
        }

        protected virtual bool OnFixedUpdateChase()
        {
            return false;
        }

        protected virtual bool OnWandering()
        {
            return false;
        }

        protected virtual bool OnWanderingFixedUpdate()
        {
            return false;
        }

        protected virtual bool OnWanderingUpdateEverySecond(int seconds)
        {
            return false;
        }
    }
}
