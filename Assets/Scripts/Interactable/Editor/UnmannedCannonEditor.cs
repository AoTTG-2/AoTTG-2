using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnmannedCannon))]
public sealed class UnmannedCannonEditor : Editor
{
    private const string
        MountInteractableName = "MountInteractable",
        MoveInteractableName = "MoveInteractable";

    private static readonly GUIContent
        GroundButtonContent = new GUIContent("Make Ground Cannon"),
        WallButtonContent = new GUIContent("Make Wall Cannon");

    private new UnmannedCannon target;
    private SerializedProperty typeProperty;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        serializedObject.Update();

        if (GUILayout.Button(GroundButtonContent))
            MakeGroundCannon();

        if (GUILayout.Button(WallButtonContent))
            MakeWallCannon();

        serializedObject.ApplyModifiedProperties();
    }

    private void MakeGroundCannon()
    {
        typeProperty.intValue = (int) CannonType.Ground;
        TryAddMountInteractable();
        TryAddMoveInteractable();
    }

    private void MakeWallCannon()
    {
        typeProperty.intValue = (int) CannonType.Wall;
        TryAddMountInteractable();
        TryRemoveMoveInteractable();
    }

    private void OnEnable()
    {
        target = (UnmannedCannon) base.target;
        typeProperty = serializedObject.FindProperty("type");
    }

    private void TryAddMountInteractable()
    {
        Transform _;
        if (target.transform.TryFindChild(MountInteractableName, out _))
            return;

        var mountInteractable = new GameObject(MountInteractableName, typeof(Interactable)).GetComponent<Interactable>();
        mountInteractable.transform.parent = target.transform;
        mountInteractable.transform.localPosition = Vector3.zero;
        mountInteractable.TryCreateCollider();
        mountInteractable.SetDefaults("Mount Cannon", (UnityEngine.Sprite) null, target.TryMount);
    }

    private void TryAddMoveInteractable()
    {
        Transform _;
        if (target.transform.TryFindChild(MoveInteractableName, out _))
            return;

        var moveInteractable = new GameObject(MoveInteractableName, typeof(Interactable)).GetComponent<Interactable>();
        moveInteractable.transform.parent = target.transform;
        moveInteractable.transform.localPosition = Vector3.zero;
        moveInteractable.TryCreateCollider();
        moveInteractable.gameObject.AddComponent<MoveGroundCannon>();
    }

    private void TryRemoveMoveInteractable()
    {
        Transform moveInteractableTransform;
        if (target.transform.TryFindChild(MoveInteractableName, out moveInteractableTransform))
            DestroyImmediate(moveInteractableTransform.gameObject);
    }
}