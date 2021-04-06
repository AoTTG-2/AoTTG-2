using Assets.Scripts.CustomMaps;
using Assets.Scripts.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Services
{
    public class CustomMapService : MonoBehaviour, ICustomMapService
    {
        public CustomMapConfiguration Configuration;

        public List<CustomMap> GetCustomMaps()
        {
            var path = $"{Application.streamingAssetsPath}{Path.AltDirectorySeparatorChar}Custom Maps";
            var files = Directory.GetFiles(path, "*.txt");
            var customMaps = files.Select(x => new CustomMap(Path.GetFileName(x), x)).ToList();
            return customMaps;
        }

        private CustomMap CurrentMap { get; set; }

        private void OnLevelWasLoaded(int level)
        {
            if (SceneManager.GetSceneByBuildIndex(level).name == "Custom")
            {
                var content = File.ReadAllText(CurrentMap.Path);
                var objects = content.Split(new[] { ";;\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
                LoadCustomMap(objects);
                FengGameManagerMKII.Level.LevelIsLoaded();
            }
        }

        public void Load(string mapName)
        {
            CurrentMap = GetCustomMaps().SingleOrDefault(x => x.Name == mapName);
            PhotonNetwork.LoadLevel("Custom");
        }

        #region Custom Map Loading
        public void LoadCustomMap(string[] objects)
        {
            var baseParent = new GameObject("Custom Map");

            foreach (var item in objects)
            {
                var attributes = item.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
                var mapItem = new CustomMapObject(attributes);
                var mapObject = GetMapObject(mapItem.ModelName);
                if (mapObject == null)
                {
                    Debug.LogWarning($"Custom Map: MODEL OBJECT {mapItem.ModelName} DOES NOT EXIST");
                    continue;
                }

                var mapGameObject = Instantiate(mapObject.Prefab, mapItem.Position, Quaternion.Euler(mapItem.Rotation));
                mapGameObject.name = mapItem.Identifier;
                mapGameObject.transform.localScale = Vector3.Scale(mapGameObject.transform.localScale, mapItem.Scale);
                mapGameObject.transform.parent = baseParent.transform; //TODO: Objects which do have parents, should not use this

                var material = GetMapMaterial(mapItem.Material);
                var texture = GetMapTexture(mapItem.Texture);
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
                    }
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

        private struct CustomMapObject
        {
            public string Identifier { get; }
            public Vector3 Position { get; }


            public string ModelName { get; }
            public string Texture { get; }
            public string Material { get; }
            public Vector3 Rotation { get; }
            public Vector3 Scale { get; }
            public Vector2? Tiling { get; }
            public Color? Color { get; }

            public CustomMapObject(string[] attributes)
            {
                var id = attributes.SingleOrDefault(x => x.Contains("i:"))?.Split(':')[1];
                Identifier = id;

                Position = ToVector3(attributes, "pos") ?? Vector3.zero;
                Rotation = ToVector3(attributes, "rot") ?? Vector3.zero;
                Scale = ToVector3(attributes, "scl") ?? Vector3.one;

                ModelName = attributes.SingleOrDefault(x => x.StartsWith("m:"))?.Split(':')[1];
                Material = attributes.SingleOrDefault(x => x.StartsWith("mat:"))?.Split(':')[1];
                Texture = attributes.SingleOrDefault(x => x.StartsWith("t:"))?.Split(':')[1];
                Tiling = ToVector2(attributes, "til");

                Color = ToColor(attributes, "clr");
            }

            private static Vector3? ToVector3(string[] attributes, string attributeName)
            {
                var data = attributes.SingleOrDefault(x => x.Contains($"{attributeName}:"))?.Split(':')[1];
                if (data == null) return null;

                var xyz = data.Split(',');
                if (xyz.Length != 3) return null;
                if (float.TryParse(xyz[0], out var x) && float.TryParse(xyz[1], out var y) && float.TryParse(xyz[2], out var z))
                {
                    return new Vector3(x, y, z);
                }
                return null;
            }

            private static Color? ToColor(string[] attributes, string attributeName)
            {
                var data = attributes.SingleOrDefault(x => x.Contains($"{attributeName}:"))?.Split(':')[1];
                if (data == null) return null;

                var rgba = data.Split(',');
                if (rgba.Length == 3)
                {
                    rgba = new[] { rgba[0], rgba[1], rgba[2], "255" };
                }

                if (rgba.Length != 4) return null;
                if (float.TryParse(rgba[0], out var r) && float.TryParse(rgba[1], out var g) && float.TryParse(rgba[2], out var b) && float.TryParse(rgba[3], out var a))
                {
                    return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
                }
                return null;
            }

            private static Vector2? ToVector2(string[] attributes, string attributeName)
            {
                var data = attributes.SingleOrDefault(x => x.Contains($"{attributeName}:"))?.Split(':')[1];
                if (data == null) return null;

                var xy = data.Split(',');
                if (xy.Length != 2) return null;
                if (float.TryParse(xy[0], out var x) && float.TryParse(xy[1], out var y))
                {
                    return new Vector2(x, y);
                }
                return null;
            }
        }
        #endregion

    }
}
