using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Interactable))]
public sealed class InteractableEditor : Editor
{
    private static readonly GUIContent
        AddButton = new GUIContent("Create Collider"),
        RemoveButton = new GUIContent("Destroy Collider");

    private new Interactable target;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button(AddButton))
            target.TryCreateCollider();

        if (GUILayout.Button(RemoveButton))
            target.TryDestroyCollider();
    }

    private void OnEnable()
    {
        target = (Interactable) base.target;
    }
}