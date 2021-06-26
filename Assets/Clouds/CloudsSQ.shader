// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CloudsSQ"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin]_Main("Main", 2D) = "white" {}
		_Detail("Detail", 2D) = "white" {}
		_WindDirection("Wind Direction", Range( 0 , 6.283185)) = 0
		_Weathervariationspeed("Weather variation speed", Float) = 1
		_Weathertilling("Weather tilling", Float) = 1
		_MainShapeSpeed("Main Shape Speed", Float) = 1
		_DetailShapeSpeed("Detail Shape Speed", Float) = 1
		_MainShapeTilling("Main Shape Tilling", Float) = 1
		_DetailShapeTilling("Detail Shape Tilling", Float) = 1
		_Detailsweight("Details weight", Range( 0 , 1)) = 0
		_WeatherSize("Weather Size", Range( 0.001 , 1)) = 0
		_CloudSize("Cloud Size", Range( 0.001 , 1)) = 0
		_CloudSoftness("Cloud Softness", Range( 0.001 , 3)) = 0
		_WeatherSoftness("Weather Softness", Range( 0.001 , 3)) = 0
		_midYvalue("midYvalue", Float) = 0
		_cloudHeight("cloudHeight", Float) = 0
		_CloudRoundness("Cloud Roundness", Float) = 0
		_SSSDiffusion("SSS Diffusion", Range( 0 , 100)) = 0
		_FogSoftness1("FogSoftness", Range( 0.001 , 3)) = 0
		_SSSbrightness("SSS brightness", Range( 0 , 1)) = 0
		_CloudDensity("Cloud Density ", Range( 0.5 , 10)) = 0
		_WeatherMask("Weather Mask", 2D) = "white" {}
		_ClippingSmoothness("Clipping Smoothness", Range( 0 , 5)) = 0
		_FogGradient1("Fog Gradient", 2D) = "white" {}
		[ASEEnd]_FogDistance1("Fog Distance", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

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
			ZWrite Off
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#define _EMISSION
			#define ASE_SRP_VERSION 80301

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

			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#define ASE_NEEDS_FRAG_SHADOWCOORDS
			#define ASE_NEEDS_VERT_POSITION


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
				float4 ase_texcoord7 : TEXCOORD7;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _FogGradient1_ST;
			float _midYvalue;
			float _CloudSize;
			float _WeatherSoftness;
			float _WeatherSize;
			float _Weathertilling;
			float _Weathervariationspeed;
			float _Detailsweight;
			float _DetailShapeTilling;
			float _DetailShapeSpeed;
			float _MainShapeTilling;
			float _MainShapeSpeed;
			float _WindDirection;
			float _FogSoftness1;
			float _FogDistance1;
			float _SSSbrightness;
			float _SSSDiffusion;
			float _CloudDensity;
			float _CloudRoundness;
			float _cloudHeight;
			float _CloudSoftness;
			float _ClippingSmoothness;
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
			sampler2D _FogGradient1;
			sampler2D _Main;
			sampler2D _Detail;
			sampler2D _WeatherMask;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 objectToViewPos = TransformWorldToView(TransformObjectToWorld(v.vertex.xyz));
				float eyeDepth = -objectToViewPos.z;
				o.ase_texcoord7.z = eyeDepth;
				
				o.ase_texcoord7.xy = v.texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord7.w = 0;
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

				float Taper144 = ( 1.0 - pow( saturate( ( abs( ( _midYvalue - WorldPosition.y ) ) / ( _cloudHeight * 0.25 ) ) ) , _CloudRoundness ) );
				float temp_output_163_0 = saturate( pow( Taper144 , _CloudDensity ) );
				float Density169 = temp_output_163_0;
				float Light184 = ( 1.0 - pow( saturate( ( ( _midYvalue - WorldPosition.y ) / ( _cloudHeight * 0.25 ) ) ) , _CloudRoundness ) );
				float LightGradient270 = saturate( pow( Light184 , 300.0 ) );
				float lerpResult188 = lerp( Density169 , (float)1 , LightGradient270);
				float lerpResult269 = lerp( lerpResult188 , LightGradient270 , LightGradient270);
				float3 temp_cast_1 = (lerpResult269).xxx;
				
				float dotResult155 = dot( WorldViewDirection , -_MainLightPosition.xyz );
				float SSS167 = ( temp_output_163_0 * ( pow( saturate( dotResult155 ) , _SSSDiffusion ) * _SSSbrightness ) );
				float clampResult278 = clamp( SSS167 , 0.0 , 1.0 );
				float ase_lightAtten = 0;
				Light ase_lightAtten_mainLight = GetMainLight( ShadowCoords );
				ase_lightAtten = ase_lightAtten_mainLight.distanceAttenuation * ase_lightAtten_mainLight.shadowAttenuation;
				float2 uv_FogGradient1 = IN.ase_texcoord7.xy * _FogGradient1_ST.xy + _FogGradient1_ST.zw;
				float Fog290 = pow( saturate( (0.0 + (tex2D( _FogGradient1, uv_FogGradient1 ).r - _FogDistance1) * (1.0 - 0.0) / (1.0 - _FogDistance1)) ) , _FogSoftness1 );
				float4 lerpResult297 = lerp( ( clampResult278 * _MainLightColor * ase_lightAtten ) , unity_FogColor , Fog290);
				
				float2 appendResult2 = (float2(WorldPosition.x , WorldPosition.z));
				float2 UV3 = appendResult2;
				float2 appendResult17 = (float2(( _TimeParameters.x * cos( _WindDirection ) ) , ( _TimeParameters.x * sin( _WindDirection ) )));
				float2 Time23 = appendResult17;
				float2 temp_output_34_0 = ( ( UV3 + ( Time23 * _MainShapeSpeed ) ) / _MainShapeTilling );
				float2 temp_output_35_0 = ( ( UV3 - ( Time23 * ( _MainShapeSpeed * 0.8 ) ) ) / ( _MainShapeTilling * 0.9 ) );
				float lerpResult132 = lerp( tex2D( _Main, temp_output_34_0 ).r , tex2D( _Main, temp_output_35_0 ).r , 0.5);
				float MainShapeBlend42 = lerpResult132;
				float2 temp_output_84_0 = ( ( UV3 + ( Time23 * _DetailShapeSpeed ) ) / _DetailShapeTilling );
				float2 temp_output_78_0 = ( ( UV3 - ( Time23 * ( _DetailShapeSpeed * 0.8 ) ) ) / ( _DetailShapeTilling * 0.9 ) );
				float lerpResult130 = lerp( tex2D( _Detail, temp_output_84_0 ).r , tex2D( _Detail, temp_output_78_0 ).r , 0.5);
				float DetailShapeBlend81 = lerpResult130;
				float lerpResult114 = lerp( MainShapeBlend42 , DetailShapeBlend81 , _Detailsweight);
				float Blendedshapes120 = lerpResult114;
				float lerpResult210 = lerp( tex2D( _WeatherMask, ( ( UV3 + ( Time23 * ( _Weathervariationspeed * 0.8 ) ) ) / _Weathertilling ) ).r , tex2D( _WeatherMask, ( ( UV3 - ( Time23 * _Weathervariationspeed ) ) / ( _Weathertilling * 0.8 ) ) ).r , 0.5);
				float weather211 = pow( saturate( (0.0 + (lerpResult210 - _WeatherSize) * (1.0 - 0.0) / (1.0 - _WeatherSize)) ) , _WeatherSoftness );
				float FinalShape128 = pow( saturate( (0.0 + (( Blendedshapes120 * weather211 * Taper144 ) - _CloudSize) * (1.0 - 0.0) / (1.0 - _CloudSize)) ) , _CloudSoftness );
				float eyeDepth = IN.ase_texcoord7.z;
				float cameraDepthFade283 = (( eyeDepth -_ProjectionParams.y - 0.0 ) / _ClippingSmoothness);
				
				float3 Albedo = temp_cast_1;
				float3 Normal = float3(0, 0, 1);
				float3 Emission = lerpResult297.rgb;
				float3 Specular = 0.5;
				float Metallic = 0;
				float Smoothness = 0.4;
				float Occlusion = 1;
				float Alpha = ( FinalShape128 * saturate( cameraDepthFade283 ) );
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
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#define _EMISSION
			#define ASE_SRP_VERSION 80301

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_POSITION


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
			float4 _FogGradient1_ST;
			float _midYvalue;
			float _CloudSize;
			float _WeatherSoftness;
			float _WeatherSize;
			float _Weathertilling;
			float _Weathervariationspeed;
			float _Detailsweight;
			float _DetailShapeTilling;
			float _DetailShapeSpeed;
			float _MainShapeTilling;
			float _MainShapeSpeed;
			float _WindDirection;
			float _FogSoftness1;
			float _FogDistance1;
			float _SSSbrightness;
			float _SSSDiffusion;
			float _CloudDensity;
			float _CloudRoundness;
			float _cloudHeight;
			float _CloudSoftness;
			float _ClippingSmoothness;
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
			sampler2D _Main;
			sampler2D _Detail;
			sampler2D _WeatherMask;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 objectToViewPos = TransformWorldToView(TransformObjectToWorld(v.vertex.xyz));
				float eyeDepth = -objectToViewPos.z;
				o.ase_texcoord2.x = eyeDepth;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.yzw = 0;
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

				float2 appendResult2 = (float2(WorldPosition.x , WorldPosition.z));
				float2 UV3 = appendResult2;
				float2 appendResult17 = (float2(( _TimeParameters.x * cos( _WindDirection ) ) , ( _TimeParameters.x * sin( _WindDirection ) )));
				float2 Time23 = appendResult17;
				float2 temp_output_34_0 = ( ( UV3 + ( Time23 * _MainShapeSpeed ) ) / _MainShapeTilling );
				float2 temp_output_35_0 = ( ( UV3 - ( Time23 * ( _MainShapeSpeed * 0.8 ) ) ) / ( _MainShapeTilling * 0.9 ) );
				float lerpResult132 = lerp( tex2D( _Main, temp_output_34_0 ).r , tex2D( _Main, temp_output_35_0 ).r , 0.5);
				float MainShapeBlend42 = lerpResult132;
				float2 temp_output_84_0 = ( ( UV3 + ( Time23 * _DetailShapeSpeed ) ) / _DetailShapeTilling );
				float2 temp_output_78_0 = ( ( UV3 - ( Time23 * ( _DetailShapeSpeed * 0.8 ) ) ) / ( _DetailShapeTilling * 0.9 ) );
				float lerpResult130 = lerp( tex2D( _Detail, temp_output_84_0 ).r , tex2D( _Detail, temp_output_78_0 ).r , 0.5);
				float DetailShapeBlend81 = lerpResult130;
				float lerpResult114 = lerp( MainShapeBlend42 , DetailShapeBlend81 , _Detailsweight);
				float Blendedshapes120 = lerpResult114;
				float lerpResult210 = lerp( tex2D( _WeatherMask, ( ( UV3 + ( Time23 * ( _Weathervariationspeed * 0.8 ) ) ) / _Weathertilling ) ).r , tex2D( _WeatherMask, ( ( UV3 - ( Time23 * _Weathervariationspeed ) ) / ( _Weathertilling * 0.8 ) ) ).r , 0.5);
				float weather211 = pow( saturate( (0.0 + (lerpResult210 - _WeatherSize) * (1.0 - 0.0) / (1.0 - _WeatherSize)) ) , _WeatherSoftness );
				float Taper144 = ( 1.0 - pow( saturate( ( abs( ( _midYvalue - WorldPosition.y ) ) / ( _cloudHeight * 0.25 ) ) ) , _CloudRoundness ) );
				float FinalShape128 = pow( saturate( (0.0 + (( Blendedshapes120 * weather211 * Taper144 ) - _CloudSize) * (1.0 - 0.0) / (1.0 - _CloudSize)) ) , _CloudSoftness );
				float eyeDepth = IN.ase_texcoord2.x;
				float cameraDepthFade283 = (( eyeDepth -_ProjectionParams.y - 0.0 ) / _ClippingSmoothness);
				
				float Alpha = ( FinalShape128 * saturate( cameraDepthFade283 ) );
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
			#define _EMISSION
			#define ASE_SRP_VERSION 80301

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
			#define ASE_NEEDS_VERT_POSITION
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
				float4 ase_texcoord : TEXCOORD0;
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
			float4 _FogGradient1_ST;
			float _midYvalue;
			float _CloudSize;
			float _WeatherSoftness;
			float _WeatherSize;
			float _Weathertilling;
			float _Weathervariationspeed;
			float _Detailsweight;
			float _DetailShapeTilling;
			float _DetailShapeSpeed;
			float _MainShapeTilling;
			float _MainShapeSpeed;
			float _WindDirection;
			float _FogSoftness1;
			float _FogDistance1;
			float _SSSbrightness;
			float _SSSDiffusion;
			float _CloudDensity;
			float _CloudRoundness;
			float _cloudHeight;
			float _CloudSoftness;
			float _ClippingSmoothness;
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
			sampler2D _FogGradient1;
			sampler2D _Main;
			sampler2D _Detail;
			sampler2D _WeatherMask;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 objectToViewPos = TransformWorldToView(TransformObjectToWorld(v.vertex.xyz));
				float eyeDepth = -objectToViewPos.z;
				o.ase_texcoord2.z = eyeDepth;
				
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.w = 0;
				
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
				float4 ase_texcoord : TEXCOORD0;

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
				o.ase_texcoord = v.ase_texcoord;
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
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
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

				float Taper144 = ( 1.0 - pow( saturate( ( abs( ( _midYvalue - WorldPosition.y ) ) / ( _cloudHeight * 0.25 ) ) ) , _CloudRoundness ) );
				float temp_output_163_0 = saturate( pow( Taper144 , _CloudDensity ) );
				float Density169 = temp_output_163_0;
				float Light184 = ( 1.0 - pow( saturate( ( ( _midYvalue - WorldPosition.y ) / ( _cloudHeight * 0.25 ) ) ) , _CloudRoundness ) );
				float LightGradient270 = saturate( pow( Light184 , 300.0 ) );
				float lerpResult188 = lerp( Density169 , (float)1 , LightGradient270);
				float lerpResult269 = lerp( lerpResult188 , LightGradient270 , LightGradient270);
				float3 temp_cast_1 = (lerpResult269).xxx;
				
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float dotResult155 = dot( ase_worldViewDir , -_MainLightPosition.xyz );
				float SSS167 = ( temp_output_163_0 * ( pow( saturate( dotResult155 ) , _SSSDiffusion ) * _SSSbrightness ) );
				float clampResult278 = clamp( SSS167 , 0.0 , 1.0 );
				float ase_lightAtten = 0;
				Light ase_lightAtten_mainLight = GetMainLight( ShadowCoords );
				ase_lightAtten = ase_lightAtten_mainLight.distanceAttenuation * ase_lightAtten_mainLight.shadowAttenuation;
				float2 uv_FogGradient1 = IN.ase_texcoord2.xy * _FogGradient1_ST.xy + _FogGradient1_ST.zw;
				float Fog290 = pow( saturate( (0.0 + (tex2D( _FogGradient1, uv_FogGradient1 ).r - _FogDistance1) * (1.0 - 0.0) / (1.0 - _FogDistance1)) ) , _FogSoftness1 );
				float4 lerpResult297 = lerp( ( clampResult278 * _MainLightColor * ase_lightAtten ) , unity_FogColor , Fog290);
				
				float2 appendResult2 = (float2(WorldPosition.x , WorldPosition.z));
				float2 UV3 = appendResult2;
				float2 appendResult17 = (float2(( _TimeParameters.x * cos( _WindDirection ) ) , ( _TimeParameters.x * sin( _WindDirection ) )));
				float2 Time23 = appendResult17;
				float2 temp_output_34_0 = ( ( UV3 + ( Time23 * _MainShapeSpeed ) ) / _MainShapeTilling );
				float2 temp_output_35_0 = ( ( UV3 - ( Time23 * ( _MainShapeSpeed * 0.8 ) ) ) / ( _MainShapeTilling * 0.9 ) );
				float lerpResult132 = lerp( tex2D( _Main, temp_output_34_0 ).r , tex2D( _Main, temp_output_35_0 ).r , 0.5);
				float MainShapeBlend42 = lerpResult132;
				float2 temp_output_84_0 = ( ( UV3 + ( Time23 * _DetailShapeSpeed ) ) / _DetailShapeTilling );
				float2 temp_output_78_0 = ( ( UV3 - ( Time23 * ( _DetailShapeSpeed * 0.8 ) ) ) / ( _DetailShapeTilling * 0.9 ) );
				float lerpResult130 = lerp( tex2D( _Detail, temp_output_84_0 ).r , tex2D( _Detail, temp_output_78_0 ).r , 0.5);
				float DetailShapeBlend81 = lerpResult130;
				float lerpResult114 = lerp( MainShapeBlend42 , DetailShapeBlend81 , _Detailsweight);
				float Blendedshapes120 = lerpResult114;
				float lerpResult210 = lerp( tex2D( _WeatherMask, ( ( UV3 + ( Time23 * ( _Weathervariationspeed * 0.8 ) ) ) / _Weathertilling ) ).r , tex2D( _WeatherMask, ( ( UV3 - ( Time23 * _Weathervariationspeed ) ) / ( _Weathertilling * 0.8 ) ) ).r , 0.5);
				float weather211 = pow( saturate( (0.0 + (lerpResult210 - _WeatherSize) * (1.0 - 0.0) / (1.0 - _WeatherSize)) ) , _WeatherSoftness );
				float FinalShape128 = pow( saturate( (0.0 + (( Blendedshapes120 * weather211 * Taper144 ) - _CloudSize) * (1.0 - 0.0) / (1.0 - _CloudSize)) ) , _CloudSoftness );
				float eyeDepth = IN.ase_texcoord2.z;
				float cameraDepthFade283 = (( eyeDepth -_ProjectionParams.y - 0.0 ) / _ClippingSmoothness);
				
				
				float3 Albedo = temp_cast_1;
				float3 Emission = lerpResult297.rgb;
				float Alpha = ( FinalShape128 * saturate( cameraDepthFade283 ) );
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
			ZWrite Off
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#define _EMISSION
			#define ASE_SRP_VERSION 80301

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
			#define ASE_NEEDS_VERT_POSITION


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
			float4 _FogGradient1_ST;
			float _midYvalue;
			float _CloudSize;
			float _WeatherSoftness;
			float _WeatherSize;
			float _Weathertilling;
			float _Weathervariationspeed;
			float _Detailsweight;
			float _DetailShapeTilling;
			float _DetailShapeSpeed;
			float _MainShapeTilling;
			float _MainShapeSpeed;
			float _WindDirection;
			float _FogSoftness1;
			float _FogDistance1;
			float _SSSbrightness;
			float _SSSDiffusion;
			float _CloudDensity;
			float _CloudRoundness;
			float _cloudHeight;
			float _CloudSoftness;
			float _ClippingSmoothness;
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
			sampler2D _Main;
			sampler2D _Detail;
			sampler2D _WeatherMask;


			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float3 objectToViewPos = TransformWorldToView(TransformObjectToWorld(v.vertex.xyz));
				float eyeDepth = -objectToViewPos.z;
				o.ase_texcoord2.x = eyeDepth;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.yzw = 0;
				
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

				float Taper144 = ( 1.0 - pow( saturate( ( abs( ( _midYvalue - WorldPosition.y ) ) / ( _cloudHeight * 0.25 ) ) ) , _CloudRoundness ) );
				float temp_output_163_0 = saturate( pow( Taper144 , _CloudDensity ) );
				float Density169 = temp_output_163_0;
				float Light184 = ( 1.0 - pow( saturate( ( ( _midYvalue - WorldPosition.y ) / ( _cloudHeight * 0.25 ) ) ) , _CloudRoundness ) );
				float LightGradient270 = saturate( pow( Light184 , 300.0 ) );
				float lerpResult188 = lerp( Density169 , (float)1 , LightGradient270);
				float lerpResult269 = lerp( lerpResult188 , LightGradient270 , LightGradient270);
				float3 temp_cast_1 = (lerpResult269).xxx;
				
				float2 appendResult2 = (float2(WorldPosition.x , WorldPosition.z));
				float2 UV3 = appendResult2;
				float2 appendResult17 = (float2(( _TimeParameters.x * cos( _WindDirection ) ) , ( _TimeParameters.x * sin( _WindDirection ) )));
				float2 Time23 = appendResult17;
				float2 temp_output_34_0 = ( ( UV3 + ( Time23 * _MainShapeSpeed ) ) / _MainShapeTilling );
				float2 temp_output_35_0 = ( ( UV3 - ( Time23 * ( _MainShapeSpeed * 0.8 ) ) ) / ( _MainShapeTilling * 0.9 ) );
				float lerpResult132 = lerp( tex2D( _Main, temp_output_34_0 ).r , tex2D( _Main, temp_output_35_0 ).r , 0.5);
				float MainShapeBlend42 = lerpResult132;
				float2 temp_output_84_0 = ( ( UV3 + ( Time23 * _DetailShapeSpeed ) ) / _DetailShapeTilling );
				float2 temp_output_78_0 = ( ( UV3 - ( Time23 * ( _DetailShapeSpeed * 0.8 ) ) ) / ( _DetailShapeTilling * 0.9 ) );
				float lerpResult130 = lerp( tex2D( _Detail, temp_output_84_0 ).r , tex2D( _Detail, temp_output_78_0 ).r , 0.5);
				float DetailShapeBlend81 = lerpResult130;
				float lerpResult114 = lerp( MainShapeBlend42 , DetailShapeBlend81 , _Detailsweight);
				float Blendedshapes120 = lerpResult114;
				float lerpResult210 = lerp( tex2D( _WeatherMask, ( ( UV3 + ( Time23 * ( _Weathervariationspeed * 0.8 ) ) ) / _Weathertilling ) ).r , tex2D( _WeatherMask, ( ( UV3 - ( Time23 * _Weathervariationspeed ) ) / ( _Weathertilling * 0.8 ) ) ).r , 0.5);
				float weather211 = pow( saturate( (0.0 + (lerpResult210 - _WeatherSize) * (1.0 - 0.0) / (1.0 - _WeatherSize)) ) , _WeatherSoftness );
				float FinalShape128 = pow( saturate( (0.0 + (( Blendedshapes120 * weather211 * Taper144 ) - _CloudSize) * (1.0 - 0.0) / (1.0 - _CloudSize)) ) , _CloudSoftness );
				float eyeDepth = IN.ase_texcoord2.x;
				float cameraDepthFade283 = (( eyeDepth -_ProjectionParams.y - 0.0 ) / _ClippingSmoothness);
				
				
				float3 Albedo = temp_cast_1;
				float Alpha = ( FinalShape128 * saturate( cameraDepthFade283 ) );
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
-1680;19;1680;989;523.5579;429.4034;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;24;-4532.587,1173.663;Inherit;False;1116;329;Time;8;17;16;19;18;21;22;20;23;;1,0.8933854,0,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-4482.587,1353.663;Inherit;False;Property;_WindDirection;Wind Direction;2;0;Create;True;0;0;0;False;0;False;0;0.9071112;0;6.283185;0;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;21;-4188.587,1331.663;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;22;-4188.587,1391.663;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;16;-4237.587,1270.663;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-3975,1343;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-3975.587,1223.663;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;12;-3512.804,1503.403;Inherit;False;641;236.001;Uvs;3;3;2;1;;0,0.2978659,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;17;-3782.587,1275.663;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;90;-4966.541,-369.7018;Inherit;False;2078.429;583.5631;Detail Shape Blend;21;71;72;73;74;85;86;87;88;89;76;77;78;79;80;82;83;84;81;130;243;244;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;1;-3462.804,1553.403;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;43;-4932.054,212.7223;Inherit;False;2049.971;582.3226;MainShapeBlend;21;42;36;37;35;39;34;14;38;13;26;31;29;27;25;28;68;69;70;132;245;246;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;280;-5298.218,1850.133;Inherit;False;2429.451;675.0229;Weather Mask;21;200;205;199;196;204;208;219;203;209;201;206;207;195;197;215;210;214;217;216;218;211;;0.4716981,0,0.00800749,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;23;-3640.587,1269.663;Inherit;False;Time;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-4910.054,709.9183;Inherit;False;Property;_MainShapeSpeed;Main Shape Speed;5;0;Create;True;0;0;0;False;0;False;1;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-4914.532,97.8614;Inherit;False;Property;_DetailShapeSpeed;Detail Shape Speed;6;0;Create;True;0;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-4912.063,638.1719;Inherit;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;0;False;0;False;0.8;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;200;-5248.218,2356.01;Inherit;False;Property;_Weathervariationspeed;Weather variation speed;3;0;Create;True;0;0;0;False;0;False;1;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;86;-4916.541,26.11527;Inherit;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;0;False;0;False;0.8;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;2;-3235.804,1604.404;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;71;-4855.532,-57.13817;Inherit;False;23;Time;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;205;-4961.397,2390.156;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-4695.063,625.1719;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;25;-4851.054,554.9184;Inherit;False;23;Time;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;-4699.541,13.11526;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;199;-5142.143,2278.856;Inherit;False;23;Time;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;11;-3416.81,793.7709;Inherit;False;540;708.9996;Textures;4;7;8;6;5;;0.0235849,1,0.1818062,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;3;-3095.804,1600.404;Inherit;False;UV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-4539.532,43.86153;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;7;-3365.81,1058.77;Inherit;True;Property;_Detail;Detail;1;0;Create;True;0;0;0;False;0;False;None;659580748e763d34ab8b7c39c5c352d5;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;5;-3356.81,1272.77;Inherit;True;Property;_Main;Main;0;0;Create;True;0;0;0;False;0;False;None;96c0a7c751bc91c468265e08caebf4c9;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.GetLocalVarNode;29;-4750.054,424.9182;Inherit;False;3;UV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-4537.209,556.293;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-4218.306,484.5514;Inherit;False;Property;_MainShapeTilling;Main Shape Tilling;7;0;Create;True;0;0;0;False;0;False;1;5467;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-4541.687,-55.76353;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;151;-4838.605,796.7518;Inherit;False;1421.75;377;Taper;12;133;134;138;135;136;139;137;142;140;141;143;144;;0.6084906,0.82586,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;219;-4944.899,2129.842;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0.8;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;208;-4734.397,2091.156;Inherit;False;Property;_Weathertilling;Weather tilling;4;0;Create;True;0;0;0;False;0;False;1;29471;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;72;-4754.532,-187.1386;Inherit;False;3;UV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;204;-4947.397,2258.156;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-4189.784,-163.5053;Inherit;False;Property;_DetailShapeTilling;Detail Shape Tilling;8;0;Create;True;0;0;0;False;0;False;1;1000;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-4535.054,655.9183;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;196;-5018.243,2031.625;Inherit;False;3;UV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;6;-3117.81,1271.77;Inherit;False;MainShape;-1;True;1;0;SAMPLER2D;0,0;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;74;-4400.535,-178.1386;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-4001.305,504.5515;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;201;-4724.397,2195.156;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-4396.057,433.9182;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-4005.784,-107.505;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;209;-4543.397,2130.156;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;134;-4777.856,846.7518;Inherit;False;Property;_midYvalue;midYvalue;14;0;Create;True;0;0;0;False;0;False;0;402;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;31;-4407.057,537.9182;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;8;-3110.81,1063.77;Inherit;False;DetailShape;-1;True;1;0;SAMPLER2D;0,0;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;73;-4411.535,-74.13831;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;203;-4711.397,1967.156;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldPosInputsNode;133;-4788.605,924.0342;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;39;-3871.31,482.5514;Inherit;False;6;MainShape;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;34;-3901.309,377.5514;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;78;-3861.797,-48.505;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;77;-3875.798,-129.5052;Inherit;False;8;DetailShape;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;35;-3857.309,563.5516;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;14;-3850.757,292.3555;Inherit;False;6;MainShape;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;84;-3905.793,-234.5055;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;135;-4571.856,894.7518;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;207;-4410.397,2177.156;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;206;-4406.397,1944.156;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;193;-4948.854,1504.729;Inherit;False;1440.148;345.514;Light max;8;174;185;177;179;181;182;183;184;;0.8679245,0.5526878,0.7371453,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;83;-3855.245,-319.7018;Inherit;False;8;DetailShape;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;138;-4749.152,1061.096;Inherit;False;Property;_cloudHeight;cloudHeight;15;0;Create;True;0;0;0;False;0;False;0;448;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;195;-4206.839,1900.133;Inherit;True;Property;_WeatherMask;Weather Mask;21;0;Create;True;0;0;0;False;0;False;-1;None;96c0a7c751bc91c468265e08caebf4c9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;197;-4201.243,2084.625;Inherit;True;Property;_TextureSample6;Texture Sample 6;21;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;195;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.AbsOpNode;136;-4430.856,895.7518;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;82;-3655.244,-319.7018;Inherit;True;Property;_TextureSample3;Texture Sample 3;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;174;-4898.854,1554.729;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;139;-4524.856,1027.752;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;38;-3647.309,522.5515;Inherit;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-3650.756,292.3555;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;76;-3651.797,-89.50507;Inherit;True;Property;_TextureSample2;Texture Sample 2;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;185;-4695.675,1598.677;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;132;-3304.815,395.7742;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;215;-3955.936,2230.129;Inherit;False;Property;_WeatherSize;Weather Size;10;0;Create;True;0;0;0;False;0;False;0;0.169;0.001;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;130;-3318.644,-190.3231;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;177;-4497.604,1715.243;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;137;-4287.856,896.7518;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;210;-3843.403,2038.156;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;121;-2883.395,-223.3931;Inherit;False;1066.467;433;Blended shapes;5;113;114;112;115;120;;0.7830189,0.365655,0.365655,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;179;-4379.702,1594.743;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;142;-4238.856,1033.752;Inherit;False;Property;_CloudRoundness;Cloud Roundness;16;0;Create;True;0;0;0;False;0;False;0;1.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;140;-4141.856,895.7518;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;-3084.625,411.8176;Inherit;False;MainShapeBlend;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;214;-3651.936,2034.13;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;81;-3067.113,-196.2393;Inherit;False;DetailShapeBlend;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;141;-3967.856,895.7518;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;181;-4233.703,1593.743;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;113;-2797.395,-173.3931;Inherit;False;42;MainShapeBlend;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;217;-3460.936,2034.13;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;216;-3651.936,2196.129;Inherit;False;Property;_WeatherSoftness;Weather Softness;13;0;Create;True;0;0;0;False;0;False;0;0.33;0.001;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;112;-2798.395,-104.3931;Inherit;False;81;DetailShapeBlend;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;115;-2835.395,-35.39304;Inherit;False;Property;_Detailsweight;Details weight;9;0;Create;True;0;0;0;False;0;False;0;0.324;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;114;-2503.395,-93.3931;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;143;-3806.856,901.7518;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;218;-3268.936,2035.13;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;182;-4059.705,1593.743;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;120;-2040.926,-98.3207;Inherit;False;Blendedshapes;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;211;-3092.769,2022.958;Inherit;False;weather;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;129;-2876.938,210.7566;Inherit;False;1143;327;Final Processing;10;122;123;125;126;124;127;128;146;147;213;;0.3706835,0.4927672,0.6603774,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;144;-3640.856,895.7518;Inherit;False;Taper;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;183;-3898.705,1599.743;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;213;-2840.17,389.6936;Inherit;False;211;weather;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;122;-2870.206,259.0004;Inherit;False;120;Blendedshapes;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;171;-2884.458,-964.7565;Inherit;False;1470.103;729.8895;SSS;16;168;165;164;163;169;158;159;167;162;152;153;155;154;156;157;160;;0.7688679,0.8887861,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;184;-3732.705,1593.743;Inherit;False;Light;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;147;-2838.833,322.6191;Inherit;False;144;Taper;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;165;-2435.744,-914.7565;Inherit;False;144;Taper;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;146;-2648.833,265.6191;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;168;-2507.417,-819.2707;Inherit;False;Property;_CloudDensity;Cloud Density ;20;0;Create;True;0;0;0;False;0;False;0;2;0.5;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;124;-2811.938,462.7566;Inherit;False;Property;_CloudSize;Cloud Size;11;0;Create;True;0;0;0;False;0;False;0;0.073;0.001;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;170;-2342.201,609.5848;Inherit;False;184;Light;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;123;-2507.938,266.7566;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;186;-2187.193,614.7444;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;300;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;164;-2179.184,-902.7953;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;127;-2506.938,421.7566;Inherit;False;Property;_CloudSoftness;Cloud Softness;12;0;Create;True;0;0;0;False;0;False;0;0.49;0.001;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;163;-2007.685,-903.2947;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;125;-2316.938,266.7566;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;187;-2065.194,607.7444;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;281;-1402.229,-220.1;Inherit;False;1744.229;859.0258;Final Render;28;145;274;273;189;188;272;192;271;269;44;268;276;275;278;47;50;48;49;45;282;283;284;285;286;287;295;296;297;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;270;-1911.675,605.6718;Inherit;False;LightGradient;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;169;-1824.799,-910.6085;Inherit;False;Density;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;126;-2124.938,267.7566;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;285;-1394.659,542.0048;Inherit;False;Property;_ClippingSmoothness;Clipping Smoothness;22;0;Create;True;0;0;0;False;0;False;0;5;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;189;-490.4019,-30.83777;Inherit;False;Constant;_Int0;Int 0;20;0;Create;True;0;0;0;False;0;False;1;0;False;0;1;INT;0
Node;AmplifyShaderEditor.GetLocalVarNode;274;-524.5503,-170.1;Inherit;False;270;LightGradient;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;145;-504.833,-100.3809;Inherit;False;169;Density;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CameraDepthFade;283;-1102.659,507.0048;Inherit;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;128;-1957.938,260.7566;Inherit;False;FinalShape;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;272;-530.5503,135.9;Inherit;False;270;LightGradient;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;44;-750.3884,448.9258;Inherit;False;128;FinalShape;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;273;-913.5503,122.9;Inherit;False;270;LightGradient;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;284;-767.6594,514.0048;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;188;-297.4019,-105.8378;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;160;-2434.458,-477.2288;Inherit;False;Property;_SSSbrightness;SSS brightness;19;0;Create;True;0;0;0;False;0;False;0;0.61;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;278;-382.5254,202.4399;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;244;-3595.887,-143.7454;Inherit;False;Uv2;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;268;-607.5648,332.0947;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;152;-2782.458,-726.2287;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;243;-3582.887,119.2546;Inherit;False;Uv1;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LightAttenuation;287;-395.2275,469.5037;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;289;-2788.966,1055.135;Inherit;False;Property;_FogDistance1;Fog Distance;24;0;Create;True;0;0;0;False;0;False;0;0.005;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;295;-84.22453,404.5966;Inherit;False;290;Fog;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;282;-523.6594,465.0048;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;291;-2002.191,850.1682;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;292;-2197.589,847.6822;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;293;-2432.003,1073.997;Inherit;False;Property;_FogSoftness1;FogSoftness;18;0;Create;True;0;0;0;False;0;False;0;0.28;0.001;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;294;-2453.116,840.7422;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;288;-2820.98,847.6498;Inherit;True;Property;_FogGradient1;Fog Gradient;23;0;Create;True;0;0;0;False;0;False;-1;None;dfb02222bd934ac4e8c0ced71117a7ce;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;290;-1823.665,844.2357;Inherit;False;Fog;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;167;-1638.357,-707.7209;Inherit;False;SSS;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;162;-1782.336,-701.9643;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FogAndAmbientColorsNode;296;-182.2245,339.5966;Inherit;False;unity_FogColor;0;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;192;-858.7405,188.7225;Inherit;False;167;SSS;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;157;-2125.458,-677.2287;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;245;-3580.032,707.2515;Inherit;False;Uv4;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NegateNode;154;-2581.458,-598.2287;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;271;-876.5503,261.9;Inherit;False;270;LightGradient;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;276;-370.7395,352.7216;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;153;-2834.458,-586.2287;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;158;-2438.458,-545.2287;Inherit;False;Property;_SSSDiffusion;SSS Diffusion;17;0;Create;True;0;0;0;False;0;False;0;8.3;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;297;198.7754,97.59661;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;156;-2287.458,-679.2287;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;246;-3594.032,465.252;Inherit;False;Uv3;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;275;-183.7395,216.7216;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;286;-211.2275,118.5037;Inherit;False;Constant;_Float2;Float 2;22;0;Create;True;0;0;0;False;0;False;0.4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;155;-2432.458,-678.2287;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;159;-1946.46,-676.2287;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;269;-139.5648,-31.90533;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;45;0,2;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;49;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;48;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;46;464.8987,-5.097688;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;CloudsSQ;94348b07e5e8bab40bd6c8a1e3df54cd;True;Forward;0;1;Forward;18;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;0;False;True;1;5;False;-1;10;False;-1;1;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;0;Hidden/InternalErrorShader;0;0;Standard;38;Workflow;1;Surface;1;  Refraction Model;0;  Blend;0;Two Sided;1;Fragment Normal Space,InvertActionOnDeselection;0;Transmission;0;  Transmission Shadow;0.5,False,-1;Translucency;0;  Translucency Strength;1,False,-1;  Normal Distortion;0.5,False,-1;  Scattering;2,False,-1;  Direct;0.9,False,-1;  Ambient;0.1,False,-1;  Shadow;0.5,False,-1;Cast Shadows;0;  Use Shadow Threshold;1;Receive Shadows;1;GPU Instancing;1;LOD CrossFade;1;Built-in Fog;0;_FinalColorxAlpha;0;Meta Pass;1;Override Baked GI;0;Extra Pre Pass;0;DOTS Instancing;0;Tessellation;0;  Phong;0;  Strength;0.5,False,-1;  Type;0;  Tess;16,False,-1;  Min;10,False,-1;  Max;25,False,-1;  Edge Length;16,False,-1;  Max Displacement;25,False,-1;Write Depth;0;  Early Z;0;Vertex Position,InvertActionOnDeselection;1;0;6;False;True;False;True;True;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;50;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;True;1;5;False;-1;10;False;-1;1;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=Universal2D;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;47;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;21;0;20;0
WireConnection;22;0;20;0
WireConnection;18;0;16;0
WireConnection;18;1;22;0
WireConnection;19;0;16;0
WireConnection;19;1;21;0
WireConnection;17;0;19;0
WireConnection;17;1;18;0
WireConnection;23;0;17;0
WireConnection;2;0;1;1
WireConnection;2;1;1;3
WireConnection;205;0;200;0
WireConnection;68;0;28;0
WireConnection;68;1;69;0
WireConnection;89;0;85;0
WireConnection;89;1;86;0
WireConnection;3;0;2;0
WireConnection;87;0;71;0
WireConnection;87;1;85;0
WireConnection;70;0;25;0
WireConnection;70;1;68;0
WireConnection;88;0;71;0
WireConnection;88;1;89;0
WireConnection;219;0;199;0
WireConnection;219;1;205;0
WireConnection;204;0;199;0
WireConnection;204;1;200;0
WireConnection;27;0;25;0
WireConnection;27;1;28;0
WireConnection;6;0;5;0
WireConnection;74;0;72;0
WireConnection;74;1;87;0
WireConnection;37;0;36;0
WireConnection;201;0;196;0
WireConnection;201;1;204;0
WireConnection;26;0;29;0
WireConnection;26;1;27;0
WireConnection;79;0;80;0
WireConnection;209;0;208;0
WireConnection;31;0;29;0
WireConnection;31;1;70;0
WireConnection;8;0;7;0
WireConnection;73;0;72;0
WireConnection;73;1;88;0
WireConnection;203;0;196;0
WireConnection;203;1;219;0
WireConnection;34;0;26;0
WireConnection;34;1;36;0
WireConnection;78;0;73;0
WireConnection;78;1;79;0
WireConnection;35;0;31;0
WireConnection;35;1;37;0
WireConnection;84;0;74;0
WireConnection;84;1;80;0
WireConnection;135;0;134;0
WireConnection;135;1;133;2
WireConnection;207;0;201;0
WireConnection;207;1;209;0
WireConnection;206;0;203;0
WireConnection;206;1;208;0
WireConnection;195;1;206;0
WireConnection;197;1;207;0
WireConnection;136;0;135;0
WireConnection;82;0;83;0
WireConnection;82;1;84;0
WireConnection;139;0;138;0
WireConnection;38;0;39;0
WireConnection;38;1;35;0
WireConnection;13;0;14;0
WireConnection;13;1;34;0
WireConnection;76;0;77;0
WireConnection;76;1;78;0
WireConnection;185;0;134;0
WireConnection;185;1;174;2
WireConnection;132;0;13;1
WireConnection;132;1;38;1
WireConnection;130;0;82;1
WireConnection;130;1;76;1
WireConnection;177;0;138;0
WireConnection;137;0;136;0
WireConnection;137;1;139;0
WireConnection;210;0;195;1
WireConnection;210;1;197;1
WireConnection;179;0;185;0
WireConnection;179;1;177;0
WireConnection;140;0;137;0
WireConnection;42;0;132;0
WireConnection;214;0;210;0
WireConnection;214;1;215;0
WireConnection;81;0;130;0
WireConnection;141;0;140;0
WireConnection;141;1;142;0
WireConnection;181;0;179;0
WireConnection;217;0;214;0
WireConnection;114;0;113;0
WireConnection;114;1;112;0
WireConnection;114;2;115;0
WireConnection;143;0;141;0
WireConnection;218;0;217;0
WireConnection;218;1;216;0
WireConnection;182;0;181;0
WireConnection;182;1;142;0
WireConnection;120;0;114;0
WireConnection;211;0;218;0
WireConnection;144;0;143;0
WireConnection;183;0;182;0
WireConnection;184;0;183;0
WireConnection;146;0;122;0
WireConnection;146;1;213;0
WireConnection;146;2;147;0
WireConnection;123;0;146;0
WireConnection;123;1;124;0
WireConnection;186;0;170;0
WireConnection;164;0;165;0
WireConnection;164;1;168;0
WireConnection;163;0;164;0
WireConnection;125;0;123;0
WireConnection;187;0;186;0
WireConnection;270;0;187;0
WireConnection;169;0;163;0
WireConnection;126;0;125;0
WireConnection;126;1;127;0
WireConnection;283;0;285;0
WireConnection;128;0;126;0
WireConnection;284;0;283;0
WireConnection;188;0;145;0
WireConnection;188;1;189;0
WireConnection;188;2;274;0
WireConnection;278;0;192;0
WireConnection;244;0;84;0
WireConnection;268;1;271;0
WireConnection;243;0;78;0
WireConnection;282;0;44;0
WireConnection;282;1;284;0
WireConnection;291;0;292;0
WireConnection;291;1;293;0
WireConnection;292;0;294;0
WireConnection;294;0;288;1
WireConnection;294;1;289;0
WireConnection;290;0;291;0
WireConnection;167;0;162;0
WireConnection;162;0;163;0
WireConnection;162;1;159;0
WireConnection;157;0;156;0
WireConnection;157;1;158;0
WireConnection;245;0;35;0
WireConnection;154;0;153;0
WireConnection;297;0;275;0
WireConnection;297;1;296;0
WireConnection;297;2;295;0
WireConnection;156;0;155;0
WireConnection;246;0;34;0
WireConnection;275;0;278;0
WireConnection;275;1;276;0
WireConnection;275;2;287;0
WireConnection;155;0;152;0
WireConnection;155;1;154;0
WireConnection;159;0;157;0
WireConnection;159;1;160;0
WireConnection;269;0;188;0
WireConnection;269;1;273;0
WireConnection;269;2;272;0
WireConnection;46;0;269;0
WireConnection;46;2;297;0
WireConnection;46;4;286;0
WireConnection;46;6;282;0
ASEEND*/
//CHKSM=EB76C30526333DD8EFA220CEC97EC4F461334B86