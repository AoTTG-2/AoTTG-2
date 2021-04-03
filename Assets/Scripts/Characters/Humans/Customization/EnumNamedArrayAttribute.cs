using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization
{
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
