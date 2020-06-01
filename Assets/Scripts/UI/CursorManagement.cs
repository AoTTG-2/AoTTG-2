using UnityEngine;

public class CursorManagement : MonoBehaviour
{
    private const string CameraModeKey = "cameraType";

    private static Mode PreferredMode = Mode.Original;
    private static Mode CurrentMode = Mode.Menu;

    public enum Mode : int
    {
        Menu,
        Loading,
        Original,
        TPS,
        WOW
    }

    public static Mode PreferredCameraMode
    {
        get { return PreferredMode; }
        set { SetPreferredCameraMode(value); }
    }

    public static Mode CameraMode
    {
        get { return CurrentMode; }
        set { SetCameraMode(value); }
    }

    public static void Cycle()
    {
        switch (CameraMode)
        {
            case Mode.Original:
                CameraMode = Mode.WOW;
                break;

            case Mode.WOW:
                CameraMode = Mode.TPS;
                break;

            case Mode.TPS:
                CameraMode = Mode.Original;
                break;

            default:
                Debug.LogWarning($"Tried to cycle in {CameraMode} mode");
                return;
        }

        PreferredCameraMode = CameraMode;
    }

    private static void SetPreferredCameraMode(Mode value)
    {
        PreferredMode = value;
        PlayerPrefs.SetInt(CameraModeKey, (int)PreferredMode);
    }

    private static void SetCameraMode(Mode newCameraMode)
    {
        Debug.Log($"{CurrentMode} -> {newCameraMode}");
        CurrentMode = newCameraMode;

        switch (CurrentMode)
        {
            case Mode.Menu:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;

            case Mode.TPS:
            case Mode.Loading:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case Mode.Original:
            case Mode.WOW:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }

    private static void ReapplyCameraMode()
    {
        SetCameraMode(CurrentMode);
    }

    private void Awake()
    {
        SetPreferredCameraMode((Mode)PlayerPrefs.GetInt(CameraModeKey, (int)PreferredMode));
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
            ReapplyCameraMode();
    }
}