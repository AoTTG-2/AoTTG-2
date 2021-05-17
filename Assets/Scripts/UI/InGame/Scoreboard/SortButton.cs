using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.UI.InGame.Scoreboard
{
    public class SortButton : MonoBehaviour
    {
        public TMP_Text sortingLabel;
        public string defaultLabel;
        private Scoreboard scoreboard;

        void Start()
        {
            sortingLabel = GetComponentInChildren<TMP_Text>();
            defaultLabel = sortingLabel.text;
            
            GetComponent<Button>().onClick.AddListener(Sort);

            scoreboard = GameObject.Find("Scoreboard").GetComponent<Scoreboard>();

            //sort score by default
            if(defaultLabel == "SCORE") Sort();
        }

        public void Sort()
        {
            scoreboard.sortLabel = defaultLabel;

            if(sortingLabel.text == scoreboard.sortLabel)
            {
                SetIndicator();
            }

            foreach(SortButton button in scoreboard.sortButtons)
            {
                if(button.defaultLabel != defaultLabel)
                {
                    button.SetDefault();
                }
            }

        }

        void SetIndicator()
        {
            if(!sortingLabel.text.Contains("↓")) sortingLabel.text += " ↓";
        }

        void SetDefault()
        {
            sortingLabel.text = defaultLabel;
        }
    }
}
