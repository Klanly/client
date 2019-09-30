// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Char/MUWings" {
	Properties {
		_MainTex ("贴图", 2D) = "white" {}
		_DLDensity ("流光密度",range(2.0,10.0)) = 3.0
		_DLColor ("流光颜色",color) = (0.8,0.5,0.5,0.0)
		_DLScale ("流光强度",range(0.0,1.0)) = 0.65
		_DLSpeed ("流光速度",range(-1.0,1.0)) = 0.1
		_MainScale ("亮度",range(0.0,3.0)) = 0.8
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		
		Pass {
			ZWrite Off
			Blend One One
			Fog { Color (0,0,0,0) }
			   
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float _DLScale;
			float _DLDensity;
			float _DLSpeed;
			float _DLDir;
			float4 _DLColor;
			float _MainScale;

	        struct v2f {
	            float4 pos : SV_POSITION;
	            float2 uv : TEXCOORD0;
	        };

	        float4 _MainTex_ST;

	        v2f vert (appdata_full v)
	        {
	            v2f o;
	            o.pos = UnityObjectToClipPos (v.vertex);
	            o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
	            return o;
	        }
			
			fixed4 frag (v2f i) : COLOR
			{
				half4 c = tex2D (_MainTex, i.uv);
				half DL = 2.0f * abs(frac (_DLDensity * (i.uv.y + 0.3f * (c.r+c.g+c.b) + _DLSpeed * _Time.y)) - 0.5);
				return _MainScale * c.a * (_MainScale * c + _DLScale * _DLColor * DL);
			}
			ENDCG
		}  
	} 
	Fallback "VertexLit"
}
