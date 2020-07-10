using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class Attack
    {
        public bool IsFinished { get; set; }
        public float Cooldown { get; set; }
        public BodyPart[] BodyParts { get; set; }
        public float Stamina { get; set; } = 10f;

        protected bool IsDisabled(MindlessTitan titan)
        {
            return titan.IsDisabled(BodyParts);
        }

        protected bool IsDisabled(MindlessTitan titan, BodyPart bodyPart)
        {
            return titan.IsDisabled(bodyPart);
        }

        public virtual bool CanAttack(MindlessTitan titan)
        {
            return true;
        }

        public virtual bool CanAttack(PlayerTitan titan)
        {
            if (IsDisabled(titan)) return false;
            return true;
        }

        public virtual void Execute(MindlessTitan titan)
        {
        }

        protected GameObject checkIfHitHand(Transform hand, float titanSize)
        {
            float num = titanSize * 2.2f + 1f;
            var mask = LayerMask.GetMask("PlayerHitBox");
            foreach (Collider collider in Physics.OverlapSphere(hand.GetComponent<SphereCollider>().transform.position, num, mask))
            {
                if (collider.transform.root.tag != "Player") continue;
                GameObject gameObject = collider.transform.root.gameObject;
                if (gameObject.GetComponent<TITAN_EREN>() != null)
                {
                    if (!gameObject.GetComponent<TITAN_EREN>().isHit)
                    {
                        gameObject.GetComponent<TITAN_EREN>().hitByTitan();
                    }
                }
                else if ((gameObject.GetComponent<Hero>() != null) && !gameObject.GetComponent<Hero>().IsInvincible())
                {
                    return gameObject;
                }
            }
            return null;
        }
    }
}
