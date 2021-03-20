// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "cloth"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Normal("Normal", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,0)
		_SubsurfaceDistortion1("Subsurface Distortion", Range( 0 , 1)) = 0.5
		_SSSpower1("SSS power", Float) = 1
		_normalintensity("normal intensity", Float) = 0
		_SSSscale1("SSS scale", Float) = 1
		_AbsorbedColor1("AbsorbedColor", Color) = (0,0,0,0)
		_SSSmap1("SSSmap", 2D) = "white" {}
		_WindNoiseTexture1("WindNoiseTexture", 2D) = "white" {}
		_WindIntensity1("Wind Intensity", Range( 0 , 2)) = 0
		_WindDirection1("Wind Direction", Range( -180 , 180)) = 0
		_WindSpeed1("WindSpeed", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform sampler2D _WindNoiseTexture1;
		uniform float _WindSpeed1;
		uniform float _WindDirection1;
		uniform float _WindIntensity1;
		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform float _normalintensity;
		uniform float4 _Color;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float4 _AbsorbedColor1;
		uniform float _SubsurfaceDistortion1;
		uniform float _SSSpower1;
		uniform float _SSSscale1;
		uniform sampler2D _SSSmap1;
		uniform float4 _SSSmap1_ST;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult17 = (float2(ase_worldPos.x , ase_worldPos.z));
			float temp_output_12_0 = ( _Time.y * _WindSpeed1 );
			float4 appendResult16 = (float4(( temp_output_12_0 * cos( _WindDirection1 ) ) , ( temp_output_12_0 * sin( _WindDirection1 ) ) , 0.0 , 0.0));
			float WindTexture25 = ( tex2Dlod( _WindNoiseTexture1, float4( ( float4( 0.5,0.5,0.5,0.5 ) * ( float4( appendResult17, 0.0 , 0.0 ) - appendResult16 ) ).xy, 0, 0.0) ).r * tex2Dlod( _WindNoiseTexture1, float4( ( float4( appendResult17, 0.0 , 0.0 ) + appendResult16 ).xy, 0, 0.0) ).r );
			float temp_output_29_0 = saturate( ( WindTexture25 * _WindIntensity1 ) );
			float3 appendResult30 = (float3(temp_output_29_0 , 0.0 , temp_output_29_0));
			v.vertex.xyz += appendResult30;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float3 tex2DNode4 = UnpackScaleNormal( tex2D( _Normal, uv_Normal ), _normalintensity );
			o.Normal = tex2DNode4;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = ( _Color * tex2D( _Albedo, uv_Albedo ) ).rgb;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 objToWorldDir67 = mul( unity_ObjectToWorld, float4( ase_vertex3Pos, 0 ) ).xyz;
			float3 normalizeResult69 = normalize( objToWorldDir67 );
			float dotResult75 = dot( ase_worldViewDir , -( ( tex2DNode4 * ase_worldlightDir ) + ( _SubsurfaceDistortion1 * normalizeResult69 ) ) );
			float LightDirection77 = dotResult75;
			float dotResult82 = dot( pow( LightDirection77 , _SSSpower1 ) , _SSSscale1 );
			float PowerControl0187 = ( saturate( dotResult82 ) * distance( ase_vertex3Pos , float3( 0,0,0 ) ) );
			float4 Color93 = ( ( ase_lightColor - _AbsorbedColor1 ) * PowerControl0187 );
			float2 uv_SSSmap1 = i.uv_texcoord * _SSSmap1_ST.xy + _SSSmap1_ST.zw;
			o.Emission = ( saturate( Color93 ) * saturate( tex2D( _SSSmap1, uv_SSSmap1 ).r ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18800
0;0;1920;1019;3435.585;924.52;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;65;-3198.701,-535.9508;Inherit;False;1263.821;714.1194;light direction ;12;77;75;74;73;72;71;70;69;68;67;66;96;;1,0.6617759,0,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;66;-3188.722,14.22998;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TransformDirectionNode;67;-3009.722,7.22998;Inherit;False;Object;World;False;Fast;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;5;-1085,184.5;Inherit;False;Property;_normalintensity;normal intensity;5;0;Create;True;0;0;0;False;0;False;0;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-3014.209,-126.855;Inherit;False;Property;_SubsurfaceDistortion1;Subsurface Distortion;3;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;69;-2798.722,6.22998;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;71;-3007.581,-294.6478;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;4;-846.2391,129.5;Inherit;True;Property;_Normal;Normal;1;0;Create;True;0;0;0;False;0;False;-1;None;925b24ed15319a34387e1404228e3cc3;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;-2766.585,-273.52;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-2735.209,-125.855;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;72;-2606.209,-210.855;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NegateNode;73;-2544.88,-327.8475;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;74;-2993.148,-493.7509;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;6;-3130.934,812.0504;Inherit;False;1889.616;918;wind;19;25;24;23;22;21;20;19;18;17;16;15;14;13;12;11;10;9;8;7;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DotProductOpNode;75;-2410.88,-392.8475;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;76;-3009.312,-991.2886;Inherit;False;1092.643;452.7633;power control ;9;87;86;84;83;82;81;80;79;78;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;8;-3130.934,1434.05;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-3051.934,1614.05;Inherit;False;Property;_WindDirection1;Wind Direction;11;0;Create;True;0;0;0;False;0;False;0;0;-180;180;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-3088.398,1332.058;Inherit;False;Property;_WindSpeed1;WindSpeed;12;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;77;-2142.88,-386.8475;Inherit;False;LightDirection;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-2951.312,-821.2885;Inherit;False;Property;_SSSpower1;SSS power;4;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;78;-2959.312,-941.2886;Inherit;False;77;LightDirection;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;10;-2839.934,1524.05;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;11;-2773.934,1619.05;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-2916.398,1395.058;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-2644.934,1326.05;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;13;-2867.934,1157.05;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;80;-2723.568,-807.625;Inherit;False;Property;_SSSscale1;SSS scale;6;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-2630.934,1485.05;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;81;-2741.312,-926.2886;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;82;-2557.312,-912.2886;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;17;-2637.934,1139.05;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;16;-2459.934,1373.05;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DistanceOpNode;83;-2501.318,-793.3289;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;84;-2421.568,-912.6252;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;18;-2360.934,1012.05;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-2324.934,1263.05;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-2230.934,908.0504;Inherit;False;2;2;0;FLOAT4;0.5,0.5,0.5,0.5;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;85;-3011.741,-1448.653;Inherit;False;864;459;Colors;6;93;92;91;90;89;88;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;19;-2608.934,862.0504;Inherit;True;Property;_WindNoiseTexture1;WindNoiseTexture;9;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;-2242.854,-915.3552;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;22;-2058.934,867.0504;Inherit;True;Property;_TextureSample1;Texture Sample 0;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;87;-2097.568,-724.6252;Inherit;False;PowerControl01;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;88;-2961.741,-1291.654;Inherit;False;Property;_AbsorbedColor1;AbsorbedColor;7;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LightColorNode;89;-2908.741,-1398.653;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;23;-2054.88,1127.35;Inherit;True;Property;_WindNoise1;WindNoise;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-1636.429,1036.935;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;90;-2715.741,-1279.653;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;91;-2793.741,-1105.653;Inherit;False;87;PowerControl01;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-2552.741,-1174.653;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-1465.319,1035.724;Inherit;False;WindTexture;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;93;-2241.741,-1182.653;Inherit;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-928.7291,755.0168;Inherit;False;Property;_WindIntensity1;Wind Intensity;10;0;Create;True;0;0;0;False;0;False;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;-960.7291,640.0168;Inherit;False;25;WindTexture;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-716.829,629.5171;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;64;-1375.399,-712.283;Inherit;True;Property;_SSSmap1;SSSmap;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;60;-1302.345,-820.1826;Inherit;False;93;Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;61;-951.5551,-714.0525;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-816,-222.5;Inherit;False;Property;_Color;Color;2;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-844,-52.5;Inherit;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;29;-405.608,564.917;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;62;-992.5552,-785.0524;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-761.5922,-787.6367;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-464,-49.5;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;30;-142.608,454.917;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;cloth;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;67;0;66;0
WireConnection;69;0;67;0
WireConnection;4;5;5;0
WireConnection;96;0;4;0
WireConnection;96;1;71;0
WireConnection;70;0;68;0
WireConnection;70;1;69;0
WireConnection;72;0;96;0
WireConnection;72;1;70;0
WireConnection;73;0;72;0
WireConnection;75;0;74;0
WireConnection;75;1;73;0
WireConnection;77;0;75;0
WireConnection;10;0;9;0
WireConnection;11;0;9;0
WireConnection;12;0;8;0
WireConnection;12;1;7;0
WireConnection;14;0;12;0
WireConnection;14;1;11;0
WireConnection;15;0;12;0
WireConnection;15;1;10;0
WireConnection;81;0;78;0
WireConnection;81;1;79;0
WireConnection;82;0;81;0
WireConnection;82;1;80;0
WireConnection;17;0;13;1
WireConnection;17;1;13;3
WireConnection;16;0;15;0
WireConnection;16;1;14;0
WireConnection;83;0;66;0
WireConnection;84;0;82;0
WireConnection;18;0;17;0
WireConnection;18;1;16;0
WireConnection;20;0;17;0
WireConnection;20;1;16;0
WireConnection;21;1;18;0
WireConnection;86;0;84;0
WireConnection;86;1;83;0
WireConnection;22;0;19;0
WireConnection;22;1;21;0
WireConnection;87;0;86;0
WireConnection;23;0;19;0
WireConnection;23;1;20;0
WireConnection;24;0;22;1
WireConnection;24;1;23;1
WireConnection;90;0;89;0
WireConnection;90;1;88;0
WireConnection;92;0;90;0
WireConnection;92;1;91;0
WireConnection;25;0;24;0
WireConnection;93;0;92;0
WireConnection;28;0;26;0
WireConnection;28;1;27;0
WireConnection;61;0;64;1
WireConnection;29;0;28;0
WireConnection;62;0;60;0
WireConnection;63;0;62;0
WireConnection;63;1;61;0
WireConnection;3;0;2;0
WireConnection;3;1;1;0
WireConnection;30;0;29;0
WireConnection;30;2;29;0
WireConnection;0;0;3;0
WireConnection;0;1;4;0
WireConnection;0;2;63;0
WireConnection;0;11;30;0
ASEEND*/
//CHKSM=4A73FEC057A0025177B9E1C1168B1B866A339EA9