using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class HeroCostume
{
    public string _3dmg_texture = string.Empty;
    public string arm_l_mesh = string.Empty;
    public string arm_r_mesh = string.Empty;
    public string beard_mesh = string.Empty;
    public int beard_texture_id = -1;
    public static string[] body_casual_fa_texture;
    public static string[] body_casual_fb_texture;
    public static string[] body_casual_ma_texture;
    public static string[] body_casual_mb_texture;
    public string body_mesh = string.Empty;
    public string body_texture = string.Empty;
    public static string[] body_uniform_fa_texture;
    public static string[] body_uniform_fb_texture;
    public static string[] body_uniform_ma_texture;
    public static string[] body_uniform_mb_texture;
    public string brand_texture = string.Empty;
    public string brand1_mesh = string.Empty;
    public string brand2_mesh = string.Empty;
    public string brand3_mesh = string.Empty;
    public string brand4_mesh = string.Empty;
    public bool cape;
    public string cape_mesh = string.Empty;
    public string cape_texture = string.Empty;
    public static HeroCostume[] costume;
    public int costumeId;
    public static HeroCostume[] costumeOption;
    public DIVISION division;
    public string eye_mesh = string.Empty;
    public int eye_texture_id = -1;
    public string face_texture = string.Empty;
    public string glass_mesh = string.Empty;
    public int glass_texture_id = -1;
    public string hair_1_mesh = string.Empty;
    public Color hair_color = new Color(0.5f, 0.1f, 0f);
    public string hair_mesh = string.Empty;
    public CostumeHair hairInfo;
    public string hand_l_mesh = string.Empty;
    public string hand_r_mesh = string.Empty;
    public int id;
    private static bool inited;
    public string mesh_3dmg = string.Empty;
    public string mesh_3dmg_belt = string.Empty;
    public string mesh_3dmg_gas_l = string.Empty;
    public string mesh_3dmg_gas_r = string.Empty;
    public string name = string.Empty;
    public string part_chest_1_object_mesh = string.Empty;
    public string part_chest_1_object_texture = string.Empty;
    public string part_chest_object_mesh = string.Empty;
    public string part_chest_object_texture = string.Empty;
    public string part_chest_skinned_cloth_mesh = string.Empty;
    public string part_chest_skinned_cloth_texture = string.Empty;
    public SEX sex;
    public int skin_color = 1;
    public string skin_texture = string.Empty;
    public HeroStat stat;
    public UNIFORM_TYPE uniform_type = UNIFORM_TYPE.CasualA;
    public string weapon_l_mesh = string.Empty;
    public string weapon_r_mesh = string.Empty;

    public void checkstat()
    {
        int num = 0;
        num = 0 + this.stat.SPD;
        num += this.stat.GAS;
        num += this.stat.BLA;
        num += this.stat.ACL;
        if (num > 400)
        {
            this.stat.ACL = 100;
            this.stat.BLA = 100;
            this.stat.GAS = 100;
            this.stat.SPD = 100;
        }
    }

    public static void init()
    {
        if (!inited)
        {
            inited = true;
            CostumeHair.init();
            body_uniform_ma_texture = new string[] { "aottg_hero_uniform_ma_1", "aottg_hero_uniform_ma_2", "aottg_hero_uniform_ma_3" };
            body_uniform_fa_texture = new string[] { "aottg_hero_uniform_fa_1", "aottg_hero_uniform_fa_2", "aottg_hero_uniform_fa_3" };
            body_uniform_mb_texture = new string[] { "aottg_hero_uniform_mb_1", "aottg_hero_uniform_mb_2", "aottg_hero_uniform_mb_3", "aottg_hero_uniform_mb_4" };
            body_uniform_fb_texture = new string[] { "aottg_hero_uniform_fb_1", "aottg_hero_uniform_fb_2" };
            body_casual_ma_texture = new string[] { "aottg_hero_casual_ma_1", "aottg_hero_casual_ma_2", "aottg_hero_casual_ma_3" };
            body_casual_fa_texture = new string[] { "aottg_hero_casual_fa_1", "aottg_hero_casual_fa_2", "aottg_hero_casual_fa_3" };
            body_casual_mb_texture = new string[] { "aottg_hero_casual_mb_1", "aottg_hero_casual_mb_2", "aottg_hero_casual_mb_3", "aottg_hero_casual_mb_4" };
            body_casual_fb_texture = new string[] { "aottg_hero_casual_fb_1", "aottg_hero_casual_fb_2" };
            costume = new HeroCostume[0x26];
            costume[0] = new HeroCostume();
            costume[0].name = "annie";
            costume[0].sex = SEX.FEMALE;
            costume[0].uniform_type = UNIFORM_TYPE.UniformB;
            costume[0].part_chest_object_mesh = "character_cap_uniform";
            costume[0].part_chest_object_texture = "aottg_hero_annie_cap_uniform";
            costume[0].cape = true;
            costume[0].body_texture = body_uniform_fb_texture[0];
            costume[0].hairInfo = CostumeHair.hairsF[5];
            costume[0].eye_texture_id = 0;
            costume[0].beard_texture_id = 0x21;
            costume[0].glass_texture_id = -1;
            costume[0].skin_color = 1;
            costume[0].hair_color = new Color(1f, 0.9f, 0.5f);
            costume[0].division = DIVISION.TheMilitaryPolice;
            costume[0].costumeId = 0;
            costume[1] = new HeroCostume();
            costume[1].name = "annie";
            costume[1].sex = SEX.FEMALE;
            costume[1].uniform_type = UNIFORM_TYPE.UniformB;
            costume[1].part_chest_object_mesh = "character_cap_uniform";
            costume[1].part_chest_object_texture = "aottg_hero_annie_cap_uniform";
            costume[1].body_texture = body_uniform_fb_texture[0];
            costume[1].cape = false;
            costume[1].hairInfo = CostumeHair.hairsF[5];
            costume[1].eye_texture_id = 0;
            costume[1].beard_texture_id = 0x21;
            costume[1].glass_texture_id = -1;
            costume[1].skin_color = 1;
            costume[1].hair_color = new Color(1f, 0.9f, 0.5f);
            costume[1].division = DIVISION.TraineesSquad;
            costume[1].costumeId = 0;
            costume[2] = new HeroCostume();
            costume[2].name = "annie";
            costume[2].sex = SEX.FEMALE;
            costume[2].uniform_type = UNIFORM_TYPE.CasualB;
            costume[2].part_chest_object_mesh = "character_cap_casual";
            costume[2].part_chest_object_texture = "aottg_hero_annie_cap_causal";
            costume[2].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[2].part_chest_1_object_texture = body_casual_fb_texture[0];
            costume[2].body_texture = body_casual_fb_texture[0];
            costume[2].cape = false;
            costume[2].hairInfo = CostumeHair.hairsF[5];
            costume[2].eye_texture_id = 0;
            costume[2].beard_texture_id = 0x21;
            costume[2].glass_texture_id = -1;
            costume[2].skin_color = 1;
            costume[2].hair_color = new Color(1f, 0.9f, 0.5f);
            costume[2].costumeId = 1;
            costume[3] = new HeroCostume();
            costume[3].name = "mikasa";
            costume[3].sex = SEX.FEMALE;
            costume[3].uniform_type = UNIFORM_TYPE.UniformB;
            costume[3].body_texture = body_uniform_fb_texture[1];
            costume[3].cape = true;
            costume[3].hairInfo = CostumeHair.hairsF[7];
            costume[3].eye_texture_id = 2;
            costume[3].beard_texture_id = 0x21;
            costume[3].glass_texture_id = -1;
            costume[3].skin_color = 1;
            costume[3].hair_color = new Color(0.15f, 0.15f, 0.145f);
            costume[3].division = DIVISION.TheSurveryCorps;
            costume[3].costumeId = 2;
            costume[4] = new HeroCostume();
            costume[4].name = "mikasa";
            costume[4].sex = SEX.FEMALE;
            costume[4].uniform_type = UNIFORM_TYPE.UniformB;
            costume[4].part_chest_skinned_cloth_mesh = "mikasa_asset_uni";
            costume[4].part_chest_skinned_cloth_texture = body_uniform_fb_texture[1];
            costume[4].body_texture = body_uniform_fb_texture[1];
            costume[4].cape = false;
            costume[4].hairInfo = CostumeHair.hairsF[7];
            costume[4].eye_texture_id = 2;
            costume[4].beard_texture_id = 0x21;
            costume[4].glass_texture_id = -1;
            costume[4].skin_color = 1;
            costume[4].hair_color = new Color(0.15f, 0.15f, 0.145f);
            costume[4].division = DIVISION.TraineesSquad;
            costume[4].costumeId = 3;
            costume[5] = new HeroCostume();
            costume[5].name = "mikasa";
            costume[5].sex = SEX.FEMALE;
            costume[5].uniform_type = UNIFORM_TYPE.CasualB;
            costume[5].part_chest_skinned_cloth_mesh = "mikasa_asset_cas";
            costume[5].part_chest_skinned_cloth_texture = body_casual_fb_texture[1];
            costume[5].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[5].part_chest_1_object_texture = body_casual_fb_texture[1];
            costume[5].body_texture = body_casual_fb_texture[1];
            costume[5].cape = false;
            costume[5].hairInfo = CostumeHair.hairsF[7];
            costume[5].eye_texture_id = 2;
            costume[5].beard_texture_id = 0x21;
            costume[5].glass_texture_id = -1;
            costume[5].skin_color = 1;
            costume[5].hair_color = new Color(0.15f, 0.15f, 0.145f);
            costume[5].costumeId = 4;
            costume[6] = new HeroCostume();
            costume[6].name = "levi";
            costume[6].sex = SEX.MALE;
            costume[6].uniform_type = UNIFORM_TYPE.UniformB;
            costume[6].body_texture = body_uniform_mb_texture[1];
            costume[6].cape = true;
            costume[6].hairInfo = CostumeHair.hairsM[7];
            costume[6].eye_texture_id = 1;
            costume[6].beard_texture_id = -1;
            costume[6].glass_texture_id = -1;
            costume[6].skin_color = 1;
            costume[6].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[6].division = DIVISION.TheSurveryCorps;
            costume[6].costumeId = 11;
            costume[7] = new HeroCostume();
            costume[7].name = "levi";
            costume[7].sex = SEX.MALE;
            costume[7].uniform_type = UNIFORM_TYPE.CasualB;
            costume[7].body_texture = body_casual_mb_texture[1];
            costume[7].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[7].part_chest_1_object_texture = body_casual_mb_texture[1];
            costume[7].cape = false;
            costume[7].hairInfo = CostumeHair.hairsM[7];
            costume[7].eye_texture_id = 1;
            costume[7].beard_texture_id = -1;
            costume[7].glass_texture_id = -1;
            costume[7].skin_color = 1;
            costume[7].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[7].costumeId = 12;
            costume[8] = new HeroCostume();
            costume[8].name = "eren";
            costume[8].sex = SEX.MALE;
            costume[8].uniform_type = UNIFORM_TYPE.UniformB;
            costume[8].body_texture = body_uniform_mb_texture[0];
            costume[8].cape = true;
            costume[8].hairInfo = CostumeHair.hairsM[4];
            costume[8].eye_texture_id = 3;
            costume[8].beard_texture_id = -1;
            costume[8].glass_texture_id = -1;
            costume[8].skin_color = 1;
            costume[8].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[8].division = DIVISION.TheSurveryCorps;
            costume[8].costumeId = 13;
            costume[9] = new HeroCostume();
            costume[9].name = "eren";
            costume[9].sex = SEX.MALE;
            costume[9].uniform_type = UNIFORM_TYPE.UniformB;
            costume[9].body_texture = body_uniform_mb_texture[0];
            costume[9].cape = false;
            costume[9].hairInfo = CostumeHair.hairsM[4];
            costume[9].eye_texture_id = 3;
            costume[9].beard_texture_id = -1;
            costume[9].glass_texture_id = -1;
            costume[9].skin_color = 1;
            costume[9].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[9].division = DIVISION.TraineesSquad;
            costume[9].costumeId = 13;
            costume[10] = new HeroCostume();
            costume[10].name = "eren";
            costume[10].sex = SEX.MALE;
            costume[10].uniform_type = UNIFORM_TYPE.CasualB;
            costume[10].body_texture = body_casual_mb_texture[0];
            costume[10].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[10].part_chest_1_object_texture = body_casual_mb_texture[0];
            costume[10].cape = false;
            costume[10].hairInfo = CostumeHair.hairsM[4];
            costume[10].eye_texture_id = 3;
            costume[10].beard_texture_id = -1;
            costume[10].glass_texture_id = -1;
            costume[10].skin_color = 1;
            costume[10].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[10].costumeId = 14;
            costume[11] = new HeroCostume();
            costume[11].name = "sasha";
            costume[11].sex = SEX.FEMALE;
            costume[11].uniform_type = UNIFORM_TYPE.UniformA;
            costume[11].body_texture = body_uniform_fa_texture[1];
            costume[11].cape = true;
            costume[11].hairInfo = CostumeHair.hairsF[10];
            costume[11].eye_texture_id = 4;
            costume[11].beard_texture_id = 0x21;
            costume[11].glass_texture_id = -1;
            costume[11].skin_color = 1;
            costume[11].hair_color = new Color(0.45f, 0.33f, 0.255f);
            costume[11].division = DIVISION.TheSurveryCorps;
            costume[11].costumeId = 5;
            costume[12] = new HeroCostume();
            costume[12].name = "sasha";
            costume[12].sex = SEX.FEMALE;
            costume[12].uniform_type = UNIFORM_TYPE.UniformA;
            costume[12].body_texture = body_uniform_fa_texture[1];
            costume[12].cape = false;
            costume[12].hairInfo = CostumeHair.hairsF[10];
            costume[12].eye_texture_id = 4;
            costume[12].beard_texture_id = 0x21;
            costume[12].glass_texture_id = -1;
            costume[12].skin_color = 1;
            costume[12].hair_color = new Color(0.45f, 0.33f, 0.255f);
            costume[12].division = DIVISION.TraineesSquad;
            costume[12].costumeId = 5;
            costume[13] = new HeroCostume();
            costume[13].name = "sasha";
            costume[13].sex = SEX.FEMALE;
            costume[13].uniform_type = UNIFORM_TYPE.CasualA;
            costume[13].body_texture = body_casual_fa_texture[1];
            costume[13].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[13].part_chest_1_object_texture = body_casual_fa_texture[1];
            costume[13].cape = false;
            costume[13].hairInfo = CostumeHair.hairsF[10];
            costume[13].eye_texture_id = 4;
            costume[13].beard_texture_id = 0x21;
            costume[13].glass_texture_id = -1;
            costume[13].skin_color = 1;
            costume[13].hair_color = new Color(0.45f, 0.33f, 0.255f);
            costume[13].costumeId = 6;
            costume[14] = new HeroCostume();
            costume[14].name = "hanji";
            costume[14].sex = SEX.FEMALE;
            costume[14].uniform_type = UNIFORM_TYPE.UniformA;
            costume[14].body_texture = body_uniform_fa_texture[2];
            costume[14].cape = true;
            costume[14].hairInfo = CostumeHair.hairsF[6];
            costume[14].eye_texture_id = 5;
            costume[14].beard_texture_id = 0x21;
            costume[14].glass_texture_id = 0x31;
            costume[14].skin_color = 1;
            costume[14].hair_color = new Color(0.45f, 0.33f, 0.255f);
            costume[14].division = DIVISION.TheSurveryCorps;
            costume[14].costumeId = 7;
            costume[15] = new HeroCostume();
            costume[15].name = "hanji";
            costume[15].sex = SEX.FEMALE;
            costume[15].uniform_type = UNIFORM_TYPE.CasualA;
            costume[15].body_texture = body_casual_fa_texture[2];
            costume[15].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[15].part_chest_1_object_texture = body_casual_fa_texture[2];
            costume[15].cape = false;
            costume[15].hairInfo = CostumeHair.hairsF[6];
            costume[15].eye_texture_id = 5;
            costume[15].beard_texture_id = 0x21;
            costume[15].glass_texture_id = 0x31;
            costume[15].skin_color = 1;
            costume[15].hair_color = new Color(0.295f, 0.23f, 0.17f);
            costume[15].costumeId = 8;
            costume[0x10] = new HeroCostume();
            costume[0x10].name = "rico";
            costume[0x10].sex = SEX.FEMALE;
            costume[0x10].uniform_type = UNIFORM_TYPE.UniformA;
            costume[0x10].body_texture = body_uniform_fa_texture[0];
            costume[0x10].cape = true;
            costume[0x10].hairInfo = CostumeHair.hairsF[9];
            costume[0x10].eye_texture_id = 6;
            costume[0x10].beard_texture_id = 0x21;
            costume[0x10].glass_texture_id = 0x30;
            costume[0x10].skin_color = 1;
            costume[0x10].hair_color = new Color(1f, 1f, 1f);
            costume[0x10].division = DIVISION.TheGarrison;
            costume[0x10].costumeId = 9;
            costume[0x11] = new HeroCostume();
            costume[0x11].name = "rico";
            costume[0x11].sex = SEX.FEMALE;
            costume[0x11].uniform_type = UNIFORM_TYPE.CasualA;
            costume[0x11].body_texture = body_casual_fa_texture[0];
            costume[0x11].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[0x11].part_chest_1_object_texture = body_casual_fa_texture[0];
            costume[0x11].cape = false;
            costume[0x11].hairInfo = CostumeHair.hairsF[9];
            costume[0x11].eye_texture_id = 6;
            costume[0x11].beard_texture_id = 0x21;
            costume[0x11].glass_texture_id = 0x30;
            costume[0x11].skin_color = 1;
            costume[0x11].hair_color = new Color(1f, 1f, 1f);
            costume[0x11].costumeId = 10;
            costume[0x12] = new HeroCostume();
            costume[0x12].name = "jean";
            costume[0x12].sex = SEX.MALE;
            costume[0x12].uniform_type = UNIFORM_TYPE.UniformA;
            costume[0x12].body_texture = body_uniform_ma_texture[1];
            costume[0x12].cape = true;
            costume[0x12].hairInfo = CostumeHair.hairsM[6];
            costume[0x12].eye_texture_id = 7;
            costume[0x12].beard_texture_id = -1;
            costume[0x12].glass_texture_id = -1;
            costume[0x12].skin_color = 1;
            costume[0x12].hair_color = new Color(0.94f, 0.84f, 0.6f);
            costume[0x12].division = DIVISION.TheSurveryCorps;
            costume[0x12].costumeId = 15;
            costume[0x13] = new HeroCostume();
            costume[0x13].name = "jean";
            costume[0x13].sex = SEX.MALE;
            costume[0x13].uniform_type = UNIFORM_TYPE.UniformA;
            costume[0x13].body_texture = body_uniform_ma_texture[1];
            costume[0x13].cape = false;
            costume[0x13].hairInfo = CostumeHair.hairsM[6];
            costume[0x13].eye_texture_id = 7;
            costume[0x13].beard_texture_id = -1;
            costume[0x13].glass_texture_id = -1;
            costume[0x13].skin_color = 1;
            costume[0x13].hair_color = new Color(0.94f, 0.84f, 0.6f);
            costume[0x13].division = DIVISION.TraineesSquad;
            costume[0x13].costumeId = 15;
            costume[20] = new HeroCostume();
            costume[20].name = "jean";
            costume[20].sex = SEX.MALE;
            costume[20].uniform_type = UNIFORM_TYPE.CasualA;
            costume[20].body_texture = body_casual_ma_texture[1];
            costume[20].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[20].part_chest_1_object_texture = body_casual_ma_texture[1];
            costume[20].cape = false;
            costume[20].hairInfo = CostumeHair.hairsM[6];
            costume[20].eye_texture_id = 7;
            costume[20].beard_texture_id = -1;
            costume[20].glass_texture_id = -1;
            costume[20].skin_color = 1;
            costume[20].hair_color = new Color(0.94f, 0.84f, 0.6f);
            costume[20].costumeId = 0x10;
            costume[0x15] = new HeroCostume();
            costume[0x15].name = "marco";
            costume[0x15].sex = SEX.MALE;
            costume[0x15].uniform_type = UNIFORM_TYPE.UniformA;
            costume[0x15].body_texture = body_uniform_ma_texture[2];
            costume[0x15].cape = false;
            costume[0x15].hairInfo = CostumeHair.hairsM[8];
            costume[0x15].eye_texture_id = 8;
            costume[0x15].beard_texture_id = -1;
            costume[0x15].glass_texture_id = -1;
            costume[0x15].skin_color = 1;
            costume[0x15].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[0x15].division = DIVISION.TraineesSquad;
            costume[0x15].costumeId = 0x11;
            costume[0x16] = new HeroCostume();
            costume[0x16].name = "marco";
            costume[0x16].sex = SEX.MALE;
            costume[0x16].uniform_type = UNIFORM_TYPE.CasualA;
            costume[0x16].body_texture = body_casual_ma_texture[2];
            costume[0x16].cape = false;
            costume[0x16].hairInfo = CostumeHair.hairsM[8];
            costume[0x16].eye_texture_id = 8;
            costume[0x16].beard_texture_id = -1;
            costume[0x16].glass_texture_id = -1;
            costume[0x16].skin_color = 1;
            costume[0x16].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[0x16].costumeId = 0x12;
            costume[0x17] = new HeroCostume();
            costume[0x17].name = "mike";
            costume[0x17].sex = SEX.MALE;
            costume[0x17].uniform_type = UNIFORM_TYPE.UniformB;
            costume[0x17].body_texture = body_uniform_mb_texture[3];
            costume[0x17].cape = true;
            costume[0x17].hairInfo = CostumeHair.hairsM[9];
            costume[0x17].eye_texture_id = 9;
            costume[0x17].beard_texture_id = 0x20;
            costume[0x17].glass_texture_id = -1;
            costume[0x17].skin_color = 1;
            costume[0x17].hair_color = new Color(0.94f, 0.84f, 0.6f);
            costume[0x17].division = DIVISION.TheSurveryCorps;
            costume[0x17].costumeId = 0x13;
            costume[0x18] = new HeroCostume();
            costume[0x18].name = "mike";
            costume[0x18].sex = SEX.MALE;
            costume[0x18].uniform_type = UNIFORM_TYPE.CasualB;
            costume[0x18].body_texture = body_casual_mb_texture[3];
            costume[0x18].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[0x18].part_chest_1_object_texture = body_casual_mb_texture[3];
            costume[0x18].cape = false;
            costume[0x18].hairInfo = CostumeHair.hairsM[9];
            costume[0x18].eye_texture_id = 9;
            costume[0x18].beard_texture_id = 0x20;
            costume[0x18].glass_texture_id = -1;
            costume[0x18].skin_color = 1;
            costume[0x18].hair_color = new Color(0.94f, 0.84f, 0.6f);
            costume[0x18].division = DIVISION.TheSurveryCorps;
            costume[0x18].costumeId = 20;
            costume[0x19] = new HeroCostume();
            costume[0x19].name = "connie";
            costume[0x19].sex = SEX.MALE;
            costume[0x19].uniform_type = UNIFORM_TYPE.UniformB;
            costume[0x19].body_texture = body_uniform_mb_texture[2];
            costume[0x19].cape = true;
            costume[0x19].hairInfo = CostumeHair.hairsM[10];
            costume[0x19].eye_texture_id = 10;
            costume[0x19].beard_texture_id = -1;
            costume[0x19].glass_texture_id = -1;
            costume[0x19].skin_color = 1;
            costume[0x19].division = DIVISION.TheSurveryCorps;
            costume[0x19].costumeId = 0x15;
            costume[0x1a] = new HeroCostume();
            costume[0x1a].name = "connie";
            costume[0x1a].sex = SEX.MALE;
            costume[0x1a].uniform_type = UNIFORM_TYPE.UniformB;
            costume[0x1a].body_texture = body_uniform_mb_texture[2];
            costume[0x1a].cape = false;
            costume[0x1a].hairInfo = CostumeHair.hairsM[10];
            costume[0x1a].eye_texture_id = 10;
            costume[0x1a].beard_texture_id = -1;
            costume[0x1a].glass_texture_id = -1;
            costume[0x1a].skin_color = 1;
            costume[0x1a].division = DIVISION.TraineesSquad;
            costume[0x1a].costumeId = 0x15;
            costume[0x1b] = new HeroCostume();
            costume[0x1b].name = "connie";
            costume[0x1b].sex = SEX.MALE;
            costume[0x1b].uniform_type = UNIFORM_TYPE.CasualB;
            costume[0x1b].body_texture = body_casual_mb_texture[2];
            costume[0x1b].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[0x1b].part_chest_1_object_texture = body_casual_mb_texture[2];
            costume[0x1b].cape = false;
            costume[0x1b].hairInfo = CostumeHair.hairsM[10];
            costume[0x1b].eye_texture_id = 10;
            costume[0x1b].beard_texture_id = -1;
            costume[0x1b].glass_texture_id = -1;
            costume[0x1b].skin_color = 1;
            costume[0x1b].costumeId = 0x16;
            costume[0x1c] = new HeroCostume();
            costume[0x1c].name = "armin";
            costume[0x1c].sex = SEX.MALE;
            costume[0x1c].uniform_type = UNIFORM_TYPE.UniformA;
            costume[0x1c].body_texture = body_uniform_ma_texture[0];
            costume[0x1c].cape = true;
            costume[0x1c].hairInfo = CostumeHair.hairsM[5];
            costume[0x1c].eye_texture_id = 11;
            costume[0x1c].beard_texture_id = -1;
            costume[0x1c].glass_texture_id = -1;
            costume[0x1c].skin_color = 1;
            costume[0x1c].hair_color = new Color(0.95f, 0.8f, 0.5f);
            costume[0x1c].division = DIVISION.TheSurveryCorps;
            costume[0x1c].costumeId = 0x17;
            costume[0x1d] = new HeroCostume();
            costume[0x1d].name = "armin";
            costume[0x1d].sex = SEX.MALE;
            costume[0x1d].uniform_type = UNIFORM_TYPE.UniformA;
            costume[0x1d].body_texture = body_uniform_ma_texture[0];
            costume[0x1d].cape = false;
            costume[0x1d].hairInfo = CostumeHair.hairsM[5];
            costume[0x1d].eye_texture_id = 11;
            costume[0x1d].beard_texture_id = -1;
            costume[0x1d].glass_texture_id = -1;
            costume[0x1d].skin_color = 1;
            costume[0x1d].hair_color = new Color(0.95f, 0.8f, 0.5f);
            costume[0x1d].division = DIVISION.TraineesSquad;
            costume[0x1d].costumeId = 0x17;
            costume[30] = new HeroCostume();
            costume[30].name = "armin";
            costume[30].sex = SEX.MALE;
            costume[30].uniform_type = UNIFORM_TYPE.CasualA;
            costume[30].body_texture = body_casual_ma_texture[0];
            costume[30].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[30].part_chest_1_object_texture = body_casual_ma_texture[0];
            costume[30].cape = false;
            costume[30].hairInfo = CostumeHair.hairsM[5];
            costume[30].eye_texture_id = 11;
            costume[30].beard_texture_id = -1;
            costume[30].glass_texture_id = -1;
            costume[30].skin_color = 1;
            costume[30].hair_color = new Color(0.95f, 0.8f, 0.5f);
            costume[30].costumeId = 0x18;
            costume[0x1f] = new HeroCostume();
            costume[0x1f].name = "petra";
            costume[0x1f].sex = SEX.FEMALE;
            costume[0x1f].uniform_type = UNIFORM_TYPE.UniformA;
            costume[0x1f].body_texture = body_uniform_fa_texture[0];
            costume[0x1f].cape = true;
            costume[0x1f].hairInfo = CostumeHair.hairsF[8];
            costume[0x1f].eye_texture_id = 0x1b;
            costume[0x1f].beard_texture_id = -1;
            costume[0x1f].glass_texture_id = -1;
            costume[0x1f].skin_color = 1;
            costume[0x1f].hair_color = new Color(1f, 0.725f, 0.376f);
            costume[0x1f].division = DIVISION.TheSurveryCorps;
            costume[0x1f].costumeId = 9;
            costume[0x20] = new HeroCostume();
            costume[0x20].name = "petra";
            costume[0x20].sex = SEX.FEMALE;
            costume[0x20].uniform_type = UNIFORM_TYPE.CasualA;
            costume[0x20].body_texture = body_casual_fa_texture[0];
            costume[0x20].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[0x20].part_chest_1_object_texture = body_casual_fa_texture[0];
            costume[0x20].cape = false;
            costume[0x20].hairInfo = CostumeHair.hairsF[8];
            costume[0x20].eye_texture_id = 0x1b;
            costume[0x20].beard_texture_id = -1;
            costume[0x20].glass_texture_id = -1;
            costume[0x20].skin_color = 1;
            costume[0x20].hair_color = new Color(1f, 0.725f, 0.376f);
            costume[0x20].division = DIVISION.TheSurveryCorps;
            costume[0x20].costumeId = 10;
            costume[0x21] = new HeroCostume();
            costume[0x21].name = "custom";
            costume[0x21].sex = SEX.FEMALE;
            costume[0x21].uniform_type = UNIFORM_TYPE.CasualB;
            costume[0x21].part_chest_skinned_cloth_mesh = "mikasa_asset_cas";
            costume[0x21].part_chest_skinned_cloth_texture = body_casual_fb_texture[1];
            costume[0x21].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[0x21].part_chest_1_object_texture = body_casual_fb_texture[1];
            costume[0x21].body_texture = body_casual_fb_texture[1];
            costume[0x21].cape = false;
            costume[0x21].hairInfo = CostumeHair.hairsF[2];
            costume[0x21].eye_texture_id = 12;
            costume[0x21].beard_texture_id = 0x21;
            costume[0x21].glass_texture_id = -1;
            costume[0x21].skin_color = 1;
            costume[0x21].hair_color = new Color(0.15f, 0.15f, 0.145f);
            costume[0x21].costumeId = 4;
            costume[0x22] = new HeroCostume();
            costume[0x22].name = "custom";
            costume[0x22].sex = SEX.MALE;
            costume[0x22].uniform_type = UNIFORM_TYPE.CasualA;
            costume[0x22].body_texture = body_casual_ma_texture[0];
            costume[0x22].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[0x22].part_chest_1_object_texture = body_casual_ma_texture[0];
            costume[0x22].cape = false;
            costume[0x22].hairInfo = CostumeHair.hairsM[3];
            costume[0x22].eye_texture_id = 0x1a;
            costume[0x22].beard_texture_id = 0x2c;
            costume[0x22].glass_texture_id = -1;
            costume[0x22].skin_color = 1;
            costume[0x22].hair_color = new Color(0.41f, 1f, 0f);
            costume[0x22].costumeId = 0x18;
            costume[0x23] = new HeroCostume();
            costume[0x23].name = "custom";
            costume[0x23].sex = SEX.FEMALE;
            costume[0x23].uniform_type = UNIFORM_TYPE.UniformA;
            costume[0x23].body_texture = body_uniform_fa_texture[1];
            costume[0x23].cape = false;
            costume[0x23].hairInfo = CostumeHair.hairsF[4];
            costume[0x23].eye_texture_id = 0x16;
            costume[0x23].beard_texture_id = 0x21;
            costume[0x23].glass_texture_id = 0x38;
            costume[0x23].skin_color = 1;
            costume[0x23].hair_color = new Color(0f, 1f, 0.874f);
            costume[0x23].costumeId = 5;
            costume[0x24] = new HeroCostume();
            costume[0x24].name = "feng";
            costume[0x24].sex = SEX.MALE;
            costume[0x24].uniform_type = UNIFORM_TYPE.CasualB;
            costume[0x24].body_texture = body_casual_mb_texture[3];
            costume[0x24].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[0x24].part_chest_1_object_texture = body_casual_mb_texture[3];
            costume[0x24].cape = true;
            costume[0x24].hairInfo = CostumeHair.hairsM[10];
            costume[0x24].eye_texture_id = 0x19;
            costume[0x24].beard_texture_id = 0x27;
            costume[0x24].glass_texture_id = 0x35;
            costume[0x24].skin_color = 1;
            costume[0x24].division = DIVISION.TheSurveryCorps;
            costume[0x24].costumeId = 20;
            costume[0x25] = new HeroCostume();
            costume[0x25].name = "AHSS";
            costume[0x25].sex = SEX.MALE;
            costume[0x25].uniform_type = UNIFORM_TYPE.CasualAHSS;
            costume[0x25].body_texture = body_casual_ma_texture[0] + "_ahss";
            costume[0x25].cape = false;
            costume[0x25].hairInfo = CostumeHair.hairsM[6];
            costume[0x25].eye_texture_id = 0x19;
            costume[0x25].beard_texture_id = 0x27;
            costume[0x25].glass_texture_id = 0x35;
            costume[0x25].skin_color = 3;
            costume[0x25].division = DIVISION.TheMilitaryPolice;
            costume[0x25].costumeId = 0x19;
            for (int i = 0; i < costume.Length; i++)
            {
                costume[i].stat = HeroStat.getInfo("CUSTOM_DEFAULT");
                costume[i].id = i;
                costume[i].setMesh2();
                costume[i].setTexture();
            }
            costumeOption = new HeroCostume[] { 
                costume[0], costume[2], costume[3], costume[4], costume[5], costume[11], costume[13], costume[14], costume[15], costume[0x10], costume[0x11], costume[6], costume[7], costume[8], costume[10], costume[0x12], 
                costume[0x13], costume[0x15], costume[0x16], costume[0x17], costume[0x18], costume[0x19], costume[0x1b], costume[0x1c], costume[30], costume[0x25]
             };
        }
    }

    public static void init2()
    {
        if (!inited)
        {
            inited = true;
            CostumeHair.init();
            body_uniform_ma_texture = new string[] { "aottg_hero_uniform_ma_1", "aottg_hero_uniform_ma_2", "aottg_hero_uniform_ma_3" };
            body_uniform_fa_texture = new string[] { "aottg_hero_uniform_fa_1", "aottg_hero_uniform_fa_2", "aottg_hero_uniform_fa_3" };
            body_uniform_mb_texture = new string[] { "aottg_hero_uniform_mb_1", "aottg_hero_uniform_mb_2", "aottg_hero_uniform_mb_3", "aottg_hero_uniform_mb_4" };
            body_uniform_fb_texture = new string[] { "aottg_hero_uniform_fb_1", "aottg_hero_uniform_fb_2" };
            body_casual_ma_texture = new string[] { "aottg_hero_casual_ma_1", "aottg_hero_casual_ma_2", "aottg_hero_casual_ma_3" };
            body_casual_fa_texture = new string[] { "aottg_hero_casual_fa_1", "aottg_hero_casual_fa_2", "aottg_hero_casual_fa_3" };
            body_casual_mb_texture = new string[] { "aottg_hero_casual_mb_1", "aottg_hero_casual_mb_2", "aottg_hero_casual_mb_3", "aottg_hero_casual_mb_4" };
            body_casual_fb_texture = new string[] { "aottg_hero_casual_fb_1", "aottg_hero_casual_fb_2" };
            costume = new HeroCostume[0x27];
            costume[0] = new HeroCostume();
            costume[0].name = "annie";
            costume[0].sex = SEX.FEMALE;
            costume[0].uniform_type = UNIFORM_TYPE.UniformB;
            costume[0].part_chest_object_mesh = "character_cap_uniform";
            costume[0].part_chest_object_texture = "aottg_hero_annie_cap_uniform";
            costume[0].cape = true;
            costume[0].body_texture = body_uniform_fb_texture[0];
            costume[0].hairInfo = CostumeHair.hairsF[5];
            costume[0].eye_texture_id = 0;
            costume[0].beard_texture_id = 0x21;
            costume[0].glass_texture_id = -1;
            costume[0].skin_color = 1;
            costume[0].hair_color = new Color(1f, 0.9f, 0.5f);
            costume[0].division = DIVISION.TheMilitaryPolice;
            costume[0].costumeId = 0;
            costume[1] = new HeroCostume();
            costume[1].name = "annie";
            costume[1].sex = SEX.FEMALE;
            costume[1].uniform_type = UNIFORM_TYPE.UniformB;
            costume[1].part_chest_object_mesh = "character_cap_uniform";
            costume[1].part_chest_object_texture = "aottg_hero_annie_cap_uniform";
            costume[1].body_texture = body_uniform_fb_texture[0];
            costume[1].cape = false;
            costume[1].hairInfo = CostumeHair.hairsF[5];
            costume[1].eye_texture_id = 0;
            costume[1].beard_texture_id = 0x21;
            costume[1].glass_texture_id = -1;
            costume[1].skin_color = 1;
            costume[1].hair_color = new Color(1f, 0.9f, 0.5f);
            costume[1].division = DIVISION.TraineesSquad;
            costume[1].costumeId = 0;
            costume[2] = new HeroCostume();
            costume[2].name = "annie";
            costume[2].sex = SEX.FEMALE;
            costume[2].uniform_type = UNIFORM_TYPE.CasualB;
            costume[2].part_chest_object_mesh = "character_cap_casual";
            costume[2].part_chest_object_texture = "aottg_hero_annie_cap_causal";
            costume[2].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[2].part_chest_1_object_texture = body_casual_fb_texture[0];
            costume[2].body_texture = body_casual_fb_texture[0];
            costume[2].cape = false;
            costume[2].hairInfo = CostumeHair.hairsF[5];
            costume[2].eye_texture_id = 0;
            costume[2].beard_texture_id = 0x21;
            costume[2].glass_texture_id = -1;
            costume[2].skin_color = 1;
            costume[2].hair_color = new Color(1f, 0.9f, 0.5f);
            costume[2].costumeId = 1;
            costume[3] = new HeroCostume();
            costume[3].name = "mikasa";
            costume[3].sex = SEX.FEMALE;
            costume[3].uniform_type = UNIFORM_TYPE.UniformB;
            costume[3].body_texture = body_uniform_fb_texture[1];
            costume[3].cape = true;
            costume[3].hairInfo = CostumeHair.hairsF[7];
            costume[3].eye_texture_id = 2;
            costume[3].beard_texture_id = 0x21;
            costume[3].glass_texture_id = -1;
            costume[3].skin_color = 1;
            costume[3].hair_color = new Color(0.15f, 0.15f, 0.145f);
            costume[3].division = DIVISION.TheSurveryCorps;
            costume[3].costumeId = 2;
            costume[4] = new HeroCostume();
            costume[4].name = "mikasa";
            costume[4].sex = SEX.FEMALE;
            costume[4].uniform_type = UNIFORM_TYPE.UniformB;
            costume[4].part_chest_skinned_cloth_mesh = "mikasa_asset_uni";
            costume[4].part_chest_skinned_cloth_texture = body_uniform_fb_texture[1];
            costume[4].body_texture = body_uniform_fb_texture[1];
            costume[4].cape = false;
            costume[4].hairInfo = CostumeHair.hairsF[7];
            costume[4].eye_texture_id = 2;
            costume[4].beard_texture_id = 0x21;
            costume[4].glass_texture_id = -1;
            costume[4].skin_color = 1;
            costume[4].hair_color = new Color(0.15f, 0.15f, 0.145f);
            costume[4].division = DIVISION.TraineesSquad;
            costume[4].costumeId = 3;
            costume[5] = new HeroCostume();
            costume[5].name = "mikasa";
            costume[5].sex = SEX.FEMALE;
            costume[5].uniform_type = UNIFORM_TYPE.CasualB;
            costume[5].part_chest_skinned_cloth_mesh = "mikasa_asset_cas";
            costume[5].part_chest_skinned_cloth_texture = body_casual_fb_texture[1];
            costume[5].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[5].part_chest_1_object_texture = body_casual_fb_texture[1];
            costume[5].body_texture = body_casual_fb_texture[1];
            costume[5].cape = false;
            costume[5].hairInfo = CostumeHair.hairsF[7];
            costume[5].eye_texture_id = 2;
            costume[5].beard_texture_id = 0x21;
            costume[5].glass_texture_id = -1;
            costume[5].skin_color = 1;
            costume[5].hair_color = new Color(0.15f, 0.15f, 0.145f);
            costume[5].costumeId = 4;
            costume[6] = new HeroCostume();
            costume[6].name = "levi";
            costume[6].sex = SEX.MALE;
            costume[6].uniform_type = UNIFORM_TYPE.UniformB;
            costume[6].body_texture = body_uniform_mb_texture[1];
            costume[6].cape = true;
            costume[6].hairInfo = CostumeHair.hairsM[7];
            costume[6].eye_texture_id = 1;
            costume[6].beard_texture_id = -1;
            costume[6].glass_texture_id = -1;
            costume[6].skin_color = 1;
            costume[6].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[6].division = DIVISION.TheSurveryCorps;
            costume[6].costumeId = 11;
            costume[7] = new HeroCostume();
            costume[7].name = "levi";
            costume[7].sex = SEX.MALE;
            costume[7].uniform_type = UNIFORM_TYPE.CasualB;
            costume[7].body_texture = body_casual_mb_texture[1];
            costume[7].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[7].part_chest_1_object_texture = body_casual_mb_texture[1];
            costume[7].cape = false;
            costume[7].hairInfo = CostumeHair.hairsM[7];
            costume[7].eye_texture_id = 1;
            costume[7].beard_texture_id = -1;
            costume[7].glass_texture_id = -1;
            costume[7].skin_color = 1;
            costume[7].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[7].costumeId = 12;
            costume[8] = new HeroCostume();
            costume[8].name = "eren";
            costume[8].sex = SEX.MALE;
            costume[8].uniform_type = UNIFORM_TYPE.UniformB;
            costume[8].body_texture = body_uniform_mb_texture[0];
            costume[8].cape = true;
            costume[8].hairInfo = CostumeHair.hairsM[4];
            costume[8].eye_texture_id = 3;
            costume[8].beard_texture_id = -1;
            costume[8].glass_texture_id = -1;
            costume[8].skin_color = 1;
            costume[8].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[8].division = DIVISION.TheSurveryCorps;
            costume[8].costumeId = 13;
            costume[9] = new HeroCostume();
            costume[9].name = "eren";
            costume[9].sex = SEX.MALE;
            costume[9].uniform_type = UNIFORM_TYPE.UniformB;
            costume[9].body_texture = body_uniform_mb_texture[0];
            costume[9].cape = false;
            costume[9].hairInfo = CostumeHair.hairsM[4];
            costume[9].eye_texture_id = 3;
            costume[9].beard_texture_id = -1;
            costume[9].glass_texture_id = -1;
            costume[9].skin_color = 1;
            costume[9].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[9].division = DIVISION.TraineesSquad;
            costume[9].costumeId = 13;
            costume[10] = new HeroCostume();
            costume[10].name = "eren";
            costume[10].sex = SEX.MALE;
            costume[10].uniform_type = UNIFORM_TYPE.CasualB;
            costume[10].body_texture = body_casual_mb_texture[0];
            costume[10].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[10].part_chest_1_object_texture = body_casual_mb_texture[0];
            costume[10].cape = false;
            costume[10].hairInfo = CostumeHair.hairsM[4];
            costume[10].eye_texture_id = 3;
            costume[10].beard_texture_id = -1;
            costume[10].glass_texture_id = -1;
            costume[10].skin_color = 1;
            costume[10].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[10].costumeId = 14;
            costume[11] = new HeroCostume();
            costume[11].name = "sasha";
            costume[11].sex = SEX.FEMALE;
            costume[11].uniform_type = UNIFORM_TYPE.UniformA;
            costume[11].body_texture = body_uniform_fa_texture[1];
            costume[11].cape = true;
            costume[11].hairInfo = CostumeHair.hairsF[10];
            costume[11].eye_texture_id = 4;
            costume[11].beard_texture_id = 0x21;
            costume[11].glass_texture_id = -1;
            costume[11].skin_color = 1;
            costume[11].hair_color = new Color(0.45f, 0.33f, 0.255f);
            costume[11].division = DIVISION.TheSurveryCorps;
            costume[11].costumeId = 5;
            costume[12] = new HeroCostume();
            costume[12].name = "sasha";
            costume[12].sex = SEX.FEMALE;
            costume[12].uniform_type = UNIFORM_TYPE.UniformA;
            costume[12].body_texture = body_uniform_fa_texture[1];
            costume[12].cape = false;
            costume[12].hairInfo = CostumeHair.hairsF[10];
            costume[12].eye_texture_id = 4;
            costume[12].beard_texture_id = 0x21;
            costume[12].glass_texture_id = -1;
            costume[12].skin_color = 1;
            costume[12].hair_color = new Color(0.45f, 0.33f, 0.255f);
            costume[12].division = DIVISION.TraineesSquad;
            costume[12].costumeId = 5;
            costume[13] = new HeroCostume();
            costume[13].name = "sasha";
            costume[13].sex = SEX.FEMALE;
            costume[13].uniform_type = UNIFORM_TYPE.CasualA;
            costume[13].body_texture = body_casual_fa_texture[1];
            costume[13].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[13].part_chest_1_object_texture = body_casual_fa_texture[1];
            costume[13].cape = false;
            costume[13].hairInfo = CostumeHair.hairsF[10];
            costume[13].eye_texture_id = 4;
            costume[13].beard_texture_id = 0x21;
            costume[13].glass_texture_id = -1;
            costume[13].skin_color = 1;
            costume[13].hair_color = new Color(0.45f, 0.33f, 0.255f);
            costume[13].costumeId = 6;
            costume[14] = new HeroCostume();
            costume[14].name = "hanji";
            costume[14].sex = SEX.FEMALE;
            costume[14].uniform_type = UNIFORM_TYPE.UniformA;
            costume[14].body_texture = body_uniform_fa_texture[2];
            costume[14].cape = true;
            costume[14].hairInfo = CostumeHair.hairsF[6];
            costume[14].eye_texture_id = 5;
            costume[14].beard_texture_id = 0x21;
            costume[14].glass_texture_id = 0x31;
            costume[14].skin_color = 1;
            costume[14].hair_color = new Color(0.45f, 0.33f, 0.255f);
            costume[14].division = DIVISION.TheSurveryCorps;
            costume[14].costumeId = 7;
            costume[15] = new HeroCostume();
            costume[15].name = "hanji";
            costume[15].sex = SEX.FEMALE;
            costume[15].uniform_type = UNIFORM_TYPE.CasualA;
            costume[15].body_texture = body_casual_fa_texture[2];
            costume[15].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[15].part_chest_1_object_texture = body_casual_fa_texture[2];
            costume[15].cape = false;
            costume[15].hairInfo = CostumeHair.hairsF[6];
            costume[15].eye_texture_id = 5;
            costume[15].beard_texture_id = 0x21;
            costume[15].glass_texture_id = 0x31;
            costume[15].skin_color = 1;
            costume[15].hair_color = new Color(0.295f, 0.23f, 0.17f);
            costume[15].costumeId = 8;
            costume[0x10] = new HeroCostume();
            costume[0x10].name = "rico";
            costume[0x10].sex = SEX.FEMALE;
            costume[0x10].uniform_type = UNIFORM_TYPE.UniformA;
            costume[0x10].body_texture = body_uniform_fa_texture[0];
            costume[0x10].cape = true;
            costume[0x10].hairInfo = CostumeHair.hairsF[9];
            costume[0x10].eye_texture_id = 6;
            costume[0x10].beard_texture_id = 0x21;
            costume[0x10].glass_texture_id = 0x30;
            costume[0x10].skin_color = 1;
            costume[0x10].hair_color = new Color(1f, 1f, 1f);
            costume[0x10].division = DIVISION.TheGarrison;
            costume[0x10].costumeId = 9;
            costume[0x11] = new HeroCostume();
            costume[0x11].name = "rico";
            costume[0x11].sex = SEX.FEMALE;
            costume[0x11].uniform_type = UNIFORM_TYPE.CasualA;
            costume[0x11].body_texture = body_casual_fa_texture[0];
            costume[0x11].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[0x11].part_chest_1_object_texture = body_casual_fa_texture[0];
            costume[0x11].cape = false;
            costume[0x11].hairInfo = CostumeHair.hairsF[9];
            costume[0x11].eye_texture_id = 6;
            costume[0x11].beard_texture_id = 0x21;
            costume[0x11].glass_texture_id = 0x30;
            costume[0x11].skin_color = 1;
            costume[0x11].hair_color = new Color(1f, 1f, 1f);
            costume[0x11].costumeId = 10;
            costume[0x12] = new HeroCostume();
            costume[0x12].name = "jean";
            costume[0x12].sex = SEX.MALE;
            costume[0x12].uniform_type = UNIFORM_TYPE.UniformA;
            costume[0x12].body_texture = body_uniform_ma_texture[1];
            costume[0x12].cape = true;
            costume[0x12].hairInfo = CostumeHair.hairsM[6];
            costume[0x12].eye_texture_id = 7;
            costume[0x12].beard_texture_id = -1;
            costume[0x12].glass_texture_id = -1;
            costume[0x12].skin_color = 1;
            costume[0x12].hair_color = new Color(0.94f, 0.84f, 0.6f);
            costume[0x12].division = DIVISION.TheSurveryCorps;
            costume[0x12].costumeId = 15;
            costume[0x13] = new HeroCostume();
            costume[0x13].name = "jean";
            costume[0x13].sex = SEX.MALE;
            costume[0x13].uniform_type = UNIFORM_TYPE.UniformA;
            costume[0x13].body_texture = body_uniform_ma_texture[1];
            costume[0x13].cape = false;
            costume[0x13].hairInfo = CostumeHair.hairsM[6];
            costume[0x13].eye_texture_id = 7;
            costume[0x13].beard_texture_id = -1;
            costume[0x13].glass_texture_id = -1;
            costume[0x13].skin_color = 1;
            costume[0x13].hair_color = new Color(0.94f, 0.84f, 0.6f);
            costume[0x13].division = DIVISION.TraineesSquad;
            costume[0x13].costumeId = 15;
            costume[20] = new HeroCostume();
            costume[20].name = "jean";
            costume[20].sex = SEX.MALE;
            costume[20].uniform_type = UNIFORM_TYPE.CasualA;
            costume[20].body_texture = body_casual_ma_texture[1];
            costume[20].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[20].part_chest_1_object_texture = body_casual_ma_texture[1];
            costume[20].cape = false;
            costume[20].hairInfo = CostumeHair.hairsM[6];
            costume[20].eye_texture_id = 7;
            costume[20].beard_texture_id = -1;
            costume[20].glass_texture_id = -1;
            costume[20].skin_color = 1;
            costume[20].hair_color = new Color(0.94f, 0.84f, 0.6f);
            costume[20].costumeId = 0x10;
            costume[0x15] = new HeroCostume();
            costume[0x15].name = "marco";
            costume[0x15].sex = SEX.MALE;
            costume[0x15].uniform_type = UNIFORM_TYPE.UniformA;
            costume[0x15].body_texture = body_uniform_ma_texture[2];
            costume[0x15].cape = false;
            costume[0x15].hairInfo = CostumeHair.hairsM[8];
            costume[0x15].eye_texture_id = 8;
            costume[0x15].beard_texture_id = -1;
            costume[0x15].glass_texture_id = -1;
            costume[0x15].skin_color = 1;
            costume[0x15].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[0x15].division = DIVISION.TraineesSquad;
            costume[0x15].costumeId = 0x11;
            costume[0x16] = new HeroCostume();
            costume[0x16].name = "marco";
            costume[0x16].sex = SEX.MALE;
            costume[0x16].uniform_type = UNIFORM_TYPE.CasualA;
            costume[0x16].body_texture = body_casual_ma_texture[2];
            costume[0x16].cape = false;
            costume[0x16].hairInfo = CostumeHair.hairsM[8];
            costume[0x16].eye_texture_id = 8;
            costume[0x16].beard_texture_id = -1;
            costume[0x16].glass_texture_id = -1;
            costume[0x16].skin_color = 1;
            costume[0x16].hair_color = new Color(0.295f, 0.295f, 0.275f);
            costume[0x16].costumeId = 0x12;
            costume[0x17] = new HeroCostume();
            costume[0x17].name = "mike";
            costume[0x17].sex = SEX.MALE;
            costume[0x17].uniform_type = UNIFORM_TYPE.UniformB;
            costume[0x17].body_texture = body_uniform_mb_texture[3];
            costume[0x17].cape = true;
            costume[0x17].hairInfo = CostumeHair.hairsM[9];
            costume[0x17].eye_texture_id = 9;
            costume[0x17].beard_texture_id = 0x20;
            costume[0x17].glass_texture_id = -1;
            costume[0x17].skin_color = 1;
            costume[0x17].hair_color = new Color(0.94f, 0.84f, 0.6f);
            costume[0x17].division = DIVISION.TheSurveryCorps;
            costume[0x17].costumeId = 0x13;
            costume[0x18] = new HeroCostume();
            costume[0x18].name = "mike";
            costume[0x18].sex = SEX.MALE;
            costume[0x18].uniform_type = UNIFORM_TYPE.CasualB;
            costume[0x18].body_texture = body_casual_mb_texture[3];
            costume[0x18].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[0x18].part_chest_1_object_texture = body_casual_mb_texture[3];
            costume[0x18].cape = false;
            costume[0x18].hairInfo = CostumeHair.hairsM[9];
            costume[0x18].eye_texture_id = 9;
            costume[0x18].beard_texture_id = 0x20;
            costume[0x18].glass_texture_id = -1;
            costume[0x18].skin_color = 1;
            costume[0x18].hair_color = new Color(0.94f, 0.84f, 0.6f);
            costume[0x18].division = DIVISION.TheSurveryCorps;
            costume[0x18].costumeId = 20;
            costume[0x19] = new HeroCostume();
            costume[0x19].name = "connie";
            costume[0x19].sex = SEX.MALE;
            costume[0x19].uniform_type = UNIFORM_TYPE.UniformB;
            costume[0x19].body_texture = body_uniform_mb_texture[2];
            costume[0x19].cape = true;
            costume[0x19].hairInfo = CostumeHair.hairsM[10];
            costume[0x19].eye_texture_id = 10;
            costume[0x19].beard_texture_id = -1;
            costume[0x19].glass_texture_id = -1;
            costume[0x19].skin_color = 1;
            costume[0x19].division = DIVISION.TheSurveryCorps;
            costume[0x19].costumeId = 0x15;
            costume[0x1a] = new HeroCostume();
            costume[0x1a].name = "connie";
            costume[0x1a].sex = SEX.MALE;
            costume[0x1a].uniform_type = UNIFORM_TYPE.UniformB;
            costume[0x1a].body_texture = body_uniform_mb_texture[2];
            costume[0x1a].cape = false;
            costume[0x1a].hairInfo = CostumeHair.hairsM[10];
            costume[0x1a].eye_texture_id = 10;
            costume[0x1a].beard_texture_id = -1;
            costume[0x1a].glass_texture_id = -1;
            costume[0x1a].skin_color = 1;
            costume[0x1a].division = DIVISION.TraineesSquad;
            costume[0x1a].costumeId = 0x15;
            costume[0x1b] = new HeroCostume();
            costume[0x1b].name = "connie";
            costume[0x1b].sex = SEX.MALE;
            costume[0x1b].uniform_type = UNIFORM_TYPE.CasualB;
            costume[0x1b].body_texture = body_casual_mb_texture[2];
            costume[0x1b].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[0x1b].part_chest_1_object_texture = body_casual_mb_texture[2];
            costume[0x1b].cape = false;
            costume[0x1b].hairInfo = CostumeHair.hairsM[10];
            costume[0x1b].eye_texture_id = 10;
            costume[0x1b].beard_texture_id = -1;
            costume[0x1b].glass_texture_id = -1;
            costume[0x1b].skin_color = 1;
            costume[0x1b].costumeId = 0x16;
            costume[0x1c] = new HeroCostume();
            costume[0x1c].name = "armin";
            costume[0x1c].sex = SEX.MALE;
            costume[0x1c].uniform_type = UNIFORM_TYPE.UniformA;
            costume[0x1c].body_texture = body_uniform_ma_texture[0];
            costume[0x1c].cape = true;
            costume[0x1c].hairInfo = CostumeHair.hairsM[5];
            costume[0x1c].eye_texture_id = 11;
            costume[0x1c].beard_texture_id = -1;
            costume[0x1c].glass_texture_id = -1;
            costume[0x1c].skin_color = 1;
            costume[0x1c].hair_color = new Color(0.95f, 0.8f, 0.5f);
            costume[0x1c].division = DIVISION.TheSurveryCorps;
            costume[0x1c].costumeId = 0x17;
            costume[0x1d] = new HeroCostume();
            costume[0x1d].name = "armin";
            costume[0x1d].sex = SEX.MALE;
            costume[0x1d].uniform_type = UNIFORM_TYPE.UniformA;
            costume[0x1d].body_texture = body_uniform_ma_texture[0];
            costume[0x1d].cape = false;
            costume[0x1d].hairInfo = CostumeHair.hairsM[5];
            costume[0x1d].eye_texture_id = 11;
            costume[0x1d].beard_texture_id = -1;
            costume[0x1d].glass_texture_id = -1;
            costume[0x1d].skin_color = 1;
            costume[0x1d].hair_color = new Color(0.95f, 0.8f, 0.5f);
            costume[0x1d].division = DIVISION.TraineesSquad;
            costume[0x1d].costumeId = 0x17;
            costume[30] = new HeroCostume();
            costume[30].name = "armin";
            costume[30].sex = SEX.MALE;
            costume[30].uniform_type = UNIFORM_TYPE.CasualA;
            costume[30].body_texture = body_casual_ma_texture[0];
            costume[30].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[30].part_chest_1_object_texture = body_casual_ma_texture[0];
            costume[30].cape = false;
            costume[30].hairInfo = CostumeHair.hairsM[5];
            costume[30].eye_texture_id = 11;
            costume[30].beard_texture_id = -1;
            costume[30].glass_texture_id = -1;
            costume[30].skin_color = 1;
            costume[30].hair_color = new Color(0.95f, 0.8f, 0.5f);
            costume[30].costumeId = 0x18;
            costume[0x1f] = new HeroCostume();
            costume[0x1f].name = "petra";
            costume[0x1f].sex = SEX.FEMALE;
            costume[0x1f].uniform_type = UNIFORM_TYPE.UniformA;
            costume[0x1f].body_texture = body_uniform_fa_texture[0];
            costume[0x1f].cape = true;
            costume[0x1f].hairInfo = CostumeHair.hairsF[8];
            costume[0x1f].eye_texture_id = 0x1b;
            costume[0x1f].beard_texture_id = -1;
            costume[0x1f].glass_texture_id = -1;
            costume[0x1f].skin_color = 1;
            costume[0x1f].hair_color = new Color(1f, 0.725f, 0.376f);
            costume[0x1f].division = DIVISION.TheSurveryCorps;
            costume[0x1f].costumeId = 9;
            costume[0x20] = new HeroCostume();
            costume[0x20].name = "petra";
            costume[0x20].sex = SEX.FEMALE;
            costume[0x20].uniform_type = UNIFORM_TYPE.CasualA;
            costume[0x20].body_texture = body_casual_fa_texture[0];
            costume[0x20].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[0x20].part_chest_1_object_texture = body_casual_fa_texture[0];
            costume[0x20].cape = false;
            costume[0x20].hairInfo = CostumeHair.hairsF[8];
            costume[0x20].eye_texture_id = 0x1b;
            costume[0x20].beard_texture_id = -1;
            costume[0x20].glass_texture_id = -1;
            costume[0x20].skin_color = 1;
            costume[0x20].hair_color = new Color(1f, 0.725f, 0.376f);
            costume[0x20].division = DIVISION.TheSurveryCorps;
            costume[0x20].costumeId = 10;
            costume[0x21] = new HeroCostume();
            costume[0x21].name = "custom";
            costume[0x21].sex = SEX.FEMALE;
            costume[0x21].uniform_type = UNIFORM_TYPE.CasualB;
            costume[0x21].part_chest_skinned_cloth_mesh = "mikasa_asset_cas";
            costume[0x21].part_chest_skinned_cloth_texture = body_casual_fb_texture[1];
            costume[0x21].part_chest_1_object_mesh = "character_body_blade_keeper_f";
            costume[0x21].part_chest_1_object_texture = body_casual_fb_texture[1];
            costume[0x21].body_texture = body_casual_fb_texture[1];
            costume[0x21].cape = false;
            costume[0x21].hairInfo = CostumeHair.hairsF[2];
            costume[0x21].eye_texture_id = 12;
            costume[0x21].beard_texture_id = 0x21;
            costume[0x21].glass_texture_id = -1;
            costume[0x21].skin_color = 1;
            costume[0x21].hair_color = new Color(0.15f, 0.15f, 0.145f);
            costume[0x21].costumeId = 4;
            costume[0x22] = new HeroCostume();
            costume[0x22].name = "custom";
            costume[0x22].sex = SEX.MALE;
            costume[0x22].uniform_type = UNIFORM_TYPE.CasualA;
            costume[0x22].body_texture = body_casual_ma_texture[0];
            costume[0x22].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[0x22].part_chest_1_object_texture = body_casual_ma_texture[0];
            costume[0x22].cape = false;
            costume[0x22].hairInfo = CostumeHair.hairsM[3];
            costume[0x22].eye_texture_id = 0x1a;
            costume[0x22].beard_texture_id = 0x2c;
            costume[0x22].glass_texture_id = -1;
            costume[0x22].skin_color = 1;
            costume[0x22].hair_color = new Color(0.41f, 1f, 0f);
            costume[0x22].costumeId = 0x18;
            costume[0x23] = new HeroCostume();
            costume[0x23].name = "custom";
            costume[0x23].sex = SEX.FEMALE;
            costume[0x23].uniform_type = UNIFORM_TYPE.UniformA;
            costume[0x23].body_texture = body_uniform_fa_texture[1];
            costume[0x23].cape = false;
            costume[0x23].hairInfo = CostumeHair.hairsF[4];
            costume[0x23].eye_texture_id = 0x16;
            costume[0x23].beard_texture_id = 0x21;
            costume[0x23].glass_texture_id = 0x38;
            costume[0x23].skin_color = 1;
            costume[0x23].hair_color = new Color(0f, 1f, 0.874f);
            costume[0x23].costumeId = 5;
            costume[0x24] = new HeroCostume();
            costume[0x24].name = "feng";
            costume[0x24].sex = SEX.MALE;
            costume[0x24].uniform_type = UNIFORM_TYPE.CasualB;
            costume[0x24].body_texture = body_casual_mb_texture[3];
            costume[0x24].part_chest_1_object_mesh = "character_body_blade_keeper_m";
            costume[0x24].part_chest_1_object_texture = body_casual_mb_texture[3];
            costume[0x24].cape = true;
            costume[0x24].hairInfo = CostumeHair.hairsM[10];
            costume[0x24].eye_texture_id = 0x19;
            costume[0x24].beard_texture_id = 0x27;
            costume[0x24].glass_texture_id = 0x35;
            costume[0x24].skin_color = 1;
            costume[0x24].division = DIVISION.TheSurveryCorps;
            costume[0x24].costumeId = 20;
            costume[0x25] = new HeroCostume();
            costume[0x25].name = "AHSS";
            costume[0x25].sex = SEX.MALE;
            costume[0x25].uniform_type = UNIFORM_TYPE.CasualAHSS;
            costume[0x25].body_texture = body_casual_ma_texture[0] + "_ahss";
            costume[0x25].cape = false;
            costume[0x25].hairInfo = CostumeHair.hairsM[6];
            costume[0x25].eye_texture_id = 0x19;
            costume[0x25].beard_texture_id = 0x27;
            costume[0x25].glass_texture_id = 0x35;
            costume[0x25].skin_color = 3;
            costume[0x25].division = DIVISION.TheMilitaryPolice;
            costume[0x25].costumeId = 0x19;
            costume[0x26] = new HeroCostume();
            costume[0x26].name = "AHSS (F)";
            costume[0x26].sex = SEX.FEMALE;
            costume[0x26].uniform_type = UNIFORM_TYPE.CasualAHSS;
            costume[0x26].body_texture = body_casual_fa_texture[0];
            costume[0x26].cape = false;
            costume[0x26].hairInfo = CostumeHair.hairsF[6];
            costume[0x26].eye_texture_id = 2;
            costume[0x26].beard_texture_id = 0x21;
            costume[0x26].glass_texture_id = -1;
            costume[0x26].skin_color = 3;
            costume[0x26].division = DIVISION.TheMilitaryPolice;
            costume[0x26].costumeId = 0x1a;
            for (int i = 0; i < costume.Length; i++)
            {
                costume[i].stat = HeroStat.getInfo("CUSTOM_DEFAULT");
                costume[i].id = i;
                costume[i].setMesh2();
                costume[i].setTexture();
            }
            costumeOption = new HeroCostume[] { 
                costume[0], costume[2], costume[3], costume[4], costume[5], costume[11], costume[13], costume[14], costume[15], costume[0x10], costume[0x11], costume[6], costume[7], costume[8], costume[10], costume[0x12], 
                costume[0x13], costume[0x15], costume[0x16], costume[0x17], costume[0x18], costume[0x19], costume[0x1b], costume[0x1c], costume[30], costume[0x25], costume[0x26]
             };
        }
    }

    public void setBodyByCostumeId(int id = -1)
    {
        if (id == -1)
        {
            id = this.costumeId;
        }
        this.costumeId = id;
        this.arm_l_mesh = costumeOption[id].arm_l_mesh;
        this.arm_r_mesh = costumeOption[id].arm_r_mesh;
        this.body_mesh = costumeOption[id].body_mesh;
        this.body_texture = costumeOption[id].body_texture;
        this.uniform_type = costumeOption[id].uniform_type;
        this.part_chest_1_object_mesh = costumeOption[id].part_chest_1_object_mesh;
        this.part_chest_1_object_texture = costumeOption[id].part_chest_1_object_texture;
        this.part_chest_object_mesh = costumeOption[id].part_chest_object_mesh;
        this.part_chest_object_texture = costumeOption[id].part_chest_object_texture;
        this.part_chest_skinned_cloth_mesh = costumeOption[id].part_chest_skinned_cloth_mesh;
        this.part_chest_skinned_cloth_texture = costumeOption[id].part_chest_skinned_cloth_texture;
    }

    public void setCape()
    {
        if (this.cape)
        {
            this.cape_mesh = "character_cape";
        }
        else
        {
            this.cape_mesh = string.Empty;
        }
    }

    public void setMesh()
    {
        this.brand1_mesh = string.Empty;
        this.brand2_mesh = string.Empty;
        this.brand3_mesh = string.Empty;
        this.brand4_mesh = string.Empty;
        this.hand_l_mesh = "character_hand_l";
        this.hand_r_mesh = "character_hand_r";
        this.mesh_3dmg = "character_3dmg";
        this.mesh_3dmg_belt = "character_3dmg_belt";
        this.mesh_3dmg_gas_l = "character_3dmg_gas_l";
        this.mesh_3dmg_gas_r = "character_3dmg_gas_r";
        this.weapon_l_mesh = "character_blade_l";
        this.weapon_r_mesh = "character_blade_r";
        if (this.uniform_type == UNIFORM_TYPE.CasualAHSS)
        {
            this.hand_l_mesh = "character_hand_l_ah";
            this.hand_r_mesh = "character_hand_r_ah";
            this.arm_l_mesh = "character_arm_casual_l_ah";
            this.arm_r_mesh = "character_arm_casual_r_ah";
            this.body_mesh = "character_body_casual_MA";
            this.mesh_3dmg = "character_3dmg_2";
            this.mesh_3dmg_belt = string.Empty;
            this.mesh_3dmg_gas_l = "character_gun_mag_l";
            this.mesh_3dmg_gas_r = "character_gun_mag_r";
            this.weapon_l_mesh = "character_gun_l";
            this.weapon_r_mesh = "character_gun_r";
        }
        else if (this.uniform_type == UNIFORM_TYPE.UniformA)
        {
            this.arm_l_mesh = "character_arm_uniform_l";
            this.arm_r_mesh = "character_arm_uniform_r";
            this.brand1_mesh = "character_brand_arm_l";
            this.brand2_mesh = "character_brand_arm_r";
            if (this.sex == SEX.FEMALE)
            {
                this.body_mesh = "character_body_uniform_FA";
                this.brand3_mesh = "character_brand_chest_f";
                this.brand4_mesh = "character_brand_back_f";
            }
            else
            {
                this.body_mesh = "character_body_uniform_MA";
                this.brand3_mesh = "character_brand_chest_m";
                this.brand4_mesh = "character_brand_back_m";
            }
        }
        else if (this.uniform_type == UNIFORM_TYPE.UniformB)
        {
            this.arm_l_mesh = "character_arm_uniform_l";
            this.arm_r_mesh = "character_arm_uniform_r";
            this.brand1_mesh = "character_brand_arm_l";
            this.brand2_mesh = "character_brand_arm_r";
            if (this.sex == SEX.FEMALE)
            {
                this.body_mesh = "character_body_uniform_FB";
                this.brand3_mesh = "character_brand_chest_f";
                this.brand4_mesh = "character_brand_back_f";
            }
            else
            {
                this.body_mesh = "character_body_uniform_MB";
                this.brand3_mesh = "character_brand_chest_m";
                this.brand4_mesh = "character_brand_back_m";
            }
        }
        else if (this.uniform_type == UNIFORM_TYPE.CasualA)
        {
            this.arm_l_mesh = "character_arm_casual_l";
            this.arm_r_mesh = "character_arm_casual_r";
            if (this.sex == SEX.FEMALE)
            {
                this.body_mesh = "character_body_casual_FA";
            }
            else
            {
                this.body_mesh = "character_body_casual_MA";
            }
        }
        else if (this.uniform_type == UNIFORM_TYPE.CasualB)
        {
            this.arm_l_mesh = "character_arm_casual_l";
            this.arm_r_mesh = "character_arm_casual_r";
            if (this.sex == SEX.FEMALE)
            {
                this.body_mesh = "character_body_casual_FB";
            }
            else
            {
                this.body_mesh = "character_body_casual_MB";
            }
        }
        if (this.hairInfo.hair.Length > 0)
        {
            this.hair_mesh = this.hairInfo.hair;
        }
        if (this.hairInfo.hasCloth)
        {
            this.hair_1_mesh = this.hairInfo.hair_1;
        }
        if (this.eye_texture_id >= 0)
        {
            this.eye_mesh = "character_eye";
        }
        if (this.beard_texture_id >= 0)
        {
            this.beard_mesh = "character_face";
        }
        else
        {
            this.beard_mesh = string.Empty;
        }
        if (this.glass_texture_id >= 0)
        {
            this.glass_mesh = "glass";
        }
        else
        {
            this.glass_mesh = string.Empty;
        }
        this.setCape();
    }

    public void setMesh2()
    {
        this.brand1_mesh = string.Empty;
        this.brand2_mesh = string.Empty;
        this.brand3_mesh = string.Empty;
        this.brand4_mesh = string.Empty;
        this.hand_l_mesh = "character_hand_l";
        this.hand_r_mesh = "character_hand_r";
        this.mesh_3dmg = "character_3dmg";
        this.mesh_3dmg_belt = "character_3dmg_belt";
        this.mesh_3dmg_gas_l = "character_3dmg_gas_l";
        this.mesh_3dmg_gas_r = "character_3dmg_gas_r";
        this.weapon_l_mesh = "character_blade_l";
        this.weapon_r_mesh = "character_blade_r";
        if (this.uniform_type == UNIFORM_TYPE.CasualAHSS)
        {
            this.hand_l_mesh = "character_hand_l_ah";
            this.hand_r_mesh = "character_hand_r_ah";
            this.arm_l_mesh = "character_arm_casual_l_ah";
            this.arm_r_mesh = "character_arm_casual_r_ah";
            if (this.sex == SEX.FEMALE)
            {
                this.body_mesh = "character_body_casual_FA";
            }
            else
            {
                this.body_mesh = "character_body_casual_MA";
            }
            this.mesh_3dmg = "character_3dmg_2";
            this.mesh_3dmg_belt = string.Empty;
            this.mesh_3dmg_gas_l = "character_gun_mag_l";
            this.mesh_3dmg_gas_r = "character_gun_mag_r";
            this.weapon_l_mesh = "character_gun_l";
            this.weapon_r_mesh = "character_gun_r";
        }
        else if (this.uniform_type == UNIFORM_TYPE.UniformA)
        {
            this.arm_l_mesh = "character_arm_uniform_l";
            this.arm_r_mesh = "character_arm_uniform_r";
            this.brand1_mesh = "character_brand_arm_l";
            this.brand2_mesh = "character_brand_arm_r";
            if (this.sex == SEX.FEMALE)
            {
                this.body_mesh = "character_body_uniform_FA";
                this.brand3_mesh = "character_brand_chest_f";
                this.brand4_mesh = "character_brand_back_f";
            }
            else
            {
                this.body_mesh = "character_body_uniform_MA";
                this.brand3_mesh = "character_brand_chest_m";
                this.brand4_mesh = "character_brand_back_m";
            }
        }
        else if (this.uniform_type == UNIFORM_TYPE.UniformB)
        {
            this.arm_l_mesh = "character_arm_uniform_l";
            this.arm_r_mesh = "character_arm_uniform_r";
            this.brand1_mesh = "character_brand_arm_l";
            this.brand2_mesh = "character_brand_arm_r";
            if (this.sex == SEX.FEMALE)
            {
                this.body_mesh = "character_body_uniform_FB";
                this.brand3_mesh = "character_brand_chest_f";
                this.brand4_mesh = "character_brand_back_f";
            }
            else
            {
                this.body_mesh = "character_body_uniform_MB";
                this.brand3_mesh = "character_brand_chest_m";
                this.brand4_mesh = "character_brand_back_m";
            }
        }
        else if (this.uniform_type == UNIFORM_TYPE.CasualA)
        {
            this.arm_l_mesh = "character_arm_casual_l";
            this.arm_r_mesh = "character_arm_casual_r";
            if (this.sex == SEX.FEMALE)
            {
                this.body_mesh = "character_body_casual_FA";
            }
            else
            {
                this.body_mesh = "character_body_casual_MA";
            }
        }
        else if (this.uniform_type == UNIFORM_TYPE.CasualB)
        {
            this.arm_l_mesh = "character_arm_casual_l";
            this.arm_r_mesh = "character_arm_casual_r";
            if (this.sex == SEX.FEMALE)
            {
                this.body_mesh = "character_body_casual_FB";
            }
            else
            {
                this.body_mesh = "character_body_casual_MB";
            }
        }
        if (this.hairInfo.hair.Length > 0)
        {
            this.hair_mesh = this.hairInfo.hair;
        }
        if (this.hairInfo.hasCloth)
        {
            this.hair_1_mesh = this.hairInfo.hair_1;
        }
        if (this.eye_texture_id >= 0)
        {
            this.eye_mesh = "character_eye";
        }
        if (this.beard_texture_id >= 0)
        {
            this.beard_mesh = "character_face";
        }
        else
        {
            this.beard_mesh = string.Empty;
        }
        if (this.glass_texture_id >= 0)
        {
            this.glass_mesh = "glass";
        }
        else
        {
            this.glass_mesh = string.Empty;
        }
        this.setCape();
    }

    public void setTexture()
    {
        if (this.uniform_type == UNIFORM_TYPE.CasualAHSS)
        {
            this._3dmg_texture = "aottg_hero_AHSS_3dmg";
        }
        else
        {
            this._3dmg_texture = "AOTTG_HERO_3DMG";
        }
        this.face_texture = "aottg_hero_eyes";
        if (this.division == DIVISION.TheMilitaryPolice)
        {
            this.brand_texture = "aottg_hero_brand_mp";
        }
        if (this.division == DIVISION.TheGarrison)
        {
            this.brand_texture = "aottg_hero_brand_g";
        }
        if (this.division == DIVISION.TheSurveryCorps)
        {
            this.brand_texture = "aottg_hero_brand_sc";
        }
        if (this.division == DIVISION.TraineesSquad)
        {
            this.brand_texture = "aottg_hero_brand_ts";
        }
        if (this.skin_color == 1)
        {
            this.skin_texture = "aottg_hero_skin_1";
        }
        else if (this.skin_color == 2)
        {
            this.skin_texture = "aottg_hero_skin_2";
        }
        else if (this.skin_color == 3)
        {
            this.skin_texture = "aottg_hero_skin_3";
        }
    }
}

