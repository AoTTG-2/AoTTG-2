using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Prefabs", menuName = "Character Prefabs")]
public class CharacterPrefabs : ScriptableObject
{
    [SerializeField] public List<GameObject> maleHair;
    [SerializeField] public List<GameObject> femaleHair;
    [SerializeField] public List<GameObject> maleUniforms;
    [SerializeField] public List<GameObject> femaleUniforms;
    [SerializeField] public List<GameObject> body;
    [SerializeField] public List<GameObject> equipment;
    [SerializeField] public List<GameObject> emblems;
}
