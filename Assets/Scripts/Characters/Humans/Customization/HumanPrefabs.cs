using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
    [CreateAssetMenu(fileName = "New Character Prefabs", menuName = "Character/Character Prefabs")]
    public class HumanPrefabs : ScriptableObject
    {
        [SerializeField] public List<GameObject> Body;
        [Header("Face")]
        [SerializeField] public GameObject Eyes;
        [SerializeField] public GameObject Glasses;
        [SerializeField] public GameObject Mouth;
        [SerializeField] public List<GameObject> MaleHair;
        [SerializeField] public List<GameObject> FemaleHair;
        [SerializeField] public List<GameObject> MaleCasualOutfits;
        [SerializeField] public List<GameObject> MaleUniformOutfits;
        [SerializeField] public List<GameObject> FemaleCasualOutfits;
        [SerializeField] public List<GameObject> FemaleUniformOutfits;
        [SerializeField] public List<GameObject> OptionalClothingCasual;
        [SerializeField] public List<GameObject> OptionalClothingUniform;
        [SerializeField] public List<GameObject> Emblems;
        [SerializeField] public List<GameObject> Equipment;
    }
}
