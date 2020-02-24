using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class Extensions
{
    public static bool AlmostEquals(this float target, float second, float floatDiff)
    {
        return (Mathf.Abs((float) (target - second)) < floatDiff);
    }

    public static bool AlmostEquals(this Quaternion target, Quaternion second, float maxAngle)
    {
        return (Quaternion.Angle(target, second) < maxAngle);
    }

    public static bool AlmostEquals(this Vector2 target, Vector2 second, float sqrMagnitudePrecision)
    {
        Vector2 vector = target - second;
        return (vector.sqrMagnitude < sqrMagnitudePrecision);
    }

    public static bool AlmostEquals(this Vector3 target, Vector3 second, float sqrMagnitudePrecision)
    {
        Vector3 vector = target - second;
        return (vector.sqrMagnitude < sqrMagnitudePrecision);
    }

    public static bool Contains(this int[] target, int nr)
    {
        if (target != null)
        {
            for (int i = 0; i < target.Length; i++)
            {
                if (target[i] == nr)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static PhotonView GetPhotonView(this GameObject go)
    {
        return go.GetComponent<PhotonView>();
    }

    public static PhotonView[] GetPhotonViewsInChildren(this GameObject go)
    {
        return go.GetComponentsInChildren<PhotonView>(true);
    }

    public static void Merge(this IDictionary target, IDictionary addHash)
    {
        if ((addHash != null) && !target.Equals(addHash))
        {
            IEnumerator enumerator = addHash.Keys.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    target[current] = addHash[current];
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }

    public static void MergeStringKeys(this IDictionary target, IDictionary addHash)
    {
        if ((addHash != null) && !target.Equals(addHash))
        {
            IEnumerator enumerator = addHash.Keys.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    if (current is string)
                    {
                        target[current] = addHash[current];
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }

    public static void StripKeysWithNullValues(this IDictionary original)
    {
        object[] objArray = new object[original.Count];
        int num = 0;
        IEnumerator enumerator = original.Keys.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                objArray[num++] = current;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
        for (int i = 0; i < objArray.Length; i++)
        {
            object key = objArray[i];
            if (original[key] == null)
            {
                original.Remove(key);
            }
        }
    }

    public static ExitGames.Client.Photon.Hashtable StripToStringKeys(this IDictionary original)
    {
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        IDictionaryEnumerator enumerator = original.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                if (enumerator.Current != null)
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    if (current.Key is string)
                    {
                        hashtable[current.Key] = current.Value;
                    }
                }
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
        return hashtable;
    }

    public static string ToStringFull(this IDictionary origin)
    {
        return SupportClass.DictionaryToString(origin, false);
    }
}

