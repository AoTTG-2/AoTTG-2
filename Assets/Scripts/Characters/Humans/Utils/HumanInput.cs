using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Characters.Humans.Utils
{
    public class HumanInput : MonoBehaviour
    {
        public InputActions InputActions { get; private set; }
        public InputActions.HumanActions HumanActions { get; private set; }

        private void Awake()
        {
            InputActions = new InputActions();

            HumanActions = InputActions.Human;
        }
        private void OnEnable()
        {
            InputActions.Enable();
        }
        private void OnDisable()
        {
            InputActions.Disable();
        }

        public void DisableActionFor(InputAction action, float seconds)
        {
            StartCoroutine(DisableAction(action, seconds));
        }

        private IEnumerator DisableAction(InputAction action, float seconds)
        {
            action.Disable();
            yield return new WaitForSeconds(seconds);
            action.Enable();
        }
    }
}
