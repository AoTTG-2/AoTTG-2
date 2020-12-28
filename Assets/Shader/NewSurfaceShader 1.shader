// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

    Shader "Custom/SetAlpha" {
        Properties
        {
            [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
            _ColorTint ("Tint", Color) = (1,1,1,1)
            _Colorin ("Color In", Color) = (1,1,1,1)
            _Colorout ("Color Out", Color) = (1,1,1,1)
            _ReplaceColorCutoff("Color Replace Threshold", float) = 0
            [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
            _AlphaColor("AlphaColor", Color) = (0,0,0,1)
            _AlphaColorCutoff("Alpha Threshold", float) = 0
        }
        SubShader
        {
            Tags
            {
                "Queue"="Transparent"
                "IgnoreProjector"="True"
                "RenderType"="Transparent"
                "PreviewType"="Plane"
                "CanUseSpriteAtlas"="True"
            }
            Cull Off
            Lighting Off
            ZWrite Off
            Fog { Mode Off }
            Blend SrcAlpha OneMinusSrcAlpha
            Pass
            {
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag        
                #pragma multi_compile DUMMY PIXELSNAP_ON
                #include "UnityCG.cginc"
     
             sampler2D _MainTex;
             fixed4 _AlphaColor;
             
             
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
                fixed4 _Colorin;
                fixed4 _Colorout;
                float _AlphaColorCutoff;
                float _ReplaceColorCutoff;
                v2f vert(appdata_t IN)
                {
                    v2f OUT;
                    OUT.vertex = UnityObjectToClipPos(IN.vertex);
                    OUT.texcoord = IN.texcoord;      
                    //This didn't work, so commenting it out, added it in fragment shader
                   // OUT.color = IN.color * _ColorTint;
                    #ifdef PIXELSNAP_ON
                    OUT.vertex = UnityPixelSnap (OUT.vertex);
                    #endif
                   
                    return OUT;
                }
             
                fixed4 frag(v2f IN) : COLOR
                {
                    float4 texColor = tex2D( _MainTex, IN.texcoord );
                   
                   
                    if( //the threshold is needed because exact values cant be used
                        (abs(texColor.r - _Colorin.r) < _ReplaceColorCutoff) &&
                        (abs(texColor.g - _Colorin.g) < _ReplaceColorCutoff) &&
                        (abs(texColor.b - _Colorin.b) < _ReplaceColorCutoff))
                    { //replace RGB and alpha with colorout RGB and alpha
                        texColor = _Colorout;
                     }
     
                    if(
                        (abs(texColor.r - _AlphaColor.r) < _AlphaColorCutoff) &&
                        (abs(texColor.g - _AlphaColor.g) < _AlphaColorCutoff) &&
                        (abs(texColor.b - _AlphaColor.b) < _AlphaColorCutoff))
                    { //replace replace alpha if matches this color
                        texColor.a = 0;
                    }
     
                    return texColor * _ColorTint; //apply the tint
                }
            ENDCG
            }
        }
    }
