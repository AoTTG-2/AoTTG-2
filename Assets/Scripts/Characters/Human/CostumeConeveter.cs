using ExitGames.Client.Photon;
using System;
using UnityEngine;

public class CostumeConeveter
{
    private static int DivisionToInt(DIVISION id)
    {
        if (id == DIVISION.TheGarrison)
        {
            return 0;
        }
        if (id == DIVISION.TheMilitaryPolice)
        {
            return 1;
        }
        if ((id != DIVISION.TheSurveryCorps) && (id == DIVISION.TraineesSquad))
        {
            return 3;
        }
        return 2;
    }

    public static void HeroCostumeToLocalData(HeroCostume costume, string slot)
    {
        slot = slot.ToUpper();
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.sex, SexToInt(costume.sex));
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.costumeId, costume.costumeId);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.heroCostumeId, costume.id);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.cape, !costume.cape ? 0 : 1);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.hairInfo, costume.hairInfo.id);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.eye_texture_id, costume.eye_texture_id);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.beard_texture_id, costume.beard_texture_id);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.glass_texture_id, costume.glass_texture_id);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.skin_color, costume.skin_color);
        PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.hair_color1, costume.hair_color.r);
        PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.hair_color2, costume.hair_color.g);
        PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.hair_color3, costume.hair_color.b);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.division, DivisionToInt(costume.division));
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statSPD, costume.stat.SPD);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statGAS, costume.stat.GAS);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statBLA, costume.stat.BLA);
        PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statACL, costume.stat.ACL);
        PlayerPrefs.SetString(slot + PhotonPlayerProperty.statSKILL, costume.stat.skillId);
    }

    public static void HeroCostumeToPhotonData(HeroCostume costume, PhotonPlayer player)
    {
        Hashtable propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.sex, SexToInt(costume.sex));
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.costumeId, costume.costumeId);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.heroCostumeId, costume.id);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.cape, costume.cape);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.hairInfo, costume.hairInfo.id);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.eye_texture_id, costume.eye_texture_id);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.beard_texture_id, costume.beard_texture_id);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.glass_texture_id, costume.glass_texture_id);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.skin_color, costume.skin_color);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.hair_color1, costume.hair_color.r);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.hair_color2, costume.hair_color.g);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.hair_color3, costume.hair_color.b);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.division, DivisionToInt(costume.division));
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statSPD, costume.stat.SPD);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statGAS, costume.stat.GAS);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statBLA, costume.stat.BLA);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statACL, costume.stat.ACL);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statSKILL, costume.stat.skillId);
        player.SetCustomProperties(propertiesToSet);
    }

    public static void HeroCostumeToPhotonData2(HeroCostume costume, PhotonPlayer player)
    {
        // HACK
        if (costume == null)
        {
            HeroCostume.init2();
            costume = HeroCostume.costume[0];
        }
        Hashtable propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.sex, SexToInt(costume.sex));
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        int costumeId = costume.costumeId;
        if (costumeId == 0x1a)
        {
            costumeId = 0x19;
        }
        propertiesToSet.Add(PhotonPlayerProperty.costumeId, costumeId);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.heroCostumeId, costume.id);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.cape, costume.cape);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.hairInfo, costume.hairInfo.id);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.eye_texture_id, costume.eye_texture_id);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.beard_texture_id, costume.beard_texture_id);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.glass_texture_id, costume.glass_texture_id);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.skin_color, costume.skin_color);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.hair_color1, costume.hair_color.r);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.hair_color2, costume.hair_color.g);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.hair_color3, costume.hair_color.b);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.division, DivisionToInt(costume.division));
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statSPD, costume.stat.SPD);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statGAS, costume.stat.GAS);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statBLA, costume.stat.BLA);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statACL, costume.stat.ACL);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.statSKILL, costume.stat.skillId);
        player.SetCustomProperties(propertiesToSet);
    }

    private static DIVISION IntToDivision(int id)
    {
        if (id == 0)
        {
            return DIVISION.TheGarrison;
        }
        if (id == 1)
        {
            return DIVISION.TheMilitaryPolice;
        }
        if ((id != 2) && (id == 3))
        {
            return DIVISION.TraineesSquad;
        }
        return DIVISION.TheSurveryCorps;
    }

    private static SEX IntToSex(int id)
    {
        if (id == 0)
        {
            return SEX.FEMALE;
        }
        if (id == 1)
        {
            return SEX.MALE;
        }
        return SEX.MALE;
    }

    public static HeroCostume LocalDataToHeroCostume(string slot)
    {
        slot = slot.ToUpper();
        if (!PlayerPrefs.HasKey(slot + PhotonPlayerProperty.sex))
        {
            return HeroCostume.costume[0];
        }
        HeroCostume costume = new HeroCostume();
        costume = new HeroCostume {
            sex = IntToSex(PlayerPrefs.GetInt(slot + PhotonPlayerProperty.sex)),
            id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.heroCostumeId),
            costumeId = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.costumeId),
            cape = (PlayerPrefs.GetInt(slot + PhotonPlayerProperty.cape) != 1) ? false : true,
            hairInfo = (costume.sex != SEX.MALE) ? CostumeHair.hairsF[PlayerPrefs.GetInt(slot + PhotonPlayerProperty.hairInfo)] : CostumeHair.hairsM[PlayerPrefs.GetInt(slot + PhotonPlayerProperty.hairInfo)],
            eye_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.eye_texture_id),
            beard_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.beard_texture_id),
            glass_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.glass_texture_id),
            skin_color = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.skin_color),
            hair_color = new Color(PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.hair_color1), PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.hair_color2), PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.hair_color3)),
            division = IntToDivision(PlayerPrefs.GetInt(slot + PhotonPlayerProperty.division)),
            stat = new HeroStat()
        };
        costume.stat.SPD = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statSPD);
        costume.stat.GAS = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statGAS);
        costume.stat.BLA = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statBLA);
        costume.stat.ACL = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statACL);
        costume.stat.skillId = PlayerPrefs.GetString(slot + PhotonPlayerProperty.statSKILL);
        costume.setBodyByCostumeId(-1);
        costume.setMesh();
        costume.setTexture();
        return costume;
    }

    public static HeroCostume PhotonDataToHeroCostume2(PhotonPlayer player)
    {
        HeroCostume costume = new HeroCostume();
        SEX sex = IntToSex((int) player.CustomProperties[PhotonPlayerProperty.sex]);
        costume = new HeroCostume {
            sex = sex,
            costumeId = (int) player.CustomProperties[PhotonPlayerProperty.costumeId],
            id = (int) player.CustomProperties[PhotonPlayerProperty.heroCostumeId],
            cape = (bool) player.CustomProperties[PhotonPlayerProperty.cape],
            hairInfo = (sex != SEX.MALE) ? CostumeHair.hairsF[(int) player.CustomProperties[PhotonPlayerProperty.hairInfo]] : CostumeHair.hairsM[(int) player.CustomProperties[PhotonPlayerProperty.hairInfo]],
            eye_texture_id = (int) player.CustomProperties[PhotonPlayerProperty.eye_texture_id],
            beard_texture_id = (int) player.CustomProperties[PhotonPlayerProperty.beard_texture_id],
            glass_texture_id = (int) player.CustomProperties[PhotonPlayerProperty.glass_texture_id],
            skin_color = (int) player.CustomProperties[PhotonPlayerProperty.skin_color],
            hair_color = new Color((float) player.CustomProperties[PhotonPlayerProperty.hair_color1], (float) player.CustomProperties[PhotonPlayerProperty.hair_color2], (float) player.CustomProperties[PhotonPlayerProperty.hair_color3]),
            division = IntToDivision((int) player.CustomProperties[PhotonPlayerProperty.division]),
            stat = new HeroStat()
        };
        costume.stat.SPD = (int) player.CustomProperties[PhotonPlayerProperty.statSPD];
        costume.stat.GAS = (int) player.CustomProperties[PhotonPlayerProperty.statGAS];
        costume.stat.BLA = (int) player.CustomProperties[PhotonPlayerProperty.statBLA];
        costume.stat.ACL = (int) player.CustomProperties[PhotonPlayerProperty.statACL];
        costume.stat.skillId = (string) player.CustomProperties[PhotonPlayerProperty.statSKILL];
        if ((costume.costumeId == 0x19) && (costume.sex == SEX.FEMALE))
        {
            costume.costumeId = 0x1a;
        }
        costume.setBodyByCostumeId(-1);
        costume.setMesh();
        costume.setTexture();
        return costume;
    }

    private static int SexToInt(SEX id)
    {
        if (id == SEX.FEMALE)
        {
            return 0;
        }
        if (id == SEX.MALE)
        {
            return 1;
        }
        return 1;
    }
}

