using UnityEngine;
using Assets.Scripts.Services;

namespace Assets.Scripts.Characters.Humans
{
    public class CombatTimer : StateTimer
    {
        protected override void SetState()
        {
            var service = Service.Music;
            var currentState = service.ActiveState;
            var combatState = currentState.Equals(MusicState.Combat);
            if (IsActiveState && !combatState)
            {
                service.SetMusicState(MusicState.Combat);
            }
            else if (!IsActiveState && combatState)
            {
                service.SetMusicState(MusicState.Neutral);
            }
        }
    }
}