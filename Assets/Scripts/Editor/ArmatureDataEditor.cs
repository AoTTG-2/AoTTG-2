using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArmatureData)), CanEditMultipleObjects]
public class ArmatureDataEditor : Editor
{
    private ArmatureData script;
    private SerializedProperty armatureObject;
    private bool showTransforms;

    private void OnEnable()
    {
        armatureObject = serializedObject.FindProperty("armatureObject");
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Set Bone References"))
            script.SetBoneReferences();

        //Display the armature object property
        EditorGUILayout.PropertyField(armatureObject);

        //Display the references foldout
        showTransforms = EditorGUILayout.Foldout(showTransforms, "References");

        //If the foldout is active, display all of the references
        if (showTransforms)
            DisplayReferences();

        serializedObject.ApplyModifiedProperties();
    }

    //Display all of the class properties except for the script itself and the armature object
    public void DisplayReferences()
    {
        EditorGUI.indentLevel++;

        SerializedProperty propIterator = serializedObject.GetIterator();
        bool iterateChildren = true;

        while (propIterator.NextVisible(iterateChildren))
        {
            if (propIterator.name != "m_Script" && propIterator.name != "armatureObject")
                EditorGUILayout.PropertyField(propIterator);

            iterateChildren = false;
        }

        EditorGUI.indentLevel--;
    }
}