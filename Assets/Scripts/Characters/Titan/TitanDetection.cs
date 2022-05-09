using System.Collections.Generic;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan
{
    /// <summary>
    /// Titan Detection is used by Titans to "detect" an enemy. Once an enemy is detecting, it will start chasing and attacking it.
    /// </summary>
    public class TitanDetection : MonoBehaviour
    {
        public MindlessTitan Titan;
        protected IFactionService FactionService => Service.Faction;

        private void Start()
        {
            if (!Titan.photonView.isMine) return;
            InvokeRepeating(nameof(CheckPlayers), 1f, 0.5f);
        }

        private readonly List<Entity> entities = new List<Entity>();

        private void OnTriggerEnter(Collider other)
        {
            var entity = other.transform.root.gameObject.GetComponent<Entity>();
            if (entity == null || FactionService.IsFriendly(Titan, entity)) return;
            if (!entities.Contains(entity)) { entities.Add(entity); }
        }

        private void OnTriggerExit(Collider other)
        {
            var entity = other.transform.root.gameObject.GetComponent<Entity>();
            entities.Remove(entity);
        }

        //TODO 160 remove colliders from this list if they are invalid
        protected void CheckPlayers()
        {
            if (Titan.HasTarget()) return;
            foreach (var entity in entities)
            {
                if (entity == null) continue;
                Vector3 targetDir = entity.transform.position - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);
                if (angle > 0 && angle < 100)
                {
                    Titan.OnTargetDetected(entity);
                    break;
                }
            }
        }
    }
}
