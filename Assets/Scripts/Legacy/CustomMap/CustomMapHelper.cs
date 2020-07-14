using System;
using UnityEngine;

namespace Assets.Scripts.Room
{
    public static class CustomMapHelper
    {
        public static GameObject LegacyMapToPrefab(string[] mapData)
        {
            var position = GetPosition(mapData);
            var rotation = GetRotation(mapData);

            return (GameObject) UnityEngine.Object.Instantiate(FengGameManagerMKII.instance.RcLegacyPrefab.Get(mapData[1]), position, rotation);
        }

        private static Vector3 GetPosition(string[] mapData)
        {
            return new Vector3(Convert.ToSingle(mapData[12]), Convert.ToSingle(mapData[13]), Convert.ToSingle(mapData[14]));
        }

        private static Quaternion GetRotation(string[] mapData)
        {
            return new Quaternion(Convert.ToSingle(mapData[15]), Convert.ToSingle(mapData[16]),
                Convert.ToSingle(mapData[17]), Convert.ToSingle(mapData[18]));
        }
    }
}
