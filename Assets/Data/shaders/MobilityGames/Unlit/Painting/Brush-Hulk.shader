Shader "MobilityGames/Unlit/Painting/Brush-Hulk" {
	Properties {
		_ColorTex ("Color", 2D) = "white" {}
		_BlackTex ("Grayscale", 2D) = "white" {}
		_Mask ("Mask", 2D) = "white" {}
	}
	SubShader {
	
		Pass {
			ZWrite Off
			Alphatest Greater 0
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			sampler2D _ColorTex;
			sampler2D _BlackTex;
			sampler2D _Mask;
			
			struct appdata {
			    float4 vertex : POSITION;
			    float4 texcoord : TEXCOORD0;
			};
			
			struct v2f {
			    float4  pos : SV_POSITION;
			    float2  uv : TEXCOORD0;
			};
			
			float4 _ColorTex_ST;
			float4 _BlackTex_ST;
			float4 _Mask_ST;
			
			v2f vert (appdata v)
			{
			    v2f o;
			    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			    o.uv = TRANSFORM_TEX (v.texcoord, _ColorTex);
			    return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
				half4 c  = tex2D(_ColorTex, i.uv);
				half4 ab = tex2D(_BlackTex, i.uv);
				half4 m  = tex2D(_Mask, i.uv);

				half4 modulated = ab * (m + 0.25);
				modulated.a = ab.a;
				half4 resultColor = lerp(ab, modulated, m.a);
				return resultColor;
			}
			ENDCG
		}		
	} 
}
