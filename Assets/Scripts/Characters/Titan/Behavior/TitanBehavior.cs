using System;

namespace Assets.Scripts.Characters.Titan.Behavior
{
    public abstract class TitanBehavior
    {
        public void Initialize(MindlessTitan titan)
        {
            Titan = titan;
        }
        protected MindlessTitan Titan { get; set; }
        public bool OnUpdate()
        {
            switch (Titan.TitanState)
            {
                case MindlessTitanState.Idle:
                    break;
                case MindlessTitanState.Dead:
                    break;
                case MindlessTitanState.Wandering:
                    return OnWandering();
                case MindlessTitanState.Turning:
                    break;
                case MindlessTitanState.Chase:
                    return OnChase();
                case MindlessTitanState.Attacking:
                    break;
                case MindlessTitanState.Recovering:
                    break;
                case MindlessTitanState.Eat:
                    break;
                case MindlessTitanState.Disabled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return false;
        }

        public bool OnFixedUpdate()
        {
            switch (Titan.TitanState)
            {
                case MindlessTitanState.Idle:
                    break;
                case MindlessTitanState.Dead:
                    break;
                case MindlessTitanState.Wandering:
                    return OnWanderingFixedUpdate();
                case MindlessTitanState.Turning:
                    break;
                case MindlessTitanState.Chase:
                    return OnFixedUpdateChase();
                case MindlessTitanState.Attacking:
                    break;
                case MindlessTitanState.Recovering:
                    break;
                case MindlessTitanState.Eat:
                    break;
                case MindlessTitanState.Disabled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return false;
        }

        public bool OnUpdateEverySecond(int seconds)
        {
            switch (Titan.TitanState)
            {
                case MindlessTitanState.Idle:
                    break;
                case MindlessTitanState.Dead:
                    break;
                case MindlessTitanState.Wandering:
                    return OnWanderingUpdateEverySecond(seconds);
                case MindlessTitanState.Turning:
                    break;
                case MindlessTitanState.Chase:
                    break;
                case MindlessTitanState.Attacking:
                    break;
                case MindlessTitanState.Recovering:
                    break;
                case MindlessTitanState.Eat:
                    break;
                case MindlessTitanState.Disabled:
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
