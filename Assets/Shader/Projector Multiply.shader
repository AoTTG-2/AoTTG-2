Shader "Projector/Multiply" {
Properties {
 _ShadowTex ("Cookie", 2D) = "gray" { TexGen ObjectLinear }
 _FalloffTex ("FallOff", 2D) = "white" { TexGen ObjectLinear }
}
SubShader { 
 Tags { "RenderType"="Transparent-1" }
 Pass {
  Tags { "RenderType"="Transparent-1" }
  ZWrite Off
  Fog {
   Color (1,1,1,1)
  }
  Blend DstColor Zero
  AlphaTest Greater 0
  ColorMask RGB
  Offset -1, -1
  SetTexture [_ShadowTex] { Matrix [_Projector] combine texture, one-texture alpha }
  SetTexture [_FalloffTex] { Matrix [_ProjectorClip] ConstantColor (1,1,1,0) combine previous lerp(texture) constant }
 }
}
}