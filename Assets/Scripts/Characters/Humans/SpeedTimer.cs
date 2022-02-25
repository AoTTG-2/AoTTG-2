using Assets.Scripts.Audio;
using Assets.Scripts.Base;
using Assets.Scripts.Events.Args;

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
            var controller = MusicController.Instance;
            var currentState = controller.ActiveState;
            var actionState = currentState.Equals(MusicState.Action);
            if (IsActiveState && !actionState && totalTimeInState > 3)
            {
                controller.SetMusicState(new MusicStateChangedEvent(MusicState.Action));
            }
            else if (!IsActiveState && actionState)
            {
                controller.SetMusicState(new MusicStateChangedEvent(MusicState.Neutral));
            }
        }
        #endregion
    }
}