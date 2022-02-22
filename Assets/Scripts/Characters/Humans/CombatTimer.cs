using UnityEngine;
using Assets.Scripts.Services;

namespace Assets.Scripts.Characters.Humans
{
    public class CombatTimer : StateTimer
    {
        protected override void SetState()
        {
            var service = Service.Audio;
            var currentState = service.GetCurrentState();
            var combatState = currentState.Equals(AudioState.Combat);
            if (IsActiveState && !combatState)
            {
                service.InvokeAudioStateChanged(AudioState.Combat);
            }
            else if (!IsActiveState && combatState)
            {
                service.InvokeAudioStateChanged(AudioState.Neutral);
            }
        }
    }

    public class SpeedTimer : CombatTimer
    {
        protected override void SetState()
        {
            var service = Service.Audio;
            var currentState = service.GetCurrentState();
            var actionState = currentState.Equals(AudioState.Action);
            var combatState = currentState.Equals(AudioState.Combat);
            if (IsActiveState && !actionState && !combatState)
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