// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Vwater"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin]_LargeWaves("Large Waves", 2D) = "bump" {}
		_SmallWaves("Small Waves", 2D) = "bump" {}
		_MediumWaves("Medium Waves", 2D) = "bump" {}
		_WaterSmoothness("Water Smoothness", Range( 0 , 1)) = 0.99
		_LargeWavesHeight("Large Waves Height", Range( 0 , 2)) = 0.4170583
		_MediumWavesHeight("Medium Waves Height", Range( 0 , 2)) = 0.4825
		_SmallWavesHeight("Small Waves Height", Range( 0 , 2)) = 0.4328485
		_LargeWaveSpeed("Large Wave Speed", Float) = 1
		_SmallWaveSpeed("Small Wave Speed", Float) = 1
		_MediumWaveSpeed("Medium Wave Speed", Float) = 1
		_LargeWaveDirection("Large Wave Direction", Range( 0 , 6.24)) = 1.56
		_MediumWaveDirection("Medium Wave Direction", Range( 0 , 6.24)) = 1.56
		_SmallWaveDirection("Small Wave Direction", Range( 0 , 6.24)) = 1.56
		_LargeWavesFlow("Large Waves Flow", Range( 0 , 1)) = 0
		_MediumWavesFlow("Medium Waves Flow", Range( 0 , 1)) = 0
		_SmallWavesFlow("Small Waves Flow", Range( 0 , 1)) = 0
		_largeWavesTilling("large Waves Tilling", Float) = 1
		_SmallWavesTilling("Small Waves Tilling", Float) = 1
		_MediumWavesTilling("Medium Waves Tilling", Float) = 1
		_EdgeBlending("Edge Blending", Range( 0 , 1)) = 0.4588235
		_LifeDensity("Life Density", Range( 0 , 100)) = 0.4588235
		_WaterDensity("Water Density", Range( 0 , 100)) = 0.4588235
		_WaterDepth("Water Depth", Range( 0 , 1)) = 0.4588235
		_LifeDepth("Life Depth", Range( 0 , 1)) = 0.4588235
		_LifeColor("Life Color", Color) = (1,0.7783019,0.9634851,0)
		_WaterAbsorbedColor("Water Absorbed Color", Color) = (0,0,0,0)
		_foam("foam", 2D) = "white" {}
		_FoamTilling("Foam  Tilling", Float) = 0
		_FoamColor("Foam Color", Color) = (1,1,1,0)
		_FoamDistance("Foam Distance", Range( 0 , 10)) = 0.5411765
		_WaveFoam("Wave Foam", Range( 0 , 1)) = 0.5411765
		_FoamSpeed("Foam Speed", Float) = 0
		_Scatteringpower("Scattering power", Float) = 0
		_ScatteringScale("Scattering Scale", Float) = 0
		_FoamSoftness("Foam Softness", Range( 0.0001 , 1)) = 0.0001
		[ASEEnd]_Scatteringbrightness("Scattering brightness", Range( 0 , 1)) = 0.3

		//_TransmissionShadow( "Transmission Shadow", Range( 0, 1 ) ) = 0.5
		//_TransStrength( "Trans Strength", Range( 0, 50 ) ) = 1
		//_TransNormal( "Trans Normal Distortion", Range( 0, 1 ) ) = 0.5
		//_TransScattering( "Trans Scattering", Range( 1, 50 ) ) = 2
		//_TransDirect( "Trans Direct", Range( 0, 1 ) ) = 0.9
		//_TransAmbient( "Trans Ambient", Range( 0, 1 ) ) = 0.1
		//_TransShadow( "Trans Shadow", Range( 0, 1 ) ) = 0.5
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
		Cull Off
		AlphaToMask Off
		HLSLINCLUDE
		#pragma target 2.0

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS
		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 80301
			#define REQUIRE_OPAQUE_TEXTURE 1
			#define REQUIRE_DEPTH_TEXTURE 1

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_FORWARD

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
			    #define ENABLE_TERRAIN_PERPIXEL_NORMAL
			#endif

			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_SHADOWCOORDS
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord : TEXCOORD0;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 lightmapUVOrVertexSH : TEXCOORD0;
				half4 fogFactorAndVertexLight : TEXCOORD1;
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord : TEXCOORD2;
				#endif
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 screenPos : TEXCOORD6;
				#endif
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _FoamColor;
			float4 _WaterAbsorbedColor;
			float4 _LifeColor;
			float _Scatteringbrightness;
			float _ScatteringScale;
			float _Scatteringpower;
			float _FoamSoftness;
			float _FoamDistance;
			float _WaveFoam;
			float _FoamTilling;
			float _FoamSpeed;
			float _WaterDensity;
			float _WaterDepth;
			float _LifeDepth;
			float _LifeDensity;
			float _SmallWavesFlow;
			float _SmallWavesHeight;
			float _SmallWavesTilling;
			float _SmallWaveDirection;
			float _SmallWaveSpeed;
			float _MediumWavesFlow;
			float _MediumWavesHeight;
			float _MediumWavesTilling;
			float _MediumWaveDirection;
			float _MediumWaveSpeed;
			float _LargeWavesFlow;
			float _LargeWavesHeight;
			float _largeWavesTilling;
			float _LargeWaveDirection;
			float _LargeWaveSpeed;
			float _WaterSmoothness;
			float _EdgeBlending;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _LargeWaves;
			sampler2D _MediumWaves;
			sampler2D _SmallWaves;
			uniform float4 _CameraDepthTexture_TexelSize;
			sampler2D _foam;


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
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 positionVS = TransformWorldToView( positionWS );
				float4 positionCS = TransformWorldToHClip( positionWS );

				VertexNormalInputs normalInput = GetVertexNormalInputs( v.ase_normal, v.ase_tangent );

				o.tSpace0 = float4( normalInput.normalWS, positionWS.x);
				o.tSpace1 = float4( normalInput.tangentWS, positionWS.y);
				o.tSpace2 = float4( normalInput.bitangentWS, positionWS.z);

				OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				OUTPUT_SH( normalInput.normalWS.xyz, o.lightmapUVOrVertexSH.xyz );

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					o.lightmapUVOrVertexSH.zw = v.texcoord;
					o.lightmapUVOrVertexSH.xy = v.texcoord * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				half3 vertexLight = VertexLighting( positionWS, normalInput.normalWS );
				#ifdef ASE_FOG
					half fogFactor = ComputeFogFactor( positionCS.z );
				#else
					half fogFactor = 0;
				#endif
				o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
				
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				
				o.clipPos = positionCS;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				o.screenPos = ComputeScreenPos(positionCS);
				#endif
				return o;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.texcoord = v.texcoord;
				o.texcoord1 = v.texcoord1;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.texcoord = patch[0].texcoord * bary.x + patch[1].texcoord * bary.y + patch[2].texcoord * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif

			half4 frag ( VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float2 sampleCoords = (IN.lightmapUVOrVertexSH.zw / _TerrainHeightmapRecipSize.zw + 0.5f) * _TerrainHeightmapRecipSize.xy;
					float3 WorldNormal = TransformObjectToWorldNormal(normalize(SAMPLE_TEXTURE2D(_TerrainNormalmapTexture, sampler_TerrainNormalmapTexture, sampleCoords).rgb * 2 - 1));
					float3 WorldTangent = -cross(GetObjectToWorldMatrix()._13_23_33, WorldNormal);
					float3 WorldBiTangent = cross(WorldNormal, -WorldTangent);
				#else
					float3 WorldNormal = normalize( IN.tSpace0.xyz );
					float3 WorldTangent = IN.tSpace1.xyz;
					float3 WorldBiTangent = IN.tSpace2.xyz;
				#endif
				float3 WorldPosition = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 WorldViewDirection = _WorldSpaceCameraPos.xyz  - WorldPosition;
				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 ScreenPos = IN.screenPos;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					ShadowCoords = IN.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
				#endif
	
				WorldViewDirection = SafeNormalize( WorldViewDirection );

				float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ScreenPos );
				float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
				float2 appendResult3 = (float2(ase_grabScreenPosNorm.r , ase_grabScreenPosNorm.g));
				float DeltaTime29 = _TimeParameters.x;
				float2 appendResult55 = (float2(( DeltaTime29 * ( _LargeWaveSpeed * sin( _LargeWaveDirection ) ) ) , ( DeltaTime29 * ( _LargeWaveSpeed * cos( _LargeWaveDirection ) ) )));
				float2 appendResult22 = (float2(WorldPosition.x , WorldPosition.z));
				float2 WorldSpaceUv23 = appendResult22;
				float3 unpack6 = UnpackNormalScale( tex2D( _LargeWaves, ( ( appendResult55 + WorldSpaceUv23 ) / ( _largeWavesTilling * 0.9 ) ) ), _LargeWavesHeight );
				unpack6.z = lerp( 1, unpack6.z, saturate(_LargeWavesHeight) );
				float3 unpack7 = UnpackNormalScale( tex2D( _LargeWaves, ( ( WorldSpaceUv23 - ( appendResult55 * ( ( _LargeWavesFlow - 0.5 ) * 2.0 ) ) ) / _largeWavesTilling ) ), _LargeWavesHeight );
				unpack7.z = lerp( 1, unpack7.z, saturate(_LargeWavesHeight) );
				float3 lerpResult9 = lerp( unpack6 , unpack7 , 0.5);
				float3 temp_output_92_0 = ( lerpResult9 * 0.1 );
				float2 appendResult80 = (float2(( DeltaTime29 * ( _MediumWaveSpeed * sin( _MediumWaveDirection ) ) ) , ( DeltaTime29 * ( _MediumWaveSpeed * cos( _MediumWaveDirection ) ) )));
				float3 unpack68 = UnpackNormalScale( tex2D( _MediumWaves, ( temp_output_92_0 + float3( ( ( appendResult80 + WorldSpaceUv23 ) / ( _MediumWavesTilling * 0.9 ) ) ,  0.0 ) ).xy ), _MediumWavesHeight );
				unpack68.z = lerp( 1, unpack68.z, saturate(_MediumWavesHeight) );
				float3 unpack69 = UnpackNormalScale( tex2D( _MediumWaves, ( temp_output_92_0 + float3( ( ( WorldSpaceUv23 - ( appendResult80 * ( ( _MediumWavesFlow - 0.5 ) * 2.0 ) ) ) / _MediumWavesTilling ) ,  0.0 ) ).xy ), _MediumWavesHeight );
				unpack69.z = lerp( 1, unpack69.z, saturate(_MediumWavesHeight) );
				float3 lerpResult94 = lerp( unpack68 , unpack69 , 0.5);
				float3 lerpResult95 = lerp( lerpResult9 , lerpResult94 , 0.5);
				float3 temp_output_123_0 = ( 0.1 * lerpResult94 );
				float2 appendResult108 = (float2(( DeltaTime29 * ( _SmallWaveSpeed * sin( _SmallWaveDirection ) ) ) , ( DeltaTime29 * ( _SmallWaveSpeed * cos( _SmallWaveDirection ) ) )));
				float3 unpack120 = UnpackNormalScale( tex2D( _SmallWaves, ( temp_output_123_0 + float3( ( ( appendResult108 + WorldSpaceUv23 ) / ( _SmallWavesTilling * 0.9 ) ) ,  0.0 ) ).xy ), _SmallWavesHeight );
				unpack120.z = lerp( 1, unpack120.z, saturate(_SmallWavesHeight) );
				float3 unpack121 = UnpackNormalScale( tex2D( _SmallWaves, ( temp_output_123_0 + float3( ( ( WorldSpaceUv23 - ( appendResult108 * ( ( _SmallWavesFlow - 0.5 ) * 2.0 ) ) ) / _SmallWavesTilling ) ,  0.0 ) ).xy ), _SmallWavesHeight );
				unpack121.z = lerp( 1, unpack121.z, saturate(_SmallWavesHeight) );
				float3 lerpResult124 = lerp( unpack120 , unpack121 , 0.5);
				float3 lerpResult125 = lerp( lerpResult95 , lerpResult124 , 0.5);
				float3 Normals11 = lerpResult125;
				float4 fetchOpaqueVal1 = float4( SHADERGRAPH_SAMPLE_SCENE_COLOR( ( float3( appendResult3 ,  0.0 ) + ( Normals11 * 0.1 ) ).xy ), 1.0 );
				float4 DistortedTransparentTexture4 = fetchOpaqueVal1;
				float4 temp_output_401_0 = saturate( _MainLightColor );
				float4 Life161 = saturate( ( temp_output_401_0 - _LifeColor ) );
				float4 ase_screenPosNorm = ScreenPos / ScreenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth137 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth137 = abs( ( screenDepth137 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( 1.0 ) );
				float Density139 = distanceDepth137;
				float4 lerpResult162 = lerp( DistortedTransparentTexture4 , Life161 , saturate( ( pow( Density139 , _LifeDensity ) * _LifeDepth ) ));
				float4 DepthColor154 = saturate( ( temp_output_401_0 - _WaterAbsorbedColor ) );
				float4 lerpResult141 = lerp( lerpResult162 , DepthColor154 , saturate( ( _WaterDepth * pow( Density139 , _WaterDensity ) ) ));
				float4 FinalColored144 = lerpResult141;
				float temp_output_204_0 = ( DeltaTime29 * _FoamSpeed );
				float3 temp_output_181_0 = ( ( Normals11 * 0.1 ) + float3( ( WorldSpaceUv23 / _FoamTilling ) ,  0.0 ) );
				float3 temp_cast_12 = (temp_output_204_0).xxx;
				float lerpResult201 = lerp( tex2D( _foam, ( temp_output_204_0 + temp_output_181_0 ).xy ).r , tex2D( _foam, ( temp_cast_12 - ( temp_output_181_0 * 0.9 ) ).xy ).r , 0.5);
				float FoamTexture230 = lerpResult201;
				float temp_output_360_0 = ( 1.0 - FoamTexture230 );
				float temp_output_285_0 = saturate( Normals11.y );
				float temp_output_394_0 = ( 1.0 - saturate( (0.0 + (temp_output_360_0 - ( 1.0 - ( ( temp_output_285_0 / 0.3 ) * _WaveFoam ) )) * (1.0 - 0.0) / (1.0 - ( 1.0 - ( ( temp_output_285_0 / 0.3 ) * _WaveFoam ) ))) ) );
				float lerpResult395 = lerp( ( temp_output_394_0 / 3.0 ) , saturate( (0.0 + (temp_output_360_0 - ( ( 1.0 - Density139 ) * _FoamDistance )) * (1.0 - 0.0) / (_FoamDistance - ( ( 1.0 - Density139 ) * _FoamDistance ))) ) , temp_output_394_0);
				float clampResult358 = clamp( lerpResult395 , 0.0 , 1.0 );
				float Foam177 = pow( saturate( clampResult358 ) , _FoamSoftness );
				float4 lerpResult175 = lerp( _FoamColor , FinalColored144 , saturate( Foam177 ));
				
				float ase_lightAtten = 0;
				Light ase_lightAtten_mainLight = GetMainLight( ShadowCoords );
				ase_lightAtten = ase_lightAtten_mainLight.distanceAttenuation * ase_lightAtten_mainLight.shadowAttenuation;
				float dotResult242 = dot( WorldViewDirection , -_MainLightPosition.xyz );
				float3 temp_cast_16 = (dotResult242).xxx;
				float dotResult259 = dot( temp_cast_16 , Normals11 );
				float dotResult248 = dot( pow( dotResult259 , _Scatteringpower ) , _ScatteringScale );
				float UnderwaterScat244 = saturate( dotResult248 );
				
				float Height286 = temp_output_285_0;
				float clampResult324 = clamp( Height286 , 0.0 , 1.0 );
				
				float screenDepth128 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth128 = abs( ( screenDepth128 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _EdgeBlending ) );
				float Edgeblending130 = distanceDepth128;
				
				float3 Albedo = lerpResult175.rgb;
				float3 Normal = Normals11;
				float3 Emission = ( ase_lightAtten * ( ( Life161 * UnderwaterScat244 ) * _Scatteringbrightness ) ).rgb;
				float3 Specular = 0.5;
				float Metallic = 0;
				float Smoothness = ( ( _WaterSmoothness * Foam177 ) * ( 1.0 - ( clampResult324 + -0.05 ) ) );
				float Occlusion = 1;
				float Alpha = saturate( Edgeblending130 );
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = 0;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				InputData inputData;
				inputData.positionWS = WorldPosition;
				inputData.viewDirectionWS = WorldViewDirection;
				inputData.shadowCoord = ShadowCoords;

				#ifdef _NORMALMAP
					#if _NORMAL_DROPOFF_TS
					inputData.normalWS = TransformTangentToWorld(Normal, half3x3( WorldTangent, WorldBiTangent, WorldNormal ));
					#elif _NORMAL_DROPOFF_OS
					inputData.normalWS = TransformObjectToWorldNormal(Normal);
					#elif _NORMAL_DROPOFF_WS
					inputData.normalWS = Normal;
					#endif
					inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
				#else
					inputData.normalWS = WorldNormal;
				#endif

				#ifdef ASE_FOG
					inputData.fogCoord = IN.fogFactorAndVertexLight.x;
				#endif

				inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float3 SH = SampleSH(inputData.normalWS.xyz);
				#else
					float3 SH = IN.lightmapUVOrVertexSH.xyz;
				#endif

				inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, SH, inputData.normalWS );
				#ifdef _ASE_BAKEDGI
					inputData.bakedGI = BakedGI;
				#endif
				half4 color = UniversalFragmentPBR(
					inputData, 
					Albedo, 
					Metallic, 
					Specular, 
					Smoothness, 
					Occlusion, 
					Emission, 
					Alpha);

				#ifdef _TRANSMISSION_ASE
				{
					float shadow = _TransmissionShadow;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );
					half3 mainTransmission = max(0 , -dot(inputData.normalWS, mainLight.direction)) * mainAtten * Transmission;
					color.rgb += Albedo * mainTransmission;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 transmission = max(0 , -dot(inputData.normalWS, light.direction)) * atten * Transmission;
							color.rgb += Albedo * transmission;
						}
					#endif
				}
				#endif

				#ifdef _TRANSLUCENCY_ASE
				{
					float shadow = _TransShadow;
					float normal = _TransNormal;
					float scattering = _TransScattering;
					float direct = _TransDirect;
					float ambient = _TransAmbient;
					float strength = _TransStrength;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );

					half3 mainLightDir = mainLight.direction + inputData.normalWS * normal;
					half mainVdotL = pow( saturate( dot( inputData.viewDirectionWS, -mainLightDir ) ), scattering );
					half3 mainTranslucency = mainAtten * ( mainVdotL * direct + inputData.bakedGI * ambient ) * Translucency;
					color.rgb += Albedo * mainTranslucency * strength;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 lightDir = light.direction + inputData.normalWS * normal;
							half VdotL = pow( saturate( dot( inputData.viewDirectionWS, -lightDir ) ), scattering );
							half3 translucency = atten * ( VdotL * direct + inputData.bakedGI * ambient ) * Translucency;
							color.rgb += Albedo * translucency * strength;
						}
					#endif
				}
				#endif

				#ifdef _REFRACTION_ASE
					float4 projScreenPos = ScreenPos / ScreenPos.w;
					float3 refractionOffset = ( RefractionIndex - 1.0 ) * mul( UNITY_MATRIX_V, WorldNormal ).xyz * ( 1.0 - dot( WorldNormal, WorldViewDirection ) );
					projScreenPos.xy += refractionOffset.xy;
					float3 refraction = SHADERGRAPH_SAMPLE_SCENE_COLOR( projScreenPos ) * RefractionColor;
					color.rgb = lerp( refraction, color.rgb, color.a );
					color.a = 1;
				#endif

				#ifdef ASE_FINAL_COLOR_ALPHA_MULTIPLY
					color.rgb *= color.a;
				#endif

				#ifdef ASE_FOG
					#ifdef TERRAIN_SPLAT_ADDPASS
						color.rgb = MixFogColor(color.rgb, half3( 0, 0, 0 ), IN.fogFactorAndVertexLight.x );
					#else
						color.rgb = MixFog(color.rgb, IN.fogFactorAndVertexLight.x);
					#endif
				#endif
				
				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif

				return color;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual
			AlphaToMask Off

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 80301
			#define REQUIRE_DEPTH_TEXTURE 1

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_SHADOWCASTER

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _FoamColor;
			float4 _WaterAbsorbedColor;
			float4 _LifeColor;
			float _Scatteringbrightness;
			float _ScatteringScale;
			float _Scatteringpower;
			float _FoamSoftness;
			float _FoamDistance;
			float _WaveFoam;
			float _FoamTilling;
			float _FoamSpeed;
			float _WaterDensity;
			float _WaterDepth;
			float _LifeDepth;
			float _LifeDensity;
			float _SmallWavesFlow;
			float _SmallWavesHeight;
			float _SmallWavesTilling;
			float _SmallWaveDirection;
			float _SmallWaveSpeed;
			float _MediumWavesFlow;
			float _MediumWavesHeight;
			float _MediumWavesTilling;
			float _MediumWaveDirection;
			float _MediumWaveSpeed;
			float _LargeWavesFlow;
			float _LargeWavesHeight;
			float _largeWavesTilling;
			float _LargeWaveDirection;
			float _LargeWaveSpeed;
			float _WaterSmoothness;
			float _EdgeBlending;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			uniform float4 _CameraDepthTexture_TexelSize;


			
			float3 _LightDirection;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord2 = screenPos;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif
				float3 normalWS = TransformObjectToWorldDir(v.ase_normal);

				float4 clipPos = TransformWorldToHClip( ApplyShadowBias( positionWS, normalWS, _LightDirection ) );

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = clipPos;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif

			half4 frag(	VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );
				
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float4 screenPos = IN.ase_texcoord2;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth128 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth128 = abs( ( screenDepth128 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _EdgeBlending ) );
				float Edgeblending130 = distanceDepth128;
				
				float Alpha = saturate( Edgeblending130 );
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					#ifdef _ALPHATEST_SHADOW_ON
						clip(Alpha - AlphaClipThresholdShadow);
					#else
						clip(Alpha - AlphaClipThreshold);
					#endif
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif
				return 0;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 80301
			#define REQUIRE_DEPTH_TEXTURE 1

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _FoamColor;
			float4 _WaterAbsorbedColor;
			float4 _LifeColor;
			float _Scatteringbrightness;
			float _ScatteringScale;
			float _Scatteringpower;
			float _FoamSoftness;
			float _FoamDistance;
			float _WaveFoam;
			float _FoamTilling;
			float _FoamSpeed;
			float _WaterDensity;
			float _WaterDepth;
			float _LifeDepth;
			float _LifeDensity;
			float _SmallWavesFlow;
			float _SmallWavesHeight;
			float _SmallWavesTilling;
			float _SmallWaveDirection;
			float _SmallWaveSpeed;
			float _MediumWavesFlow;
			float _MediumWavesHeight;
			float _MediumWavesTilling;
			float _MediumWaveDirection;
			float _MediumWaveSpeed;
			float _LargeWavesFlow;
			float _LargeWavesHeight;
			float _largeWavesTilling;
			float _LargeWaveDirection;
			float _LargeWaveSpeed;
			float _WaterSmoothness;
			float _EdgeBlending;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			uniform float4 _CameraDepthTexture_TexelSize;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord2 = screenPos;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;
				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif
			half4 frag(	VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float4 screenPos = IN.ase_texcoord2;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth128 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth128 = abs( ( screenDepth128 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _EdgeBlending ) );
				float Edgeblending130 = distanceDepth128;
				
				float Alpha = saturate( Edgeblending130 );
				float AlphaClipThreshold = 0.5;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				#ifdef ASE_DEPTH_WRITE_ON
				outputDepth = DepthValue;
				#endif
				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Meta"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 80301
			#define REQUIRE_OPAQUE_TEXTURE 1
			#define REQUIRE_DEPTH_TEXTURE 1

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_META

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_SHADOWCOORDS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT


			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _FoamColor;
			float4 _WaterAbsorbedColor;
			float4 _LifeColor;
			float _Scatteringbrightness;
			float _ScatteringScale;
			float _Scatteringpower;
			float _FoamSoftness;
			float _FoamDistance;
			float _WaveFoam;
			float _FoamTilling;
			float _FoamSpeed;
			float _WaterDensity;
			float _WaterDepth;
			float _LifeDepth;
			float _LifeDensity;
			float _SmallWavesFlow;
			float _SmallWavesHeight;
			float _SmallWavesTilling;
			float _SmallWaveDirection;
			float _SmallWaveSpeed;
			float _MediumWavesFlow;
			float _MediumWavesHeight;
			float _MediumWavesTilling;
			float _MediumWaveDirection;
			float _MediumWaveSpeed;
			float _LargeWavesFlow;
			float _LargeWavesHeight;
			float _largeWavesTilling;
			float _LargeWaveDirection;
			float _LargeWaveSpeed;
			float _WaterSmoothness;
			float _EdgeBlending;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _LargeWaves;
			sampler2D _MediumWaves;
			sampler2D _SmallWaves;
			uniform float4 _CameraDepthTexture_TexelSize;
			sampler2D _foam;


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
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord2 = screenPos;
				
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				o.clipPos = MetaVertexPosition( v.vertex, v.texcoord1.xy, v.texcoord1.xy, unity_LightmapST, unity_DynamicLightmapST );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float4 screenPos = IN.ase_texcoord2;
				float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( screenPos );
				float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
				float2 appendResult3 = (float2(ase_grabScreenPosNorm.r , ase_grabScreenPosNorm.g));
				float DeltaTime29 = _TimeParameters.x;
				float2 appendResult55 = (float2(( DeltaTime29 * ( _LargeWaveSpeed * sin( _LargeWaveDirection ) ) ) , ( DeltaTime29 * ( _LargeWaveSpeed * cos( _LargeWaveDirection ) ) )));
				float2 appendResult22 = (float2(WorldPosition.x , WorldPosition.z));
				float2 WorldSpaceUv23 = appendResult22;
				float3 unpack6 = UnpackNormalScale( tex2D( _LargeWaves, ( ( appendResult55 + WorldSpaceUv23 ) / ( _largeWavesTilling * 0.9 ) ) ), _LargeWavesHeight );
				unpack6.z = lerp( 1, unpack6.z, saturate(_LargeWavesHeight) );
				float3 unpack7 = UnpackNormalScale( tex2D( _LargeWaves, ( ( WorldSpaceUv23 - ( appendResult55 * ( ( _LargeWavesFlow - 0.5 ) * 2.0 ) ) ) / _largeWavesTilling ) ), _LargeWavesHeight );
				unpack7.z = lerp( 1, unpack7.z, saturate(_LargeWavesHeight) );
				float3 lerpResult9 = lerp( unpack6 , unpack7 , 0.5);
				float3 temp_output_92_0 = ( lerpResult9 * 0.1 );
				float2 appendResult80 = (float2(( DeltaTime29 * ( _MediumWaveSpeed * sin( _MediumWaveDirection ) ) ) , ( DeltaTime29 * ( _MediumWaveSpeed * cos( _MediumWaveDirection ) ) )));
				float3 unpack68 = UnpackNormalScale( tex2D( _MediumWaves, ( temp_output_92_0 + float3( ( ( appendResult80 + WorldSpaceUv23 ) / ( _MediumWavesTilling * 0.9 ) ) ,  0.0 ) ).xy ), _MediumWavesHeight );
				unpack68.z = lerp( 1, unpack68.z, saturate(_MediumWavesHeight) );
				float3 unpack69 = UnpackNormalScale( tex2D( _MediumWaves, ( temp_output_92_0 + float3( ( ( WorldSpaceUv23 - ( appendResult80 * ( ( _MediumWavesFlow - 0.5 ) * 2.0 ) ) ) / _MediumWavesTilling ) ,  0.0 ) ).xy ), _MediumWavesHeight );
				unpack69.z = lerp( 1, unpack69.z, saturate(_MediumWavesHeight) );
				float3 lerpResult94 = lerp( unpack68 , unpack69 , 0.5);
				float3 lerpResult95 = lerp( lerpResult9 , lerpResult94 , 0.5);
				float3 temp_output_123_0 = ( 0.1 * lerpResult94 );
				float2 appendResult108 = (float2(( DeltaTime29 * ( _SmallWaveSpeed * sin( _SmallWaveDirection ) ) ) , ( DeltaTime29 * ( _SmallWaveSpeed * cos( _SmallWaveDirection ) ) )));
				float3 unpack120 = UnpackNormalScale( tex2D( _SmallWaves, ( temp_output_123_0 + float3( ( ( appendResult108 + WorldSpaceUv23 ) / ( _SmallWavesTilling * 0.9 ) ) ,  0.0 ) ).xy ), _SmallWavesHeight );
				unpack120.z = lerp( 1, unpack120.z, saturate(_SmallWavesHeight) );
				float3 unpack121 = UnpackNormalScale( tex2D( _SmallWaves, ( temp_output_123_0 + float3( ( ( WorldSpaceUv23 - ( appendResult108 * ( ( _SmallWavesFlow - 0.5 ) * 2.0 ) ) ) / _SmallWavesTilling ) ,  0.0 ) ).xy ), _SmallWavesHeight );
				unpack121.z = lerp( 1, unpack121.z, saturate(_SmallWavesHeight) );
				float3 lerpResult124 = lerp( unpack120 , unpack121 , 0.5);
				float3 lerpResult125 = lerp( lerpResult95 , lerpResult124 , 0.5);
				float3 Normals11 = lerpResult125;
				float4 fetchOpaqueVal1 = float4( SHADERGRAPH_SAMPLE_SCENE_COLOR( ( float3( appendResult3 ,  0.0 ) + ( Normals11 * 0.1 ) ).xy ), 1.0 );
				float4 DistortedTransparentTexture4 = fetchOpaqueVal1;
				float4 temp_output_401_0 = saturate( _MainLightColor );
				float4 Life161 = saturate( ( temp_output_401_0 - _LifeColor ) );
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth137 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth137 = abs( ( screenDepth137 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( 1.0 ) );
				float Density139 = distanceDepth137;
				float4 lerpResult162 = lerp( DistortedTransparentTexture4 , Life161 , saturate( ( pow( Density139 , _LifeDensity ) * _LifeDepth ) ));
				float4 DepthColor154 = saturate( ( temp_output_401_0 - _WaterAbsorbedColor ) );
				float4 lerpResult141 = lerp( lerpResult162 , DepthColor154 , saturate( ( _WaterDepth * pow( Density139 , _WaterDensity ) ) ));
				float4 FinalColored144 = lerpResult141;
				float temp_output_204_0 = ( DeltaTime29 * _FoamSpeed );
				float3 temp_output_181_0 = ( ( Normals11 * 0.1 ) + float3( ( WorldSpaceUv23 / _FoamTilling ) ,  0.0 ) );
				float3 temp_cast_12 = (temp_output_204_0).xxx;
				float lerpResult201 = lerp( tex2D( _foam, ( temp_output_204_0 + temp_output_181_0 ).xy ).r , tex2D( _foam, ( temp_cast_12 - ( temp_output_181_0 * 0.9 ) ).xy ).r , 0.5);
				float FoamTexture230 = lerpResult201;
				float temp_output_360_0 = ( 1.0 - FoamTexture230 );
				float temp_output_285_0 = saturate( Normals11.y );
				float temp_output_394_0 = ( 1.0 - saturate( (0.0 + (temp_output_360_0 - ( 1.0 - ( ( temp_output_285_0 / 0.3 ) * _WaveFoam ) )) * (1.0 - 0.0) / (1.0 - ( 1.0 - ( ( temp_output_285_0 / 0.3 ) * _WaveFoam ) ))) ) );
				float lerpResult395 = lerp( ( temp_output_394_0 / 3.0 ) , saturate( (0.0 + (temp_output_360_0 - ( ( 1.0 - Density139 ) * _FoamDistance )) * (1.0 - 0.0) / (_FoamDistance - ( ( 1.0 - Density139 ) * _FoamDistance ))) ) , temp_output_394_0);
				float clampResult358 = clamp( lerpResult395 , 0.0 , 1.0 );
				float Foam177 = pow( saturate( clampResult358 ) , _FoamSoftness );
				float4 lerpResult175 = lerp( _FoamColor , FinalColored144 , saturate( Foam177 ));
				
				float ase_lightAtten = 0;
				Light ase_lightAtten_mainLight = GetMainLight( ShadowCoords );
				ase_lightAtten = ase_lightAtten_mainLight.distanceAttenuation * ase_lightAtten_mainLight.shadowAttenuation;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float dotResult242 = dot( ase_worldViewDir , -_MainLightPosition.xyz );
				float3 temp_cast_16 = (dotResult242).xxx;
				float dotResult259 = dot( temp_cast_16 , Normals11 );
				float dotResult248 = dot( pow( dotResult259 , _Scatteringpower ) , _ScatteringScale );
				float UnderwaterScat244 = saturate( dotResult248 );
				
				float screenDepth128 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth128 = abs( ( screenDepth128 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _EdgeBlending ) );
				float Edgeblending130 = distanceDepth128;
				
				
				float3 Albedo = lerpResult175.rgb;
				float3 Emission = ( ase_lightAtten * ( ( Life161 * UnderwaterScat244 ) * _Scatteringbrightness ) ).rgb;
				float Alpha = saturate( Edgeblending130 );
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				MetaInput metaInput = (MetaInput)0;
				metaInput.Albedo = Albedo;
				metaInput.Emission = Emission;
				
				return MetaFragment(metaInput);
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Universal2D"
			Tags { "LightMode"="Universal2D" }

			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 80301
			#define REQUIRE_OPAQUE_TEXTURE 1
			#define REQUIRE_DEPTH_TEXTURE 1

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_2D

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#define ASE_NEEDS_FRAG_WORLD_POSITION


			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _FoamColor;
			float4 _WaterAbsorbedColor;
			float4 _LifeColor;
			float _Scatteringbrightness;
			float _ScatteringScale;
			float _Scatteringpower;
			float _FoamSoftness;
			float _FoamDistance;
			float _WaveFoam;
			float _FoamTilling;
			float _FoamSpeed;
			float _WaterDensity;
			float _WaterDepth;
			float _LifeDepth;
			float _LifeDensity;
			float _SmallWavesFlow;
			float _SmallWavesHeight;
			float _SmallWavesTilling;
			float _SmallWaveDirection;
			float _SmallWaveSpeed;
			float _MediumWavesFlow;
			float _MediumWavesHeight;
			float _MediumWavesTilling;
			float _MediumWaveDirection;
			float _MediumWaveSpeed;
			float _LargeWavesFlow;
			float _LargeWavesHeight;
			float _largeWavesTilling;
			float _LargeWaveDirection;
			float _LargeWaveSpeed;
			float _WaterSmoothness;
			float _EdgeBlending;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _LargeWaves;
			sampler2D _MediumWaves;
			sampler2D _SmallWaves;
			uniform float4 _CameraDepthTexture_TexelSize;
			sampler2D _foam;


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
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord2 = screenPos;
				
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float4 screenPos = IN.ase_texcoord2;
				float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( screenPos );
				float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
				float2 appendResult3 = (float2(ase_grabScreenPosNorm.r , ase_grabScreenPosNorm.g));
				float DeltaTime29 = _TimeParameters.x;
				float2 appendResult55 = (float2(( DeltaTime29 * ( _LargeWaveSpeed * sin( _LargeWaveDirection ) ) ) , ( DeltaTime29 * ( _LargeWaveSpeed * cos( _LargeWaveDirection ) ) )));
				float2 appendResult22 = (float2(WorldPosition.x , WorldPosition.z));
				float2 WorldSpaceUv23 = appendResult22;
				float3 unpack6 = UnpackNormalScale( tex2D( _LargeWaves, ( ( appendResult55 + WorldSpaceUv23 ) / ( _largeWavesTilling * 0.9 ) ) ), _LargeWavesHeight );
				unpack6.z = lerp( 1, unpack6.z, saturate(_LargeWavesHeight) );
				float3 unpack7 = UnpackNormalScale( tex2D( _LargeWaves, ( ( WorldSpaceUv23 - ( appendResult55 * ( ( _LargeWavesFlow - 0.5 ) * 2.0 ) ) ) / _largeWavesTilling ) ), _LargeWavesHeight );
				unpack7.z = lerp( 1, unpack7.z, saturate(_LargeWavesHeight) );
				float3 lerpResult9 = lerp( unpack6 , unpack7 , 0.5);
				float3 temp_output_92_0 = ( lerpResult9 * 0.1 );
				float2 appendResult80 = (float2(( DeltaTime29 * ( _MediumWaveSpeed * sin( _MediumWaveDirection ) ) ) , ( DeltaTime29 * ( _MediumWaveSpeed * cos( _MediumWaveDirection ) ) )));
				float3 unpack68 = UnpackNormalScale( tex2D( _MediumWaves, ( temp_output_92_0 + float3( ( ( appendResult80 + WorldSpaceUv23 ) / ( _MediumWavesTilling * 0.9 ) ) ,  0.0 ) ).xy ), _MediumWavesHeight );
				unpack68.z = lerp( 1, unpack68.z, saturate(_MediumWavesHeight) );
				float3 unpack69 = UnpackNormalScale( tex2D( _MediumWaves, ( temp_output_92_0 + float3( ( ( WorldSpaceUv23 - ( appendResult80 * ( ( _MediumWavesFlow - 0.5 ) * 2.0 ) ) ) / _MediumWavesTilling ) ,  0.0 ) ).xy ), _MediumWavesHeight );
				unpack69.z = lerp( 1, unpack69.z, saturate(_MediumWavesHeight) );
				float3 lerpResult94 = lerp( unpack68 , unpack69 , 0.5);
				float3 lerpResult95 = lerp( lerpResult9 , lerpResult94 , 0.5);
				float3 temp_output_123_0 = ( 0.1 * lerpResult94 );
				float2 appendResult108 = (float2(( DeltaTime29 * ( _SmallWaveSpeed * sin( _SmallWaveDirection ) ) ) , ( DeltaTime29 * ( _SmallWaveSpeed * cos( _SmallWaveDirection ) ) )));
				float3 unpack120 = UnpackNormalScale( tex2D( _SmallWaves, ( temp_output_123_0 + float3( ( ( appendResult108 + WorldSpaceUv23 ) / ( _SmallWavesTilling * 0.9 ) ) ,  0.0 ) ).xy ), _SmallWavesHeight );
				unpack120.z = lerp( 1, unpack120.z, saturate(_SmallWavesHeight) );
				float3 unpack121 = UnpackNormalScale( tex2D( _SmallWaves, ( temp_output_123_0 + float3( ( ( WorldSpaceUv23 - ( appendResult108 * ( ( _SmallWavesFlow - 0.5 ) * 2.0 ) ) ) / _SmallWavesTilling ) ,  0.0 ) ).xy ), _SmallWavesHeight );
				unpack121.z = lerp( 1, unpack121.z, saturate(_SmallWavesHeight) );
				float3 lerpResult124 = lerp( unpack120 , unpack121 , 0.5);
				float3 lerpResult125 = lerp( lerpResult95 , lerpResult124 , 0.5);
				float3 Normals11 = lerpResult125;
				float4 fetchOpaqueVal1 = float4( SHADERGRAPH_SAMPLE_SCENE_COLOR( ( float3( appendResult3 ,  0.0 ) + ( Normals11 * 0.1 ) ).xy ), 1.0 );
				float4 DistortedTransparentTexture4 = fetchOpaqueVal1;
				float4 temp_output_401_0 = saturate( _MainLightColor );
				float4 Life161 = saturate( ( temp_output_401_0 - _LifeColor ) );
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth137 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth137 = abs( ( screenDepth137 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( 1.0 ) );
				float Density139 = distanceDepth137;
				float4 lerpResult162 = lerp( DistortedTransparentTexture4 , Life161 , saturate( ( pow( Density139 , _LifeDensity ) * _LifeDepth ) ));
				float4 DepthColor154 = saturate( ( temp_output_401_0 - _WaterAbsorbedColor ) );
				float4 lerpResult141 = lerp( lerpResult162 , DepthColor154 , saturate( ( _WaterDepth * pow( Density139 , _WaterDensity ) ) ));
				float4 FinalColored144 = lerpResult141;
				float temp_output_204_0 = ( DeltaTime29 * _FoamSpeed );
				float3 temp_output_181_0 = ( ( Normals11 * 0.1 ) + float3( ( WorldSpaceUv23 / _FoamTilling ) ,  0.0 ) );
				float3 temp_cast_12 = (temp_output_204_0).xxx;
				float lerpResult201 = lerp( tex2D( _foam, ( temp_output_204_0 + temp_output_181_0 ).xy ).r , tex2D( _foam, ( temp_cast_12 - ( temp_output_181_0 * 0.9 ) ).xy ).r , 0.5);
				float FoamTexture230 = lerpResult201;
				float temp_output_360_0 = ( 1.0 - FoamTexture230 );
				float temp_output_285_0 = saturate( Normals11.y );
				float temp_output_394_0 = ( 1.0 - saturate( (0.0 + (temp_output_360_0 - ( 1.0 - ( ( temp_output_285_0 / 0.3 ) * _WaveFoam ) )) * (1.0 - 0.0) / (1.0 - ( 1.0 - ( ( temp_output_285_0 / 0.3 ) * _WaveFoam ) ))) ) );
				float lerpResult395 = lerp( ( temp_output_394_0 / 3.0 ) , saturate( (0.0 + (temp_output_360_0 - ( ( 1.0 - Density139 ) * _FoamDistance )) * (1.0 - 0.0) / (_FoamDistance - ( ( 1.0 - Density139 ) * _FoamDistance ))) ) , temp_output_394_0);
				float clampResult358 = clamp( lerpResult395 , 0.0 , 1.0 );
				float Foam177 = pow( saturate( clampResult358 ) , _FoamSoftness );
				float4 lerpResult175 = lerp( _FoamColor , FinalColored144 , saturate( Foam177 ));
				
				float screenDepth128 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth128 = abs( ( screenDepth128 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _EdgeBlending ) );
				float Edgeblending130 = distanceDepth128;
				
				
				float3 Albedo = lerpResult175.rgb;
				float Alpha = saturate( Edgeblending130 );
				float AlphaClipThreshold = 0.5;

				half4 color = half4( Albedo, Alpha );

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				return color;
			}
			ENDHLSL
		}
		
	}
	/*ase_lod*/
	CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18900
