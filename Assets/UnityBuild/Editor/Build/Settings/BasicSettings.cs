using UnityEngine;

namespace SuperSystems.UnityBuild
{

[System.Serializable]
public class BasicSettings
{
    [FilePath(true, true, "Choose location for build output")]
    public string baseBuildFolder = "bin";
    public bool openFolderPostBuild = true;
}

}