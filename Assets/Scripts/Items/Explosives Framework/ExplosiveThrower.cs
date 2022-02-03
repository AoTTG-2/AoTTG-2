using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class ExplosiveThrower : MonoBehaviour
{    
        public float throwForce = 40f;
        public float spearAcc = 50f;
        public GameObject grenadePrefab;


      
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                ThrowGrenade();
            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                ThrowGrenade();
            }
        }
        void ThrowGrenade()
        {
            GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        }
}
    

