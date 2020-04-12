using UnityEngine;
using System.Collections;

public class SettingsGUI
{
    private static string autofocus = "Autofocus";
    private static string strVisibleOnStart = "Visible on start";
    private static float layoutWidth = 320f;
    private static float layoutHeight = 430f;
    private static string layoutWidthPlayerPrefs = "emcli_lwidth";
    private static string layoutHeightPlayerPrefs = "emcli_lheight";
    private static string autofocusPlayerPrefs = "emcli_bool_autofocus";
    private static string visibleFromStartPlayerPrefs = "emcli_bool_visibleFromStart";
    public static void OnGUI()
    {
        GUI.backgroundColor = Color.green;
        EMCli.FocusOnEnable = GUILayout.Toggle(EMCli.FocusOnEnable, autofocus);
        EMCli.VisibleOnStart = GUILayout.Toggle(EMCli.VisibleOnStart, strVisibleOnStart);

        GUILayout.BeginHorizontal();
        layoutWidth = GUILayout.HorizontalScrollbar(layoutWidth, 20, 200, 1920);
        GUILayout.TextArea(layoutWidth.ToString(), GUILayout.MaxWidth(90));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        layoutHeight = GUILayout.HorizontalScrollbar(layoutHeight, 20, 200, 1920);
        GUILayout.TextArea(layoutHeight.ToString(), GUILayout.MaxWidth(90));
        GUILayout.EndHorizontal();

        ApplyChangesIfModified();

        if(GUILayout.Button("Reset size"))
        {
            layoutWidth = 320;
            layoutHeight = 430;
        }
        else if(GUILayout.Button("Save preferences"))
        {
            SavePrefs();
        }
        else if(GUILayout.Button("Load preferences"))
        {
            LoadPrefs();
        }
        GUI.DragWindow();
    }

    public static void SavePrefs()
    {
        PlayerPrefs.SetFloat(layoutWidthPlayerPrefs, layoutWidth);
        PlayerPrefs.SetFloat(layoutHeightPlayerPrefs, layoutHeight);
        PlayerPrefs.SetInt(autofocusPlayerPrefs, EMCli.FocusOnEnable.ToInt32());
        PlayerPrefs.SetInt(visibleFromStartPlayerPrefs, EMCli.VisibleOnStart.ToInt32());
    }

    public static void LoadPrefs()
    {
        if(PlayerPrefs.HasKey(layoutWidthPlayerPrefs)) layoutWidth = PlayerPrefs.GetFloat(layoutWidthPlayerPrefs);
        if(PlayerPrefs.HasKey(layoutHeightPlayerPrefs)) layoutHeight = PlayerPrefs.GetFloat(layoutHeightPlayerPrefs);
        if(PlayerPrefs.HasKey(autofocusPlayerPrefs)) EMCli.FocusOnEnable = PlayerPrefs.GetInt(autofocusPlayerPrefs).ToBool();
        if (PlayerPrefs.HasKey(visibleFromStartPlayerPrefs)) EMCli.VisibleOnStart = PlayerPrefs.GetInt(visibleFromStartPlayerPrefs).ToBool();
    }

    public static void ApplyChangesIfModified()
    {
        if (layoutWidth != EMCli.LayoutWidth || layoutHeight != EMCli.LayoutHeight)
        {
            EMCli.SetupGUI(layoutWidth, layoutHeight);
        }
    }
}
