using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class ExplosiveThrower : MonoBehaviour
{    
        public float throwForce = 40f;
        public float spearAcc = 1000f;
        public GameObject grenadePrefab;
        public GameObject thunderspearPrefab;


      
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G)) ThrowGrenade();
            if (Input.GetKeyDown(KeyCode.Y)) ThrowThunderSpear();
        }
        void ThrowGrenade()
        {
            GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        }
        void ThrowThunderSpear()
        {
        GameObject thunderspear = Instantiate(thunderspearPrefab, transform.position, transform.rotation);
        Rigidbody rb = thunderspear.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        rb.AddForce(transform.forward * spearAcc, ForceMode.Acceleration);
    }
}
    

