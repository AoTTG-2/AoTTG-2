using Assets.Scripts.Services;
using Assets.Scripts.Events.Args;

namespace Assets.Scripts.Characters.Humans
{
    public class CombatTimer : StateTimer
    {
        #region Protected Methods
        protected override void SetState()
        {
            var service = Service.Music;
            var currentState = service.ActiveState;
            var combatState = currentState.Equals(MusicState.Combat);
            if (IsActiveState && !combatState)
            {
                service.SetMusicState(new MusicStateChangedEvent(MusicState.Combat));
            }
            else if (!IsActiveState && combatState)
            {
                service.SetMusicState(new MusicStateChangedEvent(MusicState.Neutral));
            }
        }
        #endregion
 
    }
}