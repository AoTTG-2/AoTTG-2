using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public abstract class Attack<T> where T : TitanBase
    {
        public bool IsFinished { get; set; }
        public float Cooldown { get; set; }
        public BodyPart[] BodyParts { get; set; }
        public float Stamina { get; set; } = 10f;

        public abstract Type[] TargetTypes { get; }

        protected T Titan { get; set; }

        protected bool IsDisabled()
        {
            return Titan.Body.IsDisabled(BodyParts);
        }

        protected bool IsDisabled(BodyPart bodyPart)
        {
            return Titan.Body.IsDisabled(bodyPart);
        }

        public virtual bool CanAttack()
        {
            if (IsDisabled()) return false;
            //TODO: 160 (Rather create a new Attack list upon target switch than every chase frame)
            if (TargetTypes.All(x => Titan.Target.GetType().IsSubclassOf(x))) return false;

            return true;
        }

        [Obsolete]
        public virtual bool CanAttack(PlayerTitan titan)
        {
            if (titan.Body.IsDisabled(BodyParts)) return false;
            return true;
        }

        public virtual void Execute()
        {
        }

        public virtual void Initialize(TitanBase titan)
        {
            Titan = titan as T;
        }

        [Obsolete("Replace with IsEntityHit")]
        protected GameObject checkIfHitHand(Transform hand, float titanSize)
        {
            float num = titanSize * 2.2f + 1f;
            var mask = LayerMask.GetMask("PlayerHitBox", "EnemyAABB");
            foreach (Collider collider in Physics.OverlapSphere(hand.GetComponent<SphereCollider>().transform.position, num, mask))
            {
                if (collider.transform.root.tag != "Player" && collider.name != "AABB") continue;
                GameObject gameObject = collider.transform.root.gameObject;

                var entity = collider.transform.root.gameObject.GetComponent<Entity>();
                if (gameObject.GetComponent<ErenTitan>() != null)
                {
                    if (!gameObject.GetComponent<ErenTitan>().isHit)
                    {
                        gameObject.GetComponent<ErenTitan>().hitByTitan();
                    }
                }
                else if ((gameObject.GetComponent<Hero>() != null) 
                         && !gameObject.GetComponent<Hero>().isInvincible()
                         && gameObject.GetComponent<Hero>()._state != HERO_STATE.Grab)
                {
                    return gameObject;
                }
                else if (entity is TitanBase titanBase && titanBase != Titan)
                {

                    titanBase.OnNapeHitRpc2(Titan, 100);

                    var direction = (entity.transform.position - Titan.transform.position).normalized;
                    titanBase.Rigidbody.AddForce(direction * 100f * 0.1f * 0.25f, ForceMode.VelocityChange);
                }
            }
            return null;
        }

        protected bool IsEntityHit(Transform bodyPart, out HashSet<Entity> entities)
        {
            var radius = Titan.Size * 2.2f + 1f;
            var mask = LayerMask.GetMask("PlayerHitBox", "EnemyAABB");

            entities = new HashSet<Entity>();
            foreach (var collider in Physics.OverlapSphere(bodyPart.GetComponent<Collider>().transform.position, radius, mask))
            {
                //TODO #160
                if (collider.transform.root.tag != "Player" && collider.name != "AABB") continue;
                
                var entity = collider.transform.root.gameObject.GetComponent<Entity>();
                if (entity == Titan || Service.Faction.IsFriendly(Titan, entity)) continue;

                if (entity is ErenTitan erenTitan)
                {
                    entities.Add(entity);
                    if (!erenTitan.isHit)
                    {
                        erenTitan.hitByTitan();
                    }
                }
                else if (entity is Hero hero
                        && !hero.isInvincible()
                        && hero._state != HERO_STATE.Grab)
                {
                    entities.Add(entity);
                }
                else if (entity is TitanBase titanBase)
                {
                    entities.Add(entity);

                    //TODO: Determine how damage logic should be calculated
                    titanBase.OnNapeHitRpc2(Titan, 100);

                    var direction = (entity.transform.position - Titan.transform.position).normalized;
                    titanBase.Rigidbody.AddForce(direction * 100f * 0.1f * 0.25f, ForceMode.VelocityChange);
                }
            }
            return entities.Any();
        }
    }
}
