using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillFill : MonoBehaviour
{
    private Image fill;
    private float cd = 3.5f;
    
    void Start()
    {
        fill = GetComponent<Image>();
        fill.fillAmount = 1f;
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1) && fill.fillAmount == 1f)
        {
            fill.fillAmount = 0f;
        }

        if(fill.fillAmount < 1f)
        {
            fill.fillAmount += Time.deltaTime * 1f/cd;
        }
    }
}
