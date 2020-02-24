using System;
using UnityEngine;

public class PanelOption : MonoBehaviour
{
    public GameObject label_back;
    public GameObject label_camera_tilt;
    public GameObject label_change_quality;
    public GameObject label_continue;
    public GameObject label_default;
    public GameObject label_invert_mouse_y;
    public GameObject label_KEY_LEFT;
    public GameObject label_KEY_RIGHT;
    public GameObject label_mouse_sensitivity;
    public GameObject label_quit;
    private int lang = -1;

    private void OnEnable()
    {
    }

    private void showTxt()
    {
        if (this.lang != Language.type)
        {
            this.lang = Language.type;
            this.label_KEY_LEFT.GetComponent<UILabel>().text = Language.key_set_info_1[Language.type].Replace(@"\n", "\n");
            this.label_KEY_RIGHT.GetComponent<UILabel>().text = Language.key_set_info_2[Language.type].Replace(@"\n", "\n");
            this.label_mouse_sensitivity.GetComponent<UILabel>().text = Language.mouse_sensitivity[Language.type];
            this.label_change_quality.GetComponent<UILabel>().text = Language.change_quality[Language.type];
            this.label_camera_tilt.GetComponent<UILabel>().text = Language.camera_tilt[Language.type];
            this.label_default.GetComponent<UILabel>().text = Language.btn_default[Language.type];
            this.label_invert_mouse_y.GetComponent<UILabel>().text = Language.invert_mouse[Language.type];
            if (this.label_back != null)
            {
                this.label_back.GetComponent<UILabel>().text = Language.btn_back[Language.type];
            }
            if (this.label_continue != null)
            {
                this.label_continue.GetComponent<UILabel>().text = Language.btn_continue[Language.type];
            }
            if (this.label_quit != null)
            {
                this.label_quit.GetComponent<UILabel>().text = Language.btn_quit[Language.type];
            }
        }
    }

    private void Update()
    {
        this.showTxt();
    }
}

