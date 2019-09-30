// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Scene/Waterfall" {
	Properties {
		_MainTex ("基础贴图", 2D) = "white" {}
		_AlphaScale ("透明度调节",range(0.0,1.0)) = 1.0
		_AnimSpeedU ("流速调节U",range(-2.0,2.0)) = 0.0
		_AnimSpeedV ("流速调节V",range(-2.0,2.0)) = 0.0
		_SpInt ("高光亮度",range(0.0,1.0)) = 0.5
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		Pass {
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			   
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float _AlphaScale;
			float _AnimSpeedU;
			float _AnimSpeedV;
			float _SpInt;

	        struct v2f {
	            float4 pos : SV_POSITION;
	            float2 uv : TEXCOORD0;
	            float4 color : COLOR;
	        };

	        float4 _MainTex_ST;

	        v2f vert (appdata_full v)
	        {
	            v2f o;
	            o.pos = UnityObjectToClipPos (v.vertex);
	            o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
	            o.color = v.color;
	            return o;
	        }
			
			fixed4 frag (v2f i) : COLOR
			{
				half4 c = tex2D (_MainTex, i.uv + _Time.y * float2(_AnimSpeedU,_AnimSpeedV));
				half4 c2 = tex2D (_MainTex, i.uv + 0.6 * _Time.y * float2(_AnimSpeedU,_AnimSpeedV) - 0.2);
				half3 WaterColor = (0.5 * (c.rgb + c2.rgb) + 100.0f * _SpInt * pow(c.r * c2.r , 3.0f)).rgb;
				half WaterAlpha = _AlphaScale * i.color * (1.0f + 100.0f * _SpInt * pow(c.r * c2.r , 3.0f)).r;
            	return float4(WaterColor,WaterAlpha);
			}
			ENDCG
		}  
	} 
	Fallback "VertexLit"
}
