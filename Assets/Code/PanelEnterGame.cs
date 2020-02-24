using System;
using UnityEngine;

public class PanelEnterGame : MonoBehaviour
{
    public GameObject label_camera_info;
    public GameObject label_camera_type;
    public GameObject label_human;
    public GameObject label_select_character;
    public GameObject label_select_titan;
    public GameObject label_titan;
    private int lang = -1;

    private void OnEnable()
    {
    }

    private void showTxt()
    {
        if (this.lang != Language.type)
        {
            this.lang = Language.type;
            this.label_human.GetComponent<UILabel>().text = Language.soldier[Language.type];
            this.label_titan.GetComponent<UILabel>().text = Language.titan[Language.type];
            this.label_select_character.GetComponent<UILabel>().text = Language.choose_character[Language.type];
            this.label_select_titan.GetComponent<UILabel>().text = Language.select_titan[Language.type];
            this.label_camera_type.GetComponent<UILabel>().text = Language.camera_type[Language.type];
            this.label_camera_info.GetComponent<UILabel>().text = Language.camera_info[Language.type];
            if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
            {
                this.label_select_titan.GetComponent<UILabel>().text = "Play As AHSS";
                this.label_titan.GetComponent<UILabel>().text = "AHSS";
            }
        }
    }

    private void Update()
    {
        this.showTxt();
    }
}

