using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class CustomCharacterManager : MonoBehaviour
{
    private int capeId;
    private int[] capeOption;
    public GameObject character;
    private int costumeId = 1;
    private HeroCostume[] costumeOption;
    public HeroCostume currentCostume;
    private string currentSlot = "Set 1";
    private int divisionId;
    private DIVISION[] divisionOption;
    private int eyeId;
    private int[] eyeOption;
    private int faceId;
    private int[] faceOption;
    private int glassId;
    private int[] glassOption;
    public GameObject hairB;
    public GameObject hairG;
    private int hairId;
    private int[] hairOption;
    public GameObject hairR;
    public GameObject labelACL;
    public GameObject labelBLA;
    public GameObject labelCape;
    public GameObject labelCostume;
    public GameObject labelDivision;
    public GameObject labelEye;
    public GameObject labelFace;
    public GameObject labelGAS;
    public GameObject labelGlass;
    public GameObject labelHair;
    public GameObject labelPOINT;
    public GameObject labelPreset;
    public GameObject labelSex;
    public GameObject labelSKILL;
    public GameObject labelSkin;
    public GameObject labelSPD;
    private int presetId;
    private HERO_SETUP setup;
    private int sexId;
    private SEX[] sexOption;
    private int skillId;
    private string[] skillOption;
    private int skinId;
    private int[] skinOption;

    private int calTotalPoints()
    {
        if (this.setup.myCostume != null)
        {
            int num = 0;
            num += this.setup.myCostume.stat.SPD;
            num += this.setup.myCostume.stat.GAS;
            num += this.setup.myCostume.stat.BLA;
            return (num + this.setup.myCostume.stat.ACL);
        }
        return 400;
    }

    private void copyBodyCostume(HeroCostume from, HeroCostume to)
    {
        to.arm_l_mesh = from.arm_l_mesh;
        to.arm_r_mesh = from.arm_r_mesh;
        to.body_mesh = from.body_mesh;
        to.body_texture = from.body_texture;
        to.uniform_type = from.uniform_type;
        to.part_chest_1_object_mesh = from.part_chest_1_object_mesh;
        to.part_chest_1_object_texture = from.part_chest_1_object_texture;
        to.part_chest_object_mesh = from.part_chest_object_mesh;
        to.part_chest_object_texture = from.part_chest_object_texture;
        to.part_chest_skinned_cloth_mesh = from.part_chest_skinned_cloth_mesh;
        to.part_chest_skinned_cloth_texture = from.part_chest_skinned_cloth_texture;
        to.division = from.division;
        to.id = from.id;
        to.costumeId = from.costumeId;
    }

    private void copyCostume(HeroCostume from, HeroCostume to, bool init = false)
    {
        this.copyBodyCostume(from, to);
        to.sex = from.sex;
        to.hair_mesh = from.hair_mesh;
        to.hair_1_mesh = from.hair_1_mesh;
        to.hair_color = new Color(from.hair_color.r, from.hair_color.g, from.hair_color.b);
        to.hairInfo = from.hairInfo;
        to.cape = from.cape;
        to.cape_mesh = from.cape_mesh;
        to.cape_texture = from.cape_texture;
        to.brand1_mesh = from.brand1_mesh;
        to.brand2_mesh = from.brand2_mesh;
        to.brand3_mesh = from.brand3_mesh;
        to.brand4_mesh = from.brand4_mesh;
        to.brand_texture = from.brand_texture;
        to._3dmg_texture = from._3dmg_texture;
        to.face_texture = from.face_texture;
        to.eye_mesh = from.eye_mesh;
        to.glass_mesh = from.glass_mesh;
        to.beard_mesh = from.beard_mesh;
        to.eye_texture_id = from.eye_texture_id;
        to.beard_texture_id = from.beard_texture_id;
        to.glass_texture_id = from.glass_texture_id;
        to.skin_color = from.skin_color;
        to.skin_texture = from.skin_texture;
        to.beard_texture_id = from.beard_texture_id;
        to.hand_l_mesh = from.hand_l_mesh;
        to.hand_r_mesh = from.hand_r_mesh;
        to.mesh_3dmg = from.mesh_3dmg;
        to.mesh_3dmg_gas_l = from.mesh_3dmg_gas_l;
        to.mesh_3dmg_gas_r = from.mesh_3dmg_gas_r;
        to.mesh_3dmg_belt = from.mesh_3dmg_belt;
        to.weapon_l_mesh = from.weapon_l_mesh;
        to.weapon_r_mesh = from.weapon_r_mesh;
        if (init)
        {
            to.stat = new HeroStat();
            to.stat.ACL = 100;
            to.stat.SPD = 100;
            to.stat.GAS = 100;
            to.stat.BLA = 100;
            to.stat.skillId = "mikasa";
        }
        else
        {
            to.stat = new HeroStat();
            to.stat.ACL = from.stat.ACL;
            to.stat.SPD = from.stat.SPD;
            to.stat.GAS = from.stat.GAS;
            to.stat.BLA = from.stat.BLA;
            to.stat.skillId = from.stat.skillId;
        }
    }

    private void CostumeDataToMyID()
    {
        int index = 0;
        for (index = 0; index < this.sexOption.Length; index++)
        {
            if (this.sexOption[index] == this.setup.myCostume.sex)
            {
                this.sexId = index;
                break;
            }
        }
        index = 0;
        while (index < this.eyeOption.Length)
        {
            if (this.eyeOption[index] == this.setup.myCostume.eye_texture_id)
            {
                this.eyeId = index;
                break;
            }
            index++;
        }
        this.faceId = -1;
        for (index = 0; index < this.faceOption.Length; index++)
        {
            if (this.faceOption[index] == this.setup.myCostume.beard_texture_id)
            {
                this.faceId = index;
                break;
            }
        }
        this.glassId = -1;
        for (index = 0; index < this.glassOption.Length; index++)
        {
            if (this.glassOption[index] == this.setup.myCostume.glass_texture_id)
            {
                this.glassId = index;
                break;
            }
        }
        for (index = 0; index < this.hairOption.Length; index++)
        {
            if (this.hairOption[index] == this.setup.myCostume.hairInfo.id)
            {
                this.hairId = index;
                break;
            }
        }
        for (index = 0; index < this.skinOption.Length; index++)
        {
            if (this.skinOption[index] == this.setup.myCostume.skin_color)
            {
                this.skinId = index;
                break;
            }
        }
        if (this.setup.myCostume.cape)
        {
            this.capeId = 1;
        }
        else
        {
            this.capeId = 0;
        }
        index = 0;
        while (index < this.divisionOption.Length)
        {
            if (this.divisionOption[index] == this.setup.myCostume.division)
            {
                this.divisionId = index;
                break;
            }
            index++;
        }
        this.costumeId = this.setup.myCostume.costumeId;
        float r = this.setup.myCostume.hair_color.r;
        float g = this.setup.myCostume.hair_color.g;
        float b = this.setup.myCostume.hair_color.b;
        //this.hairR.GetComponent<UISlider>().sliderValue = r;
        //this.hairG.GetComponent<UISlider>().sliderValue = g;
        //this.hairB.GetComponent<UISlider>().sliderValue = b;
        for (index = 0; index < this.skillOption.Length; index++)
        {
            if (this.skillOption[index] == this.setup.myCostume.stat.skillId)
            {
                this.skillId = index;
                break;
            }
        }
    }

    private void freshLabel()
    {
        //this.labelSex.GetComponent<UILabel>().text = this.sexOption[this.sexId].ToString();
        //this.labelEye.GetComponent<UILabel>().text = "eye_" + this.eyeId.ToString();
        //this.labelFace.GetComponent<UILabel>().text = "face_" + this.faceId.ToString();
        //this.labelGlass.GetComponent<UILabel>().text = "glass_" + this.glassId.ToString();
        //this.labelHair.GetComponent<UILabel>().text = "hair_" + this.hairId.ToString();
        //this.labelSkin.GetComponent<UILabel>().text = "skin_" + this.skinId.ToString();
        //this.labelCostume.GetComponent<UILabel>().text = "costume_" + this.costumeId.ToString();
        //this.labelCape.GetComponent<UILabel>().text = "cape_" + this.capeId.ToString();
        //this.labelDivision.GetComponent<UILabel>().text = this.divisionOption[this.divisionId].ToString();
        //this.labelPOINT.GetComponent<UILabel>().text = "Points: " + ((400 - this.calTotalPoints())).ToString();
        //this.labelSPD.GetComponent<UILabel>().text = "SPD " + this.setup.myCostume.stat.SPD.ToString();
        //this.labelGAS.GetComponent<UILabel>().text = "GAS " + this.setup.myCostume.stat.GAS.ToString();
        //this.labelBLA.GetComponent<UILabel>().text = "BLA " + this.setup.myCostume.stat.BLA.ToString();
        //this.labelACL.GetComponent<UILabel>().text = "ACL " + this.setup.myCostume.stat.ACL.ToString();
        //this.labelSKILL.GetComponent<UILabel>().text = "SKILL " + this.setup.myCostume.stat.skillId.ToString();
    }

    public void LoadData()
    {
        HeroCostume from = CostumeConeveter.LocalDataToHeroCostume(this.currentSlot);
        if (from != null)
        {
            this.copyCostume(from, this.setup.myCostume, false);
            this.setup.deleteCharacterComponent2();
            this.setup.setCharacterComponent();
        }
        this.CostumeDataToMyID();
        this.freshLabel();
    }

    public void nextOption(CreatePart part)
    {
        if (part == CreatePart.Preset)
        {
            this.presetId = this.toNext(this.presetId, HeroCostume.costume.Length, 0);
            this.copyCostume(HeroCostume.costume[this.presetId], this.setup.myCostume, true);
            this.CostumeDataToMyID();
            this.setup.deleteCharacterComponent2();
            this.setup.setCharacterComponent();
            //this.labelPreset.GetComponent<UILabel>().text = HeroCostume.costume[this.presetId].name;
            this.freshLabel();
        }
        else
        {
            this.toOption2(part, true);
        }
    }

    public void nextStatOption(CreateStat type)
    {
        if (type == CreateStat.Skill)
        {
            this.skillId = this.toNext(this.skillId, this.skillOption.Length, 0);
            this.setup.myCostume.stat.skillId = this.skillOption[this.skillId];
            this.character.GetComponent<CharacterCreateAnimationControl>().playAttack(this.setup.myCostume.stat.skillId);
            this.freshLabel();
        }
        else if (this.calTotalPoints() < 400)
        {
            this.setStatPoint(type, 1);
        }
    }

    public void OnHairBChange(float value)
    {
        if (((this.setup != null) && (this.setup.myCostume != null)) && (this.setup.part_hair != null))
        {
            this.setup.myCostume.hair_color = new Color(this.setup.part_hair.GetComponent<Renderer>().material.color.r, this.setup.part_hair.GetComponent<Renderer>().material.color.g, value);
            this.setHairColor();
        }
    }

    public void OnHairGChange(float value)
    {
        if ((this.setup.myCostume != null) && (this.setup.part_hair != null))
        {
            this.setup.myCostume.hair_color = new Color(this.setup.part_hair.GetComponent<Renderer>().material.color.r, value, this.setup.part_hair.GetComponent<Renderer>().material.color.b);
            this.setHairColor();
        }
    }

    public void OnHairRChange(float value)
    {
        if ((this.setup.myCostume != null) && (this.setup.part_hair != null))
        {
            this.setup.myCostume.hair_color = new Color(value, this.setup.part_hair.GetComponent<Renderer>().material.color.g, this.setup.part_hair.GetComponent<Renderer>().material.color.b);
            this.setHairColor();
        }
    }

    public void OnSoltChange(string id)
    {
        this.currentSlot = id;
    }

    public void prevOption(CreatePart part)
    {
        if (part == CreatePart.Preset)
        {
            this.presetId = this.toPrev(this.presetId, HeroCostume.costume.Length, 0);
            this.copyCostume(HeroCostume.costume[this.presetId], this.setup.myCostume, true);
            this.CostumeDataToMyID();
            this.setup.deleteCharacterComponent2();
            this.setup.setCharacterComponent();
            //this.labelPreset.GetComponent<UILabel>().text = HeroCostume.costume[this.presetId].name;
            this.freshLabel();
        }
        else
        {
            this.toOption2(part, false);
        }
    }

    public void prevStatOption(CreateStat type)
    {
        if (type == CreateStat.Skill)
        {
            this.skillId = this.toPrev(this.skillId, this.skillOption.Length, 0);
            this.setup.myCostume.stat.skillId = this.skillOption[this.skillId];
            this.character.GetComponent<CharacterCreateAnimationControl>().playAttack(this.setup.myCostume.stat.skillId);
            this.freshLabel();
        }
        else
        {
            this.setStatPoint(type, -1);
        }
    }

    public void SaveData()
    {
        CostumeConeveter.HeroCostumeToLocalData(this.setup.myCostume, this.currentSlot);
    }

    private void setHairColor()
    {
        if (this.setup.part_hair != null)
        {
            this.setup.part_hair.GetComponent<Renderer>().material.color = this.setup.myCostume.hair_color;
        }
        if (this.setup.part_hair_1 != null)
        {
            this.setup.part_hair_1.GetComponent<Renderer>().material.color = this.setup.myCostume.hair_color;
        }
    }

    private void setStatPoint(CreateStat type, int pt)
    {
        switch (type)
        {
            case CreateStat.SPD:
                this.setup.myCostume.stat.SPD += pt;
                break;

            case CreateStat.GAS:
                this.setup.myCostume.stat.GAS += pt;
                break;

            case CreateStat.BLA:
                this.setup.myCostume.stat.BLA += pt;
                break;

            case CreateStat.ACL:
                this.setup.myCostume.stat.ACL += pt;
                break;
        }
        this.setup.myCostume.stat.SPD = Mathf.Clamp(this.setup.myCostume.stat.SPD, 0x4b, 0x7d);
        this.setup.myCostume.stat.GAS = Mathf.Clamp(this.setup.myCostume.stat.GAS, 0x4b, 0x7d);
        this.setup.myCostume.stat.BLA = Mathf.Clamp(this.setup.myCostume.stat.BLA, 0x4b, 0x7d);
        this.setup.myCostume.stat.ACL = Mathf.Clamp(this.setup.myCostume.stat.ACL, 0x4b, 0x7d);
        this.freshLabel();
    }

    private void Start()
    {
        int num;
        QualitySettings.SetQualityLevel(5, true);
        this.costumeOption = HeroCostume.costumeOption;
        this.setup = this.character.GetComponent<HERO_SETUP>();
        this.setup.init();
        this.setup.myCostume = new HeroCostume();
        this.copyCostume(HeroCostume.costume[2], this.setup.myCostume, false);
        this.setup.myCostume.setMesh2();
        this.setup.setCharacterComponent();
        SEX[] sexArray1 = new SEX[2];
        sexArray1[1] = SEX.FEMALE;
        this.sexOption = sexArray1;
        this.eyeOption = new int[0x1c];
        for (num = 0; num < 0x1c; num++)
        {
            this.eyeOption[num] = num;
        }
        this.faceOption = new int[14];
        for (num = 0; num < 14; num++)
        {
            this.faceOption[num] = num + 0x20;
        }
        this.glassOption = new int[10];
        for (num = 0; num < 10; num++)
        {
            this.glassOption[num] = num + 0x30;
        }
        this.hairOption = new int[11];
        for (num = 0; num < 11; num++)
        {
            this.hairOption[num] = num;
        }
        this.skinOption = new int[3];
        for (num = 0; num < 3; num++)
        {
            this.skinOption[num] = num + 1;
        }
        this.capeOption = new int[2];
        for (num = 0; num < 2; num++)
        {
            this.capeOption[num] = num;
        }
        DIVISION[] divisionArray1 = new DIVISION[4];
        divisionArray1[1] = DIVISION.TheGarrison;
        divisionArray1[2] = DIVISION.TheMilitaryPolice;
        divisionArray1[3] = DIVISION.TheSurveryCorps;
        this.divisionOption = divisionArray1;
        this.skillOption = new string[] { "mikasa", "levi", "sasha", "jean", "marco", "armin", "petra" };
        this.CostumeDataToMyID();
        this.freshLabel();
    }

    private int toNext(int id, int Count, int start = 0)
    {
        id++;
        if (id >= Count)
        {
            id = start;
        }
        id = Mathf.Clamp(id, start, (start + Count) - 1);
        return id;
    }

    public void toOption(CreatePart part, bool next)
    {
        switch (part)
        {
            case CreatePart.Sex:
                this.sexId = !next ? this.toPrev(this.sexId, this.sexOption.Length, 0) : this.toNext(this.sexId, this.sexOption.Length, 0);
                if (this.sexId != 0)
                {
                    this.costumeId = 0;
                    break;
                }
                this.costumeId = 11;
                break;

            case CreatePart.Eye:
                this.eyeId = !next ? this.toPrev(this.eyeId, this.eyeOption.Length, 0) : this.toNext(this.eyeId, this.eyeOption.Length, 0);
                this.setup.myCostume.eye_texture_id = this.eyeId;
                this.setup.setFacialTexture(this.setup.part_eye, this.eyeOption[this.eyeId]);
                goto Label_06AE;

            case CreatePart.Face:
                this.faceId = !next ? this.toPrev(this.faceId, this.faceOption.Length, 0) : this.toNext(this.faceId, this.faceOption.Length, 0);
                this.setup.myCostume.beard_texture_id = this.faceOption[this.faceId];
                if (this.setup.part_face == null)
                {
                    this.setup.createFace();
                }
                this.setup.setFacialTexture(this.setup.part_face, this.faceOption[this.faceId]);
                goto Label_06AE;

            case CreatePart.Glass:
                this.glassId = !next ? this.toPrev(this.glassId, this.glassOption.Length, 0) : this.toNext(this.glassId, this.glassOption.Length, 0);
                this.setup.myCostume.glass_texture_id = this.glassOption[this.glassId];
                if (this.setup.part_glass == null)
                {
                    this.setup.createGlass();
                }
                this.setup.setFacialTexture(this.setup.part_glass, this.glassOption[this.glassId]);
                goto Label_06AE;

            case CreatePart.Hair:
                this.hairId = !next ? this.toPrev(this.hairId, this.hairOption.Length, 0) : this.toNext(this.hairId, this.hairOption.Length, 0);
                if (this.sexId != 0)
                {
                    this.setup.myCostume.hair_mesh = CostumeHair.hairsF[this.hairOption[this.hairId]].hair;
                    this.setup.myCostume.hair_1_mesh = CostumeHair.hairsF[this.hairOption[this.hairId]].hair_1;
                    this.setup.myCostume.hairInfo = CostumeHair.hairsF[this.hairOption[this.hairId]];
                }
                else
                {
                    this.setup.myCostume.hair_mesh = CostumeHair.hairsM[this.hairOption[this.hairId]].hair;
                    this.setup.myCostume.hair_1_mesh = CostumeHair.hairsM[this.hairOption[this.hairId]].hair_1;
                    this.setup.myCostume.hairInfo = CostumeHair.hairsM[this.hairOption[this.hairId]];
                }
                this.setup.createHair2();
                this.setHairColor();
                goto Label_06AE;

            case CreatePart.Skin:
                if (this.setup.myCostume.uniform_type != UNIFORM_TYPE.CasualAHSS)
                {
                    this.skinId = !next ? this.toPrev(this.skinId, 2, 0) : this.toNext(this.skinId, 2, 0);
                }
                else
                {
                    this.skinId = 2;
                }
                this.setup.myCostume.skin_color = this.skinOption[this.skinId];
                this.setup.myCostume.setTexture();
                this.setup.setSkin();
                goto Label_06AE;

            case CreatePart.Costume:
                if (this.setup.myCostume.uniform_type != UNIFORM_TYPE.CasualAHSS)
                {
                    if (this.sexId == 0)
                    {
                        this.costumeId = !next ? this.toPrev(this.costumeId, 0x18, 10) : this.toNext(this.costumeId, 0x18, 10);
                    }
                    else
                    {
                        this.costumeId = !next ? this.toPrev(this.costumeId, 10, 0) : this.toNext(this.costumeId, 10, 0);
                    }
                }
                else
                {
                    this.costumeId = 0x19;
                }
                this.copyBodyCostume(this.costumeOption[this.costumeId], this.setup.myCostume);
                this.setup.myCostume.setMesh2();
                this.setup.myCostume.setTexture();
                this.setup.createUpperBody2();
                this.setup.createLeftArm();
                this.setup.createRightArm();
                this.setup.createLowerBody();
                goto Label_06AE;

            case CreatePart.Cape:
                this.capeId = !next ? this.toPrev(this.capeId, this.capeOption.Length, 0) : this.toNext(this.capeId, this.capeOption.Length, 0);
                this.setup.myCostume.cape = this.capeId == 1;
                this.setup.myCostume.setCape();
                this.setup.myCostume.setTexture();
                this.setup.createCape2();
                goto Label_06AE;

            case CreatePart.Division:
                this.divisionId = !next ? this.toPrev(this.divisionId, this.divisionOption.Length, 0) : this.toNext(this.divisionId, this.divisionOption.Length, 0);
                this.setup.myCostume.division = this.divisionOption[this.divisionId];
                this.setup.myCostume.setTexture();
                this.setup.createUpperBody2();
                goto Label_06AE;

            default:
                goto Label_06AE;
        }
        this.copyCostume(this.costumeOption[this.costumeId], this.setup.myCostume, true);
        this.setup.myCostume.sex = this.sexOption[this.sexId];
        this.character.GetComponent<CharacterCreateAnimationControl>().toStand();
        this.CostumeDataToMyID();
        this.setup.deleteCharacterComponent2();
        this.setup.setCharacterComponent();
    Label_06AE:
        this.freshLabel();
    }

    public void toOption2(CreatePart part, bool next)
    {
        switch (part)
        {
            case CreatePart.Sex:
                this.sexId = !next ? this.toPrev(this.sexId, this.sexOption.Length, 0) : this.toNext(this.sexId, this.sexOption.Length, 0);
                if (this.sexId == 0)
                {
                    this.costumeId = 11;
                }
                else
                {
                    this.costumeId = 0;
                }
                this.copyCostume(this.costumeOption[this.costumeId], this.setup.myCostume, true);
                this.setup.myCostume.sex = this.sexOption[this.sexId];
                this.character.GetComponent<CharacterCreateAnimationControl>().toStand();
                this.CostumeDataToMyID();
                this.setup.deleteCharacterComponent2();
                this.setup.setCharacterComponent();
                goto Label_0750;

            case CreatePart.Eye:
                this.eyeId = !next ? this.toPrev(this.eyeId, this.eyeOption.Length, 0) : this.toNext(this.eyeId, this.eyeOption.Length, 0);
                this.setup.myCostume.eye_texture_id = this.eyeId;
                this.setup.setFacialTexture(this.setup.part_eye, this.eyeOption[this.eyeId]);
                goto Label_0750;

            case CreatePart.Face:
                this.faceId = !next ? this.toPrev(this.faceId, this.faceOption.Length, 0) : this.toNext(this.faceId, this.faceOption.Length, 0);
                this.setup.myCostume.beard_texture_id = this.faceOption[this.faceId];
                if (this.setup.part_face == null)
                {
                    this.setup.createFace();
                }
                this.setup.setFacialTexture(this.setup.part_face, this.faceOption[this.faceId]);
                goto Label_0750;

            case CreatePart.Glass:
                this.glassId = !next ? this.toPrev(this.glassId, this.glassOption.Length, 0) : this.toNext(this.glassId, this.glassOption.Length, 0);
                this.setup.myCostume.glass_texture_id = this.glassOption[this.glassId];
                if (this.setup.part_glass == null)
                {
                    this.setup.createGlass();
                }
                this.setup.setFacialTexture(this.setup.part_glass, this.glassOption[this.glassId]);
                goto Label_0750;

            case CreatePart.Hair:
                this.hairId = !next ? this.toPrev(this.hairId, this.hairOption.Length, 0) : this.toNext(this.hairId, this.hairOption.Length, 0);
                if (this.sexId == 0)
                {
                    this.setup.myCostume.hair_mesh = CostumeHair.hairsM[this.hairOption[this.hairId]].hair;
                    this.setup.myCostume.hair_1_mesh = CostumeHair.hairsM[this.hairOption[this.hairId]].hair_1;
                    this.setup.myCostume.hairInfo = CostumeHair.hairsM[this.hairOption[this.hairId]];
                    break;
                }
                this.setup.myCostume.hair_mesh = CostumeHair.hairsF[this.hairOption[this.hairId]].hair;
                this.setup.myCostume.hair_1_mesh = CostumeHair.hairsF[this.hairOption[this.hairId]].hair_1;
                this.setup.myCostume.hairInfo = CostumeHair.hairsF[this.hairOption[this.hairId]];
                break;

            case CreatePart.Skin:
                if (this.setup.myCostume.uniform_type == UNIFORM_TYPE.CasualAHSS)
                {
                    this.skinId = 2;
                }
                else
                {
                    this.skinId = !next ? this.toPrev(this.skinId, 2, 0) : this.toNext(this.skinId, 2, 0);
                }
                this.setup.myCostume.skin_color = this.skinOption[this.skinId];
                this.setup.myCostume.setTexture();
                this.setup.setSkin();
                goto Label_0750;

            case CreatePart.Costume:
                if (this.setup.myCostume.uniform_type == UNIFORM_TYPE.CasualAHSS)
                {
                    if (this.setup.myCostume.sex == SEX.FEMALE)
                    {
                        this.costumeId = 0x1a;
                    }
                    else if (this.setup.myCostume.sex == SEX.MALE)
                    {
                        this.costumeId = 0x19;
                    }
                }
                else if (this.sexId != 0)
                {
                    this.costumeId = !next ? this.toPrev(this.costumeId, 10, 0) : this.toNext(this.costumeId, 10, 0);
                }
                else
                {
                    this.costumeId = !next ? this.toPrev(this.costumeId, 0x18, 10) : this.toNext(this.costumeId, 0x18, 10);
                }
                this.copyBodyCostume(this.costumeOption[this.costumeId], this.setup.myCostume);
                this.setup.myCostume.setMesh2();
                this.setup.myCostume.setTexture();
                this.setup.createUpperBody2();
                this.setup.createLeftArm();
                this.setup.createRightArm();
                this.setup.createLowerBody();
                goto Label_0750;

            case CreatePart.Cape:
                this.capeId = !next ? this.toPrev(this.capeId, this.capeOption.Length, 0) : this.toNext(this.capeId, this.capeOption.Length, 0);
                this.setup.myCostume.cape = this.capeId == 1;
                this.setup.myCostume.setCape();
                this.setup.myCostume.setTexture();
                this.setup.createCape2();
                goto Label_0750;

            case CreatePart.Division:
                this.divisionId = !next ? this.toPrev(this.divisionId, this.divisionOption.Length, 0) : this.toNext(this.divisionId, this.divisionOption.Length, 0);
                this.setup.myCostume.division = this.divisionOption[this.divisionId];
                this.setup.myCostume.setTexture();
                this.setup.createUpperBody2();
                goto Label_0750;

            default:
                goto Label_0750;
        }
        this.setup.createHair2();
        this.setHairColor();
    Label_0750:
        this.freshLabel();
    }

    private int toPrev(int id, int Count, int start = 0)
    {
        id--;
        if (id < start)
        {
            id = Count - 1;
        }
        id = Mathf.Clamp(id, start, (start + Count) - 1);
        return id;
    }
}

