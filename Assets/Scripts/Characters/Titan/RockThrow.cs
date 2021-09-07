using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Attacks;
using UnityEngine;

/// <summary>
/// The logic for a rock that has been thrown by the <see cref="RockThrowAttack"/>. Needs to be refactored to improve performance
/// </summary>
public class RockThrow : Photon.MonoBehaviour
{
    private bool launched;
    private Vector3 oldP;
    private Vector3 r;
    private Vector3 v;

    private void explore()
    {
        GameObject obj2;
        if (PhotonNetwork.isMasterClient)
        {
            obj2 = PhotonNetwork.Instantiate("FX/boom6", base.transform.position, base.transform.rotation, 0);
            if (base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>() != null)
            {
                obj2.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID;
                obj2.GetComponent<EnemyfxIDcontainer>().titanName = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().titanName;
            }
        }
        else
        {
            obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("FX/boom6"), base.transform.position, base.transform.rotation);
        }
        obj2.transform.localScale = base.transform.localScale;
        float b = 1f - (Vector3.Distance(GameObject.Find("MainCamera").transform.position, obj2.transform.position) * 0.05f);
        b = Mathf.Min(1f, b);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().StartShake(b, b, 0.95f);
        PhotonNetwork.Destroy(base.photonView);
    }

    private void hitPlayer(GameObject hero)
    {
        if (((hero != null) && !hero.GetComponent<Hero>().HasDied()) && !hero.GetComponent<Hero>().IsInvincible)
        {
            if ((!hero.GetComponent<Hero>().HasDied()) && !hero.GetComponent<Hero>().IsGrabbed)
            {
                hero.GetComponent<Hero>().MarkDie();
                int myOwnerViewID = -1;
                string titanName = string.Empty;
                if (base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>() != null)
                {
                    myOwnerViewID = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID;
                    titanName = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().titanName;
                }
                Debug.Log("rock hit player " + titanName);
                object[] parameters = new object[] { (Vector3) ((this.v.normalized * 1000f) + (Vector3.up * 50f)), false, myOwnerViewID, titanName, true };
                hero.GetComponent<Hero>().photonView.RPC(nameof(Hero.NetDie), PhotonTargets.All, parameters);
            }
        }
    }

    [PunRPC]
    public void initRPC(int viewID, Vector3 scale, Vector3 pos, float level)
    {
        GameObject gameObject = PhotonView.Find(viewID).gameObject;
        Transform transform = gameObject.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
        base.transform.localScale = gameObject.transform.localScale;
        base.transform.parent = transform;
        base.transform.localPosition = pos;
    }

    public void launch(Vector3 v1)
    {
        this.launched = true;
        this.oldP = base.transform.position;
        this.v = v1;
        if (PhotonNetwork.isMasterClient)
        {
            object[] parameters = new object[] { this.v, this.oldP };
            base.photonView.RPC(nameof(launchRPC), PhotonTargets.Others, parameters);
        }
    }

    [PunRPC]
    private void launchRPC(Vector3 v, Vector3 p)
    {
        this.launched = true;
        Vector3 vector = p;
        base.transform.position = vector;
        this.oldP = vector;
        base.transform.parent = null;
        this.launch(v);
    }

    private void Start()
    {
        this.r = new Vector3(UnityEngine.Random.Range((float) -5f, (float) 5f), UnityEngine.Random.Range((float) -5f, (float) 5f), UnityEngine.Random.Range((float) -5f, (float) 5f));
    }

    private void Update()
    {
        if (this.launched)
        {
            base.transform.Rotate(this.r);
            this.v -= (Vector3) ((20f * Vector3.up) * Time.deltaTime);
            Transform transform = base.transform;
            transform.position += (Vector3) (this.v * Time.deltaTime);
            if (PhotonNetwork.isMasterClient)
            {
                LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
                LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("Players");
                LayerMask mask3 = ((int) 1) << LayerMask.NameToLayer("EnemyAABB");
                LayerMask mask4 = (mask2 | mask) | mask3;
                foreach (RaycastHit hit in Physics.SphereCastAll(base.transform.position, 2.5f * base.transform.lossyScale.x, base.transform.position - this.oldP, Vector3.Distance(base.transform.position, this.oldP), (int) mask4))
                {
                    //if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "EnemyAABB")
                    //{
                    //    GameObject gameObject = hit.collider.gameObject.transform.root.gameObject;
                    //    if ((gameObject.GetComponent<MindlessTitan>() != null)) // && !gameObject.GetComponent<MindlessTitan>().hasDie)
                    //    {
                    //        gameObject.GetComponent<MindlessTitan>().OnAnkleHit(photonView.viewID, 100);
                    //        Vector3 position = base.transform.position;
                    //        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    //        {
                    //            gameObject.GetComponent<MindlessTitan>().OnAnkleHit(photonView.viewID, 100);
                    //        }
                    //        else
                    //        {
                    //            if ((base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>() != null) && (PhotonView.Find(base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID) != null))
                    //            {
                    //                Vector3 vector3 = PhotonView.Find(base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID).transform.position;
                    //            }
                    //            //gameObject.GetComponent<Hero>().photonView.RPC("hitAnkleRPC", PhotonTargets.All, new object[0]);
                    //        }
                    //    }
                    //    this.explore();
                    //}
                    if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Players")
                    {
                        GameObject hero = hit.collider.gameObject.transform.root.gameObject;
                        if (hero.GetComponent<ErenTitan>() != null)
                        {
                            if (!hero.GetComponent<ErenTitan>().isHit)
                            {
                                hero.GetComponent<ErenTitan>().hitByTitan();
                            }
                        }
                        else if ((hero.GetComponent<Hero>() != null) && !hero.GetComponent<Hero>().IsInvincible)
                        {
                            this.hitPlayer(hero);
                        }
                    }
                    else if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Ground")
                    {
                        this.explore();
                    }
                }
                this.oldP = base.transform.position;
            }
        }
    }
}

