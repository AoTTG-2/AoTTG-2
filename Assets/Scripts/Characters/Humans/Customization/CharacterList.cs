using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
    /// <summary>
    /// A list of all available characters that can be spawned
    /// </summary>
    [CreateAssetMenu(fileName = "Character List", menuName = "Character/Character List")]
    public class CharacterList : ScriptableObject
    {
        public List<CharacterPreset> Characters;
    }
}
