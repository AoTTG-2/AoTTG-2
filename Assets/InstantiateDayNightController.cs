using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateDayNightController : MonoBehaviour
{
    public GameObject DayNightControllerPrefab = null;
    public Toggle ToggleDayNight;
    public GameObject DayNightCycle;
   // DayAndNightControl DayNightCycle;
    // Start is called before the first frame update
    void Start()
    {
        ToggleDayNight.isOn = false;
        //DayAndNightControl DayNightCycle;

    }
    void ToggleDayAndNightController()
    {
        
        Instantiate(DayNightControllerPrefab, transform.position, Quaternion.identity);
        Debug.Log("You have clicked the button!");
    }
    // Update is called once per frame
    void Update()
    {
        if (ToggleDayNight.isOn)
            ToggleDayAndNightController();
        else
            DayNightCycle = GameObject.Find("Day and Night Controller(Clone)");
            Destroy(DayNightCycle);
        var input = gameObject.GetComponent<InputField>();
    }
}
