using System;
using System.Collections.Generic;
using UnityEngine;

public static class NGUITools
{
    private static float mGlobalVolume = 1f;
    private static Color mInvisible = new Color(0f, 0f, 0f, 0f);
    private static AudioListener mListener;
    private static bool mLoaded = false;

    private static void Activate(Transform t)
    {
        SetActiveSelf(t.gameObject, true);
        int index = 0;
        int childCount = t.childCount;
        while (index < childCount)
        {
            if (t.GetChild(index).gameObject.activeSelf)
            {
                return;
            }
            index++;
        }
        int num3 = 0;
        int num4 = t.childCount;
        while (num3 < num4)
        {
            Activate(t.GetChild(num3));
            num3++;
        }
    }

    public static GameObject AddChild(GameObject parent)
    {
        GameObject obj2 = new GameObject();
        if (parent != null)
        {
            Transform transform = obj2.transform;
            transform.parent = parent.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            obj2.layer = parent.layer;
        }
        return obj2;
    }

    public static T AddChild<T>(GameObject parent) where T: Component
    {
        GameObject obj2 = AddChild(parent);
        obj2.name = GetName<T>();
        return obj2.AddComponent<T>();
    }

    public static GameObject AddChild(GameObject parent, GameObject prefab)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate(prefab) as GameObject;
        if ((obj2 != null) && (parent != null))
        {
            Transform transform = obj2.transform;
            transform.parent = parent.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            obj2.layer = parent.layer;
        }
        return obj2;
    }

    public static UISprite AddSprite(GameObject go, UIAtlas atlas, string spriteName)
    {
        UIAtlas.Sprite sprite = (atlas == null) ? null : atlas.GetSprite(spriteName);
        UISprite sprite2 = AddWidget<UISprite>(go);
        sprite2.type = ((sprite == null) || (sprite.inner == sprite.outer)) ? UISprite.Type.Simple : UISprite.Type.Sliced;
        sprite2.atlas = atlas;
        sprite2.spriteName = spriteName;
        return sprite2;
    }

    public static T AddWidget<T>(GameObject go) where T: UIWidget
    {
        int num = CalculateNextDepth(go);
        T local = AddChild<T>(go);
        local.depth = num;
        Transform transform = local.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = new Vector3(100f, 100f, 1f);
        local.gameObject.layer = go.layer;
        return local;
    }

    public static BoxCollider AddWidgetCollider(GameObject go)
    {
        if (go == null)
        {
            return null;
        }
        Collider component = go.GetComponent<Collider>();
        BoxCollider collider2 = component as BoxCollider;
        if (collider2 == null)
        {
            if (component != null)
            {
                if (Application.isPlaying)
                {
                    UnityEngine.Object.Destroy(component);
                }
                else
                {
                    UnityEngine.Object.DestroyImmediate(component);
                }
            }
            collider2 = go.AddComponent<BoxCollider>();
        }
        int num = CalculateNextDepth(go);
        Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(go.transform);
        collider2.isTrigger = true;
        collider2.center = bounds.center + ((Vector3) (Vector3.back * (num * 0.25f)));
        collider2.size = new Vector3(bounds.size.x, bounds.size.y, 0f);
        return collider2;
    }

    public static Color ApplyPMA(Color c)
    {
        if (c.a != 1f)
        {
            c.r *= c.a;
            c.g *= c.a;
            c.b *= c.a;
        }
        return c;
    }

    public static void Broadcast(string funcName)
    {
        GameObject[] objArray = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        int index = 0;
        int length = objArray.Length;
        while (index < length)
        {
            objArray[index].SendMessage(funcName, SendMessageOptions.DontRequireReceiver);
            index++;
        }
    }

    public static void Broadcast(string funcName, object param)
    {
        GameObject[] objArray = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        int index = 0;
        int length = objArray.Length;
        while (index < length)
        {
            objArray[index].SendMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
            index++;
        }
    }

    public static int CalculateNextDepth(GameObject go)
    {
        int a = -1;
        UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
        int index = 0;
        int length = componentsInChildren.Length;
        while (index < length)
        {
            a = Mathf.Max(a, componentsInChildren[index].depth);
            index++;
        }
        return (a + 1);
    }

    private static void Deactivate(Transform t)
    {
        SetActiveSelf(t.gameObject, false);
    }

    public static void Destroy(UnityEngine.Object obj)
    {
        if (obj != null)
        {
            if (Application.isPlaying)
            {
                if (obj is GameObject)
                {
                    GameObject obj2 = obj as GameObject;
                    obj2.transform.parent = null;
                }
                UnityEngine.Object.Destroy(obj);
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(obj);
            }
        }
    }

    public static void DestroyImmediate(UnityEngine.Object obj)
    {
        if (obj != null)
        {
            if (Application.isEditor)
            {
                UnityEngine.Object.DestroyImmediate(obj);
            }
            else
            {
                UnityEngine.Object.Destroy(obj);
            }
        }
    }

    public static string EncodeColor(Color c)
    {
        int num = 0xffffff & (NGUIMath.ColorToInt(c) >> 8);
        return NGUIMath.DecimalToHex(num);
    }

    public static T[] FindActive<T>() where T: Component
    {
        return (UnityEngine.Object.FindObjectsOfType(typeof(T)) as T[]);
    }

    public static Camera FindCameraForLayer(int layer)
    {
        int num = ((int) 1) << layer;
        Camera[] cameraArray = FindActive<Camera>();
        int index = 0;
        int length = cameraArray.Length;
        while (index < length)
        {
            Camera camera = cameraArray[index];
            if ((camera.cullingMask & num) != 0)
            {
                return camera;
            }
            index++;
        }
        return null;
    }

    public static T FindInParents<T>(GameObject go) where T: Component
    {
        if (go == null)
        {
            return null;
        }
        object component = go.GetComponent<T>();
        if (component == null)
        {
            for (Transform transform = go.transform.parent; transform != null; transform = transform.parent)
            {
                if (component != null)
                {
                    break;
                }
                component = transform.gameObject.GetComponent<T>();
            }
        }
        return (T) component;
    }

    public static bool GetActive(GameObject go)
    {
        return ((go != null) && go.activeInHierarchy);
    }

    public static string GetHierarchy(GameObject obj)
    {
        string name = obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            name = obj.name + "/" + name;
        }
        return ("\"" + name + "\"");
    }

    public static string GetName<T>() where T: Component
    {
        string str = typeof(T).ToString();
        if (str.StartsWith("UI"))
        {
            return str.Substring(2);
        }
        if (str.StartsWith("UnityEngine."))
        {
            str = str.Substring(12);
        }
        return str;
    }

    public static GameObject GetRoot(GameObject go)
    {
        Transform transform2;
        Transform transform = go.transform;
    Label_000B:
        transform2 = transform.parent;
        if (transform2 != null)
        {
            transform = transform2;
            goto Label_000B;
        }
        return transform.gameObject;
    }

    public static bool IsChild(Transform parent, Transform child)
    {
        if ((parent != null) && (child != null))
        {
            while (child != null)
            {
                if (child == parent)
                {
                    return true;
                }
                child = child.parent;
            }
            return false;
        }
        return false;
    }

    public static byte[] Load(string fileName)
    {
        return null;
    }

    public static void MakePixelPerfect(Transform t)
    {
        UIWidget component = t.GetComponent<UIWidget>();
        if (component != null)
        {
            component.MakePixelPerfect();
        }
        else
        {
            t.localPosition = Round(t.localPosition);
            t.localScale = Round(t.localScale);
            int index = 0;
            int childCount = t.childCount;
            while (index < childCount)
            {
                MakePixelPerfect(t.GetChild(index));
                index++;
            }
        }
    }

    public static void MarkParentAsChanged(GameObject go)
    {
        UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
        int index = 0;
        int length = componentsInChildren.Length;
        while (index < length)
        {
            componentsInChildren[index].ParentHasChanged();
            index++;
        }
    }

    public static WWW OpenURL(string url)
    {
        WWW www = null;
        try
        {
            www = new WWW(url);
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
        }
        return www;
    }

    public static WWW OpenURL(string url, WWWForm form)
    {
        if (form == null)
        {
            return OpenURL(url);
        }
        WWW www = null;
        try
        {
            www = new WWW(url, form);
        }
        catch (Exception exception)
        {
            Debug.LogError((exception == null) ? "<null>" : exception.Message);
        }
        return www;
    }

    public static Color ParseColor(string text, int offset)
    {
        int num = (NGUIMath.HexToDecimal(text[offset]) << 4) | NGUIMath.HexToDecimal(text[offset + 1]);
        int num2 = (NGUIMath.HexToDecimal(text[offset + 2]) << 4) | NGUIMath.HexToDecimal(text[offset + 3]);
        int num3 = (NGUIMath.HexToDecimal(text[offset + 4]) << 4) | NGUIMath.HexToDecimal(text[offset + 5]);
        float num4 = 0.003921569f;
        return new Color(num4 * num, num4 * num2, num4 * num3);
    }

    public static int ParseSymbol(string text, int index, List<Color> colors, bool premultiply)
    {
        int length = text.Length;
        if ((index + 2) < length)
        {
            if (text[index + 1] == '-')
            {
                if (text[index + 2] == ']')
                {
                    if ((colors != null) && (colors.Count > 1))
                    {
                        colors.RemoveAt(colors.Count - 1);
                    }
                    return 3;
                }
            }
            else if (((index + 7) < length) && (text[index + 7] == ']'))
            {
                if (colors != null)
                {
                    Color c = ParseColor(text, index + 1);
                    if (EncodeColor(c) != text.Substring(index + 1, 6).ToUpper())
                    {
                        return 0;
                    }
                    Color color2 = colors[colors.Count - 1];
                    c.a = color2.a;
                    if (premultiply && (c.a != 1f))
                    {
                        c = Color.Lerp(mInvisible, c, c.a);
                    }
                    colors.Add(c);
                }
                return 8;
            }
        }
        return 0;
    }

    public static AudioSource PlaySound(AudioClip clip)
    {
        return PlaySound(clip, 1f, 1f);
    }

    public static AudioSource PlaySound(AudioClip clip, float volume)
    {
        return PlaySound(clip, volume, 1f);
    }

    public static AudioSource PlaySound(AudioClip clip, float volume, float pitch)
    {
        volume *= soundVolume;
        if ((clip != null) && (volume > 0.01f))
        {
            if (mListener == null)
            {
                mListener = UnityEngine.Object.FindObjectOfType(typeof(AudioListener)) as AudioListener;
                if (mListener == null)
                {
                    Camera main = Camera.main;
                    if (main == null)
                    {
                        main = UnityEngine.Object.FindObjectOfType(typeof(Camera)) as Camera;
                    }
                    if (main != null)
                    {
                        mListener = main.gameObject.AddComponent<AudioListener>();
                    }
                }
            }
            if (((mListener != null) && mListener.enabled) && GetActive(mListener.gameObject))
            {
                AudioSource audio = mListener.GetComponent<AudioSource>();
                if (audio == null)
                {
                    audio = mListener.gameObject.AddComponent<AudioSource>();
                }
                audio.pitch = pitch;
                audio.PlayOneShot(clip, volume);
                return audio;
            }
        }
        return null;
    }

    public static int RandomRange(int min, int max)
    {
        if (min == max)
        {
            return min;
        }
        return UnityEngine.Random.Range(min, max + 1);
    }

    public static Vector3 Round(Vector3 v)
    {
        v.x = Mathf.Round(v.x);
        v.y = Mathf.Round(v.y);
        v.z = Mathf.Round(v.z);
        return v;
    }

    public static bool Save(string fileName, byte[] bytes)
    {
        return false;
    }

    public static void SetActive(GameObject go, bool state)
    {
        if (state)
        {
            Activate(go.transform);
        }
        else
        {
            Deactivate(go.transform);
        }
    }

    public static void SetActiveChildren(GameObject go, bool state)
    {
        Transform transform = go.transform;
        if (state)
        {
            int index = 0;
            int childCount = transform.childCount;
            while (index < childCount)
            {
                Activate(transform.GetChild(index));
                index++;
            }
        }
        else
        {
            int num3 = 0;
            int num4 = transform.childCount;
            while (num3 < num4)
            {
                Deactivate(transform.GetChild(num3));
                num3++;
            }
        }
    }

    public static void SetActiveSelf(GameObject go, bool state)
    {
        go.SetActive(state);
    }

    public static void SetLayer(GameObject go, int layer)
    {
        go.layer = layer;
        Transform transform = go.transform;
        int index = 0;
        int childCount = transform.childCount;
        while (index < childCount)
        {
            SetLayer(transform.GetChild(index).gameObject, layer);
            index++;
        }
    }

    public static string StripSymbols(string text)
    {
        if (text != null)
        {
            int index = 0;
            int length = text.Length;
            while (index < length)
            {
                char ch = text[index];
                if (ch == '[')
                {
                    int count = ParseSymbol(text, index, null, false);
                    if (count > 0)
                    {
                        text = text.Remove(index, count);
                        length = text.Length;
                        continue;
                    }
                }
                index++;
            }
        }
        return text;
    }

    public static string clipboard
    {
        get
        {
            return null;
        }
        set
        {
        }
    }

    public static bool fileAccess
    {
        get
        {
            return ((Application.platform != RuntimePlatform.WindowsWebPlayer) && (Application.platform != RuntimePlatform.OSXWebPlayer));
        }
    }

    public static float soundVolume
    {
        get
        {
            if (!mLoaded)
            {
                mLoaded = true;
                mGlobalVolume = PlayerPrefs.GetFloat("Sound", 1f);
            }
            return mGlobalVolume;
        }
        set
        {
            if (mGlobalVolume != value)
            {
                mLoaded = true;
                mGlobalVolume = value;
                PlayerPrefs.SetFloat("Sound", value);
            }
        }
    }
}

