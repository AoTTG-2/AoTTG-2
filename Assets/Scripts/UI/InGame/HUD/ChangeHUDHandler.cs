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
            if(HUD.inEditMode)
            {
                if(selectedElement != null)
                {
                    elementLabel.text = selectedElement.name;
                    selectedElement.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, 1);
                    selectedElement.GetComponent<CustomizableHUDElement>().isVisible = toggleVisibility.isOn;
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

            foreach(GameObject element in HUDelements)
            {   
                //You should want to always have the elements visible whenever you're on the edit mode.
                element.SetActive(true);
                element.GetComponent<CustomizableHUDElement>().AnimateCustomization();
            }

            // TODO: Try to figure out how to stop camera from moving when in the menu. Below was my attempt but the cursor would magically disappear.
            
            this.Show();
            menu.Hide();
        }

        // Called when the user clicks "Save" after editing their HUD. 
        public void SaveHudLayout()
        {
            foreach(GameObject element in HUDelements)
            {
                element.GetComponent<CustomizableHUDElement>().SavePosition();
                element.GetComponent<CustomizableHUDElement>().StopCustomization();
            }
           
            ClearSelection();
            HUD.inEditMode = false;
            PlayerPrefs.SetInt("hasCustomHUD", 1);
            SetVisibility();
            this.Hide();
            menu.Show();

        }

        public void LoadDefaultHudLayout()
        {
            foreach(GameObject element in HUDelements)
            {
                element.GetComponent<CustomizableHUDElement>().LoadDefault();
            }

            ClearSelection();
        }

        public void LoadCustomHudLayout()
        {
            foreach(GameObject element in HUDelements)
            {
                element.GetComponent<CustomizableHUDElement>().LoadCustom();
                PlayerPrefs.SetInt("hasCustomHUD", 1);
            }
            
            ClearSelection();
        }

        private void ClearSelection()
        {
            selectedElement = null;
            elementLabel.text = "";
        }

        
    }
}
