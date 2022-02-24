using Assets.Scripts.Events.Args;
using Assets.Scripts.Services;

namespace Assets.Scripts.Characters.Humans
{
    public class SpeedTimer : StateTimer
    {
        #region Constructors
        public SpeedTimer() : base()
        {
            maxTimer = 3;
        }
        #endregion

        #region Protected Methods
        protected override void SetState()
        {
            var service = Service.Music;
            var currentState = service.ActiveState;
            var actionState = currentState.Equals(MusicState.Action);
            if (IsActiveState && !actionState && totalTimeInState > 3)
            {
                service.SetMusicState(new MusicStateChangedEvent(MusicState.Action));
            }
            else if (!IsActiveState && actionState)
            {
                service.SetMusicState(new MusicStateChangedEvent(MusicState.Neutral));
            }
        }
        #endregion
    }
}