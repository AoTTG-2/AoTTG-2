using Assets.Scripts.Legacy.CustomMap;
using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CustomMaps
{
    public class Converter : MonoBehaviour
    {
        public TextAsset AoTTGCustomMap;
        public TextAsset AoTTG2CustomMap;

        public CustomMapConfiguration Configuration;
        public MapMaterial Transparent;

        public bool LoadAoTTG2Map;

        public CustomMapService Service;

        #region Legacy
        public void Awake()
        {
            var objects = AoTTGCustomMap.text.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            HashSet<LegacyObject> legacyObjects = new HashSet<LegacyObject>();
            AddDefaultRCObjects(legacyObjects);
            for (var i = 0; i < objects.Length; i++)
            {
                var attributes = objects[i].Split(',');
                RcObjectType type;
                if (attributes[0] == "custom")
                {
                    type = RcObjectType.Custom;

                    var modelName = GetModelName(attributes, type);
                    if (modelName == null) continue;
                    var texture = GetTexture(attributes, type);
                    var position = GetPosition(attributes, type);
                    var rotation = GetRotation(attributes, type).eulerAngles;
                    var tiling = GetTiling(attributes, type);

                    if (texture != null && texture.LegacyTiling != Vector2.one)
                    {
                        tiling = Vector2.Scale(tiling, texture.LegacyTiling);
                    }

                    var scale = GetScale(attributes, type);
                    var color = GetColor(attributes, type);
                    legacyObjects.Add(new LegacyObject($"{modelName}_{i}", position, rotation, modelName, texture?.Name, tiling, scale, color, null));
                } else if (attributes[0] == "racing")
                {
                    type = RcObjectType.Racing;

                    var modelName = GetModelName(attributes, type);
                    var material = Transparent.Name;
                    var position = GetPosition(attributes, type);
                    var rotation = GetRotation(attributes, type).eulerAngles;
                    var scale = GetScale(attributes, type);

                    var racingType = GetRacingType(attributes);
                    var color = GetColor(racingType);
                    legacyObjects.Add(new LegacyObject($"{modelName}_{i}", position, rotation, modelName, null)
                    {
                        Scale = scale,
                        Color = color,
                        Components = new List<string> { "Kill" },
                        Material = material
                    });
                } else if (attributes[0] == "misc")
                {
                    type = RcObjectType.Misc;

                    var modelName = attributes[1] == "barrier" ? "cuboid" : "";

                    if (modelName == "")
                    {
                        Debug.LogWarning($"Custom Map Converter: Misc type {attributes[1]} not mapped");
                        continue;
                    }

                    var material = Transparent.Name;
                    var position = GetPosition(attributes, type);
                    var rotation = GetRotation(attributes, type).eulerAngles;
                    var scale = GetScale(attributes, type);
                    var color = new Color(0f, 234f / 255f, 1f, 82f / 255f);
                    legacyObjects.Add(new LegacyObject($"{modelName}_{i}", position, rotation, modelName, null)
                    {
                        Scale = scale,
                        Color = color,
                        Components = new List<string> { "Barrier" },
                        Material = material
                    });
                }
            }

            var data = string.Join(";\n", legacyObjects.Select(x => x.ToString())) + ";";
            File.WriteAllText(AssetDatabase.GetAssetPath(AoTTG2CustomMap), data);
            EditorUtility.SetDirty(AoTTG2CustomMap);

            if (LoadAoTTG2Map)
            {
                //objects = AoTTG2CustomMap.text.Split(new[] { ";;\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
                objects = data.Split(new[] { ";;\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
                Service.LoadCustomMap(objects);
            }
        }

        private void AddDefaultRCObjects(HashSet<LegacyObject> objects)
        {
            var ground = GetLegacyMapObject("cuboid");
            if (ground != null)
            {
                objects.Add(new LegacyObject("ground", new Vector3(-6.8f, -32f, 6.2f), Vector3.zero, ground.Name, "grass", new Vector2(15f, 15f),  new Vector3(134.1f, 6.4f, 134.1f), null, null));
            }
        }

        private MapObject GetLegacyMapObject(string legacyName)
        {
            if (legacyName == null) return null;
            return Configuration.MapObjects.SingleOrDefault(x => x.LegacyName == legacyName);
        }

        private string GetModelName(string[] attributes, RcObjectType type)
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

            if (Configuration.MapObjects.Any(x => x.LegacyName == modelName)) return modelName;

            Debug.LogWarning($"Custom Map Converter: Model {modelName} could not be found");
            return null;
        }

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

        private string GetComponent(RacingType type)
        {
            if (type == RacingType.Kill)
            {
                return "killzone";
            }

            return null;
        }

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
            if (texture == "default") return null;

            Debug.LogWarning($"Custom Map Converter: Texture {texture} could not be found");
            return null;
        }

        private static Vector3 GetPosition(string[] mapData, RcObjectType type)
        {
            var index = GetPositionIndex(type);
            return new Vector3(Convert.ToSingle(mapData[index[0]]), Convert.ToSingle(mapData[index[1]]), Convert.ToSingle(mapData[index[2]]));
        }

        private static Vector3 GetScale(string[] mapData, RcObjectType type)
        {
            var tilingMap = type switch
            {
                RcObjectType.Custom => new[] { 3, 4, 5 },
                RcObjectType.Racing => new[] {2, 3, 4},
                RcObjectType.Misc => new[] { 2, 3, 4 },
                _ => new int[0]
            };
            if (tilingMap.Length == 0)
            {
                Debug.LogWarning($"Custom Map: Tiling not supported for {type}");
                return Vector3.one;
            }

            if (float.TryParse(mapData[tilingMap[0]], out var x) && float.TryParse(mapData[tilingMap[1]], out var y) && float.TryParse(mapData[tilingMap[2]], out var z))
            {
                return new Vector3(x, y, z);
            }

            Debug.LogWarning($"Custom Map: Could not convert scale x:{mapData[tilingMap[0]]}, y:{mapData[tilingMap[1]]}, z:{mapData[tilingMap[2]]}");
            return Vector3.one;
        }

        private static Quaternion GetRotation(string[] mapData, RcObjectType type)
        {
            var index = GetRotationIndex(type);
            return new Quaternion(Convert.ToSingle(mapData[index[0]]), Convert.ToSingle(mapData[index[1]]),
                Convert.ToSingle(mapData[index[2]]), Convert.ToSingle(mapData[index[3]]));
        }

        private static Vector2 GetTiling(string[] mapData, RcObjectType type)
        {
            var tilingMap = type switch
            {
                RcObjectType.Custom => new[] {10, 11},
                _ => new int[0]
            };
            if (tilingMap.Length == 0)
            {
                Debug.LogWarning($"Custom Map Converter: Tiling not supported for {type}");
                return Vector2.one;
            }

            return new Vector2(Convert.ToSingle(mapData[tilingMap[0]]), Convert.ToSingle(mapData[tilingMap[1]]));
        }

        private static Color? GetColor(string[] mapData, RcObjectType type)
        {
            var colorMap = type switch
            {
                RcObjectType.Custom => new[] {6, 7, 8, 9},
                _ => new int[0]
            };
            if (colorMap.Length == 0)
            {
                Debug.LogWarning($"Custom Map Converter: Color not supported for {type}");
                return null;
            }

            if (int.TryParse(mapData[colorMap[0]], out var isEnabled) && isEnabled != 0)
            {
                if (float.TryParse(mapData[colorMap[1]], out var r) && float.TryParse(mapData[colorMap[2]], out var g) && float.TryParse(mapData[colorMap[3]], out var b))
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
        #endregion

        private enum RacingType
        {
            Invalid,
            Kill,
            Start,
            Checkpoint,
            Finish
        }

        private struct LegacyObject
        {
            private string Identifier { get; set; }
            private Vector3 Position { get; set; }
            private Vector3 Rotation { get; set; }


            private string ModelName { get; set; }
            private string Texture { get; set; }
            public string Material { get; set; }
            public Vector3 Scale { get; set; }
            private Vector2 Tiling { get; set; }
            public Color? Color { get; set; }
            public List<string> Components { get; set; }

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
                StringBuilder builder = new StringBuilder($"i:{Identifier};pos:{Position.x},{Position.y},{Position.z};");

                if (Rotation != Vector3.zero)
                {
                    builder.Append($"rot:{Rotation.x},{Rotation.y},{Rotation.z};");
                }

                if (ModelName != null)
                {
                    builder.Append($"m:{ModelName};");
                    if (Scale != Vector3.one) builder.Append($"scl:{Scale.x},{Scale.y},{Scale.z};");

                    if (Texture != null)
                    {
                        builder.Append($"t:{Texture};");
                        if (Tiling != Vector2.one) builder.Append($"til:{Tiling.x},{Tiling.y};");
                    }

                    if (Material != null)
                    {
                        builder.Append($"mat:{Material};");
                    }

                    if (Color.HasValue) builder.Append($"clr:{(int) (Color.Value.r * 255f)},{(int) (Color.Value.g * 255f)},{(int) (Color.Value.b * 255f)},{(int) (Color.Value.a * 255f)};");
                }

                return builder.ToString();
            }
        }
    }
}
