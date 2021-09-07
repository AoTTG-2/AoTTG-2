using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame.Scoreboard
{
    /// <summary>
    /// PlayerInfo class which is used for the <see cref="Scoreboard"/>
    /// </summary>
    public class PlayerInfo : MonoBehaviour
    {
        public bool isMine = false;
        public TMP_Text playerId;
        public TMP_Text playerName;
        public TMP_Text playerUsername;
        public TMP_Text playerScore;
        public TMP_Text playerKills;
        public TMP_Text playerDeaths;
        public TMP_Text playerHighest;
        public TMP_Text playerTotal;
        public TMP_Text playerPing;

        private Image bg;

        void Start()
        {
            bg = GetComponent<Image>();
            TMP_Text[] allTexts = GetComponentsInChildren<TMP_Text>();
            Image[] allImages = GetComponentsInChildren<Image>();

            if (isMine)
            {
                foreach (TMP_Text text in allTexts)
                {
                    text.color = Color.black;
                }

                foreach (Image img in allImages)
                {
                    img.color = Color.black;
                }

                bg.color = Color.white;

            }
            else
            {
                foreach (TMP_Text text in allTexts)
                {
                    text.color = Color.white;
                }

                foreach (Image img in allImages)
                {
                    img.color = Color.white;
                }

                bg.color = new Vector4(0, 0, 0, 0);
            }
        }
    }
}