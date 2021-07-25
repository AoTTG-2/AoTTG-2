using Assets.Scripts.Services;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CustomMaps
{
    /// <summary>
    /// Used to test the map converter functionality in an empty scene
    /// </summary>
    public class Converter : MonoBehaviour
    {
        public TextAsset AoTTGCustomMap;
        public TextAsset AoTTG2CustomMap;

        public bool ConvertLegacyMap;
        public bool LoadAoTTG2Map;
        public bool IncludeMapBank;

        public CustomMapService Service;
        
        public void Awake()
        {
            if (IncludeMapBank)
            {
                var path = $"{Application.streamingAssetsPath}{Path.AltDirectorySeparatorChar}Legacy Map Bank";
                var files = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories);
                var mapBankOutputPath =
                    $"{Application.streamingAssetsPath}{Path.AltDirectorySeparatorChar}Custom Maps{Path.AltDirectorySeparatorChar}Map Bank";
                var di = new DirectoryInfo(mapBankOutputPath);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

                Directory.CreateDirectory(mapBankOutputPath);
                foreach (var file in files)
                {
                    try
                    {
                        var output = Service.ConvertLegacyMap(File.ReadAllText(file));
                        File.WriteAllText(Path.Combine(mapBankOutputPath, Path.GetFileName(file)), output);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Custom Map Converter: Exception in: {file}");
                        Debug.LogError(e);
                    }

                }

                return;
            }

#if UNITY_EDITOR
            if (ConvertLegacyMap)
            {
                var data = Service.ConvertLegacyMap(AoTTGCustomMap.text);
                File.WriteAllText(AssetDatabase.GetAssetPath(AoTTG2CustomMap), data);
                EditorUtility.SetDirty(AoTTG2CustomMap);

                if (LoadAoTTG2Map)
                {
                    var objects = data.Split(new[] { ";;\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
                    Service.LoadCustomMap(objects);
                }
            }
#endif
            
            if (LoadAoTTG2Map && !ConvertLegacyMap)
            {
                var objects = AoTTG2CustomMap.text.Split(new[] { ";;\n" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
                Service.LoadCustomMap(objects);
            }
        }
    }
}
