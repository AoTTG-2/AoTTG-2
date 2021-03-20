using UnityEngine;
using System;
using System.Collections.Generic;

namespace AmplifyShaderEditor
{
	[Serializable]
	public class TemplateOptionsDefinesContainer
	{
		[SerializeField]
		private List<PropertyDataCollector> m_definesList = new List<PropertyDataCollector>();

		[NonSerialized]
		private Dictionary<string, PropertyDataCollector> m_definesDict = new Dictionary<string, PropertyDataCollector>();

		void Refresh()
		{
			if( m_definesDict.Count != m_definesList.Count )
			{
				m_definesDict.Clear();
				for( int i = 0; i < m_definesList.Count; i++ )
				{
					m_definesDict.Add( m_definesList[ i ].PropertyName, m_definesList[ i ] );
				}
			}
		}

		public void RemoveTemporaries()
		{
			List<PropertyDataCollector> temporaries = m_definesList.FindAll( ( x ) => ( x.NodeId == 1 ) );
			for( int i = 0; i < temporaries.Count; i++ )
			{
				m_definesList.Remove( temporaries[ i ] );
				m_definesDict.Remove( temporaries[ i ].PropertyName );
			}
		}

		public void AddDefine( string define , bool temporary )
		{
			Refresh();
			if( !m_definesDict.ContainsKey( define ) )
			{
				int nodeId = temporary ? 1 : 0;
				PropertyDataCollector data = new PropertyDataCollector( nodeId, define );
				m_definesDict.Add( define, data );
				m_definesList.Add( data );
			}
		}

		public void RemoveDefine( string define )
		{
			Refresh();
			if( m_definesDict.ContainsKey( define ) )
			{
				m_definesList.Remove( m_definesDict[define] );
				m_definesDict.Remove( define );
			}
		}

		public void Destroy()
		{
			m_definesDict.Clear();
			m_definesDict = null;
			m_definesList.Clear();
			m_definesList = null;
		}
		public List<PropertyDataCollector> DefinesList { get { return m_definesList; } }
	}
}
