using System;
using UnityEngine;

[Obsolete("Was used in AoTTG1 to calculate stylish points. Not sure what we will do with this in AoTTG2.")]
public class StylishComponent : MonoBehaviour
{
    public GameObject bar;
    private int chainKillRank;
    private float[] chainRankMultiplier;
    private float chainTime;
    private float duration;
    private Vector3 exitPosition;
    private bool flip;
    private bool hasLostRank;
    public GameObject labelChain;
    public GameObject labelHits;
    public GameObject labelS;
    public GameObject labelS1;
    public GameObject labelS2;
    public GameObject labelsub;
    public GameObject labelTotal;
    private Vector3 originalPosition;
    private float R;
    private int styleHits;
    private float stylePoints;
    private int styleRank;
    private int[] styleRankDepletions;
    private int[] styleRankPoints;
    private string[,] styleRankText;
    private int styleTotalDamage;

    public StylishComponent()
    {
        string[,] textArray1 = { { "D", "eja Vu" }, { "C", "asual" }, { "B", "oppin!" }, { "A", "mazing!" }, { "S", "ensational!" }, { "S", "pectacular!!" }, { "S", "tylish!!!" }, { "X", "TREEME!!!" } };
        styleRankText = textArray1;
        chainRankMultiplier = new[] { 1f, 1.1f, 1.2f, 1.3f, 1.5f, 1.7f, 2f, 2.3f, 2.5f };
        styleRankPoints = new[] { 350, 950, 0x992, 0x11c6, 0x1b58, 0x3a98, 0x186a0 };
        styleRankDepletions = new[] { 1, 2, 5, 10, 15, 20, 0x19, 0x19 };
    }

    private int GetRankPercentage()
    {
        if ((styleRank > 0) && (styleRank < styleRankPoints.Length))
        {
            return (int) (((stylePoints - styleRankPoints[styleRank - 1]) * 100f) / (styleRankPoints[styleRank] - styleRankPoints[styleRank - 1]));
        }
        if (styleRank == 0)
        {
            return (((int) (stylePoints * 100f)) / styleRankPoints[styleRank]);
        }
        return 100;
    }

    private int GetStyleDepletionRate()
    {
        return styleRankDepletions[styleRank];
    }

    public void reset()
    {
        styleTotalDamage = 0;
        chainKillRank = 0;
        chainTime = 0f;
        styleRank = 0;
        stylePoints = 0f;
        styleHits = 0;
    }

    private void setPosition()
    {
        originalPosition = new Vector3((int) ((Screen.width * 0.5f) - 2f), (int) ((Screen.height * 0.5f) - 150f), 0f);
        exitPosition = new Vector3(Screen.width, originalPosition.y, originalPosition.z);
    }

    private void SetRank()
    {
        int styleRank = this.styleRank;
        int index = 0;
        while (index < styleRankPoints.Length)
        {
            if (stylePoints <= styleRankPoints[index])
            {
                break;
            }
            index++;
        }
        if (index < styleRankPoints.Length)
        {
            this.styleRank = index;
        }
        else
        {
            this.styleRank = styleRankPoints.Length;
        }
        if (this.styleRank < styleRank)
        {
            if (hasLostRank)
            {
                stylePoints = 0f;
                styleHits = 0;
                styleTotalDamage = 0;
                this.styleRank = 0;
            }
            else
            {
                hasLostRank = true;
            }
        }
        else if (this.styleRank > styleRank)
        {
            hasLostRank = false;
        }
    }

