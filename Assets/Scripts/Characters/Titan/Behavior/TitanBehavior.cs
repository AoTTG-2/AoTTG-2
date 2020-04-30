using System;

namespace Assets.Scripts.Characters.Titan.Behavior
{
    public abstract class TitanBehavior
    {
        public bool OnUpdate(MindlessTitan titan)
        {
            switch (titan.TitanState)
            {
                case MindlessTitanState.Idle:
                    break;
                case MindlessTitanState.Dead:
                    break;
                case MindlessTitanState.Wandering:
                    break;
                case MindlessTitanState.Turning:
                    break;
                case MindlessTitanState.Chase:
                    return OnChase(titan);
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
        
        protected virtual bool OnChase(MindlessTitan titan)
        {
            return false;
        }

        protected virtual bool OnWandering(MindlessTitan titan)
        {
            return false;
        }
    }
}
