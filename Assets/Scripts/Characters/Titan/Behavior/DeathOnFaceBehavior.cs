using UnityEngine;

namespace Assets.Scripts.Characters.Titan.Behavior
{
    public class DeathOnFaceBehavior : TitanBehavior
    {
        protected override bool OnChase()
        {
            var hero = this.checkIfHitCrawlerMouth(Titan.TitanBody.Head, 2.2f, Titan.Size);
            if (hero == null) return false;
            {
                Vector3 vector15 = Titan.TitanBody.Chest.position;
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                {
                    hero.Die((Vector3)(((hero.transform.position - vector15) * 15f) * Titan.Size), false);
                    return true;
                }

                if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && Titan.photonView.isMine)
                {
                    if (!hero.HasDied())
                    {
                        hero.MarkDie();
                        hero.photonView.RPC(
                            nameof(hero.netDie),
                            PhotonTargets.All,
                            (hero.transform.position - vector15) * 15f * Titan.Size,
                            true,
                            Titan.photonView.viewID,
                            Titan.name,
                            true);
                        return true;
                    }
                }
            }
            return false;
        }

        private Hero checkIfHitCrawlerMouth(Transform head, float rad, float size)
        {
            float num = rad * size;
            foreach (Hero hero in FengGameManagerMKII.instance.getPlayers())
            {
                if (hero.GetComponent<TITAN_EREN>() == null && !hero.GetComponent<Hero>().IsInvincible())
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
