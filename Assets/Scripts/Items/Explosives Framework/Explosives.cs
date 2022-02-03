using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Characters.Humans;


public class Explosives : MonoBehaviour
{
    public float blastDelay = 3f;
    public float blastRadius = 5f;
    public float explosionForce = 700f;
    public float trigger = 10f;

    public GameObject explosionEffect;

    float countdown;
    float triggerDist;
    float gametime;
    bool hasExploded;

    // Start is called before the first frame update
    void Start()
    {
        hasExploded = false;
        countdown = blastDelay;
    }

    // Countdown and Explosion Check
    void Update()
    {
        gametime = Time.deltaTime;
        triggerDist = Vector3.Distance(HumanBody.FindObjectOfType<Transform>().position, transform.position);
        if ((Input.GetKeyDown(KeyCode.G)))
        {
            StartCoroutine(Grenade());
        }
        else if ((Input.GetKeyDown(KeyCode.Y)))
        {
            StartCoroutine(ThunderSpear());
        }

    }
    IEnumerator Grenade()
    {
        while (hasExploded == false)
        {
            countdown -= gametime;
            if (countdown <= 0f && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
            yield return null;
        }
    }
    IEnumerator ThunderSpear()
    {
        while (hasExploded == false)
        {
            if (triggerDist >= trigger)
            {
                Explode();
                hasExploded = true;
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Explode();
                hasExploded = true;
            }
            //countdown -= gametime;
            //if (countdown <= 0f && !hasExploded)
            //{
            //  Explode();
            //   hasExploded = true;
            //}
            yield return null;
        }
    }
    void Explode()
    { //Particle Effect
        Instantiate(explosionEffect, transform.position, transform.rotation);
     //Rigidbody Push Effect/ Explosion Collider Sphere
        Collider[]  colliders = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, blastRadius);
            }
        }


        //Removing Explosive After Explosion
        Destroy(gameObject);
    }
}
   





