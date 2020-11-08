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

        public CharacterOutfit CurrentOutfit { get; set; }

        private CharacterPrefabs Prefabs;

        public void Apply(Human human, CharacterPrefabs prefabs)
        {
            Prefabs = prefabs;
            CurrentOutfit = CharacterOutfit[0];

            var skin = Prefabs.GetSkinPrefab(CurrentOutfit.Skin.Skin);

            CreateHead(human.Body, CurrentOutfit.Head, skin);
            CreateHair(human.Body, CurrentOutfit.Hair);
            CreateEyes(human.Body, CurrentOutfit.Eyes);
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


    }
}
