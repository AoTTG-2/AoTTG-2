using Assets.Scripts.Characters.Humans;
using System;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Gamemode.Catch
{
    public class CatchBall : MonoBehaviour
    {
        public event Action<CatchBall, Hero> OnCaught;

        private Rigidbody Rigidbody { get; set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            transform.localScale = new Vector3(CatchGamemode.BallSize, CatchGamemode.BallSize, CatchGamemode.BallSize);
        }

        private void FixedUpdate()
        {

            if (Rigidbody.velocity.magnitude < CatchGamemode.BallSpeed / 2)
            {
                Rigidbody.velocity = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), Random.Range(-1, 1f))* CatchGamemode.BallSpeed;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Ball hit: " + collision.gameObject.name);
            var hero = collision.gameObject.GetComponent<Hero>();
            if (hero != null)
            {
                OnCaught?.Invoke(this, hero);
            }
        }
    }
}
