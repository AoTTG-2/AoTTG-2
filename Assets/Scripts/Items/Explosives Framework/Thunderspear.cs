using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Services;

public class Thunderspear : MonoBehaviour
{
    public float blastRadius = 5f;
    public float explosionForce = 700f;
    public float blastDelay = 3f;
    public float trigger = 10f;
    public GameObject explosionEffect;
    public GameObject hero;

    public GameObject boomSound;

    private LineRenderer lr;
    Rigidbody rb;


    float triggerDist;
    float countdown;
    // Start is called before the first frame update
    void Start()
    {
        countdown = blastDelay;
    }

    // Update is called once per frame
    void Update()
    {
        //possible flaw in where Hero position is gathered, some bugs with hook fires affecting position
        rb = GetComponent<Rigidbody>();
        hero = Service.Player.Self.gameObject;
        lr = Service.Player.Self.GetComponent<LineRenderer>();
        triggerDist = Vector3.Distance(hero.transform.position, transform.position);
        Debug.Log(triggerDist + "triggerDist");
        //distance trigger
        if (triggerDist >= trigger)
        {
            Explode();
        }
        //Player Activation
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Explode();
        }
        //Thunder Spear Countdown: currently disabled to test distance trigger
        //blastDelay -= gametime;
        //if (blastDelay <= 0f)
        //{
        //  Explode();
        //  rb.isKinematic = false;
        //}
        //blastDelay = 3f;
        //}

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 8)
        {
            gameObject.transform.SetParent(collision.transform);
            Destroy(rb);
        }

    }
    public void Explode()
    { //Particle Effect
        Instantiate(explosionEffect, transform.position, transform.rotation);
       
        //Rigidbody Push Effect/ Explosion Collider Sphere
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
      
        Instantiate(boomSound, transform.position, transform.rotation);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody explosionRb = nearbyObject.GetComponent<Rigidbody>();
            if (explosionRb != null) rb.AddExplosionForce(explosionForce, transform.position, blastRadius);
        }
        //Removing Explosive After Explosion
        
        Destroy(gameObject);
       
    }
}


