using UnityEngine.UI;
using Assets.Scripts.Room.Chat;


public class ContextMenu : Photon.MonoBehaviour
{

    public Button Copy;
    public Button Translate;
    public string clickedOver;
    private TextTranslator translator = new TextTranslator();


    void Start()
    {

        Copy.onClick.AddListener(CopyMessage);
        Translate.onClick.AddListener(TranslateMessage);

    }

    private void TranslateMessage()
    {
        //Translate stuff and turn the context menu off

        //TODO figure out how to import the SystemLanguageConverter.GetSystemLanguageCultureCode() function from UnityEngine.Localization package

        //StartCoroutine(translator.Translate(clickedOver, "auto", SystemLanguageConverter.GetSystemLanguageCultureCode(Application.systemLanguage));
    }

    void CopyMessage()
    {

        //Copy to clipboard and turn the menu off
        ClipboardExtension.CopyToClipboard(clickedOver);

    }

}
