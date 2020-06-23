Shader "Custom/DoubleSided" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader {
 
        Pass {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
         
            ZWrite On ZTest LEqual
            Cull Off
 
            CGPROGRAM
            #pragma target 3.0
            // TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
            #pragma exclude_renderers gles
         
            // -------------------------------------
 
 
            #pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma multi_compile_shadowcaster
 
            #pragma vertex vertShadowCaster
            #pragma fragment fragShadowCaster
 
            #include "UnityStandardShadow.cginc"
 
            ENDCG
        }
 
        Tags { "RenderType"="Opaque" "PreviewType"="Plane" }
 
        LOD 200
        Cull Off
 
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
 
        #pragma surface surf Standard fullforwardshadows
 
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
 
        sampler2D _MainTex;
 
        struct Input {
            float2 uv_MainTex;
            fixed facing: VFACE;
        };
 
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
 
        void surf (Input IN, inout SurfaceOutputStandard o) {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            float tmp = IN.facing;
            o.Normal = float3(0.0, 0.0, step(0.5f, IN.facing) * 2.0 - 1.0);
            //o.Normal.z *= step(IN.facing, 0.5f) * 2.0 - 1.0;
            //o.Normal = lerp(o.Normal, -o.Normal, step(IN.facing, 0.5f));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
 
