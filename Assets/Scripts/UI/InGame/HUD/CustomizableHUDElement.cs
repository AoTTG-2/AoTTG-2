using UnityEngine;
using Assets.Scripts.UI.InGame;

/* Any HUD element that is customizable should have this script attached */
public class CustomizableHUDElement : MonoBehaviour
{
    public ChangeHUDHandler handler;
    private bool beingDragged;
    private string PlayerPrefsKey;
    public bool isVisible = true;
    private Animator anim;
    public GameObject selection;

    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        selection.SetActive(false);

        PlayerPrefsKey = gameObject.name;
        if (PlayerPrefs.GetInt("hasCustomHUD", 0) != 1)
        {
            //If there is no saved Custom HUD layout, set the current position, scale, and visibility as default.
            PlayerPrefs.SetFloat(PlayerPrefsKey + "DefaultX", this.transform.position.x);
            PlayerPrefs.SetFloat(PlayerPrefsKey + "DefaultY", this.transform.position.y);
            PlayerPrefs.SetFloat("HUDResolutionX", Screen.width);
            PlayerPrefs.SetFloat("HUDResolutionY", Screen.height);
            PlayerPrefs.SetInt(PlayerPrefsKey + "DefaultVisibility", 1);
            PlayerPrefs.SetFloat(PlayerPrefsKey + "DefaultScale", 1);
        } else
        {
            //If there is a custom HUD layout, then load them via PlayerPrefs.
            LoadCustom();
        }

        beingDragged = false;
    }

    void Update()
    {   
        if(handler.HUD.inEditMode){

            if(Input.GetMouseButtonDown(0) && !handler.elementSelected)
            {
                float thisx = this.transform.position.x;
                float thisy = this.transform.position.y;
                float mousex = Input.mousePosition.x;
                float mousey = Input.mousePosition.y;
            }

            if (beingDragged) this.transform.position = Vector3.Lerp(this.transform.position, Input.mousePosition, 0.3f);

        }
    }


    #region EventTriggers
    //This region consists of functions that are called via EventTriggers.

    public void MouseDown()
    {
        beingDragged = true;
        handler.elementSelected = true;
        handler.scaleSlider.value = transform.localScale.x; //x or y, doesn't matter. scale is 1:1
        handler.selectedElement = this.gameObject;
        handler.toggleVisibility.isOn = isVisible;

        Debug.Log("Pressed");
    }

    public void MouseUp()
    {
        beingDragged = false;
        handler.elementSelected = false;
    }

    public void HoverSelection()
    {
        if (handler.HUD.inEditMode) selection.SetActive(true);
    }

    public void CloseSelection()
    {
        if (handler.HUD.inEditMode) selection.SetActive(false);
    }

    #endregion


    #region Saving and Loading functions

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
                
        isVisible = true;

    }

    public void LoadCustom()
    {
        float PlayerPrefsX = PlayerPrefs.GetFloat(PlayerPrefsKey + "CustomX", this.transform.position.x);
        float PlayerPrefsY = PlayerPrefs.GetFloat(PlayerPrefsKey + "CustomY", this.transform.position.y);
        float loadedScale = PlayerPrefs.GetFloat(PlayerPrefsKey + "Scale", 1);
        float baseWidth = PlayerPrefs.GetFloat("HUDResolutionX", 1920);
        float baseHeight = PlayerPrefs.GetFloat("HUDResolutionY", 1080);
                
        isVisible = RCextensions.intToBool(PlayerPrefs.GetInt(PlayerPrefsKey + "Visibility", 1));

        Vector3 newPositionFromResolution = new Vector3( Screen.width * (PlayerPrefsX/baseWidth) , Screen.height * (PlayerPrefsY/baseHeight) , transform.position.z);

        transform.position = newPositionFromResolution;
        transform.localScale = new Vector3(loadedScale, loadedScale, 1);

    }

    #endregion

    public void AnimateCustomization()
    {
        anim.SetBool("Customizing", true);
    }

    public void StopCustomization()
    {
        anim.SetBool("Customizing", false);
    }

}
