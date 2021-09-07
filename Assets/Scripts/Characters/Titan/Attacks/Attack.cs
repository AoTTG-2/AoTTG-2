using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    /// <summary>
    /// The abstract class for Titan Attacks
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Attack<T> where T : TitanBase
    {
        /// <summary>
        /// The current attack animation that should be used. If this value changed, the animation will be changed next frame
        /// </summary>
        public string AttackAnimation { get; set; }

        /// <summary>
        /// Set to true to indicate that the attack has finished. A titan will be in the attacking state as long as this value is false
        /// </summary>
        public bool IsFinished { get; set; }
        //TODO: implement this
        /// <summary>
        /// Currently unused, but will specify for how long this attack cannot be used, to prevent a titan from spamming the same attack over and over.
        /// </summary>
        public float Cooldown { get; set; }
        /// <summary>
        /// A list of BodyParts that is required in order to execute this attack
        /// </summary>
        public BodyPart[] BodyParts { get; set; }
        /// <summary>
        /// The amount of stamina that this attack will consume
        /// </summary>
        public float Stamina { get; set; } = 10f;

        /// <summary>
        /// A list of entity types on which this attack can be used. A titan can attack both another titan or a human in AoTTG2.
        /// </summary>
        public virtual Type[] TargetTypes { get; }
        /// <summary>
        /// The amount of damage that a titan should deal to a target. For a <see cref="Hero"/> it will instantly die, if the target is a titan, it will do this much damage.
        /// </summary>
        public int Damage { get; set; } = 100;

        /// <summary>
        /// A reference to the titan that is using this attack
        /// </summary>
        protected T Titan { get; set; }

        /// <summary>
        /// Returns true if the titan cannot use this attack because it has a bodypart disabled
        /// </summary>
        /// <returns></returns>
        protected bool IsDisabled()
        {
            return Titan.Body.IsDisabled(BodyParts);
        }

        /// <summary>
        /// Returns true if the bodypart is disabled
        /// </summary>
        /// <param name="bodyPart"></param>
        /// <returns></returns>
        protected bool IsDisabled(BodyPart bodyPart)
        {
            return Titan.Body.IsDisabled(bodyPart);
        }

        /// <summary>
        /// Returns true if the titan can use this attack on the target. The behavior of the titan will then determine which attack will be used if there's multiple available (randomized)
        /// </summary>
        /// <returns></returns>
        public virtual bool CanAttack()
        {
            if (IsDisabled()) return false;
            //TODO: 160 (Rather create a new Attack list upon target switch than every chase frame)
            if (Titan is PlayerTitan pt && pt.Ai) return true;
            if (!TargetTypes.Any(x => Titan.Target.GetType().IsSubclassOf(x))) return false;

            return true;
        }

        [Obsolete("Use CanAttack instead")]
        public virtual bool CanAttack(PlayerTitan titan)
        {
            if (titan.Body.IsDisabled(BodyParts)) return false;
            return true;
        }

        /// <summary>
        /// The implementation for how the attack should behave
        /// </summary>
        public virtual void Execute()
        {
        }

        /// <summary>
        /// Initializes the Titan attack with its owner
        /// </summary>
        /// <param name="titan"></param>
        public virtual void Initialize(TitanBase titan)
        {
            Titan = titan as T;
        }

        [Obsolete("Replace with IsEntityHit")]
        protected GameObject checkIfHitHand(Transform hand, float titanSize)
        {
            float num = titanSize * 2.2f + 1f;
            var mask = LayerMask.GetMask("PlayerHitBox", "EnemyAABB");
            foreach (Collider collider in Physics.OverlapSphere(hand.GetComponent<Collider>().transform.position, num, mask))
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
                         && !gameObject.GetComponent<Hero>().IsInvincible                         && gameObject.GetComponent<Hero>()._state != HumanState.Grab)
                {
                    return gameObject;
                }
            }
            return null;
        }

        private string LastAttackAnimation { get; set; }
        private HashSet<Entity> HitEntities { get; } = new HashSet<Entity>();
        /// <summary>
        /// Returns true if entities were hit
        /// </summary>
        /// <param name="bodyPart">The bodypart that the titan is using for the attack</param>
        /// <param name="entities">A list of entities that were hit by the titan</param>
        /// <returns></returns>
        protected bool IsEntityHit(Transform bodyPart, out HashSet<Entity> entities)
        {
            if (LastAttackAnimation != AttackAnimation)
            {
                HitEntities.Clear();
                LastAttackAnimation = AttackAnimation;
            }

            var radius = Titan.Size * 2.2f + 1f;
            var mask = LayerMask.GetMask("PlayerHitBox", "EnemyAABB");

            entities = new HashSet<Entity>();
            foreach (var collider in Physics.OverlapSphere(bodyPart.GetComponent<Collider>().transform.position, radius, mask))
            {
                //TODO #160
                if (collider.transform.root.tag != "Player" && collider.name != "AABB") continue;

                var entity = collider.transform.root.gameObject.GetComponent<Entity>();
                if (HitEntities.Contains(entity)) continue;

                if (entity == Titan || Service.Faction.IsFriendly(Titan, entity)) continue;

                entities.Add(entity);
                HitEntities.Add(entity);
            }
            return entities.Any();
        }

        /// <summary>
        /// The logic what for what should happen after an entity has been hit by an attack
        /// </summary>
        /// <param name="entity"></param>
        protected void HitEntity(Entity entity)
        {
            if (!Titan.photonView.isMine) return;
            if (entity is Hero hero)
            {
                hero.MarkDie();
                var knockbackVector = ((hero.transform.position - Titan.Body.Chest.position) * 15f * Titan.Size);

                if (PhotonNetwork.offlineMode)
                {
                    hero.Die(knockbackVector, (this is BiteAttack));
                }
                else
                {
                    object[] netDieParameters = new object[5];
                    netDieParameters[0] = knockbackVector;
                    netDieParameters[1] = (this is BiteAttack);
                    netDieParameters[2] = Titan is PlayerTitan ? Titan.photonView.viewID : -1;
                    netDieParameters[3] = Titan.name;
                    netDieParameters[4] = true;

                    hero.photonView.RPC(nameof(Hero.NetDie), PhotonTargets.All, netDieParameters);
                }
            }
            else
            {
                entity.OnHit(Titan, Damage);
            }
        }
    }
}
