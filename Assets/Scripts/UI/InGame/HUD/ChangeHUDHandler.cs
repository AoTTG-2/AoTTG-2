using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class ChangeHUDHandler : UiMenu
    {
        public bool inEditMode; // True if the user is changing the HUD.
        public InGameMenu menu;
        public HUD.HUD HUD;
        public GameObject[] HUDelements;
        public bool elementSelected = false;

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
            foreach(GameObject element in HUDelements)
            {
                element.GetComponent<CustomizableHUDElement>().SavePosition();
            }

            Debug.Log("SAVED!!!");
           
            inEditMode = false;
            PlayerPrefs.SetInt("hasCustomHUD", 1); //CUSTOM
            //GameCursor.CameraMode = previousCameraMode;
            this.Hide();
            menu.Show();

        }

        public void LoadDefaultHudLayout()
        {
            foreach(GameObject element in HUDelements)
            {
                element.GetComponent<CustomizableHUDElement>().LoadDefault();
                PlayerPrefs.SetInt("hasCustomHUD", 0);
            }
        }

        public void LoadCustomHudLayout()
        {
            foreach(GameObject element in HUDelements)
            {
                element.GetComponent<CustomizableHUDElement>().LoadCustom();
                PlayerPrefs.SetInt("hasCustomHUD", 1);
            }
        }

        
    }
}
