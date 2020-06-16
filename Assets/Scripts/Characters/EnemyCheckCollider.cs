using UnityEngine;

public class EnemyCheckCollider : Photon.MonoBehaviour
{
    public bool active_me;
    private int count;
    public int dmg = 1;
    public bool isThisBite;

    private void FixedUpdate()
    {
        if (count > 1)
            active_me = false;
        else
            count++;
    }

    private void OnTriggerStay(Collider other)
    {
        var photonView = transform.root.gameObject.GetPhotonView();
        if (((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER) || photonView.isMine) && active_me)
        {
            if (other.gameObject.tag == "playerHitbox")
            {
                float b = 1f - (Vector3.Distance(other.gameObject.transform.position, transform.position) * 0.05f);
                b = Mathf.Min(1f, b);
                HitBox component = other.gameObject.GetComponent<HitBox>();
                var hero = component.transform.root.GetComponent<Hero>();
                if ((component != null) && (component.transform.root != null))
                {
                    if (dmg == 0)
                    {
                        Vector3 vector = component.transform.root.transform.position - transform.position;
                        float num2 = 0f;

                        var sphereCollider = gameObject.GetComponent<SphereCollider>();
                        if (sphereCollider != null)
                            num2 = transform.localScale.x * sphereCollider.radius;

                        var capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
                        if (capsuleCollider != null)
                            num2 = transform.localScale.x * capsuleCollider.height;

                        float num3 = 5f;
                        if (num2 > 0f)
                        {
                            num3 = Mathf.Max((float) 5f, (float) (num2 - vector.magnitude));
                        }
                        var force = vector.normalized * num3 + Vector3.up * 1f;
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                        {
                            hero.blowAway(force);
                        }
                        else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                        {
                            hero.photonView.RPC<Vector3>(hero.blowAway, PhotonTargets.All, force);
                        }
                    }
                    else if (!hero.isInvincible())
                    {
                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                        {
                            if (!hero.isGrabbed)
                            {
                                Vector3 delta = component.transform.root.transform.position - transform.position;
                                hero.die(delta.normalized * b * 1000f + Vector3.up * 50f, isThisBite);
                            }
                        }
                        else if (((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && !hero.HasDied()) && !hero.isGrabbed)
                        {
                            hero.markDie();
                            int myOwnerViewID = -1;
                            string titanName = string.Empty;
                            var enemyfxIDcontainer = transform.root.gameObject.GetComponent<EnemyfxIDcontainer>();
                            if (enemyfxIDcontainer != null)
                            {
                                myOwnerViewID = enemyfxIDcontainer.myOwnerViewID;
                                titanName = enemyfxIDcontainer.titanName;
                            }
                            Vector3 delta = component.transform.root.position - transform.position;
                            object[] parameters = new object[]
                            {
                                delta.normalized * b * 1000f + Vector3.up * 50f,
                                isThisBite,
                                myOwnerViewID,
                                titanName,
                                true
                            };
                            hero.photonView.RPC<Vector3, bool, int, string, bool, PhotonMessageInfo>(hero.netDie, PhotonTargets.All, parameters);
                        }
                    }
                }
            }
            else if (other.gameObject.tag == "erenHitbox")
            {
                var titanEren = other.gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>();
                if (dmg > 0 && !titanEren.isHit)
                    titanEren.hitByTitan();
            }
        }
    }

    private void Start()
    {
        active_me = true;
        count = 0;
    }
}

