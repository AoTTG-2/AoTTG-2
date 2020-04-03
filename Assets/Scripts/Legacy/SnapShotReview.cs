using System;
using UnityEngine;

[Obsolete]
public class SnapShotReview : MonoBehaviour
{
    public GameObject labelDMG;
    public GameObject labelInfo;
    public GameObject labelPage;
    //private UILabel page;
    public GameObject texture;
    private float textureH = 600f;
    private float textureW = 960f;

    private void freshInfo()
    {
        if (SnapShotSaves.getLength() == 0)
        {
            //this.page.text = "0/0";
        }
        else
        {
            //this.page.text = ((SnapShotSaves.getCurrentIndex() + 1)).ToString() + "/" + SnapShotSaves.getLength().ToString();
        }
        if (SnapShotSaves.getCurrentDMG() > 0)
        {
            //this.labelDMG.GetComponent<UILabel>().text = SnapShotSaves.getCurrentDMG().ToString();
        }
        else
        {
            //this.labelDMG.GetComponent<UILabel>().text = string.Empty;
        }
    }

    private void setTextureWH()
    {
        //if (SnapShotSaves.getLength() != 0)
        //{
        //    float num = 1.6f;
        //    float num2 = ((float) this.texture.GetComponent<UITexture>().mainTexture.width) / ((float) this.texture.GetComponent<UITexture>().mainTexture.height);
        //    if (num2 > num)
        //    {
        //        this.texture.transform.localScale = new Vector3(this.textureW, this.textureW / num2, 0f);
        //        this.labelDMG.transform.localPosition = new Vector3((float) ((int) ((this.textureW * 0.5f) - 20f)), (float) ((int) ((0f + ((this.textureW * 0.5f) / num2)) - 20f)), -20f);
        //        this.labelInfo.transform.localPosition = new Vector3((float) ((int) ((this.textureW * 0.5f) - 20f)), (float) ((int) ((0f - ((this.textureW * 0.5f) / num2)) + 20f)), -20f);
        //    }
        //    else
        //    {
        //        this.texture.transform.localScale = new Vector3(this.textureH * num2, this.textureH, 0f);
        //        this.labelDMG.transform.localPosition = new Vector3((float) ((int) (((this.textureH * num2) * 0.5f) - 20f)), (float) ((int) ((0f + (this.textureH * 0.5f)) - 20f)), -20f);
        //        this.labelInfo.transform.localPosition = new Vector3((float) ((int) (((this.textureH * num2) * 0.5f) - 20f)), (float) ((int) ((0f - (this.textureH * 0.5f)) + 20f)), -20f);
        //    }
        //}
    }

    public void ShowNextIMG()
    {
        //this.texture.GetComponent<UITexture>().mainTexture = SnapShotSaves.GetNextIMG();
        this.setTextureWH();
        this.freshInfo();
    }

    public void ShowPrevIMG()
    {
        //this.texture.GetComponent<UITexture>().mainTexture = SnapShotSaves.GetPrevIMG();
        this.setTextureWH();
        this.freshInfo();
    }

    private void Start()
    {
        QualitySettings.SetQualityLevel(5, true);
        //this.page = this.labelPage.GetComponent<UILabel>();
        if (SnapShotSaves.getLength() > 0)
        {
            //this.texture.GetComponent<UITexture>().mainTexture = SnapShotSaves.getCurrentIMG();
        }
        //this.labelInfo.GetComponent<UILabel>().text = LoginFengKAI.player.name + " " + DateTime.Today.ToShortDateString();
        this.freshInfo();
        this.setTextureWH();
    }
}

