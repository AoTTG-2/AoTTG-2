using UnityEngine.UI;
using Assets.Scripts.Room.Chat;

public class ContextMenu : Photon.MonoBehaviour
{

    public Button Copy;
    public Button Translate;
    public string clickedOver;
    private TextTranslator translator = new TextTranslator();


    void Update()
    {

        if (gameObject.activeSelf)
        {

            Copy.onClick.AddListener(CopyMessage);
            Translate.onClick.AddListener(TranslateMessage);

        }

    }

    private void TranslateMessage()
    {
        //Translate stuff and turn the context menu off
    }

    void CopyMessage()
    {

        //Copy to clipboard and turn the menu off
        ClipboardExtension.CopyToClipboard(clickedOver);

    }

}
