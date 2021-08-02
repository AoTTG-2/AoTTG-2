using Assets.Scripts.Settings.New;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Settings
{
    [CustomEditor(typeof(BaseSettings), true)]
    public class BaseSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = (BaseSettings) target;

            if (GUILayout.Button("Force Override", GUILayout.Height(40)))
            {
                script.ForceLocalOverride();
            }
            if (GUILayout.Button("Save to File", GUILayout.Height(40)))
            {
                script.Save();
            }

        }
    }
}
