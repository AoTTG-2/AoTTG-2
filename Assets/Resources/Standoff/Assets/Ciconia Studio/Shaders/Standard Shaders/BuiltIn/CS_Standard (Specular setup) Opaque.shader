Shader "Ciconia Studio/CS_Standard/Builtin/Standard (Specular setup)/Opaque"
{
	Properties
	{

		[Space(35)][Header(Surface Options )]
		[Space(15)][Enum(Off,2,On,0)] _Cull("Double Sided", Float) = 2 //"Back"
		[Enum(Off,0,On,1)] _ZWrite("ZWrite", Float) = 1.0 //"On"
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 4 //"LessEqual"

		[Space(35)][Header(Main Properties )][Space(15)]_GlobalXYTilingXYZWOffsetXY("Global --> XY(TilingXY) - ZW(OffsetXY)", Vector) = (1,1,0,0)
		_Color("Color", Color) = (1,1,1,0)
		[Toggle]_InvertABaseColor("Invert Alpha", Float) = 0
		_MainTex("Base Color", 2D) = "white" {}
		_Saturation("Saturation", Float) = 0
		_Brightness("Brightness", Range( 1 , 8)) = 1
		[Space(35)]_BumpMap("Normal Map", 2D) = "bump" {}
		_BumpScale("Normal Intensity", Float) = 0.3
		[Space(35)]_SpecularColor("Specular Color", Color) = (1,1,1,0)
		_SpecGlossMap("Specular Map -->(Smoothness A)", 2D) = "white" {}
		_SpecularIntensity("Specular Intensity", Range( 0 , 2)) = 0.2
		_Glossiness("Smoothness", Range( 0 , 2)) = 0.5
		[Space(10)][KeywordEnum(SpecularAlpha,BaseColorAlpha)] _Source("Source", Float) = 0
		[Space(35)]_ParallaxMap("Height Map", 2D) = "white" {}
		_Parallax("Height Scale", Range( -0.1 , 0.1)) = 0
		[Space(35)]_OcclusionMap("Ambient Occlusion Map", 2D) = "white" {}
		_AoIntensity("Ao Intensity", Range( 0 , 2)) = 1
		[HDR][Space(45)]_EmissionColor("Emission Color", Color) = (0,0,0,0)
		_EmissionMap("Emission Map", 2D) = "white" {}
		_EmissionIntensity("Intensity", Range( 0 , 2)) = 1
		[Space(35)][Header(Mask Properties)][Toggle]_EnableDetailMask("Enable", Float) = 0
		[Space(15)][Toggle(_VISUALIZEMASK_ON)] _VisualizeMask("Visualize Mask", Float) = 0
		[Space(15)][KeywordEnum(UVProjection,TriplanarProjection)] _Projection("Projection", Float) = 0
		[Space(15)][KeywordEnum(ObjectSpace,WorldSpace)] _TriplanarSpaceProjection("Space Projection", Float) = 0
		_TriplanarFalloff("Falloff", Float) = 20
		_TriplanarXYTilingXYZWOffsetXY("Triplanar --> XY(TilingXY) - ZW(OffsetXY)  ", Vector) = (1,1,0,0)
		[Toggle]_InvertMask("Invert Mask", Float) = 0
		_DetailMask("Detail Mask", 2D) = "black" {}
		_IntensityMask("Intensity", Range( 0 , 1)) = 1
		[Space(15)]_ContrastDetailMap("Contrast", Float) = 0
		_SpreadDetailMap("Spread", Float) = 0
		[Space(35)][Header(Detail Properties)][Space(15)][Toggle]_BlendmodeOverlay("Blend mode Overlay", Float) = 0
		[Space(35)]_DetailColor("Color", Color) = (1,1,1,0)
		_DetailAlbedoMap("Base Color", 2D) = "white" {}
		_DetailSaturation("Saturation", Float) = 0
		_DetailBrightness("Brightness", Range( 1 , 8)) = 1
		[Space(35)][Toggle]_BlendMainNormal("Blend Main Normal", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		_DetailNormalMap("Normal Map", 2D) = "bump" {}
		_DetailNormalMapScale("Scale", Float) = 0.3
		[Space(35)]_DetailSpecularColor("Specular Color", Color) = (1,1,1,0)
		_DetailSpecGlossMap("Specular Map -->(Smoothness A)", 2D) = "white" {}
		_DetailSpecularIntensity("Specular Intensity", Range( 0 , 2)) = 0.2
		_DetailGlossiness("Smoothness", Range( 0 , 2)) = 0.5
		[Space(10)][KeywordEnum(SpecularAlpha,AlbedoAlpha)] _DetailSource("Source", Float) = 0
		[Space(15)][Toggle]_UseAoFromMainProperties("Use Ao From Main Properties", Float) = 1
		[Toggle]_UseEmissionFromMainProperties("Use Emission From Main Properties", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull[_Cull]
		ZWrite[_ZWrite]
		ZTest [_ZTest]
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma shader_feature_local _PROJECTION_UVPROJECTION _PROJECTION_TRIPLANARPROJECTION
		#pragma shader_feature_local _TRIPLANARSPACEPROJECTION_OBJECTSPACE _TRIPLANARSPACEPROJECTION_WORLDSPACE
		#pragma shader_feature_local _VISUALIZEMASK_ON
		#pragma shader_feature_local _SOURCE_SPECULARALPHA _SOURCE_BASECOLORALPHA
		#pragma shader_feature_local _DETAILSOURCE_SPECULARALPHA _DETAILSOURCE_ALBEDOALPHA
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 viewDir;
			INTERNAL_DATA
			half ASEVFace : VFACE;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float _BlendMainNormal;
		uniform sampler2D _BumpMap;
		uniform float4 _BumpMap_ST;
		uniform float4 _GlobalXYTilingXYZWOffsetXY;
		uniform sampler2D _ParallaxMap;
		SamplerState sampler_ParallaxMap;
		uniform float4 _ParallaxMap_ST;
		uniform float _Parallax;
		uniform float _BumpScale;
		uniform sampler2D _DetailNormalMap;
		uniform float4 _DetailNormalMap_ST;
		uniform float _DetailNormalMapScale;
		uniform float _EnableDetailMask;
		uniform float _ContrastDetailMap;
		uniform float _InvertMask;
		uniform sampler2D _DetailMask;
		uniform float4 _DetailMask_ST;
		uniform float4 _TriplanarXYTilingXYZWOffsetXY;
		uniform float _SpreadDetailMap;
		uniform float _IntensityMask;
		uniform float _BlendmodeOverlay;
		uniform float _Brightness;
		uniform float4 _Color;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _Saturation;
		uniform float _DetailBrightness;
		uniform float4 _DetailColor;
		uniform sampler2D _DetailAlbedoMap;
		uniform float4 _DetailAlbedoMap_ST;
		uniform float _DetailSaturation;
		uniform float _UseEmissionFromMainProperties;
		uniform float4 _EmissionColor;
		uniform sampler2D _EmissionMap;
		uniform float4 _EmissionMap_ST;
		uniform float _EmissionIntensity;
		uniform float4 _SpecularColor;
		uniform sampler2D _SpecGlossMap;
		uniform float4 _SpecGlossMap_ST;
		uniform float _SpecularIntensity;
		uniform float4 _DetailSpecularColor;
		uniform sampler2D _DetailSpecGlossMap;
		uniform float4 _DetailSpecGlossMap_ST;
		uniform float _DetailSpecularIntensity;
		SamplerState sampler_SpecGlossMap;
		uniform float _Glossiness;
		uniform float _InvertABaseColor;
		SamplerState sampler_MainTex;
		SamplerState sampler_DetailSpecGlossMap;
		uniform float _DetailGlossiness;
		SamplerState sampler_DetailAlbedoMap;
		uniform float _UseAoFromMainProperties;
		uniform sampler2D _OcclusionMap;
		SamplerState sampler_OcclusionMap;
		uniform float4 _OcclusionMap_ST;
		uniform float _AoIntensity;

		UNITY_INSTANCING_BUFFER_START(CiconiaStudioCS_StandardBuiltinStandardSpecularsetupOpaque)
			UNITY_DEFINE_INSTANCED_PROP(float, _TriplanarFalloff)
#define _TriplanarFalloff_arr CiconiaStudioCS_StandardBuiltinStandardSpecularsetupOpaque
		UNITY_INSTANCING_BUFFER_END(CiconiaStudioCS_StandardBuiltinStandardSpecularsetupOpaque)


		inline float4 TriplanarSampling218( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_BumpMap = i.uv_texcoord * _BumpMap_ST.xy + _BumpMap_ST.zw;
			float2 break26_g505 = uv_BumpMap;
			float GlobalTilingX11 = ( _GlobalXYTilingXYZWOffsetXY.x - 1.0 );
			float GlobalTilingY8 = ( _GlobalXYTilingXYZWOffsetXY.y - 1.0 );
			float2 appendResult14_g505 = (float2(( break26_g505.x * GlobalTilingX11 ) , ( break26_g505.y * GlobalTilingY8 )));
			float GlobalOffsetX10 = _GlobalXYTilingXYZWOffsetXY.z;
			float GlobalOffsetY9 = _GlobalXYTilingXYZWOffsetXY.w;
			float2 appendResult13_g505 = (float2(( break26_g505.x + GlobalOffsetX10 ) , ( break26_g505.y + GlobalOffsetY9 )));
			float2 uv_ParallaxMap = i.uv_texcoord * _ParallaxMap_ST.xy + _ParallaxMap_ST.zw;
			float2 break26_g496 = uv_ParallaxMap;
			float2 appendResult14_g496 = (float2(( break26_g496.x * GlobalTilingX11 ) , ( break26_g496.y * GlobalTilingY8 )));
			float2 appendResult13_g496 = (float2(( break26_g496.x + GlobalOffsetX10 ) , ( break26_g496.y + GlobalOffsetY9 )));
			float4 temp_cast_0 = (tex2D( _ParallaxMap, ( appendResult14_g496 + appendResult13_g496 ) ).g).xxxx;
			float2 paralaxOffset36_g495 = ParallaxOffset( temp_cast_0.x , _Parallax , i.viewDir );
			float2 switchResult47_g495 = (((i.ASEVFace>0)?(paralaxOffset36_g495):(0.0)));
			float2 Parallaxe359 = switchResult47_g495;
			float3 temp_output_245_0 = UnpackScaleNormal( tex2D( _BumpMap, ( ( appendResult14_g505 + appendResult13_g505 ) + Parallaxe359 ) ), _BumpScale );
			float2 uv_DetailNormalMap = i.uv_texcoord * _DetailNormalMap_ST.xy + _DetailNormalMap_ST.zw;
			float3 NormalDetail155 = UnpackScaleNormal( tex2D( _DetailNormalMap, uv_DetailNormalMap ), _DetailNormalMapScale );
			float4 temp_cast_2 = (0.0).xxxx;
			float2 uv_DetailMask = i.uv_texcoord * _DetailMask_ST.xy + _DetailMask_ST.zw;
			float4 tex2DNode225 = tex2D( _DetailMask, uv_DetailMask );
			float2 appendResult213 = (float2(_TriplanarXYTilingXYZWOffsetXY.x , _TriplanarXYTilingXYZWOffsetXY.y));
			float _TriplanarFalloff_Instance = UNITY_ACCESS_INSTANCED_PROP(_TriplanarFalloff_arr, _TriplanarFalloff);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			#if defined(_TRIPLANARSPACEPROJECTION_OBJECTSPACE)
				float3 staticSwitch215 = ase_vertex3Pos;
			#elif defined(_TRIPLANARSPACEPROJECTION_WORLDSPACE)
				float3 staticSwitch215 = ase_worldPos;
			#else
				float3 staticSwitch215 = ase_vertex3Pos;
			#endif
			float2 appendResult220 = (float2(_TriplanarXYTilingXYZWOffsetXY.z , _TriplanarXYTilingXYZWOffsetXY.w));
			float4 triplanar218 = TriplanarSampling218( _DetailMask, ( staticSwitch215 + float3( appendResult220 ,  0.0 ) ), ase_vertexNormal, _TriplanarFalloff_Instance, appendResult213, 1.0, 0 );
			#if defined(_PROJECTION_UVPROJECTION)
				float staticSwitch221 = tex2DNode225.r;
			#elif defined(_PROJECTION_TRIPLANARPROJECTION)
				float staticSwitch221 = triplanar218.x;
			#else
				float staticSwitch221 = tex2DNode225.r;
			#endif
			float4 temp_cast_4 = ((( _InvertMask )?( ( 1.0 - staticSwitch221 ) ):( staticSwitch221 ))).xxxx;
			float4 clampResult222 = clamp( ( CalculateContrast(( _ContrastDetailMap + 1.0 ),temp_cast_4) + -_SpreadDetailMap ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float MaskIntensity316 = _IntensityMask;
			float4 Mask158 = (( _EnableDetailMask )?( ( clampResult222 * MaskIntensity316 ) ):( temp_cast_2 ));
			float3 lerpResult137 = lerp( temp_output_245_0 , NormalDetail155 , Mask158.rgb);
			float3 lerpResult205 = lerp( temp_output_245_0 , BlendNormals( temp_output_245_0 , NormalDetail155 ) , Mask158.rgb);
			float3 Normal27 = (( _BlendMainNormal )?( lerpResult205 ):( lerpResult137 ));
			o.Normal = Normal27;
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float2 break26_g502 = uv_MainTex;
			float2 appendResult14_g502 = (float2(( break26_g502.x * GlobalTilingX11 ) , ( break26_g502.y * GlobalTilingY8 )));
			float2 appendResult13_g502 = (float2(( break26_g502.x + GlobalOffsetX10 ) , ( break26_g502.y + GlobalOffsetY9 )));
			float4 tex2DNode7_g501 = tex2D( _MainTex, ( ( appendResult14_g502 + appendResult13_g502 ) + Parallaxe359 ) );
			float clampResult27_g501 = clamp( _Saturation , -1.0 , 100.0 );
			float3 desaturateInitialColor29_g501 = ( _Color * tex2DNode7_g501 ).rgb;
			float desaturateDot29_g501 = dot( desaturateInitialColor29_g501, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar29_g501 = lerp( desaturateInitialColor29_g501, desaturateDot29_g501.xxx, -clampResult27_g501 );
			float4 temp_output_372_0 = CalculateContrast(_Brightness,float4( desaturateVar29_g501 , 0.0 ));
			float2 uv_DetailAlbedoMap = i.uv_texcoord * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
			float4 tex2DNode7_g497 = tex2D( _DetailAlbedoMap, uv_DetailAlbedoMap );
			float clampResult27_g497 = clamp( _DetailSaturation , -1.0 , 100.0 );
			float3 desaturateInitialColor29_g497 = ( _DetailColor * tex2DNode7_g497 ).rgb;
			float desaturateDot29_g497 = dot( desaturateInitialColor29_g497, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar29_g497 = lerp( desaturateInitialColor29_g497, desaturateDot29_g497.xxx, -clampResult27_g497 );
			float4 AlbedoDetail153 = CalculateContrast(_DetailBrightness,float4( desaturateVar29_g497 , 0.0 ));
			float4 lerpResult343 = lerp( temp_output_372_0 , AlbedoDetail153 , Mask158);
			float4 blendOpSrc15_g516 = temp_output_372_0;
			float4 blendOpDest15_g516 = lerpResult343;
			float4 lerpBlendMode15_g516 = lerp(blendOpDest15_g516,(( blendOpDest15_g516 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest15_g516 ) * ( 1.0 - blendOpSrc15_g516 ) ) : ( 2.0 * blendOpDest15_g516 * blendOpSrc15_g516 ) ),Mask158.x);
			#ifdef _VISUALIZEMASK_ON
				float4 staticSwitch365 = Mask158;
			#else
				float4 staticSwitch365 = (( _BlendmodeOverlay )?( ( saturate( lerpBlendMode15_g516 )) ):( lerpResult343 ));
			#endif
			float4 Albedo26 = staticSwitch365;
			o.Albedo = Albedo26.xyz;
			float2 uv_EmissionMap = i.uv_texcoord * _EmissionMap_ST.xy + _EmissionMap_ST.zw;
			float2 break26_g513 = uv_EmissionMap;
			float2 appendResult14_g513 = (float2(( break26_g513.x * GlobalTilingX11 ) , ( break26_g513.y * GlobalTilingY8 )));
			float2 appendResult13_g513 = (float2(( break26_g513.x + GlobalOffsetX10 ) , ( break26_g513.y + GlobalOffsetY9 )));
			float4 temp_output_248_0 = ( _EmissionColor * tex2D( _EmissionMap, ( ( appendResult14_g513 + appendResult13_g513 ) + Parallaxe359 ) ) * _EmissionIntensity );
			float4 temp_cast_18 = (0.0).xxxx;
			float4 lerpResult190 = lerp( temp_output_248_0 , temp_cast_18 , Mask158);
			float4 Emission110 = (( _UseEmissionFromMainProperties )?( temp_output_248_0 ):( lerpResult190 ));
			o.Emission = Emission110.rgb;
			float2 uv_SpecGlossMap = i.uv_texcoord * _SpecGlossMap_ST.xy + _SpecGlossMap_ST.zw;
			float2 break26_g511 = uv_SpecGlossMap;
			float2 appendResult14_g511 = (float2(( break26_g511.x * GlobalTilingX11 ) , ( break26_g511.y * GlobalTilingY8 )));
			float2 appendResult13_g511 = (float2(( break26_g511.x + GlobalOffsetX10 ) , ( break26_g511.y + GlobalOffsetY9 )));
			float4 tex2DNode3_g510 = tex2D( _SpecGlossMap, ( ( appendResult14_g511 + appendResult13_g511 ) + Parallaxe359 ) );
			float2 uv_DetailSpecGlossMap = i.uv_texcoord * _DetailSpecGlossMap_ST.xy + _DetailSpecGlossMap_ST.zw;
			float4 tex2DNode3_g503 = tex2D( _DetailSpecGlossMap, uv_DetailSpecGlossMap );
			float4 DetailMetallic176 = ( _DetailSpecularColor * tex2DNode3_g503 * _DetailSpecularIntensity );
			float4 lerpResult179 = lerp( ( _SpecularColor * tex2DNode3_g510 * _SpecularIntensity ) , DetailMetallic176 , Mask158);
			float4 Specular41 = lerpResult179;
			o.Specular = Specular41.rgb;
			float BaseColorAlpha46 = (( _InvertABaseColor )?( ( 1.0 - tex2DNode7_g501.a ) ):( tex2DNode7_g501.a ));
			#if defined(_SOURCE_SPECULARALPHA)
				float staticSwitch31_g510 = ( tex2DNode3_g510.a * _Glossiness );
			#elif defined(_SOURCE_BASECOLORALPHA)
				float staticSwitch31_g510 = ( _Glossiness * BaseColorAlpha46 );
			#else
				float staticSwitch31_g510 = ( tex2DNode3_g510.a * _Glossiness );
			#endif
			float DetailBaseColorAlpha170 = tex2DNode7_g497.a;
			#if defined(_DETAILSOURCE_SPECULARALPHA)
				float staticSwitch31_g503 = ( tex2DNode3_g503.a * _DetailGlossiness );
			#elif defined(_DETAILSOURCE_ALBEDOALPHA)
				float staticSwitch31_g503 = ( _DetailGlossiness * DetailBaseColorAlpha170 );
			#else
				float staticSwitch31_g503 = ( tex2DNode3_g503.a * _DetailGlossiness );
			#endif
			float DetailSmoothness175 = staticSwitch31_g503;
			float lerpResult182 = lerp( staticSwitch31_g510 , DetailSmoothness175 , Mask158.r);
			float4 temp_cast_22 = (lerpResult182).xxxx;
			#ifdef _VISUALIZEMASK_ON
				float4 staticSwitch368 = Mask158;
			#else
				float4 staticSwitch368 = temp_cast_22;
			#endif
			float4 Smoothness40 = staticSwitch368;
			o.Smoothness = Smoothness40.r;
			float2 uv_OcclusionMap = i.uv_texcoord * _OcclusionMap_ST.xy + _OcclusionMap_ST.zw;
			float2 break26_g515 = uv_OcclusionMap;
			float2 appendResult14_g515 = (float2(( break26_g515.x * GlobalTilingX11 ) , ( break26_g515.y * GlobalTilingY8 )));
			float2 appendResult13_g515 = (float2(( break26_g515.x + GlobalOffsetX10 ) , ( break26_g515.y + GlobalOffsetY9 )));
			float blendOpSrc2_g514 = tex2D( _OcclusionMap, ( ( appendResult14_g515 + appendResult13_g515 ) + Parallaxe359 ) ).r;
			float blendOpDest2_g514 = ( 1.0 - _AoIntensity );
			float temp_output_369_0 = ( saturate( ( 1.0 - ( 1.0 - blendOpSrc2_g514 ) * ( 1.0 - blendOpDest2_g514 ) ) ));
			float lerpResult185 = lerp( temp_output_369_0 , 1.0 , Mask158.r);
			float AmbientOcclusion94 = (( _UseAoFromMainProperties )?( temp_output_369_0 ):( lerpResult185 ));
			o.Occlusion = AmbientOcclusion94;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecular keepalpha fullforwardshadows 

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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
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
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}