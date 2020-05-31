using UnityEngine;

public class CursorManagement : MonoBehaviour
{
    private static Mode CurrentCameraMode = Mode.Menu;

    public enum Mode
    {
        Menu,
        Loading,
        Original,
        TPS,
        WOW
    }

    public static Mode CameraMode
    {
        set { SetCameraMode(value); }
        get { return CurrentCameraMode; }
    }

    private static void SetCameraMode(Mode newCameraMode)
    {
        switch (CurrentCameraMode)
        {
            case Mode.Menu:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                return;

            case Mode.TPS:
            case Mode.Loading:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                return;

            case Mode.Original:
            case Mode.WOW:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                return;
        }

        CurrentCameraMode = newCameraMode;
    }

    private static void ReapplyCameraMode()
    {
        SetCameraMode(CurrentCameraMode);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
            ReapplyCameraMode();
    }
}