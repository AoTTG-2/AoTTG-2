using System.IO;
using UnityEditor;

namespace Assets.Editor
{
    public class SceneToAssetBundle
    {
        [MenuItem("Tools/Build Custom Maps")]
        static void BuildAllAssetBundles()
        {
            string assetBundleDirectoryWindows = "Assets/Custom Maps";
            if (!Directory.Exists(assetBundleDirectoryWindows))
            {
                Directory.CreateDirectory(assetBundleDirectoryWindows);
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectoryWindows,
                BuildAssetBundleOptions.AppendHashToAssetBundleName,
                BuildTarget.StandaloneWindows64);
        }

        [MenuItem("Tools/Build Custom Maps - Release")]
        static void BuildAllAssetBundlesMac()
        {
            string assetBundleDirectoryMac = "Assets/Custom Maps/Mac";
            if (!Directory.Exists(assetBundleDirectoryMac))
            {
                Directory.CreateDirectory(assetBundleDirectoryMac);
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectoryMac,
                BuildAssetBundleOptions.AppendHashToAssetBundleName,
                BuildTarget.StandaloneOSX);

            string assetBundleDirectoryWindows = "Assets/Custom Maps/Windows";
            if (!Directory.Exists(assetBundleDirectoryWindows))
            {
                Directory.CreateDirectory(assetBundleDirectoryWindows);
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectoryWindows,
                BuildAssetBundleOptions.AppendHashToAssetBundleName,
                BuildTarget.StandaloneWindows64);
        }

        [MenuItem("Tools/Name AssetBundles")]
        static void NameFileExtensionBundles()
        {

            string[] files = Directory.GetFiles("Assets/Custom Maps");

            foreach (var a in files)
            {
                if (!a.Contains("."))
                {
                    File.Move(a, a + ".unity3d");
                }
            }

            files = Directory.GetFiles("Assets/Custom Maps/Windows");
            foreach (var a in files)
            {
                if (!a.Contains("."))
                {
                    File.Move(a, a + ".unity3d");
                }
            }

            files = Directory.GetFiles("Assets/Custom Maps/Mac");

            foreach (var a in files)
            {
                if (!a.Contains("."))
                {
                    File.Move(a, a + ".unity3d");
                }
            }
        }
    }
}
