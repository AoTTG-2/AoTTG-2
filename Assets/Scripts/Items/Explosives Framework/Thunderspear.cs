using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Characters.Humans;

public class Thunderspear : MonoBehaviour
{
    public float blastRadius = 5f;
    public float explosionForce = 700f;
    public float blastDelay = 3f;
    public float trigger = 10f;
    public GameObject explosionEffect;

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
        triggerDist = Vector3.Distance(Hero.FindObjectOfType<GameObject>().transform.position, transform.position);
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
        //  Explosion.Instance.Explode();
        //}
        //blastDelay = 3f;
        //}
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


