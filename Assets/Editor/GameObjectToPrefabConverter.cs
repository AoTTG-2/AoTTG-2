using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    public class GameObjectToPrefabConverter : EditorWindow
    {
        public GameObject Prefab;
        [Tooltip("Modifies the scale by x, y, z")]
        public Vector3 ScaleMultiplier = new Vector3(1, 1, 1);

        [MenuItem("Tools/GameObject to Prefab")]
        private static void CreateReplaceWithPrefab()
        {
            EditorWindow.GetWindow<GameObjectToPrefabConverter>();
        }

        private void OnGUI()
        {
            Prefab = (GameObject) EditorGUILayout.ObjectField("Prefab", Prefab, typeof(GameObject), false);
            ScaleMultiplier = EditorGUILayout.Vector3Field("Scale Multiplier", ScaleMultiplier);

            if (GUILayout.Button("Convert"))
            {
                var gameObjects = Selection.gameObjects;
                if (Prefab == null)
                {
                    Debug.LogError("Prefab field is empty");
                    return;
                }

                foreach (var gameObject in gameObjects)
                {
                    SwapGameObjectWithPrefab(gameObject);
                }
                Debug.Log($"GOP:\tConverted: {gameObjects.Length} objects to {Prefab.gameObject.name}");
            }

            GUI.enabled = false;
        }

        private void SwapGameObjectWithPrefab(GameObject gameObject)
        {
            var prefab = (GameObject) PrefabUtility.InstantiatePrefab(Prefab);
            try
            {
                var transform = gameObject.transform;
                prefab.transform.position = transform.position;
                prefab.transform.rotation = transform.rotation;
                prefab.transform.localScale = Multiply(transform.lossyScale, prefab.transform.localScale);
                prefab.transform.parent = transform.parent;
                prefab.transform.SetSiblingIndex(transform.GetSiblingIndex());
                DestroyImmediate(gameObject);
            }
            // If the user wants to delete a GameObject which is inside a prefab, without using the prefab editor
            // then the DestroyImmediate(gameObject) will throw an invalid operation exception.
            // If it does, then we want to delete the created prefab as well
            catch (InvalidOperationException)
            {
                Debug.LogError($"GOP:\tCannot delete {gameObject.name}. Is this GameObject inside a prefab and you are not within the prefab editor?");
                DestroyImmediate(prefab);
            }
        }

        private Vector3 Multiply(Vector3 gameObjectLossyScale, Vector3 prefabLocalScale)
        {
            var result = Vector3.Scale(gameObjectLossyScale, prefabLocalScale);
            return Vector3.Scale(result, ScaleMultiplier);
        }
    }
}
