using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour
{
    public GameObject GasNotification;
    public float currentGas = 100f;

    void Update()
    {
        GasNotification.SetActive(currentGas <= 10f);
    }
}
