using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Interactable))]
public sealed class InteractableEditor : Editor
{
    private static readonly GUIContent
        AddButton = new GUIContent("Add Components"),
        RemoveButton = new GUIContent("Remove Components");

    private new Interactable target;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button(AddButton))
            target.AddComponents();

        if (GUILayout.Button(RemoveButton))
            target.RemoveComponents();
    }

    private void OnEnable()
    {
        target = (Interactable) base.target;
    }
}