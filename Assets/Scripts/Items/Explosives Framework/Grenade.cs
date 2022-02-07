using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float blastRadius = 5f;
    public float explosionForce = 700f;
    public float blastDelay = 3f;
    public float trigger = 10f;
    public GameObject explosionEffect;

    float countdown;
    // Start is called before the first frame update
    void Start()
    {
        countdown = blastDelay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f)
        {
            Explode();
        }
    }
    public void Explode()
    { //Particle Effect
        Instantiate(explosionEffect, transform.position, transform.rotation);
        //Rigidbody Push Effect/ Explosion Collider Sphere
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null) rb.AddExplosionForce(explosionForce, transform.position, blastRadius);
        }
        //Removing Explosive After Explosion
        Destroy(gameObject);
    }
}
