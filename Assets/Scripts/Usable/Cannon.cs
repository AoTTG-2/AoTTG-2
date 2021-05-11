using Assets.Scripts;
using Assets.Scripts.UI.Input;
using System;
using Assets.Scripts.Characters.Humans;
using UnityEngine;

public class Cannon : Photon.MonoBehaviour
{
    public Transform ballPoint;
    public Transform barrel;
    private Quaternion correctBarrelRot = Quaternion.identity;
    private Vector3 correctPlayerPos = Vector3.zero;
    private Quaternion correctPlayerRot = Quaternion.identity;
    public float currentRot = 0f;
    public Transform firingPoint;
    public bool isCannonGround;
    public GameObject myCannonBall;
    public LineRenderer myCannonLine;
    public Hero myHero;
    public string settings;
    public float SmoothingDelay = 5f;

    public void Awake()
    {
        if (base.photonView != null)
        {
            base.photonView.observed = this;
            this.barrel = base.transform.Find("Barrel");
            this.correctPlayerPos = base.transform.position;
            this.correctPlayerRot = base.transform.rotation;
            this.correctBarrelRot = this.barrel.rotation;
            if (base.photonView.isMine)
            {
                this.firingPoint = this.barrel.Find("FiringPoint");
                this.ballPoint = this.barrel.Find("BallPoint");
                this.myCannonLine = this.ballPoint.GetComponent<LineRenderer>();
                if (base.gameObject.name.Contains("CannonGround"))
                {
                    this.isCannonGround = true;
                }
            }
            if (PhotonNetwork.isMasterClient)
            {
                PhotonPlayer owner = base.photonView.owner;
                if (FengGameManagerMKII.instance.allowedToCannon.ContainsKey(owner.ID))
                {
                    this.settings = FengGameManagerMKII.instance.allowedToCannon[owner.ID].settings;
                    base.photonView.RPC(nameof(SetSize), PhotonTargets.All, new object[] { this.settings });
                    int viewID = FengGameManagerMKII.instance.allowedToCannon[owner.ID].viewID;
                    FengGameManagerMKII.instance.allowedToCannon.Remove(owner.ID);
                    CannonPropRegion component = PhotonView.Find(viewID).gameObject.GetComponent<CannonPropRegion>();
                    if (component != null)
                    {
                        component.disabled = true;
                        component.destroyed = true;
                        PhotonNetwork.Destroy(component.gameObject);
                    }
                }
                else if (!(owner.isLocal || FengGameManagerMKII.instance.restartingMC))
                {
                    FengGameManagerMKII.instance.kickPlayerRC(owner, false, "spawning cannon without request.");
                }
            }
        }
    }

    public void Fire()
    {
        if (this.myHero.skillCDDuration <= 0f)
        {
            foreach (EnemyCheckCollider collider in PhotonNetwork.Instantiate("FX/boom2", this.firingPoint.position, this.firingPoint.rotation, 0).GetComponentsInChildren<EnemyCheckCollider>())
            {
                collider.dmg = 0;
            }
            this.myCannonBall = PhotonNetwork.Instantiate("RCAsset/CannonBallObject", this.ballPoint.position, this.firingPoint.rotation, 0);
            this.myCannonBall.GetComponent<Rigidbody>().velocity = (Vector3) (this.firingPoint.forward * 300f);
            this.myCannonBall.GetComponent<CannonBall>().myHero = this.myHero;
            this.myHero.skillCDDuration = 3.5f;
        }
    }

