Shader "MobilityGames/Lightmapped/2TextureBlending" {
Properties {
    _MainTex ("Texture 0", 2D) = "white" { }
    _Tex1 ("Texture 1", 2D) = "white" { }
    _Blend("Blend", Range(0, 1)) = 0
    _LightMap ("LightMap", 2D) = "white" { }
    _LightIntensity ("LightIntensity", Float) = 1
}
SubShader {
    Pass {

CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

sampler2D _MainTex;
sampler2D _Tex1;
float _Blend;
sampler2D _LightMap;

struct appdata {
    float4 vertex : POSITION;
    float4 texcoord : TEXCOORD0;
    float4 texcoord1: TEXCOORD1;
};

struct v2f {
    float4  pos : SV_POSITION;
    float2  uv : TEXCOORD0;
    float2 lightUV : TEXCOORD1;
};

float4 _MainTex_ST;
float4 _LightMap_ST;
float _LightIntensity;

v2f vert (appdata v)
{
    v2f o;
    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
    o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
    o.lightUV = TRANSFORM_TEX (v.texcoord1, _LightMap);
    return o;
}

half4 frag (v2f i) : COLOR
{
    half4 texcol = lerp(tex2D (_MainTex, i.uv), tex2D (_Tex1, i.uv), _Blend);   
    half4 lightCol = tex2D (_LightMap, i.lightUV) * 2 * _LightIntensity;
    return (texcol * lightCol);
}
ENDCG

    }
}
Fallback "VertexLit"
} 