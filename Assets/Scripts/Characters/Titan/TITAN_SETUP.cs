using Assets.Scripts;
using Assets.Scripts.Characters.Titan;
using System.Collections;
using Assets.Scripts.Characters.Humans;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Obsolete("AoTTG class which used to contain the titans configuration. Currently it's still used for the hair colors, but it should be refactored.")]
public class TITAN_SETUP : Photon.MonoBehaviour
{
    public GameObject eye;
    private CostumeHair hair;
    private GameObject hair_go_ref;
    private int hairType;
    public bool haseye;
    private GameObject part_hair;
    public int skin;
    public TitanBody TitanBody;

    [SerializeField] private Color[] hairColors = {
        new Color(1f, 0.9f, 0.5f),
        new Color(0.15f, 0.15f, 0.145f),
        new Color(0.295f, 0.295f, 0.275f),
        new Color(0.45f, 0.33f, 0.255f),
        new Color(0.295f, 0.23f, 0.17f),
        new Color(1f, 1f, 1f),
        new Color(0.94f, 0.84f, 0.6f),
        new Color(0.95f, 0.8f, 0.5f),
        new Color(1f, 0.725f, 0.376f)
    };

    private void Awake()
    {
        this.hair_go_ref = new GameObject("Hair Reference");
        this.eye.transform.parent = TitanBody.Head.transform;
        this.hair_go_ref.transform.position = (Vector3) ((this.eye.transform.position + (Vector3.up * 3.5f)) + (base.transform.forward * 5.2f));
        this.hair_go_ref.transform.rotation = this.eye.transform.rotation;
        this.hair_go_ref.transform.RotateAround(this.eye.transform.position, base.transform.right, -20f);
        this.hair_go_ref.transform.localScale = new Vector3(210f, 210f, 210f);
        this.hair_go_ref.transform.parent = TitanBody.Head.transform;
    }

    [PunRPC]
    public IEnumerator loadskinE(int hair, int eye, string hairlink)
    {
        bool iteratorVariable0 = false;
        UnityEngine.Object.Destroy(this.part_hair);
        this.hair = CostumeHair.hairsM[hair];
        this.hairType = hair;
        if (this.hair.hair != string.Empty)
        {
            GameObject iteratorVariable1 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character/" + this.hair.hair));
            iteratorVariable1.transform.parent = this.hair_go_ref.transform.parent;
            iteratorVariable1.transform.position = this.hair_go_ref.transform.position;
            iteratorVariable1.transform.rotation = this.hair_go_ref.transform.rotation;
            iteratorVariable1.transform.localScale = this.hair_go_ref.transform.localScale;
            iteratorVariable1.GetComponent<Renderer>().material = CharacterMaterials.materials[this.hair.texture];
            bool mipmap = true;
            if (((int) FengGameManagerMKII.settings[0x3f]) == 1)
            {
                mipmap = false;
            }
            if ((!hairlink.EndsWith(".jpg") && !hairlink.EndsWith(".png")) && !hairlink.EndsWith(".jpeg"))
            {
                if (hairlink.ToLower() == "transparent")
                {
                    iteratorVariable1.GetComponent<Renderer>().enabled = false;
                }
            }
            else if (!FengGameManagerMKII.linkHash[0].ContainsKey(hairlink))
            {
                WWW link = new WWW(hairlink);
                yield return link;
                Texture2D iteratorVariable4 = RCextensions.loadimage(link, mipmap, 0x30d40);
                link.Dispose();
                if (FengGameManagerMKII.linkHash[0].ContainsKey(hairlink))
                {
                    iteratorVariable1.GetComponent<Renderer>().material = (Material) FengGameManagerMKII.linkHash[0][hairlink];
                }
                else
                {
                    iteratorVariable0 = true;
                    iteratorVariable1.GetComponent<Renderer>().material.mainTexture = iteratorVariable4;
                    FengGameManagerMKII.linkHash[0].Add(hairlink, iteratorVariable1.GetComponent<Renderer>().material);
                    iteratorVariable1.GetComponent<Renderer>().material = (Material) FengGameManagerMKII.linkHash[0][hairlink];
                }
            }
            else
            {
                iteratorVariable1.GetComponent<Renderer>().material = (Material) FengGameManagerMKII.linkHash[0][hairlink];
            }
            this.part_hair = iteratorVariable1;
        }
        if (eye >= 0)
        {
            this.setFacialTexture(this.eye, eye);
        }
        if (iteratorVariable0)
        {
            FengGameManagerMKII.instance.unloadAssets();
        }
    }

