using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
    /// <summary>
    /// Uses an enum for Unity.PropertyAttribute
    /// </summary>
    public class EnumNamedArrayAttribute : PropertyAttribute
    {
        public string[] Names;
        public Type ObjectType;
        public EnumNamedArrayAttribute(Type enumType, Type objectType)
        {
            Names = Enum.GetNames(enumType);
            ObjectType = objectType;
        }
    }
}
