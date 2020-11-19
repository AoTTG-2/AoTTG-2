using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class ToggleDayNightController : MonoBehaviour
{
    public GameObject DayNightControllerPrefab=null;
    public GameObject MainLightPrefab=null; //default light used in aottg
    public Toggle ToggleDayNight;
    public GameObject DayNightController;
    public GameObject MainLight;
    // Start is called before the first frame update
    void Start()
    {
        ToggleDayNight.isOn = false;
        Destroy(GameObject.Find("LightSet"));
    }
    void InstantiateDayAndNightController()
    {
        
        Instantiate(DayNightControllerPrefab, transform.position, Quaternion.identity);
        MainLight = GameObject.Find("LightSet");
        
        Destroy(MainLight);
        MainLight = GameObject.Find("LightSet(Clone)");
        Destroy(MainLight);
        Debug.Log("You have clicked the button!");
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (ToggleDayNight.isOn)
        {
            InstantiateDayAndNightController();
        }
        else
        {

            if (!GameObject.Find("LightSet(Clone)") )
            {
                Instantiate(MainLightPrefab, transform.position, Quaternion.identity);
                DayNightController = GameObject.Find("Day and Night Controller(Clone)");
                Destroy(DayNightController);
                Debug.Log("!exists");
            }
            
               
                
            
        }
    }
}
