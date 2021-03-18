using UnityEngine;
using TMPro;

namespace MapEditor
{
    public class ImportExportManager : MonoBehaviour
    {
        //A reference to the canvas object
        [SerializeField] private GameObject canvas;
        //The canvas group component attached to the canvas
        private CanvasGroup canvasGroup;

        //The UI element that prevents interaction with anything except for the pop-up
        [SerializeField] private GameObject raycastBlocker;

        //The Import pop-up
        [SerializeField] private GameObject importPopup;
        //Displays the imported text
        [SerializeField] private GameObject importTextArea;
        //The text area component attached to the import text area object
        private TextArea importTextAreaComponent;

        //Displays the importing message
        [SerializeField] private GameObject importingMessage;
        //Displays how many map objects have been imported
        [SerializeField] private GameObject progressText;
        //The text component attached to the progress text object
        private TextMeshProUGUI progressTextComponent;
        //Displays how many map objects are in the script
        [SerializeField] private GameObject totalText;
        //The text component attached to the total text object
        private TextMeshProUGUI totalTextComponent;

        //The export pop-up
        [SerializeField] private GameObject exportPopup;
        //Displays the exported text
        [SerializeField] private GameObject exportTextArea;
        //The text area component attached to the export text area object
        private TextArea exportTextAreaComponent;

        private void Start()
        {
            //Find and store the needed component references
            canvasGroup = canvas.GetComponent<CanvasGroup>();
            importTextAreaComponent = importTextArea.GetComponent<TextArea>();
            exportTextAreaComponent = exportTextArea.GetComponent<TextArea>();
            progressTextComponent = progressText.GetComponent<TextMeshProUGUI>();
            totalTextComponent = totalText.GetComponent<TextMeshProUGUI>();
        }

        //Hide or show the import pop-up screen
        public void ToggleImportPopup()
        {
            importPopup.SetActive(!importPopup.activeSelf);

            //When the pop-up is enabled, make sure the text area is not in focus
            if (importPopup.activeSelf)
                importTextAreaComponent.SetFocused(false);

            ToggleMode();
        }

        //Hide or show the export pop-up screen
        public void ToggleExportPopup()
        {
            exportPopup.SetActive(!exportPopup.activeSelf);

            //If the export pop-up was enabled, export the map script and set it as the text area content
            if (exportPopup.activeSelf)
            {
                exportTextAreaComponent.SetFocused(false);
                exportTextAreaComponent.Text = MapManager.Instance.ToString();
            }

            ToggleMode();
        }

        //Set the editor mode based on if the pop-ups are enabled or not
        private void ToggleMode()
        {
            if (importPopup.activeSelf || exportPopup.activeSelf)
            {
                EditorManager.Instance.CurrentMode = EditorMode.UI;
                raycastBlocker.SetActive(true);
            }
            else
            {
                EditorManager.Instance.CurrentMode = EditorMode.Edit;
                raycastBlocker.SetActive(false);
            }
        }

        //Import the map text in the input field
        public void ImportFromTextField()
        {
            //Disable the UI, shortcuts, and drag select
            canvasGroup.blocksRaycasts = false;
            EditorManager.Instance.ShortcutsEnabled = false;
            DragSelect.Instance.enabled = false;
            //Show the importing text
            importingMessage.SetActive(true);

            //Clear the old map before loading the new one
            MapManager.Instance.clearMap();
            //Import the map script in the text field
            StartCoroutine(MapManager.Instance.LoadMap(importTextAreaComponent.Text, progressTextComponent, totalTextComponent, EndImport));
            
            //Clear the import text area
            importTextAreaComponent.ClearText();
            //Hide the import pop-up
            ToggleImportPopup();
        }

        //Prepares the editor for use after importing
        private void EndImport()
        {
            //Enable the UI, shortcuts, and drag select
            canvasGroup.blocksRaycasts = true;
            EditorManager.Instance.ShortcutsEnabled = true;
            DragSelect.Instance.enabled = true;
            //Clear the command history
            EditHistory.Instance.ResetHistory();
            //Hide the importing text
            importingMessage.SetActive(false);
        }
    }
}