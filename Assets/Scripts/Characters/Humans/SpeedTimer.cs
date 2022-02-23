using Assets.Scripts.Services;

namespace Assets.Scripts.Characters.Humans
{
    public class SpeedTimer : StateTimer
    {
        public SpeedTimer() : base()
        {
            maxTimer = 3;
        }

        protected override void SetState()
        {
            var service = Service.Music;
            var currentState = service.ActiveState;
            var actionState = currentState.Equals(MusicState.Action);
            if (IsActiveState && !actionState && totalTimeInState > 3)
            {
                service.SetMusicState(MusicState.Action);
            }
            else if (!IsActiveState && actionState)
            {
                service.SetMusicState(MusicState.Neutral);
            }
        }
    }
}