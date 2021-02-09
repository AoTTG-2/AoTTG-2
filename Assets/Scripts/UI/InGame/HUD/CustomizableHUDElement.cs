using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.UI.InGame;
/* Any HUD element that is customizable should have this script attached */

public class CustomizableHUDElement : MonoBehaviour
{
    public ChangeHUDHandler handler;
    private bool beingDragged;

    void Start()
    {
       beingDragged = false;
    }

    // Update is called once per frame
    void Update()
    {   
        if(Input.GetMouseButtonDown(0))
        {
            float thisx = this.transform.position.x;
            float thisy = this.transform.position.y;
            float mousex = Input.mousePosition.x;
            float mousey = Input.mousePosition.y;

            if(Mathf.Abs(thisx - mousex) < 30 && Mathf.Abs(thisy - mousey) < 30){
                MouseDown();
            }        
        }

        if(Input.GetMouseButtonUp(0))
        {
            MouseUp();      
        }

        //if(handler.inEditMode && beingDragged)
        if(beingDragged)
        {
            Debug.Log("trying to drag minimap");
            this.transform.position = Input.mousePosition;
        }
    }

    private void MouseDown()
    {
        Debug.Log("mouse down");
        beingDragged = true;
    }

    private void MouseUp()
    {
        Debug.Log("mouse up");
        beingDragged = false;
    }
}
