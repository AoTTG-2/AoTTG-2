using Assets.Scripts.Events.Args;
using Assets.Scripts.Audio;
using Assets.Scripts.Base;

namespace Assets.Scripts.Characters.Humans
{
    public class CombatTimer : StateTimer
    {
        #region Protected Methods
        protected override void SetState()
        {
            var controller = MusicController.Instance;
            var currentState = controller.ActiveState;
            var combatState = currentState.Equals(MusicState.Combat);
            if (IsActiveState && !combatState)
            {
                controller.SetMusicState(new MusicStateChangedEvent(MusicState.Combat));
            }
            else if (!IsActiveState && combatState)
            {
                controller.SetMusicState(new MusicStateChangedEvent(MusicState.Neutral));
            }
        }
        #endregion
 
    }
}