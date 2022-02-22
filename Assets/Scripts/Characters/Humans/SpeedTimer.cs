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
            var service = Service.Audio;
            var currentState = service.GetCurrentState();
            var actionState = currentState.Equals(AudioState.Action);
            if (IsActiveState && !actionState && totalTimeInState > 5)
            {
                service.InvokeAudioStateChanged(AudioState.Action);
            }
            else if (!IsActiveState && actionState)
            {
                service.InvokeAudioStateChanged(AudioState.Neutral);
            }
        }
    }
}