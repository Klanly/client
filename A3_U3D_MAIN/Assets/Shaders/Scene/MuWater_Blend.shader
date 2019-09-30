// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Scene/MuWater_Blend" {
	Properties {		
		_WaterTex1 ("水贴图1", 2D) = "black" {}
		_WaterTex2 ("水贴图2", 2D) = "black" {}
		_AlphaScale ("透明度",float) = 0.5
		_Speed ("流速xy水1uv_zw水2uv",Vector) = (0.02,0.0,-0.02,0.0)		
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		
		Pass {
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Fog { Color (0,0,0,0) }
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			sampler2D _WaterTex1;
			sampler2D _WaterTex2;
			float _AlphaScale;
			float4 _Speed;

	        struct v2f {
	            float4 pos : SV_POSITION;
	            float2 uv_WaterTex1 : TEXCOORD0;
	            float2 uv_WaterTex2 : TEXCOORD1;
	            float4 color : COLOR;
	        };

	        float4 _WaterTex1_ST;
	        float4 _WaterTex2_ST;

	        v2f vert (appdata_full v)
	        {
	            v2f o;
	            o.pos = UnityObjectToClipPos (v.vertex);
	            o.uv_WaterTex1 = TRANSFORM_TEX (v.texcoord, _WaterTex1);
	            o.uv_WaterTex2 = TRANSFORM_TEX (v.texcoord, _WaterTex2);
	            o.color = v.color;
	            return o;
	        }
			
			fixed4 frag (v2f i) : COLOR
			{
				half4 WaterTex1 = tex2D (_WaterTex1, (i.uv_WaterTex1 + _Time.y * _Speed.xy));
				half4 WaterTex2 = tex2D (_WaterTex2, (i.uv_WaterTex2 + _Time.y * _Speed.zw));
            	return half4(0.5f * (WaterTex1 + WaterTex2).rgb,i.color.a * _AlphaScale);
			}
			ENDCG
		}  
	} 
	Fallback "VertexLit"
}
