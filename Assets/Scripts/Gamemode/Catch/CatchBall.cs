using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Game.Gamemodes;
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
        private CatchGamemodeSetting Settings => Setting.Gamemode as CatchGamemodeSetting;

        private void Awake()
        {
            if (Settings == null)
            {
                DestroyImmediate(this);
                Debug.LogWarning("Tried to spawn a CatchBall while not within the Catch Gamemode");
                return;
            }

            Rigidbody = GetComponent<Rigidbody>();
            transform.localScale = new Vector3(Settings.BallSize.Value, Settings.BallSize.Value, Settings.BallSize.Value);
        }

        private void FixedUpdate()
        {
            if (Rigidbody.velocity.magnitude < Settings.BallSpeed.Value / 2)
            {
                Rigidbody.velocity = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), Random.Range(-1, 1f)) * Settings.BallSpeed.Value;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            var hero = collision.gameObject.GetComponent<Hero>();
            if (hero != null)
            {
                OnCaught?.Invoke(this, hero);
            }
        }
    }
}
