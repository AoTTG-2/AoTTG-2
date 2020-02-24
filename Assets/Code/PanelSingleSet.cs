using System;
using UnityEngine;

public class PanelSingleSet : MonoBehaviour
{
    public GameObject label_ab;
    public GameObject label_BACK;
    public GameObject label_camera;
    public GameObject label_choose_character;
    public GameObject label_choose_map;
    public GameObject label_difficulty;
    public GameObject label_hard;
    public GameObject label_normal;
    public GameObject label_original;
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
            this.label_camera.GetComponent<UILabel>().text = Language.camera_type[Language.type];
            this.label_original.GetComponent<UILabel>().text = Language.camera_original[Language.type];
            this.label_wow.GetComponent<UILabel>().text = Language.camera_wow[Language.type];
            this.label_tps.GetComponent<UILabel>().text = Language.camera_tps[Language.type];
            this.label_choose_character.GetComponent<UILabel>().text = Language.choose_character[Language.type];
            this.label_difficulty.GetComponent<UILabel>().text = Language.difficulty[Language.type];
            this.label_choose_map.GetComponent<UILabel>().text = Language.choose_map[Language.type];
            this.label_normal.GetComponent<UILabel>().text = Language.normal[Language.type];
            this.label_hard.GetComponent<UILabel>().text = Language.hard[Language.type];
            this.label_ab.GetComponent<UILabel>().text = Language.abnormal[Language.type];
        }
    }

    private void Update()
    {
        this.showTxt();
    }
}

