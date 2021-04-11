using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
    [CreateAssetMenu(fileName = "Character List", menuName = "Character/Character List")]
    public class CharacterList : ScriptableObject
    {
        public List<CharacterPreset> Characters;
    }
}
