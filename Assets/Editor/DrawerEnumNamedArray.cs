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

            //EnumNamedArrayAttribute enumNames = attribute as EnumNamedArrayAttribute;
            ////propertyPath returns something like component_hp_max.Array.data[4]
            ////so get the index from there
            //int index = System.Convert.ToInt32(property.propertyPath.Substring(property.propertyPath.IndexOf("[")).Replace("[", "").Replace("]", ""));
            ////change the label
            //label.text = enumNames.Names[index];
            ////draw field
            //EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
