using TMPro;
using UnityEngine;

public class ChatMessage : Photon.MonoBehaviour
{

    [SerializeField] private TMP_Text thisMessage;
    [SerializeField] private GameObject contextMenu;

    //Detect right click
    //Open a context menu with options (buttons?) like Copy and Translate

    void Update()
    {

        if (Cursor.visible)
        {

            if (Input.GetMouseButtonDown(1))
            {

                int line = TMP_TextUtilities.FindIntersectingLink(thisMessage, Input.mousePosition, null);

                if (line > -1)
                {

                    var text = thisMessage.textInfo.linkInfo[line];
                    //Create the ContextMenu and set ContextMenu.clickedOver to text.GetLinkText()

                }

            }

        }

        return;

    }

}
