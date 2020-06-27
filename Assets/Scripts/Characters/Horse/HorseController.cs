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
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            enabled = false;
    }

    private void Update()
    {
        var inputY =
            (InputManager.KeyPressed(InputHorse.Forward) ? 1 : 0)
            + (InputManager.KeyPressed(InputHorse.Backward) ? -1 : 0);

        var inputX =
            (InputManager.KeyPressed(InputHorse.Left) ? -1 : 0)
            + (InputManager.KeyPressed(InputHorse.Right) ? 1 : 0);

        if (inputX != 0 || inputY != 0)
        {
            var cameraRotY = camera.transform.rotation.eulerAngles.y;
            var inputRotY = -(Mathf.Atan2(inputY, inputX) * 57.29578f) + 90f;
            TargetDirection = cameraRotY + inputRotY;
        }
        else
        {
            TargetDirection = -874f;
        }

        if (TargetDirection != -874f)
            CurrentDirection = TargetDirection;

        ShouldJump = InputManager.KeyDown(InputHorse.Jump);
        ShouldWalk = InputManager.KeyPressed(InputHorse.Walk);
    }
}