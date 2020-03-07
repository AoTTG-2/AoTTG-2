using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XffectCache : MonoBehaviour
{
    private readonly Dictionary<string, ArrayList> ObjectDic = new Dictionary<string, ArrayList>();

    protected Transform AddObject(string name)
    {
        Transform original = base.transform.Find(name);
        if (original == null)
        {
            Debug.Log("object:" + name + "doesn't exist!");
            return null;
        }
        Transform transform2 = UnityEngine.Object.Instantiate(original, Vector3.zero, Quaternion.identity) as Transform;
        this.ObjectDic[name].Add(transform2);
        transform2.gameObject.SetActive(false);
        Xffect component = transform2.GetComponent<Xffect>();
        if (component != null)
        {
            component.Initialize();
        }
        return transform2;
    }

    private void Awake()
    {
        IEnumerator enumerator = base.transform.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform)enumerator.Current;
                this.ObjectDic[current.name] = new ArrayList();
                this.ObjectDic[current.name].Add(current);
                Xffect component = current.GetComponent<Xffect>();
                if (component != null)
                {
                    component.Initialize();
                }
                current.gameObject.SetActive(false);
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }
    }

    public Transform GetObject(string name)
    {
        ArrayList list = this.ObjectDic[name];
        if (list == null)
        {
            Debug.LogError(name + ": cache doesnt exist!");
            return null;
        }
        IEnumerator enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform)enumerator.Current;
                if (current != null && !current.gameObject.activeInHierarchy)
                {
                    current.gameObject.SetActive(true);
                    return current;
                }
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }
        return this.AddObject(name);
    }

    public ArrayList GetObjectCache(string name)
    {
        ArrayList list = this.ObjectDic[name];
        if (list == null)
        {
            Debug.LogError(name + ": cache doesnt exist!");
            return null;
        }
        return list;
    }

    private void Start()
    {
    }
}

