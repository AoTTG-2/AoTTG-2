using System;
using UnityEngine;

public class PanelMultiWait : MonoBehaviour
{
    public GameObject label_BACK;
    public GameObject label_camera;
    public GameObject label_character;
    public GameObject label_original;
    public GameObject label_READY;
    public GameObject label_START;
    public GameObject label_tps;
    public GameObject label_wow;
    private int lang = -1;

    private void OnEnable()
    {
    }

    private void showTxt()
    {
        if (this.lang != Language.type)
        {
            this.lang = Language.type;
            this.label_START.GetComponent<UILabel>().text = Language.btn_start[Language.type];
            this.label_BACK.GetComponent<UILabel>().text = Language.btn_back[Language.type];
            this.label_READY.GetComponent<UILabel>().text = Language.btn_ready[Language.type];
            this.label_camera.GetComponent<UILabel>().text = Language.camera_type[Language.type];
            this.label_original.GetComponent<UILabel>().text = Language.camera_original[Language.type];
            this.label_wow.GetComponent<UILabel>().text = Language.camera_wow[Language.type];
            this.label_tps.GetComponent<UILabel>().text = Language.camera_tps[Language.type];
            this.label_character.GetComponent<UILabel>().text = Language.choose_character[Language.type];
        }
    }

    private void Update()
    {
        this.showTxt();
    }
}

