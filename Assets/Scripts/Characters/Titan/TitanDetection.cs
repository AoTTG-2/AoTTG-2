using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan
{
    public class TitanDetection : MonoBehaviour
    {
        public MindlessTitan Titan;

        void Start()
        {
            InvokeRepeating("CheckPlayers", 1f, 0.5f);
        }

        private List<Collider> colliders = new List<Collider>();

        private void OnTriggerEnter(Collider other)
        {
            if (!colliders.Contains(other)) { colliders.Add(other); }
        }

        private void OnTriggerExit(Collider other)
        {
            colliders.Remove(other);
        }

        protected void CheckPlayers()
        {
            if (Titan.HasTarget()) return;
            foreach (var collider in colliders)
            {
                var target = collider.transform.root.gameObject;
                if (target.layer != 8) continue;
                Vector3 targetDir = target.transform.position - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);
                var distance = Vector3.Distance(transform.position, target.transform.position);
                Debug.Log(distance);
                if (angle > 0 && angle < 100 && target.GetPhotonView().isMine)
                {
                    Titan.OnTargetDetected(target);
                    break;
                }
            }
        }
    }
}
