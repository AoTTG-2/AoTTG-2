using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArmatureData))]
public class ArmatureDataEditor : Editor
{
    private ArmatureData script;
    private bool showTransforms;

    private void OnEnable()
    {
        script = (ArmatureData)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Set Bone References"))
            script.SetBoneReferences();

        script.armatureObject = (GameObject)EditorGUILayout.ObjectField("Armature", script.armatureObject, typeof(GameObject), true);

        showTransforms = EditorGUILayout.Foldout(showTransforms, "References");

        EditorGUI.indentLevel++;

        if (showTransforms)
            DrawDefaultInspector();
    }
}