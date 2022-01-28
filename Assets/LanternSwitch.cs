using Assets.Scripts.DayNightCycle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternSwitch : MonoBehaviour
{
    public GameObject DayAndNightController;
    DayAndNightControl Controller;
    public bool skybox = false;

    // Start is called before the first frame update
    void Awake()
    {
        Controller = DayAndNightController.GetComponent<DayAndNightControl>();
        gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        gameObject.GetComponent<Light>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Controller.StaticSkybox == true)
        {
            SwitchOff();

        }
        else
        {
            if (Controller.CurrentTime <= 5)
            {
                SwitchOn();
            }
            else if (Controller.CurrentTime > 5 && Controller.CurrentTime <= 7)
            {
                SwitchOn();
            }
            else if (Controller.CurrentTime > 7 && Controller.CurrentTime <= 17)
            {
                SwitchOff();
            }
            else
            {
                SwitchOn();
            }
            
        }

    }

    private void SwitchOff()
    {
        if (skybox == false)
        {
            gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            gameObject.GetComponent<Light>().enabled = false;
            skybox = true;
            Debug.Log("I turned off");
        }
    }
    private void SwitchOn()
    {
        if(skybox == true)
        {
            gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            gameObject.GetComponent<Light>().enabled = true;
            skybox = false;
            Debug.Log("I turned on");
        }
    }
}
