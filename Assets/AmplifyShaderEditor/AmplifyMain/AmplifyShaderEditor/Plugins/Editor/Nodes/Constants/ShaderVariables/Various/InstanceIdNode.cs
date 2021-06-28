// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using System;

namespace AmplifyShaderEditor
{
	[Serializable]
	[NodeAttributes( "Instance ID", "Vertex Data", "Indicates the per-instance identifier" )]
	public class InstanceIdNode : ParentNode
	{
		private readonly string[] InstancingVariableAttrib =
		{   "uint currInstanceId = 0;",
			"#ifdef UNITY_INSTANCING_ENABLED",
			"currInstanceId = unity_InstanceID;",
			"#endif"};
		private const string InstancingInnerVariable = "currInstanceId";

		protected override void CommonInit( int uniqueId )
		{
			base.CommonInit( uniqueId );
			AddOutputPort( WirePortDataType.INT, "Out" );
			m_previewShaderGUID = "03febce56a8cf354b90e7d5180c1dbd7";
		}

		public override string GenerateShaderForOutput( int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalvar )
		{
			if( dataCollector.IsTemplate )
			{
				dataCollector.TemplateDataCollectorInstance.SetupInstancing();
			}

			if( !dataCollector.HasLocalVariable( InstancingVariableAttrib[ 0 ] ) )
			{
				dataCollector.AddLocalVariable( UniqueId, InstancingVariableAttrib[ 0 ] ,true );
				dataCollector.AddLocalVariable( UniqueId, InstancingVariableAttrib[ 1 ] ,true );
				dataCollector.AddLocalVariable( UniqueId, InstancingVariableAttrib[ 2 ] ,true );
				dataCollector.AddLocalVariable( UniqueId, InstancingVariableAttrib[ 3 ] ,true );
			}
			return InstancingInnerVariable;
		}
	}
}
