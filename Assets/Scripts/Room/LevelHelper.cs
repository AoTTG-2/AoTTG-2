using System.IO;
using System.Linq;
using Assets.Scripts.CustomMaps;
using UnityEngine;

namespace Assets.Scripts.Room
{
    public static class LevelHelper
    {

        private static AssetBundle CurrentAssetBundle { get; set; }

        public static string[] GetAll()
        {
            Directory.CreateDirectory(Path.Combine(Application.dataPath, "Custom Maps"));
            var files = Directory.GetFiles(Path.Combine(Application.dataPath, "Custom Maps")).Where(x => x.EndsWith(".unity3d")).ToArray();
            files = files.Select(x => x.Split(Path.DirectorySeparatorChar).Last().Replace(".unity3d", "")).ToArray();
            return files;
        }

        public static void Load(Level level)
        {
            if (CurrentAssetBundle != null && SceneManagerHelper.ActiveSceneName != level.SceneName)
            {
                CurrentAssetBundle.Unload(true);
            }

            if (level.IsCustom)
            {
                if (level.Type == CustomMapType.CustomMap)
                {

                }
                else
                {
                    CurrentAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, Path.Combine("Custom Maps", level.AssetBundle + ".unity3d")));
                    level.SceneName = CurrentAssetBundle.GetAllScenePaths().FirstOrDefault();
                }
                
            }

            PhotonNetwork.LoadLevel(level.SceneName);
        }
    }
}