    public void OnDestroy()
    {
        if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.instance.isRestarting)
        {
            string[] strArray = this.settings.Split(new char[] { ',' });
            if (strArray[0] == "photon")
            {
                if (strArray.Length > 15)
                {
                    GameObject go = PhotonNetwork.Instantiate("RCAsset/" + strArray[1] + "Prop", new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[0x10]), Convert.ToSingle(strArray[0x11]), Convert.ToSingle(strArray[0x12])), 0);
                    go.GetComponent<CannonPropRegion>().settings = this.settings;
                    go.GetPhotonView().RPC(nameof(SetSize), PhotonTargets.AllBuffered, new object[] { this.settings });
                }
                else
                {
                    PhotonNetwork.Instantiate("RCAsset/" + strArray[1] + "Prop", new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4])), new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])), 0).GetComponent<CannonPropRegion>().settings = this.settings;
                }
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(base.transform.position);
            stream.SendNext(base.transform.rotation);
            stream.SendNext(this.barrel.rotation);
        }
        else
        {
            this.correctPlayerPos = (Vector3) stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion) stream.ReceiveNext();
            this.correctBarrelRot = (Quaternion) stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void SetSize(string settings, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            string[] strArray = settings.Split(new char[] { ',' });
            if (strArray.Length > 15)
            {
                float a = 1f;
                GameObject gameObject = null;
                gameObject = base.gameObject;
                if (strArray[2] != "default")
                {
                    if (strArray[2].StartsWith("transparent"))
                    {
                        float num2;
                        if (float.TryParse(strArray[2].Substring(11), out num2))
                        {
                            a = num2;
                        }
                        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
                        {
                            renderer.material = FengGameManagerMKII.instance.RcLegacy.GetMaterial("transparent");
                            if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                            {
                                renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                            }
                        }
                    }
                    else
                    {
                        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
                        {
                            if (!renderer.name.Contains("Line Renderer"))
                            {
                                renderer.material = FengGameManagerMKII.instance.RcLegacy.GetMaterial(strArray[2]);
                                if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                {
                                    renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                }
                            }
                        }
                    }
                }
                float x = gameObject.transform.localScale.x * Convert.ToSingle(strArray[3]);
                x -= 0.001f;
                float y = gameObject.transform.localScale.y * Convert.ToSingle(strArray[4]);
                float z = gameObject.transform.localScale.z * Convert.ToSingle(strArray[5]);
                gameObject.transform.localScale = new Vector3(x, y, z);
                if (strArray[6] != "0")
                {
                    Color color = new Color(Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), a);
                    foreach (MeshFilter filter in gameObject.GetComponentsInChildren<MeshFilter>())
                    {
                        Mesh mesh = filter.mesh;
                        Color[] colorArray = new Color[mesh.vertexCount];
                        for (int i = 0; i < mesh.vertexCount; i++)
                        {
                            colorArray[i] = color;
                        }
                        mesh.colors = colorArray;
                    }
                }
            }
        }
    }

    public void Update()
    {
        if (!base.photonView.isMine)
        {
            base.transform.position = Vector3.Lerp(base.transform.position, this.correctPlayerPos, Time.deltaTime * this.SmoothingDelay);
            base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.correctPlayerRot, Time.deltaTime * this.SmoothingDelay);
            this.barrel.rotation = Quaternion.Lerp(this.barrel.rotation, this.correctBarrelRot, Time.deltaTime * this.SmoothingDelay);
        }
        else
        {
            Vector3 vector = new Vector3(0f, -30f, 0f);
            Vector3 position = this.ballPoint.position;
            Vector3 vector3 = (Vector3) (this.ballPoint.forward * 300f);
            float num = 40f / vector3.magnitude;
            this.myCannonLine.SetWidth(0.5f, 40f);
            this.myCannonLine.SetVertexCount(100);
            for (int i = 0; i < 100; i++)
            {
                this.myCannonLine.SetPosition(i, position);
                position += (Vector3) ((vector3 * num) + (((0.5f * vector) * num) * num));
                vector3 += (Vector3) (vector * num);
            }
            float num3 = 30f;
            if (InputManager.Key(InputCannon.Slow))
            {
                num3 = 5f;
            }
            if (this.isCannonGround)
            {
                if (InputManager.Key(InputCannon.Up))
                {
                    if (this.currentRot <= 32f)
                    {
                        this.currentRot += Time.deltaTime * num3;
                        this.barrel.Rotate(new Vector3(0f, 0f, Time.deltaTime * num3));
                    }
                }
                else if (InputManager.Key(InputCannon.Down) && (this.currentRot >= -18f))
                {
                    this.currentRot += Time.deltaTime * -num3;
                    this.barrel.Rotate(new Vector3(0f, 0f, Time.deltaTime * -num3));
                }
                if (InputManager.Key(InputCannon.Left))
                {
                    base.transform.Rotate(new Vector3(0f, Time.deltaTime * -num3, 0f));
                }
                else if (InputManager.Key(InputCannon.Right))
                {
                    base.transform.Rotate(new Vector3(0f, Time.deltaTime * num3, 0f));
                }
            }
            else
            {
                if (InputManager.Key(InputCannon.Up))
                {
                    if (this.currentRot >= -50f)
                    {
                        this.currentRot += Time.deltaTime * -num3;
                        this.barrel.Rotate(new Vector3(Time.deltaTime * -num3, 0f, 0f));
                    }
                }
                else if (InputManager.Key(InputCannon.Down) && (this.currentRot <= 40f))
                {
                    this.currentRot += Time.deltaTime * num3;
                    this.barrel.Rotate(new Vector3(Time.deltaTime * num3, 0f, 0f));
                }
                if (InputManager.Key(InputCannon.Left))
                {
                    base.transform.Rotate(new Vector3(0f, Time.deltaTime * -num3, 0f));
                }
                else if (InputManager.Key(InputCannon.Right))
                {
                    base.transform.Rotate(new Vector3(0f, Time.deltaTime * num3, 0f));
                }
            }
            if (InputManager.Key(InputCannon.Shoot))
            {
                this.Fire();
            }
            else if (InputManager.KeyDown(InputCannon.Mount))
            {
                if (this.myHero != null)
                {
                    this.myHero.isCannon = false;
                    this.myHero.myCannonRegion = null;
                    Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(this.myHero.gameObject, true, false);
                    this.myHero.Rigidbody.velocity = Vector3.zero;
                    this.myHero.photonView.RPC(nameof(Hero.ReturnFromCannon), PhotonTargets.Others, new object[0]);
                    this.myHero.skillCDLast = this.myHero.skillCDLastCannon;
                    this.myHero.skillCDDuration = this.myHero.skillCDLast;
                }
                PhotonNetwork.Destroy(base.gameObject);
            }
        }
    }
}

