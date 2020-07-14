using Assets.Scripts.Legacy.CustomMap;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Editor
{
    public class GameObjectToPrefab : EditorWindow
    {
        [SerializeField] public RCLegacyPrefab rcLegacyPrefab;
        private List<GameObjectInformation> gameObjectCache;

        private bool HasClosed { get; set; }

        [MenuItem("Tools/Replace With Prefab")]
        private static void CreateReplaceWithPrefab()
        {
            EditorWindow.GetWindow<GameObjectToPrefab>();
        }

        private struct GameObjectInformation
        {
            public GameObjectInformation(GameObject gameObject)
            {
                LocalPosition = gameObject.transform.localPosition;
                LocalScale = gameObject.transform.localScale;
                LocalRotation = gameObject.transform.localRotation;
                PrefabName = gameObject.name.Replace("(Clone)", "");
                SiblingIndex = gameObject.transform.GetSiblingIndex();
            }

            public readonly Vector3 LocalPosition;
            public readonly Vector3 LocalScale;
            public readonly Quaternion LocalRotation;
            public readonly string PrefabName;
            public readonly int SiblingIndex;
        }

        private void OnGUI()
        {
            rcLegacyPrefab = (RCLegacyPrefab) EditorGUILayout.ObjectField("RC Legacy Prefabs", rcLegacyPrefab, typeof(RCLegacyPrefab), false);

            if (GUILayout.Button("Replace"))
            {
                gameObjectCache = new List<GameObjectInformation>();
                var selection = Selection.gameObjects;
                foreach (var selected in selection)
                {
                    gameObjectCache.Add(new GameObjectInformation(selected));
                }

                Debug.Log(gameObjectCache.Count);
                EditorApplication.ExecuteMenuItem("Edit/Play");
                HasClosed = true;
            }

            if (HasClosed && !Application.isPlaying)
            {
                HasClosed = false;
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

                foreach (var cachedGameObject in gameObjectCache)
                {
                    var prefab = rcLegacyPrefab.Get(cachedGameObject.PrefabName);

                    var prefabType = PrefabUtility.GetPrefabType(prefab);
                    GameObject newObject;

                    if (prefabType == PrefabType.Prefab)
                    {
                        newObject = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
                    }
                    else
                    {
                        newObject = Instantiate(prefab);
                        newObject.name = prefab.name;
                    }

                    if (newObject == null)
                    {
                        Debug.LogError("Error instantiating prefab");
                        break;
                    }

                    Undo.RegisterCreatedObjectUndo(newObject, "Replace With Prefabs");
                    newObject.transform.localPosition = cachedGameObject.LocalPosition;
                    newObject.transform.localRotation = cachedGameObject.LocalRotation;
                    newObject.transform.localScale = cachedGameObject.LocalScale;
                    newObject.transform.SetSiblingIndex(cachedGameObject.SiblingIndex);
                }
            }

            GUI.enabled = false;
            EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
        }
    }
}
