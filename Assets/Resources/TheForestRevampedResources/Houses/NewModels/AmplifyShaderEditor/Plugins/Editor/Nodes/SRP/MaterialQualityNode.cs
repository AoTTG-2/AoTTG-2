// Amplify Shader Editor - Visual Shader vEditing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using UnityEngine;
using UnityEditor;
using System;

namespace AmplifyShaderEditor
{
	[NodeAttributes( "Material Quality", "Logical Operators", "Choose between separate branches according to currently selected Quality (SRP only) ", Available = true )]
	public class MaterialQualityNode : ParentNode
	{
		private const string SRPError = "Node intended to be used only on SRP templates as it makes use of keywords defined over that environment.";

		private const string MaxKeyword = "MATERIAL_QUALITY_HIGH";
		private const string MedKeyword = "MATERIAL_QUALITY_MEDIUM";
		private const string MinKeyword = "MATERIAL_QUALITY_LOW";
		private const string MaterialPragmas = "#pragma shader_feature " + MaxKeyword + " " + MedKeyword + " " + MinKeyword;
		private readonly string[] MaterialCode =
		{
			"#if defined("+MaxKeyword+")",
			"#elif defined("+MedKeyword+")",
			"#else",
			"#endif"
		};
		protected override void CommonInit( int uniqueId )
		{
			base.CommonInit( uniqueId );
			AddInputPort( WirePortDataType.FLOAT, false, "High" );
			AddInputPort( WirePortDataType.FLOAT, false, "Medium" );
			AddInputPort( WirePortDataType.FLOAT, false, "Low" );
			AddOutputPort( WirePortDataType.FLOAT, Constants.EmptyPortValue );
			m_errorMessageTypeIsError = NodeMessageType.Error;
			m_errorMessageTooltip = SRPError;
		}

		public override void OnNodeLogicUpdate( DrawInfo drawInfo )
		{
			base.OnNodeLogicUpdate( drawInfo );
			if( !ContainerGraph.IsSRP )
			{
				if( !m_showErrorMessage )
				{
					m_showErrorMessage = true;
				}
			}
			else
			{
				if( m_showErrorMessage )
				{
					m_showErrorMessage = false;
				}
			}
		}

		public override string GenerateShaderForOutput( int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalvar )
		{
			if( m_outputPorts[ 0 ].IsLocalValue( dataCollector.PortCategory ) )
				return m_outputPorts[ 0 ].LocalValue( dataCollector.PortCategory );

			dataCollector.AddToDirectives( MaterialPragmas );
			string maxQualityValue = m_inputPorts[ 0 ].GeneratePortInstructions( ref dataCollector );
			string medQualityValue = m_inputPorts[ 1 ].GeneratePortInstructions( ref dataCollector );
			string minQualityValue = m_inputPorts[ 2 ].GeneratePortInstructions( ref dataCollector );
			string localVarName = "currQuality" + OutputId;
			dataCollector.AddLocalVariable( UniqueId, CurrentPrecisionType, m_outputPorts[ 0 ].DataType, localVarName, "0" );

			//High
			dataCollector.AddLocalVariable( UniqueId, MaterialCode[ 0 ], true );
			dataCollector.AddLocalVariable( UniqueId, localVarName, maxQualityValue, false, true );
			
			//Medium
			dataCollector.AddLocalVariable( UniqueId, MaterialCode[ 1 ], true );
			dataCollector.AddLocalVariable( UniqueId, localVarName, medQualityValue, false, true );

			//Low
			dataCollector.AddLocalVariable( UniqueId, MaterialCode[ 2 ], true );
			dataCollector.AddLocalVariable( UniqueId, localVarName, minQualityValue,false,true );
			m_outputPorts[ 0 ].SetLocalValue( localVarName, dataCollector.PortCategory );

			dataCollector.AddLocalVariable( UniqueId, MaterialCode[ 3 ], true );
			return localVarName;
		}
	}
}
