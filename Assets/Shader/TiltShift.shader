Shader "Hidden/tiltShift" {
Properties {
 _MainTex ("Base", 2D) = "" {}
 _Blurred ("Blurred", 2D) = "" {}
 _Coc ("Coc", 2D) = "" {}
}
SubShader { 
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
"!!ARBvp1.0
PARAM c[5] = { program.local[0],
		state.matrix.mvp };
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 5 instructions, 0 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_TexelSize]
"vs_2_0
def c5, 0.00000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.x, c5
slt r0.x, c4.y, r0
max r0.x, -r0, r0
slt r0.x, c5, r0
add r0.y, -r0.x, c5
mul r0.z, v1.y, r0.y
add r0.y, -v1, c5
mad oT1.y, r0.x, r0, r0.z
mov oT0.xy, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
mov oT1.x, v1
"
}
SubProgram "d3d11 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 80
Vector 32 [_MainTex_TexelSize]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0
eefiecedgalbggkgcijkbbogllmajnckopibikhoabaaaaaahmacaaaaadaaaaaa
cmaaaaaaiaaaaaaapaaaaaaaejfdeheoemaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaaebaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaafaepfdejfeejepeoaafeeffiedepepfceeaaklkl
epfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadamaaaa
fmaaaaaaabaaaaaaaaaaaaaaadaaaaaaabaaaaaaamadaaaafdfgfpfagphdgjhe
gjgpgoaafeeffiedepepfceeaaklklklfdeieefcieabaaaaeaaaabaagbaaaaaa
fjaaaaaeegiocaaaaaaaaaaaadaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaa
fpaaaaadpcbabaaaaaaaaaaafpaaaaaddcbabaaaabaaaaaaghaaaaaepccabaaa
aaaaaaaaabaaaaaagfaaaaaddccabaaaabaaaaaagfaaaaadmccabaaaabaaaaaa
giaaaaacabaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaa
abaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaa
agbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaa
abaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaa
aaaaaaaaegiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaa
dbaaaaaibcaabaaaaaaaaaaabkiacaaaaaaaaaaaacaaaaaaabeaaaaaaaaaaaaa
aaaaaaaiccaabaaaaaaaaaaabkbabaiaebaaaaaaabaaaaaaabeaaaaaaaaaiadp
dhaaaaajiccabaaaabaaaaaaakaabaaaaaaaaaaabkaabaaaaaaaaaaabkbabaaa
abaaaaaadgaaaaafhccabaaaabaaaaaaegbabaaaabaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 80
Vector 32 [_MainTex_TexelSize]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0_level_9_1
eefiecedkfiplpebenglpjinfbgeeejidigadnefabaaaaaalmadaaaaaeaaaaaa
daaaaaaagmabaaaapiacaaaaemadaaaaebgpgodjdeabaaaadeabaaaaaaacpopp
peaaaaaaeaaaaaaaacaaceaaaaaadmaaaaaadmaaaaaaceaaabaadmaaaaaaacaa
abaaabaaaaaaaaaaabaaaaaaaeaaacaaaaaaaaaaaaaaaaaaaaacpoppfbaaaaaf
agaaapkaaaaaaaaaaaaaaamaaaaaiadpaaaaaaaabpaaaaacafaaaaiaaaaaapja
bpaaaaacafaaabiaabaaapjaabaaaaacaaaaabiaagaaaakaamaaaaadaaaaabia
abaaffkaaaaaaaiaaeaaaaaeaaaaaciaabaaffjaagaaffkaagaakkkaaeaaaaae
aaaaaeoaaaaaaaiaaaaaffiaabaaffjaafaaaaadaaaaapiaaaaaffjaadaaoeka
aeaaaaaeaaaaapiaacaaoekaaaaaaajaaaaaoeiaaeaaaaaeaaaaapiaaeaaoeka
aaaakkjaaaaaoeiaaeaaaaaeaaaaapiaafaaoekaaaaappjaaaaaoeiaaeaaaaae
aaaaadmaaaaappiaaaaaoekaaaaaoeiaabaaaaacaaaaammaaaaaoeiaabaaaaac
aaaaaloaabaacejappppaaaafdeieefcieabaaaaeaaaabaagbaaaaaafjaaaaae
egiocaaaaaaaaaaaadaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaafpaaaaad
pcbabaaaaaaaaaaafpaaaaaddcbabaaaabaaaaaaghaaaaaepccabaaaaaaaaaaa
abaaaaaagfaaaaaddccabaaaabaaaaaagfaaaaadmccabaaaabaaaaaagiaaaaac
abaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaa
abaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaa
aaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaa
acaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaa
egiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadbaaaaai
bcaabaaaaaaaaaaabkiacaaaaaaaaaaaacaaaaaaabeaaaaaaaaaaaaaaaaaaaai
ccaabaaaaaaaaaaabkbabaiaebaaaaaaabaaaaaaabeaaaaaaaaaiadpdhaaaaaj
iccabaaaabaaaaaaakaabaaaaaaaaaaabkaabaaaaaaaaaaabkbabaaaabaaaaaa
dgaaaaafhccabaaaabaaaaaaegbabaaaabaaaaaadoaaaaabejfdeheoemaaaaaa
acaaaaaaaiaaaaaadiaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaa
ebaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadadaaaafaepfdejfeejepeo
aafeeffiedepepfceeaaklklepfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadamaaaafmaaaaaaabaaaaaaaaaaaaaaadaaaaaaabaaaaaa
amadaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklkl"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [_ZBufferParams]
Vector 1 [_SimpleDofParams]
SetTexture 0 [_CameraDepthTexture] 2D 0
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
PARAM c[5] = { program.local[0..1],
		{ 0.99902344, 1, 0, 0.0024999999 },
		{ 1, 255, 65025, 16581375 },
		{ 2, 0.0039215689 } };
