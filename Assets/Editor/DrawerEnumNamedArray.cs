using Assets.Scripts.Characters.Humans.Customization;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomPropertyDrawer(typeof(EnumNamedArrayAttribute))]
    public class DrawerEnumNamedArray : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            try
            {
                int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
                EnumNamedArrayAttribute enumNames = attribute as EnumNamedArrayAttribute;
                EditorGUI.PropertyField(position, property, new GUIContent((enumNames.Names[pos])), true);
            }
            catch
            {
                EditorGUI.ObjectField(position, property, label);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
