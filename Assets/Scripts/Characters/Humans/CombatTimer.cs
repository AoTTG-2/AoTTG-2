using Assets.Scripts.Events.Args;
using Assets.Scripts.Audio;
using Assets.Scripts.Base;
using Assets.Scripts.Services;

namespace Assets.Scripts.Characters.Humans
{
    public class CombatTimer : StateTimer
    {
        #region Protected Methods
        protected override void SetState()
        {
            var controller = Service.Music;
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