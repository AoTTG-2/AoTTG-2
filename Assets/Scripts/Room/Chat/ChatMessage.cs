using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Room.Chat;
using UnityEngine.Localization.Settings;
using Assets.Scripts.UI.InGame.Scoreboard;

public class ChatMessage : Photon.MonoBehaviour
{

    [SerializeField] private TMP_Text thisMessage;
    private string originalMessage;
    private string translatedMessage;
    private bool hasBeenTranslated = false;
    private bool originalLang = true;
    [SerializeField] private Button translateButton;
    [SerializeField] private string playerID;
    

    private void Start()
    {

        translateButton.onClick.AddListener(TranslateThisText);
        originalMessage = thisMessage.text;
        string[] playersplit = thisMessage.text.Split(':');
        playerID = playersplit[0];

    }

    public void TranslateThisText()
    {

        if (Cursor.visible)
        {

            if (!hasBeenTranslated)
            {

                var line = TMP_TextUtilities.FindIntersectingLink(thisMessage, thisMessage.transform.position, null);

                if (line > -1)
                {

                    var text = thisMessage.textInfo.linkInfo[line];

                    string langCode = LocalizationSettings.SelectedLocale.Identifier.CultureInfo.TwoLetterISOLanguageName;

                    Debug.Log("Your language is: " + langCode);

                    StartCoroutine(TextTranslator.Translate(text.GetLinkText(), "auto", langCode, results =>
                    {

                        if (results.Length > 1)
                        {

                            translatedMessage = playerID + ":  " + results[1];
                            hasBeenTranslated = true;
                            originalLang = true;
                            UpdateChatBox();
                            return;

                        }
                        else
                        {

                            Debug.LogError($"Translation Error: {results[0]}");

                        }

                    }));

                }
            }

            UpdateChatBox();
        }

    }

    private void UpdateChatBox()
    {

        if (originalLang)
        {

            thisMessage.text = translatedMessage;
            originalLang = false;

        }

        else
        {

            thisMessage.text = originalMessage;
            originalLang = true;

        }

    }
}
