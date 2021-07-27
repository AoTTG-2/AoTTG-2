using Assets.Scripts.Characters.Humans.Customization.Components;
using Assets.Scripts.Characters.Humans.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
    /// <summary>
    /// A ScriptableObject which contains references to all character component prefabs
    /// </summary>
    [CreateAssetMenu(fileName = "New Character Prefabs", menuName = "Character/Character Prefabs")]
    public class CharacterPrefabs : ScriptableObject
    {
        [SerializeField] public GameObject Base;
        [Header("Face")]

        [SerializeField] public HumanHeadComponent Head;
        [SerializeField] public List<HumanHairComponent> Hair;
        [SerializeField] public List<HumanEyesComponent> Eyes;
        [SerializeField] public List<HumanGlassesComponent> Glasses;
        [SerializeField] public List<FacialComponent> Facial;
        [SerializeField] public List<HeadgearComponent> Headgear;

        [Header("Body")]
        [SerializeField] public List<HumanOutfitComponent> Outfits;

        [EnumNamedArray(typeof(EquipmentType), typeof(EquipmentPrefab))]
        [SerializeField]
        public List<EquipmentPrefab> Equipment;
        [SerializeField] public List<SkinPrefab> Skin;

        [SerializeField] public GameObject Legs;
        [SerializeField] public GameObject Chest;
        [SerializeField] public List<CapeComponent> Cape;
        [SerializeField] public EmblemPrefab Emblem;
        
        public SkinPrefab GetSkinPrefab(Skin skin)
        {
            return Skin.First(x => x.Skin == skin);
        }

        public EquipmentPrefab GetEquipmentPrefab(EquipmentType equipment)
        {
            return Equipment.First(x => x.EquipmentType == equipment);
        }
    }
    
    [Serializable]
    public struct HairPrefabTexture
    {
        [SerializeField] public Texture2D File;
    }
    
    [Serializable]
    public struct SkinPrefab
    {
        [SerializeField] public Texture2D File;
        [SerializeField] public Skin Skin;
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
