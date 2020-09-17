using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class InstantiateDayNightController : MonoBehaviour
{
    public GameObject DayNightControllerPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(DayNightControllerPrefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
