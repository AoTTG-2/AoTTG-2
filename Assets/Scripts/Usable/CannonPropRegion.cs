using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonPropRegion : Photon.MonoBehaviour
{
    public bool destroyed;
    public bool disabled;
    public string settings;
    public Hero storedHero;

    private static readonly Dictionary<Cannon.Type, string> PrefabNameByType = new Dictionary<Cannon.Type, string>
    {
        { Cannon.Type.Ground, "CannonGround" },
        { Cannon.Type.Wall, "CannonWall" }
    };

    [SerializeField]
    private Cannon.Type type = Cannon.Type.Ground;

    [SerializeField]
    private bool autoGenerateSettings = true;

    [PunRPC]
    public void RequestControlRPC(int viewID, PhotonMessageInfo info)
    {
        if (!((!base.photonView.isMine || !PhotonNetwork.isMasterClient) || this.disabled))
        {
            Hero requestingHero = PhotonView.Find(viewID).gameObject.GetComponent<Hero>();
            if (requestingHero != null
                && requestingHero.photonView.owner == info.sender
                && !FengGameManagerMKII.instance.allowedToCannon.ContainsKey(info.sender.ID))
            {
                this.disabled = true;
                base.StartCoroutine(this.WaitAndEnable());
                FengGameManagerMKII.instance.allowedToCannon.Add(info.sender.ID,
                    new CannonValues(base.photonView.viewID, this.settings));
                requestingHero.photonView.RPC("SpawnCannonRPC",
                    info.sender,
                    new object[] { this.settings });
            }
        }
    }

    [PunRPC]
    public void SetSize(string settings, PhotonMessageInfo info)
    {
        if (info.sender.IsMasterClient)
        {
            string[] strArray = settings.Split(new char[] { ',' });
            if (strArray.Length > 15)
            {
                float a = 1f;
                GameObject gameObject = base.gameObject;
                if (strArray[2] != "default")
                {
                    if (strArray[2].StartsWith("transparent"))
                    {
                        float foo;
                        if (float.TryParse(strArray[2].Substring(11), out foo))
                            a = foo;

                        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
                        {
                            renderer.material = (Material)FengGameManagerMKII.RCassets.LoadAsset("transparent");
                            if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                            {
                                renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]),
                                    renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                            }
                        }
                    }
                    else
                    {
                        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
                        {
                            renderer.material = (Material)FengGameManagerMKII.RCassets.LoadAsset(strArray[2]);
                            if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                            {
                                renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]),
                                    renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
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
                    Color color = new Color(Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]),
                        Convert.ToSingle(strArray[9]), a);
                    foreach (MeshFilter filter in gameObject.GetComponentsInChildren<MeshFilter>())
                    {
                        Mesh mesh = filter.mesh;
                        Color[] colorArray = new Color[mesh.vertexCount];
                        for (int i = 0; i < mesh.vertexCount; i++)
                            colorArray[i] = color;

                        mesh.colors = colorArray;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        GameObject other = collider.transform.root.gameObject;
        if (other.layer == 8 && other.GetPhotonView().isMine)
        {
            Hero otherHero = other.GetComponent<Hero>();
            if (otherHero != null && !otherHero.isCannon)
            {
                if (otherHero.myCannonRegion != null)
                    otherHero.myCannonRegion.storedHero = null;

                otherHero.myCannonRegion = this;
                this.storedHero = otherHero;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        GameObject other = collider.transform.root.gameObject;
        if (other.layer == 8 && other.GetPhotonView().isMine)
        {
            Hero otherHero = other.GetComponent<Hero>();
            if (otherHero != null && this.storedHero != null && otherHero == this.storedHero)
            {
                otherHero.myCannonRegion = null;
                otherHero.ClearPopup();
                this.storedHero = null;
            }
        }
    }

    private void Start()
    {
        if ((int)FengGameManagerMKII.settings[0x40] >= 100)
            base.GetComponent<Collider>().enabled = false;
    }

    private IEnumerator WaitAndEnable()
    {
        yield return new WaitForSeconds(5f);
        if (!this.destroyed)
            this.disabled = false;
    }

    private void OnDestroy()
    {
        if (this.storedHero != null)
        {
            this.storedHero.myCannonRegion = null;
            this.storedHero.ClearPopup();
        }
    }

    private void OnValidate()
    {
        if (this.autoGenerateSettings)
        {
            var pos = this.transform.position;
            var rot = this.transform.rotation;
            var prefabName = PrefabNameByType[this.type];
            this.settings = $"photon,{prefabName},{pos.x},{pos.y},{pos.z},{rot.x},{rot.y},{rot.z},{rot.w}";
        }
    }
}