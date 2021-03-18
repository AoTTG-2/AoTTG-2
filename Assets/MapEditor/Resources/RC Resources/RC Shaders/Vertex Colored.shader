Shader "RC/Vertex Colored" {
	Properties{
	 _Color("Main Color", Color) = (1,1,1,1)
	 _Shininess("Shininess", Range(0.01,1)) = 0.7
	 _MainTex("Base (RGB)", 2D) = "white" {}
	}
		SubShader{
		 Pass {
		  Lighting On
		  Material {
		   Shininess[_Shininess]
		  }
		  ColorMaterial AmbientAndDiffuse
		  SetTexture[_MainTex] { combine texture * primary, texture alpha * primary alpha }
		  SetTexture[_MainTex] { ConstantColor[_Color] combine previous * constant double, previous alpha * constant alpha }
		 }
	}
		Fallback " VertexLit"
}