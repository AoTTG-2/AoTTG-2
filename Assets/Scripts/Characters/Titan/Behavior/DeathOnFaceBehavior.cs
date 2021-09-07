using Assets.Scripts.Characters.Humans;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Behavior
{
    /// <summary>
    /// Used for Crawlers. If the Mouth of the titan is hit, the player will die.
    /// </summary>
    public class DeathOnFaceBehavior : TitanBehavior
    {
        protected override bool OnChase()
        {
            var hero = this.checkIfHitCrawlerMouth(Titan.Body.Head, 2.2f, Titan.Size);
            if (hero == null) return false;
            {
                Vector3 vector15 = Titan.Body.Chest.position;
                if (Titan.photonView.isMine)
                {
                    if (!hero.HasDied())
                    {
                        hero.MarkDie();
                        object[] objArray9 = new object[] { (Vector3)(((hero.transform.position - vector15) * 15f) * Titan.Size), true, Titan.photonView.viewID, Titan.name, true };
                        hero.photonView.RPC(nameof(Hero.NetDie), PhotonTargets.All, objArray9);
                        return true;
                    }
                }
            }
            return false;
        }

        private Hero checkIfHitCrawlerMouth(Transform head, float rad, float size)
        {
            float num = rad * size;
            foreach (Hero hero in EntityService.GetAll<Hero>())
            {
                if (hero.GetComponent<ErenTitan>() == null && !hero.GetComponent<Hero>().IsInvincible)
                {
                    float num3 = hero.GetComponent<CapsuleCollider>().height * 0.5f;
                    if (Vector3.Distance(hero.transform.position + ((Vector3)(Vector3.up * num3)), head.position - ((Vector3)((Vector3.up * 1.5f) * size))) < (num + num3))
                    {
                        return hero;
                    }
                }
            }
            return null;
        }
    }
}
