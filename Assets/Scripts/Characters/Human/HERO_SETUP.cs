using UnityEngine;

public class HERO_SETUP : MonoBehaviour
{
    private ArmatureData armatureData;

    public string aniname;
    public float anitime;
    public bool change;
    public GameObject chest_info;
    public int currentOne;
    public SkinnedMeshRenderer[][] elements;
    public bool isDeadBody;
    public HeroCostume myCostume;
    public GameObject part_arm_l;
    public GameObject part_arm_r;
    public GameObject part_asset_1;
    public GameObject part_asset_2;
    public GameObject part_brand_1;
    public GameObject part_brand_2;
    public GameObject part_brand_3;
    public GameObject part_brand_4;
    public GameObject part_cape;
    public GameObject part_chest;
    public GameObject part_chest_1;
    public GameObject part_chest_2;
    public GameObject part_chest_3;
    public GameObject part_eye;
    public GameObject part_face;
    public GameObject part_glass;
    public GameObject part_hair;
    public GameObject part_hair_1;
    public GameObject part_hair_2;
    public GameObject part_hand_l;
    public GameObject part_hand_r;
    public GameObject part_head;
    public GameObject part_leg;
    public GameObject part_upper_body;
    public GameObject reference;
    public float timer;

    private void Awake()
    {
        armatureData = gameObject.GetComponent<ArmatureData>();

        this.part_head.transform.parent = armatureData.head.transform;
        reference = this.transform.gameObject;
    }

    public void createCape2()
    {
        if (!this.isDeadBody)
        {
            ClothFactory.DisposeObject(this.part_cape);
            if (this.myCostume.cape_mesh.Length > 0)
            {
                this.part_cape = ClothFactory.GetCape(this.reference, "Character/" + this.myCostume.cape_mesh, CharacterMaterials.materials[this.myCostume.brand_texture]);
            }
        }
    }

    public void createFace()
    {
        this.part_face = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character/character_face"));
        this.part_face.transform.position = this.part_head.transform.position;
        this.part_face.transform.rotation = this.part_head.transform.rotation;
        this.part_face.transform.parent = armatureData.head.transform;
    }

    public void createGlass()
    {
        this.part_glass = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character/glass"));
        this.part_glass.transform.position = this.part_head.transform.position;
        this.part_glass.transform.rotation = this.part_head.transform.rotation;
        this.part_glass.transform.parent = armatureData.head.transform;
    }

