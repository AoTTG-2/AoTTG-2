using System;

public static class PhotonViewExtension
{
    public static void RPC(this PhotonView that, Action method, PhotonTargets target, params object[] parameters)
    {
        that.RPC(method.Method.Name, target, parameters);
    }

    public static void RPC<T1>(this PhotonView that, Action<T1> method, PhotonTargets target, params object[] parameters)
    {
        that.RPC(method.Method.Name, target, parameters);
    }

    public static void RPC<T1, T2>(this PhotonView that, Action<T1, T2> method, PhotonTargets target, params object[] parameters)
    {
        that.RPC(method.Method.Name, target, parameters);
    }

    public static void RPC<T1, T2, T3>(this PhotonView that, Action<T1, T2, T3> method, PhotonTargets target, params object[] parameters)
    {
        that.RPC(method.Method.Name, target, parameters);
    }

    public static void RPC<T1, T2, T3, T4>(this PhotonView that, Action<T1, T2, T3, T4> method, PhotonTargets target, params object[] parameters)
    {
        that.RPC(method.Method.Name, target, parameters);
    }

    public static void RPC<T1, T2, T3, T4, T5>(this PhotonView that, Action<T1, T2, T3, T4, T5> method, PhotonTargets target, params object[] parameters)
    {
        that.RPC(method.Method.Name, target, parameters);
    }

    public static void RPC<T1, T2, T3, T4, T5, T6>(this PhotonView that, Action<T1, T2, T3, T4, T5, T6> method, PhotonTargets target, params object[] parameters)
    {
        that.RPC(method.Method.Name, target, parameters);
    }

    public static void RPC<T1, T2, T3, T4, T5, T6, T7>(this PhotonView that, Action<T1, T2, T3, T4, T5, T6, T7> method, PhotonTargets target, params object[] parameters)
    {
        that.RPC(method.Method.Name, target, parameters);
    }

    public static void RPC(this PhotonView that, Action method, PhotonPlayer targetPlayer, params object[] parameters)
    {
        that.RPC(method.Method.Name, targetPlayer, parameters);
    }

    public static void RPC<T1>(this PhotonView that, Action<T1> method, PhotonPlayer targetPlayer, params object[] parameters)
    {
        that.RPC(method.Method.Name, targetPlayer, parameters);
    }

    public static void RPC<T1, T2>(this PhotonView that, Action<T1, T2> method, PhotonPlayer targetPlayer, params object[] parameters)
    {
        that.RPC(method.Method.Name, targetPlayer, parameters);
    }

    public static void RPC<T1>(this PhotonView that, Func<T1> method, PhotonTargets target, params object[] parameters)
    {
        that.RPC(method.Method.Name, target, parameters);
    }
}