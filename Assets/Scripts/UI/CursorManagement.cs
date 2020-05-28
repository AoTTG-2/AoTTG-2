using UnityEngine;

public class CursorManagement : MonoBehaviour 
{
    private bool savedVisible;
    private CursorLockMode savedLockState;

    public static void SwitchToMenuMode()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; 
    }
    public static void SwitchToLoadingOrTPSMode()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public static void SwitchToOriginalOrWOWMode()
    {
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        /* There are a few problems with this: 
         * 1) OnApplicationFocus() does appear to be called at some point with hasFocus = true before it is called with
         * hasFocus = false. This means that the current saved cursor state will be undefined. I can't tell if this is
         * right when the application is loaded, when a scene is loaded, or something else. Regardless, it's a problem. 
         * This is what the output log said: "Loading saved cursor state. Cursor.visible: False, Cursor.lockState: None" 
         * 2) For some reason, when alt+tabbing out of the game, before OnApplicationFocus() is called something is setting
         * Cursor.lockState = CursorLockMode.None, but not setting Cursor.visible = true. 
         * I have no idea why this is happening, and I suspect I will have to dig through the code to find out. 
         * I was playing the game on TPS mode with no menus open or anything, I alt+tabbed out, and this appears to be what
         * happened: "Saving cursor state. Cursor.visible: False, Cursor.lockState: None". Cursor.visible is correct, but 
         * Cursor.lockState is not. It did, however, load this saved state correctly; but of course TPS mode was then screwed up.
         */

        if (hasFocus)
        {
            //Load saved Cursor state
            Cursor.visible = this.savedVisible;
            Cursor.lockState = this.savedLockState;

            Debug.Log("Loading saved cursor state. Cursor.visible: " + Cursor.visible + ", Cursor.lockState: " + Cursor.lockState);
        }
        else
        {
            //Save current Cursor state
            this.savedVisible = Cursor.visible;
            this.savedLockState = Cursor.lockState;
            Debug.Log("Saving cursor state. Cursor.visible: " + Cursor.visible + ", Cursor.lockState: " + Cursor.lockState); 
        }
    }

}
