using UnityEngine;

namespace Assets.Scripts.Utility
{
    /// <summary>
    /// Extension methods which are used for Chat formatting.
    /// </summary>
    public static class StringExtensionMethods
    {
        public static string Color(this string text, float r, float g, float b, float a = 1)
        {
            return text.Color(new Color(r, g, b, a));
        }
        public static string Color(this string text, Color color, bool useAlpha = false)
        {
            return text.Color(useAlpha ? ColorUtility.ToHtmlStringRGBA(color) : ColorUtility.ToHtmlStringRGB(color));
        }
        public static string Color(this string text, string hexCode)
        {
            return $"<color=#{hexCode}>{text}</color>";
        }
        public static string Bold(this string text)
        {
            return $"<b>{text}</b>";
        }
        public static string Italics(this string text)
        {
            return $"<i>{text}</i>";
        }
        public static string Size(this string text, int size)
        {
            return $"<size={size}>{text}</size>";
        }

    }
}
