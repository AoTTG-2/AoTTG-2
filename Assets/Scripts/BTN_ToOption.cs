using System;
using UnityEngine;

public class BTN_ToOption : MonoBehaviour
{
    private void OnClick()
    {
        NGUITools.SetActive(base.transform.parent.gameObject, false);
        NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelOption, true);
        GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().showKeyMap();
        GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = true;
    }
}