-1680;19;1680;983;3536.82;1475.511;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;127;-5266.77,-541.8333;Inherit;False;4472.592;1895.67;Normals;88;41;47;49;40;60;50;51;32;53;61;52;89;87;55;62;88;86;59;24;65;81;84;85;82;58;57;79;83;66;19;63;8;98;80;64;78;6;77;75;99;100;7;10;101;104;9;102;93;103;74;73;76;92;106;105;107;71;70;72;96;108;97;67;109;110;68;69;111;94;126;113;112;114;115;123;116;119;117;118;122;120;121;124;95;125;11;33;29;;0.6180778,0.4510947,0.8773585,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;33;-4678.818,-418.2551;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-5216.77,-146.2248;Inherit;False;Property;_LargeWaveDirection;Large Wave Direction;10;0;Create;True;0;0;0;False;0;False;1.56;0;0;6.24;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-5144.203,-214.2249;Inherit;False;Property;_LargeWaveSpeed;Large Wave Speed;7;0;Create;True;0;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;49;-4905.482,-132.6225;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;47;-4905.482,-332.6225;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;29;-4461.39,-423.6026;Inherit;False;DeltaTime;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;25;-1995.527,-1192.074;Inherit;False;595;233;World Uv;3;21;22;23;;0.6995144,0.745283,0.4745906,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-4729.482,-223.6225;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;32;-4897.104,-408.5278;Inherit;False;29;DeltaTime;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-4728.482,-135.6225;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-4612.165,79.70903;Inherit;False;Property;_LargeWavesFlow;Large Waves Flow;13;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;21;-1945.526,-1142.075;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-4560.482,-203.6225;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;22;-1759.527,-1112.075;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-4626.676,487.3451;Inherit;False;Property;_MediumWaveDirection;Medium Wave Direction;11;0;Create;True;0;0;0;False;0;False;1.56;0;0;6.24;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;61;-4284.481,66.41356;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-4545.482,-115.6225;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-4128.48,14.41354;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;23;-1624.526,-1117.075;Inherit;False;WorldSpaceUv;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;55;-4411.482,-163.6225;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;88;-4554.11,419.3449;Inherit;False;Property;_MediumWaveSpeed;Medium Wave Speed;9;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;87;-4315.39,500.9476;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;86;-4315.39,300.9473;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-4139.39,409.9473;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-4022.074,713.2789;Inherit;False;Property;_MediumWavesFlow;Medium Waves Flow;14;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-3766.48,-101.5865;Inherit;False;Property;_largeWavesTilling;large Waves Tilling;16;0;Create;True;0;0;0;False;0;False;1;342;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;-4109.602,-229.5077;Inherit;True;23;WorldSpaceUv;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;-4138.39,497.9476;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-3985.565,-34.69077;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;58;-3788.167,-13.89106;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-3955.391,517.9477;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;57;-3793.391,-209.2259;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-3970.391,429.9472;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-3558.48,-207.5865;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;79;-3694.39,699.984;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;-3538.39,647.984;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;63;-3347.48,-313.5865;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;80;-3821.391,469.9474;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-3920.434,1006.198;Inherit;False;Property;_SmallWaveDirection;Small Wave Direction;12;0;Create;True;0;0;0;False;0;False;1.56;0;0;6.24;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;8;-4200.242,-491.8333;Inherit;True;Property;_LargeWaves;Large Waves;0;0;Create;True;0;0;0;False;0;False;None;aa6e4cd27490758438b9d978a6b2d7e8;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleDivideOpNode;64;-3339.48,-131.5865;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-3414.815,10.62195;Inherit;False;Property;_LargeWavesHeight;Large Waves Height;4;0;Create;True;0;0;0;False;0;False;0.4170583;0.767952;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-2976.241,-15.83338;Inherit;False;Constant;_05;0.5;1;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-3121.596,-379.0031;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;101;-3847.868,938.1979;Inherit;False;Property;_SmallWaveSpeed;Small Wave Speed;8;0;Create;True;0;0;0;False;0;False;1;0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-3176.39,531.9838;Inherit;False;Property;_MediumWavesTilling;Medium Waves Tilling;18;0;Create;True;0;0;0;False;0;False;1;44.64;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;99;-3609.148,1019.8;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-3120.241,-197.8333;Inherit;True;Property;_TextureSample1;Texture Sample 1;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;100;-3609.148,819.7999;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-3395.475,598.8795;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;9;-2723.241,-273.8334;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;-3432.149,1016.8;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;-3433.149,928.7999;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;104;-3315.833,1232.132;Inherit;False;Property;_SmallWavesFlow;Small Waves Flow;15;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;74;-3201.613,419.2795;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;76;-3198.077,619.679;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-2968.39,425.9832;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;93;-2912.904,61.8276;Inherit;False;Constant;_Float1;Float 1;13;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;70;-2757.39,319.9832;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;105;-2988.149,1218.837;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;106;-3249.15,1036.8;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-2587.417,-168.1081;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;71;-2746.39,471.9836;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;-3264.15,948.7999;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;96;-2595.768,77.27623;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-2848.725,644.192;Inherit;False;Property;_MediumWavesHeight;Medium Waves Height;5;0;Create;True;0;0;0;False;0;False;0.4825;0.8025;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-2832.149,1166.837;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;97;-2585.527,340.5994;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;108;-3115.15,988.7998;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;67;-3143.35,179.4102;Inherit;True;Property;_MediumWaves;Medium Waves;2;0;Create;True;0;0;0;False;0;False;None;aa6e4cd27490758438b9d978a6b2d7e8;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;69;-2435.079,258.6393;Inherit;True;Property;_TextureSample3;Texture Sample 3;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;68;-2436.434,77.46965;Inherit;True;Property;_TextureSample2;Texture Sample 2;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;110;-2689.234,1117.732;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;111;-2470.151,1050.836;Inherit;False;Property;_SmallWavesTilling;Small Waves Tilling;17;0;Create;True;0;0;0;False;0;False;1;9.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;-2262.156,944.8359;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;94;-1725.671,-63.52977;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-2280.727,437.3374;Inherit;False;Constant;_Float2;Float 2;13;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;113;-2491.838,1138.532;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;112;-2495.374,938.1319;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;123;-2087.272,362.5418;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0.1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;115;-1994.158,671.8359;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;116;-2040.158,990.8358;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;117;-1938.295,868.452;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;122;-1901.569,417.4417;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;118;-2142.493,1163.045;Inherit;False;Property;_SmallWavesHeight;Small Waves Height;6;0;Create;True;0;0;0;False;0;False;0.4328485;0.4948568;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;119;-2437.111,698.2629;Inherit;True;Property;_SmallWaves;Small Waves;1;0;Create;True;0;0;0;False;0;False;None;e1ae957667afd214fbc2678819bcc6b9;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;121;-1741.281,476.9959;Inherit;True;Property;_TextureSample5;Texture Sample 5;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;120;-1742.636,295.8261;Inherit;True;Property;_TextureSample4;Texture Sample 4;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;124;-1398.085,395.8673;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;95;-1550.671,-60.52976;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;125;-1211.62,395.2402;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;11;-1018.177,382.4644;Inherit;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;277;-6697.965,-1264.219;Inherit;False;1650.399;717.2432;Foam Texture;18;230;201;190;180;205;199;196;178;200;198;204;202;203;181;182;183;191;179;;0.5716028,0.9433962,0.5651478,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;179;-6647.965,-844.0467;Inherit;False;23;WorldSpaceUv;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;191;-6414.166,-983.837;Inherit;False;Constant;_Float3;Float 3;30;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;180;-6448.965,-1114.045;Inherit;False;11;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;183;-6540.449,-757.074;Inherit;False;Property;_FoamTilling;Foam  Tilling;27;0;Create;True;0;0;0;False;0;False;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;190;-6231.166,-1042.837;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;182;-6266.449,-889.074;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;205;-6324.749,-1175.219;Inherit;False;Property;_FoamSpeed;Foam Speed;31;0;Create;True;0;0;0;False;0;False;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;203;-6160.966,-749.5419;Inherit;False;Constant;_Float4;Float 4;30;0;Create;True;0;0;0;False;0;False;0.9;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;199;-6493.749,-1214.219;Inherit;False;29;DeltaTime;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;181;-6084.965,-944.0467;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;204;-6126.966,-1198.54;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;202;-5987.966,-817.5419;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;198;-5866.966,-931.5419;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;200;-5852.966,-762.5419;Inherit;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;282;-5826.092,-1723.684;Inherit;False;11;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;178;-5704.876,-947.1312;Inherit;True;Property;_foam;foam;26;0;Create;True;0;0;0;False;0;False;-1;None;173dc16bde300614cbb14a634af2b947;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;196;-5703.987,-767.8753;Inherit;True;Property;_TextureSample6;Texture Sample 6;26;0;Create;True;0;0;0;False;0;False;-1;None;b21c81c63c65f534294098248e4aae99;True;0;False;white;Auto;False;Instance;178;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;283;-5572.092,-1736.684;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SaturateNode;285;-5390.198,-1758.783;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;201;-5395.966,-839.5419;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;230;-5248.512,-736.3623;Inherit;False;FoamTexture;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;387;-5166.646,-1763.959;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;295;-5533.508,-1476.787;Inherit;False;Property;_WaveFoam;Wave Foam;30;0;Create;True;0;0;0;False;0;False;0.5411765;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;137;-1334.624,1493.613;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;279;-5048.643,-1128.282;Inherit;False;230;FoamTexture;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;139;-1043.624,1507.613;Inherit;False;Density;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;389;-4955.692,-1596.097;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;360;-4797.103,-1137.003;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;193;-4777.12,-900.922;Inherit;False;139;Density;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;393;-4750.169,-1638.927;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;185;-4715.968,-1026.416;Inherit;False;Property;_FoamDistance;Foam Distance;29;0;Create;True;0;0;0;False;0;False;0.5411765;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;26;-2000.102,-958.4152;Inherit;False;1203.649;411.0117;Transparent Texture;8;17;2;12;16;3;15;1;4;;0.3956924,0.5347803,0.7169812,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;194;-4533.441,-907.6077;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;391;-4598.556,-1565.676;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;171;-2911.454,-1231.177;Inherit;False;909.396;686.0884;colors;11;158;157;151;159;150;152;160;161;156;154;401;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;351;-4362,-897;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;2;-1950.102,-908.4152;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LightColorNode;158;-2884.722,-1009.286;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;12;-1903.221,-726.4841;Inherit;False;11;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;392;-4337.169,-1555.927;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1877.561,-663.4034;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;3;-1703.102,-877.4152;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-1664.561,-760.4034;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;401;-2722.82,-964.5115;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;157;-2853.138,-1181.177;Inherit;False;Property;_LifeColor;Life Color;24;0;Create;True;0;0;0;False;0;False;1,0.7783019,0.9634851,0;0.8901961,0,0.8168629,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;354;-4015,-952;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;394;-4218.301,-1555.757;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;278;-2001.427,-1659.2;Inherit;False;2298.421;542.7161;color processing;17;144;141;145;162;155;168;163;164;148;167;149;147;170;138;166;143;169;;0.9144705,0.6618013,0.9811321,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;396;-3913.832,-1537.93;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;356;-3758,-946;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;150;-2861.454,-867.089;Inherit;False;Property;_WaterAbsorbedColor;Water Absorbed Color;25;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0.9960784,0.9921568,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;169;-1949.941,-1545.107;Inherit;False;Property;_LifeDensity;Life Density;20;0;Create;True;0;0;0;False;0;False;0.4588235;0.382;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;143;-1813.01,-1319.525;Inherit;False;139;Density;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;159;-2612.722,-1085.285;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;15;-1506.68,-858.1772;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;170;-1951.428,-1609.2;Inherit;False;Property;_LifeDepth;Life Depth;23;0;Create;True;0;0;0;False;0;False;0.4588235;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;138;-1948.996,-1409.391;Inherit;False;Property;_WaterDensity;Water Density;21;0;Create;True;0;0;0;False;0;False;0.4588235;0.1;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;166;-1625.689,-1554.154;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;152;-2567.454,-785.089;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;160;-2465.722,-1084.285;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;395;-3699.832,-1365.93;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;1;-1306.32,-856.9487;Inherit;False;Global;_GrabScreen0;Grab Screen 0;0;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;167;-1489.689,-1451.154;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;358;-3461.754,-1315.864;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;4;-1097.454,-853.3546;Inherit;False;DistortedTransparentTexture;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;161;-2283.722,-1094.285;Inherit;False;Life;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;149;-1950.483,-1473.484;Inherit;False;Property;_WaterDepth;Water Depth;22;0;Create;True;0;0;0;False;0;False;0.4588235;0.795;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;147;-1630.487,-1335.484;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;156;-2379.058,-780.9021;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;-1481.487,-1333.484;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;359;-3299.8,-1080.804;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;370;-3607.894,-823.3728;Inherit;False;Property;_FoamSoftness;Foam Softness;34;0;Create;True;0;0;0;False;0;False;0.0001;0.143;0.0001;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;154;-2226.058,-778.9021;Inherit;False;DepthColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;163;-1392.98,-1572.721;Inherit;False;4;DistortedTransparentTexture;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;168;-1307.689,-1408.154;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;164;-1376.98,-1507.721;Inherit;False;161;Life;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;145;-1312.872,-1338.903;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;136;-1606.334,1397.119;Inherit;False;Property;_EdgeBlending;Edge Blending;19;0;Create;True;0;0;0;False;0;False;0.4588235;0.529;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;162;-1097.98,-1480.721;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;155;-845.1736,-1488.369;Inherit;False;154;DepthColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;369;-3208.693,-999.5728;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;128;-1344.927,1390.088;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;177;-3083.833,-1022.268;Inherit;False;Foam;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;141;-287.011,-1461.525;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;130;-1022.199,1385.271;Inherit;False;Edgeblending;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;173;-802.8633,-129.4009;Inherit;True;177;Foam;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;144;-64.01099,-1399.525;Inherit;False;FinalColored;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;329;-518.3701,-117.9603;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;5;-561.7976,-31.74023;Inherit;False;144;FinalColored;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;176;-772.6277,-385.3215;Inherit;False;Property;_FoamColor;Foam Color;28;0;Create;True;0;0;0;False;0;False;1,1,1,0;0.8306337,0.9546936,0.9622641,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;132;-324.0203,280.7552;Inherit;False;130;Edgeblending;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;259;-3077.805,1496.636;Inherit;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;244;-2032.583,1433.209;Inherit;False;UnderwaterScat;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;304;-214.7467,405.316;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;287;-318.2878,-204.0663;Inherit;False;286;Height;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;242;-3259.583,1433.209;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;134;-142.0587,272.5428;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;251;-3764.189,1744.848;Inherit;True;11;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;371;-376.2276,403.2052;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;239;-3539.583,1393.209;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;327;-339.9933,579.1609;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;264;-422.9147,70.49651;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;246;-2700.189,1457.848;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;248;-2455.189,1453.848;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;249;-2674.189,1583.848;Inherit;False;Property;_ScatteringScale;Scattering Scale;33;0;Create;True;0;0;0;False;0;False;0;2.136;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;258;-919.7888,56.53191;Inherit;False;161;Life;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;305;-800.6553,577.4326;Inherit;False;286;Height;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;286;-5141.395,-1920.885;Inherit;False;Height;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;324;-645.0282,581.8848;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;241;-3412.583,1553.209;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;175;-269.6277,-132.3215;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;263;-188.9147,68.49646;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;247;-2881.189,1581.848;Inherit;False;Property;_Scatteringpower;Scattering power;32;0;Create;True;0;0;0;False;0;False;0;14.61024;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;245;-917.5261,226.7584;Inherit;False;244;UnderwaterScat;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;265;-396.9147,135.4965;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;257;-617.7888,140.5319;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;266;-711.9147,288.4965;Inherit;False;Property;_Scatteringbrightness;Scattering brightness;35;0;Create;True;0;0;0;False;0;False;0.3;0.14;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;151;-2807.454,-704.089;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;328;-475.9933,595.1609;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.05;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;20;-292.5447,-0.007392883;Inherit;False;11;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;243;-2185.583,1441.209;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;240;-3775.583,1611.209;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;18;-733.4706,396.9374;Inherit;False;Property;_WaterSmoothness;Water Smoothness;3;0;Create;True;0;0;0;False;0;False;0.99;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;236;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;237;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;233;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;235;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;234;0,0;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;Vwater;94348b07e5e8bab40bd6c8a1e3df54cd;True;Forward;0;1;Forward;18;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;0;False;True;1;5;False;-1;10;False;-1;1;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;0;Hidden/InternalErrorShader;0;0;Standard;38;Workflow;1;Surface;1;  Refraction Model;0;  Blend;0;Two Sided;1;Fragment Normal Space,InvertActionOnDeselection;0;Transmission;0;  Transmission Shadow;0.5,False,-1;Translucency;0;  Translucency Strength;1,False,-1;  Normal Distortion;0.5,False,-1;  Scattering;2,False,-1;  Direct;0.9,False,-1;  Ambient;0.1,False,-1;  Shadow;0.5,False,-1;Cast Shadows;1;  Use Shadow Threshold;0;Receive Shadows;1;GPU Instancing;1;LOD CrossFade;1;Built-in Fog;1;_FinalColorxAlpha;0;Meta Pass;1;Override Baked GI;0;Extra Pre Pass;0;DOTS Instancing;0;Tessellation;0;  Phong;0;  Strength;0.5,False,-1;  Type;0;  Tess;16,False,-1;  Min;10,False,-1;  Max;25,False,-1;  Edge Length;16,False,-1;  Max Displacement;25,False,-1;Write Depth;0;  Early Z;0;Vertex Position,InvertActionOnDeselection;1;0;6;False;True;True;True;True;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;238;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;True;1;5;False;-1;10;False;-1;1;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=Universal2D;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;49;0;41;0
WireConnection;47;0;41;0
WireConnection;29;0;33;0
WireConnection;50;0;40;0
WireConnection;50;1;47;0
WireConnection;51;0;40;0
WireConnection;51;1;49;0
WireConnection;52;0;32;0
WireConnection;52;1;50;0
WireConnection;22;0;21;1
WireConnection;22;1;21;3
WireConnection;61;0;60;0
WireConnection;53;0;32;0
WireConnection;53;1;51;0
WireConnection;62;0;61;0
WireConnection;23;0;22;0
WireConnection;55;0;52;0
WireConnection;55;1;53;0
WireConnection;87;0;89;0
WireConnection;86;0;89;0
WireConnection;84;0;88;0
WireConnection;84;1;86;0
WireConnection;85;0;88;0
WireConnection;85;1;87;0
WireConnection;59;0;55;0
WireConnection;59;1;62;0
WireConnection;58;0;24;0
WireConnection;58;1;59;0
WireConnection;82;0;32;0
WireConnection;82;1;85;0
WireConnection;57;0;55;0
WireConnection;57;1;24;0
WireConnection;83;0;32;0
WireConnection;83;1;84;0
WireConnection;66;0;65;0
WireConnection;79;0;81;0
WireConnection;78;0;79;0
WireConnection;63;0;57;0
WireConnection;63;1;66;0
WireConnection;80;0;83;0
WireConnection;80;1;82;0
WireConnection;64;0;58;0
WireConnection;64;1;65;0
WireConnection;6;0;8;0
WireConnection;6;1;63;0
WireConnection;6;5;19;0
WireConnection;99;0;98;0
WireConnection;7;0;8;0
WireConnection;7;1;64;0
WireConnection;7;5;19;0
WireConnection;100;0;98;0
WireConnection;77;0;80;0
WireConnection;77;1;78;0
WireConnection;9;0;6;0
WireConnection;9;1;7;0
WireConnection;9;2;10;0
WireConnection;103;0;101;0
WireConnection;103;1;99;0
WireConnection;102;0;101;0
WireConnection;102;1;100;0
WireConnection;74;0;80;0
WireConnection;74;1;24;0
WireConnection;76;0;24;0
WireConnection;76;1;77;0
WireConnection;73;0;75;0
WireConnection;70;0;74;0
WireConnection;70;1;73;0
WireConnection;105;0;104;0
WireConnection;106;0;32;0
WireConnection;106;1;103;0
WireConnection;92;0;9;0
WireConnection;92;1;93;0
WireConnection;71;0;76;0
WireConnection;71;1;75;0
WireConnection;107;0;32;0
WireConnection;107;1;102;0
WireConnection;96;0;92;0
WireConnection;96;1;70;0
WireConnection;109;0;105;0
WireConnection;97;0;92;0
WireConnection;97;1;71;0
WireConnection;108;0;107;0
WireConnection;108;1;106;0
WireConnection;69;0;67;0
WireConnection;69;1;97;0
WireConnection;69;5;72;0
WireConnection;68;0;67;0
WireConnection;68;1;96;0
WireConnection;68;5;72;0
WireConnection;110;0;108;0
WireConnection;110;1;109;0
WireConnection;114;0;111;0
WireConnection;94;0;68;0
WireConnection;94;1;69;0
WireConnection;94;2;10;0
WireConnection;113;0;24;0
WireConnection;113;1;110;0
WireConnection;112;0;108;0
WireConnection;112;1;24;0
WireConnection;123;0;126;0
WireConnection;123;1;94;0
WireConnection;115;0;112;0
WireConnection;115;1;114;0
WireConnection;116;0;113;0
WireConnection;116;1;111;0
WireConnection;117;0;123;0
WireConnection;117;1;116;0
WireConnection;122;0;123;0
WireConnection;122;1;115;0
WireConnection;121;0;119;0
WireConnection;121;1;117;0
WireConnection;121;5;118;0
WireConnection;120;0;119;0
WireConnection;120;1;122;0
WireConnection;120;5;118;0
WireConnection;124;0;120;0
WireConnection;124;1;121;0
WireConnection;124;2;10;0
WireConnection;95;0;9;0
WireConnection;95;1;94;0
WireConnection;95;2;10;0
WireConnection;125;0;95;0
WireConnection;125;1;124;0
WireConnection;125;2;10;0
WireConnection;11;0;125;0
WireConnection;190;0;180;0
WireConnection;190;1;191;0
WireConnection;182;0;179;0
WireConnection;182;1;183;0
WireConnection;181;0;190;0
WireConnection;181;1;182;0
WireConnection;204;0;199;0
WireConnection;204;1;205;0
WireConnection;202;0;181;0
WireConnection;202;1;203;0
WireConnection;198;0;204;0
WireConnection;198;1;181;0
WireConnection;200;0;204;0
WireConnection;200;1;202;0
WireConnection;178;1;198;0
WireConnection;196;1;200;0
WireConnection;283;0;282;0
WireConnection;285;0;283;1
WireConnection;201;0;178;1
WireConnection;201;1;196;1
WireConnection;230;0;201;0
WireConnection;387;0;285;0
WireConnection;139;0;137;0
WireConnection;389;0;387;0
WireConnection;389;1;295;0
WireConnection;360;0;279;0
WireConnection;393;0;389;0
WireConnection;194;0;193;0
WireConnection;391;0;360;0
WireConnection;391;1;393;0
WireConnection;351;0;194;0
WireConnection;351;1;185;0
WireConnection;392;0;391;0
WireConnection;3;0;2;1
WireConnection;3;1;2;2
WireConnection;16;0;12;0
WireConnection;16;1;17;0
WireConnection;401;0;158;0
WireConnection;354;0;360;0
WireConnection;354;1;351;0
WireConnection;354;2;185;0
WireConnection;394;0;392;0
WireConnection;396;0;394;0
WireConnection;356;0;354;0
WireConnection;159;0;401;0
WireConnection;159;1;157;0
WireConnection;15;0;3;0
WireConnection;15;1;16;0
WireConnection;166;0;143;0
WireConnection;166;1;169;0
WireConnection;152;0;401;0
WireConnection;152;1;150;0
WireConnection;160;0;159;0
WireConnection;395;0;396;0
WireConnection;395;1;356;0
WireConnection;395;2;394;0
WireConnection;1;0;15;0
WireConnection;167;0;166;0
WireConnection;167;1;170;0
WireConnection;358;0;395;0
WireConnection;4;0;1;0
WireConnection;161;0;160;0
WireConnection;147;0;143;0
WireConnection;147;1;138;0
WireConnection;156;0;152;0
WireConnection;148;0;149;0
WireConnection;148;1;147;0
WireConnection;359;0;358;0
WireConnection;154;0;156;0
WireConnection;168;0;167;0
WireConnection;145;0;148;0
WireConnection;162;0;163;0
WireConnection;162;1;164;0
WireConnection;162;2;168;0
WireConnection;369;0;359;0
WireConnection;369;1;370;0
WireConnection;128;0;136;0
WireConnection;177;0;369;0
WireConnection;141;0;162;0
WireConnection;141;1;155;0
WireConnection;141;2;145;0
WireConnection;130;0;128;0
WireConnection;144;0;141;0
WireConnection;329;0;173;0
WireConnection;259;0;242;0
WireConnection;259;1;251;0
WireConnection;244;0;243;0
WireConnection;304;0;371;0
WireConnection;304;1;327;0
WireConnection;242;0;239;0
WireConnection;242;1;241;0
WireConnection;134;0;132;0
WireConnection;371;0;18;0
WireConnection;371;1;173;0
WireConnection;327;0;328;0
WireConnection;246;0;259;0
WireConnection;246;1;247;0
WireConnection;248;0;246;0
WireConnection;248;1;249;0
WireConnection;286;0;285;0
WireConnection;324;0;305;0
WireConnection;241;0;240;0
WireConnection;175;0;176;0
WireConnection;175;1;5;0
WireConnection;175;2;329;0
WireConnection;263;0;264;0
WireConnection;263;1;265;0
WireConnection;265;0;257;0
WireConnection;265;1;266;0
WireConnection;257;0;258;0
WireConnection;257;1;245;0
WireConnection;328;0;324;0
WireConnection;243;0;248;0
WireConnection;234;0;175;0
WireConnection;234;1;20;0
WireConnection;234;2;263;0
WireConnection;234;4;304;0
WireConnection;234;6;134;0
ASEEND*/
//CHKSM=A831FD8A0F66A84C18E209970981EFF86C94E62B