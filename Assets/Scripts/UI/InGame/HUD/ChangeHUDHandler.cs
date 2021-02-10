using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.UI.InGame
{
    public class ChangeHUDHandler : UiMenu
    {
        public InGameMenu menu;
        public HUD.HUD HUD;
        public GameObject[] HUDelements;
        public bool elementSelected = false;
        public GameObject selectedElement;
        public Slider scaleSlider;
        public TMP_Text elementLabel;
        public Toggle toggleVisibility;

        public void Update()
        {
            if(selectedElement != null)
            {
                elementLabel.text = selectedElement.name;
                selectedElement.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, 1);
                selectedElement.GetComponent<CustomizableHUDElement>().isVisible = toggleVisibility.isOn;
            }

            if(HUD.inEditMode)
            {
                foreach(GameObject element in HUDelements)
                {
                    element.SetActive(true); //WHILE IN EDIT MODE
                }
            } else
            {
                SetVisibility();
            }

        }

        public void SetVisibility()
        {
            foreach(GameObject element in HUDelements)
            {
                element.SetActive(element.GetComponent<CustomizableHUDElement>().isVisible);
            }
        }

        // Called when the user starts editing the HUD after clicking the "Change HUD" button.
        public void EnterEditMode()
        {
            HUD.inEditMode = true;

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
           
            HUD.inEditMode = false;
            PlayerPrefs.SetInt("hasCustomHUD", 1); //CUSTOM
            //GameCursor.CameraMode = previousCameraMode;
            SetVisibility();
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
