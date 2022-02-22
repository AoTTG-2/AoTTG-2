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
}