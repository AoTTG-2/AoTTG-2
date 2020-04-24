using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Attacks
{
    public class Attack
    {
        public bool IsFinished;
        public float Cooldown;

        public virtual bool CanAttack(MindlessTitan titan)
        {
            return true;
        }

        public virtual void Execute(MindlessTitan titan)
        {
        }

        protected virtual bool ExecuteBoomAttack(MindlessTitan titan, string attackAnimation, float boomTimer, string effectName, bool hasExploded, Vector3 position)
        {
            if (IsFinished) return true;
            if (!titan.Animation.IsPlaying(attackAnimation))
            {
                titan.Animation.CrossFade(attackAnimation, 0.1f);
                return false;
            }

            if (!hasExploded && titan.Animation[attackAnimation].normalizedTime >= boomTimer)
            {
                hasExploded = true;
                GameObject obj9;
                var rotation = Quaternion.Euler(270f, 0f, 0f);
                if (titan.photonView.isMine)
                {
                    obj9 = PhotonNetwork.Instantiate(effectName, position, rotation, 0);
                }
                else
                {
                    return true;
                }
                obj9.transform.localScale = titan.transform.localScale;
                if (obj9.GetComponent<EnemyfxIDcontainer>() != null)
                {
                    obj9.GetComponent<EnemyfxIDcontainer>().titanName = titan.name;
                }

                return true;
            }

            if (titan.Animation[attackAnimation].normalizedTime >= 1f)
            {
                IsFinished = true;
                return true;
            }

            return hasExploded;
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
