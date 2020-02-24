using ExitGames.Client.Photon;
using System;
using UnityEngine;

internal static class CustomTypes
{
    private static object DeserializePhotonPlayer(byte[] bytes)
    {
        int num2;
        int offset = 0;
        Protocol.Deserialize(out num2, bytes, ref offset);
        if (PhotonNetwork.networkingPeer.mActors.ContainsKey(num2))
        {
            return PhotonNetwork.networkingPeer.mActors[num2];
        }
        return null;
    }

    private static object DeserializeQuaternion(byte[] bytes)
    {
        Quaternion quaternion = new Quaternion();
        int offset = 0;
        Protocol.Deserialize(out quaternion.w, bytes, ref offset);
        Protocol.Deserialize(out quaternion.x, bytes, ref offset);
        Protocol.Deserialize(out quaternion.y, bytes, ref offset);
        Protocol.Deserialize(out quaternion.z, bytes, ref offset);
        return quaternion;
    }

    private static object DeserializeVector2(byte[] bytes)
    {
        Vector2 vector = new Vector2();
        int offset = 0;
        Protocol.Deserialize(out vector.x, bytes, ref offset);
        Protocol.Deserialize(out vector.y, bytes, ref offset);
        return vector;
    }

    private static object DeserializeVector3(byte[] bytes)
    {
        Vector3 vector = new Vector3();
        int offset = 0;
        Protocol.Deserialize(out vector.x, bytes, ref offset);
        Protocol.Deserialize(out vector.y, bytes, ref offset);
        Protocol.Deserialize(out vector.z, bytes, ref offset);
        return vector;
    }

    internal static void Register()
    {
        PhotonPeer.RegisterType(typeof(Vector2), 0x57, new SerializeMethod(CustomTypes.SerializeVector2), new DeserializeMethod(CustomTypes.DeserializeVector2));
        PhotonPeer.RegisterType(typeof(Vector3), 0x56, new SerializeMethod(CustomTypes.SerializeVector3), new DeserializeMethod(CustomTypes.DeserializeVector3));
        PhotonPeer.RegisterType(typeof(Quaternion), 0x51, new SerializeMethod(CustomTypes.SerializeQuaternion), new DeserializeMethod(CustomTypes.DeserializeQuaternion));
        PhotonPeer.RegisterType(typeof(PhotonPlayer), 80, new SerializeMethod(CustomTypes.SerializePhotonPlayer), new DeserializeMethod(CustomTypes.DeserializePhotonPlayer));
    }

    private static byte[] SerializePhotonPlayer(object customobject)
    {
        int iD = ((PhotonPlayer) customobject).ID;
        byte[] target = new byte[4];
        int targetOffset = 0;
        Protocol.Serialize(iD, target, ref targetOffset);
        return target;
    }

    private static byte[] SerializeQuaternion(object obj)
    {
        Quaternion quaternion = (Quaternion) obj;
        byte[] target = new byte[0x10];
        int targetOffset = 0;
        Protocol.Serialize(quaternion.w, target, ref targetOffset);
        Protocol.Serialize(quaternion.x, target, ref targetOffset);
        Protocol.Serialize(quaternion.y, target, ref targetOffset);
        Protocol.Serialize(quaternion.z, target, ref targetOffset);
        return target;
    }

    private static byte[] SerializeVector2(object customobject)
    {
        Vector2 vector = (Vector2) customobject;
        byte[] target = new byte[8];
        int targetOffset = 0;
        Protocol.Serialize(vector.x, target, ref targetOffset);
        Protocol.Serialize(vector.y, target, ref targetOffset);
        return target;
    }

    private static byte[] SerializeVector3(object customobject)
    {
        Vector3 vector = (Vector3) customobject;
        int targetOffset = 0;
        byte[] target = new byte[12];
        Protocol.Serialize(vector.x, target, ref targetOffset);
        Protocol.Serialize(vector.y, target, ref targetOffset);
        Protocol.Serialize(vector.z, target, ref targetOffset);
        return target;
    }
}

