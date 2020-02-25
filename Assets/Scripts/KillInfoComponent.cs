using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class KillInfoComponent : MonoBehaviour
{
    private float alpha = 1f;
    private int col;
    public GameObject groupBig;
    public GameObject groupSmall;
    public GameObject labelNameLeft;
    public GameObject labelNameRight;
    public GameObject labelScore;
    public GameObject leftTitan;
    private float lifeTime = 8f;
    private float maxScale = 1.5f;
    private int offset = 0x18;
    public GameObject rightTitan;
    public GameObject slabelNameLeft;
    public GameObject slabelNameRight;
    public GameObject slabelScore;
    public GameObject sleftTitan;
    public GameObject spriteSkeleton;
    public GameObject spriteSword;
    public GameObject srightTitan;
    public GameObject sspriteSkeleton;
    public GameObject sspriteSword;
    private bool start;
    private float timeElapsed;

    public void destory()
    {
        this.timeElapsed = this.lifeTime;
    }

    public void moveOn()
    {
        this.col++;
        if (this.col > 4)
        {
            this.timeElapsed = this.lifeTime;
        }
        this.groupBig.SetActive(false);
        this.groupSmall.SetActive(true);
    }

    private void setAlpha(float alpha)
    {
        if (this.groupBig.activeInHierarchy)
        {
            this.labelScore.GetComponent<UILabel>().color = new Color(this.labelScore.GetComponent<UILabel>().color.r, this.labelScore.GetComponent<UILabel>().color.g, this.labelScore.GetComponent<UILabel>().color.b, alpha);
            this.leftTitan.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, alpha);
            this.rightTitan.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, alpha);
            this.labelNameLeft.GetComponent<UILabel>().color = new Color(1f, 1f, 1f, alpha);
            this.labelNameRight.GetComponent<UILabel>().color = new Color(1f, 1f, 1f, alpha);
            this.spriteSkeleton.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, alpha);
            this.spriteSword.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, alpha);
        }
        if (this.groupSmall.activeInHierarchy)
        {
            this.slabelScore.GetComponent<UILabel>().color = new Color(this.labelScore.GetComponent<UILabel>().color.r, this.labelScore.GetComponent<UILabel>().color.g, this.labelScore.GetComponent<UILabel>().color.b, alpha);
            this.sleftTitan.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, alpha);
            this.srightTitan.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, alpha);
            this.slabelNameLeft.GetComponent<UILabel>().color = new Color(1f, 1f, 1f, alpha);
            this.slabelNameRight.GetComponent<UILabel>().color = new Color(1f, 1f, 1f, alpha);
            this.sspriteSkeleton.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, alpha);
            this.sspriteSword.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, alpha);
        }
    }

    public void show(bool isTitan1, string name1, bool isTitan2, string name2, int dmg = 0)
    {
        this.groupBig.SetActive(true);
        this.groupSmall.SetActive(true);
        if (!isTitan1)
        {
            this.leftTitan.SetActive(false);
            this.spriteSkeleton.SetActive(false);
            this.sleftTitan.SetActive(false);
            this.sspriteSkeleton.SetActive(false);
            Transform transform = this.labelNameLeft.transform;
            transform.position += new Vector3(18f, 0f, 0f);
            Transform transform2 = this.slabelNameLeft.transform;
            transform2.position += new Vector3(16f, 0f, 0f);
        }
        else
        {
            this.spriteSword.SetActive(false);
            this.sspriteSword.SetActive(false);
            Transform transform3 = this.labelNameRight.transform;
            transform3.position -= new Vector3(18f, 0f, 0f);
            Transform transform4 = this.slabelNameRight.transform;
            transform4.position -= new Vector3(16f, 0f, 0f);
        }
        if (!isTitan2)
        {
            this.rightTitan.SetActive(false);
            this.srightTitan.SetActive(false);
        }
        this.labelNameLeft.GetComponent<UILabel>().text = name1;
        this.labelNameRight.GetComponent<UILabel>().text = name2;
        this.slabelNameLeft.GetComponent<UILabel>().text = name1;
        this.slabelNameRight.GetComponent<UILabel>().text = name2;
        if (dmg == 0)
        {
            this.labelScore.GetComponent<UILabel>().text = string.Empty;
            this.slabelScore.GetComponent<UILabel>().text = string.Empty;
        }
        else
        {
            this.labelScore.GetComponent<UILabel>().text = dmg.ToString();
            this.slabelScore.GetComponent<UILabel>().text = dmg.ToString();
            if (dmg > 0x3e8)
            {
                this.labelScore.GetComponent<UILabel>().color = Color.red;
                this.slabelScore.GetComponent<UILabel>().color = Color.red;
            }
        }
        this.groupSmall.SetActive(false);
    }

    private void Start()
    {
        this.start = true;
        base.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
        base.transform.localPosition = new Vector3(0f, -100f + (Screen.height * 0.5f), 0f);
    }

    private void Update()
    {
        if (this.start)
        {
            this.timeElapsed += Time.deltaTime;
            if (this.timeElapsed < 0.2f)
            {
                base.transform.localScale = Vector3.Lerp(base.transform.localScale, (Vector3) (Vector3.one * this.maxScale), Time.deltaTime * 10f);
            }
            else if (this.timeElapsed < 1f)
            {
                base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.one, Time.deltaTime * 10f);
            }
            if (this.timeElapsed > this.lifeTime)
            {
                base.transform.position += new Vector3(0f, Time.deltaTime * 0.15f, 0f);
                this.alpha = ((1f - (Time.deltaTime * 45f)) + this.lifeTime) - this.timeElapsed;
                this.setAlpha(this.alpha);
            }
            else
            {
                float num = ((int) (100f - (Screen.height * 0.5f))) + (this.col * this.offset);
                base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, new Vector3(0f, -num, 0f), Time.deltaTime * 10f);
            }
            if (this.timeElapsed > (this.lifeTime + 0.5f))
            {
                UnityEngine.Object.Destroy(base.gameObject);
            }
        }
    }
}

