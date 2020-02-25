Shader "Unlit/Transparent Colored (Packed) (AlphaClip)" {
Properties {
 _MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  Offset -1, -1
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Vector 5 [_MainTex_ST]
"!!ARBvp1.0
PARAM c[6] = { program.local[0],
		state.matrix.mvp,
		program.local[5] };
MOV result.color, vertex.color;
MOV result.texcoord[0].xy, vertex.texcoord[0];
MAD result.texcoord[1].xy, vertex.position, c[5], c[5].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 7 instructions, 0 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_ST]
"vs_2_0
dcl_position0 v0
dcl_color0 v1
dcl_texcoord0 v2
mov oD0, v1
mov oT0.xy, v2
mad oT1.xy, v0, c4, c4.zwzw
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
SubProgram "d3d11 " {
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 32
Vector 16 [_MainTex_ST]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0
eefiecedmplpfgelnflnchcglmiodcklnmgnfmigabaaaaaakiacaaaaadaaaaaa
cmaaaaaajmaaaaaaciabaaaaejfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaafjaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaafpaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
adadaaaafaepfdejfeejepeoaaedepemepfcaafeeffiedepepfceeaaepfdeheo
ieaaaaaaaeaaaaaaaiaaaaaagiaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaa
apaaaaaaheaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapaaaaaahkaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaacaaaaaaadamaaaahkaaaaaaabaaaaaaaaaaaaaa
adaaaaaaacaaaaaaamadaaaafdfgfpfagphdgjhegjgpgoaaedepemepfcaafeef
fiedepepfceeaaklfdeieefchiabaaaaeaaaabaafoaaaaaafjaaaaaeegiocaaa
aaaaaaaaacaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaafpaaaaadpcbabaaa
aaaaaaaafpaaaaadpcbabaaaabaaaaaafpaaaaaddcbabaaaacaaaaaaghaaaaae
pccabaaaaaaaaaaaabaaaaaagfaaaaadpccabaaaabaaaaaagfaaaaaddccabaaa
acaaaaaagfaaaaadmccabaaaacaaaaaagiaaaaacabaaaaaadiaaaaaipcaabaaa
aaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaaabaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaacaaaaaakgbkbaaaaaaaaaaa
egaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaaegiocaaaabaaaaaaadaaaaaa
pgbpbaaaaaaaaaaaegaobaaaaaaaaaaadgaaaaafpccabaaaabaaaaaaegbobaaa
abaaaaaadcaaaaalmccabaaaacaaaaaaagbebaaaaaaaaaaaagiecaaaaaaaaaaa
abaaaaaakgiocaaaaaaaaaaaabaaaaaadgaaaaafdccabaaaacaaaaaaegbabaaa
acaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 32
Vector 16 [_MainTex_ST]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0_level_9_1
eefiecedfhabjlmfoclkdafnmilckfjkenoajgleabaaaaaaliadaaaaaeaaaaaa
daaaaaaadmabaaaalmacaaaacmadaaaaebgpgodjaeabaaaaaeabaaaaaaacpopp
meaaaaaaeaaaaaaaacaaceaaaaaadmaaaaaadmaaaaaaceaaabaadmaaaaaaabaa
abaaabaaaaaaaaaaabaaaaaaaeaaacaaaaaaaaaaaaaaaaaaaaacpoppbpaaaaac
afaaaaiaaaaaapjabpaaaaacafaaabiaabaaapjabpaaaaacafaaaciaacaaapja
aeaaaaaeabaaamoaaaaabejaabaabekaabaalekaafaaaaadaaaaapiaaaaaffja
adaaoekaaeaaaaaeaaaaapiaacaaoekaaaaaaajaaaaaoeiaaeaaaaaeaaaaapia
aeaaoekaaaaakkjaaaaaoeiaaeaaaaaeaaaaapiaafaaoekaaaaappjaaaaaoeia
aeaaaaaeaaaaadmaaaaappiaaaaaoekaaaaaoeiaabaaaaacaaaaammaaaaaoeia
abaaaaacaaaaapoaabaaoejaabaaaaacabaaadoaacaaoejappppaaaafdeieefc
hiabaaaaeaaaabaafoaaaaaafjaaaaaeegiocaaaaaaaaaaaacaaaaaafjaaaaae
egiocaaaabaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaadpcbabaaa
abaaaaaafpaaaaaddcbabaaaacaaaaaaghaaaaaepccabaaaaaaaaaaaabaaaaaa
gfaaaaadpccabaaaabaaaaaagfaaaaaddccabaaaacaaaaaagfaaaaadmccabaaa
acaaaaaagiaaaaacabaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaa
egiocaaaabaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaa
aaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaa
egiocaaaabaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaak
pccabaaaaaaaaaaaegiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaa
aaaaaaaadgaaaaafpccabaaaabaaaaaaegbobaaaabaaaaaadcaaaaalmccabaaa
acaaaaaaagbebaaaaaaaaaaaagiecaaaaaaaaaaaabaaaaaakgiocaaaaaaaaaaa
abaaaaaadgaaaaafdccabaaaacaaaaaaegbabaaaacaaaaaadoaaaaabejfdeheo
giaaaaaaadaaaaaaaiaaaaaafaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apapaaaafjaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapapaaaafpaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaacaaaaaaadadaaaafaepfdejfeejepeoaaedepem
epfcaafeeffiedepepfceeaaepfdeheoieaaaaaaaeaaaaaaaiaaaaaagiaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaheaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapaaaaaahkaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
adamaaaahkaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaaamadaaaafdfgfpfa
gphdgjhegjgpgoaaedepemepfcaafeeffiedepepfceeaakl"
}
}
Program "fp" {
SubProgram "opengl " {
SetTexture 0 [_MainTex] 2D 0
"!!ARBfp1.0
PARAM c[2] = { { -2.0408571, 1, 0, 0.5 },
		{ 0.50976563 } };
TEMP R0;
TEMP R1;
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R1, fragment.color.primary, -c[0].w;
FLR R1, -R1;
MOV_SAT R1, -R1;
MUL R0, R1, R0;
ADD R0.x, R0, R0.y;
ADD R0.x, R0, R0.z;
ADD R0.z, R0.x, R0.w;
ABS R0.xy, fragment.texcoord[1];
MAD R1, R1, c[1].x, -fragment.color.primary;
MUL_SAT R1, R1, c[0].x;
MAX R0.x, R0, R0.y;
ADD R0.x, -R0, c[0].y;
CMP R0.x, R0, c[0].z, R1.w;
MUL result.color.w, R0.x, R0.z;
MOV result.color.xyz, R1;
END
# 16 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D 0
"ps_2_0
dcl_2d s0
def c0, -0.50000000, 1.00000000, 0.50976563, -2.04085708
def c1, 0.00000000, 0, 0, 0
dcl v0
dcl t0.xy
dcl t1.xy
texld r1, t0, s0
add_pp r2, v0, c0.x
frc_pp r0, -r2
add_pp r0, -r2, -r0
abs r2.xy, t1
max r2.x, r2, r2.y
mov_pp_sat r0, -r0
mul_pp r1, r0, r1
add_pp r1.x, r1, r1.y
add_pp r1.x, r1, r1.z
mad_pp r0, r0, c0.z, -v0
mul_pp_sat r0, r0, c0.w
add r2.x, -r2, c0.y
add_pp r1.x, r1, r1.w
cmp_pp r2.x, r2, r0.w, c1
mul_pp r0.w, r2.x, r1.x
mov_pp oC0, r0
"
}
SubProgram "d3d11 " {
SetTexture 0 [_MainTex] 2D 0
"ps_4_0
eefiecedihilbignjfhboendcnfpibbfpaanafbaabaaaaaaeeadaaaaadaaaaaa
cmaaaaaaliaaaaaaomaaaaaaejfdeheoieaaaaaaaeaaaaaaaiaaaaaagiaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaheaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaahkaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
adadaaaahkaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaaamamaaaafdfgfpfa
gphdgjhegjgpgoaaedepemepfcaafeeffiedepepfceeaaklepfdeheocmaaaaaa
abaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapaaaaaa
fdfgfpfegbhcghgfheaaklklfdeieefcfaacaaaaeaaaaaaajeaaaaaafkaaaaad
aagabaaaaaaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaagcbaaaadpcbabaaa
abaaaaaagcbaaaaddcbabaaaacaaaaaagcbaaaadmcbabaaaacaaaaaagfaaaaad
pccabaaaaaaaaaaagiaaaaacacaaaaaaaaaaaaakpcaabaaaaaaaaaaaegbobaaa
abaaaaaaaceaaaaaaaaaaalpaaaaaalpaaaaaalpaaaaaalpeccaaaafpcaabaaa
aaaaaaaaegaobaaaaaaaaaaaefaaaaajpcaabaaaabaaaaaaegbabaaaacaaaaaa
eghobaaaaaaaaaaaaagabaaaaaaaaaaadiaaaaahdcaabaaaabaaaaaaegaabaaa
aaaaaaaaegaabaaaabaaaaaaaaaaaaahbcaabaaaabaaaaaabkaabaaaabaaaaaa
akaabaaaabaaaaaadcaaaaajbcaabaaaabaaaaaackaabaaaabaaaaaackaabaaa
aaaaaaaaakaabaaaabaaaaaadcaaaaajbcaabaaaabaaaaaadkaabaaaabaaaaaa
dkaabaaaaaaaaaaaakaabaaaabaaaaaadcaaaaanpcaabaaaaaaaaaaaegaobaaa
aaaaaaaaaceaaaaafmipacdpfmipacdpfmipacdpfmipacdpegbobaiaebaaaaaa
abaaaaaadicaaaakpcaabaaaaaaaaaaaegaobaaaaaaaaaaaaceaaaaalmjmacma
lmjmacmalmjmacmalmjmacmadeaaaaajccaabaaaabaaaaaadkbabaiaibaaaaaa
acaaaaaackbabaiaibaaaaaaacaaaaaaaaaaaaaiccaabaaaabaaaaaabkaabaia
ebaaaaaaabaaaaaaabeaaaaaaaaaiadpdbaaaaahccaabaaaabaaaaaabkaabaaa
abaaaaaaabeaaaaaaaaaaaaadhaaaaajicaabaaaaaaaaaaabkaabaaaabaaaaaa
abeaaaaaaaaaaaaadkaabaaaaaaaaaaadgaaaaafhccabaaaaaaaaaaaegacbaaa
aaaaaaaadiaaaaahiccabaaaaaaaaaaaakaabaaaabaaaaaadkaabaaaaaaaaaaa
doaaaaab"
}
SubProgram "d3d11_9x " {
SetTexture 0 [_MainTex] 2D 0
"ps_4_0_level_9_1
eefiecedonljanjgjklbphddemngjpgjfaciegjkabaaaaaaniaeaaaaaeaaaaaa
daaaaaaamaabaaaabiaeaaaakeaeaaaaebgpgodjiiabaaaaiiabaaaaaaacpppp
gaabaaaaciaaaaaaaaaaciaaaaaaciaaaaaaciaaabaaceaaaaaaciaaaaaaaaaa
aaacppppfbaaaaafaaaaapkaaaaaaalpfmipacdplmjmacmaaaaaiadpfbaaaaaf
abaaapkaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaabpaaaaacaaaaaaiaaaaacpla
bpaaaaacaaaaaaiaabaaaplabpaaaaacaaaaaajaaaaiapkaecaaaaadaaaacpia
abaaoelaaaaioekaacaaaaadabaacpiaaaaaoelaaaaaaakabdaaaaacacaacpia
abaaoeibacaaaaadabaadpiaabaaoeiaacaaoeiaafaaaaadaaaacdiaaaaaoeia
abaaoeiaacaaaaadaaaacbiaaaaaffiaaaaaaaiaaeaaaaaeaaaacbiaaaaakkia
abaakkiaaaaaaaiaaeaaaaaeaaaacbiaaaaappiaabaappiaaaaaaaiaaeaaaaae
abaacpiaabaaoeiaaaaaffkaaaaaoelbafaaaaadabaadpiaabaaoeiaaaaakkka
cdaaaaacaaaaamiaabaaoelaalaaaaadacaaabiaaaaappiaaaaakkiaacaaaaad
aaaaaciaacaaaaibaaaappkafiaaaaaeaaaacciaaaaaffiaabaappiaabaaaaka
afaaaaadabaaciiaaaaaaaiaaaaaffiaabaaaaacaaaicpiaabaaoeiappppaaaa
fdeieefcfaacaaaaeaaaaaaajeaaaaaafkaaaaadaagabaaaaaaaaaaafibiaaae
aahabaaaaaaaaaaaffffaaaagcbaaaadpcbabaaaabaaaaaagcbaaaaddcbabaaa
acaaaaaagcbaaaadmcbabaaaacaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaac
acaaaaaaaaaaaaakpcaabaaaaaaaaaaaegbobaaaabaaaaaaaceaaaaaaaaaaalp
aaaaaalpaaaaaalpaaaaaalpeccaaaafpcaabaaaaaaaaaaaegaobaaaaaaaaaaa
efaaaaajpcaabaaaabaaaaaaegbabaaaacaaaaaaeghobaaaaaaaaaaaaagabaaa
aaaaaaaadiaaaaahdcaabaaaabaaaaaaegaabaaaaaaaaaaaegaabaaaabaaaaaa
aaaaaaahbcaabaaaabaaaaaabkaabaaaabaaaaaaakaabaaaabaaaaaadcaaaaaj
bcaabaaaabaaaaaackaabaaaabaaaaaackaabaaaaaaaaaaaakaabaaaabaaaaaa
dcaaaaajbcaabaaaabaaaaaadkaabaaaabaaaaaadkaabaaaaaaaaaaaakaabaaa
abaaaaaadcaaaaanpcaabaaaaaaaaaaaegaobaaaaaaaaaaaaceaaaaafmipacdp
fmipacdpfmipacdpfmipacdpegbobaiaebaaaaaaabaaaaaadicaaaakpcaabaaa
aaaaaaaaegaobaaaaaaaaaaaaceaaaaalmjmacmalmjmacmalmjmacmalmjmacma
deaaaaajccaabaaaabaaaaaadkbabaiaibaaaaaaacaaaaaackbabaiaibaaaaaa
acaaaaaaaaaaaaaiccaabaaaabaaaaaabkaabaiaebaaaaaaabaaaaaaabeaaaaa
aaaaiadpdbaaaaahccaabaaaabaaaaaabkaabaaaabaaaaaaabeaaaaaaaaaaaaa
dhaaaaajicaabaaaaaaaaaaabkaabaaaabaaaaaaabeaaaaaaaaaaaaadkaabaaa
aaaaaaaadgaaaaafhccabaaaaaaaaaaaegacbaaaaaaaaaaadiaaaaahiccabaaa
aaaaaaaaakaabaaaabaaaaaadkaabaaaaaaaaaaadoaaaaabejfdeheoieaaaaaa
aeaaaaaaaiaaaaaagiaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaa
heaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapapaaaahkaaaaaaaaaaaaaa
aaaaaaaaadaaaaaaacaaaaaaadadaaaahkaaaaaaabaaaaaaaaaaaaaaadaaaaaa
acaaaaaaamamaaaafdfgfpfagphdgjhegjgpgoaaedepemepfcaafeeffiedepep
fceeaaklepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklkl"
}
}
 }
}
Fallback Off
}