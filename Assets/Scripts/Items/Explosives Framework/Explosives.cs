using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters;


public class Explosives : MonoBehaviour
{
    public float blastDelay = 3f;
    public float blastRadius = 5f;
    public float explosionForce = 700f;
    public float trigger = 10f;

    public GameObject explosionEffect;

    float triggerDist;
    float countdown;
  //bool possible fix to key code if statement
    bool thunderspearOn;

    // Start is called before the first frame update
    void Start()
    {
        thunderspearOn = false;
        countdown = blastDelay;
    }

    // Countdown and Explosion Check
    void Update()
    {
        //possible flaw in where Hero position is gathered, some bugs with hook fires affecting position
        triggerDist = Vector3.Distance(Hero.FindObjectOfType<GameObject>().transform.position, transform.position);
        Debug.Log(triggerDist + "triggerDist");
        //Placing if statements inside of Coroutines with bool checks might fix with more testing ie:
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
          //thunderspearOn = true;
        //}
        if(Input.GetKeyDown(KeyCode.Y))
        {
            thunderspearOn = true;
        }
        if (thunderspearOn)
        {
            //distance trigger
            if (triggerDist >= trigger)
            {
                Explode();
                thunderspearOn = false;
            }
            //Player Activation
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Explode();
                thunderspearOn = false;
            }
            //Thunder Spear Countdown: currently disabled to test distance trigger
            //blastDelay -= gametime;
            //if (blastDelay <= 0f)
            //{
            //  Explode();
            //}
            //blastDelay = 3f;
            //}
        }
        else if(!thunderspearOn)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f)
            {
                Explode();
            }
        }
    }
    private void ThunderSpear()
    {
               
    }
    private void Explode()
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






