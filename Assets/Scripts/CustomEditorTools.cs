using UnityEditor;
using UnityEngine;

public class CustomEditorTools : MonoBehaviour
{
    [MenuItem("Assets/Tools/Set Dirty")]
    private static void SetDirty()
    {
        foreach (var o in Selection.objects)
            EditorUtility.SetDirty(o);
    }
}
