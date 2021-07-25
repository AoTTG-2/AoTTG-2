using Assets.Scripts.Constants;
using Assets.Scripts.CustomMaps;
using Assets.Scripts.Extensions;
using Assets.Scripts.Legacy.CustomMap;
using Assets.Scripts.Services.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Services
{
    public class CustomMapService : MonoBehaviour, ICustomMapService
    {
        public CustomMapConfiguration Configuration;
        public MapMaterial Transparent;
        public MapMaterial TransparentDoubleSided;
        public MapObject SpawnTitan;
        public MapObject SpawnHuman;
        private static readonly Color BarrierColor = new Color(0f, 234f / 255f, 1f, 82f / 255f);
        /// <summary>
        /// Default value for float.TryParse
        /// </summary>
        private static readonly NumberStyles NumberStyles = NumberStyles.Float | NumberStyles.AllowThousands;

        public List<CustomMap> GetCustomMaps()
        {
            var path = $"{Application.streamingAssetsPath}{Path.AltDirectorySeparatorChar}Custom Maps";
            var files = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories);
            var customMaps = files.Select(x => 
                new CustomMap($"{Path.GetFileNameWithoutExtension(x)}", x)).ToList();
            return customMaps;
        }

        private CustomMap CurrentMap { get; set; }

        private async void OnLevelWasLoaded(int level)
        {
            if ((level != 0) && ((Application.loadedLevelName != "characterCreation") && (Application.loadedLevelName != "SnapShot")))
            {
                while (Service.Settings.Get() == null)
                {
                    await Task.Delay(500);
                }
            }

            if (SceneManager.GetSceneByBuildIndex(level).name == "Custom")
            {
                //TODO: What to do if the custom map doesn't exist?
                var levelz = FengGameManagerMKII.Level;
                RenderSettings.fog = false;
                CurrentMap = GetCustomMaps().SingleOrDefault(x => x.Name == levelz.Name);
                var content = File.ReadAllText(CurrentMap.Path);
                var objects = content.Split(new[] { ";;" }, StringSplitOptions.RemoveEmptyEntries).Select(x => $"{x.Trim()};").ToArray();
                LoadCustomMap(objects);
                Service.Level.InvokeLevelLoaded(level, null);
            }
            else
            {
                RenderSettings.fog = true;
                Service.Level.InvokeLevelLoaded(level, null);
            }
        }

        public void Load(string mapName)
        {
            CurrentMap = GetCustomMaps().SingleOrDefault(x => x.Name == mapName);
            PhotonNetwork.LoadLevel("Custom");
        }

        public void LoadScene(string sceneName)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }

        #region Custom Map Loading
        public void LoadCustomMap(string[] objects)
        {
            var baseParent = new GameObject("Custom Map");
            var prefabGameObjects = new Dictionary<string, GameObject>();
            var prefabs = new List<CustomMapPrefab>();
            CustomMapPrefab? currentPrefab = null;
            foreach (var item in objects)
            {
                try
                {
                    var attributes = item.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim()).ToArray();

                    if (attributes[0] == "prefab")
                    {
                        currentPrefab = new CustomMapPrefab(attributes);
                        continue;
                    }
                    
                    if (attributes[0] == "prefabend")
                    {
                        var prefabValue = currentPrefab.Value;
                        var prefabGameObject = Instantiate(new GameObject(prefabValue.Name));
                        foreach (var prefabContent in currentPrefab.Value.Objects)
                        {
                            var prefabContentGameObject = CreateGameObject(prefabContent);
                            prefabContentGameObject.transform.parent = prefabGameObject.transform;
                        }

                        prefabGameObjects.Add(prefabValue.Name, prefabGameObject);
                        prefabs.Add(prefabValue);
                        currentPrefab = null;
                        continue;
                    }

                    var mapItem = new CustomMapObject(attributes);
                    if (currentPrefab.HasValue)
                    {
                        mapItem.SetPrefabOffset(currentPrefab.Value.Offset);
                        currentPrefab.Value.Objects.Add(mapItem);
                        continue;
                    }

                    var createdObject = CreateGameObject(mapItem);
                    StaticBatchingUtility.Combine(createdObject);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Custom Map Loader: Unhandled exception for {item}");
                    Debug.LogError(e);
                }
            }

            GameObject CreateGameObject(CustomMapObject mapItem)
            {
                GameObject mapGameObject;
                var mapObject = GetMapObject(mapItem.ModelName);
                if (mapItem.Prefab != null)
                {
                    if (prefabs.All(x => x.Name != mapItem.Prefab) || !prefabGameObjects.TryGetValue(mapItem.Prefab, out var prefabGameObject))
                    {
                        Debug.LogWarning($"Custom Map Loader: PREFAB {mapItem.Prefab} DOES NOT EXIST");
                        return null;
                    }
                    mapGameObject = Instantiate(prefabGameObject, mapItem.Position, new Quaternion());
                    mapGameObject.transform.rotation = Quaternion.Euler(mapItem.Rotation);
                }
                else
                {
                    if (mapObject == null)
                    {
                        Debug.LogWarning($"Custom Map Loader: MODEL OBJECT {mapItem.ModelName} DOES NOT EXIST");
                        return null;
                    }
                    mapGameObject = Instantiate(mapObject.Prefab, mapItem.Position,
                        Quaternion.Euler(mapItem.Rotation));
                }

                mapGameObject.name = mapItem.Identifier;
                mapGameObject.transform.localScale =
                    Vector3.Scale(mapGameObject.transform.localScale, mapItem.Scale);
                mapGameObject.transform.parent =
                    baseParent.transform; //TODO: Objects which do have parents, should not use this

                if (mapItem.Layer.HasValue)
                {
                    mapGameObject.layer = mapItem.Layer.Value;
                    foreach (Transform child in mapGameObject.transform)
                    {
                        if (child == null) continue;
                        child.gameObject.layer = mapItem.Layer.Value;
                    }
                }

                var material = GetMapMaterial(mapItem.Material);
                var texture = GetMapTexture(mapItem.Texture);

                //TODO: Should be moved into graphics settings
                foreach (var renderer in mapGameObject.GetComponentsInChildren<Renderer>())
                {
                    renderer.shadowCastingMode = ShadowCastingMode.Off;
                    renderer.lightProbeUsage = LightProbeUsage.Off;
                    renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
                }

                if (material != null || texture != null || mapItem.Color.HasValue)
                {
                    foreach (var renderer in mapGameObject.GetComponentsInChildren<Renderer>())
                    {
                        if (material != null)
                        {
                            renderer.material = Instantiate(material.Material);
                        }

                        if (texture != null)
                        {
                            renderer.material.mainTexture = texture.Texture;
                            if (mapItem.Tiling.HasValue)
                            {
                                renderer.material.mainTextureScale = new Vector2(
                                    renderer.material.mainTextureScale.x * mapItem.Tiling.Value.x,
                                    renderer.material.mainTextureScale.y * mapItem.Tiling.Value.y);
                            }
                        }

                        //TODO: Only apply vertex coloring if a Vertex Shader is used
                        if (mapItem.Color.HasValue)
                        {
                            renderer.material.color = mapItem.Color.Value;
                        }

                        //if (mapItem.Color.HasValue)
                        //{
                        //    foreach (MeshFilter filter in mapGameObject.GetComponentsInChildren<MeshFilter>())
                        //    {
                        //        var mesh = filter.mesh;
                        //        var colorArray = new Color[mesh.vertexCount];
                        //        var targetColor = mapItem.Color.Value;
                        //        var num8 = 0;
                        //        while (num8 < mesh.vertexCount)
                        //        {
                        //            colorArray[num8] = targetColor;
                        //            num8++;
                        //        }

                        //        mesh.colors = colorArray;
                        //    }
                        //}
                        //}
                    }
                }

                foreach (var component in mapItem.Components)
                {
                    var mapComponent = GetMapComponent(component.Name);
                    if (mapComponent != null)
                    {
                        mapComponent.AddComponent(mapGameObject, mapObject, material, component.Args);
                    }
                }

                return mapGameObject;
            }
        }

        private MapObject GetMapObject(string objectName)
        {
            if (objectName == null) return null;
            return Configuration.MapObjects.SingleOrDefault(x => x.Name == objectName);
        }

        private MapMaterial GetMapMaterial(string materialName)
        {
            if (materialName == null) return null;
            var material = Configuration.MapMaterials.SingleOrDefault(x => x.Name == materialName);
            return material == null ? null : material;
        }

        private MapTexture GetMapTexture(string textureName)
        {
            if (textureName == null) return null;
            var texture = Configuration.MapTextures.SingleOrDefault(x => x.Name == textureName);
            return texture == null ? null : texture;
        }

        private MapComponent GetMapComponent(string componentName)
        {
            if (componentName == null) return null;
            return Configuration.MapComponents.SingleOrDefault(x => x.Name == componentName);
        }
        
        private interface ICustomMapItem
        {
            
        }

        private struct CustomMapPrefab : ICustomMapItem
        {
            public string Name { get; }
            public Vector3 Offset { get; }
            public List<CustomMapObject> Objects { get; }

            public CustomMapPrefab(string[] attributes)
            {
                Name = attributes.SingleOrDefault(x => x.Contains("n:"))?.Split(':')[1];
                Offset = ToVector3(attributes, "off") ?? Vector3.zero;
                Objects = new List<CustomMapObject>();
            }

            private static Vector3? ToVector3(string[] attributes, string attributeName)
            {
                var data = attributes.SingleOrDefault(x => x.StartsWith($"{attributeName}:"))?.Split(':')[1];
                if (data == null) return null;

                var xyz = data.Split(',');
                if (xyz.Length != 3) return null;
                if (float.TryParse(xyz[0], NumberStyles, CultureInfo.InvariantCulture, out var x) 
                    && float.TryParse(xyz[1], NumberStyles, CultureInfo.InvariantCulture, out var y) 
                    && float.TryParse(xyz[2], NumberStyles, CultureInfo.InvariantCulture, out var z))
                {
                    return new Vector3(x, y, z);
                }
                return null;
            }
        }

        private struct CustomMapObject : ICustomMapItem
        {
            public string Identifier { get; }
            public Vector3 Position { get; private set; }


            public string ModelName { get; }
            public string Prefab { get;  }
            public string Texture { get; }
            public string Material { get; }
            public Vector3 Rotation { get; }
            public Vector3 Scale { get; }
            public Vector2? Tiling { get; }
            public Color? Color { get; }
            public List<CustomMapComponent> Components { get; }
            public byte? Layer { get; }

            public CustomMapObject(string[] attributes)
            {
                var id = attributes.SingleOrDefault(x => x.Contains("i:"))?.Split(':')[1];
                Identifier = id;

                Position = ToVector3(attributes, "pos") ?? Vector3.zero;
                Rotation = ToVector3(attributes, "rot") ?? Vector3.zero;
                Scale = ToVector3(attributes, "scl") ?? Vector3.one;

                ModelName = attributes.SingleOrDefault(x => x.StartsWith("m:"))?.Split(':')[1];
                Prefab = ToString(attributes, "pre");
                Material = attributes.SingleOrDefault(x => x.StartsWith("mat:"))?.Split(':')[1];
                Texture = attributes.SingleOrDefault(x => x.StartsWith("t:"))?.Split(':')[1];
                Components = new List<CustomMapComponent>();

                var components = attributes.Where(x => x.StartsWith("c:")).Select(x => x.Split(new []{':'}, 2)[1]).ToArray();
                foreach (var component in components)
                {
                    string[] args = null;
                    if (component.Contains(','))
                    {
                        var componentSplit = component.Split(new [] {','}, 2);
                        Components.Add(new CustomMapComponent
                        {
                            Name = componentSplit[0].Trim().ToLowerInvariant(),
                            Args = componentSplit[1].Split(',').Select(x => x.Trim().ToLowerInvariant()).ToArray()
                        });
                    }
                    else
                    {
                        Components.Add(new CustomMapComponent
                        {
                            Name = component,
                            Args = null
                        });
                    }
                }

                Tiling = ToVector2(attributes, "til");

                Color = ToColor(attributes, "clr");
                Layer = ToByte(attributes, "l");
                if (Layer.HasValue && Layer.Value > 32) Layer = null;
            }

            public void SetPrefabOffset(Vector3 offset)
            {
                Position += offset;
            }

            private static Vector3? ToVector3(string[] attributes, string attributeName)
            {
                var data = attributes.SingleOrDefault(x => x.StartsWith($"{attributeName}:"))?.Split(':')[1];
                if (data == null) return null;

                var xyz = data.Split(',');
                if (xyz.Length != 3) return null;
                if (float.TryParse(xyz[0], NumberStyles, CultureInfo.InvariantCulture, out var x) 
                    && float.TryParse(xyz[1], NumberStyles, CultureInfo.InvariantCulture, out var y) 
                    && float.TryParse(xyz[2], NumberStyles, CultureInfo.InvariantCulture, out var z))
                {
                    return new Vector3(x, y, z);
                }
                return null;
            }

            private static Color? ToColor(string[] attributes, string attributeName)
            {
                var data = attributes.SingleOrDefault(x => x.StartsWith($"{attributeName}:"))?.Split(':')[1];
                if (data == null) return null;

                var rgba = data.Split(',');
                if (rgba.Length == 3)
                {
                    rgba = new[] { rgba[0], rgba[1], rgba[2], "255" };
                }

                if (rgba.Length != 4) return null;
                if (float.TryParse(rgba[0], NumberStyles, CultureInfo.InvariantCulture, out var r) 
                    && float.TryParse(rgba[1], NumberStyles, CultureInfo.InvariantCulture, out var g) 
                    && float.TryParse(rgba[2], NumberStyles, CultureInfo.InvariantCulture, out var b) 
                    && float.TryParse(rgba[3], NumberStyles, CultureInfo.InvariantCulture, out var a))
                {
                    return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
                }
                return null;
            }

            private static Vector2? ToVector2(string[] attributes, string attributeName)
            {
                var data = attributes.SingleOrDefault(x => x.StartsWith($"{attributeName}:"))?.Split(':')[1];
                if (data == null) return null;

                var xy = data.Split(',');
                if (xy.Length != 2) return null;
                if (float.TryParse(xy[0], NumberStyles, CultureInfo.InvariantCulture, out var x) 
                    && float.TryParse(xy[1], NumberStyles, CultureInfo.InvariantCulture, out var y))
                {
                    return new Vector2(x, y);
                }
                return null;
            }

            private static byte? ToByte(string[] attributes, string attributeName)
            {
                var data = attributes.SingleOrDefault(x => x.StartsWith($"{attributeName}:"))?.Split(':')[1];
                if (data == null) return null;

                if (byte.TryParse(data, out var value))
                {
                    return value;
                }
                return null;
            }

            private static string ToString(string[] attributes, string attributeName)
            {
                return attributes.SingleOrDefault(x => x.StartsWith($"{attributeName}:"))?.Split(':')[1].Trim();
            }
        }

        private struct CustomMapComponent
        {
            public string Name { get; set; }
            public string[] Args { get; set; }
        }
        #endregion

        #region Legacy Map Conversion
        public string ConvertLegacyMap(string legacyMap)
        {
            var objects = legacyMap.ToLowerInvariant().Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.TrimAll()).ToArray();
            HashSet<LegacyObject> legacyObjects = new HashSet<LegacyObject>();
            var disableBounds = objects.Contains("map,disablebounds");
            AddDefaultRCObjects(legacyObjects, disableBounds);
            for (var i = 0; i < objects.Length; i++)
            {
                var attributes = objects[i].Split(',').Select(x => x.ToLowerInvariant().Trim()).ToArray();
                RcObjectType type;
                if (attributes[0] == "custom" || attributes[0] == "customb")
                {
                    type = RcObjectType.Custom;

                    var mapObject = GetMapObject(attributes, type);
                    if (mapObject == null) continue;
                    var texture = GetTexture(attributes, type);
                    var color = GetColor(attributes, type);
                    var material = mapObject.LegacyMaterial?.Name;

                    byte? layer = null;
                    if (attributes[2] == "bombexplosiontex")
                    {
                        material = TransparentDoubleSided.Name;
                        if (color.HasValue) color = new Color(color.Value.r, color.Value.g, color.Value.b, 200f / 255f);
                    }

                    if (attributes[2] == "barriereditormat")
                    {
                        material = Transparent.Name;
                        color = BarrierColor;
                        texture = null;
                    }
                    //TODO: On Bug on the Rose (Hard) these require the GROUND layer
                    //if (attributes[2] == "cannonregionmat")
                    //{
                    //    layer = (byte) Layers.Default;
                    //}

                    var position = GetPosition(attributes, type);
                    var rotation = GetRotation(attributes, type).eulerAngles;
                    var tiling = GetTiling(attributes, type);

                    if (texture != null && texture.LegacyTiling != Vector2.one)
                    {
                        tiling = Vector2.Scale(tiling, texture.LegacyTiling);
                    }

                    var scale = GetScale(attributes, type);

                    legacyObjects.Add(new LegacyObject($"{mapObject.Name}_{i}", position, rotation, mapObject.Name, texture?.Name, tiling, scale, color, null)
                    {
                        Material = material,
                        Layer = layer
                    });
                }
                else if (attributes[0] == "racing")
                {
                    type = RcObjectType.Racing;

                    var modelName = GetMapObject(attributes, type);
                    var material = Transparent.Name;
                    var position = GetPosition(attributes, type);
                    var rotation = GetRotation(attributes, type).eulerAngles;
                    var scale = GetScale(attributes, type);

                    var racingType = GetRacingType(attributes);
                    var color = GetColor(racingType);
                    var component = GetComponent(racingType);
                    legacyObjects.Add(new LegacyObject($"{modelName.Name}_{i}", position, rotation, modelName.Name, null)
                    {
                        Layer = (byte) Layers.Default,
                        Scale = scale,
                        Color = color,
                        Components = component?.ToList(),
                        Material = material
                    });
                }
                else if (attributes[0] == "misc")
                {
                    type = RcObjectType.Misc;
                    var miscType = GetMiscType(attributes);

                    if (miscType == MiscType.Invalid)
                    {
                        Debug.LogWarning($"Custom Map Converter: Misc type {attributes[1]} not mapped");
                        continue;
                    }

                    var modelName = miscType == MiscType.Barrier ? "cuboid" : "";
                    var material = Transparent.Name;
                    var position = GetPosition(attributes, type);
                    var rotation = GetRotation(attributes, type).eulerAngles;
                    var scale = GetScale(attributes, type);
                    var color = new Color(0f, 234f / 255f, 1f, 82f / 255f);
                    var layer = miscType == MiscType.Barrier ? (byte) Layers.MapEditor : (byte?) null;

                    legacyObjects.Add(new LegacyObject($"{modelName}_{i}", position, rotation, modelName, null)
                    {
                        Scale = scale,
                        Color = color,
                        Layer = layer,
                        Components = new List<string> { "Barrier" },
                        Material = material
                    });
                }
                else if (attributes[0] == "spawnpoint")
                {
                    type = RcObjectType.Spawnpoint;
                    var position = GetPosition(attributes, type);
                    var rotation = GetRotation(attributes, type).eulerAngles;
                    var spawnType = GetSpawnType(attributes);
                    var modelName = spawnType == SpawnType.Titan ? SpawnTitan.Name : SpawnHuman.Name;
                    var layer = (byte) Layers.MapEditor;
                    var component = GetComponent(spawnType);
                    legacyObjects.Add(new LegacyObject($"{modelName}_{i}", position, rotation, modelName, null)
                    {
                        Layer = layer,
                        Components = component?.ToList()
                    });
                }
            }

            var data = string.Join(";\n", legacyObjects.Select(x => x.ToString())) + ";";
            return data;
        }

        private void AddDefaultRCObjects(HashSet<LegacyObject> objects, bool disableBounds)
        {
            var cuboid = GetLegacyMapObject("cuboid");
            if (cuboid == null) return;
            objects.Add(new LegacyObject("ground", new Vector3(-6.8f, -32f, 6.2f), Vector3.zero, cuboid.Name, "grass1", new Vector2(15f, 15f), new Vector3(134.1f, 6.4f, 134.1f), null, null));
            if (disableBounds) return;

            objects.Add(CreateBarrier("barrier_1", cuboid.Name, new Vector3(-700f, 745.795f, -1.525f),
                new Vector3(10f, 160f, 160f)));
            objects.Add(CreateBarrier("barrier_2", cuboid.Name, new Vector3(0f, 745.795f, -700f),
                new Vector3(160f, 160f, 10f)));
            objects.Add(CreateBarrier("barrier_3", cuboid.Name, new Vector3(0f, 745.795f, 700f),
                new Vector3(160f, 160f, 10f)));
            objects.Add(CreateBarrier("barrier_4", cuboid.Name, new Vector3(700f, 745.795f, -1.525f),
                new Vector3(10f, 160f, 160f)));
            objects.Add(CreateBarrier("barrier_5", cuboid.Name, new Vector3(-2.22f, 1253.08f, 17.87f),
                new Vector3(160f, 10f, 160f)));
        }

        private LegacyObject CreateBarrier(string identifier, string model, Vector3 position, Vector3 scale)
        {
            return new LegacyObject(identifier, position)
            {
                ModelName = model,
                Scale = scale,
                Layer = (byte) Layers.MapEditor,
                Material = Transparent.Name,
                Color = BarrierColor
            };
        }

        private MapObject GetLegacyMapObject(string legacyName)
        {
            if (legacyName == null) return null;
            return Configuration.MapObjects.SingleOrDefault(x => x.LegacyName == legacyName);
        }

        private MapComponent GetMapComponent(MapComponentType type) =>
            Configuration.MapComponents.SingleOrDefault(x => x.Type == type);

        private MapObject GetMapObject(string[] attributes, RcObjectType type)
        {
            var modelName = type switch
            {
                RcObjectType.Custom => attributes[1],
                RcObjectType.Racing => attributes[1].ToLowerInvariant()
                    .Replace("kill", string.Empty)
                    .Replace("checkpoint", string.Empty)
                    .Replace("start", string.Empty)
                    .Replace("end", string.Empty),
                _ => null
            };

            if (modelName == null)
            {
                Debug.LogWarning($"Custom Map Converter: ModelName is not supported for {type}");
            }

            var mapObject = Configuration.MapObjects.SingleOrDefault(x => x.LegacyName == modelName);
            if (mapObject != null) return mapObject;

            Debug.LogWarning($"Custom Map Converter: Model {modelName} could not be found");
            return null;
        }

        private MiscType GetMiscType(string[] attributes)
        {
            var miscType = attributes[1];
            if (miscType == "barrier")
            {
                return MiscType.Barrier;
            }

            return MiscType.Invalid;
        }

        #region Racing Objects
        private RacingType GetRacingType(string[] attributes)
        {
            var racingType = attributes[1];
            if (racingType.Contains("kill"))
            {
                return RacingType.Kill;
            }

            if (racingType.Contains("checkpoint"))
            {
                return RacingType.Checkpoint;
            }

            if (racingType.Contains("start"))
            {
                return RacingType.Start;
            }

            if (racingType.Contains("end"))
            {
                return RacingType.Finish;
            }

            Debug.LogWarning($"Custom Map Converter: Unable to determine racing type of {racingType}");
            return RacingType.Invalid;
        }

        private Color? GetColor(RacingType type)
        {
            var alpha = 175f / 255f;
            return type switch
            {
                RacingType.Kill => new Color(1f, 0f, 0f, alpha),
                RacingType.Checkpoint => new Color(0f, 1f, 0f, alpha),
                RacingType.Start => new Color(0f, 0f, 1f, alpha),
                RacingType.Finish => new Color(1f, 1f, 0f, alpha),
                _ => null
            };
        }

        private string[] GetComponent(RacingType type)
        {
            var trigger = MapComponentType.Trigger.ToString();
            return type switch
            {
                RacingType.Kill => new[] { trigger, MapComponentType.Killzone.ToString() },
                RacingType.Start => new[] { MapComponentType.Start.ToString() },
                RacingType.Finish => new[] { trigger, MapComponentType.Finish.ToString() },
                RacingType.Checkpoint => new[] { trigger, MapComponentType.Checkpoint.ToString() },
                _ => null
            };
        }
        #endregion

        #region Spawners

        private static SpawnType GetSpawnType(string[] attributes)
        {
            var spawnType = attributes[1];
            return spawnType switch
            {
                "titan" => SpawnType.Titan,
                "playerc" => SpawnType.PlayerCyan,
                "playerm" => SpawnType.PlayerMagenta,
                _ => SpawnType.Invalid
            };
        }

        private string[] GetComponent(SpawnType type)
        {
            var spawner = GetMapComponent(MapComponentType.Spawner)?.Name;
            return type switch
            {
                SpawnType.Titan => new[] { $"{spawner}, f:2, t:2" },
                SpawnType.PlayerCyan => new[] { $"{spawner}, f:1, t:1" },
                SpawnType.PlayerMagenta => new[] { $"{spawner}, f:2, t:1" },
                _ => null
            };
        }

        #endregion


        private MapTexture GetTexture(string[] attributes, RcObjectType type)
        {
            var texture = type switch
            {
                RcObjectType.Custom => attributes[2],
                _ => null,
            };

            if (texture == null)
            {
                Debug.LogWarning($"Custom Map Converter: Texture is not supported for {type}");
            }

            var mapTexture = Configuration.MapTextures.SingleOrDefault(x => x.LegacyName == texture);
            if (mapTexture != null) return mapTexture;
            if (texture == "default" || texture == "barriereditormat" || texture == "bombexplosiontex") return null;

            Debug.LogWarning($"Custom Map Converter: Texture {texture} could not be found");
            return null;
        }

        private static Vector3 GetPosition(string[] mapData, RcObjectType type)
        {
            var index = GetPositionIndex(type);
            return new Vector3(Convert.ToSingle(mapData[index[0]], CultureInfo.InvariantCulture),
                Convert.ToSingle(mapData[index[1]], CultureInfo.InvariantCulture), Convert.ToSingle(mapData[index[2]], CultureInfo.InvariantCulture));
        }

        private static Vector3 GetScale(string[] mapData, RcObjectType type)
        {
            var tilingMap = type switch
            {
                RcObjectType.Custom => new[] { 3, 4, 5 },
                RcObjectType.Racing => new[] { 2, 3, 4 },
                RcObjectType.Misc => new[] { 2, 3, 4 },
                _ => new int[0]
            };
            if (tilingMap.Length == 0)
            {
                Debug.LogWarning($"Custom Map: Tiling not supported for {type}");
                return Vector3.one;
            }

            if (float.TryParse(mapData[tilingMap[0]], NumberStyles, CultureInfo.InvariantCulture, out var x) 
                && float.TryParse(mapData[tilingMap[1]], NumberStyles, CultureInfo.InvariantCulture, out var y) 
                && float.TryParse(mapData[tilingMap[2]], NumberStyles, CultureInfo.InvariantCulture, out var z))
            {
                return new Vector3(x, y, z);
            }

            Debug.LogWarning($"Custom Map: Could not convert scale x:{mapData[tilingMap[0]]}, y:{mapData[tilingMap[1]]}, z:{mapData[tilingMap[2]]}");
            return Vector3.one;
        }

        private static Quaternion GetRotation(string[] mapData, RcObjectType type)
        {
            var index = GetRotationIndex(type);
            return new Quaternion(Convert.ToSingle(mapData[index[0]], CultureInfo.InvariantCulture), Convert.ToSingle(mapData[index[1]], CultureInfo.InvariantCulture),
                Convert.ToSingle(mapData[index[2]], CultureInfo.InvariantCulture), Convert.ToSingle(mapData[index[3]], CultureInfo.InvariantCulture));
        }

        private static Vector2 GetTiling(string[] mapData, RcObjectType type)
        {
            var tilingMap = type switch
            {
                RcObjectType.Custom => new[] { 10, 11 },
                _ => new int[0]
            };
            if (tilingMap.Length == 0)
            {
                Debug.LogWarning($"Custom Map Converter: Tiling not supported for {type}");
                return Vector2.one;
            }

            return new Vector2(Convert.ToSingle(mapData[tilingMap[0]], CultureInfo.InvariantCulture), Convert.ToSingle(mapData[tilingMap[1]], CultureInfo.InvariantCulture));
        }

        private static Color? GetColor(string[] mapData, RcObjectType type)
        {
            var colorMap = type switch
            {
                RcObjectType.Custom => new[] { 6, 7, 8, 9 },
                _ => new int[0]
            };
            if (colorMap.Length == 0)
            {
                Debug.LogWarning($"Custom Map Converter: Color not supported for {type}");
                return null;
            }

            if (int.TryParse(mapData[colorMap[0]], NumberStyles.Integer, CultureInfo.InvariantCulture, out var isEnabled) && isEnabled != 0)
            {
                if (float.TryParse(mapData[colorMap[1]], NumberStyles, CultureInfo.InvariantCulture, out var r) 
                    && float.TryParse(mapData[colorMap[2]], NumberStyles, CultureInfo.InvariantCulture, out var g) 
                    && float.TryParse(mapData[colorMap[3]], NumberStyles, CultureInfo.InvariantCulture, out var b))
                {
                    return new Color(r, g, b);
                }
            }

            return null;
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

        private enum RacingType
        {
            Invalid,
            Kill,
            Start,
            Checkpoint,
            Finish
        }

        private enum MiscType
        {
            Invalid,
            Barrier,
            Cuboid
        }

        private enum SpawnType
        {
            Invalid,
            Titan,
            PlayerCyan,
            PlayerMagenta
        }

        private struct LegacyObject
        {
            private string Identifier { get; set; }
            private Vector3 Position { get; set; }
            private Vector3 Rotation { get; set; }


            public string ModelName { get; set; }
            private string Texture { get; set; }
            public string Material { get; set; }
            public Vector3 Scale { get; set; }
            private Vector2 Tiling { get; set; }
            public Color? Color { get; set; }
            public List<string> Components { get; set; }
            public byte? Layer { get; set; }

            public LegacyObject(string identifier, Vector3 position)
            {
                Identifier = identifier;
                Position = position;
                Rotation = Vector3.zero;
                ModelName = null;
                Texture = null;
                Material = null;
                Scale = Vector3.one;
                Tiling = Vector2.one;
                Color = null;
                Components = null;
                Layer = null;
            }

            public LegacyObject(string identifier, Vector3 position, Vector3 rotation, string modelName, string texture) : this(identifier, position)
            {
                Rotation = rotation;
                ModelName = modelName;
                Texture = texture;
            }

            public LegacyObject(string identifier, Vector3 position, Vector3 rotation, string modelName, string texture, Vector2 tiling, Vector3 scale, Color? color, List<string> components) : this(identifier, position)
            {
                Rotation = rotation;
                ModelName = modelName;
                Texture = texture;
                Tiling = tiling;
                Scale = scale;
                Color = color;
                Components = components;
            }

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder(FormattableString.Invariant($"i:{Identifier};pos:{Position.x},{Position.y},{Position.z};"));

                if (Rotation != Vector3.zero)
                {
                    builder.Append(FormattableString.Invariant($"rot:{Rotation.x},{Rotation.y},{Rotation.z};"));
                }

                if (ModelName != null)
                {
                    builder.Append($"m:{ModelName};");
                    if (Scale != Vector3.one) builder.Append(FormattableString.Invariant($"scl:{Scale.x},{Scale.y},{Scale.z};"));

                    if (Texture != null)
                    {
                        builder.Append($"t:{Texture};");
                        if (Tiling != Vector2.one) builder.Append(FormattableString.Invariant($"til:{Tiling.x},{Tiling.y};"));
                    }

                    if (Material != null)
                    {
                        builder.Append($"mat:{Material};");
                    }

                    if (Color.HasValue) builder.Append(FormattableString.Invariant($"clr:{(int) (Color.Value.r * 255f)},{(int) (Color.Value.g * 255f)},{(int) (Color.Value.b * 255f)},{(int) (Color.Value.a * 255f)};"));
                    if (Layer.HasValue) builder.Append(FormattableString.Invariant($"l:{Layer.Value};"));
                }

                if (Components != null)
                {
                    foreach (var component in Components)
                    {
                        builder.Append($"c:{component};".ToLowerInvariant());
                    }
                }

                return builder.ToString();
            }
        }
#endregion
    }
}
