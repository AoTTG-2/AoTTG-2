Shader "Unlit/Transparent Colored (Packed)" {
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
"!!ARBvp1.0
PARAM c[5] = { program.local[0],
		state.matrix.mvp };
MOV result.color, vertex.color;
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 6 instructions, 0 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
"vs_2_0
dcl_position0 v0
dcl_color0 v1
dcl_texcoord0 v2
mov oD0, v1
mov oT0.xy, v2
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
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "UnityPerDraw" 0
"vs_4_0
eefiecedgkjagpdphghibkdkhcjmdlcpdknikoifabaaaaaaeiacaaaaadaaaaaa
cmaaaaaajmaaaaaabaabaaaaejfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaafjaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaafpaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
adadaaaafaepfdejfeejepeoaaedepemepfcaafeeffiedepepfceeaaepfdeheo
gmaaaaaaadaaaaaaaiaaaaaafaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaa
apaaaaaafmaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapaaaaaagcaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaacaaaaaaadamaaaafdfgfpfagphdgjhegjgpgoaa
edepemepfcaafeeffiedepepfceeaaklfdeieefcdaabaaaaeaaaabaaemaaaaaa
fjaaaaaeegiocaaaaaaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaad
pcbabaaaabaaaaaafpaaaaaddcbabaaaacaaaaaaghaaaaaepccabaaaaaaaaaaa
abaaaaaagfaaaaadpccabaaaabaaaaaagfaaaaaddccabaaaacaaaaaagiaaaaac
abaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaaaaaaaaa
abaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaaaaaaaaaaaaaaaaaagbabaaa
aaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaaaaaaaaa
acaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaa
egiocaaaaaaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadgaaaaaf
pccabaaaabaaaaaaegbobaaaabaaaaaadgaaaaafdccabaaaacaaaaaaegbabaaa
acaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "UnityPerDraw" 0
"vs_4_0_level_9_1
eefiecedfobljhhcaggcaobcihnkmeanoimnachiabaaaaaadiadaaaaaeaaaaaa
daaaaaaabmabaaaafeacaaaameacaaaaebgpgodjoeaaaaaaoeaaaaaaaaacpopp
laaaaaaadeaaaaaaabaaceaaaaaadaaaaaaadaaaaaaaceaaabaadaaaaaaaaaaa
aeaaabaaaaaaaaaaaaaaaaaaaaacpoppbpaaaaacafaaaaiaaaaaapjabpaaaaac
afaaabiaabaaapjabpaaaaacafaaaciaacaaapjaafaaaaadaaaaapiaaaaaffja
acaaoekaaeaaaaaeaaaaapiaabaaoekaaaaaaajaaaaaoeiaaeaaaaaeaaaaapia
adaaoekaaaaakkjaaaaaoeiaaeaaaaaeaaaaapiaaeaaoekaaaaappjaaaaaoeia
aeaaaaaeaaaaadmaaaaappiaaaaaoekaaaaaoeiaabaaaaacaaaaammaaaaaoeia
abaaaaacaaaaapoaabaaoejaabaaaaacabaaadoaacaaoejappppaaaafdeieefc
daabaaaaeaaaabaaemaaaaaafjaaaaaeegiocaaaaaaaaaaaaeaaaaaafpaaaaad
pcbabaaaaaaaaaaafpaaaaadpcbabaaaabaaaaaafpaaaaaddcbabaaaacaaaaaa
ghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaadpccabaaaabaaaaaagfaaaaad
dccabaaaacaaaaaagiaaaaacabaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaa
aaaaaaaaegiocaaaaaaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaa
aaaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaaaaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaakpccabaaaaaaaaaaaegiocaaaaaaaaaaaadaaaaaapgbpbaaaaaaaaaaa
egaobaaaaaaaaaaadgaaaaafpccabaaaabaaaaaaegbobaaaabaaaaaadgaaaaaf
dccabaaaacaaaaaaegbabaaaacaaaaaadoaaaaabejfdeheogiaaaaaaadaaaaaa
aiaaaaaafaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaafjaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapapaaaafpaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaacaaaaaaadadaaaafaepfdejfeejepeoaaedepemepfcaafeeffiedep
epfceeaaepfdeheogmaaaaaaadaaaaaaaiaaaaaafaaaaaaaaaaaaaaaabaaaaaa
adaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaa
apaaaaaagcaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaaadamaaaafdfgfpfa
gphdgjhegjgpgoaaedepemepfcaafeeffiedepepfceeaakl"
}
}
Program "fp" {
SubProgram "opengl " {
SetTexture 0 [_MainTex] 2D 0
"!!ARBfp1.0
PARAM c[1] = { { -2.0408571, 0.5, 0.50976563 } };
TEMP R0;
TEMP R1;
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R1, fragment.color.primary, -c[0].y;
FLR R1, -R1;
MOV_SAT R1, -R1;
MUL R0, R1, R0;
ADD R0.x, R0, R0.y;
ADD R0.x, R0, R0.z;
MAD R1, R1, c[0].z, -fragment.color.primary;
MUL_SAT R1, R1, c[0].x;
ADD R0.x, R0, R0.w;
MUL result.color.w, R1, R0.x;
MOV result.color.xyz, R1;
END
# 12 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D 0
"ps_2_0
dcl_2d s0
def c0, -0.50000000, 0.50976563, -2.04085708, 0
dcl v0
dcl t0.xy
texld r0, t0, s0
add_pp r1, v0, c0.x
frc_pp r2, -r1
add_pp r1, -r1, -r2
mov_pp_sat r1, -r1
mul_pp r0, r1, r0
add_pp r0.x, r0, r0.y
mad_pp r1, r1, c0.y, -v0
add_pp r0.x, r0, r0.z
mul_pp_sat r1, r1, c0.z
add_pp r0.x, r0, r0.w
mul_pp r1.w, r1, r0.x
mov_pp oC0, r1
"
}
SubProgram "d3d11 " {
SetTexture 0 [_MainTex] 2D 0
"ps_4_0
eefieceddafckhhgggibjnbngfednpodpjjiaphaabaaaaaajmacaaaaadaaaaaa
cmaaaaaakaaaaaaaneaaaaaaejfdeheogmaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaagcaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
adadaaaafdfgfpfagphdgjhegjgpgoaaedepemepfcaafeeffiedepepfceeaakl
epfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaa
aaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcmaabaaaaeaaaaaaa
haaaaaaafkaaaaadaagabaaaaaaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaa
gcbaaaadpcbabaaaabaaaaaagcbaaaaddcbabaaaacaaaaaagfaaaaadpccabaaa
aaaaaaaagiaaaaacacaaaaaaaaaaaaakpcaabaaaaaaaaaaaegbobaaaabaaaaaa
aceaaaaaaaaaaalpaaaaaalpaaaaaalpaaaaaalpeccaaaafpcaabaaaaaaaaaaa
egaobaaaaaaaaaaaefaaaaajpcaabaaaabaaaaaaegbabaaaacaaaaaaeghobaaa
aaaaaaaaaagabaaaaaaaaaaadiaaaaahdcaabaaaabaaaaaaegaabaaaaaaaaaaa
egaabaaaabaaaaaaaaaaaaahbcaabaaaabaaaaaabkaabaaaabaaaaaaakaabaaa
abaaaaaadcaaaaajbcaabaaaabaaaaaackaabaaaabaaaaaackaabaaaaaaaaaaa
akaabaaaabaaaaaadcaaaaajbcaabaaaabaaaaaadkaabaaaabaaaaaadkaabaaa
aaaaaaaaakaabaaaabaaaaaadcaaaaanpcaabaaaaaaaaaaaegaobaaaaaaaaaaa
aceaaaaafmipacdpfmipacdpfmipacdpfmipacdpegbobaiaebaaaaaaabaaaaaa
dicaaaakpcaabaaaaaaaaaaaegaobaaaaaaaaaaaaceaaaaalmjmacmalmjmacma
lmjmacmalmjmacmadiaaaaahiccabaaaaaaaaaaaakaabaaaabaaaaaadkaabaaa
aaaaaaaadgaaaaafhccabaaaaaaaaaaaegacbaaaaaaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
SetTexture 0 [_MainTex] 2D 0
"ps_4_0_level_9_1
eefiecednobfodmbcpcchaldajomnoekkkbfjknjabaaaaaaniadaaaaaeaaaaaa
daaaaaaagiabaaaadaadaaaakeadaaaaebgpgodjdaabaaaadaabaaaaaaacpppp
aiabaaaaciaaaaaaaaaaciaaaaaaciaaaaaaciaaabaaceaaaaaaciaaaaaaaaaa
aaacppppfbaaaaafaaaaapkaaaaaaalpfmipacdplmjmacmaaaaaaaaabpaaaaac
aaaaaaiaaaaacplabpaaaaacaaaaaaiaabaaadlabpaaaaacaaaaaajaaaaiapka
ecaaaaadaaaacpiaabaaoelaaaaioekaacaaaaadabaacpiaaaaaoelaaaaaaaka
bdaaaaacacaacpiaabaaoeibacaaaaadabaadpiaabaaoeiaacaaoeiaafaaaaad
aaaacdiaaaaaoeiaabaaoeiaacaaaaadaaaacbiaaaaaffiaaaaaaaiaaeaaaaae
aaaacbiaaaaakkiaabaakkiaaaaaaaiaaeaaaaaeaaaacbiaaaaappiaabaappia
aaaaaaiaaeaaaaaeabaacpiaabaaoeiaaaaaffkaaaaaoelbafaaaaadabaadpia
abaaoeiaaaaakkkaafaaaaadabaaciiaaaaaaaiaabaappiaabaaaaacaaaicpia
abaaoeiappppaaaafdeieefcmaabaaaaeaaaaaaahaaaaaaafkaaaaadaagabaaa
aaaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaagcbaaaadpcbabaaaabaaaaaa
gcbaaaaddcbabaaaacaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacacaaaaaa
aaaaaaakpcaabaaaaaaaaaaaegbobaaaabaaaaaaaceaaaaaaaaaaalpaaaaaalp
aaaaaalpaaaaaalpeccaaaafpcaabaaaaaaaaaaaegaobaaaaaaaaaaaefaaaaaj
pcaabaaaabaaaaaaegbabaaaacaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaa
diaaaaahdcaabaaaabaaaaaaegaabaaaaaaaaaaaegaabaaaabaaaaaaaaaaaaah
bcaabaaaabaaaaaabkaabaaaabaaaaaaakaabaaaabaaaaaadcaaaaajbcaabaaa
abaaaaaackaabaaaabaaaaaackaabaaaaaaaaaaaakaabaaaabaaaaaadcaaaaaj
bcaabaaaabaaaaaadkaabaaaabaaaaaadkaabaaaaaaaaaaaakaabaaaabaaaaaa
dcaaaaanpcaabaaaaaaaaaaaegaobaaaaaaaaaaaaceaaaaafmipacdpfmipacdp
fmipacdpfmipacdpegbobaiaebaaaaaaabaaaaaadicaaaakpcaabaaaaaaaaaaa
egaobaaaaaaaaaaaaceaaaaalmjmacmalmjmacmalmjmacmalmjmacmadiaaaaah
iccabaaaaaaaaaaaakaabaaaabaaaaaadkaabaaaaaaaaaaadgaaaaafhccabaaa
aaaaaaaaegacbaaaaaaaaaaadoaaaaabejfdeheogmaaaaaaadaaaaaaaiaaaaaa
faaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaa
aaaaaaaaadaaaaaaabaaaaaaapapaaaagcaaaaaaaaaaaaaaaaaaaaaaadaaaaaa
acaaaaaaadadaaaafdfgfpfagphdgjhegjgpgoaaedepemepfcaafeeffiedepep
fceeaaklepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklkl"
}
}
 }
}
Fallback Off
}