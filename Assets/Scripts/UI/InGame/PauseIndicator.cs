using Assets.Scripts.Services;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    /// <summary>
    /// A popup indicating that the game is paused or is about to get unpaused
    /// </summary>
    public class PauseIndicator : UiElement
    {
        private bool UnPausing { get; set; }

        /// <summary>
        /// Displays the pause indicator with the text "Game Paused".
        /// </summary>
        public void Pause()
        {
            if (PhotonNetwork.offlineMode) return;

            gameObject.GetComponentInChildren<Text>().text = "Game Paused";
            UnPausing = false;
            Show();
        }

        /// <summary>
        /// Sets the process to remove the pause indicator.
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
                    Hide();
                }
            }
        }
    }
}
