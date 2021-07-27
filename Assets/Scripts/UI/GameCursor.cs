using System;
using UnityEngine;

/// <summary>
/// The GameCursor class managed logic related to the behavior of the cursor
/// </summary>
public class GameCursor : MonoBehaviour
{
    private const string CameraModeKey = "cameraType";

    private static CameraMode _cameraMode = CameraMode.Original;
    private static CursorMode _cursorMode = CursorMode.Menu;

    private static bool _forceFreeCursor = false;

    /// <summary>
    /// The current CameraMode
    /// </summary>
    public static CameraMode CameraMode
    {
        get { return _cameraMode; }
        set { SetCameraMode(value); }
    }

    /// <summary>
    /// The current CursorMode
    /// </summary>
    public static CursorMode CursorMode
    {
        get { return _cursorMode; }
        set { SetCursorMode(value); }
    }

    /// <summary>
    /// Forces the Cursor to be visible in a non-locked state
    /// </summary>
    public static bool ForceFreeCursor
    {
        get
        {
            return _forceFreeCursor;
        }

        set
        {
            _forceFreeCursor = value;

            if (value)
                ApplyFreeCursor();
            else if (MenuManager.IsAnyMenuOpen)
                ApplyCursorMode();
            else
                ApplyCameraMode();
        }
    }

    public static void ApplyCameraMode()
    {
        SetCameraMode(CameraMode);
    }

    public static void ApplyCursorMode()
    {
        SetCursorMode(CursorMode);
    }

    /// <summary>
    /// Changes the Current CameraMode
    /// </summary>
    public static void Cycle()
    {
        switch (CameraMode)
        {
            case CameraMode.Original:
                CameraMode = CameraMode.WOW;
                break;

            case CameraMode.WOW:
                CameraMode = CameraMode.TPS;
                break;

            case CameraMode.TPS:
                CameraMode = CameraMode.Original;
                break;

            default:
                Debug.LogWarning($"Tried to cycle in {CameraMode} mode");
                return;
        }
    }

    private static void ApplyFreeCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private static void LoadPlayerPrefs()
    {
        var savedValue = PlayerPrefs.GetInt(CameraModeKey, (int) CameraMode);
        if (Enum.IsDefined(typeof(CameraMode), savedValue))
        {
            _cameraMode = (CameraMode) savedValue;
            SetPreferredCameraMode(_cameraMode);
        }
    }

    private static void SetCameraMode(CameraMode newCameraMode)
    {
        _cameraMode = newCameraMode;
        SetPreferredCameraMode(newCameraMode);

        if (!ForceFreeCursor)
        {
            switch (CameraMode)
            {
                case CameraMode.TPS:
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    break;

                case CameraMode.Original:
                case CameraMode.WOW:
                    Cursor.visible = false;

#if UNITY_EDITOR

                    // Confined does nothing in the editor,
                    // so I switch to the closest we can get.
                    Cursor.lockState = CursorLockMode.None;
#else
                    Cursor.lockState = CursorLockMode.Confined;
#endif

                    break;
            }
        }
    }

    private static void SetCursorMode(CursorMode newCursorMode)
    {
        _cursorMode = newCursorMode;

        if (!ForceFreeCursor)
        {
            switch (CursorMode)
            {
                case CursorMode.Menu:
                case CursorMode.Loading:
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    break;
            }
        }
    }

    private static void SetPreferredCameraMode(CameraMode value)
    {
        PlayerPrefs.SetInt(CameraModeKey, (int) value);
    }

    private void Awake()
    {
        LoadPlayerPrefs();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            if (MenuManager.IsAnyMenuOpen)
                ApplyCursorMode();
            else
                ApplyCameraMode();
        }
    }

    private void OnDisable()
    {
        MenuManager.MenuOpened -= OnMenuOpened;
        MenuManager.MenuClosed -= OnMenuClosed;
    }

    private void OnEnable()
    {
        MenuManager.MenuOpened += OnMenuOpened;
        MenuManager.MenuClosed += OnMenuClosed;
    }

    private void OnLevelWasLoaded()
    {
        if (MenuManager.IsAnyMenuOpen)
            CursorMode = CursorMode.Menu;
    }

    private void OnMenuClosed()
    {
        ApplyCameraMode();
    }

    private void OnMenuOpened()
    {
        CursorMode = CursorMode.Menu;
    }
}