TEMP R0;
TEX R0.x, fragment.texcoord[0], texture[0], 2D;
MAD R0.x, R0, c[0], c[0].y;
RCP R0.y, R0.x;
ADD R0.z, -R0.y, c[1].y;
SLT R0.y, c[1], R0;
ABS R0.w, R0.y;
ADD R0.z, R0, -c[2].w;
MUL R0.y, R0.z, c[4].x;
CMP R0.z, -R0.w, c[2], c[2].y;
CMP R0.y, -R0.z, R0, c[2].z;
RCP R0.x, c[1].w;
MUL R0.x, R0.y, R0;
MIN_SAT R0.x, R0, c[2];
MUL R0, R0.x, c[3];
FRC R0, R0;
MAD result.color, -R0.yzww, c[4].y, R0;
END
# 16 instructions, 1 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_ZBufferParams]
Vector 1 [_SimpleDofParams]
SetTexture 0 [_CameraDepthTexture] 2D 0
"ps_2_0
dcl_2d s0
def c2, 0.00000000, 1.00000000, -0.00250000, 2.00000000
def c3, 0.99902344, 0.00392157, 0, 0
def c4, 1.00000000, 255.00000000, 65025.00000000, 16581375.00000000
dcl t1.xy
texld r0, t1, s0
mad r0.x, r0, c0, c0.y
rcp r1.x, r0.x
add r0.x, -r1, c1.y
add r1.x, -r1, c1.y
add r1.x, r1, c2.z
mul r2.x, r1, c2.w
cmp r0.x, r0, c2, c2.y
abs_pp r0.x, r0
rcp_pp r1.x, c1.w
cmp_pp r0.x, -r0, r2, c2
mul_pp r0.x, r0, r1
min_pp_sat r0.x, r0, c3
mul r0, r0.x, c4
frc r1, r0
mov r0.z, -r1.w
mov r0.xyw, -r1.yzxw
mad r0, r0, c3.y, r1
mov_pp oC0, r0
"
}
SubProgram "d3d11 " {
SetTexture 0 [_CameraDepthTexture] 2D 0
ConstBuffer "$Globals" 80
Vector 48 [_SimpleDofParams]
ConstBuffer "UnityPerCamera" 128
Vector 112 [_ZBufferParams]
BindCB  "$Globals" 0
BindCB  "UnityPerCamera" 1
"ps_4_0
eefiecedfgnpgjfegabnaklpdbfkmfiabinfnmkaabaaaaaacaadaaaaadaaaaaa
cmaaaaaajmaaaaaanaaaaaaaejfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadaaaaaafmaaaaaaabaaaaaaaaaaaaaaadaaaaaaabaaaaaa
amamaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklklepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklklfdeieefceiacaaaaeaaaaaaajcaaaaaa
fjaaaaaeegiocaaaaaaaaaaaaeaaaaaafjaaaaaeegiocaaaabaaaaaaaiaaaaaa
fkaaaaadaagabaaaaaaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaagcbaaaad
mcbabaaaabaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacabaaaaaaefaaaaaj
pcaabaaaaaaaaaaaogbkbaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaa
dcaaaaalbcaabaaaaaaaaaaaakiacaaaabaaaaaaahaaaaaaakaabaaaaaaaaaaa
bkiacaaaabaaaaaaahaaaaaaaoaaaaakbcaabaaaaaaaaaaaaceaaaaaaaaaiadp
aaaaiadpaaaaiadpaaaaiadpakaabaaaaaaaaaaaaaaaaaajccaabaaaaaaaaaaa
akaabaiaebaaaaaaaaaaaaaabkiacaaaaaaaaaaaadaaaaaadbaaaaaibcaabaaa
aaaaaaaabkiacaaaaaaaaaaaadaaaaaaakaabaaaaaaaaaaaaaaaaaahccaabaaa
aaaaaaaabkaabaaaaaaaaaaaabeaaaaaaknhcdllaaaaaaahccaabaaaaaaaaaaa
bkaabaaaaaaaaaaabkaabaaaaaaaaaaadhaaaaajbcaabaaaaaaaaaaaakaabaaa
aaaaaaaaabeaaaaaaaaaaaaabkaabaaaaaaaaaaaaoaaaaaibcaabaaaaaaaaaaa
akaabaaaaaaaaaaadkiacaaaaaaaaaaaadaaaaaadeaaaaahbcaabaaaaaaaaaaa
akaabaaaaaaaaaaaabeaaaaaaaaaaaaaddaaaaahbcaabaaaaaaaaaaaakaabaaa
aaaaaaaaabeaaaaahhlohpdpdiaaaaakpcaabaaaaaaaaaaaagaabaaaaaaaaaaa
aceaaaaaaaaaiadpaaaahpedaaabhoehppachnelbkaaaaafpcaabaaaaaaaaaaa
egaobaaaaaaaaaaadcaaaaanpccabaaaaaaaaaaajgapbaiaebaaaaaaaaaaaaaa
aceaaaaaibiaiadlibiaiadlibiaiadlibiaiadlegaobaaaaaaaaaaadoaaaaab
"
}
SubProgram "d3d11_9x " {
SetTexture 0 [_CameraDepthTexture] 2D 0
ConstBuffer "$Globals" 80
Vector 48 [_SimpleDofParams]
ConstBuffer "UnityPerCamera" 128
Vector 112 [_ZBufferParams]
BindCB  "$Globals" 0
BindCB  "UnityPerCamera" 1
"ps_4_0_level_9_1
eefiecedkjkedohlehhkldabikofdpamjpkoaoljabaaaaaapeaeaaaaaeaaaaaa
daaaaaaaaaacaaaafaaeaaaamaaeaaaaebgpgodjmiabaaaamiabaaaaaaacpppp
iiabaaaaeaaaaaaaacaaciaaaaaaeaaaaaaaeaaaabaaceaaaaaaeaaaaaaaaaaa
aaaaadaaabaaaaaaaaaaaaaaabaaahaaabaaabaaaaaaaaaaaaacppppfbaaaaaf
acaaapkaaknhcdllaaaaaaaahhlohplpibiaiadlfbaaaaafadaaapkaaaaaiadp
aaaahpedaaabhoehppachnelfbaaaaafaeaaapkahhlohpdpaaljdodpaaaahkdp
aaaaaaaabpaaaaacaaaaaaiaaaaaaplabpaaaaacaaaaaajaaaaiapkaabaaaaac
aaaaadiaaaaabllaecaaaaadaaaaapiaaaaaoeiaaaaioekaaeaaaaaeaaaaabia
abaaaakaaaaaaaiaabaaffkaagaaaaacaaaaabiaaaaaaaiaacaaaaadaaaaabia
aaaaaaibaaaaffkaacaaaaadaaaaaciaaaaaaaiaacaaaakaacaaaaadaaaaccia
aaaaffiaaaaaffiafiaaaaaeaaaacbiaaaaaaaiaaaaaffiaacaaffkaagaaaaac
aaaaaciaaaaappkaafaaaaadaaaacbiaaaaaffiaaaaaaaiaalaaaaadabaaciia
aaaaaaiaacaaffkaafaaaaadaaaaapiaabaappiaadaaoekaacaaaaadabaaabia
abaappiaacaakkkabdaaaaacaaaaapiaaaaaoeiafiaaaaaeaaaaapiaabaaaaia
aeaaoekaaaaaoeiaaeaaaaaeabaacliaaaaamjiaacaappkbaaaaoeiaaeaaaaae
abaaceiaaaaappiaacaappkbaaaakkiaabaaaaacaaaicpiaabaaoeiappppaaaa
fdeieefceiacaaaaeaaaaaaajcaaaaaafjaaaaaeegiocaaaaaaaaaaaaeaaaaaa
fjaaaaaeegiocaaaabaaaaaaaiaaaaaafkaaaaadaagabaaaaaaaaaaafibiaaae
aahabaaaaaaaaaaaffffaaaagcbaaaadmcbabaaaabaaaaaagfaaaaadpccabaaa
aaaaaaaagiaaaaacabaaaaaaefaaaaajpcaabaaaaaaaaaaaogbkbaaaabaaaaaa
eghobaaaaaaaaaaaaagabaaaaaaaaaaadcaaaaalbcaabaaaaaaaaaaaakiacaaa
abaaaaaaahaaaaaaakaabaaaaaaaaaaabkiacaaaabaaaaaaahaaaaaaaoaaaaak
bcaabaaaaaaaaaaaaceaaaaaaaaaiadpaaaaiadpaaaaiadpaaaaiadpakaabaaa
aaaaaaaaaaaaaaajccaabaaaaaaaaaaaakaabaiaebaaaaaaaaaaaaaabkiacaaa
aaaaaaaaadaaaaaadbaaaaaibcaabaaaaaaaaaaabkiacaaaaaaaaaaaadaaaaaa
akaabaaaaaaaaaaaaaaaaaahccaabaaaaaaaaaaabkaabaaaaaaaaaaaabeaaaaa
aknhcdllaaaaaaahccaabaaaaaaaaaaabkaabaaaaaaaaaaabkaabaaaaaaaaaaa
dhaaaaajbcaabaaaaaaaaaaaakaabaaaaaaaaaaaabeaaaaaaaaaaaaabkaabaaa
aaaaaaaaaoaaaaaibcaabaaaaaaaaaaaakaabaaaaaaaaaaadkiacaaaaaaaaaaa
adaaaaaadeaaaaahbcaabaaaaaaaaaaaakaabaaaaaaaaaaaabeaaaaaaaaaaaaa
ddaaaaahbcaabaaaaaaaaaaaakaabaaaaaaaaaaaabeaaaaahhlohpdpdiaaaaak
pcaabaaaaaaaaaaaagaabaaaaaaaaaaaaceaaaaaaaaaiadpaaaahpedaaabhoeh
ppachnelbkaaaaafpcaabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaanpccabaaa
aaaaaaaajgapbaiaebaaaaaaaaaaaaaaaceaaaaaibiaiadlibiaiadlibiaiadl
ibiaiadlegaobaaaaaaaaaaadoaaaaabejfdeheogiaaaaaaadaaaaaaaiaaaaaa
faaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaa
aaaaaaaaadaaaaaaabaaaaaaadaaaaaafmaaaaaaabaaaaaaaaaaaaaaadaaaaaa
abaaaaaaamamaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklkl
epfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaa
aaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklkl"
}
}
 }
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
"!!ARBvp1.0
PARAM c[5] = { program.local[0],
		state.matrix.mvp };
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 5 instructions, 0 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_TexelSize]
"vs_2_0
def c5, 0.00000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.x, c5
slt r0.x, c4.y, r0
max r0.x, -r0, r0
slt r0.x, c5, r0
add r0.y, -r0.x, c5
mul r0.z, v1.y, r0.y
add r0.y, -v1, c5
mad oT1.y, r0.x, r0, r0.z
mov oT0.xy, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
mov oT1.x, v1
"
}
SubProgram "d3d11 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 80
Vector 32 [_MainTex_TexelSize]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0
eefiecedgalbggkgcijkbbogllmajnckopibikhoabaaaaaahmacaaaaadaaaaaa
cmaaaaaaiaaaaaaapaaaaaaaejfdeheoemaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaaebaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaafaepfdejfeejepeoaafeeffiedepepfceeaaklkl
epfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadamaaaa
fmaaaaaaabaaaaaaaaaaaaaaadaaaaaaabaaaaaaamadaaaafdfgfpfagphdgjhe
gjgpgoaafeeffiedepepfceeaaklklklfdeieefcieabaaaaeaaaabaagbaaaaaa
fjaaaaaeegiocaaaaaaaaaaaadaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaa
fpaaaaadpcbabaaaaaaaaaaafpaaaaaddcbabaaaabaaaaaaghaaaaaepccabaaa
aaaaaaaaabaaaaaagfaaaaaddccabaaaabaaaaaagfaaaaadmccabaaaabaaaaaa
giaaaaacabaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaa
abaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaa
agbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaa
abaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaa
aaaaaaaaegiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaa
dbaaaaaibcaabaaaaaaaaaaabkiacaaaaaaaaaaaacaaaaaaabeaaaaaaaaaaaaa
aaaaaaaiccaabaaaaaaaaaaabkbabaiaebaaaaaaabaaaaaaabeaaaaaaaaaiadp
dhaaaaajiccabaaaabaaaaaaakaabaaaaaaaaaaabkaabaaaaaaaaaaabkbabaaa
abaaaaaadgaaaaafhccabaaaabaaaaaaegbabaaaabaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 80
Vector 32 [_MainTex_TexelSize]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0_level_9_1
eefiecedkfiplpebenglpjinfbgeeejidigadnefabaaaaaalmadaaaaaeaaaaaa
daaaaaaagmabaaaapiacaaaaemadaaaaebgpgodjdeabaaaadeabaaaaaaacpopp
peaaaaaaeaaaaaaaacaaceaaaaaadmaaaaaadmaaaaaaceaaabaadmaaaaaaacaa
abaaabaaaaaaaaaaabaaaaaaaeaaacaaaaaaaaaaaaaaaaaaaaacpoppfbaaaaaf
agaaapkaaaaaaaaaaaaaaamaaaaaiadpaaaaaaaabpaaaaacafaaaaiaaaaaapja
bpaaaaacafaaabiaabaaapjaabaaaaacaaaaabiaagaaaakaamaaaaadaaaaabia
abaaffkaaaaaaaiaaeaaaaaeaaaaaciaabaaffjaagaaffkaagaakkkaaeaaaaae
aaaaaeoaaaaaaaiaaaaaffiaabaaffjaafaaaaadaaaaapiaaaaaffjaadaaoeka
aeaaaaaeaaaaapiaacaaoekaaaaaaajaaaaaoeiaaeaaaaaeaaaaapiaaeaaoeka
aaaakkjaaaaaoeiaaeaaaaaeaaaaapiaafaaoekaaaaappjaaaaaoeiaaeaaaaae
aaaaadmaaaaappiaaaaaoekaaaaaoeiaabaaaaacaaaaammaaaaaoeiaabaaaaac
aaaaaloaabaacejappppaaaafdeieefcieabaaaaeaaaabaagbaaaaaafjaaaaae
egiocaaaaaaaaaaaadaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaafpaaaaad
pcbabaaaaaaaaaaafpaaaaaddcbabaaaabaaaaaaghaaaaaepccabaaaaaaaaaaa
abaaaaaagfaaaaaddccabaaaabaaaaaagfaaaaadmccabaaaabaaaaaagiaaaaac
abaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaa
abaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaa
aaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaa
acaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaa
egiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadbaaaaai
bcaabaaaaaaaaaaabkiacaaaaaaaaaaaacaaaaaaabeaaaaaaaaaaaaaaaaaaaai
ccaabaaaaaaaaaaabkbabaiaebaaaaaaabaaaaaaabeaaaaaaaaaiadpdhaaaaaj
iccabaaaabaaaaaaakaabaaaaaaaaaaabkaabaaaaaaaaaaabkbabaaaabaaaaaa
dgaaaaafhccabaaaabaaaaaaegbabaaaabaaaaaadoaaaaabejfdeheoemaaaaaa
acaaaaaaaiaaaaaadiaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaa
ebaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadadaaaafaepfdejfeejepeo
aafeeffiedepepfceeaaklklepfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadamaaaafmaaaaaaabaaaaaaaaaaaaaaadaaaaaaabaaaaaa
amadaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklkl"
}
}
Program "fp" {
SubProgram "opengl " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Blurred] 2D 1
SetTexture 2 [_Coc] 2D 2
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
PARAM c[1] = { { 1, 0.0039215689, 1.53787e-005, 6.0308629e-008 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEX R1, fragment.texcoord[0], texture[2], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R2, fragment.texcoord[0], texture[1], 2D;
ADD R2, R2, -R0;
DP4 R1.x, R1, c[0];
MAD result.color, R1.x, R2, R0;
END
# 6 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Blurred] 2D 1
SetTexture 2 [_Coc] 2D 2
"ps_2_0
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c0, 1.00000000, 0.00392157, 0.00001538, 0.00000006
dcl t0.xy
dcl t1.xy
texld r1, t1, s1
texld r0, t1, s2
texld r2, t0, s0
add_pp r1, r1, -r2
dp4 r0.x, r0, c0
mad_pp r0, r0.x, r1, r2
mov_pp oC0, r0
"
}
SubProgram "d3d11 " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Blurred] 2D 1
SetTexture 2 [_Coc] 2D 2
"ps_4_0
eefiecedapemjpcioppnpdiefppheddficjghcapabaaaaaadmacaaaaadaaaaaa
cmaaaaaajmaaaaaanaaaaaaaejfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaafmaaaaaaabaaaaaaaaaaaaaaadaaaaaaabaaaaaa
amamaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklklepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcgeabaaaaeaaaaaaafjaaaaaa
fkaaaaadaagabaaaaaaaaaaafkaaaaadaagabaaaabaaaaaafkaaaaadaagabaaa
acaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaafibiaaaeaahabaaaabaaaaaa
ffffaaaafibiaaaeaahabaaaacaaaaaaffffaaaagcbaaaaddcbabaaaabaaaaaa
gcbaaaadmcbabaaaabaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacadaaaaaa
efaaaaajpcaabaaaaaaaaaaaogbkbaaaabaaaaaaeghobaaaacaaaaaaaagabaaa
acaaaaaabbaaaaakbcaabaaaaaaaaaaaegaobaaaaaaaaaaaaceaaaaaaaaaiadp
ibiaiadlicabibdhafidibddefaaaaajpcaabaaaabaaaaaaogbkbaaaabaaaaaa
eghobaaaabaaaaaaaagabaaaabaaaaaaefaaaaajpcaabaaaacaaaaaaegbabaaa
abaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaaaaaaaaaipcaabaaaabaaaaaa
egaobaaaabaaaaaaegaobaiaebaaaaaaacaaaaaadcaaaaajpccabaaaaaaaaaaa
agaabaaaaaaaaaaaegaobaaaabaaaaaaegaobaaaacaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Blurred] 2D 1
SetTexture 2 [_Coc] 2D 2
"ps_4_0_level_9_1
eefiecedcjhkchpgbgeafbadmbenhnlmelfgboekabaaaaaadeadaaaaaeaaaaaa
daaaaaaaceabaaaajaacaaaaaaadaaaaebgpgodjomaaaaaaomaaaaaaaaacpppp
lmaaaaaadaaaaaaaaaaadaaaaaaadaaaaaaadaaaadaaceaaaaaadaaaaaaaaaaa
abababaaacacacaaaaacppppfbaaaaafaaaaapkaaaaaiadpibiaiadlicabibdh
afidibddbpaaaaacaaaaaaiaaaaaaplabpaaaaacaaaaaajaaaaiapkabpaaaaac
aaaaaajaabaiapkabpaaaaacaaaaaajaacaiapkaabaaaaacaaaaadiaaaaablla
ecaaaaadabaacpiaaaaaoelaaaaioekaecaaaaadacaaapiaaaaaoeiaacaioeka
ecaaaaadaaaacpiaaaaaoeiaabaioekaajaaaaadacaacbiaacaaoeiaaaaaoeka
bcaaaaaeadaacpiaacaaaaiaaaaaoeiaabaaoeiaabaaaaacaaaicpiaadaaoeia
ppppaaaafdeieefcgeabaaaaeaaaaaaafjaaaaaafkaaaaadaagabaaaaaaaaaaa
fkaaaaadaagabaaaabaaaaaafkaaaaadaagabaaaacaaaaaafibiaaaeaahabaaa
aaaaaaaaffffaaaafibiaaaeaahabaaaabaaaaaaffffaaaafibiaaaeaahabaaa
acaaaaaaffffaaaagcbaaaaddcbabaaaabaaaaaagcbaaaadmcbabaaaabaaaaaa
gfaaaaadpccabaaaaaaaaaaagiaaaaacadaaaaaaefaaaaajpcaabaaaaaaaaaaa
ogbkbaaaabaaaaaaeghobaaaacaaaaaaaagabaaaacaaaaaabbaaaaakbcaabaaa
aaaaaaaaegaobaaaaaaaaaaaaceaaaaaaaaaiadpibiaiadlicabibdhafidibdd
efaaaaajpcaabaaaabaaaaaaogbkbaaaabaaaaaaeghobaaaabaaaaaaaagabaaa
abaaaaaaefaaaaajpcaabaaaacaaaaaaegbabaaaabaaaaaaeghobaaaaaaaaaaa
aagabaaaaaaaaaaaaaaaaaaipcaabaaaabaaaaaaegaobaaaabaaaaaaegaobaia
ebaaaaaaacaaaaaadcaaaaajpccabaaaaaaaaaaaagaabaaaaaaaaaaaegaobaaa
abaaaaaaegaobaaaacaaaaaadoaaaaabejfdeheogiaaaaaaadaaaaaaaiaaaaaa
faaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaa
aaaaaaaaadaaaaaaabaaaaaaadadaaaafmaaaaaaabaaaaaaaaaaaaaaadaaaaaa
abaaaaaaamamaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklkl
epfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaa
aaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklkl"
}
}
 }
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [offsets]
"!!ARBvp1.0
PARAM c[7] = { { 2, -2, 3, -3 },
		state.matrix.mvp,
		program.local[5],
		{ 1, -1 } };
