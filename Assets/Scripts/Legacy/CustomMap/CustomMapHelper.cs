using System;
using Assets.Scripts.Legacy.CustomMap;
using UnityEngine;

namespace Assets.Scripts.Room
{
    public static class CustomMapHelper
    {
        public static GameObject LegacyMapToPrefab(string[] mapData, RcObjectType type)
        {
            var position = GetPosition(mapData, type);
            var rotation = GetRotation(mapData, type);

            return (GameObject) UnityEngine.Object.Instantiate(FengGameManagerMKII.instance.RcLegacy.GetPrefab(mapData[1]), position, rotation);
        }

        private static Vector3 GetPosition(string[] mapData, RcObjectType type)
        {
            var index = GetPositionIndex(type);
            return new Vector3(Convert.ToSingle(mapData[index[0]]), Convert.ToSingle(index[1]), Convert.ToSingle(mapData[index[2]]));
        }

        private static Quaternion GetRotation(string[] mapData, RcObjectType type)
        {
            var index = GetRotationIndex(type);
            return new Quaternion(Convert.ToSingle(mapData[index[0]]), Convert.ToSingle(mapData[index[1]]),
                Convert.ToSingle(mapData[index[2]]), Convert.ToSingle(mapData[index[3]]));
        }

        private static int[] GetPositionIndex(RcObjectType type)
        {
            switch (type)
            {
                case RcObjectType.Custom:
                case RcObjectType.Base:
                    return new[] {12, 13, 14};
                case RcObjectType.BaseSpecial:
                    return new[] { 2, 3, 4 };
                case RcObjectType.Misc:
                case RcObjectType.Racing:
                    return new[] { 5, 6, 7 };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static int[] GetRotationIndex(RcObjectType type)
        {
            switch (type)
            {
                case RcObjectType.Custom:
                case RcObjectType.Base:
                    return new[] { 15, 16, 17, 18 };
                case RcObjectType.BaseSpecial:
                    return new[] { 5, 6, 7, 8 };
                case RcObjectType.Misc:
                case RcObjectType.Racing:
                    return new[] { 8, 9, 10, 11 };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
