using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters;

namespace Assets.Scripts.Items.ExplosivesFramework
{
    public class Explosion : MonoBehaviour
    {
        public static Explosion Instance { get; private set; }
        public float blastRadius = 5f;
        public float explosionForce = 700f;
        public GameObject explosionEffect;
        // Countdown and Explosion Check
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
}






