using System.IO;
using UnityEditor;

namespace Assets.Editor
{
    public class SceneToAssetBundle
    {
        [MenuItem("Tools/Build Custom Maps")]
        static void BuildAllAssetBundles()
        {
            string assetBundleDirectory = "Assets/Custom Maps";
            if (!Directory.Exists(assetBundleDirectory))
            {
                Directory.CreateDirectory(assetBundleDirectory);
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectory,
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
        }
    }
}
