using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using UnityEngine.Networking;

namespace Assets.Scripts.Room.Chat
{
    /// <summary>
    /// A text translation utility class that uses the Google Translate API.
    /// </summary>
    public static class TextTranslator
    {
        /// <summary>
        /// A Unity coroutine that takes in a string, an ISO 639-1 code to translate from (or "auto"), an ISO 639-1 code to translate to, and a callback action.
        /// </summary>
        /// 
        /// <example>
        /// Example usage of <see cref="Translate(string, string, string, Action{string[]})"/>
        /// <code>
        /// StartCoroutine(TextTranslator.GetTranslatedText("Guten tag!", "auto", "en", results => {
        ///     if (results.Length > 1) // We have both a language code AND the translated text!
        ///     {
        ///         Console.WriteLine($"*From {results[0]}*: {results[1]");
        ///     } else // Unity encountered an error somewhere
        ///     {
        ///         Console.WriteLine($"*Translation Error: {results[0]");
        ///     }
        /// }));
        /// </code>
        /// </example>
        /// 
        /// <param name="text">The original text to translate</param>
        /// <param name="langCodeFrom">The original language to translate from (use "auto" to have Google auto-detect).</param>
        /// <param name="langCodeTo">The language to translate the text to.</param>
        /// <param name="callback">Gets invoked with a string[] array that returns either the language code and translated text, or the error Unity encountered.</param>
        /// 
        /// <returns>Nothing, the results are invoked by the 'callback' parameter.</returns>
        public static IEnumerator Translate(string text, string langCodeFrom, string langCodeTo, Action<string[]> callback)
        {
            string query = UnityWebRequest.EscapeURL(text);
            string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={langCodeFrom}&tl={langCodeTo}&dt=t&q={query}";

            using (UnityWebRequest uwr = UnityWebRequest.Get(url))
            {
                yield return uwr.SendWebRequest();

                if (uwr.responseCode == 200)
                {
                    JArray array = JArray.Parse(uwr.downloadHandler.text);

                    callback.Invoke(new string[]
                    {
                        array[2].ToString(), // The language code the text was translated from.
                        array[0][0][0].ToString() // The actual translated text, supplied by Google.
                    });
                }
                else if (uwr.error != null) // Uh oh!
                {
                    callback.Invoke(new string[] {
                        uwr.error
                    });
                }
            }
        }
    }
}