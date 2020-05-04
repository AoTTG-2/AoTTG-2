using UnityEngine;
using System.Collections;

public class CommandsGUI
{
    private static Vector2 scroll = Vector2.zero;
    public static void OnGUI()
    {
        foreach(Command command in CommandHandler.Instance.Commands)
        {
            scroll = GUILayout.BeginScrollView(scroll);
            bool copied = InputLine.inputLine.Equals(command.Name);
            if (GUILayout.Button(command.Name))
            {
                if (!copied) InputLine.inputLine = command.Name;
                else InputLine.inputLine = "/help " + command.Name;
            }
            GUILayout.EndScrollView();
        }
        GUILayout.BeginHorizontal();
        EMCli.InputGUI();
        GUILayout.EndHorizontal();
        GUI.DragWindow();
    }
}
