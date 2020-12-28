// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/PixelColors" {
    Properties
    {
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _ColorTint ("Tint", Color) = (1,1,1,1)
        _Color1in ("Color 1 In", Color) = (1,1,1,1)
        _Color1out ("Color 1 Out", Color) = (1,1,1,1)
        _Color1Threshold("Color Replace Threshold", float) = 0.02
        _Color2in ("Color 2 In", Color) = (1,1,1,1)
        _Color2out ("Color 2 Out", Color) = (1,1,1,1)
        _Color3in ("Color 3 In", Color) = (1,1,1,1)
        _Color3out ("Color 3 Out", Color) = (1,1,1,1)
        _Color4in ("Color 4 In", Color) = (1,1,1,1)
        _Color4out ("Color 4 Out", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
 
    SubShader
    {
        Tags
        {
            "Queue"="AlphaTest"
            "IgnoreProjector"="True"
            "RenderType"="Opaque"
        }
        LOD 200
 
        Blend SrcAlpha OneMinusSrcAlpha
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert Lambert alphatest:_Cutoff
            #pragma fragment frag Lambert alphatest:_Cutoff   
            #pragma multi_compile DUMMY PIXELSNAP_ON
            #include "UnityCG.cginc"
           
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };
 
            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
            };
           
            fixed4 _ColorTint;

            fixed4 _Color1in;
            fixed4 _Color1out;
            float _Color1Threshold;

            fixed4 _Color2in;
            fixed4 _Color2out;
            fixed4 _Color3in;
            fixed4 _Color3out;
            fixed4 _Color4in;
            fixed4 _Color4out;
 
            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;            
                OUT.color = IN.color * _ColorTint;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif
 
                return OUT;
            }

            float3 rgb2hsv(float3 c) 
            {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            float3 hsv2rgb(float3 c) 
            {
                c = float3(c.x, clamp(c.yz, 0.0, 1.0));
                float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
            }
 
            sampler2D _MainTex;        
           
            fixed4 frag(v2f IN) : COLOR
            {
                float4 texColor = tex2D( _MainTex, IN.texcoord );

                if( //the threshold is needed because exact values cant be used
                        (abs(texColor.r - _Color1in.r) < _Color1Threshold) &&
                        (abs(texColor.g - _Color1in.g) < _Color1Threshold) &&
                        (abs(texColor.b - _Color1in.b) < _Color1Threshold))
                { //replace RGB and alpha with colorout RGB and alpha
                    if (texColor.a > 0) 
                    {

                        float3 currentColorHsv = rgb2hsv(texColor.rgb);
                        float3 selectedHsv = rgb2hsv(_Color1in.rgb);
                        float3 colorOutHsv = rgb2hsv(_Color1out.rgb);

                        float diffS = currentColorHsv.y / selectedHsv.y;
                        float diffV = currentColorHsv.z / selectedHsv.z;
                        
                        colorOutHsv.y = colorOutHsv.y * diffS;
                        colorOutHsv.z = colorOutHsv.z * diffV;
                        float3 colorOutRGB = hsv2rgb(colorOutHsv);

                        texColor = float4(colorOutRGB.x, colorOutRGB.y, colorOutRGB.z, _Color1out.a);
                    }
                    
                }

                //texColor = all(texColor == _Color1in) ? _Color1out : texColor;
                texColor = all(texColor == _Color2in) ? _Color2out : texColor;
                texColor = all(texColor == _Color3in) ? _Color3out : texColor;
                texColor = all(texColor == _Color4in) ? _Color4out : texColor;
                 
                return texColor * IN.color;
            }

        ENDCG
        }
    }

    Fallback "Ciconia Studio/Double Sided/Transparent/Diffuse Bump Cutout"
}
 