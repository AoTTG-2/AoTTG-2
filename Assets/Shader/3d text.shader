Shader "Custom/3d text" {
	Properties {
		_MainTex ("Font Texture", 2D) = "white" {}
	}
 
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Lighting Off Cull Off ZWrite Off Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		Pass {
			SetTexture [_MainTex] {
				combine primary, texture * primary
			}
		}
	}
}
