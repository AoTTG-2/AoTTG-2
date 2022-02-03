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
  //bool possible fix to key code if statement
  //bool thunderspearOn;

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
        //possible flaw in where Hero position is gathered, some bugs with hook fires affecting position
        triggerDist = Vector3.Distance(HumanBody.FindObjectOfType<Transform>().position, transform.position);
        //Placing if statements inside of Coroutines with bool checks might fix with more testing ie:
        //if (Input.GetKeyDown(KeyCode.Y)))
        //{
        //  thunderspearOn == true;
        //}
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
      //if (thunderspearOn == true)
      //{
            while (hasExploded == false)
            {
                //distance trigger
                if (triggerDist >= trigger)
                {
                    Explode();
                    hasExploded = true;
                }
                //Player Activation
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Explode();
                    hasExploded = true;
                }
                //Thunder Spear Countdown: currently disabled to test distance trigger
                //countdown -= gametime;
                //if (countdown <= 0f && !hasExploded)
                //{
                //  Explode();
                //   hasExploded = true;
                //}
            yield return null;
            }
            //thunderspearOn = False;
      //}
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
   