TEMP R0;
TEMP R1;
MOV R1, c[0];
MOV R0.xy, c[6];
MAD result.texcoord[1], R0.xxyy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[2], R1.xxyy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[3], R1.zzww, c[5].xyxy, vertex.texcoord[0].xyxy;
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 10 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [offsets]
"vs_2_0
def c5, 1.00000000, -1.00000000, 2.00000000, -2.00000000
def c6, 3.00000000, -3.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.xy, c4
mad oT1, c5.xxyy, r0.xyxy, v1.xyxy
mov r0.xy, c4
mov r0.zw, c4.xyxy
mad oT2, c5.zzww, r0.xyxy, v1.xyxy
mad oT3, c6.xxyy, r0.zwzw, v1.xyxy
mov oT0.xy, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
SubProgram "d3d11 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 80
Vector 16 [offsets]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0
eefiecedblembfcmhpeadakcfcnppiicngchjdjdabaaaaaapmacaaaaadaaaaaa
cmaaaaaaiaaaaaaacaabaaaaejfdeheoemaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaaebaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaafaepfdejfeejepeoaafeeffiedepepfceeaaklkl
epfdeheojiaaaaaaafaaaaaaaiaaaaaaiaaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaaimaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadamaaaa
imaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaaapaaaaaaimaaaaaaacaaaaaa
aaaaaaaaadaaaaaaadaaaaaaapaaaaaaimaaaaaaadaaaaaaaaaaaaaaadaaaaaa
aeaaaaaaapaaaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklkl
fdeieefcneabaaaaeaaaabaahfaaaaaafjaaaaaeegiocaaaaaaaaaaaacaaaaaa
fjaaaaaeegiocaaaabaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaad
dcbabaaaabaaaaaaghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaaddccabaaa
abaaaaaagfaaaaadpccabaaaacaaaaaagfaaaaadpccabaaaadaaaaaagfaaaaad
pccabaaaaeaaaaaagiaaaaacabaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaa
aaaaaaaaegiocaaaabaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaa
abaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaabaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaakpccabaaaaaaaaaaaegiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaa
egaobaaaaaaaaaaadgaaaaafdccabaaaabaaaaaaegbabaaaabaaaaaadcaaaaan
pccabaaaacaaaaaaegiecaaaaaaaaaaaabaaaaaaaceaaaaaaaaaiadpaaaaiadp
aaaaialpaaaaialpegbebaaaabaaaaaadcaaaaanpccabaaaadaaaaaaegiecaaa
aaaaaaaaabaaaaaaaceaaaaaaaaaaaeaaaaaaaeaaaaaaamaaaaaaamaegbebaaa
abaaaaaadcaaaaanpccabaaaaeaaaaaaegiecaaaaaaaaaaaabaaaaaaaceaaaaa
aaaaeaeaaaaaeaeaaaaaeamaaaaaeamaegbebaaaabaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 80
Vector 16 [offsets]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0_level_9_1
eefiecedlfpckmkimfmonkgfiaeojfndcdbnmiipabaaaaaafiaeaaaaaeaaaaaa
daaaaaaaiiabaaaageadaaaaliadaaaaebgpgodjfaabaaaafaabaaaaaaacpopp
baabaaaaeaaaaaaaacaaceaaaaaadmaaaaaadmaaaaaaceaaabaadmaaaaaaabaa
abaaabaaaaaaaaaaabaaaaaaaeaaacaaaaaaaaaaaaaaaaaaaaacpoppfbaaaaaf
agaaapkaaaaaiadpaaaaialpaaaaaaeaaaaaaamafbaaaaafahaaapkaaaaaeaea
aaaaeamaaaaaaaaaaaaaaaaabpaaaaacafaaaaiaaaaaapjabpaaaaacafaaabia
abaaapjaabaaaaacaaaaadiaabaaoekaaeaaaaaeabaaapoaaaaaeeiaagaafaka
abaaeejaaeaaaaaeacaaapoaaaaaeeiaagaapkkaabaaeejaaeaaaaaeadaaapoa
aaaaeeiaahaafakaabaaeejaafaaaaadaaaaapiaaaaaffjaadaaoekaaeaaaaae
aaaaapiaacaaoekaaaaaaajaaaaaoeiaaeaaaaaeaaaaapiaaeaaoekaaaaakkja
aaaaoeiaaeaaaaaeaaaaapiaafaaoekaaaaappjaaaaaoeiaaeaaaaaeaaaaadma
aaaappiaaaaaoekaaaaaoeiaabaaaaacaaaaammaaaaaoeiaabaaaaacaaaaadoa
abaaoejappppaaaafdeieefcneabaaaaeaaaabaahfaaaaaafjaaaaaeegiocaaa
aaaaaaaaacaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaafpaaaaadpcbabaaa
aaaaaaaafpaaaaaddcbabaaaabaaaaaaghaaaaaepccabaaaaaaaaaaaabaaaaaa
gfaaaaaddccabaaaabaaaaaagfaaaaadpccabaaaacaaaaaagfaaaaadpccabaaa
adaaaaaagfaaaaadpccabaaaaeaaaaaagiaaaaacabaaaaaadiaaaaaipcaabaaa
aaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaaabaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaacaaaaaakgbkbaaaaaaaaaaa
egaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaaegiocaaaabaaaaaaadaaaaaa
pgbpbaaaaaaaaaaaegaobaaaaaaaaaaadgaaaaafdccabaaaabaaaaaaegbabaaa
abaaaaaadcaaaaanpccabaaaacaaaaaaegiecaaaaaaaaaaaabaaaaaaaceaaaaa
aaaaiadpaaaaiadpaaaaialpaaaaialpegbebaaaabaaaaaadcaaaaanpccabaaa
adaaaaaaegiecaaaaaaaaaaaabaaaaaaaceaaaaaaaaaaaeaaaaaaaeaaaaaaama
aaaaaamaegbebaaaabaaaaaadcaaaaanpccabaaaaeaaaaaaegiecaaaaaaaaaaa
abaaaaaaaceaaaaaaaaaeaeaaaaaeaeaaaaaeamaaaaaeamaegbebaaaabaaaaaa
doaaaaabejfdeheoemaaaaaaacaaaaaaaiaaaaaadiaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaaaaaaaaaapapaaaaebaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaa
adadaaaafaepfdejfeejepeoaafeeffiedepepfceeaaklklepfdeheojiaaaaaa
afaaaaaaaiaaaaaaiaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaa
imaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadamaaaaimaaaaaaabaaaaaa
aaaaaaaaadaaaaaaacaaaaaaapaaaaaaimaaaaaaacaaaaaaaaaaaaaaadaaaaaa
adaaaaaaapaaaaaaimaaaaaaadaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaa
fdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklkl"
}
}
Program "fp" {
SubProgram "opengl " {
SetTexture 0 [_Coc] 2D 0
SetTexture 1 [_MainTex] 2D 1
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
PARAM c[2] = { { 0.30000001, 0.15000001, 0.125, 0.075000003 },
		{ 1, 0.0039215689, 1.53787e-005, 6.0308629e-008 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEMP R6;
TEX R0, fragment.texcoord[0], texture[0], 2D;
MOV R1.xy, fragment.texcoord[0];
DP4 R3.x, R0, c[1];
ADD R2, fragment.texcoord[1], -R1.xyxy;
MAD R0, R3.x, R2, fragment.texcoord[0].xyxy;
ADD R2, -R1.xyxy, fragment.texcoord[3];
MAD R2, R3.x, R2, fragment.texcoord[0].xyxy;
ADD R1, -R1.xyxy, fragment.texcoord[2];
MAD R1, R3.x, R1, fragment.texcoord[0].xyxy;
TEX R6, R2.zwzw, texture[1], 2D;
TEX R5, R2, texture[1], 2D;
TEX R2, R0.zwzw, texture[1], 2D;
TEX R4, R1.zwzw, texture[1], 2D;
TEX R3, R1, texture[1], 2D;
TEX R1, R0, texture[1], 2D;
TEX R0, fragment.texcoord[0], texture[1], 2D;
MUL R2, R2, c[0].y;
MUL R1, R1, c[0].y;
MUL R0, R0, c[0].x;
ADD R0, R0, R1;
ADD R0, R0, R2;
MUL R1, R3, c[0].z;
ADD R0, R0, R1;
MUL R2, R4, c[0].z;
ADD R0, R0, R2;
MUL R1, R5, c[0].w;
MUL R2, R6, c[0].w;
ADD R0, R0, R1;
ADD result.color, R0, R2;
END
# 29 instructions, 7 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_Coc] 2D 0
SetTexture 1 [_MainTex] 2D 1
"ps_2_0
dcl_2d s0
dcl_2d s1
def c0, 0.30000001, 0.15000001, 0.12500000, 0.07500000
def c1, 1.00000000, 0.00392157, 0.00001538, 0.00000006
dcl t0.xy
dcl t1
dcl t2
dcl t3
texld r0, t0, s0
texld r6, t0, s1
dp4 r3.x, r0, c1
mov r1.xy, t0
mov r0.yw, -r1.y
mov r0.xz, -r1.x
add r1, t1, r0
mov r0.yw, t0.y
mov r0.xz, t0.x
mad r5, r3.x, r1, r0
mov r0.y, r5.w
mov r0.x, r5.z
mov r4.xy, r0
mov r0.xz, -t0.x
mov r0.yw, -t0.y
mov r1, t3
add r1, r0, r1
mov r0.yw, t0.y
mov r0.xz, t0.x
mad r1, r3.x, r1, r0
mov r0.xz, -t0.x
mov r0.yw, -t0.y
mov r2, t2
add r2, r0, r2
mov r0.yw, t0.y
mov r0.xz, t0.x
mad r3, r3.x, r2, r0
mov r0.y, r1.w
mov r0.x, r1.z
mov r2.y, r3.w
mov r2.x, r3.z
mul r6, r6, c0.x
texld r0, r0, s1
texld r2, r2, s1
texld r4, r4, s1
texld r1, r1, s1
texld r3, r3, s1
texld r5, r5, s1
mul r5, r5, c0.y
mul r4, r4, c0.y
add_pp r5, r6, r5
add_pp r4, r5, r4
mul r3, r3, c0.z
mul r2, r2, c0.z
add_pp r3, r4, r3
add_pp r2, r3, r2
mul r1, r1, c0.w
mul r0, r0, c0.w
add_pp r1, r2, r1
add_pp r0, r1, r0
mov_pp oC0, r0
"
}
SubProgram "d3d11 " {
SetTexture 0 [_Coc] 2D 1
SetTexture 1 [_MainTex] 2D 0
"ps_4_0
eefiecedalkgakmjnlkcaafgnhhppeniedeibhijabaaaaaaomaeaaaaadaaaaaa
cmaaaaaammaaaaaaaaabaaaaejfdeheojiaaaaaaafaaaaaaaiaaaaaaiaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaimaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaaimaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apapaaaaimaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaaapapaaaaimaaaaaa
adaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapapaaaafdfgfpfagphdgjhegjgpgoaa
feeffiedepepfceeaaklklklepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklkl
fdeieefcoeadaaaaeaaaaaaapjaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaad
aagabaaaabaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaafibiaaaeaahabaaa
abaaaaaaffffaaaagcbaaaaddcbabaaaabaaaaaagcbaaaadpcbabaaaacaaaaaa
gcbaaaadpcbabaaaadaaaaaagcbaaaadpcbabaaaaeaaaaaagfaaaaadpccabaaa
aaaaaaaagiaaaaacaeaaaaaaefaaaaajpcaabaaaaaaaaaaaegbabaaaabaaaaaa
eghobaaaabaaaaaaaagabaaaaaaaaaaaaaaaaaaipcaabaaaabaaaaaaegbebaia
ebaaaaaaabaaaaaaegbobaaaacaaaaaaefaaaaajpcaabaaaacaaaaaaegbabaaa
abaaaaaaeghobaaaaaaaaaaaaagabaaaabaaaaaabbaaaaakbcaabaaaacaaaaaa
egaobaaaacaaaaaaaceaaaaaaaaaiadpibiaiadlicabibdhafidibdddcaaaaaj
pcaabaaaabaaaaaaagaabaaaacaaaaaaegaobaaaabaaaaaaegbebaaaabaaaaaa
efaaaaajpcaabaaaadaaaaaaegaabaaaabaaaaaaeghobaaaabaaaaaaaagabaaa
aaaaaaaaefaaaaajpcaabaaaabaaaaaaogakbaaaabaaaaaaeghobaaaabaaaaaa
aagabaaaaaaaaaaadiaaaaakpcaabaaaadaaaaaaegaobaaaadaaaaaaaceaaaaa
jkjjbjdojkjjbjdojkjjbjdojkjjbjdodcaaaaampcaabaaaaaaaaaaaegaobaaa
aaaaaaaaaceaaaaajkjjjjdojkjjjjdojkjjjjdojkjjjjdoegaobaaaadaaaaaa
dcaaaaampcaabaaaaaaaaaaaegaobaaaabaaaaaaaceaaaaajkjjbjdojkjjbjdo
jkjjbjdojkjjbjdoegaobaaaaaaaaaaaaaaaaaaipcaabaaaabaaaaaaegbebaia
ebaaaaaaabaaaaaaegbobaaaadaaaaaadcaaaaajpcaabaaaabaaaaaaagaabaaa
acaaaaaaegaobaaaabaaaaaaegbebaaaabaaaaaaefaaaaajpcaabaaaadaaaaaa
egaabaaaabaaaaaaeghobaaaabaaaaaaaagabaaaaaaaaaaaefaaaaajpcaabaaa
abaaaaaaogakbaaaabaaaaaaeghobaaaabaaaaaaaagabaaaaaaaaaaadcaaaaam
pcaabaaaaaaaaaaaegaobaaaadaaaaaaaceaaaaaaaaaaadoaaaaaadoaaaaaado
aaaaaadoegaobaaaaaaaaaaadcaaaaampcaabaaaaaaaaaaaegaobaaaabaaaaaa
aceaaaaaaaaaaadoaaaaaadoaaaaaadoaaaaaadoegaobaaaaaaaaaaaaaaaaaai
pcaabaaaabaaaaaaegbebaiaebaaaaaaabaaaaaaegbobaaaaeaaaaaadcaaaaaj
pcaabaaaabaaaaaaagaabaaaacaaaaaaegaobaaaabaaaaaaegbebaaaabaaaaaa
efaaaaajpcaabaaaacaaaaaaegaabaaaabaaaaaaeghobaaaabaaaaaaaagabaaa
aaaaaaaaefaaaaajpcaabaaaabaaaaaaogakbaaaabaaaaaaeghobaaaabaaaaaa
aagabaaaaaaaaaaadcaaaaampcaabaaaaaaaaaaaegaobaaaacaaaaaaaceaaaaa
jkjjjjdnjkjjjjdnjkjjjjdnjkjjjjdnegaobaaaaaaaaaaadcaaaaampccabaaa
aaaaaaaaegaobaaaabaaaaaaaceaaaaajkjjjjdnjkjjjjdnjkjjjjdnjkjjjjdn
egaobaaaaaaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
SetTexture 0 [_Coc] 2D 1
SetTexture 1 [_MainTex] 2D 0
"ps_4_0_level_9_1
eefiecedjcmghimddljndgajkbfecfemijfjfhpeabaaaaaapeahaaaaaeaaaaaa
daaaaaaadeadaaaacaahaaaamaahaaaaebgpgodjpmacaaaapmacaaaaaaacpppp
naacaaaacmaaaaaaaaaacmaaaaaacmaaaaaacmaaacaaceaaaaaacmaaabaaaaaa
aaababaaaaacppppfbaaaaafaaaaapkaaaaaiadpibiaiadlicabibdhafidibdd
fbaaaaafabaaapkajkjjbjdojkjjjjdoaaaaaadojkjjjjdnbpaaaaacaaaaaaia
aaaaadlabpaaaaacaaaaaaiaabaaaplabpaaaaacaaaaaaiaacaaaplabpaaaaac
aaaaaaiaadaaaplabpaaaaacaaaaaajaaaaiapkabpaaaaacaaaaaajaabaiapka
abaaaaacaaaaadiaaaaaoelaacaaaaadabaaakiaaaaaffibabaaoelaacaaaaad
abaaafiaaaaaaaibabaaoelaecaaaaadacaaapiaaaaaoelaabaioekaajaaaaad
aaaaceiaacaaoeiaaaaaoekaaeaaaaaeacaaakiaaaaakkiaabaaoeiaaaaaffla
aeaaaaaeacaaafiaaaaakkiaabaaoeiaaaaaaalaabaaaaacabaaabiaacaakkia
abaaaaacabaaaciaacaappiaacaaaaadadaaakiaaaaaffibacaaoelaacaaaaad
adaaafiaaaaaaaibacaaoelaaeaaaaaeaeaaakiaaaaakkiaadaaoeiaaaaaffla
aeaaaaaeaeaaafiaaaaakkiaadaaoeiaaaaaaalaabaaaaacadaaabiaaeaakkia
abaaaaacadaaaciaaeaappiaacaaaaadafaaakiaaaaaffibadaaoelaacaaaaad
afaaafiaaaaaaaibadaaoelaaeaaaaaeagaaakiaaaaakkiaafaaoeiaaaaaffla
aeaaaaaeagaaafiaaaaakkiaafaaoeiaaaaaaalaabaaaaacaaaaabiaagaakkia
abaaaaacaaaaaciaagaappiaecaaaaadafaaapiaaaaaoelaaaaioekaecaaaaad
acaaapiaacaaoeiaaaaioekaecaaaaadabaaapiaabaaoeiaaaaioekaecaaaaad
aeaaapiaaeaaoeiaaaaioekaecaaaaadadaaapiaadaaoeiaaaaioekaecaaaaad
agaaapiaagaaoeiaaaaioekaecaaaaadaaaaapiaaaaaoeiaaaaioekaafaaaaad
acaaapiaacaaoeiaabaaaakaaeaaaaaeacaacpiaafaaoeiaabaaffkaacaaoeia
aeaaaaaeabaacpiaabaaoeiaabaaaakaacaaoeiaaeaaaaaeabaacpiaaeaaoeia
abaakkkaabaaoeiaaeaaaaaeabaacpiaadaaoeiaabaakkkaabaaoeiaaeaaaaae
abaacpiaagaaoeiaabaappkaabaaoeiaaeaaaaaeaaaacpiaaaaaoeiaabaappka
abaaoeiaabaaaaacaaaicpiaaaaaoeiappppaaaafdeieefcoeadaaaaeaaaaaaa
pjaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaadaagabaaaabaaaaaafibiaaae
aahabaaaaaaaaaaaffffaaaafibiaaaeaahabaaaabaaaaaaffffaaaagcbaaaad
dcbabaaaabaaaaaagcbaaaadpcbabaaaacaaaaaagcbaaaadpcbabaaaadaaaaaa
gcbaaaadpcbabaaaaeaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacaeaaaaaa
efaaaaajpcaabaaaaaaaaaaaegbabaaaabaaaaaaeghobaaaabaaaaaaaagabaaa
aaaaaaaaaaaaaaaipcaabaaaabaaaaaaegbebaiaebaaaaaaabaaaaaaegbobaaa
acaaaaaaefaaaaajpcaabaaaacaaaaaaegbabaaaabaaaaaaeghobaaaaaaaaaaa
aagabaaaabaaaaaabbaaaaakbcaabaaaacaaaaaaegaobaaaacaaaaaaaceaaaaa
aaaaiadpibiaiadlicabibdhafidibdddcaaaaajpcaabaaaabaaaaaaagaabaaa
acaaaaaaegaobaaaabaaaaaaegbebaaaabaaaaaaefaaaaajpcaabaaaadaaaaaa
egaabaaaabaaaaaaeghobaaaabaaaaaaaagabaaaaaaaaaaaefaaaaajpcaabaaa
abaaaaaaogakbaaaabaaaaaaeghobaaaabaaaaaaaagabaaaaaaaaaaadiaaaaak
pcaabaaaadaaaaaaegaobaaaadaaaaaaaceaaaaajkjjbjdojkjjbjdojkjjbjdo
jkjjbjdodcaaaaampcaabaaaaaaaaaaaegaobaaaaaaaaaaaaceaaaaajkjjjjdo
jkjjjjdojkjjjjdojkjjjjdoegaobaaaadaaaaaadcaaaaampcaabaaaaaaaaaaa
egaobaaaabaaaaaaaceaaaaajkjjbjdojkjjbjdojkjjbjdojkjjbjdoegaobaaa
aaaaaaaaaaaaaaaipcaabaaaabaaaaaaegbebaiaebaaaaaaabaaaaaaegbobaaa
adaaaaaadcaaaaajpcaabaaaabaaaaaaagaabaaaacaaaaaaegaobaaaabaaaaaa
egbebaaaabaaaaaaefaaaaajpcaabaaaadaaaaaaegaabaaaabaaaaaaeghobaaa
abaaaaaaaagabaaaaaaaaaaaefaaaaajpcaabaaaabaaaaaaogakbaaaabaaaaaa
eghobaaaabaaaaaaaagabaaaaaaaaaaadcaaaaampcaabaaaaaaaaaaaegaobaaa
adaaaaaaaceaaaaaaaaaaadoaaaaaadoaaaaaadoaaaaaadoegaobaaaaaaaaaaa
dcaaaaampcaabaaaaaaaaaaaegaobaaaabaaaaaaaceaaaaaaaaaaadoaaaaaado
aaaaaadoaaaaaadoegaobaaaaaaaaaaaaaaaaaaipcaabaaaabaaaaaaegbebaia
ebaaaaaaabaaaaaaegbobaaaaeaaaaaadcaaaaajpcaabaaaabaaaaaaagaabaaa
acaaaaaaegaobaaaabaaaaaaegbebaaaabaaaaaaefaaaaajpcaabaaaacaaaaaa
egaabaaaabaaaaaaeghobaaaabaaaaaaaagabaaaaaaaaaaaefaaaaajpcaabaaa
abaaaaaaogakbaaaabaaaaaaeghobaaaabaaaaaaaagabaaaaaaaaaaadcaaaaam
pcaabaaaaaaaaaaaegaobaaaacaaaaaaaceaaaaajkjjjjdnjkjjjjdnjkjjjjdn
jkjjjjdnegaobaaaaaaaaaaadcaaaaampccabaaaaaaaaaaaegaobaaaabaaaaaa
aceaaaaajkjjjjdnjkjjjjdnjkjjjjdnjkjjjjdnegaobaaaaaaaaaaadoaaaaab
ejfdeheojiaaaaaaafaaaaaaaiaaaaaaiaaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaaimaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadadaaaa
imaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaaapapaaaaimaaaaaaacaaaaaa
aaaaaaaaadaaaaaaadaaaaaaapapaaaaimaaaaaaadaaaaaaaaaaaaaaadaaaaaa
aeaaaaaaapapaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklkl
epfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaa
aaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklkl"
}
}
 }
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [offsets]
"!!ARBvp1.0
PARAM c[7] = { { 2, -2, 3, -3 },
		state.matrix.mvp,
		program.local[5],
		{ 1, -1 } };
TEMP R0;
TEMP R1;
MOV R1, c[0];
MOV R0.xy, c[6];
MAD result.texcoord[1], R0.xxyy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[2], R1.xxyy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[3], R1.zzww, c[5].xyxy, vertex.texcoord[0].xyxy;
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 10 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [offsets]
"vs_2_0
def c5, 1.00000000, -1.00000000, 2.00000000, -2.00000000
def c6, 3.00000000, -3.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.xy, c4
mad oT1, c5.xxyy, r0.xyxy, v1.xyxy
mov r0.xy, c4
mov r0.zw, c4.xyxy
mad oT2, c5.zzww, r0.xyxy, v1.xyxy
mad oT3, c6.xxyy, r0.zwzw, v1.xyxy
mov oT0.xy, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
SubProgram "d3d11 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 80
Vector 16 [offsets]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0
eefiecedblembfcmhpeadakcfcnppiicngchjdjdabaaaaaapmacaaaaadaaaaaa
cmaaaaaaiaaaaaaacaabaaaaejfdeheoemaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaaebaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaafaepfdejfeejepeoaafeeffiedepepfceeaaklkl
epfdeheojiaaaaaaafaaaaaaaiaaaaaaiaaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaaimaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadamaaaa
imaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaaapaaaaaaimaaaaaaacaaaaaa
aaaaaaaaadaaaaaaadaaaaaaapaaaaaaimaaaaaaadaaaaaaaaaaaaaaadaaaaaa
aeaaaaaaapaaaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklkl
fdeieefcneabaaaaeaaaabaahfaaaaaafjaaaaaeegiocaaaaaaaaaaaacaaaaaa
fjaaaaaeegiocaaaabaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaad
dcbabaaaabaaaaaaghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaaddccabaaa
abaaaaaagfaaaaadpccabaaaacaaaaaagfaaaaadpccabaaaadaaaaaagfaaaaad
pccabaaaaeaaaaaagiaaaaacabaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaa
aaaaaaaaegiocaaaabaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaa
abaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaabaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaakpccabaaaaaaaaaaaegiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaa
egaobaaaaaaaaaaadgaaaaafdccabaaaabaaaaaaegbabaaaabaaaaaadcaaaaan
pccabaaaacaaaaaaegiecaaaaaaaaaaaabaaaaaaaceaaaaaaaaaiadpaaaaiadp
aaaaialpaaaaialpegbebaaaabaaaaaadcaaaaanpccabaaaadaaaaaaegiecaaa
aaaaaaaaabaaaaaaaceaaaaaaaaaaaeaaaaaaaeaaaaaaamaaaaaaamaegbebaaa
abaaaaaadcaaaaanpccabaaaaeaaaaaaegiecaaaaaaaaaaaabaaaaaaaceaaaaa
aaaaeaeaaaaaeaeaaaaaeamaaaaaeamaegbebaaaabaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 80
Vector 16 [offsets]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0_level_9_1
eefiecedlfpckmkimfmonkgfiaeojfndcdbnmiipabaaaaaafiaeaaaaaeaaaaaa
daaaaaaaiiabaaaageadaaaaliadaaaaebgpgodjfaabaaaafaabaaaaaaacpopp
baabaaaaeaaaaaaaacaaceaaaaaadmaaaaaadmaaaaaaceaaabaadmaaaaaaabaa
abaaabaaaaaaaaaaabaaaaaaaeaaacaaaaaaaaaaaaaaaaaaaaacpoppfbaaaaaf
agaaapkaaaaaiadpaaaaialpaaaaaaeaaaaaaamafbaaaaafahaaapkaaaaaeaea
aaaaeamaaaaaaaaaaaaaaaaabpaaaaacafaaaaiaaaaaapjabpaaaaacafaaabia
abaaapjaabaaaaacaaaaadiaabaaoekaaeaaaaaeabaaapoaaaaaeeiaagaafaka
abaaeejaaeaaaaaeacaaapoaaaaaeeiaagaapkkaabaaeejaaeaaaaaeadaaapoa
aaaaeeiaahaafakaabaaeejaafaaaaadaaaaapiaaaaaffjaadaaoekaaeaaaaae
aaaaapiaacaaoekaaaaaaajaaaaaoeiaaeaaaaaeaaaaapiaaeaaoekaaaaakkja
aaaaoeiaaeaaaaaeaaaaapiaafaaoekaaaaappjaaaaaoeiaaeaaaaaeaaaaadma
aaaappiaaaaaoekaaaaaoeiaabaaaaacaaaaammaaaaaoeiaabaaaaacaaaaadoa
abaaoejappppaaaafdeieefcneabaaaaeaaaabaahfaaaaaafjaaaaaeegiocaaa
aaaaaaaaacaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaafpaaaaadpcbabaaa
aaaaaaaafpaaaaaddcbabaaaabaaaaaaghaaaaaepccabaaaaaaaaaaaabaaaaaa
gfaaaaaddccabaaaabaaaaaagfaaaaadpccabaaaacaaaaaagfaaaaadpccabaaa
adaaaaaagfaaaaadpccabaaaaeaaaaaagiaaaaacabaaaaaadiaaaaaipcaabaaa
aaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaaabaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaacaaaaaakgbkbaaaaaaaaaaa
egaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaaegiocaaaabaaaaaaadaaaaaa
pgbpbaaaaaaaaaaaegaobaaaaaaaaaaadgaaaaafdccabaaaabaaaaaaegbabaaa
abaaaaaadcaaaaanpccabaaaacaaaaaaegiecaaaaaaaaaaaabaaaaaaaceaaaaa
aaaaiadpaaaaiadpaaaaialpaaaaialpegbebaaaabaaaaaadcaaaaanpccabaaa
adaaaaaaegiecaaaaaaaaaaaabaaaaaaaceaaaaaaaaaaaeaaaaaaaeaaaaaaama
aaaaaamaegbebaaaabaaaaaadcaaaaanpccabaaaaeaaaaaaegiecaaaaaaaaaaa
abaaaaaaaceaaaaaaaaaeaeaaaaaeaeaaaaaeamaaaaaeamaegbebaaaabaaaaaa
doaaaaabejfdeheoemaaaaaaacaaaaaaaiaaaaaadiaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaaaaaaaaaapapaaaaebaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaa
adadaaaafaepfdejfeejepeoaafeeffiedepepfceeaaklklepfdeheojiaaaaaa
afaaaaaaaiaaaaaaiaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaa
imaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadamaaaaimaaaaaaabaaaaaa
aaaaaaaaadaaaaaaacaaaaaaapaaaaaaimaaaaaaacaaaaaaaaaaaaaaadaaaaaa
adaaaaaaapaaaaaaimaaaaaaadaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaa
fdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklkl"
}
}
Program "fp" {
SubProgram "opengl " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Coc] 2D 1
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
PARAM c[1] = { { 0.30000001, 0.15000001, 0.125, 0.075000003 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEMP R6;
TEMP R7;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R2, fragment.texcoord[1].zwzw, texture[0], 2D;
TEX R1, fragment.texcoord[1], texture[0], 2D;
TEX R7, fragment.texcoord[0], texture[1], 2D;
TEX R6, fragment.texcoord[3].zwzw, texture[0], 2D;
TEX R5, fragment.texcoord[3], texture[0], 2D;
TEX R4, fragment.texcoord[2].zwzw, texture[0], 2D;
TEX R3, fragment.texcoord[2], texture[0], 2D;
MUL R2, R2, c[0].y;
MUL R1, R1, c[0].y;
MUL R0, R0, c[0].x;
ADD R0, R0, R1;
ADD R0, R0, R2;
MUL R1, R3, c[0].z;
ADD R0, R0, R1;
MUL R2, R4, c[0].z;
ADD R0, R0, R2;
MUL R1, R5, c[0].w;
MUL R2, R6, c[0].w;
ADD R0, R0, R1;
ADD R0, R0, R2;
MAX result.color, R0, R7;
END
# 22 instructions, 8 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Coc] 2D 1
"ps_2_0
dcl_2d s0
dcl_2d s1
def c0, 0.30000001, 0.15000001, 0.12500000, 0.07500000
dcl t0.xy
dcl t1
dcl t2
dcl t3
texld r2, t3, s0
texld r4, t2, s0
texld r7, t0, s0
texld r6, t1, s0
mov r0.y, t1.w
mov r0.x, t1.z
mov r5.xy, r0
mov r0.y, t2.w
mov r0.x, t2.z
mov r3.xy, r0
mov r0.y, t3.w
mov r0.x, t3.z
mov r1.xy, r0
mul r6, r6, c0.y
mul r7, r7, c0.x
add_pp r6, r7, r6
mul r4, r4, c0.z
mul r2, r2, c0.w
texld r0, t0, s1
texld r1, r1, s0
texld r3, r3, s0
texld r5, r5, s0
mul r5, r5, c0.y
add_pp r5, r6, r5
add_pp r4, r5, r4
mul r3, r3, c0.z
add_pp r3, r4, r3
add_pp r2, r3, r2
mul r1, r1, c0.w
add_pp r1, r2, r1
max_pp r0, r1, r0
mov_pp oC0, r0
"
}
SubProgram "d3d11 " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Coc] 2D 1
"ps_4_0
eefiecedjnpdbjefdeogjeaeoddpaodcdfhjeifkabaaaaaabeaeaaaaadaaaaaa
cmaaaaaammaaaaaaaaabaaaaejfdeheojiaaaaaaafaaaaaaaiaaaaaaiaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaimaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaaimaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apapaaaaimaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaaapapaaaaimaaaaaa
adaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapapaaaafdfgfpfagphdgjhegjgpgoaa
feeffiedepepfceeaaklklklepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklkl
fdeieefcamadaaaaeaaaaaaamdaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaad
aagabaaaabaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaafibiaaaeaahabaaa
abaaaaaaffffaaaagcbaaaaddcbabaaaabaaaaaagcbaaaadpcbabaaaacaaaaaa
gcbaaaadpcbabaaaadaaaaaagcbaaaadpcbabaaaaeaaaaaagfaaaaadpccabaaa
aaaaaaaagiaaaaacacaaaaaaefaaaaajpcaabaaaaaaaaaaaegbabaaaacaaaaaa
eghobaaaaaaaaaaaaagabaaaaaaaaaaadiaaaaakpcaabaaaaaaaaaaaegaobaaa
aaaaaaaaaceaaaaajkjjbjdojkjjbjdojkjjbjdojkjjbjdoefaaaaajpcaabaaa
abaaaaaaegbabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadcaaaaam
pcaabaaaaaaaaaaaegaobaaaabaaaaaaaceaaaaajkjjjjdojkjjjjdojkjjjjdo
jkjjjjdoegaobaaaaaaaaaaaefaaaaajpcaabaaaabaaaaaaogbkbaaaacaaaaaa
eghobaaaaaaaaaaaaagabaaaaaaaaaaadcaaaaampcaabaaaaaaaaaaaegaobaaa
abaaaaaaaceaaaaajkjjbjdojkjjbjdojkjjbjdojkjjbjdoegaobaaaaaaaaaaa
efaaaaajpcaabaaaabaaaaaaegbabaaaadaaaaaaeghobaaaaaaaaaaaaagabaaa
aaaaaaaadcaaaaampcaabaaaaaaaaaaaegaobaaaabaaaaaaaceaaaaaaaaaaado
aaaaaadoaaaaaadoaaaaaadoegaobaaaaaaaaaaaefaaaaajpcaabaaaabaaaaaa
ogbkbaaaadaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadcaaaaampcaabaaa
aaaaaaaaegaobaaaabaaaaaaaceaaaaaaaaaaadoaaaaaadoaaaaaadoaaaaaado
egaobaaaaaaaaaaaefaaaaajpcaabaaaabaaaaaaegbabaaaaeaaaaaaeghobaaa
aaaaaaaaaagabaaaaaaaaaaadcaaaaampcaabaaaaaaaaaaaegaobaaaabaaaaaa
aceaaaaajkjjjjdnjkjjjjdnjkjjjjdnjkjjjjdnegaobaaaaaaaaaaaefaaaaaj
pcaabaaaabaaaaaaogbkbaaaaeaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaa
dcaaaaampcaabaaaaaaaaaaaegaobaaaabaaaaaaaceaaaaajkjjjjdnjkjjjjdn
jkjjjjdnjkjjjjdnegaobaaaaaaaaaaaefaaaaajpcaabaaaabaaaaaaegbabaaa
abaaaaaaeghobaaaabaaaaaaaagabaaaabaaaaaadeaaaaahpccabaaaaaaaaaaa
egaobaaaaaaaaaaaegaobaaaabaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Coc] 2D 1
"ps_4_0_level_9_1
eefiecedibbbmbmoooeielfhdfabbagcmabaidbbabaaaaaacaagaaaaaeaaaaaa
daaaaaaadiacaaaaemafaaaaomafaaaaebgpgodjaaacaaaaaaacaaaaaaacpppp
neabaaaacmaaaaaaaaaacmaaaaaacmaaaaaacmaaacaaceaaaaaacmaaaaaaaaaa
abababaaaaacppppfbaaaaafaaaaapkajkjjbjdojkjjjjdoaaaaaadojkjjjjdn
bpaaaaacaaaaaaiaaaaaadlabpaaaaacaaaaaaiaabaaaplabpaaaaacaaaaaaia
acaaaplabpaaaaacaaaaaaiaadaaaplabpaaaaacaaaaaajaaaaiapkabpaaaaac
aaaaaajaabaiapkaabaaaaacaaaaabiaabaakklaabaaaaacaaaaaciaabaappla
abaaaaacabaaabiaacaakklaabaaaaacabaaaciaacaapplaabaaaaacacaaabia
adaakklaabaaaaacacaaaciaadaapplaecaaaaadadaaapiaabaaoelaaaaioeka
ecaaaaadaeaaapiaaaaaoelaaaaioekaecaaaaadaaaaapiaaaaaoeiaaaaioeka
ecaaaaadafaaapiaacaaoelaaaaioekaecaaaaadabaaapiaabaaoeiaaaaioeka
ecaaaaadagaaapiaadaaoelaaaaioekaecaaaaadacaaapiaacaaoeiaaaaioeka
ecaaaaadahaacpiaaaaaoelaabaioekaafaaaaadadaaapiaadaaoeiaaaaaaaka
aeaaaaaeadaacpiaaeaaoeiaaaaaffkaadaaoeiaaeaaaaaeaaaacpiaaaaaoeia
aaaaaakaadaaoeiaaeaaaaaeaaaacpiaafaaoeiaaaaakkkaaaaaoeiaaeaaaaae
aaaacpiaabaaoeiaaaaakkkaaaaaoeiaaeaaaaaeaaaacpiaagaaoeiaaaaappka
aaaaoeiaaeaaaaaeaaaacpiaacaaoeiaaaaappkaaaaaoeiaalaaaaadabaacpia
aaaaoeiaahaaoeiaabaaaaacaaaicpiaabaaoeiappppaaaafdeieefcamadaaaa
eaaaaaaamdaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaadaagabaaaabaaaaaa
fibiaaaeaahabaaaaaaaaaaaffffaaaafibiaaaeaahabaaaabaaaaaaffffaaaa
gcbaaaaddcbabaaaabaaaaaagcbaaaadpcbabaaaacaaaaaagcbaaaadpcbabaaa
adaaaaaagcbaaaadpcbabaaaaeaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaac
acaaaaaaefaaaaajpcaabaaaaaaaaaaaegbabaaaacaaaaaaeghobaaaaaaaaaaa
aagabaaaaaaaaaaadiaaaaakpcaabaaaaaaaaaaaegaobaaaaaaaaaaaaceaaaaa
jkjjbjdojkjjbjdojkjjbjdojkjjbjdoefaaaaajpcaabaaaabaaaaaaegbabaaa
abaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadcaaaaampcaabaaaaaaaaaaa
egaobaaaabaaaaaaaceaaaaajkjjjjdojkjjjjdojkjjjjdojkjjjjdoegaobaaa
aaaaaaaaefaaaaajpcaabaaaabaaaaaaogbkbaaaacaaaaaaeghobaaaaaaaaaaa
aagabaaaaaaaaaaadcaaaaampcaabaaaaaaaaaaaegaobaaaabaaaaaaaceaaaaa
jkjjbjdojkjjbjdojkjjbjdojkjjbjdoegaobaaaaaaaaaaaefaaaaajpcaabaaa
abaaaaaaegbabaaaadaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadcaaaaam
pcaabaaaaaaaaaaaegaobaaaabaaaaaaaceaaaaaaaaaaadoaaaaaadoaaaaaado
aaaaaadoegaobaaaaaaaaaaaefaaaaajpcaabaaaabaaaaaaogbkbaaaadaaaaaa
eghobaaaaaaaaaaaaagabaaaaaaaaaaadcaaaaampcaabaaaaaaaaaaaegaobaaa
abaaaaaaaceaaaaaaaaaaadoaaaaaadoaaaaaadoaaaaaadoegaobaaaaaaaaaaa
efaaaaajpcaabaaaabaaaaaaegbabaaaaeaaaaaaeghobaaaaaaaaaaaaagabaaa
aaaaaaaadcaaaaampcaabaaaaaaaaaaaegaobaaaabaaaaaaaceaaaaajkjjjjdn
jkjjjjdnjkjjjjdnjkjjjjdnegaobaaaaaaaaaaaefaaaaajpcaabaaaabaaaaaa
ogbkbaaaaeaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadcaaaaampcaabaaa
aaaaaaaaegaobaaaabaaaaaaaceaaaaajkjjjjdnjkjjjjdnjkjjjjdnjkjjjjdn
egaobaaaaaaaaaaaefaaaaajpcaabaaaabaaaaaaegbabaaaabaaaaaaeghobaaa
abaaaaaaaagabaaaabaaaaaadeaaaaahpccabaaaaaaaaaaaegaobaaaaaaaaaaa
egaobaaaabaaaaaadoaaaaabejfdeheojiaaaaaaafaaaaaaaiaaaaaaiaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaimaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaaimaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apapaaaaimaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaaapapaaaaimaaaaaa
adaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapapaaaafdfgfpfagphdgjhegjgpgoaa
feeffiedepepfceeaaklklklepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklkl
"
}
}
 }
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
"!!ARBvp1.0
PARAM c[5] = { program.local[0],
		state.matrix.mvp };
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 5 instructions, 0 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_TexelSize]
"vs_2_0
def c5, 0.00000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.x, c5
slt r0.x, c4.y, r0
max r0.x, -r0, r0
slt r0.x, c5, r0
add r0.y, -r0.x, c5
mul r0.z, v1.y, r0.y
add r0.y, -v1, c5
mad oT1.y, r0.x, r0, r0.z
mov oT0.xy, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
mov oT1.x, v1
"
}
SubProgram "d3d11 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 80
Vector 32 [_MainTex_TexelSize]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0
eefiecedgalbggkgcijkbbogllmajnckopibikhoabaaaaaahmacaaaaadaaaaaa
cmaaaaaaiaaaaaaapaaaaaaaejfdeheoemaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaaebaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaafaepfdejfeejepeoaafeeffiedepepfceeaaklkl
epfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadamaaaa
fmaaaaaaabaaaaaaaaaaaaaaadaaaaaaabaaaaaaamadaaaafdfgfpfagphdgjhe
gjgpgoaafeeffiedepepfceeaaklklklfdeieefcieabaaaaeaaaabaagbaaaaaa
fjaaaaaeegiocaaaaaaaaaaaadaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaa
fpaaaaadpcbabaaaaaaaaaaafpaaaaaddcbabaaaabaaaaaaghaaaaaepccabaaa
aaaaaaaaabaaaaaagfaaaaaddccabaaaabaaaaaagfaaaaadmccabaaaabaaaaaa
giaaaaacabaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaa
abaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaa
agbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaa
abaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaa
aaaaaaaaegiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaa
dbaaaaaibcaabaaaaaaaaaaabkiacaaaaaaaaaaaacaaaaaaabeaaaaaaaaaaaaa
aaaaaaaiccaabaaaaaaaaaaabkbabaiaebaaaaaaabaaaaaaabeaaaaaaaaaiadp
dhaaaaajiccabaaaabaaaaaaakaabaaaaaaaaaaabkaabaaaaaaaaaaabkbabaaa
abaaaaaadgaaaaafhccabaaaabaaaaaaegbabaaaabaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 80
Vector 32 [_MainTex_TexelSize]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0_level_9_1
eefiecedkfiplpebenglpjinfbgeeejidigadnefabaaaaaalmadaaaaaeaaaaaa
daaaaaaagmabaaaapiacaaaaemadaaaaebgpgodjdeabaaaadeabaaaaaaacpopp
peaaaaaaeaaaaaaaacaaceaaaaaadmaaaaaadmaaaaaaceaaabaadmaaaaaaacaa
abaaabaaaaaaaaaaabaaaaaaaeaaacaaaaaaaaaaaaaaaaaaaaacpoppfbaaaaaf
agaaapkaaaaaaaaaaaaaaamaaaaaiadpaaaaaaaabpaaaaacafaaaaiaaaaaapja
bpaaaaacafaaabiaabaaapjaabaaaaacaaaaabiaagaaaakaamaaaaadaaaaabia
abaaffkaaaaaaaiaaeaaaaaeaaaaaciaabaaffjaagaaffkaagaakkkaaeaaaaae
aaaaaeoaaaaaaaiaaaaaffiaabaaffjaafaaaaadaaaaapiaaaaaffjaadaaoeka
aeaaaaaeaaaaapiaacaaoekaaaaaaajaaaaaoeiaaeaaaaaeaaaaapiaaeaaoeka
aaaakkjaaaaaoeiaaeaaaaaeaaaaapiaafaaoekaaaaappjaaaaaoeiaaeaaaaae
aaaaadmaaaaappiaaaaaoekaaaaaoeiaabaaaaacaaaaammaaaaaoeiaabaaaaac
aaaaaloaabaacejappppaaaafdeieefcieabaaaaeaaaabaagbaaaaaafjaaaaae
egiocaaaaaaaaaaaadaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaafpaaaaad
pcbabaaaaaaaaaaafpaaaaaddcbabaaaabaaaaaaghaaaaaepccabaaaaaaaaaaa
abaaaaaagfaaaaaddccabaaaabaaaaaagfaaaaadmccabaaaabaaaaaagiaaaaac
abaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaa
abaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaa
aaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaa
acaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaa
egiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadbaaaaai
bcaabaaaaaaaaaaabkiacaaaaaaaaaaaacaaaaaaabeaaaaaaaaaaaaaaaaaaaai
ccaabaaaaaaaaaaabkbabaiaebaaaaaaabaaaaaaabeaaaaaaaaaiadpdhaaaaaj
iccabaaaabaaaaaaakaabaaaaaaaaaaabkaabaaaaaaaaaaabkbabaaaabaaaaaa
dgaaaaafhccabaaaabaaaaaaegbabaaaabaaaaaadoaaaaabejfdeheoemaaaaaa
acaaaaaaaiaaaaaadiaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaa
ebaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadadaaaafaepfdejfeejepeo
aafeeffiedepepfceeaaklklepfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadamaaaafmaaaaaaabaaaaaaaaaaaaaaadaaaaaaabaaaaaa
amadaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklkl"
}
}
Program "fp" {
SubProgram "opengl " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Blurred] 2D 1
SetTexture 2 [_Coc] 2D 2
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
PARAM c[2] = { { 1, 0.0039215689, 1.53787e-005, 6.0308629e-008 },
		{ 0, 1, 0.5 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEX R1, fragment.texcoord[0], texture[2], 2D;
TEX R2, fragment.texcoord[0], texture[1], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
ADD R2, R2, c[1].xyxy;
MAD R2, R2, c[1].z, -R0;
DP4 R1.x, R1, c[0];
MAD result.color, R1.x, R2, R0;
END
# 7 instructions, 3 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Blurred] 2D 1
SetTexture 2 [_Coc] 2D 2
"ps_2_0
dcl_2d s0
dcl_2d s1
dcl_2d s2
def c0, 0.00000000, 1.00000000, 0.50000000, 0
def c1, 1.00000000, 0.00392157, 0.00001538, 0.00000006
dcl t0.xy
dcl t1.xy
texld r0, t1, s2
texld r2, t1, s1
texld r3, t0, s0
mov r1.yw, c0.y
mov r1.xz, c0.x
add_pp r1, r2, r1
mad_pp r1, r1, c0.z, -r3
dp4 r0.x, r0, c1
mad_pp r0, r0.x, r1, r3
mov_pp oC0, r0
"
}
SubProgram "d3d11 " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Blurred] 2D 1
SetTexture 2 [_Coc] 2D 2
"ps_4_0
eefiecedoebhbknfjcjejmcceinlhkackekobgagabaaaaaahiacaaaaadaaaaaa
cmaaaaaajmaaaaaanaaaaaaaejfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaafmaaaaaaabaaaaaaaaaaaaaaadaaaaaaabaaaaaa
amamaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklklepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklklfdeieefckaabaaaaeaaaaaaagiaaaaaa
fkaaaaadaagabaaaaaaaaaaafkaaaaadaagabaaaabaaaaaafkaaaaadaagabaaa
acaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaafibiaaaeaahabaaaabaaaaaa
ffffaaaafibiaaaeaahabaaaacaaaaaaffffaaaagcbaaaaddcbabaaaabaaaaaa
gcbaaaadmcbabaaaabaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacadaaaaaa
efaaaaajpcaabaaaaaaaaaaaogbkbaaaabaaaaaaeghobaaaacaaaaaaaagabaaa
acaaaaaabbaaaaakbcaabaaaaaaaaaaaegaobaaaaaaaaaaaaceaaaaaaaaaiadp
ibiaiadlicabibdhafidibddefaaaaajpcaabaaaabaaaaaaogbkbaaaabaaaaaa
eghobaaaabaaaaaaaagabaaaabaaaaaaaaaaaaakpcaabaaaabaaaaaaegaobaaa
abaaaaaaaceaaaaaaaaaaaaaaaaaiadpaaaaaaaaaaaaiadpefaaaaajpcaabaaa
acaaaaaaegbabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadcaaaaan
pcaabaaaabaaaaaaegaobaaaabaaaaaaaceaaaaaaaaaaadpaaaaaadpaaaaaadp
aaaaaadpegaobaiaebaaaaaaacaaaaaadcaaaaajpccabaaaaaaaaaaaagaabaaa
aaaaaaaaegaobaaaabaaaaaaegaobaaaacaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Blurred] 2D 1
SetTexture 2 [_Coc] 2D 2
"ps_4_0_level_9_1
eefiecedkglbjipdjcpcnlfhpefffelkabdcaoenabaaaaaameadaaaaaeaaaaaa
daaaaaaahiabaaaacaadaaaajaadaaaaebgpgodjeaabaaaaeaabaaaaaaacpppp
baabaaaadaaaaaaaaaaadaaaaaaadaaaaaaadaaaadaaceaaaaaadaaaaaaaaaaa
abababaaacacacaaaaacppppfbaaaaafaaaaapkaaaaaaaaaaaaaiadpaaaaaaaa
aaaaiadpfbaaaaafabaaapkaaaaaaadpaaaaaaaaaaaaaaaaaaaaaaaafbaaaaaf
acaaapkaaaaaiadpibiaiadlicabibdhafidibddbpaaaaacaaaaaaiaaaaaapla
bpaaaaacaaaaaajaaaaiapkabpaaaaacaaaaaajaabaiapkabpaaaaacaaaaaaja
acaiapkaabaaaaacaaaaadiaaaaabllaecaaaaadabaaapiaaaaaoeiaacaioeka
ecaaaaadaaaacpiaaaaaoeiaabaioekaecaaaaadacaacpiaaaaaoelaaaaioeka
acaaaaadaaaacpiaaaaaoeiaaaaaoekaajaaaaadabaacbiaabaaoeiaacaaoeka
aeaaaaaeaaaacpiaaaaaoeiaabaaaakaacaaoeibaeaaaaaeaaaacpiaabaaaaia
aaaaoeiaacaaoeiaabaaaaacaaaicpiaaaaaoeiappppaaaafdeieefckaabaaaa
eaaaaaaagiaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaadaagabaaaabaaaaaa
fkaaaaadaagabaaaacaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaafibiaaae
aahabaaaabaaaaaaffffaaaafibiaaaeaahabaaaacaaaaaaffffaaaagcbaaaad
dcbabaaaabaaaaaagcbaaaadmcbabaaaabaaaaaagfaaaaadpccabaaaaaaaaaaa
giaaaaacadaaaaaaefaaaaajpcaabaaaaaaaaaaaogbkbaaaabaaaaaaeghobaaa
acaaaaaaaagabaaaacaaaaaabbaaaaakbcaabaaaaaaaaaaaegaobaaaaaaaaaaa
aceaaaaaaaaaiadpibiaiadlicabibdhafidibddefaaaaajpcaabaaaabaaaaaa
ogbkbaaaabaaaaaaeghobaaaabaaaaaaaagabaaaabaaaaaaaaaaaaakpcaabaaa
abaaaaaaegaobaaaabaaaaaaaceaaaaaaaaaaaaaaaaaiadpaaaaaaaaaaaaiadp
efaaaaajpcaabaaaacaaaaaaegbabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaa
aaaaaaaadcaaaaanpcaabaaaabaaaaaaegaobaaaabaaaaaaaceaaaaaaaaaaadp
aaaaaadpaaaaaadpaaaaaadpegaobaiaebaaaaaaacaaaaaadcaaaaajpccabaaa
aaaaaaaaagaabaaaaaaaaaaaegaobaaaabaaaaaaegaobaaaacaaaaaadoaaaaab
ejfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadadaaaa
fmaaaaaaabaaaaaaaaaaaaaaadaaaaaaabaaaaaaamamaaaafdfgfpfagphdgjhe
gjgpgoaafeeffiedepepfceeaaklklklepfdeheocmaaaaaaabaaaaaaaiaaaaaa
caaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgf
heaaklkl"
}
}
 }
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
"!!ARBvp1.0
PARAM c[5] = { program.local[0],
		state.matrix.mvp };
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 5 instructions, 0 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_TexelSize]
"vs_2_0
def c5, 0.00000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.x, c5
slt r0.x, c4.y, r0
max r0.x, -r0, r0
slt r0.x, c5, r0
add r0.y, -r0.x, c5
mul r0.z, v1.y, r0.y
add r0.y, -v1, c5
mad oT1.y, r0.x, r0, r0.z
mov oT0.xy, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
mov oT1.x, v1
"
}
SubProgram "d3d11 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 80
Vector 32 [_MainTex_TexelSize]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0
eefiecedgalbggkgcijkbbogllmajnckopibikhoabaaaaaahmacaaaaadaaaaaa
cmaaaaaaiaaaaaaapaaaaaaaejfdeheoemaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaaebaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaafaepfdejfeejepeoaafeeffiedepepfceeaaklkl
epfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadamaaaa
fmaaaaaaabaaaaaaaaaaaaaaadaaaaaaabaaaaaaamadaaaafdfgfpfagphdgjhe
gjgpgoaafeeffiedepepfceeaaklklklfdeieefcieabaaaaeaaaabaagbaaaaaa
fjaaaaaeegiocaaaaaaaaaaaadaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaa
fpaaaaadpcbabaaaaaaaaaaafpaaaaaddcbabaaaabaaaaaaghaaaaaepccabaaa
aaaaaaaaabaaaaaagfaaaaaddccabaaaabaaaaaagfaaaaadmccabaaaabaaaaaa
giaaaaacabaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaa
abaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaa
agbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaa
abaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaa
aaaaaaaaegiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaa
dbaaaaaibcaabaaaaaaaaaaabkiacaaaaaaaaaaaacaaaaaaabeaaaaaaaaaaaaa
aaaaaaaiccaabaaaaaaaaaaabkbabaiaebaaaaaaabaaaaaaabeaaaaaaaaaiadp
dhaaaaajiccabaaaabaaaaaaakaabaaaaaaaaaaabkaabaaaaaaaaaaabkbabaaa
abaaaaaadgaaaaafhccabaaaabaaaaaaegbabaaaabaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 80
Vector 32 [_MainTex_TexelSize]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0_level_9_1
eefiecedkfiplpebenglpjinfbgeeejidigadnefabaaaaaalmadaaaaaeaaaaaa
daaaaaaagmabaaaapiacaaaaemadaaaaebgpgodjdeabaaaadeabaaaaaaacpopp
peaaaaaaeaaaaaaaacaaceaaaaaadmaaaaaadmaaaaaaceaaabaadmaaaaaaacaa
abaaabaaaaaaaaaaabaaaaaaaeaaacaaaaaaaaaaaaaaaaaaaaacpoppfbaaaaaf
agaaapkaaaaaaaaaaaaaaamaaaaaiadpaaaaaaaabpaaaaacafaaaaiaaaaaapja
bpaaaaacafaaabiaabaaapjaabaaaaacaaaaabiaagaaaakaamaaaaadaaaaabia
abaaffkaaaaaaaiaaeaaaaaeaaaaaciaabaaffjaagaaffkaagaakkkaaeaaaaae
aaaaaeoaaaaaaaiaaaaaffiaabaaffjaafaaaaadaaaaapiaaaaaffjaadaaoeka
aeaaaaaeaaaaapiaacaaoekaaaaaaajaaaaaoeiaaeaaaaaeaaaaapiaaeaaoeka
aaaakkjaaaaaoeiaaeaaaaaeaaaaapiaafaaoekaaaaappjaaaaaoeiaaeaaaaae
aaaaadmaaaaappiaaaaaoekaaaaaoeiaabaaaaacaaaaammaaaaaoeiaabaaaaac
aaaaaloaabaacejappppaaaafdeieefcieabaaaaeaaaabaagbaaaaaafjaaaaae
egiocaaaaaaaaaaaadaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaafpaaaaad
pcbabaaaaaaaaaaafpaaaaaddcbabaaaabaaaaaaghaaaaaepccabaaaaaaaaaaa
abaaaaaagfaaaaaddccabaaaabaaaaaagfaaaaadmccabaaaabaaaaaagiaaaaac
abaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaa
abaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaa
aaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaa
acaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaa
egiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadbaaaaai
bcaabaaaaaaaaaaabkiacaaaaaaaaaaaacaaaaaaabeaaaaaaaaaaaaaaaaaaaai
ccaabaaaaaaaaaaabkbabaiaebaaaaaaabaaaaaaabeaaaaaaaaaiadpdhaaaaaj
iccabaaaabaaaaaaakaabaaaaaaaaaaabkaabaaaaaaaaaaabkbabaaaabaaaaaa
dgaaaaafhccabaaaabaaaaaaegbabaaaabaaaaaadoaaaaabejfdeheoemaaaaaa
acaaaaaaaiaaaaaadiaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaa
ebaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadadaaaafaepfdejfeejepeo
aafeeffiedepepfceeaaklklepfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadamaaaafmaaaaaaabaaaaaaaaaaaaaaadaaaaaaabaaaaaa
amadaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklkl"
}
}
Program "fp" {
SubProgram "opengl " {
Vector 0 [_ZBufferParams]
Vector 1 [_SimpleDofParams]
SetTexture 0 [_CameraDepthTexture] 2D 0
SetTexture 1 [_Coc] 2D 1
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
PARAM c[5] = { program.local[0..1],
		{ 0.99902344, 1, 0, 0.0024999999 },
		{ 1, 255, 65025, 16581375 },
		{ 2, 0.0039215689 } };
TEMP R0;
TEMP R1;
TEX R1.x, fragment.texcoord[0], texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[1], 2D;
MAD R1.x, R1, c[0], c[0].y;
RCP R1.y, R1.x;
ADD R1.z, R1.y, -c[1].y;
SLT R1.y, c[1], R1;
ABS R1.w, R1.y;
ADD R1.z, R1, -c[2].w;
MUL R1.y, R1.z, c[4].x;
CMP R1.z, -R1.w, c[2], c[2].y;
CMP R1.y, -R1.z, c[2].z, R1;
RCP R1.x, c[1].w;
MUL R1.x, R1.y, R1;
MIN_SAT R1.x, R1, c[2];
MUL R1, R1.x, c[3];
FRC R1, R1;
MAD R1, -R1.yzww, c[4].y, R1;
MAX result.color, R1, R0;
END
# 18 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Vector 0 [_ZBufferParams]
Vector 1 [_SimpleDofParams]
SetTexture 0 [_CameraDepthTexture] 2D 0
SetTexture 1 [_Coc] 2D 1
"ps_2_0
dcl_2d s0
dcl_2d s1
def c2, 0.00000000, 1.00000000, -0.00250000, 2.00000000
def c3, 0.99902344, 0.00392157, 0, 0
def c4, 1.00000000, 255.00000000, 65025.00000000, 16581375.00000000
dcl t1.xy
texld r0, t1, s0
texld r1, t1, s1
mad r0.x, r0, c0, c0.y
rcp r2.x, r0.x
add r0.x, -r2, c1.y
add r2.x, r2, -c1.y
add r2.x, r2, c2.z
mul r3.x, r2, c2.w
cmp r0.x, r0, c2, c2.y
abs_pp r0.x, r0
rcp_pp r2.x, c1.w
cmp_pp r0.x, -r0, c2, r3
mul_pp r0.x, r0, r2
min_pp_sat r0.x, r0, c3
mul r0, r0.x, c4
frc r2, r0
mov r0.xyw, -r2.yzxw
mov r0.z, -r2.w
mad r0, r0, c3.y, r2
max r0, r0, r1
mov_pp oC0, r0
"
}
SubProgram "d3d11 " {
SetTexture 0 [_CameraDepthTexture] 2D 0
SetTexture 1 [_Coc] 2D 1
ConstBuffer "$Globals" 80
Vector 48 [_SimpleDofParams]
ConstBuffer "UnityPerCamera" 128
Vector 112 [_ZBufferParams]
BindCB  "$Globals" 0
BindCB  "UnityPerCamera" 1
"ps_4_0
eefiecedgneonljdpncokkdmgbcfdadkeneiikikabaaaaaaheadaaaaadaaaaaa
cmaaaaaajmaaaaaanaaaaaaaejfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadaaaaaafmaaaaaaabaaaaaaaaaaaaaaadaaaaaaabaaaaaa
amamaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklklepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcjmacaaaaeaaaaaaakhaaaaaa
fjaaaaaeegiocaaaaaaaaaaaaeaaaaaafjaaaaaeegiocaaaabaaaaaaaiaaaaaa
fkaaaaadaagabaaaaaaaaaaafkaaaaadaagabaaaabaaaaaafibiaaaeaahabaaa
aaaaaaaaffffaaaafibiaaaeaahabaaaabaaaaaaffffaaaagcbaaaadmcbabaaa
abaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacacaaaaaaefaaaaajpcaabaaa
aaaaaaaaogbkbaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadcaaaaal
bcaabaaaaaaaaaaaakiacaaaabaaaaaaahaaaaaaakaabaaaaaaaaaaabkiacaaa
abaaaaaaahaaaaaaaoaaaaakbcaabaaaaaaaaaaaaceaaaaaaaaaiadpaaaaiadp
aaaaiadpaaaaiadpakaabaaaaaaaaaaaaaaaaaajccaabaaaaaaaaaaaakaabaaa
aaaaaaaabkiacaiaebaaaaaaaaaaaaaaadaaaaaadbaaaaaibcaabaaaaaaaaaaa
bkiacaaaaaaaaaaaadaaaaaaakaabaaaaaaaaaaaaaaaaaahccaabaaaaaaaaaaa
bkaabaaaaaaaaaaaabeaaaaaaknhcdllaaaaaaahccaabaaaaaaaaaaabkaabaaa
aaaaaaaabkaabaaaaaaaaaaaabaaaaahbcaabaaaaaaaaaaabkaabaaaaaaaaaaa
akaabaaaaaaaaaaaaoaaaaaibcaabaaaaaaaaaaaakaabaaaaaaaaaaadkiacaaa
aaaaaaaaadaaaaaadeaaaaahbcaabaaaaaaaaaaaakaabaaaaaaaaaaaabeaaaaa
aaaaaaaaddaaaaahbcaabaaaaaaaaaaaakaabaaaaaaaaaaaabeaaaaahhlohpdp
diaaaaakpcaabaaaaaaaaaaaagaabaaaaaaaaaaaaceaaaaaaaaaiadpaaaahped
aaabhoehppachnelbkaaaaafpcaabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaan
pcaabaaaaaaaaaaajgapbaiaebaaaaaaaaaaaaaaaceaaaaaibiaiadlibiaiadl
ibiaiadlibiaiadlegaobaaaaaaaaaaaefaaaaajpcaabaaaabaaaaaaogbkbaaa
abaaaaaaeghobaaaabaaaaaaaagabaaaabaaaaaadeaaaaahpccabaaaaaaaaaaa
egaobaaaaaaaaaaaegaobaaaabaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
SetTexture 0 [_CameraDepthTexture] 2D 0
SetTexture 1 [_Coc] 2D 1
ConstBuffer "$Globals" 80
Vector 48 [_SimpleDofParams]
ConstBuffer "UnityPerCamera" 128
Vector 112 [_ZBufferParams]
BindCB  "$Globals" 0
BindCB  "UnityPerCamera" 1
"ps_4_0_level_9_1
eefiecedfdgbanghilgheblpinbpoghiefcefapiabaaaaaaiiafaaaaaeaaaaaa
daaaaaaaeaacaaaaoeaeaaaafeafaaaaebgpgodjaiacaaaaaiacaaaaaaacpppp
meabaaaaeeaaaaaaacaacmaaaaaaeeaaaaaaeeaaacaaceaaaaaaeeaaaaaaaaaa
abababaaaaaaadaaabaaaaaaaaaaaaaaabaaahaaabaaabaaaaaaaaaaaaacpppp
fbaaaaafacaaapkaaknhcdllaaaaaaaahhlohplpibiaiadlfbaaaaafadaaapka
aaaaiadpaaaahpedaaabhoehppachnelfbaaaaafaeaaapkahhlohpdpaaljdodp
aaaahkdpaaaaaaaabpaaaaacaaaaaaiaaaaaaplabpaaaaacaaaaaajaaaaiapka
bpaaaaacaaaaaajaabaiapkaabaaaaacaaaaadiaaaaabllaecaaaaadabaaapia
aaaaoeiaaaaioekaecaaaaadaaaacpiaaaaaoeiaabaioekaaeaaaaaeabaaabia
abaaaakaabaaaaiaabaaffkaagaaaaacabaaabiaabaaaaiaacaaaaadabaaacia
abaaaaiaaaaaffkbacaaaaadabaaabiaabaaaaibaaaaffkaacaaaaadabaaacia
abaaffiaacaaaakaacaaaaadabaacciaabaaffiaabaaffiafiaaaaaeabaacbia
abaaaaiaacaaffkaabaaffiaagaaaaacabaaaciaaaaappkaafaaaaadabaacbia
abaaffiaabaaaaiaalaaaaadacaaciiaabaaaaiaacaaffkaafaaaaadabaaapia
acaappiaadaaoekaacaaaaadacaaabiaacaappiaacaakkkabdaaaaacabaaapia
abaaoeiafiaaaaaeabaaapiaacaaaaiaaeaaoekaabaaoeiaaeaaaaaeacaaclia
abaamjiaacaappkbabaaoeiaaeaaaaaeacaaceiaabaappiaacaappkbabaakkia
alaaaaadabaacpiaacaaoeiaaaaaoeiaabaaaaacaaaicpiaabaaoeiappppaaaa
fdeieefcjmacaaaaeaaaaaaakhaaaaaafjaaaaaeegiocaaaaaaaaaaaaeaaaaaa
fjaaaaaeegiocaaaabaaaaaaaiaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaad
aagabaaaabaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaafibiaaaeaahabaaa
abaaaaaaffffaaaagcbaaaadmcbabaaaabaaaaaagfaaaaadpccabaaaaaaaaaaa
giaaaaacacaaaaaaefaaaaajpcaabaaaaaaaaaaaogbkbaaaabaaaaaaeghobaaa
aaaaaaaaaagabaaaaaaaaaaadcaaaaalbcaabaaaaaaaaaaaakiacaaaabaaaaaa
ahaaaaaaakaabaaaaaaaaaaabkiacaaaabaaaaaaahaaaaaaaoaaaaakbcaabaaa
aaaaaaaaaceaaaaaaaaaiadpaaaaiadpaaaaiadpaaaaiadpakaabaaaaaaaaaaa
aaaaaaajccaabaaaaaaaaaaaakaabaaaaaaaaaaabkiacaiaebaaaaaaaaaaaaaa
adaaaaaadbaaaaaibcaabaaaaaaaaaaabkiacaaaaaaaaaaaadaaaaaaakaabaaa
aaaaaaaaaaaaaaahccaabaaaaaaaaaaabkaabaaaaaaaaaaaabeaaaaaaknhcdll
aaaaaaahccaabaaaaaaaaaaabkaabaaaaaaaaaaabkaabaaaaaaaaaaaabaaaaah
bcaabaaaaaaaaaaabkaabaaaaaaaaaaaakaabaaaaaaaaaaaaoaaaaaibcaabaaa
aaaaaaaaakaabaaaaaaaaaaadkiacaaaaaaaaaaaadaaaaaadeaaaaahbcaabaaa
aaaaaaaaakaabaaaaaaaaaaaabeaaaaaaaaaaaaaddaaaaahbcaabaaaaaaaaaaa
akaabaaaaaaaaaaaabeaaaaahhlohpdpdiaaaaakpcaabaaaaaaaaaaaagaabaaa
aaaaaaaaaceaaaaaaaaaiadpaaaahpedaaabhoehppachnelbkaaaaafpcaabaaa
aaaaaaaaegaobaaaaaaaaaaadcaaaaanpcaabaaaaaaaaaaajgapbaiaebaaaaaa
aaaaaaaaaceaaaaaibiaiadlibiaiadlibiaiadlibiaiadlegaobaaaaaaaaaaa
efaaaaajpcaabaaaabaaaaaaogbkbaaaabaaaaaaeghobaaaabaaaaaaaagabaaa
abaaaaaadeaaaaahpccabaaaaaaaaaaaegaobaaaaaaaaaaaegaobaaaabaaaaaa
doaaaaabejfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaaaaaaaaaaabaaaaaa
adaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaa
adaaaaaafmaaaaaaabaaaaaaaaaaaaaaadaaaaaaabaaaaaaamamaaaafdfgfpfa
gphdgjhegjgpgoaafeeffiedepepfceeaaklklklepfdeheocmaaaaaaabaaaaaa
aiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapaaaaaafdfgfpfe
gbhcghgfheaaklkl"
}
}
 }
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Vector 5 [offsets]
"!!ARBvp1.0
PARAM c[7] = { { 2, -2, 3, -3 },
		state.matrix.mvp,
		program.local[5],
		{ 1, -1 } };
