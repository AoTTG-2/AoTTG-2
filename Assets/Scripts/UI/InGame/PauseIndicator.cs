using Assets.Scripts.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class PauseIndicator : MonoBehaviour
    {
        private bool UnPausing { get; set; }
        public bool ShowUi { get; set; } = true; 

        /// <summary>
        /// Displays the pause indicator with the text "Game Paused"
        /// </summary>
        public void Pause()
        {
            if (!ShowUi) return;
            gameObject.GetComponentInChildren<Text>().text = "Game Paused";
            gameObject.SetActive(true);
            UnPausing = false;
        }

        /// <summary>
        /// Sets the process to remove the pause indicator
        /// </summary>
        public void UnPause()
        {
            UnPausing = true;
        }
        
        // Update is called once per frame
        private void Update()
        {
            // Only need constant updates when unpausing
            if (UnPausing)
            {
                float timeRemaining = Service.Pause.PauseTimer;
                gameObject.GetComponentInChildren<Text>().text = $"Unpausing in:\n{timeRemaining:0.00}";
                if (timeRemaining <= 0f)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
