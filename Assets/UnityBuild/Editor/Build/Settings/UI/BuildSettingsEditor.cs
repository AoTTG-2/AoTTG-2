using UnityEditor;
using UnityEngine;

namespace SuperSystems.UnityBuild
{

[CustomEditor(typeof(BuildSettings))]
public class BuildSettingsEditor : Editor
{
    private SerializedProperty versionManagerProperty;

    private void OnEnable()
    {
        versionManagerProperty = serializedObject.FindProperty("_versionManager");   
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(versionManagerProperty);
        serializedObject.ApplyModifiedProperties();

        Color defaultBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;

        if (GUILayout.Button("Open SuperUnityBuild", GUILayout.ExpandWidth(true), GUILayout.MinHeight(30)))
        {
            UnityBuildWindow.ShowWindow();
        }

        GUI.backgroundColor = defaultBackgroundColor;
    }
}

}