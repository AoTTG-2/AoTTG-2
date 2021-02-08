using Assets.Scripts.UI.Input;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIInputHandler : MonoBehaviour
    {
        private GameObject interactionWheel;
        private GameObject leaderboard;

        private void Start()
        {
            interactionWheel = gameObject.GetComponentInChildren<InteractionWheel>(true).gameObject;
            leaderboard = gameObject.GetComponentInChildren<Leaderboard>(true).gameObject;
        }

        private void Update()
        {
            if (InputManager.KeyDown(InputUi.InteractionWheel))
            {
                if (!interactionWheel.activeSelf)
                    interactionWheel.SetActive(true);
            }

            if (InputManager.KeyUp(InputUi.InteractionWheel))
            {
                if (interactionWheel.activeSelf)
                    interactionWheel.SetActive(false);
            }

            if (InputManager.KeyDown(InputUi.Leaderboard))
            {
                if (!leaderboard.activeSelf)
                    leaderboard.SetActive(true);
            }

            if (InputManager.KeyUp(InputUi.Leaderboard))
            {
                if (leaderboard.activeSelf)
                    leaderboard.SetActive(false);
            }

        }
    }
}