    public void createHair2()
    {
        UnityEngine.Object.Destroy(this.part_hair);
        if (!this.isDeadBody)
        {
            ClothFactory.DisposeObject(this.part_hair_1);
        }
        if (this.myCostume.hair_mesh != string.Empty)
        {
            this.part_hair = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character/" + this.myCostume.hair_mesh));
            this.part_hair.transform.position = this.part_head.transform.position;
            this.part_hair.transform.rotation = this.part_head.transform.rotation;
            this.part_hair.transform.parent = armatureData.head.transform;
            this.part_hair.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.hairInfo.texture];
            this.part_hair.GetComponent<Renderer>().material.color = this.myCostume.hair_color;
        }
        if ((this.myCostume.hair_1_mesh.Length > 0) && !this.isDeadBody)
        {
            string name = "Character/" + this.myCostume.hair_1_mesh;
            Material material = CharacterMaterials.materials[this.myCostume.hairInfo.texture];
            this.part_hair_1 = ClothFactory.GetHair(this.reference, name, material, this.myCostume.hair_color);
        }
    }

    public void createHead2()
    {
        UnityEngine.Object.Destroy(this.part_eye);
        UnityEngine.Object.Destroy(this.part_face);
        UnityEngine.Object.Destroy(this.part_glass);
        UnityEngine.Object.Destroy(this.part_hair);
        if (!this.isDeadBody)
        {
            ClothFactory.DisposeObject(this.part_hair_1);
        }
        this.createHair2();
        if (this.myCostume.eye_mesh.Length > 0)
        {
            this.part_eye = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character/" + this.myCostume.eye_mesh));
            this.part_eye.transform.position = this.part_head.transform.position;
            this.part_eye.transform.rotation = this.part_head.transform.rotation;
            this.part_eye.transform.parent = armatureData.head.transform;
            this.setFacialTexture(this.part_eye, this.myCostume.eye_texture_id);
        }
        if (this.myCostume.beard_texture_id >= 0)
        {
            this.createFace();
            this.setFacialTexture(this.part_face, this.myCostume.beard_texture_id);
        }
        if (this.myCostume.glass_texture_id >= 0)
        {
            this.createGlass();
            this.setFacialTexture(this.part_glass, this.myCostume.glass_texture_id);
        }
        this.part_head.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.skin_texture];
        this.part_chest.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.skin_texture];
    }

    public void createLeftArm()
    {
        UnityEngine.Object.Destroy(this.part_arm_l);
        if (this.myCostume.arm_l_mesh.Length > 0)
        {
            this.part_arm_l = this.GenerateCloth(this.reference, "Character/" + this.myCostume.arm_l_mesh);
            this.part_arm_l.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.body_texture];
        }
        UnityEngine.Object.Destroy(this.part_hand_l);
        if (this.myCostume.hand_l_mesh.Length > 0)
        {
            this.part_hand_l = this.GenerateCloth(this.reference, "Character/" + this.myCostume.hand_l_mesh);
            this.part_hand_l.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.skin_texture];
        }
    }

    public void createLowerBody()
    {
        this.part_leg.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.body_texture];
    }

    public void createRightArm()
    {
        UnityEngine.Object.Destroy(this.part_arm_r);
        if (this.myCostume.arm_r_mesh.Length > 0)
        {
            this.part_arm_r = this.GenerateCloth(this.reference, "Character/" + this.myCostume.arm_r_mesh);
            this.part_arm_r.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.body_texture];
        }
        UnityEngine.Object.Destroy(this.part_hand_r);
        if (this.myCostume.hand_r_mesh.Length > 0)
        {
            this.part_hand_r = this.GenerateCloth(this.reference, "Character/" + this.myCostume.hand_r_mesh);
            this.part_hand_r.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.skin_texture];
        }
    }

    public void createUpperBody()
    {
        UnityEngine.Object.Destroy(this.part_upper_body);
        UnityEngine.Object.Destroy(this.part_brand_1);
        UnityEngine.Object.Destroy(this.part_brand_2);
        UnityEngine.Object.Destroy(this.part_brand_3);
        UnityEngine.Object.Destroy(this.part_brand_4);
        UnityEngine.Object.Destroy(this.part_chest_1);
        UnityEngine.Object.Destroy(this.part_chest_2);
        if (!this.isDeadBody)
        {
            ClothFactory.DisposeObject(this.part_chest_3);
        }
        this.createCape2();
        if (this.myCostume.part_chest_object_mesh.Length > 0)
        {
            this.part_chest_1 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character/" + this.myCostume.part_chest_object_mesh));
            this.part_chest_1.transform.position = this.chest_info.transform.position;
            this.part_chest_1.transform.rotation = this.chest_info.transform.rotation;
            this.part_chest_1.transform.parent = armatureData.chest.transform;
            this.part_chest_1.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.part_chest_object_texture];
        }
        if (this.myCostume.part_chest_1_object_mesh.Length > 0)
        {
            this.part_chest_2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Character/" + this.myCostume.part_chest_1_object_mesh));
            this.part_chest_2.transform.position = this.chest_info.transform.position;
            this.part_chest_2.transform.rotation = this.chest_info.transform.rotation;
            this.part_chest_2.transform.parent = armatureData.chest.transform;
            this.part_chest_2.transform.parent = armatureData.chest.transform;
            this.part_chest_2.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.part_chest_1_object_texture];
        }
        if ((this.myCostume.part_chest_skinned_cloth_mesh.Length > 0) && !this.isDeadBody)
        {
            this.part_chest_3 = ClothFactory.GetCape(this.reference, "Character/" + this.myCostume.part_chest_skinned_cloth_mesh, CharacterMaterials.materials[this.myCostume.part_chest_skinned_cloth_texture]);
        }
        if (this.myCostume.body_mesh.Length > 0)
        {
            this.part_upper_body = this.GenerateCloth(this.reference, "Character/" + this.myCostume.body_mesh);
            this.part_upper_body.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.body_texture];
        }
        if (this.myCostume.brand1_mesh.Length > 0)
        {
            this.part_brand_1 = this.GenerateCloth(this.reference, "Character/" + this.myCostume.brand1_mesh);
            this.part_brand_1.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.brand_texture];
        }
        if (this.myCostume.brand2_mesh.Length > 0)
        {
            this.part_brand_2 = this.GenerateCloth(this.reference, "Character/" + this.myCostume.brand2_mesh);
            this.part_brand_2.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.brand_texture];
        }
        if (this.myCostume.brand3_mesh.Length > 0)
        {
            this.part_brand_3 = this.GenerateCloth(this.reference, "Character/" + this.myCostume.brand3_mesh);
            this.part_brand_3.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.brand_texture];
        }
        if (this.myCostume.brand4_mesh.Length > 0)
        {
            this.part_brand_4 = this.GenerateCloth(this.reference, "Character/" + this.myCostume.brand4_mesh);
            this.part_brand_4.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.brand_texture];
        }
        this.part_head.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.skin_texture];
        this.part_chest.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.skin_texture];
    }

    public void deleteCharacterComponent()
    {
        UnityEngine.Object.Destroy(this.part_eye);
        UnityEngine.Object.Destroy(this.part_face);
        UnityEngine.Object.Destroy(this.part_glass);
        UnityEngine.Object.Destroy(this.part_hair);
        if (!this.isDeadBody)
        {
            ClothFactory.DisposeObject(this.part_hair_1);
        }
        UnityEngine.Object.Destroy(this.part_upper_body);
        UnityEngine.Object.Destroy(this.part_arm_l);
        UnityEngine.Object.Destroy(this.part_arm_r);
        if (!this.isDeadBody)
        {
            ClothFactory.DisposeObject(this.part_hair_2);
            ClothFactory.DisposeObject(this.part_cape);
        }
        UnityEngine.Object.Destroy(this.part_brand_1);
        UnityEngine.Object.Destroy(this.part_brand_2);
        UnityEngine.Object.Destroy(this.part_brand_3);
        UnityEngine.Object.Destroy(this.part_brand_4);
        UnityEngine.Object.Destroy(this.part_chest_1);
        UnityEngine.Object.Destroy(this.part_chest_2);
        UnityEngine.Object.Destroy(this.part_chest_3);
    }

    public GameObject GenerateCloth(GameObject go, string res)
    {
        try
        {
            if (go.GetComponent<SkinnedMeshRenderer>() == null)
            {
                go.AddComponent<SkinnedMeshRenderer>();
            }
        }
        catch (UnassignedReferenceException e)
        {
            Debug.LogError("We are missing:" + res);
        }

        SkinnedMeshRenderer component = go.GetComponent<SkinnedMeshRenderer>();
        Transform[] bones = component.bones;
        SkinnedMeshRenderer renderer2 = ((GameObject) UnityEngine.Object.Instantiate(Resources.Load(res))).GetComponent<SkinnedMeshRenderer>();
        renderer2.gameObject.transform.parent = component.gameObject.transform.parent;
        renderer2.transform.localPosition = Vector3.zero;
        renderer2.transform.localScale = Vector3.one;
        renderer2.bones = bones;
        renderer2.quality = SkinQuality.Bone4;
        return renderer2.gameObject;
    }

    public void init()
    {
        CharacterMaterials.init();
    }

    public void setCharacterComponent()
    {
        this.createHead2();
        this.createUpperBody();
        this.createLeftArm();
        this.createRightArm();
        this.createLowerBody();
    }

    public void setFacialTexture(GameObject go, int id)
    {
        if (id >= 0)
        {
            go.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.face_texture];
            float num = 0.125f;
            float x = num * ((int) (((float) id) / 8f));
            float y = -0.125f * (id % 8);
            go.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(x, y);
        }
    }

    public void setSkin()
    {
        this.part_head.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.skin_texture];
        this.part_chest.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.skin_texture];
        this.part_hand_l.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.skin_texture];
        this.part_hand_r.GetComponent<Renderer>().material = CharacterMaterials.materials[this.myCostume.skin_texture];
    }
}

