using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.UI.InGame;

/* Any HUD element that is customizable should have this script attached */
public class CustomizableHUDElement : MonoBehaviour
{
    public ChangeHUDHandler handler;
    [Range(30f, 200f)]
    public float clickAreaSizeX = 30f;
    [Range(30f, 200f)]
    public float clickAreaSizeY = 30f;
    private bool beingDragged;
    private string PlayerPrefsKey;
    public bool isVisible = true;

    void Start()
    {
        PlayerPrefsKey = gameObject.name;

        if (PlayerPrefs.GetInt("hasCustomHUD", 0) != 1)
        {
            // Set default positions. 
            PlayerPrefs.SetFloat(PlayerPrefsKey + "DefaultX", this.transform.position.x);
            PlayerPrefs.SetFloat(PlayerPrefsKey + "DefaultY", this.transform.position.y);
            PlayerPrefs.SetFloat("HUDResolutionX", Screen.width);
            PlayerPrefs.SetFloat("HUDResolutionY", Screen.height);
            PlayerPrefs.SetInt(PlayerPrefsKey + "DefaultVisibility", 1);
            PlayerPrefs.SetFloat(PlayerPrefsKey + "DefaultScale", 1);
        } else
        {
            // Grab saved positions.
            LoadCustom();
        }


        beingDragged = false;

    }

    // Update is called once per frame
    void Update()
    {   
        if(handler.HUD.inEditMode){
            if(Input.GetMouseButtonDown(0) && !handler.elementSelected)
            {
                float thisx = this.transform.position.x;
                float thisy = this.transform.position.y;
                float mousex = Input.mousePosition.x;
                float mousey = Input.mousePosition.y;

                if(Mathf.Abs(thisx - mousex) < clickAreaSizeX && Mathf.Abs(thisy - mousey) < clickAreaSizeY){
                    MouseDown();
                    handler.elementSelected = true;
                    handler.scaleSlider.value = transform.localScale.x; //x or y, doesn't matter. scale is 1:1
                    handler.selectedElement = this.gameObject;
                    handler.toggleVisibility.isOn = isVisible;
                }        
            }

            if(Input.GetMouseButtonUp(0))
            {
                MouseUp();      
                handler.elementSelected = false;
            }

            if(beingDragged)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, Input.mousePosition, 0.3f);
            }
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
        PlayerPrefs.SetFloat(PlayerPrefsKey + "Scale", this.transform.localScale.x);
        PlayerPrefs.SetFloat("HUDResolutionX", Screen.width);
        PlayerPrefs.SetFloat("HUDResolutionY", Screen.height);
        PlayerPrefs.SetInt(PlayerPrefsKey + "Visibility", RCextensions.boolToInt(isVisible));
    }

    public void LoadDefault()
    {
        float PlayerPrefsX = PlayerPrefs.GetFloat(PlayerPrefsKey + "DefaultX", this.transform.position.x);
        float PlayerPrefsY = PlayerPrefs.GetFloat(PlayerPrefsKey + "DefaultY", this.transform.position.y);
        float loadedScale = PlayerPrefs.GetFloat(PlayerPrefsKey + "DefaultScale", 1);
        float baseWidth = PlayerPrefs.GetFloat("HUDResolutionX", 1920);
        float baseHeight = PlayerPrefs.GetFloat("HUDResolutionY", 1080);


        Vector3 newPositionFromResolution = new Vector3( Screen.width * (PlayerPrefsX/baseWidth) , Screen.height * (PlayerPrefsY/baseHeight) , transform.position.z);

        transform.position = newPositionFromResolution;
        transform.localScale = new Vector3(loadedScale, loadedScale, 1);
                
        //Load Visibility
        isVisible = RCextensions.intToBool(PlayerPrefs.GetInt(PlayerPrefsKey + "DefaultVisibility", 1));

    }

    public void LoadCustom()
    {
        float PlayerPrefsX = PlayerPrefs.GetFloat(PlayerPrefsKey + "CustomX", this.transform.position.x);
        float PlayerPrefsY = PlayerPrefs.GetFloat(PlayerPrefsKey + "CustomY", this.transform.position.y);
        float loadedScale = PlayerPrefs.GetFloat(PlayerPrefsKey + "Scale", 1);
        float baseWidth = PlayerPrefs.GetFloat("HUDResolutionX", 1920);
        float baseHeight = PlayerPrefs.GetFloat("HUDResolutionY", 1080);
                
        //Load Visibility
        isVisible = RCextensions.intToBool(PlayerPrefs.GetInt(PlayerPrefsKey + "Visibility", 1));
        this.gameObject.SetActive(RCextensions.intToBool(PlayerPrefs.GetInt(PlayerPrefsKey + "Visibility", 1)));


        Vector3 newPositionFromResolution = new Vector3( Screen.width * (PlayerPrefsX/baseWidth) , Screen.height * (PlayerPrefsY/baseHeight) , transform.position.z);

        transform.position = newPositionFromResolution;
        transform.localScale = new Vector3(loadedScale, loadedScale, 1);

    }


}
