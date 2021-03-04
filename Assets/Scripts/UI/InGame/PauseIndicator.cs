using Assets.Scripts.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class PauseIndicator : UiElement
    {
        private bool UnPausing { get; set; }

        /// <summary>
        /// Displays the pause indicator with the text "Game Paused".
        /// </summary>
        public void Pause()
        {
            if (PhotonNetwork.offlineMode)
            {
                GameObject.Find("Gamemode").GetComponent<Gamemode.RacingGamemode>().enabled = false;
                return;
            }
            GameObject.Find("Gamemode").GetComponent<Gamemode.RacingGamemode>().enabled = false;
            gameObject.GetComponentInChildren<Text>().text = "Game Paused";
            UnPausing = false;
            Show();
        }

        /// <summary>
        /// Sets the process to remove the pause indicator.
        /// </summary>
        public void UnPause()
        {
            if (PhotonNetwork.offlineMode)
            {
                GameObject.Find("Gamemode").GetComponent<Gamemode.RacingGamemode>().enabled = true;
                return;
            }
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
                    GameObject.Find("Gamemode").GetComponent<Gamemode.RacingGamemode>().enabled = true;
                    Hide();
                }
            }
        }
    }
}
