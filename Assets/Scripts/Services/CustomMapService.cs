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
                if (GetMapTexture(mapItem.Texture, out var texture))
                {
                    foreach (var renderer in mapGameObject.GetComponentsInChildren<Renderer>())
                    {
                        renderer.material = texture.Material;
                        if (mapItem.Tiling.HasValue)
                        {
                            renderer.material.mainTextureScale = new Vector2(
                                renderer.material.mainTextureScale.x * mapItem.Tiling.Value.x,
                                renderer.material.mainTextureScale.y * mapItem.Tiling.Value.y);
                        }
                    }
                }

                if (mapItem.Color.HasValue)
                {
                    foreach (MeshFilter filter in mapGameObject.GetComponentsInChildren<MeshFilter>())
                    {
                        var mesh = filter.mesh;
                        var colorArray = new Color[mesh.vertexCount];
                        var targetColor = mapItem.Color.Value;
                        var num8 = 0;
                        while (num8 < mesh.vertexCount)
                        {
                            colorArray[num8] = targetColor;
                            num8++;
                        }

                        mesh.colors = colorArray;
                    }
                }

            }
        }

        private MapObject GetMapObject(string objectName)
        {
            if (objectName == null) return null;
            return Configuration.MapObjects.SingleOrDefault(x => x.Name == objectName);
        }

        private bool GetMapTexture(string textureName, out MapTexture texture)
        {
            texture = null;
            if (textureName == null) return false;
            texture = Configuration.MapTextures.SingleOrDefault(x => x.Name == textureName);
            return texture != null;
        }

        private struct CustomMapObject
        {
            public string Identifier { get; }
            public Vector3 Position { get; }


            public string ModelName { get; }
            public string Texture { get; }
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
                Texture = attributes.SingleOrDefault(x => x.StartsWith("t:"))?.Split(':')[1];
                Tiling = ToVector2(attributes, "til");

                Color = null;
                var color = ToVector3(attributes, "clr");
                if (color.HasValue) { Color = new Color(color.Value.x / 255f, color.Value.y / 255f, color.Value.z / 255f, 1f); }
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
