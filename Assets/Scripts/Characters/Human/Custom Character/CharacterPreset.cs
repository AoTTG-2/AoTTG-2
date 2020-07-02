using UnityEngine;

[CreateAssetMenu(fileName = "New Character Preset", menuName = "Character/Character Preset")]
public class CharacterPreset : ScriptableObject
{
    [SerializeField] private string presetName;
    [SerializeField] public CharacterOutfit characterOutfit;
    [SerializeField] public CharacterStat characterStat;
}