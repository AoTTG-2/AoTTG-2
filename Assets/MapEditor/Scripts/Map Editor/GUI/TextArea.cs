using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MapEditor
{
    //Manages a custom text area
    public class TextArea : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler
    {
        //The two images used for the text area
        [SerializeField] private UnityEngine.Sprite unfocusedSprite;

        [SerializeField] private UnityEngine.Sprite focusedSprite;

        //The child game object that contains the text component
        [SerializeField] private GameObject textObject;

        //Toggles the ability to delete the text area contents using key inputs
        [SerializeField] private bool canDelete = false;

        //Toggles the ability to paste new content into the text area
        [SerializeField] private bool canPaste = false;

        //The image component attached to this game object
        private Image imageComponent;

        //The text component attached to the child object
        private Text textComponent;

        //By default, the text area is out of focus
        private bool isFocused = false;

        //Boolean values to store the pointer data
        private bool overTextArea = false;

        //Determines if the Start function has been called or not
        private bool initialized = false;

        //A property for accessing the contents of the text component
        public string Text
        {
            get => textComponent.text;

            set
            {
                //If this function was called before initialization, call the start function
                if (!initialized)
                    Start();

                textComponent.text = value;
            }
        }

        private void Start()
        {
            //Find and store the needed component references
            imageComponent = gameObject.GetComponent<Image>();
            textComponent = textObject.GetComponent<Text>();
        }

        private void Update()
        {
            //Check if the text area should be focused or not
            if (Input.GetMouseButtonUp(0))
            {
                if (overTextArea)
                    SetFocused(true);
                else
                    SetFocused(false);
            }
            else if (Input.GetKey(KeyCode.Escape))
                SetFocused(false);

            //If ctrl + p or ctrl + c where pressed, copy or paste the text
            if (isFocused && Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.C))
                    CopyFromTextArea();
                else if (canPaste && Input.GetKeyDown(KeyCode.V))
                    PasteToTextArea();
            }

            //If the delete or backspace key is pressed, clear the text
            if (canDelete && isFocused &&
                (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete)))
            {
                ClearText();
            }
        }

        //Set the sprite image for the game object
        public void SetFocused(bool focused)
        {
            if (isFocused == focused)
                return;

            isFocused = focused;

            if (focused)
                imageComponent.sprite = focusedSprite;
            else
                imageComponent.sprite = unfocusedSprite;
        }

        //Check if the pointer is over the text area or not
        public void OnPointerEnter(PointerEventData data)
        {
            overTextArea = true;
        }

        public void OnPointerExit(PointerEventData data)
        {
            overTextArea = false;
        }

        //Copy the contents of the text area to clipboard
        public void CopyFromTextArea()
        {
            TextEditor te = new TextEditor();
            te.text = textComponent.text;
            te.SelectAll();
            te.Copy();
        }

        //Paste the contents of the clipboard to the text area
        public void PasteToTextArea()
        {
            TextEditor te = new TextEditor();
            te.Paste();
            textComponent.text = te.text;
        }

        public void ClearText()
        {
            textComponent.text = "";
        }
    }
}