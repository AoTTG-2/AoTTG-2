using System;
using Assets.Scripts.Characters.Titan;
using UnityEngine;
using Zenject;

namespace Cannon
{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody), typeof(PhotonView))]
    internal sealed class CannonBall : Photon.MonoBehaviour
    {
        private BoomFactory boomFactory;
        private new Collider collider;
        private int heroViewId;
        private new Rigidbody rigidbody;
        private Settings settings;
        private new Transform transform;

        private void Awake()
        {
            transform = base.transform;
            collider = GetComponent<SphereCollider>();
            rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            collider.isTrigger = true;
        }

        private void FixedUpdate()
        {
            rigidbody.AddForce(settings.Gravity);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!photonView.isMine) return;

            var titan = other.gameObject.GetComponent<MindlessTitan>();
            if (titan)
                titan.photonView.RPC(nameof(titan.OnCannonHitRpc), titan.photonView.owner, heroViewId, other.collider.name);
            
            Explode();
        }

        private void OnTriggerExit(Collider other)
        {
            collider.isTrigger = false;
        }

        private void Explode()
        {
            boomFactory.Create(4, transform.position, transform.rotation);
            PhotonNetwork.Destroy(gameObject);
        }

        [Inject]
        private void Construct(
            Settings settings,
            BoomFactory boomFactory,
            Vector3 velocity,
            int heroViewId)
        {
            this.settings = settings;
            this.boomFactory = boomFactory;
            this.heroViewId = heroViewId;
            
            rigidbody.velocity = velocity;
        }

        public sealed class Factory : PlaceholderFactory<int, Vector3, Vector3, Quaternion, byte, CannonBall> {}

        [Serializable]
        public class Settings
        {
            public Vector3 Gravity = Vector3.down * 30f;
        }
    }
}