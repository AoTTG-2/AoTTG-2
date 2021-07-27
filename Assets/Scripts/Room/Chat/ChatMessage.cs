using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using System.Collections;
using Assets.Scripts.UI.Input;

namespace Assets.Scripts.Room.Chat
{
    /// <summary>
    /// Represents an indivual message in the chat.
    /// On Start it initializes the message to determine if translation is needed or necessary,
    /// it then caches the translated text and enables the message's translate button translate button.
    /// </summary>
    public class ChatMessage : MonoBehaviour
    {
        public TMP_Text thisMessage;
        public Button translateButton;
        private string originalMessage;
        private string translatedMessage;
        private bool originalLang = true;
        private string playerID;

        private void Start()
        {
            translateButton.gameObject.SetActive(false);

            if (InputManager.Settings.Translate)
            {
                originalMessage = thisMessage.text;
                string[] playersplit = thisMessage.text.Split(':');
                playerID = playersplit[0];
                StartCoroutine(DelayTranslate());
                translateButton.onClick.AddListener(ValidateCursor);
            }
        }

        private IEnumerator DelayTranslate()
        {
            yield return null;
            TranslateThisText();
        }

        public void TranslateThisText()
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
                            return;

                        translatedMessage = playerID + ":  " + results[1];
                        originalLang = true;
                        translateButton.gameObject.SetActive(true);
                        if (InputManager.Settings.AutoTranslate)
                            UpdateChatBox();

                    }
                    else
                    {
                        Debug.LogError($"Translation Error: {results[0]}");
                    }

                }));

            }

        }

        private void ValidateCursor()
        {
            if (Cursor.visible && InputManager.Settings.Translate)
            {

                UpdateChatBox();

            }
        }

        private void UpdateChatBox()
        {

            if (originalLang)
                thisMessage.text = translatedMessage;


            else
                thisMessage.text = originalMessage;

            originalLang = !originalLang;

        }
    }
}
