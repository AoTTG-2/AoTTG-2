 using UnityEngine.EventSystems;
 using UnityEngine;
using Assets.Scripts.UI.InGame;

/* Any HUD element that is customizable should have this script attached */
public class CustomizableHUDElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    public string elementName;
    public ChangeHUDHandler handler;
    private bool beingDragged;
    private string PlayerPrefsKey;
    public bool isVisible = true;
    private Animator anim;
    public GameObject selection;
    private bool onSelection;

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverSelection();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CloseSelection();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        MouseUp();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MouseDown();
    }

    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        selection.SetActive(false);

        PlayerPrefsKey = gameObject.name;

        // If the player has a custom layout, then load that. 
        if (PlayerPrefs.GetInt("hasCustomHUD", 0) == 1 && Screen.width == PlayerPrefs.GetFloat("HUDResolutionX", Screen.width))
        {
            //Debug.Log("Got into if statement and will soon call LoadCustom()");
            LoadCustom();
        }else // If resolution changes, then reset hasCustomLayout.
        {   
            PlayerPrefs.SetInt("hasCustomHUD", 0);
            // Load the default layout every time the player starts the game. This is because different resolutions have different default layouts. 
            PlayerPrefs.SetFloat(PlayerPrefsKey + "DefaultX", this.transform.position.x);
            PlayerPrefs.SetFloat(PlayerPrefsKey + "DefaultY", this.transform.position.y);
            PlayerPrefs.SetInt(PlayerPrefsKey + "DefaultVisibility", 1);
            PlayerPrefs.SetFloat(PlayerPrefsKey + "DefaultScale", 1);
        }

        // Setting the HUD resolution after the default/custom layout logic ensures that the default layout will be chosen if the resolution changes. 
        PlayerPrefs.SetFloat("HUDResolutionX", Screen.width);
        PlayerPrefs.SetFloat("HUDResolutionY", Screen.height);

        beingDragged = false;
    }

    void Update()
    {   
        if (beingDragged && handler.HUD.inEditMode) 
        {
            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;
            handler.hasChanged = true;


            // Check if the mouse is inside of the bounds of the screen. This avoid dragging HUD elements off screen. 
            if(mouseX < Screen.width && mouseX > 0 && mouseY < Screen.height && mouseY > 0)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, Input.mousePosition, 0.3f);
            }
        }

        if(!handler.HUD.inEditMode)
        {
            onSelection = false;
            selection.SetActive(false);
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

        foreach(GameObject element in handler.HUDelements) 
        {
            element.GetComponent<CustomizableHUDElement>().selection.SetActive(element == this.gameObject);
            element.GetComponent<CustomizableHUDElement>().onSelection = (element == this.gameObject);
        }
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
        if (handler.HUD.inEditMode && !onSelection) selection.SetActive(false);
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
        float baseWidth = PlayerPrefs.GetFloat("HUDResolutionX", Screen.width);
        float baseHeight = PlayerPrefs.GetFloat("HUDResolutionY", Screen.height);


        Vector3 newPositionFromResolution = new Vector3( Screen.width * (PlayerPrefsX/baseWidth) , Screen.height * (PlayerPrefsY/baseHeight) , transform.position.z);

        transform.position = newPositionFromResolution;
        transform.localScale = Vector3.one * loadedScale;
                
        isVisible = true;

    }

    public void LoadCustom()
    {
        float PlayerPrefsX = PlayerPrefs.GetFloat(PlayerPrefsKey + "CustomX", this.transform.position.x);
        float PlayerPrefsY = PlayerPrefs.GetFloat(PlayerPrefsKey + "CustomY", this.transform.position.y);
        float loadedScale = PlayerPrefs.GetFloat(PlayerPrefsKey + "Scale", 1);
        float baseWidth = PlayerPrefs.GetFloat("HUDResolutionX", Screen.width);
        float baseHeight = PlayerPrefs.GetFloat("HUDResolutionY", Screen.height);
                
        isVisible = RCextensions.intToBool(PlayerPrefs.GetInt(PlayerPrefsKey + "Visibility", 1));

        Vector3 newPositionFromResolution = new Vector3( Screen.width * (PlayerPrefsX/baseWidth) , Screen.height * (PlayerPrefsY/baseHeight) , transform.position.z);

        transform.position = newPositionFromResolution;
        transform.localScale = Vector3.one * loadedScale;
    }

    public void SaveTempPosition()
    {
        PlayerPrefs.SetFloat(PlayerPrefsKey + "TempX", this.transform.position.x);
        PlayerPrefs.SetFloat(PlayerPrefsKey + "TempY", this.transform.position.y);
        PlayerPrefs.SetFloat(PlayerPrefsKey + "TempScale", this.transform.localScale.x);
        PlayerPrefs.SetFloat("HUDResolutionX", Screen.width);
        PlayerPrefs.SetFloat("HUDResolutionY", Screen.height);
        PlayerPrefs.SetInt(PlayerPrefsKey + "TempVisibility", RCextensions.boolToInt(isVisible));
    }

    public void LoadTempPosition()
    {
        float PlayerPrefsX = PlayerPrefs.GetFloat(PlayerPrefsKey + "TempX", this.transform.position.x);
        float PlayerPrefsY = PlayerPrefs.GetFloat(PlayerPrefsKey + "TempY", this.transform.position.y);
        float loadedScale = PlayerPrefs.GetFloat(PlayerPrefsKey + "TempScale", 1);
        float baseWidth = PlayerPrefs.GetFloat("HUDResolutionX", Screen.width);
        float baseHeight = PlayerPrefs.GetFloat("HUDResolutionY", Screen.height);
                
        isVisible = RCextensions.intToBool(PlayerPrefs.GetInt(PlayerPrefsKey + "TempVisibility", 1));

        Vector3 newPositionFromResolution = new Vector3( Screen.width * (PlayerPrefsX/baseWidth) , Screen.height * (PlayerPrefsY/baseHeight) , transform.position.z);

        transform.position = newPositionFromResolution;
        transform.localScale = Vector3.one * loadedScale;
    }

    public void SetVisibility()
    {
        if(isVisible)
        {
            ShowElement();
        }
        else
        {
            //To avoid nullref exceptions, change this element's scale to zero instead of disabling the GameObject.
            this.transform.localScale = Vector3.zero;
        }
    }

    public void ShowElement()
    {
        if (PlayerPrefs.GetInt("hasCustomHUD", 0) == 1)
        {
            this.transform.localScale = Vector3.one * PlayerPrefs.GetFloat(PlayerPrefsKey + "Scale", 1);
        } else
        {
            this.transform.localScale = Vector3.one * PlayerPrefs.GetFloat(PlayerPrefsKey + "DefaultScale", 1);
        }
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
