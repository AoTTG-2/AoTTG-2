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
    public Button ResetDayNightButton;
    public GameObject DayNightController;
    public GameObject MainLight;
    public Text Label;
    
    private string time;
    private double seconds;
    DayAndNightControl DayNightCycle;
    
    // Start is called before the first frame update
    void Start()
    {
        ToggleDayNight.isOn = false;
       
        Destroy(GameObject.Find("LightSet"));
        if (!GameObject.Find("LightSet(Clone)"))
        {
            Instantiate(MainLightPrefab, transform.position, Quaternion.identity);
            
        }
        //TimeSlider.value = GameObject.Find("Day and Night Controller(Clone)").GetComponent<DayAndNightControl>().currentTime;
        Button btn = ResetDayNightButton.GetComponent<Button>();
        btn.onClick.AddListener(PauseDayNightSystem);
        DayNightCycle = GameObject.Find("Day and Night Controller(Clone)").GetComponent<DayAndNightControl>();
        
        
    }
    
    public void PauseDayNightSystem()
    {
        /*ToggleDayNight.isOn = false;
        if (!GameObject.Find("LightSet(Clone)"))
        {
            Instantiate(MainLightPrefab, transform.position, Quaternion.identity);
            DayNightController = GameObject.Find("Day and Night Controller(Clone)");
            Destroy(DayNightController);
            
        }*/
        DayNightCycle = GameObject.Find("Day and Night Controller(Clone)").GetComponent<DayAndNightControl>();
        
        DayNightCycle.pause = !DayNightCycle.pause;
        
        
        Debug.Log(DayNightCycle.pause);
    }
    // Update is called once per frame
    void Update()
    {
        DynamicGI.UpdateEnvironment();
        if (ToggleDayNight.isOn)
        {
            Instantiate(DayNightControllerPrefab, transform.position, Quaternion.identity);
            
        }
        else
        {

            if (!GameObject.Find("LightSet(Clone)"))
            {
                Instantiate(MainLightPrefab, transform.position, Quaternion.identity);
                DayNightController = GameObject.Find("Day and Night Controller(Clone)");
                Destroy(DayNightController);

            }



        }
        
    }
    
    
}
