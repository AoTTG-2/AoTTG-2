using System;
using UnityEngine;

public class PanelMultiSet : MonoBehaviour
{
    public GameObject label_ab;
    public GameObject label_BACK;
    public GameObject label_choose_map;
    public GameObject label_difficulty;
    public GameObject label_game_time;
    public GameObject label_hard;
    public GameObject label_max_player;
    public GameObject label_max_time;
    public GameObject label_normal;
    public GameObject label_server_name;
    public GameObject label_START;
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
            this.label_choose_map.GetComponent<UILabel>().text = Language.choose_map[Language.type];
            this.label_server_name.GetComponent<UILabel>().text = Language.server_name[Language.type];
            this.label_max_player.GetComponent<UILabel>().text = Language.max_player[Language.type];
            this.label_max_time.GetComponent<UILabel>().text = Language.max_Time[Language.type];
            this.label_game_time.GetComponent<UILabel>().text = Language.game_time[Language.type];
            this.label_difficulty.GetComponent<UILabel>().text = Language.difficulty[Language.type];
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