    private void setRankText()
    {
        //labelS.GetComponent<UILabel>().text = styleRankText[styleRank, 0];
        //if (styleRank == 5)
        //{
        //    labelS2.GetComponent<UILabel>().text = "[" + ColorSet.color_SS + "]S";
        //}
        //else
        //{
        //    labelS2.GetComponent<UILabel>().text = string.Empty;
        //}
        //if (styleRank == 6)
        //{
        //    labelS2.GetComponent<UILabel>().text = "[" + ColorSet.color_SSS + "]S";
        //    labelS1.GetComponent<UILabel>().text = "[" + ColorSet.color_SSS + "]S";
        //}
        //else
        //{
        //    labelS1.GetComponent<UILabel>().text = string.Empty;
        //}
        //if (styleRank == 0)
        //{
        //    labelS.GetComponent<UILabel>().text = "[" + ColorSet.color_D + "]D";
        //}
        //if (styleRank == 1)
        //{
        //    labelS.GetComponent<UILabel>().text = "[" + ColorSet.color_C + "]C";
        //}
        //if (styleRank == 2)
        //{
        //    labelS.GetComponent<UILabel>().text = "[" + ColorSet.color_B + "]B";
        //}
        //if (styleRank == 3)
        //{
        //    labelS.GetComponent<UILabel>().text = "[" + ColorSet.color_A + "]A";
        //}
        //if (styleRank == 4)
        //{
        //    labelS.GetComponent<UILabel>().text = "[" + ColorSet.color_S + "]S";
        //}
        //if (styleRank == 5)
        //{
        //    labelS.GetComponent<UILabel>().text = "[" + ColorSet.color_SS + "]S";
        //}
        //if (styleRank == 6)
        //{
        //    labelS.GetComponent<UILabel>().text = "[" + ColorSet.color_SSS + "]S";
        //}
        //if (styleRank == 7)
        //{
        //    labelS.GetComponent<UILabel>().text = "[" + ColorSet.color_X + "]X";
        //}
        //labelsub.GetComponent<UILabel>().text = styleRankText[styleRank, 1];
    }

    private void shakeUpdate()
    {
        if (duration > 0f)
        {
            duration -= Time.deltaTime;
            if (flip)
            {
                gameObject.transform.localPosition = originalPosition + Vector3.up * R;
            }
            else
            {
                gameObject.transform.localPosition = originalPosition - Vector3.up * R;
            }
            flip = !flip;
            if (duration <= 0f)
            {
                gameObject.transform.localPosition = originalPosition;
            }
        }
    }

    private void Start()
    {
        setPosition();
        transform.localPosition = exitPosition;
    }

    public void startShake(int R, float duration)
    {
        if (this.duration < duration)
        {
            this.R = R;
            this.duration = duration;
        }
    }

    public void Style(int damage)
    {
        if (damage != -1)
        {
            stylePoints += (int) ((damage + 200) * chainRankMultiplier[chainKillRank]);
            styleTotalDamage += damage;
            chainKillRank = (chainKillRank >= (chainRankMultiplier.Length - 1)) ? chainKillRank : (chainKillRank + 1);
            chainTime = 5f;
            styleHits++;
            SetRank();
        }
        else if (stylePoints == 0f)
        {
            stylePoints++;
            SetRank();
        }
        startShake(5, 0.3f);
        setPosition();
        //labelTotal.GetComponent<UILabel>().text = ((int) stylePoints).ToString();
        //labelHits.GetComponent<UILabel>().text = styleHits + ((styleHits <= 1) ? "Hit" : "Hits");
        if (chainKillRank == 0)
        {
            //labelChain.GetComponent<UILabel>().text = string.Empty;
        }
        else
        {
            //labelChain.GetComponent<UILabel>().text = "x" + chainRankMultiplier[chainKillRank] + "!";
        }
    }

    private void Update()
    {
        if (!IN_GAME_MAIN_CAMERA.isPausing)
        {
            if (stylePoints > 0f)
            {
                setRankText();
                //bar.GetComponent<UISprite>().fillAmount = GetRankPercentage() * 0.01f;
                stylePoints -= (GetStyleDepletionRate() * Time.deltaTime) * 10f;
                SetRank();
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, exitPosition, Time.deltaTime * 3f);
            }
            if (chainTime > 0f)
            {
                chainTime -= Time.deltaTime;
            }
            else
            {
                chainTime = 0f;
                chainKillRank = 0;
            }
            shakeUpdate();
        }
    }
}

