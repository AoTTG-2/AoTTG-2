using UnityEngine;
using System.Collections;

public class ConsoleOptionsGUI
{
    private static Color backgroundColor = Color.black;
    public static GUI.WindowFunction WindowFunctionConsoleOptionsGUI = new GUI.WindowFunction(WindowLayoutConsoleOptionsGUI);
    private static string backColorCode = "[ffffff]";
    private static string back = "<".RepaintCustom(backColorCode, true);
    public static bool IsOpened = false;
    static int toolbarInt = 0;
    static string[] toolbarStrings = { "Commands".RepaintCustom(backColorCode, true), "DebugLevels".RepaintCustom(backColorCode, true), "Settings".RepaintCustom(backColorCode, true) };
    static Vector2 scrollView = Vector2.zero;

    public static void WindowLayoutConsoleOptionsGUI(int windowID)
    {
        GUI.backgroundColor = backgroundColor;
        if (GUILayout.Button(back, GUILayout.MaxWidth(25)))
        {
            Switch();
        }
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);

        scrollView = GUILayout.BeginScrollView(scrollView);
        switch (toolbarInt)
        {
            case 0:
                CommandsGUI.OnGUI();
                break;
            case 1:
                DebugLevelsGUI.OnGUI();
                break;
            case 2:
                SettingsGUI.OnGUI();
                break;
        }
        GUILayout.EndScrollView();
        GUI.DragWindow();
    }
    
    public static void Switch()
    {
        if (IsOpened = !IsOpened)
        {
            EMCli.WindowFunctionMainGUI = WindowFunctionConsoleOptionsGUI;
        }
        else EMCli.SwitchBack();
    }
}
