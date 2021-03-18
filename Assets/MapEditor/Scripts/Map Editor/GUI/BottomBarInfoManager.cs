using UnityEngine;
using TMPro;

namespace MapEditor
{
    public class BottomBarInfoManager : MonoBehaviour
    {
        //The categories of controls each editor mode
        [SerializeField] private GameObject flyEditMode;
        [SerializeField] private GameObject flyMode;
        [SerializeField] private GameObject editMode;

        //The three groups of controls for edit mode
        [SerializeField] private GameObject generalEditorControls;
        [SerializeField] private GameObject commandList;
        [SerializeField] private GameObject dragSelectControls;

        //The game object and text component displaying the current mode
        [SerializeField] private GameObject currentMode;
        private TextMeshProUGUI currentModeText;
        //The game object and text component displaying the object count
        [SerializeField] private GameObject objectCount;
        private TextMeshProUGUI objectCountText;
        //The game object and text component displaying the action of the "ctrl+A" command
        [SerializeField] private GameObject selectAll;
        private TextMeshProUGUI selectAllText;

        private void Start()
        {
            //Find and store the needed component references
            currentModeText = currentMode.GetComponent<TextMeshProUGUI>();
            objectCountText = objectCount.GetComponent<TextMeshProUGUI>();
            selectAllText = selectAll.GetComponent<TextMeshProUGUI>();

            //Listen for when the editor mode is changed
            EditorManager.Instance.OnChangeMode += OnModeChange;
        }

        private void Update()
        {
            //Update the object count
            objectCountText.text = "Objects: " + ObjectSelection.Instance.GetSelectionCount() + "/" + ObjectSelection.Instance.GetSelectableCount();

            //If the editor is in edit mode, find which controls should be displayed
            if (EditorManager.Instance.CurrentMode == EditorMode.Edit)
            {
                //If a drag selection is in progress, show the drag select modifiers
                if (DragSelect.Instance.GetDragging())
                {
                    generalEditorControls.SetActive(false);
                    commandList.SetActive(false);
                    dragSelectControls.SetActive(true);
                    flyEditMode.SetActive(false);
                }
                //If control is held and nothing is being dragged, show the commands list
                else if (Input.GetKey(KeyCode.LeftControl))
                {
                    generalEditorControls.SetActive(false);
                    commandList.SetActive(true);
                    dragSelectControls.SetActive(false);
                    flyEditMode.SetActive(false);

                    //Set the text of the "ctrl+A" command based on if any objects are selected
                    if (ObjectSelection.Instance.GetSelectionCount() == 0)
                        selectAllText.text = "Select All";
                    else
                        selectAllText.text = "Deselect All";
                }
                //Otherwise show the general editor keys
                else
                {
                    generalEditorControls.SetActive(true);
                    commandList.SetActive(false);
                    dragSelectControls.SetActive(false);
                    flyEditMode.SetActive(true);
                }
            }
        }

        //When the editor mode is changed, show the corresponding control group
        private void OnModeChange(EditorMode prevMode, EditorMode newMode)
        {
            //Toggle the control groups based on the current mode
            flyEditMode.SetActive(newMode != EditorMode.UI);
            flyMode.SetActive(newMode == EditorMode.Fly);
            editMode.SetActive(newMode == EditorMode.Edit);

            //Set the mode text to display the current mode
            currentModeText.text = newMode.ToString();
        }
    }
}