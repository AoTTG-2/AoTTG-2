// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace AmplifyShaderEditor
{
	public class Preferences
	{
		public enum ShowOption
		{
			Always = 0,
			OnNewVersion = 1,
			Never = 2
		}

		private static readonly GUIContent StartUp = new GUIContent( "Show start screen on Unity launch", "You can set if you want to see the start screen everytime Unity launchs, only just when there's a new version available or never." );
		public static readonly string PrefStartUp = "ASELastSession" + Application.productName;
		public static ShowOption GlobalStartUp = ShowOption.Always;

		private static readonly GUIContent AutoSRP = new GUIContent( "Auto import SRP shader templates", "By default Amplify Shader Editor checks for your SRP version and automatically imports the correct corresponding shader templates.\nTurn this OFF if you prefer to import them manually." );
		public static readonly string PrefAutoSRP = "ASEAutoSRP" + Application.productName;
		public static bool GlobalAutoSRP = true;

		private static readonly GUIContent DefineSymbol = new GUIContent( "Add Amplify Shader Editor define symbol", "Turning it OFF will disable the automatic insertion of the define symbol and remove it from the list while turning it ON will do the opposite.\nThis is used for compatibility with other plugins, if you are not sure if you need this leave it ON." );
		public static readonly string PrefDefineSymbol = "ASEDefineSymbol" + Application.productName;
		public static bool GlobalDefineSymbol = true;

		private static readonly GUIContent ClearLog = new GUIContent( "Clear Log on Update", "Clears the previously generated log each time the Update button is pressed" );
		public static readonly string PrefClearLog = "ASEClearLog" + Application.productName;
		public static bool GlobalClearLog = true;

		private static bool PrefsLoaded = false;

#if UNITY_2019_1_OR_NEWER
		[SettingsProvider]
		public static SettingsProvider ImpostorsSettings()
		{
			var provider = new SettingsProvider( "Preferences/Amplify Shader Editor", SettingsScope.User )
			{
				guiHandler = ( string searchContext ) =>
				{
					PreferencesGUI();
				},

				keywords = new HashSet<string>( new[] { "start", "screen", "import", "shader", "templates", "macros", "macros", "define", "symbol" } ),

			};
			return provider;
		}
#else
		[PreferenceItem( "Amplify Shader Editor" )]
#endif
		public static void PreferencesGUI()
		{
			if( !PrefsLoaded )
			{
				LoadDefaults();
				PrefsLoaded = true;
			}

			var cache = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 250;
			EditorGUI.BeginChangeCheck();
			GlobalStartUp = (ShowOption)EditorGUILayout.EnumPopup( StartUp, GlobalStartUp );
			if( EditorGUI.EndChangeCheck() )
			{
				EditorPrefs.SetInt( PrefStartUp, (int)GlobalStartUp );
			}

			EditorGUI.BeginChangeCheck();
			GlobalAutoSRP = EditorGUILayout.Toggle( AutoSRP, GlobalAutoSRP );
			if( EditorGUI.EndChangeCheck() )
			{
				EditorPrefs.SetBool( PrefAutoSRP, GlobalAutoSRP );
			}

			EditorGUI.BeginChangeCheck();
			GlobalDefineSymbol = EditorGUILayout.Toggle( DefineSymbol, GlobalDefineSymbol );
			if( EditorGUI.EndChangeCheck() )
			{
				EditorPrefs.SetBool( PrefDefineSymbol, GlobalDefineSymbol );
				if( GlobalDefineSymbol )
					IOUtils.SetAmplifyDefineSymbolOnBuildTargetGroup( EditorUserBuildSettings.selectedBuildTargetGroup );
				else
					IOUtils.RemoveAmplifyDefineSymbolOnBuildTargetGroup( EditorUserBuildSettings.selectedBuildTargetGroup );
			}

			EditorGUI.BeginChangeCheck();
			GlobalClearLog = EditorGUILayout.Toggle( ClearLog, GlobalClearLog );
			if( EditorGUI.EndChangeCheck() )
			{
				EditorPrefs.SetBool( PrefClearLog, GlobalClearLog );
			}


			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if( GUILayout.Button( "Reset and Forget All" ) )
			{
				EditorPrefs.DeleteKey( PrefStartUp );
				GlobalStartUp = ShowOption.Always;

				EditorPrefs.DeleteKey( PrefAutoSRP );
				GlobalAutoSRP = true;

				EditorPrefs.DeleteKey( PrefDefineSymbol );
				GlobalDefineSymbol = true;
				IOUtils.SetAmplifyDefineSymbolOnBuildTargetGroup( EditorUserBuildSettings.selectedBuildTargetGroup );

				EditorPrefs.DeleteKey( PrefClearLog );
				GlobalClearLog = true;
			}
			EditorGUILayout.EndHorizontal();
			EditorGUIUtility.labelWidth = cache;
		}

		public static void LoadDefaults()
		{
			GlobalStartUp = (ShowOption)EditorPrefs.GetInt( PrefStartUp, 0 );
			GlobalAutoSRP = EditorPrefs.GetBool( PrefAutoSRP, true );
			GlobalDefineSymbol = EditorPrefs.GetBool( PrefDefineSymbol, true );
			GlobalClearLog = EditorPrefs.GetBool( PrefClearLog, true );
		}
	}
}
