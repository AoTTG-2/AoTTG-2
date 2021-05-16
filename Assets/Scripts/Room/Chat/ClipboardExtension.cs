using UnityEngine;

namespace Assets.Scripts.Room.Chat
{

    public static class ClipboardExtension
    {

        public static void CopyToClipboard(this string str)
        {

            GUIUtility.systemCopyBuffer = str;

        }
    
    }

}
