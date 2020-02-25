Shader "Unlit/Depth" {
SubShader { 
 LOD 100
 Tags { "QUEUE"="Geometry+1" "RenderType"="Opaque" }
 Pass {
  Tags { "QUEUE"="Geometry+1" "RenderType"="Opaque" }
  ColorMask 0
 }
}
}