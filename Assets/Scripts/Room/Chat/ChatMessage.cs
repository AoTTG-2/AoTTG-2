using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using System.Collections;
using Assets.Scripts.UI.Input;

namespace Assets.Scripts.Room.Chat
{
    public class ChatMessage : MonoBehaviour
    {

        public TMP_Text thisMessage;
        private string originalMessage;
        private string translatedMessage;
        private bool originalLang = true;
        public Button translateButton;
        private string playerID;


        private void Start()
        {

            translateButton.gameObject.SetActive(false);
            originalMessage = thisMessage.text;
            string[] playersplit = thisMessage.text.Split(':');
            playerID = playersplit[0];

            if (InputManager.Settings.Translate)
                StartCoroutine(DelayTranslate());

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
                        {

                            translateButton.gameObject.SetActive(false);
                            return;

                        }

                        translatedMessage = playerID + ":  " + results[1];
                        originalLang = true;
                        translateButton.onClick.AddListener(ValidateCursor);
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
            {

                thisMessage.text = translatedMessage;

            }

            else
            {

                thisMessage.text = originalMessage;

            }

            originalLang = !originalLang;

        }
    }
}
