using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Constants;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings;
using System.Collections;
using UnityEngine;

/// <summary>
/// The logic for a blade that has been thrown by the <see cref="BladeThrowSkill"/>.
/// </summary>
public class ThrownBlade : Photon.MonoBehaviour
{

    private const float BladeRotationSpeed = 1000f; // In degrees

    private int viewID;
    private string ownerName;
    private int myTeam;

    private Vector3 oldP;
    private Vector3 velocity;

    [PunRPC]
    public void InitRPC(int viewID, Vector3 pos, int myTeam)
    {
        base.transform.position = pos;
        this.oldP = base.transform.position;
        this.myTeam = myTeam;
    }

    private void Start()
    {
        if (!base.transform.root.gameObject.GetPhotonView().isMine)
        {
            base.enabled = false;
            return;
        }
        if (base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>() != null)
        {
            this.viewID = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID;
            this.ownerName = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().titanName;
        }
    }
    private void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
            transform.position += velocity * Time.deltaTime;
            transform.Rotate(new Vector3(0, 0, 1), BladeRotationSpeed * Time.deltaTime);

            LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer() | Layers.Player.ToLayer() | Layers.EnemyAABB.ToLayer();
            foreach (RaycastHit hit in Physics.SphereCastAll(base.transform.position, base.transform.lossyScale.x / 2, base.transform.position - this.oldP, Vector3.Distance(base.transform.position, this.oldP), (int) mask))
            {
                if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Players" && GameSettings.PvP.Mode != PvpMode.Disabled)
                {
                    GameObject hero = hit.collider.gameObject.transform.root.gameObject;
                    if (!hero.GetComponent<Hero>().IsInvincible)
                    {
                        if (hero.GetComponent<ErenTitan>() != null)
                        {
                            if (!hero.GetComponent<ErenTitan>().isHit)
                            {
                                hero.GetComponent<ErenTitan>().hitByTitan();
                            }
                        }
                        else if ((hero.GetComponent<Hero>() != null))
                        {
                            this.HitPlayer(hero);
                        }
                    }
                }
            }
            this.oldP = base.transform.position;
        }
    }

    private void HitPlayer(GameObject hero)
    {
        if (((hero != null) && !hero.GetComponent<Hero>().HasDied()))
        {
            if ((!hero.GetComponent<Hero>().HasDied()) && !hero.GetComponent<Hero>().IsGrabbed)
            {
                hero.GetComponent<Hero>().MarkDie();
                Debug.Log("blade hit player " + ownerName);
                object[] parameters = new object[] { (velocity.normalized * 1000f) + (Vector3.up * 50f), false, viewID, ownerName, true };
                hero.GetComponent<Hero>().photonView.RPC(nameof(Hero.NetDie), PhotonTargets.All, parameters);
            }
        }
    }

    public void SetVelocity(Vector3 vel)
    {
        velocity = vel;
    }

}

