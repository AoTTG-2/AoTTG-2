using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class Attack<T> where T : TitanBase
    {
        public string AttackAnimation { get; set; }

        public bool IsFinished { get; set; }
        public float Cooldown { get; set; }
        public BodyPart[] BodyParts { get; set; }
        public float Stamina { get; set; } = 10f;

        public virtual Type[] TargetTypes { get; }
        public int Damage { get; set; } = 100;

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
            if (Titan is PlayerTitan pt && pt.Ai) return true;
            if (!TargetTypes.Any(x => Titan.Target.GetType().IsSubclassOf(x))) return false;

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
