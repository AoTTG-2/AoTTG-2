using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Characters.Humans.Customization.Components;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
    [CreateAssetMenu(fileName = "New Character Preset", menuName = "Character/Character Preset")]
    public class CharacterPreset : ScriptableObject
    {
        [SerializeField] public string Name;
        [SerializeField] public string Description;
        [SerializeField] public List<CharacterOutfit> CharacterOutfit;
        [SerializeField] public List<CharacterBuild> CharacterBuild;

        public CharacterOutfit CurrentOutfit { get; set; }
        public CharacterBuild CurrentBuild { get; set; }

        private CharacterPrefabs Prefabs;

        public void Apply(Human human, CharacterPrefabs prefabs)
        {
            Prefabs = prefabs;
            CurrentOutfit = CharacterOutfit[0];

            var skin = Prefabs.GetSkinPrefab(CurrentOutfit.Skin.Skin);

            CreateHead(human.Body, CurrentOutfit.Head, skin);
            CreateHair(human.Body, CurrentOutfit.Hair);
            CreateEyes(human.Body, CurrentOutfit.Eyes);
            CreateOutfit(human.Body, CurrentOutfit.Outfit);

            CreateEquipment(human.Body);
        }

        private List<GameObject> HeadObjects { get; } = new List<GameObject>();
        private void CreateHead(HumanBody body, HeadComponent head, SkinPrefab skin)
        {
            HeadObjects.ForEach(Destroy);
            HeadObjects.Clear();

            var prefab = Prefabs.GetHeadPrefab(head.Model);
            var headObject = Instantiate(prefab.Prefab);
            var renderer = headObject.GetComponent<Renderer>();

            renderer.material.mainTexture = skin.File;
            headObject.transform.parent = body.Head;

            HeadObjects.Add(headObject);
        }

        private List<GameObject> HairObjects { get; } = new List<GameObject>();
        private void CreateHair(HumanBody body, HairComponent hair)
        {
            HairObjects.ForEach(Destroy);
            HairObjects.Clear();

            var prefab = Prefabs.GetHairPrefab(hair.Model);
            var hairObject = Instantiate(prefab.Prefab);
            var renderer = hairObject.GetComponent<Renderer>();

            if (hair.Color != default)
                renderer.material.color = hair.Color;

            renderer.material.mainTexture = prefab.GetTexture(hair.Texture).File;
            hairObject.transform.parent = body.Head;

            HairObjects.Add(hairObject);
        }

        private List<GameObject> EyeObjects { get; } = new List<GameObject>();
        private void CreateEyes(HumanBody body, EyesComponent eyes)
        {
            EyeObjects.ForEach(Destroy);
            EyeObjects.Clear();

            var prefab = Prefabs.Eyes;
            var hairObject = Instantiate(prefab.Prefab);
            var renderer = hairObject.GetComponent<Renderer>();

            if (eyes.Color != default)
                renderer.material.color = eyes.Color;

            renderer.material.mainTexture = prefab .GetTexture(eyes.Texture).File;
            hairObject.transform.parent = body.Head;

            EyeObjects.Add(hairObject);
        }

        private void CreateOutfit(HumanBody body, OutfitComponent outfit)
        {
            var prefab = Prefabs.GetOutfitPrefab(outfit.Model);
            var hairObject = Instantiate(prefab.Prefab);
            var renderer = hairObject.GetComponent<Renderer>();

            if (outfit.Color != default)
                renderer.material.color = outfit.Color;

            renderer.material.mainTexture = prefab.GetTexture(outfit.Texture).File;
            hairObject.transform.parent = body.Chest;

            CreateLegs(body, outfit);
            CreateArms(body, outfit);
        }

        private void CreateLegs(HumanBody body, OutfitComponent outfit)
        {
            var prefab = Prefabs.GetOutfitPrefab(outfit.Model);
            var legObject = Instantiate(Prefabs.Legs);
            var renderer = legObject.GetComponent<Renderer>();

            renderer.material.mainTexture = prefab.GetTexture(outfit.Texture).File;
            legObject.transform.parent = body.Chest;
        }

        private void CreateArms(HumanBody body, OutfitComponent outfit)
        {
            var prefab = Prefabs.GetOutfitPrefab(outfit.Model);

            GameObject armLeft, armRight;
            if (outfit.Model == OutfitModel.CasualFemaleA || outfit.Model == OutfitModel.CasualMaleA)
            {
                armLeft = Prefabs.Arms.LeftCasual;
                armRight = Prefabs.Arms.RightCasual;
            } else if (outfit.Model == OutfitModel.CasualFemaleB || outfit.Model == OutfitModel.CasualMaleB)
            {
                armLeft = Prefabs.Arms.LeftAhss;
                armRight = Prefabs.Arms.RightAhss;
            }
            else
            {
                armLeft = Prefabs.Arms.LeftUniform;
                armRight = Prefabs.Arms.RightUniform;
            }

            armLeft = Instantiate(armLeft);
            armRight = Instantiate(armRight);

            var outfitTexture = prefab.GetTexture(outfit.Texture).File;

            armLeft.GetComponent<Renderer>().material.mainTexture = outfitTexture;
            armRight.GetComponent<Renderer>().material.mainTexture = outfitTexture;

            armLeft.transform.parent = body.ShoulderLeft;
            armRight.transform.parent = body.ShoulderRight;
        }

        private void CreateEquipment(HumanBody body)
        {
            var prefab = Prefabs.GetEquipmentPrefab(CurrentBuild.Equipment);
            var skin = Prefabs.GetSkinPrefab(CurrentOutfit.Skin.Skin);

            var handLeft = Instantiate(prefab.HandLeft);
            var handRight = Instantiate(prefab.HandRight);
            var odmg = Instantiate(prefab.Equipment);

            handLeft.GetComponent<Renderer>().material.mainTexture = skin.File;
            handRight.GetComponent<Renderer>().material.mainTexture = skin.File;

            handLeft.transform.parent = body.HandLeft;
            handRight.transform.parent = body.HandRight;
            odmg.transform.parent = body.Chest;
        }
    }
}
