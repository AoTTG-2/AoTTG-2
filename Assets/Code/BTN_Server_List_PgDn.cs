using System;
using UnityEngine;

public class BTN_Server_List_PgDn : MonoBehaviour
{
    private void OnClick()
    {
        GameObject.Find("PanelMultiROOM").GetComponent<PanelMultiJoin>().pageDown();
    }
}

