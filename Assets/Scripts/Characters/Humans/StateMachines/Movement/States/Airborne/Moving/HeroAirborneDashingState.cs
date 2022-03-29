using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Characters.Humans.Constants;
using Assets.Scripts.Characters.Humans.StateMachines;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Airborne.Moving
{
    public class HeroAirborneDashingState : HeroAirborneMovingState
    {
        private const float FOUR_PERCENT = 100 / 4;
        public HeroAirborneDashingState(HeroMovementStateMachine heroMovementStateMachine) : base(heroMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            Dash();
        }
        #endregion
        #region Main Methods
        private void Dash()
        {
            UseGas(FOUR_PERCENT);
            Vector3 movementVector = GetMovementInputDirection();
            facingDirection = GetGlobalFacingDirection(movementVector.x, movementVector.z);
            Vector3 dashV = GetGlobalFacingVector3(facingDirection);
            SnapToFaceDirection();
            Quaternion rotation = Quaternion.Euler(0f, facingDirection, 0f);
            targetRotation = rotation;
            PhotonNetwork.Instantiate("FX/boost_smoke", stateMachine.Hero.transform.position, stateMachine.Hero.transform.rotation, 0);
            stateMachine.Hero.Rigidbody.AddForce((dashV * airborneData.SpeedModifier), ForceMode.VelocityChange);
            UpdateAnimation();

            /*if (((dashTime <= 0f) && (currentGas > 0f)) && !isMounted && (burstCD.ElapsedMilliseconds <= BurstCDmin || burstCD.ElapsedMilliseconds >= BurstCDmax))
            {
                Animation[HeroAnim.DASH].time = 0.1f;
                FalseAttack();
            }*/
        }

        private void UpdateAnimation()
        {
            CrossFade(HeroAnim.DASH, 0.1f);
        }
        #endregion
    }
}
