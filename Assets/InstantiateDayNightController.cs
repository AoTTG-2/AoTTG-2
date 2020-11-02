using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateDayNightController : MonoBehaviour
{
    public GameObject DayNightControllerPrefab = null;
    public Button ActivateDayNightButton;
    // Start is called before the first frame update
    void Start()
    {
        // Instantiate(DayNightControllerPrefab, transform.position, Quaternion.identity);
        Button btn = ActivateDayNightButton.GetComponent<Button>();
        btn.onClick.AddListener(InstantiateDayAndNightController);
    }
    void InstantiateDayAndNightController()
    {
        Instantiate(DayNightControllerPrefab, transform.position, Quaternion.identity);
        Debug.Log("You have clicked the button!");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
