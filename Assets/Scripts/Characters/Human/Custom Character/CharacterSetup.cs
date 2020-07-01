using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ArmatureData))]
public class CharacterSetup : MonoBehaviour
{
    [SerializeField] private CharacterPrefabs characterPrefabs;
    private ArmatureData armatureData;

    private void Awake()
    {
        armatureData = gameObject.GetComponent<ArmatureData>();
    }

    public void CreateCharacter(CharacterOutfit outfit, CharacterStat stat)
    {
        CreateBody(outfit.odmg, outfit.skinColor);
        CreateFacePart(characterPrefabs.eyes, outfit.eyeID);
        CreateFacePart(characterPrefabs.glasses, outfit.glassesID);
        CreateFacePart(characterPrefabs.mouth, outfit.mouthID);
        CreateHair(outfit.sex, outfit.hairID, outfit.hairColor);
        CreateOdmg(outfit.odmg, stat);
        CreateOutfit(outfit.sex, outfit.outfitType, outfit.outfitID, outfit.outfitTextureID);
        CreateEmblems(outfit.sex, outfit.division);
        CreateOptional(outfit);
    }

    private void AttachToArmature(GameObject characterObject)
    {
        characterObject.GetComponent<ArmatureAttachment>().AttachToArmature(armatureData);
    }

    private void SetColor(GameObject characterObject, Color hairColor)
    {
        characterObject.GetComponent<Renderer>().material.color = hairColor;
    }

    #region Prefab Instantiation
    private void CreateBody(Odmg odmg, SkinColor skinColor)
    {
        GameObject bodyObject;

        if (odmg == Odmg.Blades)
            bodyObject = Instantiate(characterPrefabs.body[0]);
        else
            bodyObject = Instantiate(characterPrefabs.body[1]);

        CharacterTextures bodyTextures = bodyObject.GetComponent<CharacterTextures>();

        if (skinColor == SkinColor.Light)
            bodyTextures.SetTexture(0);
        else
            bodyTextures.SetTexture(1);

        AttachToArmature(bodyObject);
    }

    private void CreateFacePart(GameObject faceObject, int spriteID)
    {
        GameObject faceInstance =  Instantiate(faceObject);
        faceInstance.GetComponent<CharacterSpriteSheet>().SetSprite(spriteID);
        AttachToArmature(faceInstance);
    }

    private void CreateHair(Sex sex, int hairID, Color hairColor)
    {
        GameObject hairObject;

        if (sex == Sex.Male)
            hairObject = Instantiate(characterPrefabs.maleHair[hairID]);
        else
            hairObject = Instantiate(characterPrefabs.femaleHair[hairID]);

        SetColor(hairObject, hairColor);
        AttachToArmature(hairObject);
    }

    private void CreateOdmg(Odmg odmg, CharacterStat stat)
    {
        GameObject equipment;
        OdmgEquipment odmgScript;

        if (odmg == Odmg.Blades)
        {
            equipment = Instantiate(characterPrefabs.equipment[0]);
            odmgScript = equipment.GetComponent<Blades>();
        }
        else
        {
            equipment = Instantiate(characterPrefabs.equipment[1]);
            odmgScript = equipment.GetComponent<Ahss>();
        }

        odmgScript.SetHero(gameObject);
        odmgScript.SetStats(stat);

        equipment.transform.parent = transform;
    }

    private void CreateOutfit(Sex sex, OutfitType outfitType, int outfitID, int outfitTextureID)
    {
        List<GameObject> outfitList;

        if (sex == Sex.Male)
        {
            if (outfitType == OutfitType.Casual)
                outfitList = characterPrefabs.maleCasualOutfits;
            else
                outfitList = characterPrefabs.maleUniformOutfits;
        }
        else
        {
            if (outfitType == OutfitType.Casual)
                outfitList = characterPrefabs.femaleCasualOutfits;
            else
                outfitList = characterPrefabs.femaleUniformOutfits;
        }

        GameObject outfitObject = Instantiate(outfitList[(int) outfitType]);
        outfitObject.GetComponent<CharacterTextures>().SetTexture(outfitTextureID);
        outfitObject.transform.parent = transform;
    }

    private void CreateEmblems(Sex sex, Division division)
    {
        GameObject emblemObject;

        if (sex == Sex.Male)
            emblemObject = Instantiate(characterPrefabs.emblems[0]);
        else
            emblemObject = Instantiate(characterPrefabs.emblems[1]);

        emblemObject.GetComponent<CharacterTextures>().SetTexture((int) division);
        AttachToArmature(emblemObject);
    }

    private void CreateOptional(CharacterOutfit outfit)
    {
        List<GameObject> optionalOutfitList;

        if (outfit.outfitType == OutfitType.Casual)
            optionalOutfitList = characterPrefabs.optionalClothingCasual;
        else
            optionalOutfitList = characterPrefabs.optionalClothingUniform;

        if (outfit.capeEnabled)
        {
            GameObject clothingObject = optionalOutfitList[0];
            AttachToArmature(clothingObject);
        }

        else if (outfit.hoodEnabled)
        {
            GameObject clothingObject = optionalOutfitList[0];
            AttachToArmature(clothingObject);
        }

        else if (outfit.scarfEnabled)
        {
            GameObject clothingObject = optionalOutfitList[0];
            AttachToArmature(clothingObject);
        }
    }
    #endregion
}