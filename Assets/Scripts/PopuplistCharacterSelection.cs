using System;
using UnityEngine;

public class PopuplistCharacterSelection : MonoBehaviour
{
    public GameObject ACL;
    public GameObject BLA;
    public GameObject GAS;
    public GameObject SPD;

    private void onCharacterChange()
    {
        HeroStat stat;
        string selection = base.GetComponent<UIPopupList>().selection;
        if ((!(selection == "Set 1") && !(selection == "Set 2")) && !(selection == "Set 3"))
        {
            stat = HeroStat.getInfo(base.GetComponent<UIPopupList>().selection);
        }
        else
        {
            HeroCostume costume = CostumeConeveter.LocalDataToHeroCostume(selection.ToUpper());
            if (costume == null)
            {
                stat = new HeroStat();
            }
            else
            {
                stat = costume.stat;
            }
        }
        this.SPD.transform.localScale = new Vector3((float) stat.SPD, 20f, 0f);
        this.GAS.transform.localScale = new Vector3((float) stat.GAS, 20f, 0f);
        this.BLA.transform.localScale = new Vector3((float) stat.BLA, 20f, 0f);
        this.ACL.transform.localScale = new Vector3((float) stat.ACL, 20f, 0f);
    }
}

