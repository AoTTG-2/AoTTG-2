using UnityEngine;

namespace Assets.Scripts.Characters.Humans
{
    public class CombatTimer : MonoBehaviour
    {
        private int maxTimer;
        private float timer;
        public bool IsEngaged { get { return timer > 0; } }
        public CombatTimer()
        {
            this.maxTimer = 15;
        }

        private void Awake()
        {
            enabled = true;
        }

        private void FixedUpdate()
        {
            SubtractTime();
            CheckState();
        }

        private void SubtractTime()
        {
            var deltaTime = Time.deltaTime;
            var result = timer - deltaTime;
            timer = result < 0 ? 0 : result;
        }

        private void CheckState()
        {
            var currentState = AudioController.Instance.GetActiveState();
            var combatState = currentState.Equals(AudioState.Combat);
            if (IsEngaged && !combatState)
            {
                AudioController.Instance.SetState(AudioState.Combat);
            }
            else if (!IsEngaged && combatState)
            {
                AudioController.Instance.SetState(AudioState.Neutral);
            }
            
        }

        public void AddTime(int time)
        {
            var total = timer + time;
            timer = (total < maxTimer) ? total : maxTimer;
        }
    }
}