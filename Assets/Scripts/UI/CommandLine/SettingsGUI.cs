using UnityEngine;
using System.Collections;

public class SettingsGUI
{
    private static string autofocus = "Autofocus";
    private static float layoutWidth = 320f;
    private static float layoutHeight = 430f;
    public static void OnGUI()
    {
        GUI.backgroundColor = Color.green;
        EMCli.FocusOnEnable = GUILayout.Toggle(EMCli.FocusOnEnable, autofocus);

        GUILayout.BeginHorizontal();
        layoutWidth = GUILayout.HorizontalScrollbar(layoutWidth, 20, 200, 1920);
        GUILayout.TextArea(layoutWidth.ToString(), GUILayout.MaxWidth(90));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        layoutHeight = GUILayout.HorizontalScrollbar(layoutHeight, 20, 200, 1920);
        GUILayout.TextArea(layoutHeight.ToString(), GUILayout.MaxWidth(90));
        GUILayout.EndHorizontal();
        
        if (layoutHeight != EMCli.LayoutWidth || layoutHeight != EMCli.LayoutHeight) EMCli.SetupGUI(layoutWidth, layoutHeight);

        if(GUILayout.Button("Reset size"))
        {
            layoutWidth = 320;
            layoutHeight = 430;
        }
    }
}
