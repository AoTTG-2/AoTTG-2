using UnityEngine;
using Zenject;

public static class PhotonFactoryUtility
{
    public static TValue Instantiate<TValue>(string prefabName, Vector3 position, Quaternion rotation, byte group)
    {
        return PhotonNetwork.Instantiate(prefabName, position, rotation, group).GetComponent<TValue>();
    }

    public static TValue InstantiateAndInject<TValue>(
        this DiContainer that,
        string prefabName,
        Vector3 position,
        Quaternion rotation,
        byte group,
        object[] args)
    {
        var instance = Instantiate<TValue>(
            prefabName,
            position,
            rotation,
            group);
        that.Inject(instance, args);
        return instance;
    }
}

public class PhotonFactory
{
    protected readonly DiContainer Container;
    protected readonly string PrefabName;

    public PhotonFactory(DiContainer container, string prefabName)
    {
        Container = container;
        PrefabName = prefabName;
    }
}

public class PhotonFactory<TValue> : PhotonFactory, IFactory<Vector3, Quaternion, byte, TValue>
{
    public PhotonFactory(DiContainer container, string prefabName)
        : base(container, prefabName)
    {
    }

    public TValue Create(Vector3 position, Quaternion rotation, byte group)
    {
        return PhotonFactoryUtility.Instantiate<TValue>(PrefabName, position, rotation, group);
    }
}

public class PhotonFactory<TParam1, TValue> : PhotonFactory, IFactory<TParam1, Vector3, Quaternion, byte, TValue>
{
    public PhotonFactory(DiContainer container, string prefabName)
        : base(container, prefabName)
    {
    }

    public TValue Create(TParam1 param1, Vector3 position, Quaternion rotation, byte group)
    {
        var args = new object[] { param1 };
        return Container.InstantiateAndInject<TValue>(PrefabName, position, rotation, group, args);
    }
}

public class PhotonFactory<TParam1, TParam2, TValue> : PhotonFactory, IFactory<TParam1, TParam2, Vector3, Quaternion, byte, TValue>
{
    public PhotonFactory(DiContainer container, string prefabName)
        : base(container, prefabName)
    {
    }

    public TValue Create(TParam1 param1, TParam2 param2, Vector3 position, Quaternion rotation, byte group)
    {
        var args = new object[] { param1, param2 };
        return Container.InstantiateAndInject<TValue>(PrefabName, position, rotation, group, args);
    }
}

public class PhotonFactory<TParam1, TParam2, TParam3, TValue> : PhotonFactory<TParam1, TValue>, IFactory<TParam1, TParam2, TParam3, Vector3, Quaternion, byte, TValue>
{
    public PhotonFactory(DiContainer container, string prefabName)
        : base(container, prefabName)
    {
    }

    public TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, Vector3 position, Quaternion rotation, byte group)
    {
        var args = new object[] { param1, param2, param3 };
        return Container.InstantiateAndInject<TValue>(PrefabName, position, rotation, group, args);
    }
}

public class PhotonFactory<TParam1, TParam2, TParam3, TParam4, TValue> : PhotonFactory<TParam1, TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, Vector3, Quaternion, byte, TValue>
{
    public PhotonFactory(DiContainer container, string prefabName)
        : base(container, prefabName)
    {
    }

    public TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, Vector3 position, Quaternion rotation, byte group)
    {
        var args = new object[] { param1, param2, param3, param4 };
        return Container.InstantiateAndInject<TValue>(PrefabName, position, rotation, group, args);
    }
}

public class PhotonFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> : PhotonFactory<TParam1, TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, Vector3, Quaternion, byte, TValue>
{
    public PhotonFactory(DiContainer container, string prefabName)
        : base(container, prefabName)
    {
    }

    public TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, Vector3 position, Quaternion rotation, byte group)
    {
        var args = new object[] { param1, param2, param3, param4, param5 };
        return Container.InstantiateAndInject<TValue>(PrefabName, position, rotation, group, args);
    }
}

public class PhotonFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> : PhotonFactory<TParam1, TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, Vector3, Quaternion, byte, TValue>
{
    public PhotonFactory(DiContainer container, string prefabName)
        : base(container, prefabName)
    {
    }

    public TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, Vector3 position, Quaternion rotation, byte group)
    {
        var args = new object[] { param1, param2, param3, param4, param5, param6 };
        return Container.InstantiateAndInject<TValue>(PrefabName, position, rotation, group, args);
    }
}

public class PhotonFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> : PhotonFactory<TParam1, TValue>, IFactory<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, Vector3, Quaternion, byte, TValue>
{
    public PhotonFactory(DiContainer container, string prefabName)
        : base(container, prefabName)
    {
    }

    public TValue Create(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, Vector3 position, Quaternion rotation, byte group)
    {
        var args = new object[] { param1, param2, param3, param4, param5, param6, param7 };
        return Container.InstantiateAndInject<TValue>(PrefabName, position, rotation, group, args);
    }
}

public class PhotonFactoryWithArgs<TValue> : PhotonFactory, IFactory<object[], Vector3, Quaternion, byte, TValue>
{
    public PhotonFactoryWithArgs(DiContainer container, string prefabName)
        : base(container, prefabName)
    {
    }

    public TValue Create(object[] args, Vector3 position, Quaternion rotation, byte group)
    {
        return Container.InstantiateAndInject<TValue>(PrefabName, position, rotation, group, args);
    }
}