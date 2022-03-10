using Assets.Scripts.Characters.Humans.Data.States.Grounded;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.StateMachines.Movement.States.Grounded
{
    public class HeroDashinState : HeroGroundedState
    {
        private readonly HeroDashData dashData;
        public HeroDashinState(HeroMovementStateMachine heroMovementStateMachine) : base(heroMovementStateMachine)
        {
            dashData = movementData.DashData;
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
            if (stateMachine.ReusableData.MovementInput != Vector2.zero) return;
            Vector2 movementInput = stateMachine.ReusableData.MovementInput;
            facingDirection = GetGlobalFacingDirection(movementInput.y, movementInput.x);
            Vector3 dashV = GetGlobaleFacingVector3(facingDirection);
            SnapToFaceDirection();
            PhotonNetwork.Instantiate("FX/boost_smoke", stateMachine.Hero.transform.position, stateMachine.Hero.transform.rotation, 0);
            stateMachine.Hero.Rigidbody.AddForce((dashV * dashData.SpeedModifier), ForceMode.VelocityChange);
            stateMachine.Hero.HumanInput.DisableActionFor(stateMachine.Hero.HumanInput.HumanActions.Dodge, dashData.DashCooldown);
        }
        #endregion
    }
}