//Shader "Custom/Char/DynamicShadow" {
//	Properties {
//		_MainTex ("Base (RGB)", 2D) = "white" {}
//		_Shadow ("阴影深度",range(0.0,1.0)) = 0.8
//	}
//	SubShader {
//		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
//		LOD 200
//		Zwrite off
//				
//		CGPROGRAM
//		#pragma surface surf Lambert alpha
//
//		sampler2D _MainTex;
//		float _Shadow;
//
//		struct Input {
//			float2 uv_MainTex;
//		};
//
//		void surf (Input IN, inout SurfaceOutput o) {
//			half4 c = tex2D (_MainTex, IN.uv_MainTex);
//			o.Albedo = 0;
//			o.Alpha = lerp(0, _Shadow , min(1,ceil(c.r+c.g+c.b-0.001)));
//		}
//		ENDCG
//	} 
//	FallBack "Diffuse"
//}
Shader "Custom/Char/DynamicShadow" { 
	Properties {
		_ShadowColor ("影子颜色",color) = (0.4,0.4,0.4,1.0)
		_ShadowTex ("阴影序列", 2D) = "gray" {}
		_OffSet ("模糊偏移",range(0,0.005)) = 0.001	
		_FalloffTex ("FallOff", 2D) = "white" {}
	}
	Subshader {
		Tags {"Queue"="Transparent"}
		Pass {
			ZWrite Off
			Fog { Color (1, 1, 1) }
			AlphaTest Greater 0
			ColorMask RGB
			Blend DstColor Zero
			Offset -1, -1
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct v2f {
				float4 uvShadow : TEXCOORD0;
				float4 uvFalloff : TEXCOORD1;
				float4 pos : SV_POSITION;
			};
			
			float4x4 _Projector;
			float4x4 _ProjectorClip;
			float4 _ShadowColor;
			
			v2f vert (float4 vertex : POSITION)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, vertex);
				o.uvShadow = mul (_Projector, vertex);
				o.uvFalloff = mul (_ProjectorClip, vertex);
				return o;
			}
			
			sampler2D _ShadowTex;
			sampler2D _FalloffTex;
			float _OffSet;
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 texS = tex2Dproj (_ShadowTex, UNITY_PROJ_COORD(i.uvShadow));
				texS.a = 1.0-texS.a;
				fixed4 texF = tex2Dproj (_FalloffTex, UNITY_PROJ_COORD(i.uvFalloff));
				fixed4 res = lerp(fixed4(1,1,1,0), fixed4(_ShadowColor.rgb,1.0) , texF.a * texS.a);
				return res;
				
//				fixed texS1 = 1.0 * (1 - tex2Dproj (_ShadowTex, UNITY_PROJ_COORD(i.uvShadow) + _OffSet).a);
//				fixed texS2 = 1.0 * (1 - tex2Dproj (_ShadowTex, UNITY_PROJ_COORD(i.uvShadow) - _OffSet).a);
//				fixed texS3 = 0.6 * (1 - tex2Dproj (_ShadowTex, UNITY_PROJ_COORD(i.uvShadow) + 2 * _OffSet).a);
//				fixed texS4 = 0.6 * (1 - tex2Dproj (_ShadowTex, UNITY_PROJ_COORD(i.uvShadow) - 2 * _OffSet).a);
//				fixed texS5 = 0.3 * (1 - tex2Dproj (_ShadowTex, UNITY_PROJ_COORD(i.uvShadow) + 3 * _OffSet).a);
//				fixed texS6 = 0.3 * (1 - tex2Dproj (_ShadowTex, UNITY_PROJ_COORD(i.uvShadow) - 3 * _OffSet).a);
//				fixed texS = min (1.0,texS1 + texS2 + texS3 + texS4 + texS5 + texS6); 
//				fixed4 texF = tex2Dproj (_FalloffTex, UNITY_PROJ_COORD(i.uvFalloff));
//				fixed4 res = lerp(fixed4(1,1,1,0), fixed4(_ShadowColor.rgb,1.0) , texF.a * texS);
//				return res;
			}
			ENDCG
		}
	}
}
