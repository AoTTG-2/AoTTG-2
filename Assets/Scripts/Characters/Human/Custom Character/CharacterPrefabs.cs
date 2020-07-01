using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Prefabs", menuName = "Character Prefabs")]
public class CharacterPrefabs : ScriptableObject
{
    [SerializeField] public List<GameObject> body;
    [Header("Face")]
    [SerializeField] public GameObject eyes;
    [SerializeField] public GameObject glasses;
    [SerializeField] public GameObject mouth;
    [SerializeField] public List<GameObject> maleHair;
    [SerializeField] public List<GameObject> femaleHair;
    [SerializeField] public List<GameObject> maleCasualOutfits;
    [SerializeField] public List<GameObject> maleUniformOutfits;
    [SerializeField] public List<GameObject> femaleCasualOutfits;
    [SerializeField] public List<GameObject> femaleUniformOutfits;
    [SerializeField] public List<GameObject> optionalClothingCasual;
    [SerializeField] public List<GameObject> optionalClothingUniform;
    [SerializeField] public List<GameObject> emblems;
    [SerializeField] public List<GameObject> equipment;
}
