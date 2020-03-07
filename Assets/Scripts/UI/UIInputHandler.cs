using UnityEngine;

public class UIInputHandler : MonoBehaviour
{

    public FengCustomInputs inputManager;
    private GameObject interactionWheel;

    void Start()
    {
        interactionWheel = gameObject.GetComponentInChildren<CircularMenu>(true).gameObject;
    }
	
	void Update () 
    {

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
