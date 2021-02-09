using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class ChangeHUDHandler : UiMenu
    {
        public bool inEditMode; // True if the user is changing the HUD.
        //private CameraMode previousCameraMode;
        public InGameMenu menu;

        void Start()
        {
            inEditMode = false;
        }

        // Called when the user starts editing the HUD after clicking the "Change HUD" button.
        public void EnterEditMode()
        {
            inEditMode = true;

            // TODO: Try to figure out how to stop camera from moving when in the menu. Below was my attempt but the cursor would magically disappear.
            //previousCameraMode = GameCursor.CameraMode;
            //GameCursor.CameraMode = CameraMode.WOW;
            this.Show();
            menu.Hide();
        }

        // Called when the user clicks "Save" after editing their HUD. 
        public void SaveHudLayout()
        {
            Debug.Log("got into save hud layout");
            inEditMode = false;
            //GameCursor.CameraMode = previousCameraMode;
            this.Hide();
            menu.Show();
        }
    }
}
