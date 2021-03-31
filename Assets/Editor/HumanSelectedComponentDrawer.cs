using Assets.Scripts.Characters.Humans.Customization.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomPropertyDrawer(typeof(HumanSelectedComponent<>), true)]
    public class HumanSelectedComponentDrawer : PropertyDrawer
    {
        private int Selected { get; set; } = 0;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var amountRect = new Rect(position.x, position.y, 100, position.height);
            var unitRect = new Rect(position.x + 105, position.y, 100, position.height);
            var spriteRect = new Rect(position.x + 205, position.y, 50, position.height);
            var colorRect = new Rect(position.x + 345, position.y, 75, position.height);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            var component = property.FindPropertyRelative("Component");
            var texture = property.FindPropertyRelative("Texture");

            EditorGUI.PropertyField(amountRect, component, GUIContent.none);

            if (component.propertyType == SerializedPropertyType.ObjectReference)
            {
                var objectData = component.objectReferenceValue as HumanComponent;

                if (texture.propertyType == SerializedPropertyType.Integer)
                {
                    Selected = texture.intValue;
                }

                if (objectData != null && objectData.Textures.Count > 0)
                {
                    Selected = EditorGUI.Popup(unitRect, Selected, objectData.Textures.Select(x => x.name).ToArray());
                    EditorGUI.BeginDisabledGroup(true);

                    if (Selected > objectData.Textures.Count - 1
                        || Selected < 0)
                    {
                        Selected = 0;
                    }

                    EditorGUI.ObjectField(spriteRect, objectData.Textures[Selected], typeof(Texture2D), false);
                    EditorGUI.EndDisabledGroup();
                    spriteRect.x += spriteRect.width + 10;
                    spriteRect.width += 15;
                    if (GUI.Button(spriteRect, "Select"))
                        ObjectBrowser.Open(objectData.Textures, i => Selected = i);
                }

                if (objectData is HumanOutfitComponent outfitComponent)
                {
                    Rect emblemRect = spriteRect;
                    if (outfitComponent.EmblemFront != null)
                    {
                        emblemRect = DrawEmblemComponent(property.FindPropertyRelative(nameof(HumanOutfitSelected.EmblemFront)), emblemRect);
                    }

                    if (outfitComponent.EmblemBack != null)
                    {
                        emblemRect = DrawEmblemComponent(property.FindPropertyRelative(nameof(HumanOutfitSelected.EmblemBack)), emblemRect);
                    }

                    if (outfitComponent.ArmLeft != null && outfitComponent.ArmLeft.Emblem != null)
                    {
                        emblemRect = DrawEmblemComponent(property.FindPropertyRelative(nameof(HumanOutfitSelected.EmblemLeft)), emblemRect);
                    }

                    if (outfitComponent.ArmRight != null && outfitComponent.ArmRight.Emblem != null)
                    {
                        emblemRect = DrawEmblemComponent(property.FindPropertyRelative(nameof(HumanOutfitSelected.EmblemRight)), emblemRect);
                    }
                }
            }

            if (texture.propertyType == SerializedPropertyType.Integer)
            {
                texture.intValue = Selected;
            }

            var color = property.FindPropertyRelative("Color");
            if (color?.propertyType == SerializedPropertyType.Color)
            {
                color.colorValue = EditorGUI.ColorField(colorRect, color.colorValue);
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        /// <summary>
        /// Gets visible children of `SerializedProperty` at 1 level depth.
        /// </summary>
        /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
        /// <returns>Collection of `SerializedProperty` children.</returns>
        public static IEnumerable<SerializedProperty> GetVisibleChildren(SerializedProperty serializedProperty)
        {
            SerializedProperty currentProperty = serializedProperty.Copy();
            SerializedProperty nextSiblingProperty = serializedProperty.Copy();
            {
                nextSiblingProperty.NextVisible(false);
            }

            if (currentProperty.NextVisible(true))
            {
                do
                {
                    if (SerializedProperty.EqualContents(currentProperty, nextSiblingProperty))
                        break;

                    yield return currentProperty;
                }
                while (currentProperty.NextVisible(false));
            }
        }

        private Rect DrawEmblemComponent(SerializedProperty property, Rect position)
        {
            position.x += position.width;
            var emblemComp = property.FindPropertyRelative(nameof(EmblemSelected.Component));
            var emblemComponent = (EmblemComponent) EditorGUI.ObjectField(position, emblemComp.objectReferenceValue, typeof(EmblemComponent), allowSceneObjects: false);
            emblemComp.objectReferenceValue = emblemComponent;

            if (emblemComp.objectReferenceValue == null) return position;
            var selectedTexture = 0;
            var texture = property.FindPropertyRelative(nameof(EmblemSelected.Texture));
            if (texture.propertyType == SerializedPropertyType.Integer)
            {
                selectedTexture = texture.intValue;
            }
            position.x += position.width;
            texture.intValue = EditorGUI.Popup(position, selectedTexture, emblemComponent.Textures.Select(x => x.name).ToArray());
            return position;
        }
    }
}
