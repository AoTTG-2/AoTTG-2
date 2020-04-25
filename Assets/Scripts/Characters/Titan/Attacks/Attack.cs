using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class Attack
    {
        public bool IsFinished { get; set; }
        public float Cooldown { get; set; }

        public virtual bool CanAttack(MindlessTitan titan)
        {
            return true;
        }

        public virtual void Execute(MindlessTitan titan)
        {
        }

        protected GameObject checkIfHitHand(Transform hand, float titanSize)
        {
            float num = 2.4f * titanSize;
            foreach (Collider collider in Physics.OverlapSphere(hand.GetComponent<SphereCollider>().transform.position, num + 1f))
            {
                if (collider.transform.root.tag == "Player")
                {
                    GameObject gameObject = collider.transform.root.gameObject;
                    if (gameObject.GetComponent<TITAN_EREN>() != null)
                    {
                        if (!gameObject.GetComponent<TITAN_EREN>().isHit)
                        {
                            gameObject.GetComponent<TITAN_EREN>().hitByTitan();
                        }
                    }
                    else if ((gameObject.GetComponent<Hero>() != null) && !gameObject.GetComponent<Hero>().isInvincible())
                    {
                        return gameObject;
                    }
                }
            }
            return null;
        }
    }
}
