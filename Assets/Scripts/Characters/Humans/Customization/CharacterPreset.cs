using Assets.Scripts.Characters.Humans.Customization.Components;
using Assets.Scripts.Characters.Humans.Equipment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Characters.Humans.Customization
{
    /// <summary>
    /// A ScriptableObject which contains the Outfits and Builds for a single character
    /// </summary>
    [CreateAssetMenu(fileName = "New Character Preset", menuName = "Character/Character Preset")]
    public class CharacterPreset : ScriptableObject
    {
        [SerializeField] public string Name;
        [SerializeField] public string Description;
        [SerializeField] public List<CharacterOutfit> CharacterOutfit;
        [SerializeField] public List<CharacterBuild> CharacterBuild;

        /// <summary>
        /// The currently equipped outfit
        /// </summary>
        public CharacterOutfit CurrentOutfit { get; set; }
        /// <summary>
        /// The currently equipped build
        /// </summary>
        public CharacterBuild CurrentBuild { get; set; }

        private CharacterPrefabs Prefabs;
        private HumanBody Body;
        private Transform HumanTransform { get; set; }

        public void Apply(Human human, CharacterPrefabs prefabs)
        {
            Prefabs = prefabs;
            Body = human.Body;
            HumanTransform = human.transform;

            CreateHead(CurrentOutfit.Head);
            CreateEyes(CurrentOutfit.Eyes);
            CreateHair(CurrentOutfit.Hair);
            CreateHeadgear(CurrentOutfit.Headgear);
            CreateGlasses(CurrentOutfit.Glasses);
            CreateFacial(CurrentOutfit.Facial);

            CreateOutfit(CurrentOutfit.Outfit);
            CreateCape(CurrentOutfit.Cape);
            CreateEmblems();
            CreateEquipment(CurrentBuild.EquipmentComponent);

        }

        public void Apply(GameObject humanBase, CharacterPrefabs prefabs)
        {
            Prefabs = prefabs;
            CurrentOutfit = CharacterOutfit[0];
            CurrentBuild = CharacterBuild[0];
            Body = humanBase.GetComponentInChildren<HumanBody>();
            HumanTransform = humanBase.transform;

            CreateHead(CurrentOutfit.Head);
            CreateHair(CurrentOutfit.Hair);
            CreateGlasses(CurrentOutfit.Glasses);
            CreateFacial(CurrentOutfit.Facial);

            CreateOutfit(CurrentOutfit.Outfit);
            CreateCape(CurrentOutfit.Cape);
            CreateEmblems();
            CreateEquipment(CurrentBuild.EquipmentComponent);

            CreateEyes(CurrentOutfit.Eyes);
        }

        private GameObject CreateComponent(GameObject prefab, Texture2D texture, Color color = default, Transform parent = null)
        {
            var prefabObject = Instantiate(prefab);
            if (prefabObject.TryGetComponent(out SkinnedMeshRenderer meshRenderer))
            {
                meshRenderer.material.SetTexture("_MainTex", texture);
                //meshRenderer.material.mainTexture = texture;
                if (color != default)
                    meshRenderer.material.color = color;

                meshRenderer.rootBone = Body.ControllerBody;
                meshRenderer.bones = Body.Bones;
                prefabObject.transform.parent = parent ?? Body.ControllerBody;
                prefabObject.transform.position = HumanTransform.position;
                prefabObject.transform.rotation = Quaternion.Euler(270f, HumanTransform.rotation.eulerAngles.y, 0f);
            }

            if (prefabObject.TryGetComponent(out Renderer renderer))
            {
                if (renderer.material.shader.name.Contains("Two Sided Lit"))
                {
                    renderer.material.SetTexture("_MainTex", texture);
                }
                else
                {
                    renderer.material.mainTexture = texture;
                }

                if (color != default)
                    renderer.material.color = color;

                prefabObject.transform.parent = parent ?? Body.ControllerBody;
                prefabObject.transform.position = HumanTransform.position;
                prefabObject.transform.rotation = Quaternion.Euler(prefabObject.transform.rotation.eulerAngles.x, prefabObject.transform.rotation.eulerAngles.y, prefabObject.transform.rotation.eulerAngles.z);
            }

            return prefabObject;
        }

        private GameObject CreateComponent(GameObject prefab, Texture2D texture, Transform parent)
        {
            return CreateComponent(prefab, texture, default, parent);
        }

        private GameObject CreateComponentAsync(GameObject prefab, string textureUrl, Color color = default,
            Transform parent = null)
        {
            var result = CreateComponent(prefab, null, color, parent);
            if (result.TryGetComponent(out Renderer renderer))
            {
                Body.StartCoroutine(DownloadImage(renderer, textureUrl));
            }

            return result;
        }

        private void CreateHead(HumanHeadSelected head)
        {
            CreateComponent(head.Component.Model, head.Component.Textures[head.Texture], Body.head);
            CreateComponent(Prefabs.Chest, head.Component.Textures[head.Texture]);
        }

        private void CreateHair(HumanHairSelected hair)
        {
            if (hair.Component == null) return;
            var result = CreateComponent(hair.Component.Model, hair.Component.Textures[hair.Texture], hair.Color,Body.head);
            //var prefab = Prefabs.GetHairPrefab(hair.Model);

            //if (!string.IsNullOrWhiteSpace(hair.CustomUrl))
            //{
            //    CreateComponentAsync(prefab.Prefab, hair.CustomUrl, hair.Color, Body.head);
            //}
            //else
            //{
            //    var texture = Prefabs.GetHairTexture(hair.Texture);
            //    CreateComponent(prefab.Prefab, texture.File, hair.Color, Body.head);
            //}
        }

        private void CreateHeadgear(HeadgearSelected headgear)
        {
            if (headgear.Component == null) return;
            var result = CreateComponent(headgear.Component.Model, headgear.Component.Textures[headgear.Texture], Body.head);
            //var prefab = Prefabs.GetHairPrefab(hair.Model);

            //if (!string.IsNullOrWhiteSpace(hair.CustomUrl))
            //{
            //    CreateComponentAsync(prefab.Prefab, hair.CustomUrl, hair.Color, Body.head);
            //}
            //else
            //{
            //    var texture = Prefabs.GetHairTexture(hair.Texture);
            //    CreateComponent(prefab.Prefab, texture.File, hair.Color, Body.head);
            //}
        }
        
        private void CreateEyes(HumanEyesSelected eyes)
        {
            var result = CreateComponent(eyes.Component.Model, eyes.Component.Textures[eyes.Texture], Body.head);

            //var prefab = Prefabs.Eyes;

            //if (!string.IsNullOrWhiteSpace(eyes.CustomUrl))
            //{
            //    var result = CreateComponentAsync(prefab.Prefab, eyes.CustomUrl, eyes.Color, Body.head);
            //}
            //else
            //{
            //    var texture = prefab.GetTexture(eyes.Texture);
            //    CreateComponent(prefab.Prefab, texture.File, eyes.Color, Body.head);
            //}
        }

        private void CreateGlasses(HumanGlassesSelected glasses)
        {
            if (glasses.Component == null) return;
            var result = CreateComponent(glasses.Component.Model, glasses.Component.Textures[glasses.Texture], Body.head);

            //if (glasses.Texture == GlassesTexture.None) return;
            //var prefab = Prefabs.Glasses;

            //if (!string.IsNullOrWhiteSpace(glasses.CustomUrl))
            //{
            //    var result = CreateComponentAsync(prefab.Prefab, glasses.CustomUrl, glasses.Color, Body.head);
            //}
            //else
            //{
            //    var texture = prefab.GetTexture(glasses.Texture);
            //    CreateComponent(prefab.Prefab, texture.File, glasses.Color, Body.head);
            //}
        }

        private void CreateFacial(FacialSelected facial)
        {
            if (facial.Component == null) return;
            var result = CreateComponent(facial.Component.Model, facial.Component.Textures[facial.Texture], Body.head);
            //var prefab = Prefabs.Facial;
            //if (!string.IsNullOrWhiteSpace(facial.CustomUrl))
            //{
            //    var result = CreateComponentAsync(prefab.Prefab, facial.CustomUrl, facial.Color, Body.head);
            //}
            //else
            //{
            //    var texture = prefab.GetTexture(facial.Texture);
            //    CreateComponent(prefab.Prefab, texture.File, facial.Color, Body.head);
            //}
        }

        private void CreateOutfit(HumanOutfitSelected outfit)
        {
            var result = CreateComponent(outfit.Component.Model, outfit.Component.Textures[outfit.Texture]);

            //var prefab = Prefabs.GetOutfitPrefab(outfit.Model);
            //var texture = Prefabs.GetOutfitTexture(outfit.Texture);
            //CreateComponent(prefab.Prefab, texture.File, outfit.Color);

            CreateLegs(outfit);
            CreateArms(outfit);
        }

        private void CreateLegs(HumanOutfitSelected outfit)
        {
            CreateComponent(outfit.Component.Legs, outfit.Component.Textures[outfit.Texture]);
        }

        private void CreateArms(HumanOutfitSelected outfit)
        {
            CreateComponent(outfit.Component.ArmLeft.Model, outfit.Component.Textures[outfit.Texture]);
            CreateComponent(outfit.Component.ArmRight.Model, outfit.Component.Textures[outfit.Texture]);
            
            if (outfit.Component.EmblemFront != null && outfit.EmblemFront?.Component != null)
            {
                CreateComponent(outfit.Component.EmblemFront,
                    outfit.EmblemFront.Component.Textures[outfit.EmblemFront.Texture]);
            }

            if (outfit.Component.EmblemBack != null && outfit.EmblemBack?.Component != null)
            {
                CreateComponent(outfit.Component.EmblemBack,
                    outfit.EmblemBack.Component.Textures[outfit.EmblemBack.Texture]);
            }

            if (outfit.Component.ArmLeft != null && outfit.Component.ArmLeft.Emblem != null && outfit.EmblemLeft?.Component != null)
            {
                CreateComponent(outfit.Component.ArmLeft.Emblem,
                    outfit.EmblemLeft.Component.Textures[outfit.EmblemLeft.Texture]);
            }

            if (outfit.Component.ArmRight != null && outfit.Component.ArmRight.Emblem != null && outfit.EmblemRight?.Component != null)
            {
                CreateComponent(outfit.Component.ArmRight.Emblem,
                    outfit.EmblemRight.Component.Textures[outfit.EmblemRight.Texture]);
            }

        }

        private void CreateEquipment(EquipmentComponent equipment)
        {
            var prefab = Prefabs.GetEquipmentPrefab(CurrentBuild.Equipment);
            var skin = prefab.GetTexture(equipment.HandGrid.Texture);
            var ammo = prefab.GetTexture(equipment.Texture).File;

            var handLeft = Instantiate(prefab.HandLeft);
            var handRight = Instantiate(prefab.HandRight);
            var ammoLeft = Instantiate(prefab.AmmoLeft);
            var ammoRight = Instantiate(prefab.AmmoRight);
            var weaponLeft = Instantiate(prefab.WeaponLeft);
            var weaponRight = Instantiate(prefab.WeaponRight);

            handLeft.GetComponent<Renderer>().material.SetTexture("_MainTex", skin.File);
            handRight.GetComponent<Renderer>().material.SetTexture("_MainTex", skin.File);
            ammoLeft.GetComponent<Renderer>().material.mainTexture = ammo;
            ammoRight.GetComponent<Renderer>().material.mainTexture = ammo;
            weaponLeft.GetComponent<Renderer>().material.mainTexture = ammo;
            weaponRight.GetComponent<Renderer>().material.mainTexture = ammo;

            ammoLeft.transform.parent = CurrentBuild.Equipment == EquipmentType.Ahss
                ? Body.thigh_L
                : Body.ControllerBody;
            ammoLeft.transform.position = HumanTransform.position;
            ammoLeft.transform.rotation = Quaternion.Euler(270f, HumanTransform.rotation.eulerAngles.y, 0f);

            ammoRight.transform.parent = CurrentBuild.Equipment == EquipmentType.Ahss
                ? Body.thigh_R
                : Body.ControllerBody;
            ammoRight.transform.position = HumanTransform.position;
            ammoRight.transform.rotation = Quaternion.Euler(270f, HumanTransform.rotation.eulerAngles.y, 0f);

            handLeft.transform.parent = Body.hand_L;
            handLeft.transform.position = HumanTransform.position;
            handLeft.transform.rotation = Quaternion.Euler(270f, HumanTransform.rotation.eulerAngles.y, 0f);

            handRight.transform.parent = Body.hand_R;
            handRight.transform.position = HumanTransform.position;
            handRight.transform.rotation = Quaternion.Euler(270f, HumanTransform.rotation.eulerAngles.y, 0f);

            weaponLeft.transform.parent = Body.hand_L;
            weaponLeft.transform.position = HumanTransform.position;
            weaponLeft.transform.rotation = Quaternion.Euler(270f, HumanTransform.rotation.eulerAngles.y, 0f);

            weaponRight.transform.parent = Body.hand_R;
            weaponRight.transform.position = HumanTransform.position;
            weaponRight.transform.rotation = Quaternion.Euler(270f, HumanTransform.rotation.eulerAngles.y, 0f);

            CreateOdmg(equipment);
        }

        private void CreateOdmg(EquipmentComponent equipment)
        {
            var prefab = Prefabs.GetEquipmentPrefab(CurrentBuild.Equipment);
            var texture = prefab.GetTexture(equipment.Texture);
            var result = CreateComponent(prefab.Equipment, texture.File, equipment.Color);
            result.transform.parent = Body.chest;
            result.transform.position = HumanTransform.position;
            result.transform.rotation = Quaternion.Euler(270f, HumanTransform.rotation.eulerAngles.y, 0f);

            foreach (var extra in prefab.Extras)
            {
                result = CreateComponent(extra, texture.File, equipment.Color, HumanTransform);
                result.transform.parent = Body.ControllerBody;
                result.transform.position = HumanTransform.position;
            }
        }

        private void CreateCape(CapeSelected cape)
        {
            if (cape.Component == null) return;

            //TODO: Add Mikasa Cape
            if (cape.Component.Model.TryGetComponent<SkinnedMeshRenderer>(out _))
            {
                var capeObject = CreateComponent(cape.Component.Model, cape.Component.Textures[cape.Texture]);
                capeObject.AddComponent<ParentFollow>().SetParent(Body.transform.parent.transform);
                capeObject.transform.localScale = Vector3.one;
            }
            else
            {
                // The Hoodie (Mesh Renderer) needs to be instantiated like this
                var capeObject = CreateComponent(cape.Component.Model, cape.Component.Textures[cape.Texture], Body.neck);
            }
        }

        private void CreateEmblems()
        {
            //var texture = Prefabs.Cape.GetTexture(CurrentOutfit.Cape.Texture).File;
            //var prefab = Prefabs.Emblem;
            //var gender = CurrentOutfit.Gender;

            //var emblems = new List<GameObject>
            //    {prefab.ArmLeft, prefab.ArmRight, prefab.GetBackPrefab(gender), prefab.GetChestPrefab(gender)};

            //emblems.ForEach(x => CreateComponent(x, texture));
        }

        IEnumerator DownloadImage(Renderer renderer, string url)
        {
            var request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log($"Could not downloaded skin. Error: {request.error}");

            }
            else
            {
                renderer.material.mainTexture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            }
        }
    }
}
