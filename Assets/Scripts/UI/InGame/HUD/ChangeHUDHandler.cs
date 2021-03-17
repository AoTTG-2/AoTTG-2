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
        public bool hasChanged = false;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable() {
            base.OnDisable();
		}
        
        public void Update()
        {
            if(HUD.inEditMode)
            {
                if(selectedElement != null)
                {
                    elementLabel.text = selectedElement.GetComponent<CustomizableHUDElement>().elementName;
                    selectedElement.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, 1);
                    selectedElement.GetComponent<CustomizableHUDElement>().isVisible = toggleVisibility.isOn;
                } else
                {
                    elementLabel.text = "Select an Element";
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
                element.GetComponent<CustomizableHUDElement>().SetVisibility();
            }
        }

        // Called when the user starts editing the HUD after clicking the "Change HUD" button.
        public void EnterEditMode()
        {
            HUD.inEditMode = true;

            foreach(GameObject element in HUDelements)
            {   
                //You should want to always have the elements visible whenever you're on the edit mode.
                element.GetComponent<CustomizableHUDElement>().ShowElement();
                element.GetComponent<CustomizableHUDElement>().AnimateCustomization();
                element.GetComponent<CustomizableHUDElement>().SaveTempPosition();
            }
            
            this.Show();
            menu.Hide();
        }

        // Called when the user clicks "Save" after editing their HUD. 
        public void SaveHudLayout()
        {
            if(hasChanged)
            {
                foreach(GameObject element in HUDelements)
                {
                    element.GetComponent<CustomizableHUDElement>().SavePosition();
                    element.GetComponent<CustomizableHUDElement>().StopCustomization();
                }

                PlayerPrefs.SetInt("hasCustomHUD", 1);

                hasChanged = false;
            } else
            {
                foreach(GameObject element in HUDelements)
                {
                    element.GetComponent<CustomizableHUDElement>().StopCustomization();
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
            foreach(GameObject element in HUDelements)
            {
                element.GetComponent<CustomizableHUDElement>().LoadDefault();
            }
            PlayerPrefs.SetInt("hasCustomHUD", 0);
            ClearSelection();
        }

        public void LoadCustomHudLayout()
        {
            foreach(GameObject element in HUDelements)
            {
                element.GetComponent<CustomizableHUDElement>().LoadCustom();
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
            foreach(GameObject element in HUDelements)
            {
                element.GetComponent<CustomizableHUDElement>().StopCustomization();
                element.GetComponent<CustomizableHUDElement>().LoadTempPosition();
            }
            HUD.inEditMode = false;
            SetVisibility();
            this.Hide();
            menu.Show();
            ClearSelection();
        }
        
    }
}
