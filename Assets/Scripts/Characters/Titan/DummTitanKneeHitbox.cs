using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummTitanKneeHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            Debug.Log("Knee is hit");
        }
    }
}
