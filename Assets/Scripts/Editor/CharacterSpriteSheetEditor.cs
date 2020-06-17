using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterSpriteSheet))]
public class CharacterSpriteSheetEditor : Editor
{
    private SerializedProperty spriteMaterial;
    private SerializedProperty sheetColumns;
    private SerializedProperty sheetRows;
    private SerializedProperty startColumn;
    private SerializedProperty startRow;
    private SerializedProperty endColumn;
    private SerializedProperty endRow;

    private void OnEnable()
    {
        spriteMaterial = serializedObject.FindProperty("spriteMaterial");
        sheetColumns = serializedObject.FindProperty("sheetColumns");
        sheetRows = serializedObject.FindProperty("sheetRows");
        startColumn = serializedObject.FindProperty("startColumn");
        startRow = serializedObject.FindProperty("startRow");
        endColumn = serializedObject.FindProperty("endColumn");
        endRow = serializedObject.FindProperty("endRow");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(spriteMaterial);

        DisplayPair(sheetColumns, sheetRows);
        EditorGUILayout.LabelField("Cells start at (0,0) and are read top-down, left to right", EditorStyles.boldLabel);
        DisplayPair(startColumn, startRow);
        DisplayPair(endColumn, endRow);
    }

    private void DisplayPair(SerializedProperty property1, SerializedProperty property2)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(property1);
        EditorGUILayout.PropertyField(property2);
        EditorGUILayout.EndHorizontal();
    }
}