using Assets.Scripts.Characters.Humans.Customization.Components;
using Assets.Scripts.Characters.Humans.Equipment;
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

        [EnumNamedArray(typeof(HeadModel), typeof(HeadPrefab))]
        [SerializeField]
        public List<HeadPrefab> Head;

        [SerializeField] public EyePrefab Eyes;
        [SerializeField] public GlassesPrefab Glasses;
        [SerializeField] public FacialPrefab Facial;

        [EnumNamedArray(typeof(HairModel), typeof(HairPrefab))]
        [SerializeField]
        public List<HairPrefab> Hair;

        [EnumNamedArray(typeof(OutfitModel), typeof(OutfitPrefab))]
        [SerializeField]
        public List<OutfitPrefab> Outfits;

        [EnumNamedArray(typeof(EquipmentType), typeof(EquipmentPrefab))]
        [SerializeField]
        public List<EquipmentPrefab> Equipment;
        [SerializeField] public List<SkinPrefab> Skin;

        [SerializeField] public GameObject Legs;
        [SerializeField] public GameObject Chest;
        [SerializeField] public ArmPrefab Arms;
        [SerializeField] public CapePrefab Cape;
        [SerializeField] public EmblemPrefab Emblem;

        [Header("Textures")]
        [EnumNamedArray(typeof(OutfitTexture), typeof(OutfitPrefabTexture))]
        [SerializeField] public List<OutfitPrefabTexture> OutfitTextures;

        [EnumNamedArray(typeof(HairTexture), typeof(HairPrefabTexture))]
        [SerializeField] public List<HairPrefabTexture> HairTextures;

        public HeadPrefab GetHeadPrefab(HeadModel model)
        {
            return Head.First();
        }

        public HairPrefab GetHairPrefab(HairModel model)
        {
            return Hair[(int) model];
        }

        public SkinPrefab GetSkinPrefab(Skin skin)
        {
            return Skin.First(x => x.Skin == skin);
        }

        public OutfitPrefab GetOutfitPrefab(OutfitModel outfit)
        {
            return Outfits[(int) outfit];
        }

        public EquipmentPrefab GetEquipmentPrefab(EquipmentType equipment)
        {
            return Equipment.First(x => x.EquipmentType == equipment);
        }

        public OutfitPrefabTexture GetOutfitTexture(OutfitTexture texture)
        {
            return OutfitTextures[(int) texture];
        }

        public HairPrefabTexture GetHairTexture(HairTexture texture)
        {
            return HairTextures[(int) texture];
        }
    }

    [Serializable]
    public struct HairPrefab
    {
        [SerializeField] public GameObject Prefab;
        [SerializeField] public Gender Gender;
        [SerializeField] public List<HairTexture> Textures;
    }

    [Serializable]
    public struct HairPrefabTexture
    {
        [SerializeField] public Texture2D File;
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
        }

        [SerializeField] public GameObject Prefab;

        [EnumNamedArray(typeof(EyesTexture), typeof(EyePrefabTexture))]
        [SerializeField]
        private List<EyePrefabTexture> Textures;

        public EyePrefabTexture GetTexture(EyesTexture texture)
        {
            return Textures[(int) texture];
        }
    }

    [Serializable]
    public struct GlassesPrefab
    {
        [Serializable]
        public struct GlassesPrefabTexture
        {
            [SerializeField] public Texture2D File;
        }

        [SerializeField] public GameObject Prefab;

        [EnumNamedArray(typeof(GlassesTexture), typeof(GlassesPrefabTexture))]
        [SerializeField]
        private List<GlassesPrefabTexture> Textures;

        public GlassesPrefabTexture GetTexture(GlassesTexture texture)
        {
            return Textures[(int) texture];
        }
    }

    [Serializable]
    public struct FacialPrefab
    {
        [Serializable]
        public struct FacialPrefabTexture
        {
            [SerializeField] public Texture2D File;
        }

        [SerializeField] public GameObject Prefab;

        [EnumNamedArray(typeof(FacialTexture), typeof(FacialPrefabTexture))]
        [SerializeField]
        private List<FacialPrefabTexture> Textures;

        public FacialPrefabTexture GetTexture(FacialTexture texture)
        {
            return Textures[(int) texture];
        }
    }

    [Serializable]
    public struct OutfitPrefab
    {
        [SerializeField] public GameObject Prefab;
        [SerializeField] public Gender Gender;
        [SerializeField] public List<OutfitTexture> Textures;
    }

    [Serializable]
    public struct OutfitPrefabTexture
    {
        [SerializeField] public Texture2D File;
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

    [Serializable]
    public struct EquipmentPrefab
    {
        public string Name;
        public GameObject HandLeft;
        public GameObject HandRight;
        public GameObject AmmoLeft;
        public GameObject AmmoRight;
        public GameObject WeaponLeft;
        public GameObject WeaponRight;
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
