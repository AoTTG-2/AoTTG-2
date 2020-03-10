using System;
using System.IO;
using System.Net;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AoTExtensions
{
    public static class AoTExtensions
    {
        private static Regex hexCode = new Regex(@"\[([0-9a-f]{6})\]", RegexOptions.IgnoreCase);

        /// <summary>
        /// Converts hexcode to Color
        /// </summary>
        /// <param name="hex">Hexcode</param>
        /// <param name="a">Transparency</param>
        /// <returns></returns>
        public static Color HexToColor(this string hex, byte a = 255)
        {
            if (hex.Length != 6) return Color.white;
            byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            return new Color32(r, g, b, a);
        }

        public static int toInt(this object l)
        {
            return Convert.ToInt32(l);
        }

        public static PhotonPlayer toPlayer(this object obj)
        {
            return PhotonPlayer.Find(obj.toInt());
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("http://google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        //used for skins
        public static bool IsImage(this string path)
        {
            return path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".jpeg");
        }
        public static int CountWords(this string s, string s1)
        {
            return (s.Length - s.Replace(s1, "").Length) / s1.Length;
        }

        /// <summary>
        /// Limit a string to a specified length. 
        /// </summary>
        public static string LimitToLength(this string value, int length)
        {
            return value.Length <= length ? value : value.Substring(0, length) + "...";
        }

        /// <summary>
        /// Limit a string a specified length (it doesn't count length of HTML and hex colors in the string). 
        /// </summary>
        public static string LimitToLengthStripped(this string value, int length)
        {
            return value.RemoveHex().RemoveHTML().Length <= length ? value : (value.Substring(0, length) + "...");
        }

        /// <summary>
        /// Remove hex colors from a string. 
        /// Example: [000000] would be removed
        /// </summary>
        public static string RemoveHex(this string str)
        {
            return hexCode.Replace(str, string.Empty).Replace("[-]", string.Empty);
        }

        /// <summary>
        /// Remove HTML tags like bold, italic, size and color from a string. 
        /// </summary>
        public static string RemoveHTML(this string str)
        {
            return Regex.Replace(str, @"((<(\/|)(color(?(?=\=).*?)>|b>|size.*?>|i>)))", "", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Remove hex colors and HTML tags like bold, italic, size and color from a string. 
        /// </summary>
        public static string RemoveAll(this string x)
        {
            return Regex.Replace(x, @"((\[([0-9a-f]{6})\])|(\[\-\])|(<(\/|)(color(?(?=\=).*?)>))|(<size=(\w*)?>?|<\/size>?)|(<\/?[bi]>))", "");
        }

        /// <summary>
        /// Remove size tags from a string. 
        /// </summary>
        public static string RemoveSize(this string str)
        {
            return Regex.Replace(str, "<size=(\w*)?>?|<\\/size>?", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Convert hex color to an HTML code. 
        /// Example: [000000] would be changed to <color=#000000></color>
        /// </summary>
        public static string ToHTMLFormat(this string str)
        {
            if (hexCode.IsMatch(str))
            {
                str = str.Contains("[-]") ? hexCode.Replace(str, "<color=#$1>").Replace("[-]", "</color>") : hexCode.Replace(str, "<color=#$1>");
                var c = (short)(str.CountWords("<color=") - str.CountWords("</color>"));
                for (short i = 0; i < c; i++)
                {
                    str += "</color>";
                }
            }
            return str;
        }
    }
}
