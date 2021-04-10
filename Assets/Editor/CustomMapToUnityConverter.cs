using Assets.Scripts.Gamemode.Racing;
using Assets.Scripts.Legacy.CustomMap;
using Assets.Scripts.Room;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets.Editor
{
    public class CustomMapToUnityConverter : EditorWindow
    {
        public RCLegacy RcLegacy;
        public TextAsset CustomMap;

        [MenuItem("Tools/Custom Map To Unity Scene")]
        private static void CreateReplaceWithPrefab()
        {
            EditorWindow.GetWindow<CustomMapToUnityConverter>();
        }

        private void OnGUI()
        {
            RcLegacy = (RCLegacy) EditorGUILayout.ObjectField("RC Legacy Prefabs", RcLegacy, typeof(RCLegacy), false);
            CustomMap = (TextAsset) EditorGUILayout.ObjectField("Custom Map", CustomMap, typeof(TextAsset), false);

            if (GUILayout.Button("Convert"))
            {
                if (RcLegacy == null || CustomMap == null)
                {
                    Debug.LogError("RcLegacy or CustomMap field is empty");
                    return;
                }

                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                var level = CustomMap.text.Split(';').Select(x => x.Trim()).ToArray();
                CreateLevel(level);
            }

            GUI.enabled = false;
        }

        private void CreateLevel(string[] content)
        {
            PrefabUtility.InstantiatePrefab(RcLegacy.GetPrefab("cameraDefaultPosition"));
            PrefabUtility.InstantiatePrefab(RcLegacy.GetPrefab("Cube_001"));
            PrefabUtility.InstantiatePrefab(RcLegacy.GetPrefab("LightSet"));
            PrefabUtility.InstantiatePrefab(RcLegacy.GetPrefab("gameobjectOutSide"));

            int num;
            string[] strArray;
            for (num = 0; num < content.Length; num++)
            {
                try
                {
                    float num2;
                    GameObject obj2;
                    float num3;
                    float num5;
                    float num6;
                    float num7;
                    Color color;
                    Mesh mesh;
                    Color[] colorArray;
                    int num8;
                    strArray = content[num].Split(new char[] {','});
                    if (strArray[0].StartsWith("custom"))
                    {
                        num2 = 1f;
                        obj2 = CreatePrefab(strArray, RcObjectType.Custom);

                        // Objects which start with custom and use a racing prefab, should not have a script
                        DestroyImmediate(obj2.GetComponentInChildren<RacingKillTrigger>());
                        DestroyImmediate(obj2.GetComponentInChildren<RacingCheckpointTrigger>());
                        DestroyImmediate(obj2.GetComponentInChildren<RacingStartBarrier>());

                        if (strArray[2] != "default")
                        {
                            if (strArray[2].StartsWith("transparent"))
                            {
                                if (float.TryParse(strArray[2].Substring(11), out num3))
                                {
                                    num2 = num3;
                                }

                                foreach (Renderer renderer in obj2.GetComponentsInChildren<Renderer>())
                                {
                                    renderer.material = RcLegacy.GetMaterial("transparent");
                                    if ((Convert.ToSingle(strArray[10]) != 1f) ||
                                        (Convert.ToSingle(strArray[11]) != 1f))
                                    {
                                        renderer.material.mainTextureScale = new Vector2(
                                            renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]),
                                            renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                    }
                                }
                            }
                            else
                            {
                                foreach (Renderer renderer in obj2.GetComponentsInChildren<Renderer>())
                                {
                                    renderer.material = RcLegacy.GetMaterial(strArray[2]);
                                    if ((Convert.ToSingle(strArray[10]) != 1f) ||
                                        (Convert.ToSingle(strArray[11]) != 1f))
                                    {
                                        renderer.material.mainTextureScale = new Vector2(
                                            renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]),
                                            renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                    }
                                }
                            }
                        }

                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[3]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[4]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[5]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        if (strArray[6] != "0")
                        {
                            color = new Color(Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]),
                                Convert.ToSingle(strArray[9]), num2);
                            foreach (MeshFilter filter in obj2.GetComponentsInChildren<MeshFilter>())
                            {
                                mesh = filter.mesh;
                                colorArray = new Color[mesh.vertexCount];
                                num8 = 0;
                                while (num8 < mesh.vertexCount)
                                {
                                    colorArray[num8] = color;
                                    num8++;
                                }

                                mesh.colors = colorArray;
                            }
                        }
                    }
                    else if (strArray[0].StartsWith("base"))
                    {
                        if (strArray.Length < 15)
                        {
                            CreatePrefab(strArray, RcObjectType.BaseSpecial);
                        }
                        else
                        {
                            num2 = 1f;
                            obj2 = CreatePrefab(strArray, RcObjectType.Base);
                            if (strArray[2] != "default")
                            {
                                if (strArray[2].StartsWith("transparent"))
                                {
                                    if (float.TryParse(strArray[2].Substring(11), out num3))
                                    {
                                        num2 = num3;
                                    }

                                    foreach (Renderer renderer in obj2.GetComponentsInChildren<Renderer>())
                                    {
                                        renderer.material = RcLegacy.GetMaterial("transparent");
                                        if ((Convert.ToSingle(strArray[10]) != 1f) ||
                                            (Convert.ToSingle(strArray[11]) != 1f))
                                        {
                                            renderer.material.mainTextureScale =
                                                new Vector2(
                                                    renderer.material.mainTextureScale.x *
                                                    Convert.ToSingle(strArray[10]),
                                                    renderer.material.mainTextureScale.y *
                                                    Convert.ToSingle(strArray[11]));
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (Renderer renderer in obj2.GetComponentsInChildren<Renderer>())
                                    {
                                        if (!(renderer.name.Contains("Particle System") &&
                                              obj2.name.Contains("aot_supply")))
                                        {
                                            renderer.material = RcLegacy.GetMaterial(strArray[2]);
                                            if ((Convert.ToSingle(strArray[10]) != 1f) ||
                                                (Convert.ToSingle(strArray[11]) != 1f))
                                            {
                                                renderer.material.mainTextureScale =
                                                    new Vector2(
                                                        renderer.material.mainTextureScale.x *
                                                        Convert.ToSingle(strArray[10]),
                                                        renderer.material.mainTextureScale.y *
                                                        Convert.ToSingle(strArray[11]));
                                            }
                                        }
                                    }
                                }
                            }

                            num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[3]);
                            num5 -= 0.001f;
                            num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[4]);
                            num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[5]);
                            obj2.transform.localScale = new Vector3(num5, num6, num7);
                            if (strArray[6] != "0")
                            {
                                color = new Color(Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]),
                                    Convert.ToSingle(strArray[9]), num2);
                                foreach (MeshFilter filter in obj2.GetComponentsInChildren<MeshFilter>())
                                {
                                    mesh = filter.mesh;
                                    colorArray = new Color[mesh.vertexCount];
                                    for (num8 = 0; num8 < mesh.vertexCount; num8++)
                                    {
                                        colorArray[num8] = color;
                                    }

                                    mesh.colors = colorArray;
                                }
                            }
                        }
                    }
                    else if (strArray[0].StartsWith("misc"))
                    {
                        if (strArray[1].StartsWith("barrier"))
                        {
                            obj2 = CreatePrefab(strArray, RcObjectType.Misc);
                            num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                            num5 -= 0.001f;
                            num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                            num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                            obj2.transform.localScale = new Vector3(num5, num6, num7);
                        }
                        else if (strArray[1].StartsWith("racingStart"))
                        {
                            obj2 = CreatePrefab(strArray, RcObjectType.Misc);
                            num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                            num5 -= 0.001f;
                            num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                            num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                            obj2.transform.localScale = new Vector3(num5, num6, num7);
                        }
                        else if (strArray[1].StartsWith("racingEnd"))
                        {
                            obj2 = CreatePrefab(strArray, RcObjectType.Misc);
                            num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                            num5 -= 0.001f;
                            num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                            num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                            obj2.transform.localScale = new Vector3(num5, num6, num7);
                            obj2.AddComponent<LevelTriggerRacingEnd>();
                        }
                        else if (strArray[1].StartsWith("region") && PhotonNetwork.isMasterClient)
                        {
                            Debug.LogWarning("RC Regions are obsolete in AoTTG2");
                        }
                    }
                    else if (strArray[0].StartsWith("racing"))
                    {
                        if (strArray[1].StartsWith("start"))
                        {
                            obj2 = CreatePrefab(strArray, RcObjectType.Racing);
                            num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                            num5 -= 0.001f;
                            num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                            num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                            obj2.transform.localScale = new Vector3(num5, num6, num7);
                        }
                        else if (strArray[1].StartsWith("end"))
                        {
                            obj2 = CreatePrefab(strArray, RcObjectType.Racing);
                            num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                            num5 -= 0.001f;
                            num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                            num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                            obj2.transform.localScale = new Vector3(num5, num6, num7);
                            obj2.GetComponentInChildren<Collider>().gameObject.AddComponent<LevelTriggerRacingEnd>();
                        }
                        else if (strArray[1].StartsWith("kill"))
                        {
                            obj2 = CreatePrefab(strArray, RcObjectType.Racing);
                            num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                            num5 -= 0.001f;
                            num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                            num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                            obj2.transform.localScale = new Vector3(num5, num6, num7);
                            obj2.GetComponentInChildren<Collider>().gameObject.AddComponent<RacingKillTrigger>();
                        }
                        else if (strArray[1].StartsWith("checkpoint"))
                        {
                            obj2 = CreatePrefab(strArray, RcObjectType.Racing);
                            num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                            num5 -= 0.001f;
                            num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                            num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                            obj2.transform.localScale = new Vector3(num5, num6, num7);
                            obj2.GetComponentInChildren<Collider>().gameObject.AddComponent<RacingCheckpointTrigger>();
                        }
                    }
                    else if (strArray[0].StartsWith("map"))
                    {
                        if (strArray[1].StartsWith("disablebounds"))
                        {
                            UnityEngine.Object.Destroy(GameObject.Find("gameobjectOutSide"));
                            //Outside Barriers are no longer required as per issue 197
                        }
                    }
                    else if (strArray[0].StartsWith("spawnpoint"))
                    {
                        strArray = content[num].Split(new char[] {','});
                        var position = new Vector3(Convert.ToSingle(strArray[2]),
                            Convert.ToSingle(strArray[3]),
                            Convert.ToSingle(strArray[4]));
                        var rotation = new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]));

                        if (strArray[1] == "titan")
                        {
                            CreatePrefab("titanRespawn", position, rotation);
                        }
                        else if (strArray[1] == "playerC")
                        {
                            var playerRespawn = CreatePrefab("playerRespawn", position, rotation).GetComponent<HumanSpawner>();
                            playerRespawn.Type = PlayerSpawnType.Cyan;
                        }
                        else if (strArray[1] == "playerM")
                        {
                            var playerRespawn = CreatePrefab("playerRespawn", position, rotation).GetComponent<HumanSpawner>();
                            playerRespawn.Type = PlayerSpawnType.Magenta;
                        }
                    }
                    else if (strArray[0].StartsWith("photon"))
                    {
                        if (strArray[1].StartsWith("Cannon"))
                        {
                            Debug.LogWarning("Cannons are not supported yet");
                            //if (strArray.Length > 15)
                            //{
                            //    GameObject go = PhotonNetwork.Instantiate("RCAsset/" + strArray[1] + "Prop",
                            //        new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]),
                            //            Convert.ToSingle(strArray[14])),
                            //        new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[0x10]),
                            //            Convert.ToSingle(strArray[0x11]), Convert.ToSingle(strArray[0x12])), 0);
                            //    go.GetComponent<CannonPropRegion>().settings = content[num];
                            //    go.GetPhotonView().RPC(nameof(SetSize), PhotonTargets.AllBuffered,
                            //        new object[] {content[num]});
                            //}
                            //else
                            //{
                            //    PhotonNetwork.Instantiate("RCAsset/" + strArray[1] + "Prop",
                            //            new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]),
                            //                Convert.ToSingle(strArray[4])),
                            //            new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]),
                            //                Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])), 0)
                            //        .GetComponent<CannonPropRegion>().settings = content[num];
                            //}
                        }
                        else if (strArray[1].StartsWith("spawn"))
                        {
                            var titanSpawnerObject = CreatePrefab("titanRespawn", new Vector3(
                                Convert.ToSingle(strArray[4]),
                                Convert.ToSingle(strArray[5]),
                                Convert.ToSingle(strArray[6])));

                            var titanSpawner = titanSpawnerObject.GetComponent<TitanSpawner>();
                            num5 = 30f;
                            if (float.TryParse(strArray[2], out num3))
                            {
                                num5 = Mathf.Max(Convert.ToSingle(strArray[2]), 1f);
                            }

                            titanSpawner.Type = GetTitanSpawnerType(strArray[1]);
                            titanSpawner.Delay = num5;
                            titanSpawner.Endless = strArray[3] == "1";
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"{content[num]}: {e.StackTrace}");
                }
            }

            if (GameObject.FindObjectOfType<HumanSpawner>() == null)
            {
                PrefabUtility.InstantiatePrefab(RcLegacy.GetPrefab("playerRespawn"));
            }
        }

        private GameObject CreatePrefab(string[] mapData, RcObjectType type)
        {
            var gameObject = (GameObject) PrefabUtility.InstantiatePrefab(RcLegacy.GetPrefab(mapData[1]));
            var position = GetPosition(mapData, type);
            var rotation = GetRotation(mapData, type);
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            return gameObject;
        }

        private GameObject CreatePrefab(string prefabName, Vector3 position, Quaternion rotation = new Quaternion())
        {
            var gameObject = (GameObject) PrefabUtility.InstantiatePrefab(RcLegacy.GetPrefab(prefabName));
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            return gameObject;
        }

        private static Vector3 GetPosition(string[] mapData, RcObjectType type)
        {
            var index = GetPositionIndex(type);
            return new Vector3(Convert.ToSingle(mapData[index[0]]), Convert.ToSingle(mapData[index[1]]), Convert.ToSingle(mapData[index[2]]));
        }

        private static Quaternion GetRotation(string[] mapData, RcObjectType type)
        {
            var index = GetRotationIndex(type);
            return new Quaternion(Convert.ToSingle(mapData[index[0]]), Convert.ToSingle(mapData[index[1]]),
                Convert.ToSingle(mapData[index[2]]), Convert.ToSingle(mapData[index[3]]));
        }

        private static int[] GetPositionIndex(RcObjectType type)
        {
            switch (type)
            {
                case RcObjectType.Custom:
                case RcObjectType.Base:
                    return new[] { 12, 13, 14 };
                case RcObjectType.BaseSpecial:
                case RcObjectType.Spawnpoint:
                    return new[] { 2, 3, 4 };
                case RcObjectType.Misc:
                case RcObjectType.Racing:
                    return new[] { 5, 6, 7 };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static int[] GetRotationIndex(RcObjectType type)
        {
            switch (type)
            {
                case RcObjectType.Custom:
                case RcObjectType.Base:
                    return new[] { 15, 16, 17, 18 };
                case RcObjectType.BaseSpecial:
                case RcObjectType.Spawnpoint:
                    return new[] { 5, 6, 7, 8 };
                case RcObjectType.Misc:
                case RcObjectType.Racing:
                    return new[] { 8, 9, 10, 11 };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private TitanSpawnerType GetTitanSpawnerType(string type)
        {
            if (type.Contains("Abnormal"))
            {
                return TitanSpawnerType.Aberrant;
            } 
            
            if (type.Contains("Crawler"))
            {
                return TitanSpawnerType.Crawler;
            }

            if (type.Contains("Jumper"))
            {
                return TitanSpawnerType.Jumper;
            }

            if (type.Contains("Punk"))
            {
                return TitanSpawnerType.Punk;
            }

            if (type.Contains("Titan"))
            {
                return TitanSpawnerType.Normal;
            }

            if (type.Contains("Annie"))
            {
                return TitanSpawnerType.Annie;
            }

            return TitanSpawnerType.Normal;
        }


    }
}
