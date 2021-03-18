using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MapEditor
{
    public class ToolButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
        IPointerUpHandler
    {
        #region Data Members

        //A list of the states the button can be in
        private enum ButtonState
        {
            Unselected,
            Pressed,
            Selected
        }

        //Determines if the left mouse button is currently pressed or not
        private static bool _mouseDown;

        //The button script that is currently pressed down
        private static ToolButtonManager _pressedButton;

        //The button that is currently selected
        private static ToolButtonManager _selectedButton;

        //A dictionary mapping the different tools to their respective button scripts
        private static readonly Dictionary<Tool, ToolButtonManager> ToolTable =
            new Dictionary<Tool, ToolButtonManager>();

        //The images used for the button
        [SerializeField] private UnityEngine.Sprite unselected;
        [SerializeField] private UnityEngine.Sprite pressed;
        [SerializeField] private UnityEngine.Sprite selected;

        //The image component attached to this game object
        private Image imageScript;

        //The current state of the button
        [SerializeField] private ButtonState currentState;

        //The tool this button corresponds to
        [SerializeField] private Tool toolType;

        //The keyboard key that selects this button
        [SerializeField] private KeyCode shortCutKey;

        #endregion


        //Initialize data members and set up the triggers
        private void Start()
        {
            //Save the image component
            imageScript = gameObject.GetComponent<Image>();

            //Add this button management script to the static table
            ToolTable.Add(toolType, this);

            //If this button is the one to be selected by default, select it
            if (currentState == ButtonState.Selected)
                Select();
            //Otherwise the button should be unselected
            else
                Unselect();

            _mouseDown = false;
        }


        //Check if the shortcut key was pressed
        private void Update()
        {
            if (EditorManager.Instance.CurrentMode == EditorMode.Edit &&
                EditorManager.Instance.ShortcutsEnabled &&
                Input.GetKeyDown(shortCutKey))
            {
                _selectedButton.Unselect();
                Select();
            }
        }


        #region Events

        //If the game loses focus and a button is pressed down, unpress it
        private void OnApplicationFocus(bool focus)
        {
            if (!focus && currentState == ButtonState.Pressed)
                Unselect();
        }

        #endregion

        #region Button Methods

        //Change the image and state to selected
        private void Select()
        {
            //Set the button images
            imageScript.sprite = selected;
            currentState = ButtonState.Selected;
            _selectedButton = this;
            //Execute the action of the button
            PerformAction();
        }

        //Change the image and state to unselected
        private void Unselect()
        {
            imageScript.sprite = unselected;
            currentState = ButtonState.Unselected;
        }

        //The action triggered by the button press
        private void PerformAction()
        {
            ObjectSelection.Instance.SetTool(toolType);
        }

        //Set the current tool and select the appropriate button from an external script
        public static void SetTool(Tool newTool)
        {
            //Deselect the currently active button and select the button for the new tool
            _selectedButton.Unselect();
            ToolTable[newTool].Select();
        }

        #endregion

        #region Pointer Methods

        //If this button was last pressed and the mouse moves over it, change to the pressed image
        public void OnPointerEnter(PointerEventData data)
        {
            if (_pressedButton == this && _mouseDown && currentState == ButtonState.Unselected)
                OnPointerDown(data);
        }

        //If the button was pressed and the cursor moves off of the button, change to the unselected image
        public void OnPointerExit(PointerEventData data)
        {
            if (currentState == ButtonState.Pressed)
                Unselect();
        }

        //If the mouse is pressed down on the button and its not selected, change to the pressed image
        public void OnPointerDown(PointerEventData data)
        {
            _mouseDown = true;

            if (currentState == ButtonState.Unselected)
            {
                imageScript.sprite = pressed;
                currentState = ButtonState.Pressed;
                _pressedButton = this;
            }
        }

        //If this button is clicked, select it and unselect all other buttons
        public void OnPointerUp(PointerEventData data)
        {
            _mouseDown = false;

            if (currentState == ButtonState.Pressed)
            {
                _selectedButton.Unselect();
                Select();
            }
        }

        #endregion
    }
}