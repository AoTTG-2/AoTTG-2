using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Prefabs
{
    public class PrefabReplacer : EditorWindow
    {
        [MenuItem("Tools/Prefabs/Replacer")]
        static void Open()
        {
            var win = GetWindow<PrefabReplacer>();
            win.Show();
        }

        public GameObject prefab;

        private void OnGUI()
        {
            prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);
            GUILayout.Label("Selected Objects:");
#if UNITY_EDITOR
            GUILayout.Label(Selection.gameObjects != null ? Selection.count.ToString() : 0.ToString());
#endif

            if (GUILayout.Button("Replace All"))
            {
                var objs = Selection.gameObjects;
                for(int i = 0; i < objs.Length; i++)
                {
                    var cur = objs[i];
                    var p = (GameObject)PrefabUtility.InstantiatePrefab(prefab, cur.transform.parent);
                    p.transform.position = cur.transform.position;
                    p.transform.rotation = cur.transform.rotation;
                    p.transform.localScale = cur.transform.localScale;

                    cur.SetActive(false);
                }
            }
        }
    }
}