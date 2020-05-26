using UnityEngine;

public class CursorManagement : MonoBehaviour 
{
    private bool savedVisible;
    private CursorLockMode savedLockState;

    public static void switchToMenuMode()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; 
    }
    public static void switchToLoadingOrTPSMode()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public static void switchToOriginalOrWOWMode()
    {
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            //Load saved Cursor state
            Cursor.visible = this.savedVisible;
            Cursor.lockState = this.savedLockState; 
        }
        else
        {
            //Save current Cursor state
            this.savedVisible = Cursor.visible;
            this.savedLockState = Cursor.lockState;
        }
    }

}
