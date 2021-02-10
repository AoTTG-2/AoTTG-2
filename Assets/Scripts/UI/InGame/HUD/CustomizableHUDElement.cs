using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.UI.InGame;

/* Any HUD element that is customizable should have this script attached */
public class CustomizableHUDElement : MonoBehaviour
{
    public ChangeHUDHandler handler;
    [Range(30f, 150f)]
    public float clickAreaSize = 30f;
    private bool beingDragged;
    private string PlayerPrefsKey;

    void Start()
    {
        PlayerPrefsKey = gameObject.name;

        if (PlayerPrefs.GetInt("hasCustomHUD", 0) != 1)
        {
            // Set default positions. 
            PlayerPrefs.SetFloat(PlayerPrefsKey + "DefaultX", this.transform.position.x);
            PlayerPrefs.SetFloat(PlayerPrefsKey + "DefaultY", this.transform.position.y);
        } else
        {
            // Grab saved positions.
            transform.position = new Vector3( PlayerPrefs.GetFloat(PlayerPrefsKey + "CustomX", this.transform.position.x), PlayerPrefs.GetFloat(PlayerPrefsKey + "CustomY", this.transform.position.y), transform.position.z);
        }
        beingDragged = false;
    }

    // Update is called once per frame
    void Update()
    {   
        if(Input.GetMouseButtonDown(0) && !handler.elementSelected)
        {
            float thisx = this.transform.position.x;
            float thisy = this.transform.position.y;
            float mousex = Input.mousePosition.x;
            float mousey = Input.mousePosition.y;

            if(Mathf.Abs(thisx - mousex) < clickAreaSize && Mathf.Abs(thisy - mousey) < clickAreaSize){
                MouseDown();
                handler.elementSelected = true;
            }        
        }

        if(Input.GetMouseButtonUp(0))
        {
            MouseUp();      
            handler.elementSelected = false;
        }

        if(beingDragged && handler.inEditMode)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, Input.mousePosition, 0.25f);
        }
    }

    public void MouseDown()
    {
        beingDragged = true;
    }

    public void MouseUp()
    {
        beingDragged = false;
    }

    public void SavePosition()
    {
        PlayerPrefs.SetFloat(PlayerPrefsKey + "CustomX", this.transform.position.x);
        PlayerPrefs.SetFloat(PlayerPrefsKey + "CustomY", this.transform.position.y);
    }

    public void LoadDefault()
    {
        transform.position = new Vector3( PlayerPrefs.GetFloat(PlayerPrefsKey + "DefaultX", this.transform.position.x), PlayerPrefs.GetFloat(PlayerPrefsKey + "DefaultY", this.transform.position.y), transform.position.z);
    }

    public void LoadCustom()
    {
        transform.position = new Vector3( PlayerPrefs.GetFloat(PlayerPrefsKey + "CustomX", this.transform.position.x), PlayerPrefs.GetFloat(PlayerPrefsKey + "CustomY", this.transform.position.y), transform.position.z);
    }
}
