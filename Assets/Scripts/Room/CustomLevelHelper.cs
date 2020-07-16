using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Gamemode.Settings;
using UnityEngine;

namespace Assets.Scripts.Room
{
    public static class CustomLevelHelper
    {
        public static string[] GetAll()
        {
            Directory.CreateDirectory(Path.Combine(Application.dataPath, "Custom Maps"));
            var files = Directory.GetFiles(Path.Combine(Application.dataPath, "Custom Maps")).Where(x => x.EndsWith(".unity3d")).ToArray();
            files = files.Select(x => x.Split('\\').Last().Replace(".unity3d", "")).ToArray();
            return files;
        }

        public static Level Load(string assetBundleName)
        {
            AssetBundle.UnloadAllAssetBundles(true);
            var sceneName = assetBundleName.Split('_')[0];
            var assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, Path.Combine("Custom Maps", assetBundleName + ".unity3d")));
            var scenes = assetBundle.GetAllScenePaths().FirstOrDefault();
            //TODO: How should we check which Gamemodes a Scene supports?
            return new Level
            {
                Name = sceneName,
                SceneName = scenes,
                Gamemodes = new List<GamemodeSettings>
                {
                    new RacingSettings(),
                    new KillTitansSettings(),
                    new WaveGamemodeSettings(),
                    new InfectionGamemodeSettings(),
                    new CaptureGamemodeSettings(),
                    new RushSettings(),
                    new EndlessSettings(),
                    new PvPAhssSettings()
                }
            };
        }
    }
}
