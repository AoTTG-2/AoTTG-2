using Assets.Scripts.Legacy.CustomMap;
using Assets.Scripts.Room;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Editor
{
    public class GameObjectToPrefab : EditorWindow
    {
        [SerializeField] public RCLegacy RcLegacy;
        private List<GameObjectInformation> gameObjectCache;
        private List<SpawnInformation> spawnersCache;
        private List<TitanSpawnerInformation> titanSpawnerCache;
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
                Materials = gameObject.GetComponentsInChildren<Renderer>().Select(x => new MaterialInformation(x))
                    .ToArray();
                Colors = gameObject.GetComponentsInChildren<MeshFilter>().FirstOrDefault(x => x.mesh.colors.Length > 0)?
                    .mesh.colors.FirstOrDefault();
                Tag = gameObject.tag;
            }

            public readonly Vector3 LocalPosition;
            public readonly Vector3 LocalScale;
            public readonly Quaternion LocalRotation;
            public readonly string PrefabName;
            public readonly int SiblingIndex;
            public readonly MaterialInformation[] Materials;
            public readonly Color? Colors;
            public readonly string Tag;
        }

        private struct MaterialInformation
        {
            public MaterialInformation(Renderer renderer)
            {
                Material = renderer.material.name.Replace("(Instance)", "").Trim();
                TextureScale = renderer.material.mainTextureScale;
            }

            public readonly string Material;
            public readonly Vector2 TextureScale;
        }

        private enum SpawnType
        {
            HumanCyan,
            HumanMagenta,
            Titan
        }

        private struct SpawnInformation
        {
            public SpawnInformation(Vector3 position, SpawnType type)
            {
                Position = position;
                Type = type;
            }

            public readonly Vector3 Position;
            public readonly SpawnType Type;
        }

        private struct TitanSpawnerInformation
        {
            public TitanSpawnerInformation(TitanSpawner spawner)
            {
                Position = spawner.gameObject.transform.position;
                Delay = spawner.Delay;
                Endless = spawner.Endless;
                Type = spawner.Type;
            }

            public readonly Vector3 Position;
            public readonly float Delay;
            public readonly bool Endless;
            public readonly TitanSpawnerType Type;
        }

        private void OnGUI()
        {
            RcLegacy = (RCLegacy) EditorGUILayout.ObjectField("RC Legacy Prefabs", RcLegacy, typeof(RCLegacy), false);

            if (GUILayout.Button("Convert"))
            {
                gameObjectCache = new List<GameObjectInformation>();
                spawnersCache = new List<SpawnInformation>();
                titanSpawnerCache = new List<TitanSpawnerInformation>();
                var sceneObjects = new List<GameObject>();
                Scene scene = SceneManager.GetActiveScene();
                scene.GetRootGameObjects(sceneObjects);
                foreach (var selected in sceneObjects)
                {
                    if (selected.name.Contains("titanRespawn")) continue;
                    if (selected.name == "playerRespawn")
                    {
                        foreach (Transform child in selected.transform)
                        {
                            child.gameObject.transform.localScale = new Vector3(20, 1, 20);
                            gameObjectCache.Add(new GameObjectInformation(child.gameObject));
                        }
                    }
                    else
                    {
                        gameObjectCache.Add(new GameObjectInformation(selected));
                    }
                }

                FengGameManagerMKII.instance.playerSpawnsC.ForEach(x =>
                    spawnersCache.Add(new SpawnInformation(x, SpawnType.HumanCyan)));
                FengGameManagerMKII.instance.playerSpawnsM.ForEach(x =>
                    spawnersCache.Add(new SpawnInformation(x, SpawnType.HumanMagenta)));

                FengGameManagerMKII.instance.TitanSpawners.ForEach(x => titanSpawnerCache.Add(new TitanSpawnerInformation(x)));

                Debug.Log(gameObjectCache.Count);
                EditorApplication.ExecuteMenuItem("Edit/Play");
                HasClosed = true;
            }

            if (HasClosed && !Application.isPlaying)
            {
                HasClosed = false;
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                CreateObjects();
                spawnersCache.ForEach(CreateSpawner);
                titanSpawnerCache.ForEach(CreateTitanSpawner);
            }

            GUI.enabled = false;
        }

        private void CreateObjects()
        {
            PrefabUtility.InstantiatePrefab(RcLegacy.GetPrefab("cameraDefaultPosition"));
            PrefabUtility.InstantiatePrefab(RcLegacy.GetPrefab("Cube_001"));
            PrefabUtility.InstantiatePrefab(RcLegacy.GetPrefab("LightSet"));

            foreach (var cachedGameObject in gameObjectCache)
            {
                var prefab = RcLegacy.GetPrefab(cachedGameObject.PrefabName);
                if (prefab == null) continue;
                var prefabType = PrefabUtility.GetPrefabType(prefab);
                if (prefabType == PrefabType.Prefab)
                {
                    CreatePrefab(prefab, cachedGameObject);
                }
            }
        }

        private void CreatePrefab(GameObject prefab, GameObjectInformation information)
        {
            var gameObject = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
            if (gameObject == null)
            {
                Debug.LogError($"Error instantiating prefab: {information.PrefabName}");
                return;
            }

            gameObject.transform.localPosition = information.LocalPosition;
            gameObject.transform.localRotation = information.LocalRotation;
            gameObject.transform.localScale = information.LocalScale;
            gameObject.transform.SetSiblingIndex(information.SiblingIndex);

            var renderers = gameObject.GetComponentsInChildren<Renderer>();
            for (var i = 0; i < renderers.Length; i++)
            {
                try
                {
                    renderers[i].material = RcLegacy.GetMaterial(information.Materials[i].Material);
                    renderers[i].material.mainTextureScale = information.Materials[i].TextureScale;
                }
                catch
                {
                    Debug.LogError($"Material exception");
                }

                try
                {
                    if (information.Colors.HasValue)
                    {
                        renderers[i].material.color = information.Colors.Value;
                    }
                }
                catch
                {
                    Debug.LogError($"Color: {information.Colors} exception");
                }

            }
        }

        private void CreateSpawner(SpawnInformation information)
        {
            if (information.Type == SpawnType.HumanCyan)
            {
                var prefab = RcLegacy.GetPrefab("playerRespawn");
                var gameObject = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
                gameObject.transform.position = information.Position;
                gameObject.GetComponent<PlayerSpawner>().Type = PlayerSpawnType.Cyan;
            }
            else if (information.Type == SpawnType.HumanMagenta)
            {
                var prefab = RcLegacy.GetPrefab("playerRespawn");
                var gameObject = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
                gameObject.transform.position = information.Position;
                gameObject.GetComponent<PlayerSpawner>().Type = PlayerSpawnType.Magenta;
            }
            else if (information.Type == SpawnType.Titan)
            {

            }
        }

        private void CreateTitanSpawner(TitanSpawnerInformation information)
        {
            var prefab = RcLegacy.GetPrefab("titanRespawn");
            var gameObject = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
            gameObject.transform.position = information.Position;
            var spawner = gameObject.GetComponent<TitanSpawner>();
            spawner.Delay = information.Delay;
            spawner.Endless = information.Endless;
            spawner.Type = information.Type;
        }

    }
}