    private void setFacialTexture(GameObject go, int id)
    {
        if (id >= 0)
        {
            float num2 = 0.125f;
            float x = num2 * ((int) (((float) id) / 8f));
            float y = -0.25f * (id % 4);
            go.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(x, y);
        }
    }

    public void setHair2()
    {
        int num;
        object[] objArray2;
        if ((((int) FengGameManagerMKII.settings[1]) == 1) && base.photonView.isMine)
        {
            Color color;
            num = UnityEngine.Random.Range(0, 9);
            if (num == 3)
            {
                num = 9;
            }
            int index = this.skin - 70;
            if (((int) FengGameManagerMKII.settings[0x20]) == 1)
            {
                index = UnityEngine.Random.Range(0x10, 20);
            }
            if (((int) FengGameManagerMKII.settings[index]) >= 0)
            {
                num = (int) FengGameManagerMKII.settings[index];
            }
            string hairlink = (string) FengGameManagerMKII.settings[index + 5];
            int eye = UnityEngine.Random.Range(1, 8);
            if (this.haseye)
            {
                eye = 0;
            }
            bool flag2 = false;
            if ((hairlink.EndsWith(".jpg") || hairlink.EndsWith(".png")) || hairlink.EndsWith(".jpeg"))
            {
                flag2 = true;
            }
            if (base.photonView.isMine)
            {
                if (flag2)
                {
                    objArray2 = new object[] { num, eye, hairlink };
                    base.photonView.RPC(nameof(setHairRPC2), PhotonTargets.AllBuffered, objArray2);
                }
                else
                {
                    color = hairColors[Random.Range(0, hairColors.Length)];
                    objArray2 = new object[] { num, eye, color.r, color.g, color.b };
                    base.photonView.RPC(nameof(setHairPRC), PhotonTargets.AllBuffered, objArray2);
                }
            }
        }
        else
        {
            num = UnityEngine.Random.Range(0, CostumeHair.hairsM.Length);
            if (num == 3)
            {
                num = 9;
            }
            UnityEngine.Object.Destroy(this.part_hair);
            this.hairType = num;
            this.hair = CostumeHair.hairsM[num];
            if (this.hair.hair == string.Empty)
            {
                this.hair = CostumeHair.hairsM[9];
                this.hairType = 9;
            }
            this.part_hair = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character/" + this.hair.hair));
            this.part_hair.transform.parent = this.hair_go_ref.transform.parent;
            this.part_hair.transform.position = this.hair_go_ref.transform.position;
            this.part_hair.transform.rotation = this.hair_go_ref.transform.rotation;
            this.part_hair.transform.localScale = this.hair_go_ref.transform.localScale;
            this.part_hair.GetComponent<Renderer>().material = CharacterMaterials.materials[this.hair.texture];
            this.part_hair.GetComponent<Renderer>().material.color = hairColors[Random.Range(0, hairColors.Length)];
            int id = UnityEngine.Random.Range(1, 8);
            this.setFacialTexture(this.eye, id);
            if (base.photonView.isMine)
            {
                objArray2 = new object[] { this.hairType, id, this.part_hair.GetComponent<Renderer>().material.color.r, this.part_hair.GetComponent<Renderer>().material.color.g, this.part_hair.GetComponent<Renderer>().material.color.b };
                base.photonView.RPC(nameof(setHairPRC), PhotonTargets.OthersBuffered, objArray2);
            }
        }
    }

    [PunRPC]
    private void setHairPRC(int type, int eye_type, float c1, float c2, float c3)
    {
        UnityEngine.Object.Destroy(this.part_hair);
        this.hair = CostumeHair.hairsM[type];
        this.hairType = type;
        if (this.hair.hair != string.Empty)
        {
            GameObject obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character/" + this.hair.hair));
            obj2.transform.parent = this.hair_go_ref.transform.parent;
            obj2.transform.position = this.hair_go_ref.transform.position;
            obj2.transform.rotation = this.hair_go_ref.transform.rotation;
            obj2.transform.localScale = this.hair_go_ref.transform.localScale;
            obj2.GetComponent<Renderer>().material = CharacterMaterials.materials[this.hair.texture];
            obj2.GetComponent<Renderer>().material.color = new Color(c1, c2, c3);
            this.part_hair = obj2;
        }
        this.setFacialTexture(this.eye, eye_type);
    }

    [PunRPC]
    public void setHairRPC2(int hair, int eye, string hairlink)
    {
        if (((int) FengGameManagerMKII.settings[1]) == 1)
        {
            base.StartCoroutine(this.loadskinE(hair, eye, hairlink));
        }
    }

    public void setVar(int skin, bool haseye)
    {
        this.skin = skin;
        this.haseye = haseye;
    }
}