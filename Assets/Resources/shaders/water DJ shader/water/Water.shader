// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Water"
{
	Properties
	{
		[Normal]_bigwaves("big waves", 2D) = "bump" {}
		[Normal]_smallwaves("small waves", 2D) = "bump" {}
		_foamtexture("foam texture", 2D) = "white" {}
		_Smallwavetilling("Small wave tilling", Float) = 1
		_bigwavetilling("big wave tilling", Float) = 1
		_foamtilling("foam tilling", Float) = 1
		_bigwavesspeed("big waves speed", Float) = 0
		_smallwavesSpeed("small waves Speed", Float) = 0
		_foamspeed("foam speed", Float) = 0
		_smallwavesintensity("small waves intensity", Range( 0.01 , 5)) = 1
		_bigwavesintensity("big waves intensity", Range( 0.01 , 5)) = 1
		_smallwavesflow("small waves flow", Range( 0 , 1)) = 1
		_bigwavesflow("big waves flow", Range( 0 , 1)) = 1
		_smallwavesDirection("small waves Direction", Range( -180 , 180)) = 0
		_bigwavesdirection("big waves direction", Range( -180 , 180)) = 0
		_foamdirection("foam direction", Range( -180 , 180)) = 0
		_absorbedcolor("absorbed color", Color) = (0,0,0,0)
		_waterabsorbtion("water absorbtion", Float) = 100
		_edgeblending("edge blending", Float) = 0
		_distortionintensity("distortion intensity", Float) = 1
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_foamintensity("foam intensity", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		struct Input
		{
			float3 worldPos;
			float4 screenPos;
		};

		uniform sampler2D _bigwaves;
		uniform float _bigwavetilling;
		uniform float _bigwavesspeed;
		uniform float _bigwavesdirection;
		uniform float _bigwavesflow;
		uniform float _bigwavesintensity;
		uniform sampler2D _smallwaves;
		uniform float _Smallwavetilling;
		uniform float _smallwavesSpeed;
		uniform float _smallwavesDirection;
		uniform float _smallwavesflow;
		uniform float _smallwavesintensity;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _distortionintensity;
		uniform float4 _absorbedcolor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _waterabsorbtion;
		uniform sampler2D _foamtexture;
		uniform float _foamspeed;
		uniform float _foamdirection;
		uniform float _foamtilling;
		uniform float _foamintensity;
		uniform float _Smoothness;
		uniform float _edgeblending;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float temp_output_40_0 = ( _bigwavetilling / 100.0 );
			float3 ase_worldPos = i.worldPos;
			float2 appendResult4 = (float2(ase_worldPos.x , ase_worldPos.z));
			float temp_output_50_0 = ( _Time.y * _bigwavesspeed );
			float4 appendResult49 = (float4(( temp_output_50_0 * cos( _bigwavesdirection ) ) , ( temp_output_50_0 * sin( _bigwavesdirection ) ) , 0.0 , 0.0));
			float temp_output_41_0 = ( _Smallwavetilling / 100.0 );
			float temp_output_14_0 = ( _Time.y * _smallwavesSpeed );
			float4 appendResult19 = (float4(( temp_output_14_0 * cos( _smallwavesDirection ) ) , ( temp_output_14_0 * sin( _smallwavesDirection ) ) , 0.0 , 0.0));
			float3 lerpResult102 = lerp( ( ( UnpackNormal( tex2D( _bigwaves, ( temp_output_40_0 * ( float4( appendResult4, 0.0 , 0.0 ) - ( appendResult49 * _bigwavesflow ) ) ).xy ) ) * UnpackNormal( tex2D( _bigwaves, ( temp_output_40_0 * ( float4( appendResult4, 0.0 , 0.0 ) + appendResult49 ) ).xy ) ) ) * _bigwavesintensity ) , ( ( UnpackNormal( tex2D( _smallwaves, ( temp_output_41_0 * ( float4( appendResult4, 0.0 , 0.0 ) - ( appendResult19 * _smallwavesflow ) ) ).xy ) ) * UnpackNormal( tex2D( _smallwaves, ( temp_output_41_0 * ( float4( appendResult4, 0.0 , 0.0 ) + appendResult19 ) ).xy ) ) ) * _smallwavesintensity ) , 0.5);
			float3 Normals56 = lerpResult102;
			float3 temp_output_57_0 = Normals56;
			o.Normal = temp_output_57_0;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float2 appendResult10 = (float2(ase_grabScreenPosNorm.r , ase_grabScreenPosNorm.g));
			float4 screenColor1 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( float3( appendResult10 ,  0.0 ) - ( _distortionintensity * ( Normals56 * 0.1 ) ) ).xy);
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth78 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth78 = abs( ( screenDepth78 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _waterabsorbtion ) );
			float4 lerpResult97 = lerp( screenColor1 , ( ase_lightColor - _absorbedcolor ) , saturate( distanceDepth78 ));
			float temp_output_122_0 = ( _Time.y * _foamspeed );
			float4 appendResult128 = (float4(( temp_output_122_0 * cos( _foamdirection ) ) , ( temp_output_122_0 * sin( _foamdirection ) ) , 0.0 , 0.0));
			float temp_output_150_0 = ( _foamtilling / 100.0 );
			float3 temp_output_153_0 = ( Normals56 * 0.1 );
			float Foam120 = ( tex2D( _foamtexture, ( ( ( appendResult128 - float4( appendResult4, 0.0 , 0.0 ) ) * temp_output_150_0 ) + float4( temp_output_153_0 , 0.0 ) ).xy ).a * tex2D( _foamtexture, ( float4( temp_output_153_0 , 0.0 ) + ( temp_output_150_0 * ( appendResult128 + float4( appendResult4, 0.0 , 0.0 ) ) ) ).xy ).a );
			float temp_output_144_0 = ( Foam120 * ( 1.0 - saturate( ( distanceDepth78 * _foamintensity ) ) ) );
			float ifLocalVar164 = 0;
			if( temp_output_144_0 < 1.0 )
				ifLocalVar164 = temp_output_144_0;
			o.Albedo = ( lerpResult97 + ifLocalVar164 ).rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = ( distanceDepth78 * _edgeblending );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD1;
				float4 screenPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18800
-1680;21;1680;989;4822.467;-124.8506;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;11;-4217.357,995.4919;Inherit;False;Property;_smallwavesDirection;small waves Direction;13;0;Create;True;0;0;0;False;0;False;0;-30;-180;180;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-4931.355,-246.1313;Inherit;False;Property;_bigwavesdirection;big waves direction;14;0;Create;True;0;0;0;False;0;False;0;-30;-180;180;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-4898.785,-707.687;Inherit;False;Property;_bigwavesspeed;big waves speed;6;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;13;-5203.502,-633.7458;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-4358.073,787.3999;Inherit;False;Property;_smallwavesSpeed;small waves Speed;7;0;Create;True;0;0;0;False;0;False;0;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;15;-3939.357,1000.492;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-4640.785,-605.687;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;16;-4005.357,905.4919;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;45;-4564.321,-476.6949;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;46;-4498.321,-381.6949;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-4081.821,776.5;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-3796.357,866.4919;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-3810.357,707.4919;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-4369.321,-674.695;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-4355.32,-515.6949;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;49;-4172.321,-721.695;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-4141.143,-839.9667;Inherit;False;Property;_bigwavesflow;big waves flow;12;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-4156.625,397.5804;Inherit;False;Property;_smallwavesflow;small waves flow;11;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;19;-3613.357,660.4919;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldPosInputsNode;3;-5192.518,-529.7726;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-3890.566,-636.1664;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-4043.303,203.7413;Inherit;False;Property;_Smallwavetilling;Small wave tilling;3;0;Create;True;0;0;0;False;0;False;1;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-3572.625,278.5804;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-4361.402,-78.65794;Inherit;False;Property;_bigwavetilling;big wave tilling;4;0;Create;True;0;0;0;False;0;False;1;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;4;-4969.41,-516.5512;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;42;-3779.253,-420.5648;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;40;-4193.402,-78.65794;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-3397.625,411.5804;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;26;-3417.625,136.5804;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;41;-3813.918,174.3782;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;43;-3792.253,-105.5647;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-3304.303,265.7413;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-3602.401,-184.6579;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TexturePropertyNode;31;-4186.402,-354.658;Inherit;True;Property;_bigwaves;big waves;0;1;[Normal];Create;True;0;0;0;False;0;False;None;edcc3a8c156f0a1458b4590c1960448d;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;22;-4399.625,40.58044;Inherit;True;Property;_smallwaves;small waves;1;1;[Normal];Create;True;0;0;0;False;0;False;None;b5a75b42868baad43aae83887c42341a;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-3327.625,38.58044;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-3609.401,-393.658;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;33;-3458.401,-292.658;Inherit;True;Property;_TextureSample2;Texture Sample 2;1;0;Create;True;0;0;0;False;0;False;-1;None;b5a75b42868baad43aae83887c42341a;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;23;-3175.32,-5.032288;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;b5a75b42868baad43aae83887c42341a;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;32;-3445.401,-486.658;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;None;b5a75b42868baad43aae83887c42341a;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-3174.915,236.0902;Inherit;True;Property;_SmallWaves;Small Waves;1;0;Create;True;0;0;0;False;0;False;-1;None;b5a75b42868baad43aae83887c42341a;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-4090.303,294.7413;Inherit;False;Property;_smallwavesintensity;small waves intensity;9;0;Create;True;0;0;0;False;0;False;1;5;0.01;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-3475.402,-69.6579;Inherit;False;Property;_bigwavesintensity;big waves intensity;10;0;Create;True;0;0;0;False;0;False;1;2.4;0.01;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-3157.311,-188.8854;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;131;-3567.154,-1782.158;Inherit;False;Property;_foamspeed;foam speed;8;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;125;-3698.041,-1602.536;Inherit;False;Property;_foamdirection;foam direction;15;0;Create;True;0;0;0;False;0;False;0;-122;-180;180;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-2815.625,43.5804;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-2897.356,-176.0334;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CosOpNode;123;-3437.833,-1699.779;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;122;-3514.296,-1828.771;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;103;-2519.762,179.7617;Inherit;False;Constant;_Float1;Float 1;17;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-2627.618,40.0498;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SinOpNode;124;-3371.833,-1604.779;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;126;-3228.832,-1738.779;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;-3242.833,-1897.78;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;102;-2405.762,-71.23831;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;128;-3045.833,-1944.78;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;136;-3048.799,-1740.588;Inherit;False;Property;_foamtilling;foam tilling;5;0;Create;True;0;0;0;False;0;False;1;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;56;-2155.663,-67.10883;Inherit;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;133;-2794.428,-1883.747;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;132;-2781.667,-1656.599;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;150;-2829.917,-1768.669;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-2636.449,-1726.487;Inherit;False;Constant;_Float0;Float 0;22;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;142;-2639.039,-1794.06;Inherit;False;56;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;-2596.799,-1889.588;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;135;-2579.799,-1655.588;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;153;-2419.449,-1805.487;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-1381.695,-374.474;Inherit;False;Property;_waterabsorbtion;water absorbtion;17;0;Create;True;0;0;0;False;0;False;100;5.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;155;-2228.449,-1988.487;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TexturePropertyNode;138;-2898.793,-2141.353;Inherit;True;Property;_foamtexture;foam texture;2;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleAddOpNode;156;-2227.449,-1743.487;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;119;-2011.513,-1833.47;Inherit;True;Property;_Foam;Foam;17;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;57;-1816.16,156.9012;Inherit;False;56;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;105;-1659.762,17.76172;Inherit;False;Constant;_Float2;Float 2;17;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;145;-822.5761,-664.2393;Inherit;False;Property;_foamintensity;foam intensity;21;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;78;-1146.185,-355.5894;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;139;-2061.585,-2118.937;Inherit;True;Property;_TextureSample3;Texture Sample 3;17;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;146;-722.1763,-568.8391;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-1505.318,-82.78339;Inherit;False;Property;_distortionintensity;distortion intensity;19;0;Create;True;0;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;8;-1711.599,-292.1399;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;-1474.762,26.76172;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;-1637.219,-1904.49;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;10;-1438.599,-266.1399;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;120;-1440.312,-1898.57;Inherit;False;Foam;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;148;-572.3102,-558.6952;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-1290.318,-72.78339;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;143;-612.0115,-642.1334;Inherit;False;120;Foam;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;149;-425.21,-453.0952;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;87;-1198.102,-702.3406;Inherit;False;Property;_absorbedcolor;absorbed color;16;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0.7933914,0.7877358,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;9;-1168.599,-169.1399;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LightColorNode;90;-1153.695,-494.474;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleSubtractOpNode;89;-868.6949,-480.474;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;98;-707.9138,-227.3406;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;144;-230.2116,-538.1334;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;1;-684,-113.5;Inherit;False;Global;_GrabScreen0;Grab Screen 0;0;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;165;-354.1777,-262.9944;Inherit;False;Constant;_Float3;Float 3;22;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;164;-136.1777,-329.9944;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-1146.319,-262.6732;Inherit;False;Property;_edgeblending;edge blending;18;0;Create;True;0;0;0;False;0;False;0;30;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;97;-264.6948,-89.474;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-534.6013,128.575;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-365.7197,215.7358;Inherit;False;Property;_Smoothness;Smoothness;20;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;163;34.82227,-47.99445;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;167,-13;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;2;False;-1;1;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;11;0
WireConnection;50;0;13;0
WireConnection;50;1;51;0
WireConnection;16;0;11;0
WireConnection;45;0;44;0
WireConnection;46;0;44;0
WireConnection;14;0;13;0
WireConnection;14;1;12;0
WireConnection;18;0;14;0
WireConnection;18;1;16;0
WireConnection;17;0;14;0
WireConnection;17;1;15;0
WireConnection;48;0;50;0
WireConnection;48;1;46;0
WireConnection;47;0;50;0
WireConnection;47;1;45;0
WireConnection;49;0;47;0
WireConnection;49;1;48;0
WireConnection;19;0;18;0
WireConnection;19;1;17;0
WireConnection;53;0;49;0
WireConnection;53;1;52;0
WireConnection;27;0;19;0
WireConnection;27;1;28;0
WireConnection;4;0;3;1
WireConnection;4;1;3;3
WireConnection;42;0;4;0
WireConnection;42;1;53;0
WireConnection;40;0;39;0
WireConnection;20;0;4;0
WireConnection;20;1;19;0
WireConnection;26;0;4;0
WireConnection;26;1;27;0
WireConnection;41;0;7;0
WireConnection;43;0;4;0
WireConnection;43;1;49;0
WireConnection;6;0;41;0
WireConnection;6;1;20;0
WireConnection;38;0;40;0
WireConnection;38;1;43;0
WireConnection;24;0;41;0
WireConnection;24;1;26;0
WireConnection;37;0;40;0
WireConnection;37;1;42;0
WireConnection;33;0;31;0
WireConnection;33;1;38;0
WireConnection;23;0;22;0
WireConnection;23;1;24;0
WireConnection;32;0;31;0
WireConnection;32;1;37;0
WireConnection;2;0;22;0
WireConnection;2;1;6;0
WireConnection;36;0;32;0
WireConnection;36;1;33;0
WireConnection;30;0;23;0
WireConnection;30;1;2;0
WireConnection;64;0;36;0
WireConnection;64;1;34;0
WireConnection;123;0;125;0
WireConnection;122;0;13;0
WireConnection;122;1;131;0
WireConnection;63;0;30;0
WireConnection;63;1;5;0
WireConnection;124;0;125;0
WireConnection;126;0;122;0
WireConnection;126;1;123;0
WireConnection;127;0;122;0
WireConnection;127;1;124;0
WireConnection;102;0;64;0
WireConnection;102;1;63;0
WireConnection;102;2;103;0
WireConnection;128;0;126;0
WireConnection;128;1;127;0
WireConnection;56;0;102;0
WireConnection;133;0;128;0
WireConnection;133;1;4;0
WireConnection;132;0;128;0
WireConnection;132;1;4;0
WireConnection;150;0;136;0
WireConnection;134;0;133;0
WireConnection;134;1;150;0
WireConnection;135;0;150;0
WireConnection;135;1;132;0
WireConnection;153;0;142;0
WireConnection;153;1;154;0
WireConnection;155;0;134;0
WireConnection;155;1;153;0
WireConnection;156;0;153;0
WireConnection;156;1;135;0
WireConnection;119;0;138;0
WireConnection;119;1;156;0
WireConnection;78;0;96;0
WireConnection;139;0;138;0
WireConnection;139;1;155;0
WireConnection;146;0;78;0
WireConnection;146;1;145;0
WireConnection;104;0;57;0
WireConnection;104;1;105;0
WireConnection;158;0;139;4
WireConnection;158;1;119;4
WireConnection;10;0;8;1
WireConnection;10;1;8;2
WireConnection;120;0;158;0
WireConnection;148;0;146;0
WireConnection;81;0;82;0
WireConnection;81;1;104;0
WireConnection;149;0;148;0
WireConnection;9;0;10;0
WireConnection;9;1;81;0
WireConnection;89;0;90;0
WireConnection;89;1;87;0
WireConnection;98;0;78;0
WireConnection;144;0;143;0
WireConnection;144;1;149;0
WireConnection;1;0;9;0
WireConnection;164;0;144;0
WireConnection;164;1;165;0
WireConnection;164;4;144;0
WireConnection;97;0;1;0
WireConnection;97;1;89;0
WireConnection;97;2;98;0
WireConnection;67;0;78;0
WireConnection;67;1;68;0
WireConnection;163;0;97;0
WireConnection;163;1;164;0
WireConnection;0;0;163;0
WireConnection;0;1;57;0
WireConnection;0;4;62;0
WireConnection;0;9;67;0
ASEEND*/
//CHKSM=408D7189C062463381007ACB022EA42FC165D357