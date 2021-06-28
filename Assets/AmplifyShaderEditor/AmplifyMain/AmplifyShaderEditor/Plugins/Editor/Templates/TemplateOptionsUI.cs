// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AmplifyShaderEditor
{
	// UI STRUCTURES
	[Serializable]
	public class TemplateOptionUIItem
	{
		public delegate void OnActionPerformed( bool isRefreshing, bool invertAction, TemplateOptionUIItem uiItem, params TemplateActionItem[] validActions );
		public event OnActionPerformed OnActionPerformedEvt;

		[SerializeField]
		private bool m_isVisible = true;

		[SerializeField]
		private bool m_wasVisible = true;

		[SerializeField]
		private int m_currentOption = 0;

		[SerializeField]
		private TemplateOptionsItem m_options;

		[SerializeField]
		private bool m_checkOnExecute = false;

		[SerializeField]
		private bool m_invertActionOnDeselection = false;

		public TemplateOptionUIItem( TemplateOptionsItem options )
		{
			m_options = options;
			m_currentOption = m_options.DefaultOptionIndex;
			m_invertActionOnDeselection = options.Setup == AseOptionItemSetup.InvertActionOnDeselection;
		}

		public void CopyValuesFrom( TemplateOptionUIItem origin )
		{
			m_isVisible = origin.IsVisible;
			m_wasVisible = origin.WasVisible;
			m_currentOption = origin.CurrentOption;
			m_checkOnExecute = origin.CheckOnExecute;
			m_invertActionOnDeselection = origin.InvertActionOnDeselection;
		}

		public void Draw( UndoParentNode owner )
		{
			if( m_isVisible )
			{
				int lastOption = m_currentOption;
				EditorGUI.BeginChangeCheck();
				switch( m_options.UIWidget )
				{
					case AseOptionsUIWidget.Dropdown:
					{
						m_currentOption = owner.EditorGUILayoutPopup( m_options.Name, m_currentOption, m_options.DisplayOptions );
					}
					break;
					case AseOptionsUIWidget.Toggle:
					{
						m_currentOption = owner.EditorGUILayoutToggle( m_options.Name, m_currentOption == 1 ) ? 1 : 0;
					}
					break;
				}
				if( EditorGUI.EndChangeCheck() )
				{
					if( OnActionPerformedEvt != null )
					{
						if( m_invertActionOnDeselection )
							OnActionPerformedEvt( false, lastOption != m_options.DisableIdx, this, m_options.ActionsPerOption[ lastOption ] );

						OnActionPerformedEvt( false, false, this, m_options.ActionsPerOption[ m_currentOption ] );
					}
				}
			}
		}

		public void CheckEnDisable()
		{
			//string deb = string.Empty;// "-- Checked --" + m_options.Name+" "+ m_isVisible + " "+ m_wasVisible;
			if( m_isVisible )
			{
				if( !m_wasVisible )
				{
					//deb = "-- Enable --" + m_options.Name;
					//Debug.Log( deb );
					if( OnActionPerformedEvt != null )
					{
						if( m_invertActionOnDeselection )
						{
							for( int i = 0; i < m_options.Count; i++ )
							{
								if( i != m_currentOption && i != m_options.DisableIdx )
								{
									OnActionPerformedEvt( false, true, this, m_options.ActionsPerOption[ i ] );
								}
							}
						}

						OnActionPerformedEvt( false, false, this, m_options.ActionsPerOption[ m_currentOption ] );
						//if( !m_isVisible )
							//OnActionPerformedEvt( isRefreshing, false, this, m_options.ActionsPerOption[ m_options.DisableIdx ] );
					}
				}

				m_wasVisible = true;
			}
			else if( m_wasVisible )
			{
				//deb = "-- Disable --" + m_options.Name;
				//Debug.Log( deb );
				m_wasVisible = false;

				if( OnActionPerformedEvt != null )
				{
					OnActionPerformedEvt( false, false, this, m_options.ActionsPerOption[ m_options.DisableIdx ] );
				}
			}
		}

		public void FillDataCollector( ref MasterNodeDataCollector dataCollector )
		{
			if( m_isVisible && m_checkOnExecute )
			{
				for( int i = 0; i < m_options.ActionsPerOption[ m_currentOption ].Length; i++ )
				{
					switch( m_options.ActionsPerOption[ m_currentOption ][ i ].ActionType )
					{
						case AseOptionsActionType.SetDefine:
						{
							dataCollector.AddToDefines( -1, m_options.ActionsPerOption[ m_currentOption ][ i ].ActionData );
						}
						break;
						case AseOptionsActionType.SetUndefine:
						{
							dataCollector.AddToDefines( -1, m_options.ActionsPerOption[ m_currentOption ][ i ].ActionData, false );
						}
						break;
					}
				}
			}
		}

		public void Refresh()
		{
			if( OnActionPerformedEvt != null )
			{
				if( m_invertActionOnDeselection )
				{
					for( int i = 0; i < m_options.Count; i++ )
					{
						if( i != m_currentOption && i != m_options.DisableIdx )
						{
							OnActionPerformedEvt( true, true, this, m_options.ActionsPerOption[ i ] );
						}
					}
				}

				OnActionPerformedEvt( true, false, this, m_options.ActionsPerOption[ m_currentOption ] );
			}
		}

		public TemplateOptionsItem Options { get { return m_options; } }

		public void Destroy()
		{
			OnActionPerformedEvt = null;
		}

		public bool IsVisible
		{
			get { return m_isVisible; }
			set { m_isVisible = value; }
		}

		public bool WasVisible
		{
			get { return m_wasVisible; }
			set { m_wasVisible = value; }
		}

		public bool CheckOnExecute
		{
			get { return m_checkOnExecute; }
			set { m_checkOnExecute = value; }
		}

		public int CurrentOption
		{
			get { return m_currentOption; }
			set
			{
				m_currentOption = Mathf.Clamp( value, 0, m_options.Options.Length - 1 );
				// why refreshing here?
				//Refresh();
			}
		}

		public int CurrentOptionIdx
		{
			set
			{
				m_currentOption = Mathf.Clamp( value, 0, m_options.Options.Length - 1 );
			}
		}
		public bool EmptyEvent { get { return OnActionPerformedEvt == null; } }
		public TemplateActionItemGrid.TemplateActionItemRow CurrentOptionActions
		{
			get
			{
				return m_options.ActionsPerOption.Rows[m_currentOption];
			}
		}
		public bool InvertActionOnDeselection { get { return m_invertActionOnDeselection; } }
	}
}