TEMP R0;
TEMP R1;
MOV R1, c[0];
MOV R0.xy, c[6];
MAD result.texcoord[1], R0.xxyy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[2], R1.xxyy, c[5].xyxy, vertex.texcoord[0].xyxy;
MAD result.texcoord[3], R1.zzww, c[5].xyxy, vertex.texcoord[0].xyxy;
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 10 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [offsets]
"vs_2_0
def c5, 1.00000000, -1.00000000, 2.00000000, -2.00000000
def c6, 3.00000000, -3.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.xy, c4
mad oT1, c5.xxyy, r0.xyxy, v1.xyxy
mov r0.xy, c4
mov r0.zw, c4.xyxy
mad oT2, c5.zzww, r0.xyxy, v1.xyxy
mad oT3, c6.xxyy, r0.zwzw, v1.xyxy
mov oT0.xy, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
"
}
SubProgram "d3d11 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 80
Vector 16 [offsets]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0
eefiecedblembfcmhpeadakcfcnppiicngchjdjdabaaaaaapmacaaaaadaaaaaa
cmaaaaaaiaaaaaaacaabaaaaejfdeheoemaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaaebaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaafaepfdejfeejepeoaafeeffiedepepfceeaaklkl
epfdeheojiaaaaaaafaaaaaaaiaaaaaaiaaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaaimaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadamaaaa
imaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaaapaaaaaaimaaaaaaacaaaaaa
aaaaaaaaadaaaaaaadaaaaaaapaaaaaaimaaaaaaadaaaaaaaaaaaaaaadaaaaaa
aeaaaaaaapaaaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklkl
fdeieefcneabaaaaeaaaabaahfaaaaaafjaaaaaeegiocaaaaaaaaaaaacaaaaaa
fjaaaaaeegiocaaaabaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaad
dcbabaaaabaaaaaaghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaaddccabaaa
abaaaaaagfaaaaadpccabaaaacaaaaaagfaaaaadpccabaaaadaaaaaagfaaaaad
pccabaaaaeaaaaaagiaaaaacabaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaa
aaaaaaaaegiocaaaabaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaa
abaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaabaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaakpccabaaaaaaaaaaaegiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaa
egaobaaaaaaaaaaadgaaaaafdccabaaaabaaaaaaegbabaaaabaaaaaadcaaaaan
pccabaaaacaaaaaaegiecaaaaaaaaaaaabaaaaaaaceaaaaaaaaaiadpaaaaiadp
aaaaialpaaaaialpegbebaaaabaaaaaadcaaaaanpccabaaaadaaaaaaegiecaaa
aaaaaaaaabaaaaaaaceaaaaaaaaaaaeaaaaaaaeaaaaaaamaaaaaaamaegbebaaa
abaaaaaadcaaaaanpccabaaaaeaaaaaaegiecaaaaaaaaaaaabaaaaaaaceaaaaa
aaaaeaeaaaaaeaeaaaaaeamaaaaaeamaegbebaaaabaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 80
Vector 16 [offsets]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0_level_9_1
eefiecedlfpckmkimfmonkgfiaeojfndcdbnmiipabaaaaaafiaeaaaaaeaaaaaa
daaaaaaaiiabaaaageadaaaaliadaaaaebgpgodjfaabaaaafaabaaaaaaacpopp
baabaaaaeaaaaaaaacaaceaaaaaadmaaaaaadmaaaaaaceaaabaadmaaaaaaabaa
abaaabaaaaaaaaaaabaaaaaaaeaaacaaaaaaaaaaaaaaaaaaaaacpoppfbaaaaaf
agaaapkaaaaaiadpaaaaialpaaaaaaeaaaaaaamafbaaaaafahaaapkaaaaaeaea
aaaaeamaaaaaaaaaaaaaaaaabpaaaaacafaaaaiaaaaaapjabpaaaaacafaaabia
abaaapjaabaaaaacaaaaadiaabaaoekaaeaaaaaeabaaapoaaaaaeeiaagaafaka
abaaeejaaeaaaaaeacaaapoaaaaaeeiaagaapkkaabaaeejaaeaaaaaeadaaapoa
aaaaeeiaahaafakaabaaeejaafaaaaadaaaaapiaaaaaffjaadaaoekaaeaaaaae
aaaaapiaacaaoekaaaaaaajaaaaaoeiaaeaaaaaeaaaaapiaaeaaoekaaaaakkja
aaaaoeiaaeaaaaaeaaaaapiaafaaoekaaaaappjaaaaaoeiaaeaaaaaeaaaaadma
aaaappiaaaaaoekaaaaaoeiaabaaaaacaaaaammaaaaaoeiaabaaaaacaaaaadoa
abaaoejappppaaaafdeieefcneabaaaaeaaaabaahfaaaaaafjaaaaaeegiocaaa
aaaaaaaaacaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaafpaaaaadpcbabaaa
aaaaaaaafpaaaaaddcbabaaaabaaaaaaghaaaaaepccabaaaaaaaaaaaabaaaaaa
gfaaaaaddccabaaaabaaaaaagfaaaaadpccabaaaacaaaaaagfaaaaadpccabaaa
adaaaaaagfaaaaadpccabaaaaeaaaaaagiaaaaacabaaaaaadiaaaaaipcaabaaa
aaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaaabaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaacaaaaaakgbkbaaaaaaaaaaa
egaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaaegiocaaaabaaaaaaadaaaaaa
pgbpbaaaaaaaaaaaegaobaaaaaaaaaaadgaaaaafdccabaaaabaaaaaaegbabaaa
abaaaaaadcaaaaanpccabaaaacaaaaaaegiecaaaaaaaaaaaabaaaaaaaceaaaaa
aaaaiadpaaaaiadpaaaaialpaaaaialpegbebaaaabaaaaaadcaaaaanpccabaaa
adaaaaaaegiecaaaaaaaaaaaabaaaaaaaceaaaaaaaaaaaeaaaaaaaeaaaaaaama
aaaaaamaegbebaaaabaaaaaadcaaaaanpccabaaaaeaaaaaaegiecaaaaaaaaaaa
abaaaaaaaceaaaaaaaaaeaeaaaaaeaeaaaaaeamaaaaaeamaegbebaaaabaaaaaa
doaaaaabejfdeheoemaaaaaaacaaaaaaaiaaaaaadiaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaaaaaaaaaapapaaaaebaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaa
adadaaaafaepfdejfeejepeoaafeeffiedepepfceeaaklklepfdeheojiaaaaaa
afaaaaaaaiaaaaaaiaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaa
imaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadamaaaaimaaaaaaabaaaaaa
aaaaaaaaadaaaaaaacaaaaaaapaaaaaaimaaaaaaacaaaaaaaaaaaaaaadaaaaaa
adaaaaaaapaaaaaaimaaaaaaadaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaa
fdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklkl"
}
}
Program "fp" {
SubProgram "opengl " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Coc] 2D 1
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
PARAM c[1] = { { 1, 0.0039215689, 1.53787e-005, 6.0308629e-008 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
TEMP R5;
TEMP R6;
TEMP R7;
TEMP R8;
TEMP R9;
TEX R2, fragment.texcoord[1].zwzw, texture[1], 2D;
TEX R0, fragment.texcoord[0], texture[1], 2D;
TEX R1, fragment.texcoord[1], texture[1], 2D;
TEX R3, fragment.texcoord[2], texture[1], 2D;
TEX R4, fragment.texcoord[2].zwzw, texture[1], 2D;
TEX R9.xyz, fragment.texcoord[2].zwzw, texture[0], 2D;
TEX R8.xyz, fragment.texcoord[2], texture[0], 2D;
TEX R7.xyz, fragment.texcoord[1].zwzw, texture[0], 2D;
TEX R5.xyz, fragment.texcoord[0], texture[0], 2D;
TEX R6.xyz, fragment.texcoord[1], texture[0], 2D;
DP4 R2.x, R2, c[0];
DP4 R0.w, R0, c[0];
DP4 R1.w, R1, c[0];
MUL R1.xyz, R6, R1.w;
MAD R0.xyz, R0.w, R5, R1;
DP4 R2.y, R3, c[0];
DP4 R2.z, R4, c[0];
MOV R1.z, R2;
MOV R1.y, R2;
MOV R1.x, R2;
DP4 R1.x, R1.wxyz, c[0].x;
MAD R3.xyz, R7, R2.x, R0;
ADD R0.x, R0.w, R1;
MAD R1.xyz, R8, R2.y, R3;
RCP R0.x, R0.x;
MAD R1.xyz, R9, R2.z, R1;
MUL result.color.xyz, R1, R0.x;
MOV result.color.w, R0;
END
# 28 instructions, 10 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Coc] 2D 1
"ps_2_0
dcl_2d s0
dcl_2d s1
def c0, 1.00000000, 0.00392157, 0.00001538, 0.00000006
dcl t0.xy
dcl t1
dcl t2
texld r8, t2, s0
texld r6, t0, s0
texld r5, t1, s1
texld r4, t1, s0
dp4 r5.x, r5, c0
mul_pp r4.xyz, r4, r5.x
mov r0.y, t1.w
mov r0.x, t1.z
mov r1.xy, r0
mov r0.y, t2.w
mov r0.x, t2.z
mov r2.y, t2.w
mov r2.x, t2.z
mov r3.xy, r2
mov r2.y, t1.w
mov r2.x, t1.z
texld r7, r3, s0
texld r9, r2, s0
texld r2, r0, s1
texld r3, t2, s1
texld r0, t0, s1
texld r1, r1, s1
dp4 r1.x, r1, c0
dp4 r2.x, r2, c0
dp4 r3.x, r3, c0
dp4 r0.x, r0, c0
mov_pp r5.y, r1.x
mov_pp r5.w, r2.x
mov_pp r5.z, r3.x
mad_pp r4.xyz, r0.x, r6, r4
mad_pp r4.xyz, r9, r1.x, r4
dp4_pp r5.x, r5, c0.x
add_pp r1.x, r0, r5
mad_pp r3.xyz, r8, r3.x, r4
rcp_pp r1.x, r1.x
mad_pp r2.xyz, r7, r2.x, r3
mul_pp r1.xyz, r2, r1.x
mov_pp r1.w, r0.x
mov_pp oC0, r1
"
}
SubProgram "d3d11 " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Coc] 2D 1
"ps_4_0
eefiecedjiocahennfklaogknpdjjhhmaagnlgpbabaaaaaaneaeaaaaadaaaaaa
cmaaaaaammaaaaaaaaabaaaaejfdeheojiaaaaaaafaaaaaaaiaaaaaaiaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaimaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaaimaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apapaaaaimaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaaapapaaaaimaaaaaa
adaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaafdfgfpfagphdgjhegjgpgoaa
feeffiedepepfceeaaklklklepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklkl
fdeieefcmmadaaaaeaaaaaaapdaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaad
aagabaaaabaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaafibiaaaeaahabaaa
abaaaaaaffffaaaagcbaaaaddcbabaaaabaaaaaagcbaaaadpcbabaaaacaaaaaa
gcbaaaadpcbabaaaadaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacaeaaaaaa
efaaaaajpcaabaaaaaaaaaaaegbabaaaacaaaaaaeghobaaaaaaaaaaaaagabaaa
aaaaaaaaefaaaaajpcaabaaaabaaaaaaegbabaaaacaaaaaaeghobaaaabaaaaaa
aagabaaaabaaaaaabbaaaaakbcaabaaaabaaaaaaegaobaaaabaaaaaaaceaaaaa
aaaaiadpibiaiadlicabibdhafidibdddiaaaaahhcaabaaaaaaaaaaaegacbaaa
aaaaaaaaagaabaaaabaaaaaaefaaaaajpcaabaaaacaaaaaaegbabaaaabaaaaaa
eghobaaaaaaaaaaaaagabaaaaaaaaaaaefaaaaajpcaabaaaadaaaaaaegbabaaa
abaaaaaaeghobaaaabaaaaaaaagabaaaabaaaaaabbaaaaakicaabaaaaaaaaaaa
egaobaaaadaaaaaaaceaaaaaaaaaiadpibiaiadlicabibdhafidibdddcaaaaaj
hcaabaaaaaaaaaaaegacbaaaacaaaaaapgapbaaaaaaaaaaaegacbaaaaaaaaaaa
efaaaaajpcaabaaaacaaaaaaogbkbaaaacaaaaaaeghobaaaaaaaaaaaaagabaaa
aaaaaaaaefaaaaajpcaabaaaadaaaaaaogbkbaaaacaaaaaaeghobaaaabaaaaaa
aagabaaaabaaaaaabbaaaaakccaabaaaabaaaaaaegaobaaaadaaaaaaaceaaaaa
aaaaiadpibiaiadlicabibdhafidibdddcaaaaajhcaabaaaaaaaaaaaegacbaaa
acaaaaaafgafbaaaabaaaaaaegacbaaaaaaaaaaaefaaaaajpcaabaaaacaaaaaa
egbabaaaadaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaaefaaaaajpcaabaaa
adaaaaaaegbabaaaadaaaaaaeghobaaaabaaaaaaaagabaaaabaaaaaabbaaaaak
ecaabaaaabaaaaaaegaobaaaadaaaaaaaceaaaaaaaaaiadpibiaiadlicabibdh
afidibdddcaaaaajhcaabaaaaaaaaaaaegacbaaaacaaaaaakgakbaaaabaaaaaa
egacbaaaaaaaaaaaefaaaaajpcaabaaaacaaaaaaogbkbaaaadaaaaaaeghobaaa
aaaaaaaaaagabaaaaaaaaaaaefaaaaajpcaabaaaadaaaaaaogbkbaaaadaaaaaa
eghobaaaabaaaaaaaagabaaaabaaaaaabbaaaaakicaabaaaabaaaaaaegaobaaa
adaaaaaaaceaaaaaaaaaiadpibiaiadlicabibdhafidibdddcaaaaajhcaabaaa
aaaaaaaaegacbaaaacaaaaaapgapbaaaabaaaaaaegacbaaaaaaaaaaabbaaaaak
bcaabaaaabaaaaaaaceaaaaaaaaaiadpaaaaiadpaaaaiadpaaaaiadpegaobaaa
abaaaaaaaaaaaaahbcaabaaaabaaaaaadkaabaaaaaaaaaaaakaabaaaabaaaaaa
dgaaaaaficcabaaaaaaaaaaadkaabaaaaaaaaaaaaoaaaaahhccabaaaaaaaaaaa
egacbaaaaaaaaaaaagaabaaaabaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Coc] 2D 1
"ps_4_0_level_9_1
eefiecedgpommcgffgfdogjienbhjmikognedngiabaaaaaadaahaaaaaeaaaaaa
daaaaaaaiiacaaaafmagaaaapmagaaaaebgpgodjfaacaaaafaacaaaaaaacpppp
ceacaaaacmaaaaaaaaaacmaaaaaacmaaaaaacmaaacaaceaaaaaacmaaaaaaaaaa
abababaaaaacppppfbaaaaafaaaaapkaaaaaiadpibiaiadlicabibdhafidibdd
bpaaaaacaaaaaaiaaaaaadlabpaaaaacaaaaaaiaabaaaplabpaaaaacaaaaaaia
acaaaplabpaaaaacaaaaaajaaaaiapkabpaaaaacaaaaaajaabaiapkaecaaaaad
aaaacpiaabaaoelaaaaioekaecaaaaadabaaapiaabaaoelaabaioekaecaaaaad
acaacpiaaaaaoelaaaaioekaecaaaaadadaaapiaaaaaoelaabaioekaecaaaaad
aeaacpiaacaaoelaaaaioekaecaaaaadafaaapiaacaaoelaabaioekaajaaaaad
abaacbiaabaaoeiaaaaaoekaafaaaaadaaaachiaaaaaoeiaabaaaaiaajaaaaad
adaaciiaadaaoeiaaaaaoekaaeaaaaaeaaaachiaacaaoeiaadaappiaaaaaoeia
abaaaaacacaaabiaabaakklaabaaaaacacaaaciaabaapplaabaaaaacagaaabia
acaakklaabaaaaacagaaaciaacaapplaecaaaaadahaacpiaacaaoeiaaaaioeka
ecaaaaadacaaapiaacaaoeiaabaioekaecaaaaadaiaacpiaagaaoeiaaaaioeka
ecaaaaadagaaapiaagaaoeiaabaioekaajaaaaadabaacciaacaaoeiaaaaaoeka
aeaaaaaeaaaachiaahaaoeiaabaaffiaaaaaoeiaajaaaaadabaaceiaafaaoeia
aaaaoekaaeaaaaaeaaaachiaaeaaoeiaabaakkiaaaaaoeiaajaaaaadabaaciia
agaaoeiaaaaaoekaaeaaaaaeaaaachiaaiaaoeiaabaappiaaaaaoeiaajaaaaad
aaaaciiaaaaaaakaabaaoeiaacaaaaadaaaaciiaaaaappiaadaappiaagaaaaac
aaaaaiiaaaaappiaafaaaaadadaachiaaaaappiaaaaaoeiaabaaaaacaaaicpia
adaaoeiappppaaaafdeieefcmmadaaaaeaaaaaaapdaaaaaafkaaaaadaagabaaa
aaaaaaaafkaaaaadaagabaaaabaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaa
fibiaaaeaahabaaaabaaaaaaffffaaaagcbaaaaddcbabaaaabaaaaaagcbaaaad
pcbabaaaacaaaaaagcbaaaadpcbabaaaadaaaaaagfaaaaadpccabaaaaaaaaaaa
giaaaaacaeaaaaaaefaaaaajpcaabaaaaaaaaaaaegbabaaaacaaaaaaeghobaaa
aaaaaaaaaagabaaaaaaaaaaaefaaaaajpcaabaaaabaaaaaaegbabaaaacaaaaaa
eghobaaaabaaaaaaaagabaaaabaaaaaabbaaaaakbcaabaaaabaaaaaaegaobaaa
abaaaaaaaceaaaaaaaaaiadpibiaiadlicabibdhafidibdddiaaaaahhcaabaaa
aaaaaaaaegacbaaaaaaaaaaaagaabaaaabaaaaaaefaaaaajpcaabaaaacaaaaaa
egbabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaaefaaaaajpcaabaaa
adaaaaaaegbabaaaabaaaaaaeghobaaaabaaaaaaaagabaaaabaaaaaabbaaaaak
icaabaaaaaaaaaaaegaobaaaadaaaaaaaceaaaaaaaaaiadpibiaiadlicabibdh
afidibdddcaaaaajhcaabaaaaaaaaaaaegacbaaaacaaaaaapgapbaaaaaaaaaaa
egacbaaaaaaaaaaaefaaaaajpcaabaaaacaaaaaaogbkbaaaacaaaaaaeghobaaa
aaaaaaaaaagabaaaaaaaaaaaefaaaaajpcaabaaaadaaaaaaogbkbaaaacaaaaaa
eghobaaaabaaaaaaaagabaaaabaaaaaabbaaaaakccaabaaaabaaaaaaegaobaaa
adaaaaaaaceaaaaaaaaaiadpibiaiadlicabibdhafidibdddcaaaaajhcaabaaa
aaaaaaaaegacbaaaacaaaaaafgafbaaaabaaaaaaegacbaaaaaaaaaaaefaaaaaj
pcaabaaaacaaaaaaegbabaaaadaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaa
efaaaaajpcaabaaaadaaaaaaegbabaaaadaaaaaaeghobaaaabaaaaaaaagabaaa
abaaaaaabbaaaaakecaabaaaabaaaaaaegaobaaaadaaaaaaaceaaaaaaaaaiadp
ibiaiadlicabibdhafidibdddcaaaaajhcaabaaaaaaaaaaaegacbaaaacaaaaaa
kgakbaaaabaaaaaaegacbaaaaaaaaaaaefaaaaajpcaabaaaacaaaaaaogbkbaaa
adaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaaefaaaaajpcaabaaaadaaaaaa
ogbkbaaaadaaaaaaeghobaaaabaaaaaaaagabaaaabaaaaaabbaaaaakicaabaaa
abaaaaaaegaobaaaadaaaaaaaceaaaaaaaaaiadpibiaiadlicabibdhafidibdd
dcaaaaajhcaabaaaaaaaaaaaegacbaaaacaaaaaapgapbaaaabaaaaaaegacbaaa
aaaaaaaabbaaaaakbcaabaaaabaaaaaaaceaaaaaaaaaiadpaaaaiadpaaaaiadp
aaaaiadpegaobaaaabaaaaaaaaaaaaahbcaabaaaabaaaaaadkaabaaaaaaaaaaa
akaabaaaabaaaaaadgaaaaaficcabaaaaaaaaaaadkaabaaaaaaaaaaaaoaaaaah
hccabaaaaaaaaaaaegacbaaaaaaaaaaaagaabaaaabaaaaaadoaaaaabejfdeheo
jiaaaaaaafaaaaaaaiaaaaaaiaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaa
apaaaaaaimaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadadaaaaimaaaaaa
abaaaaaaaaaaaaaaadaaaaaaacaaaaaaapapaaaaimaaaaaaacaaaaaaaaaaaaaa
adaaaaaaadaaaaaaapapaaaaimaaaaaaadaaaaaaaaaaaaaaadaaaaaaaeaaaaaa
apaaaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklklepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklkl"
}
}
 }
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
"!!ARBvp1.0
PARAM c[5] = { program.local[0],
		state.matrix.mvp };
