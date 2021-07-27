using Assets.Scripts.Gamemode.Racing;
using Assets.Scripts.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Extensions;
using UnityEngine;
using UnityEngine.Video;

namespace Assets.Scripts.CustomMaps
{
    /// <summary>
    /// A MapComponent is the equivalent of Unity's Components which can be attached to a GameObject, but then used for Custom Maps
    /// </summary>
    [CreateAssetMenu(fileName = "Map Component", menuName = "Custom Map/Component")]
    public class MapComponent : ScriptableObject
    {
        /// <summary>
        /// The name of the MapMaterial that is used within the Custom Map File. Changing this can cause compatibility issues, so a new migration tool needs to be created.
        /// </summary>
        public string Name;
        public string Description;
        public MapComponentType Type;

        /// <summary>
        /// A list of compatible MapObjects for this component. Leave empty if all are supported
        /// </summary>
        public List<MapObject> CompatibleMapObjects;
        /// <summary>
        /// A list of compatible MapMaterials for this component. Leave empty if all are supported
        /// </summary>
        public List<MapMaterial> CompatibleMapMaterials;

        public void AddComponent(GameObject gameObject, MapObject mapObject, MapMaterial mapMaterial, string[] args)
        {
            if (CompatibleMapObjects.Any())
            {
                if (!CompatibleMapObjects.Contains(mapObject))
                {
                    Debug.LogError($"Custom Map Loader: MapObject {mapObject?.Name} is not supported for Component {Type}");
                    return;
                }
            }

            if (CompatibleMapMaterials.Any())
            {
                if (!CompatibleMapMaterials.Contains(mapMaterial))
                {
                    Debug.LogError($"Custom Map Loader: MapMaterial {mapMaterial?.Name} is not supported for Component {Type}");
                    return;
                }
            }

            switch (Type)
            {
                case MapComponentType.Video:
                    var player = gameObject.AddComponent<VideoPlayer>();
                    player.url =
                        "https://r4---sn-5hneknee.googlevideo.com/videoplayback?expire=1617993745&ei=sUtwYNrGGK6ox_APj421iAE&ip=27.2.34.87&id=o-ALOTqZpqMsmu7rcDinwPNiy8eOu4izvpo6WQk4tyqTEA&itag=18&source=youtube&requiressl=yes&vprv=1&mime=video%2Fmp4&ns=-756KcRNiUPPdl7wAxQnzCsF&gir=yes&clen=20105348&ratebypass=yes&dur=307.525&lmt=1587740345009032&fvip=4&fexp=24001373,24007246&c=WEB&txp=5531432&n=kQwrL3r58kfpCIPv&sparams=expire%2Cei%2Cip%2Cid%2Citag%2Csource%2Crequiressl%2Cvprv%2Cmime%2Cns%2Cgir%2Cclen%2Cratebypass%2Cdur%2Clmt&sig=AOq0QJ8wRgIhAIRWnygD2sRwlhF4C3hPirX-JoZqipEf8kd7oYWeOxJgAiEArVmO3VFdjLx9ro8mVrNngnC2M1l99pABeyzryGoRFi0%3D&rm=sn-n5pbvoj5caxu8-nbos7z,sn-42u-nboez7z,sn-i3b6y7z&req_id=458e31d02230a3ee&redirect_counter=3&cms_redirect=yes&ipbypass=yes&mh=q9&mip=80.114.12.144&mm=30&mn=sn-5hneknee&ms=nxu&mt=1617971102&mv=u&mvi=4&pl=18&lsparams=ipbypass,mh,mip,mm,mn,ms,mv,mvi,pl&lsig=AG3C_xAwRAIgXUFSzVLZv_EUGuJsRh_7wyK3H4jET3g1kfHIbYAt5S4CIG3IIVTp-V9f9xO_jhVpqnhKohhwsnvfWNJLLoPovCDK";
                    player.isLooping = true;
                    player.Play();
                    break;
                case MapComponentType.Killzone:
                    var colliders = gameObject.GetComponentsInChildren<Collider>();
                    foreach (var collider in colliders)
                    {
                        collider.gameObject.AddComponent<RacingKillTrigger>();
                    }
                    break;
                case MapComponentType.Start:
                    gameObject.AddComponent<RacingStartBarrier>();
                    break;
                case MapComponentType.Checkpoint:
                    colliders = gameObject.GetComponentsInChildren<Collider>();
                    foreach (var collider in colliders)
                    {
                        collider.gameObject.AddComponent<RacingCheckpointTrigger>();
                    }
                    break;
                case MapComponentType.Finish:
                    colliders = gameObject.GetComponentsInChildren<Collider>();
                    foreach (var collider in colliders)
                    {
                        collider.gameObject.AddComponent<LevelTriggerRacingEnd>();
                    }
                    break;
                case MapComponentType.Trigger:
                    colliders = gameObject.GetComponentsInChildren<Collider>();
                    foreach (var collider in colliders)
                    {
                        if (collider is MeshCollider meshCollider)
                        {
                            meshCollider.convex = true;
                        }
                        collider.isTrigger = true;
                    }
                    break;
                case MapComponentType.Spawner:
                    var faction = args.GetCustomMapAttribute("f") ?? "0";
                    var entityType = args.GetCustomMapAttribute("t") ?? "0";
                    if (entityType == "1") gameObject.AddComponent<HumanSpawner>();
                    if (entityType == "2") gameObject.AddComponent<TitanSpawner>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }

    public interface IMapComponent
    {
        void Initialize(string[] args);
    }

    public enum MapComponentType
    {
        Video,
        Killzone,
        Checkpoint,
        Start,
        Finish,
        Trigger,
        Spawner
    }
}