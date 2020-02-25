using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BTN_ToJoin : MonoBehaviour
{
    public static GameObject CreateInput(GameObject parent, GameObject toClone, Vector3 position, Quaternion rotation, string name, string hint, uint width = 100, int maxChars = 100, bool isPassword = false)
    {
        GameObject prefab = (GameObject) UnityEngine.Object.Instantiate(toClone);
        GameObject obj3 = NGUITools.AddChild(parent, prefab);
        obj3.name = name;
        obj3.transform.localPosition = position;
        obj3.transform.localRotation = rotation;
        obj3.transform.Find("Label").gameObject.GetComponent<UILabel>().text = hint;
        obj3.GetComponent<UIInput>().isPassword = isPassword;
        obj3.GetComponent<UIInput>().maxChars = maxChars;
        Vector3 size = obj3.GetComponent<BoxCollider>().size;
        float x = size.x;
        size.x = width;
        obj3.GetComponent<BoxCollider>().size = size;
        obj3.GetComponent<UIInput>().label.lineWidth = (int) width;
        size = obj3.transform.Find("Background").localScale;
        size.x *= ((float) width) / x;
        obj3.transform.Find("Background").localScale = size;
        obj3.transform.Find("Background").position = obj3.GetComponent<UIInput>().label.transform.position;
        return obj3;
    }

    public static GameObject CreateLabel(GameObject parent, GameObject toClone, Vector3 position, Quaternion rotation, string name, string text, int fontsize, int lineWidth = 130)
    {
        GameObject prefab = (GameObject) UnityEngine.Object.Instantiate(toClone);
        GameObject obj3 = NGUITools.AddChild(parent, prefab);
        obj3.name = name;
        obj3.transform.localPosition = position;
        obj3.transform.localRotation = rotation;
        obj3.GetComponent<UILabel>().text = text;
        obj3.GetComponent<UILabel>().font.dynamicFontSize = fontsize;
        obj3.GetComponent<UILabel>().lineWidth = lineWidth;
        return obj3;
    }

    private void OnClick()
    {
        Vector3 vector;
        NGUITools.SetActive(base.transform.parent.gameObject, false);
        NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().PanelMultiJoinPrivate, true);
        Transform transform = GameObject.Find("UIRefer").GetComponent<UIMainReferences>().PanelMultiJoinPrivate.transform;
        Transform transform2 = transform.Find("ButtonJOIN");
        if (transform2.GetComponent<BTN_Join_LAN>() == null)
        {
            transform2.gameObject.AddComponent<BTN_Join_LAN>();
        }
        Transform transform3 = transform.Find("InputIP");
        Transform transform4 = transform.Find("InputPort");
        string str = PlayerPrefs.GetString("lastIP", "127.0.0.1");
        string str2 = PlayerPrefs.GetString("lastPort", "5055");
        transform3.GetComponent<UIInput>().text = str;
        transform3.GetComponent<UIInput>().label.text = str;
        transform4.GetComponent<UIInput>().text = str2;
        transform4.GetComponent<UIInput>().label.text = str2;
        transform3.GetComponent<UIInput>().label.shrinkToFit = true;
        transform4.GetComponent<UIInput>().label.shrinkToFit = true;
        Transform transform5 = transform.Find("LabelAuthPass");
        Transform transform6 = transform.Find("InputAuthPass");
        if (transform6 == null)
        {
            uint x = (uint) transform3.transform.Find("Background").localScale.x;
            vector = transform2.localPosition + new Vector3(0f, 61f, 0f);
            transform6 = CreateInput(transform.gameObject, transform3.gameObject, vector, transform2.rotation, "InputAuthPass", string.Empty, x, 100, false).transform;
            transform6.GetComponent<UIInput>().label.shrinkToFit = true;
        }
        if (transform5 == null)
        {
            vector = transform6.localPosition + new Vector3(0f, 35f, 0f);
            GameObject gameObject = transform.Find("LabelIP").gameObject;
            transform5 = CreateLabel(transform.gameObject, gameObject, vector, transform6.rotation, "LabelAuthPass", "Admin Password (Optional)", gameObject.GetComponent<UILabel>().font.dynamicFontSize, gameObject.GetComponent<UILabel>().lineWidth).transform;
            transform5.localScale = gameObject.transform.localScale;
            transform5.GetComponent<UILabel>().color = gameObject.GetComponent<UILabel>().color;
        }
        string str3 = PlayerPrefs.GetString("lastAuthPass", string.Empty);
        transform6.GetComponent<UIInput>().text = str3;
        transform6.GetComponent<UIInput>().label.text = str3;
    }

    private void Start()
    {
    }
}

