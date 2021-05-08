using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame.HUD
{
    public class ChangeHudHandler : UiMenu
    {
        public InGameMenu menu;
        public HUD HUD;
        public GameObject[] HUDelements;
        public bool elementSelected = false;
        public GameObject selectedElement;
        public Slider scaleSlider;
        public TMP_Text elementLabel;
        public Toggle toggleVisibility;
        public bool hasChanged = false;
        
        public void Update()
        {
            if (HUD.inEditMode)
            {
                if (selectedElement != null)
                {
                    elementLabel.text = selectedElement.GetComponent<CustomizableHudElement>().elementName;
                    selectedElement.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, 1);
                    selectedElement.GetComponent<CustomizableHudElement>().isVisible = toggleVisibility.isOn;
                }
                else
                {
                    elementLabel.text = "Select an Element";
                }
            }
            else
            {
                SetVisibility();
            }
        }

        public void SetVisibility()
        {
            foreach (GameObject element in HUDelements)
            {
                element.GetComponent<CustomizableHudElement>().SetVisibility();
            }
        }

        // Called when the user starts editing the HUD after clicking the "Change HUD" button.
        public void EnterEditMode()
        {
            HUD.inEditMode = true;

            foreach (GameObject element in HUDelements)
            {
                //You should want to always have the elements visible whenever you're on the edit mode.
                element.GetComponent<CustomizableHudElement>().ShowElement();
                element.GetComponent<CustomizableHudElement>().AnimateCustomization();
                element.GetComponent<CustomizableHudElement>().SaveTempPosition();
            }

            this.Show();
            menu.Hide();
        }

        // Called when the user clicks "Save" after editing their HUD. 
        public void SaveHudLayout()
        {
            if (hasChanged)
            {
                foreach (GameObject element in HUDelements)
                {
                    element.GetComponent<CustomizableHudElement>().SavePosition();
                    element.GetComponent<CustomizableHudElement>().StopCustomization();
                }

                PlayerPrefs.SetInt("hasCustomHUD", 1);

                hasChanged = false;
            }
            else
            {
                foreach (GameObject element in HUDelements)
                {
                    element.GetComponent<CustomizableHudElement>().StopCustomization();
                }
            }

            HUD.inEditMode = false;
            SetVisibility();
            this.Hide();
            menu.Show();
            ClearSelection();
        }

        public void LoadDefaultHudLayout()
        {
            foreach (GameObject element in HUDelements)
            {
                element.GetComponent<CustomizableHudElement>().LoadDefault();
            }
            PlayerPrefs.SetInt("hasCustomHUD", 0);
            ClearSelection();
        }

        public void LoadCustomHudLayout()
        {
            foreach (GameObject element in HUDelements)
            {
                element.GetComponent<CustomizableHudElement>().LoadCustom();
            }
            PlayerPrefs.SetInt("hasCustomHUD", 1);
            ClearSelection();
        }

        private void ClearSelection()
        {
            selectedElement = null;
        }

        public void CancelChanges()
        {
            foreach (GameObject element in HUDelements)
            {
                element.GetComponent<CustomizableHudElement>().StopCustomization();
                element.GetComponent<CustomizableHudElement>().LoadTempPosition();
            }
            HUD.inEditMode = false;
            SetVisibility();
            this.Hide();
            menu.Show();
            ClearSelection();
        }

    }
}
