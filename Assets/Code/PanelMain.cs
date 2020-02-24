using System;
using UnityEngine;

public class PanelMain : MonoBehaviour
{
    public GameObject label_credits;
    public GameObject label_multi;
    public GameObject label_option;
    public GameObject label_single;
    private int lang = -1;

    private void OnEnable()
    {
    }

    private void showTxt()
    {
        if (this.lang != Language.type)
        {
            this.lang = Language.type;
            this.label_single.GetComponent<UILabel>().text = Language.btn_single[Language.type];
            this.label_multi.GetComponent<UILabel>().text = Language.btn_multiplayer[Language.type];
            this.label_option.GetComponent<UILabel>().text = Language.btn_option[Language.type];
            this.label_credits.GetComponent<UILabel>().text = Language.btn_credits[Language.type];
        }
    }

    private void Update()
    {
        this.showTxt();
    }
}

