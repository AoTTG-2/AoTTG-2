using Assets.Scripts.Interfaces;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    public class ViewIDebuggables : EditorWindow
    {
        GameObject selected;
        IDebugable debuggable;
        StringBuilder sb;

        [MenuItem("Window/Debug/View IDebugable")]
        static void Open()
        {
            var win = GetWindow<ViewIDebuggables>();
            win.titleContent.text = "View IDebugable";
            win.Show();
        }

        private void OnEnable()
        {
            sb = new StringBuilder();
            Selection.selectionChanged += OnSelectionChanged;
            OnSelectionChanged();
        }

        private void OnSelectionChanged()
        {
            if (Selection.activeGameObject)
            {
                selected = Selection.activeGameObject;
                debuggable = selected.GetComponent<IDebugable>();
            }
            else
            {
                debuggable = null;
                selected = null;
            }
            Repaint();
        }

        private void OnGUI()
        {
            if (debuggable != null)
            {
                GUI.enabled = false;
                EditorGUILayout.ObjectField("Selected: ", selected, typeof(GameObject), true);
                GUI.enabled = true;
                GUILayout.Space(10);

                sb.Clear();
                if (debuggable == null)
                    return;
                GUILayout.Label(debuggable.GetDebugString(sb));
                Repaint();
            }
            else
            {
                EditorGUILayout.HelpBox("Select an Object with a Component that implements IDebugable", MessageType.Warning);
            }
        }
    }

}
