using Assets.Scripts.Characters.Humans.Customization.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
    [CreateAssetMenu(fileName = "New Character Prefabs", menuName = "Character/Character Prefabs")]
    public class CharacterPrefabs : ScriptableObject
    {
        [SerializeField] public GameObject Base;
        [Header("Face")] 
        [SerializeField] public List<HeadPrefab> Head;
        [SerializeField] public EyePrefab Eyes;
        [SerializeField] public GameObject Glasses;
        [SerializeField] public GameObject Mouth;
        [SerializeField] public List<HairPrefab> Hair;
        [SerializeField] public List<OutfitPrefab> Outfits;
        [SerializeField] public List<GameObject> OptionalClothingCasual;
        [SerializeField] public List<GameObject> OptionalClothingUniform;
        [SerializeField] public List<EquipmentPrefab> Equipment;
        [SerializeField] public List<SkinPrefab> Skin;

        [SerializeField] public GameObject Legs;
        [SerializeField] public GameObject Chest;
        [SerializeField] public ArmPrefab Arms;
        [SerializeField] public CapePrefab Cape;
        [SerializeField] public EmblemPrefab Emblem;
        
        public HeadPrefab GetHeadPrefab(HeadModel model)
        {
            return Head.First(x => x.Model == model);
        }

        public HairPrefab GetHairPrefab(HairModel model)
        {
            return Hair.First(x => x.Model == model);
        }

        public SkinPrefab GetSkinPrefab(Skin skin)
        {
            return Skin.First(x => x.Skin == skin);
        }

        public OutfitPrefab GetOutfitPrefab(OutfitModel outfit)
        {
            return Outfits.First(x => x.Model == outfit);
        }

        public EquipmentPrefab GetEquipmentPrefab(EquipmentType equipment)
        {
            return Equipment.First(x => x.EquipmentType == equipment);
        }
    }

    [Serializable]
    public struct HairPrefab
    {
        [SerializeField] public GameObject Prefab;
        [SerializeField] public HairModel Model;
        [SerializeField] public Gender Gender;
        [SerializeField] public List<HairPrefabTexture> Textures;

        public HairPrefabTexture GetTexture(HairTexture texture)
        {
            return Textures.FirstOrDefault(x => x.Texture == texture);
        }
    }

    [Serializable]
    public struct HairPrefabTexture
    {
        [SerializeField] public Texture2D File;
        [SerializeField] public HairTexture Texture;
    }

    [Serializable]
    public struct HeadPrefab
    {
        [SerializeField] public GameObject Prefab;
        [SerializeField] public HeadModel Model;
    }

    [Serializable]
    public struct SkinPrefab
    {
        [SerializeField] public Texture2D File;
        [SerializeField] public Skin Skin;
    }

    [Serializable]
    public struct EyePrefab
    {
        [Serializable]
        public struct EyePrefabTexture
        {
            [SerializeField] public Texture2D File;
            [SerializeField] public EyesTexture Texture;
        }

        [SerializeField] public GameObject Prefab;
        [SerializeField] private List<EyePrefabTexture> Textures;

        public EyePrefabTexture GetTexture(EyesTexture texture)
        {
            return Textures.First(x => x.Texture == texture);
        }


    }

    [Serializable]
    public struct OutfitPrefab
    {
        [SerializeField] public GameObject Prefab;
        [SerializeField] public OutfitModel Model;
        [SerializeField] public Gender Gender;
        [SerializeField] public List<OutfitPrefabTexture> Textures;

        public OutfitPrefabTexture GetTexture(OutfitTexture texture)
        {
            return Textures.FirstOrDefault(x => x.Texture == texture);
        }

        [Serializable]
        public struct OutfitPrefabTexture
        {
            [SerializeField] public Texture2D File;
            [SerializeField] public OutfitTexture Texture;
        }
    }

    [Serializable]
    public struct ArmPrefab
    {
        [SerializeField] public GameObject LeftCasual;
        [SerializeField] public GameObject LeftAhss;
        [SerializeField] public GameObject LeftUniform;
        [SerializeField] public GameObject RightCasual;
        [SerializeField] public GameObject RightAhss;
        [SerializeField] public GameObject RightUniform;
    }

    //TODO: Equipment Extras (Gas Canisters for blades, Ammo rounds for AHSS)

    [Serializable]
    public struct EquipmentPrefab
    {
        public string Name;
        public GameObject HandLeft;
        public GameObject HandRight;
        public GameObject AmmoLeft;
        public GameObject AmmoRight;
        public GameObject Equipment;
        public List<GameObject> Extras;
        public EquipmentType EquipmentType;
        public List<EquipmentPrefabTexture> EquipmentTextures;
        public List<HandGripPrefabTexture> HandGripTextures;

        public EquipmentPrefabTexture GetTexture(EquipmentTexture texture)
        {
            return EquipmentTextures.FirstOrDefault(x => x.Texture == texture);
        }

        public HandGripPrefabTexture GetTexture(HandGripTexture handGrip)
        {
            return HandGripTextures.FirstOrDefault(x => x.Texture == handGrip);
        }

        [Serializable]
        public struct EquipmentPrefabTexture
        {
            [SerializeField] public Texture2D File;
            [SerializeField] public EquipmentTexture Texture;
        }

        [Serializable]
        public struct HandGripPrefabTexture
        {
            [SerializeField] public Texture2D File;
            [SerializeField] public HandGripTexture Texture;
        }
    }

    [Serializable]
    public struct CapePrefab
    {
        [SerializeField] public GameObject Prefab;
        [SerializeField] public List<CapePrefabTexture> Textures;

        public CapePrefabTexture GetTexture(CapeTexture texture)
        {
            return Textures.FirstOrDefault(x => x.Texture == texture);
        }

        [Serializable]
        public struct CapePrefabTexture
        {
            [SerializeField] public Texture2D File;
            [SerializeField] public CapeTexture Texture;
        }
    }

    [Serializable]
    public struct EmblemPrefab
    {
        [SerializeField] public GameObject ArmLeft;
        [SerializeField] public GameObject ArmRight;
        [SerializeField] public GameObject BackMale;
        [SerializeField] public GameObject BackFemale;
        [SerializeField] public GameObject ChestMale;
        [SerializeField] public GameObject ChestFemale;

        public GameObject GetBackPrefab(Gender gender)
        {
            return gender == Gender.Female
                ? BackFemale
                : BackMale;
        }

        public GameObject GetChestPrefab(Gender gender)
        {
            return gender == Gender.Female
                ? ChestFemale
                : ChestMale;
        }
    }
}
