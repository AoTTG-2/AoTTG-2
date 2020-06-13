using System;
using UnityEditor;
using UnityEngine;

namespace SuperSystems.UnityBuild
{

[CustomPropertyDrawer(typeof(ProductParameters))]
public class ProductParametersDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, GUIContent.none, property);

        EditorGUILayout.BeginHorizontal();

        bool show = property.isExpanded;
        UnityBuildGUIUtility.DropdownHeader("Product Parameters", ref show, false, GUILayout.ExpandWidth(true));
        property.isExpanded = show;

        UnityBuildGUIUtility.HelpButton("Parameter-Details#product-parameters");
        EditorGUILayout.EndHorizontal();

        if (show)
        {
            EditorGUILayout.BeginVertical(UnityBuildGUIUtility.dropdownContentStyle);

            if (BuildSettings.versionManager)
            {
                EditorGUILayout.LabelField("Version", BuildSettings.versionManager.Version);
                if (BuildSettings.versionManager.BranchNameDirty)
                {
                    BuildSettings.productParameters.buildCounter = 0;
                    BuildSettings.versionManager.BranchNameDirty = false;
                }
            }
            else
            {
                EditorGUILayout.LabelField("Version", "None");

                BuildNotificationList.instance.AddNotification(new BuildNotification(
                BuildNotification.Category.Error,
                "Version Manager isn't set.",
                "Please set the VersionManager in the BuildSettings.",
                true, null));
            }

            EditorGUILayout.PropertyField(property.FindPropertyRelative("buildCounter"));

            if (GUILayout.Button("Reset Build Counter", GUILayout.ExpandWidth(true)))
                property.FindPropertyRelative("buildCounter").intValue = 0;

            property.serializedObject.ApplyModifiedProperties();

            EditorGUILayout.EndVertical();
        }

        EditorGUI.EndProperty();
    }
}

}