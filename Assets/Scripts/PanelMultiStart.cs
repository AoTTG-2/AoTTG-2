using System;
using UnityEngine;

public class PanelMultiStart : MonoBehaviour
{
    public GameObject label_BACK;
    public GameObject label_LAN;
    public GameObject label_QUICK_MATCH;
    public GameObject label_server;
    public GameObject label_server_ASIA;
    public GameObject label_server_EU;
    public GameObject label_server_JAPAN;
    public GameObject label_server_US;
    private int lang = -1;

    private void OnEnable()
    {
    }

    private void showTxt()
    {
        if (this.lang != Language.type)
        {
            this.lang = Language.type;
            this.label_BACK.GetComponent<UILabel>().text = Language.btn_back[Language.type];
            this.label_LAN.GetComponent<UILabel>().text = Language.btn_LAN[Language.type];
            this.label_server_US.GetComponent<UILabel>().text = Language.btn_server_US[Language.type];
            this.label_server_EU.GetComponent<UILabel>().text = Language.btn_server_EU[Language.type];
            this.label_server_ASIA.GetComponent<UILabel>().text = Language.btn_server_ASIA[Language.type];
            this.label_server_JAPAN.GetComponent<UILabel>().text = Language.btn_server_JAPAN[Language.type];
            this.label_QUICK_MATCH.GetComponent<UILabel>().text = Language.btn_QUICK_MATCH[Language.type];
            this.label_server.GetComponent<UILabel>().text = Language.choose_region_server[Language.type];
        }
    }

    private void Update()
    {
        this.showTxt();
    }
}

