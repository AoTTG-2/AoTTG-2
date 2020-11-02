using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class PauseIndicator : MonoBehaviour
    {
        private FengGameManagerMKII GameManager { get; set; }
        private bool UnPausing { get; set; }

        /// <summary>
        /// Displays the pause indicator with the text "Game Paused"
        /// </summary>
        public void Pause()
        {
            gameObject.GetComponentInChildren<Text>().text = "Game Paused";
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Sets the process to remove the pause indicator
        /// </summary>
        public void UnPause()
        {
            UnPausing = true;
        }

        // Use this for initialization
        private void Start()
        {
            GameManager = FengGameManagerMKII.instance;
        }

        // Update is called once per frame
        private void Update()
        {
            // Only need constant updates when unpausing
            if (UnPausing)
            {
                float timeRemaining = GameManager.pauseWaitTime;
                gameObject.GetComponentInChildren<Text>().text = $"Unpausing in:\n{timeRemaining:0.00}";
                if (timeRemaining <= 0f)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