MOV result.texcoord[0].xy, vertex.texcoord[0];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 5 instructions, 0 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_TexelSize]
"vs_2_0
def c5, 0.00000000, 1.00000000, 0, 0
dcl_position0 v0
dcl_texcoord0 v1
mov r0.x, c5
slt r0.x, c4.y, r0
max r0.x, -r0, r0
slt r0.x, c5, r0
add r0.y, -r0.x, c5
mul r0.z, v1.y, r0.y
add r0.y, -v1, c5
mad oT1.y, r0.x, r0, r0.z
mov oT0.xy, v1
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
mov oT1.x, v1
"
}
SubProgram "d3d11 " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 80
Vector 32 [_MainTex_TexelSize]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0
eefiecedgalbggkgcijkbbogllmajnckopibikhoabaaaaaahmacaaaaadaaaaaa
cmaaaaaaiaaaaaaapaaaaaaaejfdeheoemaaaaaaacaaaaaaaiaaaaaadiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaaebaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaafaepfdejfeejepeoaafeeffiedepepfceeaaklkl
epfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadamaaaa
fmaaaaaaabaaaaaaaaaaaaaaadaaaaaaabaaaaaaamadaaaafdfgfpfagphdgjhe
gjgpgoaafeeffiedepepfceeaaklklklfdeieefcieabaaaaeaaaabaagbaaaaaa
fjaaaaaeegiocaaaaaaaaaaaadaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaa
fpaaaaadpcbabaaaaaaaaaaafpaaaaaddcbabaaaabaaaaaaghaaaaaepccabaaa
aaaaaaaaabaaaaaagfaaaaaddccabaaaabaaaaaagfaaaaadmccabaaaabaaaaaa
giaaaaacabaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaa
abaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaa
agbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaa
abaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaa
aaaaaaaaegiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaa
dbaaaaaibcaabaaaaaaaaaaabkiacaaaaaaaaaaaacaaaaaaabeaaaaaaaaaaaaa
aaaaaaaiccaabaaaaaaaaaaabkbabaiaebaaaaaaabaaaaaaabeaaaaaaaaaiadp
dhaaaaajiccabaaaabaaaaaaakaabaaaaaaaaaaabkaabaaaaaaaaaaabkbabaaa
abaaaaaadgaaaaafhccabaaaabaaaaaaegbabaaaabaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
Bind "vertex" Vertex
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 80
Vector 32 [_MainTex_TexelSize]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "$Globals" 0
BindCB  "UnityPerDraw" 1
"vs_4_0_level_9_1
eefiecedkfiplpebenglpjinfbgeeejidigadnefabaaaaaalmadaaaaaeaaaaaa
daaaaaaagmabaaaapiacaaaaemadaaaaebgpgodjdeabaaaadeabaaaaaaacpopp
peaaaaaaeaaaaaaaacaaceaaaaaadmaaaaaadmaaaaaaceaaabaadmaaaaaaacaa
abaaabaaaaaaaaaaabaaaaaaaeaaacaaaaaaaaaaaaaaaaaaaaacpoppfbaaaaaf
agaaapkaaaaaaaaaaaaaaamaaaaaiadpaaaaaaaabpaaaaacafaaaaiaaaaaapja
bpaaaaacafaaabiaabaaapjaabaaaaacaaaaabiaagaaaakaamaaaaadaaaaabia
abaaffkaaaaaaaiaaeaaaaaeaaaaaciaabaaffjaagaaffkaagaakkkaaeaaaaae
aaaaaeoaaaaaaaiaaaaaffiaabaaffjaafaaaaadaaaaapiaaaaaffjaadaaoeka
aeaaaaaeaaaaapiaacaaoekaaaaaaajaaaaaoeiaaeaaaaaeaaaaapiaaeaaoeka
aaaakkjaaaaaoeiaaeaaaaaeaaaaapiaafaaoekaaaaappjaaaaaoeiaaeaaaaae
aaaaadmaaaaappiaaaaaoekaaaaaoeiaabaaaaacaaaaammaaaaaoeiaabaaaaac
aaaaaloaabaacejappppaaaafdeieefcieabaaaaeaaaabaagbaaaaaafjaaaaae
egiocaaaaaaaaaaaadaaaaaafjaaaaaeegiocaaaabaaaaaaaeaaaaaafpaaaaad
pcbabaaaaaaaaaaafpaaaaaddcbabaaaabaaaaaaghaaaaaepccabaaaaaaaaaaa
abaaaaaagfaaaaaddccabaaaabaaaaaagfaaaaadmccabaaaabaaaaaagiaaaaac
abaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaa
abaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaa
aaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaa
acaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaa
egiocaaaabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadbaaaaai
bcaabaaaaaaaaaaabkiacaaaaaaaaaaaacaaaaaaabeaaaaaaaaaaaaaaaaaaaai
ccaabaaaaaaaaaaabkbabaiaebaaaaaaabaaaaaaabeaaaaaaaaaiadpdhaaaaaj
iccabaaaabaaaaaaakaabaaaaaaaaaaabkaabaaaaaaaaaaabkbabaaaabaaaaaa
dgaaaaafhccabaaaabaaaaaaegbabaaaabaaaaaadoaaaaabejfdeheoemaaaaaa
acaaaaaaaiaaaaaadiaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaa
ebaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadadaaaafaepfdejfeejepeo
aafeeffiedepepfceeaaklklepfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadamaaaafmaaaaaaabaaaaaaaaaaaaaaadaaaaaaabaaaaaa
amadaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklkl"
}
}
Program "fp" {
SubProgram "opengl " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Coc] 2D 1
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
TEMP R0;
TEMP R1;
TEX R1, fragment.texcoord[0], texture[1], 2D;
TEX R0, fragment.texcoord[0], texture[0], 2D;
MAX result.color, R0, R1;
END
# 3 instructions, 2 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Coc] 2D 1
"ps_2_0
dcl_2d s0
dcl_2d s1
dcl t0.xy
texld r0, t0, s1
texld r1, t0, s0
max_pp r0, r1, r0
mov_pp oC0, r0
"
}
SubProgram "d3d11 " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Coc] 2D 1
"ps_4_0
eefiecedgcaloollclapdkcmkglifbldanaocnglabaaaaaakaabaaaaadaaaaaa
cmaaaaaajmaaaaaanaaaaaaaejfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaafmaaaaaaabaaaaaaaaaaaaaaadaaaaaaabaaaaaa
amaaaaaafdfgfpfagphdgjhegjgpgoaafeeffiedepepfceeaaklklklepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcmiaaaaaaeaaaaaaadcaaaaaa
fkaaaaadaagabaaaaaaaaaaafkaaaaadaagabaaaabaaaaaafibiaaaeaahabaaa
aaaaaaaaffffaaaafibiaaaeaahabaaaabaaaaaaffffaaaagcbaaaaddcbabaaa
abaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacacaaaaaaefaaaaajpcaabaaa
aaaaaaaaegbabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaaefaaaaaj
pcaabaaaabaaaaaaegbabaaaabaaaaaaeghobaaaabaaaaaaaagabaaaabaaaaaa
deaaaaahpccabaaaaaaaaaaaegaobaaaaaaaaaaaegaobaaaabaaaaaadoaaaaab
"
}
SubProgram "d3d11_9x " {
SetTexture 0 [_MainTex] 2D 0
SetTexture 1 [_Coc] 2D 1
"ps_4_0_level_9_1
eefiecedbknjjpenbplmhkdgdmedeaiogedafjcaabaaaaaaeaacaaaaaeaaaaaa
daaaaaaammaaaaaajmabaaaaamacaaaaebgpgodjjeaaaaaajeaaaaaaaaacpppp
giaaaaaacmaaaaaaaaaacmaaaaaacmaaaaaacmaaacaaceaaaaaacmaaaaaaaaaa
abababaaaaacppppbpaaaaacaaaaaaiaaaaaaplabpaaaaacaaaaaajaaaaiapka
bpaaaaacaaaaaajaabaiapkaecaaaaadaaaacpiaaaaaoelaaaaioekaecaaaaad
abaacpiaaaaaoelaabaioekaalaaaaadacaacpiaaaaaoeiaabaaoeiaabaaaaac
aaaicpiaacaaoeiappppaaaafdeieefcmiaaaaaaeaaaaaaadcaaaaaafkaaaaad
aagabaaaaaaaaaaafkaaaaadaagabaaaabaaaaaafibiaaaeaahabaaaaaaaaaaa
ffffaaaafibiaaaeaahabaaaabaaaaaaffffaaaagcbaaaaddcbabaaaabaaaaaa
gfaaaaadpccabaaaaaaaaaaagiaaaaacacaaaaaaefaaaaajpcaabaaaaaaaaaaa
egbabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaaefaaaaajpcaabaaa
abaaaaaaegbabaaaabaaaaaaeghobaaaabaaaaaaaagabaaaabaaaaaadeaaaaah
pccabaaaaaaaaaaaegaobaaaaaaaaaaaegaobaaaabaaaaaadoaaaaabejfdeheo
giaaaaaaadaaaaaaaiaaaaaafaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaa
apaaaaaafmaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadadaaaafmaaaaaa
abaaaaaaaaaaaaaaadaaaaaaabaaaaaaamaaaaaafdfgfpfagphdgjhegjgpgoaa
feeffiedepepfceeaaklklklepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklkl
"
}
}
 }
}
Fallback Off
}