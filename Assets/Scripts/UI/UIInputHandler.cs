using UnityEngine;
public class UIInputHandler : MonoBehaviour
{

    public FengCustomInputs inputManager;
    private GameObject interactionWheel;

    private void Start()
    {
        interactionWheel = gameObject.GetComponentInChildren<InteractionWheel>(true).gameObject;
    }

    private void Update()
    {

        if (inputManager == null) return;

        if (inputManager.isInput[InputCode.interactionWheel])
        {
            if (!interactionWheel.activeSelf)
                interactionWheel.SetActive(true);
        }

        if (inputManager.isInputUp[InputCode.interactionWheel])
        {
            if (interactionWheel.activeSelf)
                interactionWheel.SetActive(false);
        }

    }
}