// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AmplifyShaderEditor
{
	public class Preferences
	{
#if UNITY_2019_1_OR_NEWER
		[SettingsProvider]
		public static SettingsProvider ImpostorsSettings()
		{
			var provider = new SettingsProvider( "Preferences/Amplify Shader Editor", SettingsScope.User )
			{
				guiHandler = ( string searchContext ) =>
				{
					PreferencesGUI();
				}
			};
			return provider;
		}
#else
		[PreferenceItem( "Amplify Shader Editor" )]
#endif
		public static void PreferencesGUI()
		{
			bool startup = EditorPrefs.GetBool( "ASELastSession", true );
			EditorGUI.BeginChangeCheck();
			startup = EditorGUILayout.Toggle( "Start screen on Unity start", startup );
			if( EditorGUI.EndChangeCheck() )
			{
				EditorPrefs.SetBool( "ASELastSession", startup );
			}
		}
	}
}
