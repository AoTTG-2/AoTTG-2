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
        private HumanBody Body;
        
        public void Apply(Human human, CharacterPrefabs prefabs)
        {
            Prefabs = prefabs;
            CurrentOutfit = CharacterOutfit[0];
            CurrentBuild = CharacterBuild[0];
            Body = human.Body;

            var skin = Prefabs.GetSkinPrefab(CurrentOutfit.Skin.Skin);

            CreateHead(CurrentOutfit.Head, skin);
            CreateHair(CurrentOutfit.Hair);
            CreateEyes(CurrentOutfit.Eyes);
            CreateOutfit(CurrentOutfit.Outfit);

            CreateEquipment();
        }

        private void CreateComponent(GameObject prefab, Texture2D texture, Color color = default, Transform parent = null)
        {
            var prefabObject = Instantiate(prefab);
            if (prefabObject.TryGetComponent(out SkinnedMeshRenderer meshRenderer))
            {
                meshRenderer.material.mainTexture = texture;
                if (color != default)
                    meshRenderer.material.color = color;

                meshRenderer.rootBone = Body.ControllerBody;
                meshRenderer.bones = Body.Bones;
                prefabObject.transform.parent = parent ?? Body.ControllerBody;
            }

            if (prefabObject.TryGetComponent(out Renderer renderer))
            {
                if (color != default)
                    renderer.material.color = color;

                renderer.material.mainTexture = texture;
                prefabObject.transform.parent = parent ?? Body.ControllerBody;
            }
        }

        private void CreateComponent(GameObject prefab, Texture2D texture, Transform parent)
        {
            CreateComponent(prefab, texture, default, parent);
        }

        private void CreateHead(HeadComponent head, SkinPrefab skin)
        {
            CreateComponent(Prefabs.GetHeadPrefab(head.Model).Prefab, skin.File, Body.head);
        }

        private void CreateHair(HairComponent hair)
        {
            var prefab = Prefabs.GetHairPrefab(hair.Model);
            var texture = prefab.GetTexture(hair.Texture);
            CreateComponent(prefab.Prefab, texture.File, hair.Color, Body.head);
        }

        private void CreateEyes(EyesComponent eyes)
        {
            var prefab = Prefabs.Eyes;
            var texture = prefab.GetTexture(eyes.Texture);
            CreateComponent(prefab.Prefab, texture.File, eyes.Color, Body.head);
        }

        private void CreateOutfit(OutfitComponent outfit)
        {
            var prefab = Prefabs.GetOutfitPrefab(outfit.Model);
            var texture = prefab.GetTexture(outfit.Texture);
            CreateComponent(prefab.Prefab, texture.File, outfit.Color);
            
            CreateLegs(outfit);
            CreateArms(outfit);
        }

        private void CreateLegs(OutfitComponent outfit)
        {
            var prefab = Prefabs.GetOutfitPrefab(outfit.Model);
            var texture = prefab.GetTexture(outfit.Texture);
            CreateComponent(Prefabs.Legs, texture.File, outfit.Color);
        }

        private void CreateArms(OutfitComponent outfit)
        {
            var prefab = Prefabs.GetOutfitPrefab(outfit.Model);
            var texture = prefab.GetTexture(outfit.Texture);

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

            CreateComponent(armLeft, texture.File, outfit.Color);
            CreateComponent(armRight, texture.File, outfit.Color);
        }

        private void CreateEquipment()
        {
            var prefab = Prefabs.GetEquipmentPrefab(CurrentBuild.Equipment);
            var skin = Prefabs.GetSkinPrefab(CurrentOutfit.Skin.Skin);

            var handLeft = Instantiate(prefab.HandLeft);
            var handRight = Instantiate(prefab.HandRight);

            handLeft.GetComponent<Renderer>().material.mainTexture = skin.File;
            handRight.GetComponent<Renderer>().material.mainTexture = skin.File;

            handLeft.transform.parent = Body.hand_L;
            handRight.transform.parent = Body.hand_R;

            CreateOdmg(CurrentBuild.EquipmentComponent);
        }

        private void CreateOdmg(EquipmentComponent equipment)
        {
            var prefab = Prefabs.GetEquipmentPrefab(CurrentBuild.Equipment);
            var texture = prefab.GetTexture(equipment.Texture);
            CreateComponent(prefab.Equipment, texture.File, equipment.Color, Body.chest);
        }
    }
}
