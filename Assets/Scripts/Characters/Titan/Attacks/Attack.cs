using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class Attack
    {
        public bool IsFinished { get; set; }
        public float Cooldown { get; set; }
        public BodyPart[] BodyParts { get; set; }
        public float Stamina { get; set; } = 10f;
        protected MindlessTitan Titan { get; set; }

        protected bool IsDisabled()
        {
            return Titan.IsDisabled(BodyParts);
        }

        protected bool IsDisabled(BodyPart bodyPart)
        {
            return Titan.IsDisabled(bodyPart);
        }

        public virtual bool CanAttack()
        {
            return true;
        }

        public virtual bool CanAttack(PlayerTitan titan)
        {
            if (titan.IsDisabled(BodyParts)) return false;
            return true;
        }

        public virtual void Execute()
        {
        }

        public virtual void Initialize(MindlessTitan titan)
        {
            Titan = titan;
        }

        protected GameObject checkIfHitHand(Transform hand, float titanSize)
        {
            float num = titanSize * 2.2f + 1f;
            var mask = LayerMask.GetMask("PlayerHitBox");
            foreach (Collider collider in Physics.OverlapSphere(hand.GetComponent<SphereCollider>().transform.position, num, mask))
            {
                if (collider.transform.root.tag != "Player") continue;
                GameObject gameObject = collider.transform.root.gameObject;
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
            }
            return null;
        }
    }
}
