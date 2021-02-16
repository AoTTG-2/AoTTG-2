Shader "Xffect/displacement/screen" {
Properties {
 _DispMap ("Displacement Map (RG)", 2D) = "white" {}
 _MaskTex ("Mask (R)", 2D) = "white" {}
 _DispScrollSpeedX ("Map Scroll Speed X", Float) = 0
 _DispScrollSpeedY ("Map Scroll Speed Y", Float) = 0
 _StrengthX ("Displacement Strength X", Float) = 1
 _StrengthY ("Displacement Strength Y", Float) = -1
}
SubShader { 
 Tags { "QUEUE"="Transparent+99" "RenderType"="Transparent" }
 GrabPass {
  Name "BASE"
  Tags { "LIGHTMODE"="Always" }
 }
 Pass {
  Name "BASE"
  Tags { "LIGHTMODE"="Always" "QUEUE"="Transparent+99" "RenderType"="Transparent" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "color", Color
   Bind "texcoord", TexCoord
  }
  ZTest Always
  ZWrite Off
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
Vector 5 [_DispMap_ST]
"!!ARBvp1.0
PARAM c[6] = { { 0.5 },
		state.matrix.mvp,
		program.local[5] };
TEMP R0;
TEMP R1;
DP4 R0.z, vertex.position, c[4];
DP4 R0.x, vertex.position, c[1];
DP4 R0.y, vertex.position, c[2];
MOV R1.w, R0.z;
DP4 R1.z, vertex.position, c[3];
MOV R1.x, R0;
MOV R1.y, R0;
ADD R0.xy, R0.z, R0;
MOV result.position, R1;
MOV result.color, vertex.color;
MOV result.texcoord[2].zw, R1;
MUL result.texcoord[2].xy, R0, c[0].x;
MAD result.texcoord[0].xy, vertex.texcoord[0], c[5], c[5].zwzw;
MOV result.texcoord[1].xy, vertex.texcoord[1];
END
# 14 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_DispMap_ST]
"vs_2_0
def c5, 0.50000000, 0, 0, 0
dcl_position0 v0
dcl_color0 v1
dcl_texcoord0 v2
dcl_texcoord1 v3
dp4 r0.z, v0, c3
dp4 r0.x, v0, c0
dp4 r0.y, v0, c1
mov r1.y, r0
mov r1.w, r0.z
dp4 r1.z, v0, c2
mov r1.x, r0
mov r0.y, -r0
add r0.xy, r0.z, r0
mov oPos, r1
mov oD0, v1
mov oT2.zw, r1
mul oT2.xy, r0, c5.x
mad oT0.xy, v2, c4, c4.zwzw
mov oT1.xy, v3
"
}
SubProgram "d3d11 " {
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
ConstBuffer "$Globals" 64
Vector 32 [_DispMap_ST]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0
eefiecedjnckchnnhmcbglojhfmmdnchjihajjlnabaaaaaahaadaaaaadaaaaaa
cmaaaaaaleaaaaaafiabaaaaejfdeheoiaaaaaaaaeaaaaaaaiaaaaaagiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaahbaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaahhaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
adadaaaahhaaaaaaabaaaaaaaaaaaaaaadaaaaaaadaaaaaaadadaaaafaepfdej
feejepeoaaedepemepfcaafeeffiedepepfceeaaepfdeheojmaaaaaaafaaaaaa
aiaaaaaaiaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaimaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapaaaaaajcaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaacaaaaaaadamaaaajcaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaa
amadaaaajcaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaaapaaaaaafdfgfpfa
gphdgjhegjgpgoaaedepemepfcaafeeffiedepepfceeaaklfdeieefcbaacaaaa
eaaaabaaieaaaaaafjaaaaaeegiocaaaaaaaaaaaadaaaaaafjaaaaaeegiocaaa
abaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaadpcbabaaaabaaaaaa
fpaaaaaddcbabaaaacaaaaaafpaaaaaddcbabaaaadaaaaaaghaaaaaepccabaaa
aaaaaaaaabaaaaaagfaaaaadpccabaaaabaaaaaagfaaaaaddccabaaaacaaaaaa
gfaaaaadmccabaaaacaaaaaagfaaaaadpccabaaaadaaaaaagiaaaaacabaaaaaa
diaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaaabaaaaaa
dcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaaaaaaaaaa
egaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaacaaaaaa
kgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaa
abaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadgaaaaafpccabaaa
aaaaaaaaegaobaaaaaaaaaaadgaaaaafpccabaaaabaaaaaaegbobaaaabaaaaaa
dcaaaaaldccabaaaacaaaaaaegbabaaaacaaaaaaegiacaaaaaaaaaaaacaaaaaa
ogikcaaaaaaaaaaaacaaaaaadgaaaaafmccabaaaacaaaaaaagbebaaaadaaaaaa
dcaaaaamdcaabaaaaaaaaaaaegaabaaaaaaaaaaaaceaaaaaaaaaiadpaaaaialp
aaaaaaaaaaaaaaaapgapbaaaaaaaaaaadgaaaaafmccabaaaadaaaaaakgaobaaa
aaaaaaaadiaaaaakdccabaaaadaaaaaaegaabaaaaaaaaaaaaceaaaaaaaaaaadp
aaaaaadpaaaaaaaaaaaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "texcoord1" TexCoord1
ConstBuffer "$Globals" 64
Vector 32 [_DispMap_ST]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0_level_9_1
eefiecedgfopdkcneoceaamnganiabmgihfpihknabaaaaaaneaeaaaaaeaaaaaa
daaaaaaajaabaaaakiadaaaadaaeaaaaebgpgodjfiabaaaafiabaaaaaaacpopp
biabaaaaeaaaaaaaacaaceaaaaaadmaaaaaadmaaaaaaceaaabaadmaaaaaaacaa
abaaabaaaaaaaaaaabaaaaaaaeaaacaaaaaaaaaaaaaaaaaaaaacpoppfbaaaaaf
agaaapkaaaaaiadpaaaaialpaaaaaadpaaaaaaaabpaaaaacafaaaaiaaaaaapja
bpaaaaacafaaabiaabaaapjabpaaaaacafaaaciaacaaapjabpaaaaacafaaadia
adaaapjaafaaaaadaaaaapiaaaaaffjaadaaoekaaeaaaaaeaaaaapiaacaaoeka
aaaaaajaaaaaoeiaaeaaaaaeaaaaapiaaeaaoekaaaaakkjaaaaaoeiaaeaaaaae
aaaaapiaafaaoekaaaaappjaaaaaoeiaaeaaaaaeabaaadiaaaaaoeiaagaaoeka
aaaappiaafaaaaadacaaadoaabaaoeiaagaakkkaaeaaaaaeabaaadoaacaaoeja
abaaoekaabaaookaaeaaaaaeaaaaadmaaaaappiaaaaaoekaaaaaoeiaabaaaaac
aaaaammaaaaaoeiaabaaaaacacaaamoaaaaaoeiaabaaaaacaaaaapoaabaaoeja
abaaaaacabaaamoaadaabejappppaaaafdeieefcbaacaaaaeaaaabaaieaaaaaa
fjaaaaaeegiocaaaaaaaaaaaadaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaa
fpaaaaadpcbabaaaaaaaaaaafpaaaaadpcbabaaaabaaaaaafpaaaaaddcbabaaa
acaaaaaafpaaaaaddcbabaaaadaaaaaaghaaaaaepccabaaaaaaaaaaaabaaaaaa
gfaaaaadpccabaaaabaaaaaagfaaaaaddccabaaaacaaaaaagfaaaaadmccabaaa
acaaaaaagfaaaaadpccabaaaadaaaaaagiaaaaacabaaaaaadiaaaaaipcaabaaa
aaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaaabaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaacaaaaaakgbkbaaaaaaaaaaa
egaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaadaaaaaa
pgbpbaaaaaaaaaaaegaobaaaaaaaaaaadgaaaaafpccabaaaaaaaaaaaegaobaaa
aaaaaaaadgaaaaafpccabaaaabaaaaaaegbobaaaabaaaaaadcaaaaaldccabaaa
acaaaaaaegbabaaaacaaaaaaegiacaaaaaaaaaaaacaaaaaaogikcaaaaaaaaaaa
acaaaaaadgaaaaafmccabaaaacaaaaaaagbebaaaadaaaaaadcaaaaamdcaabaaa
aaaaaaaaegaabaaaaaaaaaaaaceaaaaaaaaaiadpaaaaialpaaaaaaaaaaaaaaaa
pgapbaaaaaaaaaaadgaaaaafmccabaaaadaaaaaakgaobaaaaaaaaaaadiaaaaak
dccabaaaadaaaaaaegaabaaaaaaaaaaaaceaaaaaaaaaaadpaaaaaadpaaaaaaaa
aaaaaaaadoaaaaabejfdeheoiaaaaaaaaeaaaaaaaiaaaaaagiaaaaaaaaaaaaaa
aaaaaaaaadaaaaaaaaaaaaaaapapaaaahbaaaaaaaaaaaaaaaaaaaaaaadaaaaaa
abaaaaaaapapaaaahhaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaaadadaaaa
hhaaaaaaabaaaaaaaaaaaaaaadaaaaaaadaaaaaaadadaaaafaepfdejfeejepeo
aaedepemepfcaafeeffiedepepfceeaaepfdeheojmaaaaaaafaaaaaaaiaaaaaa
iaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaimaaaaaaaaaaaaaa
aaaaaaaaadaaaaaaabaaaaaaapaaaaaajcaaaaaaaaaaaaaaaaaaaaaaadaaaaaa
acaaaaaaadamaaaajcaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaaamadaaaa
jcaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaaapaaaaaafdfgfpfagphdgjhe
gjgpgoaaedepemepfcaafeeffiedepepfceeaakl"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [_Time]
Float 1 [_StrengthX]
Float 2 [_StrengthY]
Float 3 [_DispScrollSpeedY]
Float 4 [_DispScrollSpeedX]
SetTexture 0 [_DispMap] 2D 0
SetTexture 1 [_GrabTexture] 2D 1
SetTexture 2 [_MaskTex] 2D 2
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
PARAM c[5] = { program.local[0..4] };
TEMP R0;
MOV R0.y, c[3].x;
MOV R0.x, c[4];
MUL R0.y, R0, c[0];
MUL R0.x, R0, c[0].y;
ADD R0.xy, fragment.texcoord[0], R0;
MOV R0.w, fragment.texcoord[2];
TEX R0.xy, R0, texture[0], 2D;
MUL R0.y, R0, c[2].x;
MUL R0.y, R0, fragment.texcoord[1].x;
MUL R0.x, R0, c[1];
MUL R0.x, fragment.texcoord[1], R0;
ADD R0.z, R0.y, fragment.texcoord[2].y;
ADD R0.y, fragment.texcoord[2].x, R0.x;
TEX R0.x, fragment.texcoord[0], texture[2], 2D;
TXP result.color.xyz, R0.yzzw, texture[1], 2D;
MUL result.color.w, fragment.color.primary, R0.x;
END
# 16 instructions, 1 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_Time]
Float 1 [_StrengthX]
Float 2 [_StrengthY]
Float 3 [_DispScrollSpeedY]
Float 4 [_DispScrollSpeedX]
SetTexture 0 [_DispMap] 2D 0
SetTexture 1 [_GrabTexture] 2D 1
SetTexture 2 [_MaskTex] 2D 2
"ps_2_0
dcl_2d s0
dcl_2d s1
dcl_2d s2
dcl v0.xyzw
dcl t0.xy
dcl t1.x
dcl t2
mov r0.y, c0
mov r1.y, c0
mul r1.x, c4, r0.y
mul r1.y, c3.x, r1
add r0.xy, t0, r1
mov r1.zw, t2
texld r0, r0, s0
mul_pp r1.x, r0.y, c2
mul r0.y, r1.x, t1.x
mul_pp r0.x, r0, c1
mul r0.x, t1, r0
add r1.y, r0, t2
add r1.x, t2, r0
texldp r1, r1, s1
texld r0, t0, s2
mul_pp r1.w, v0, r0.x
mov_pp oC0, r1
"
}
SubProgram "d3d11 " {
SetTexture 0 [_DispMap] 2D 0
SetTexture 1 [_GrabTexture] 2D 2
SetTexture 2 [_MaskTex] 2D 1
ConstBuffer "$Globals" 64
Float 16 [_StrengthX]
Float 20 [_StrengthY]
Float 48 [_DispScrollSpeedY]
Float 52 [_DispScrollSpeedX]
ConstBuffer "UnityPerCamera" 128
Vector 0 [_Time]
BindCB  "$Globals" 0
BindCB  "UnityPerCamera" 1
"ps_4_0
eefiecedcogbkjcaapejolkejelkjboobhdmjpcfabaaaaaapiacaaaaadaaaaaa
cmaaaaaanaaaaaaaaeabaaaaejfdeheojmaaaaaaafaaaaaaaiaaaaaaiaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaimaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapaiaaaajcaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
adadaaaajcaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaaamaeaaaajcaaaaaa
acaaaaaaaaaaaaaaadaaaaaaadaaaaaaapalaaaafdfgfpfagphdgjhegjgpgoaa
edepemepfcaafeeffiedepepfceeaaklepfdeheocmaaaaaaabaaaaaaaiaaaaaa
caaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgf
heaaklklfdeieefcomabaaaaeaaaaaaahlaaaaaafjaaaaaeegiocaaaaaaaaaaa
aeaaaaaafjaaaaaeegiocaaaabaaaaaaabaaaaaafkaaaaadaagabaaaaaaaaaaa
fkaaaaadaagabaaaabaaaaaafkaaaaadaagabaaaacaaaaaafibiaaaeaahabaaa
aaaaaaaaffffaaaafibiaaaeaahabaaaabaaaaaaffffaaaafibiaaaeaahabaaa
acaaaaaaffffaaaagcbaaaadicbabaaaabaaaaaagcbaaaaddcbabaaaacaaaaaa
gcbaaaadecbabaaaacaaaaaagcbaaaadlcbabaaaadaaaaaagfaaaaadpccabaaa
aaaaaaaagiaaaaacabaaaaaadcaaaaaldcaabaaaaaaaaaaafgifcaaaabaaaaaa
aaaaaaaabgifcaaaaaaaaaaaadaaaaaaegbabaaaacaaaaaaefaaaaajpcaabaaa
aaaaaaaaegaabaaaaaaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadiaaaaai
dcaabaaaaaaaaaaaegaabaaaaaaaaaaaegiacaaaaaaaaaaaabaaaaaadcaaaaaj
dcaabaaaaaaaaaaaegaabaaaaaaaaaaakgbkbaaaacaaaaaaegbabaaaadaaaaaa
aoaaaaahdcaabaaaaaaaaaaaegaabaaaaaaaaaaapgbpbaaaadaaaaaaefaaaaaj
pcaabaaaaaaaaaaaegaabaaaaaaaaaaaeghobaaaabaaaaaaaagabaaaacaaaaaa
dgaaaaafhccabaaaaaaaaaaaegacbaaaaaaaaaaaefaaaaajpcaabaaaaaaaaaaa
egbabaaaacaaaaaaeghobaaaacaaaaaaaagabaaaabaaaaaadiaaaaahiccabaaa
aaaaaaaaakaabaaaaaaaaaaadkbabaaaabaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
SetTexture 0 [_DispMap] 2D 0
SetTexture 1 [_GrabTexture] 2D 2
SetTexture 2 [_MaskTex] 2D 1
ConstBuffer "$Globals" 64
Float 16 [_StrengthX]
Float 20 [_StrengthY]
Float 48 [_DispScrollSpeedY]
Float 52 [_DispScrollSpeedX]
ConstBuffer "UnityPerCamera" 128
Vector 0 [_Time]
BindCB  "$Globals" 0
BindCB  "UnityPerCamera" 1
"ps_4_0_level_9_1
eefiecedinncnmkpiapobohldkmlnmileffjdjebabaaaaaajaaeaaaaaeaaaaaa
daaaaaaameabaaaaliadaaaafmaeaaaaebgpgodjimabaaaaimabaaaaaaacpppp
diabaaaafeaaaaaaadaadaaaaaaafeaaaaaafeaaadaaceaaaaaafeaaaaaaaaaa
acababaaabacacaaaaaaabaaabaaaaaaaaaaaaaaaaaaadaaabaaabaaaaaaaaaa
abaaaaaaabaaacaaaaaaaaaaaaacppppbpaaaaacaaaaaaiaaaaacplabpaaaaac
aaaaaaiaabaaaplabpaaaaacaaaaaaiaacaaaplabpaaaaacaaaaaajaaaaiapka
bpaaaaacaaaaaajaabaiapkabpaaaaacaaaaaajaacaiapkaabaaaaacaaaaaiia
acaaffkaafaaaaadaaaacbiaaaaappiaabaaffkaafaaaaadaaaacciaaaaappia
abaaaakaacaaaaadaaaaadiaaaaaoeiaabaaoelaecaaaaadaaaacpiaaaaaoeia
aaaioekaafaaaaadaaaaadiaaaaaoeiaaaaaoekaabaaaaacaaaaaiiaabaappla
aeaaaaaeabaaaciaaaaaffiaaaaappiaacaafflaaeaaaaaeabaaabiaaaaaaaia
aaaappiaacaaaalaagaaaaacaaaaabiaacaapplaafaaaaadaaaaadiaaaaaaaia
abaaoeiaecaaaaadaaaacpiaaaaaoeiaacaioekaecaaaaadabaacpiaabaaoela
abaioekaafaaaaadaaaaciiaabaaaaiaaaaapplaabaaaaacaaaicpiaaaaaoeia
ppppaaaafdeieefcomabaaaaeaaaaaaahlaaaaaafjaaaaaeegiocaaaaaaaaaaa
aeaaaaaafjaaaaaeegiocaaaabaaaaaaabaaaaaafkaaaaadaagabaaaaaaaaaaa
fkaaaaadaagabaaaabaaaaaafkaaaaadaagabaaaacaaaaaafibiaaaeaahabaaa
aaaaaaaaffffaaaafibiaaaeaahabaaaabaaaaaaffffaaaafibiaaaeaahabaaa
acaaaaaaffffaaaagcbaaaadicbabaaaabaaaaaagcbaaaaddcbabaaaacaaaaaa
gcbaaaadecbabaaaacaaaaaagcbaaaadlcbabaaaadaaaaaagfaaaaadpccabaaa
aaaaaaaagiaaaaacabaaaaaadcaaaaaldcaabaaaaaaaaaaafgifcaaaabaaaaaa
aaaaaaaabgifcaaaaaaaaaaaadaaaaaaegbabaaaacaaaaaaefaaaaajpcaabaaa
aaaaaaaaegaabaaaaaaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadiaaaaai
dcaabaaaaaaaaaaaegaabaaaaaaaaaaaegiacaaaaaaaaaaaabaaaaaadcaaaaaj
dcaabaaaaaaaaaaaegaabaaaaaaaaaaakgbkbaaaacaaaaaaegbabaaaadaaaaaa
aoaaaaahdcaabaaaaaaaaaaaegaabaaaaaaaaaaapgbpbaaaadaaaaaaefaaaaaj
pcaabaaaaaaaaaaaegaabaaaaaaaaaaaeghobaaaabaaaaaaaagabaaaacaaaaaa
dgaaaaafhccabaaaaaaaaaaaegacbaaaaaaaaaaaefaaaaajpcaabaaaaaaaaaaa
egbabaaaacaaaaaaeghobaaaacaaaaaaaagabaaaabaaaaaadiaaaaahiccabaaa
aaaaaaaaakaabaaaaaaaaaaadkbabaaaabaaaaaadoaaaaabejfdeheojmaaaaaa
afaaaaaaaiaaaaaaiaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaa
imaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapaiaaaajcaaaaaaaaaaaaaa
aaaaaaaaadaaaaaaacaaaaaaadadaaaajcaaaaaaabaaaaaaaaaaaaaaadaaaaaa
acaaaaaaamaeaaaajcaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaaapalaaaa
fdfgfpfagphdgjhegjgpgoaaedepemepfcaafeeffiedepepfceeaaklepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklkl"
}
}
 }
}
SubShader { 
 Tags { "QUEUE"="Transparent+99" "RenderType"="Transparent" }
 Pass {
  Name "BASE"
  Tags { "QUEUE"="Transparent+99" "RenderType"="Transparent" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "color", Color
   Bind "texcoord", TexCoord
  }
  ZTest Always
  ZWrite Off
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { combine texture * primary double, texture alpha * primary alpha }
 }
}
}