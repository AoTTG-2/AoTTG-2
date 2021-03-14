using Assets.Scripts.UI.Input;
using UnityEngine;

public class HorseController : MonoBehaviour
{
    public float CurrentDirection;
    public bool ShouldJump;
    public bool ShouldWalk;
    public float TargetDirection;
    private new Camera camera;

    private void Start()
    {
        camera = Camera.main;
        if (PhotonNetwork.offlineMode)
            enabled = false;
    }

    private void Update()
    {
        var inputY =
            (InputManager.Key(InputHorse.Forward) ? 1 : 0)
            + (InputManager.Key(InputHorse.Backward) ? -1 : 0);

        var inputX =
            (InputManager.Key(InputHorse.Left) ? -1 : 0)
            + (InputManager.Key(InputHorse.Right) ? 1 : 0);

        if (inputX != 0 || inputY != 0)
        {
            var cameraRotY = camera.transform.rotation.eulerAngles.y;
            var inputRotY = -(Mathf.Atan2(inputY, inputX) * Mathf.Rad2Deg) + 90f;
            TargetDirection = cameraRotY + inputRotY;
        }
        else
        {
            TargetDirection = -874f;
        }

        if (TargetDirection != -874f)
            CurrentDirection = TargetDirection;

        ShouldJump = InputManager.Key(InputHorse.Jump);
        ShouldWalk = InputManager.Key(InputHorse.Walk);
    }
}