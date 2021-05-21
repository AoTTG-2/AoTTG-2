using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Room.Chat;
using UnityEngine.Localization.Settings;
using System.Collections;
using Assets.Scripts.UI.Input;
using Assets.Scripts.Events;
public class ChatMessage : Photon.MonoBehaviour
{

    [SerializeField] private TMP_Text thisMessage;
    private OnTranslateSettingsChanged translateSettingsChanged;
    public GameObject controlsPage;
    private string originalMessage;
    private string translatedMessage;
    private bool hasBeenTranslated = false;
    private bool originalLang = true;
    private bool sameLang = true;
    [SerializeField] private Button translateButton;
    [SerializeField] private string playerID;


    private void Start()
    {

        translateSettingsChanged += OnSettingsChanged;
        translateButton.gameObject.SetActive(false);
        originalMessage = thisMessage.text;
        string[] playersplit = thisMessage.text.Split(':');
        playerID = playersplit[0];

        if (InputManager.Settings.AutoTranslate)
        {

            StartCoroutine(DelayTranslate());

        }

    }

    private void OnSettingsChanged(bool autoTranslate)
    {
        Debug.Log($"Settings changed set to {InputManager.Settings.AutoTranslate}");
    }

    private IEnumerator DelayTranslate()
    {

        yield return null;
        TranslateThisText();

    }

    public void TranslateThisText()
    {

        if (!hasBeenTranslated)
        {
            var line = TMP_TextUtilities.FindIntersectingLink(thisMessage, thisMessage.transform.position, null);

            if (line > -1)
            {

                var text = thisMessage.textInfo.linkInfo[line];

                string langCode = LocalizationSettings.SelectedLocale.Identifier.CultureInfo.TwoLetterISOLanguageName;

                StartCoroutine(TextTranslator.Translate(text.GetLinkText(), "auto", langCode, results =>
                {

                    if (results.Length > 1)
                    {

                        if (langCode == results[0])
                        {

                            translateButton.gameObject.SetActive(false);
                            hasBeenTranslated = true;
                            sameLang = true;
                            thisMessage.text = originalMessage;
                            return;

                        }

                        translatedMessage = playerID + ":  " + results[1];
                        hasBeenTranslated = true;
                        originalLang = true;
                        sameLang = false;
                        translateButton.onClick.AddListener(TranslateThisText);
                        translateButton.gameObject.SetActive(true);
                        thisMessage.text = originalMessage;
                        return;

                    }
                    else
                    {

                        Debug.LogError($"Translation Error: {results[0]}");
                        return;

                    }

                }));

            }
        }

        if (!sameLang)
            UpdateChatBox();

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
