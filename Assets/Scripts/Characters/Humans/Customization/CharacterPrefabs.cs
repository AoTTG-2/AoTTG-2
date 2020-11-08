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
        [SerializeField] public List<GameObject> MaleCasualOutfits;
        [SerializeField] public List<GameObject> MaleUniformOutfits;
        [SerializeField] public List<GameObject> FemaleCasualOutfits;
        [SerializeField] public List<GameObject> FemaleUniformOutfits;
        [SerializeField] public List<GameObject> OptionalClothingCasual;
        [SerializeField] public List<GameObject> OptionalClothingUniform;
        [SerializeField] public List<GameObject> Emblems;
        [SerializeField] public List<GameObject> Equipment;
        [SerializeField] public List<SkinPrefab> Skin;

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
}
