using System;
using UnityEngine;

public class GameCursor : MonoBehaviour
{
    private const string CameraModeKey = "cameraType";

    private static CameraMode _CameraMode = CameraMode.Original;
    private static CursorMode _CursorMode = CursorMode.Menu;

    public static CameraMode CameraMode
    {
        get { return _CameraMode; }
        set { SetCameraMode(value); }
    }

    public static CursorMode CursorMode
    {
        get { return _CursorMode; }
        set { SetCursorMode(value); }
    }

    public static void ApplyCameraMode()
    {
        SetCameraMode(CameraMode);
    }

    public static void ApplyCursorMode()
    {
        SetCursorMode(CursorMode);
    }

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

    private static void LoadPlayerPrefs()
    {
        var savedValue = PlayerPrefs.GetInt(CameraModeKey, (int)CameraMode);
        if (Enum.IsDefined(typeof(CameraMode), savedValue))
        {
            _CameraMode = (CameraMode)savedValue;
            SetPreferredCameraMode(_CameraMode);
        }
    }

    private static void SetCameraMode(CameraMode newCameraMode)
    {
        Debug.Log($"{CameraMode} -> {newCameraMode}");
        _CameraMode = newCameraMode;
        SetPreferredCameraMode(newCameraMode);

        switch (CameraMode)
        {
            case CameraMode.TPS:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case CameraMode.Original:
            case CameraMode.WOW:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }

    private static void SetCursorMode(CursorMode newCursorMode)
    {
        Debug.Log($"{CursorMode} -> {newCursorMode}");
        _CursorMode = newCursorMode;

        switch (CursorMode)
        {
            case CursorMode.Menu:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;

            case CursorMode.Loading:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }

    private static void SetPreferredCameraMode(CameraMode value)
    {
        PlayerPrefs.SetInt(CameraModeKey, (int)CameraMode);
    }

    private void Awake()
    {
        LoadPlayerPrefs();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            if (MenuManager.IsMenuOpen)
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
        if (MenuManager.IsMenuOpen)
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