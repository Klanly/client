Shader "Custom/Fx/AddFade_Tex4" {
	Properties {
		_Intensity ("整体亮度", range(0.0,2.0)) = 1.0
		_MainTex1 ("底层贴图", 2D) = "black" {}
		_MainColor1 ("底层颜色",color) = (1.0,1.0,1.0,1.0)
		_MainTex2 ("下层贴图", 2D) = "black" {}
		_MainColor2 ("下层颜色",color) = (1.0,1.0,1.0,1.0)
		_MainTex3 ("上层贴图", 2D) = "black" {}
		_MainColor3 ("上层颜色",color) = (1.0,1.0,1.0,1.0)
		_MainTex4 ("顶层贴图", 2D) = "black" {}
		_MainColor4 ("顶层颜色",color) = (1.0,1.0,1.0,1.0)
	}
	SubShader {
		Tags {
      	"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
     	CGPROGRAM
		#pragma surface surf Lambert decal:add

		fixed _Intensity;
		sampler2D _MainTex1;
		half4 _MainColor1;
		sampler2D _MainTex2;
		half4 _MainColor2;
		sampler2D _MainTex3;
		half4 _MainColor3;
		sampler2D _MainTex4;
		half4 _MainColor4;

		struct Input {
			float2 uv_MainTex1;
			float2 uv_MainTex2;
			float2 uv_MainTex3;
			float2 uv_MainTex4;
			float4 color:COLOR;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half3 c1 = tex2D (_MainTex1, IN.uv_MainTex1).rgb * _MainColor1.rgb;
			half3 c2 = tex2D (_MainTex2, IN.uv_MainTex2).rgb * _MainColor2.rgb;
			half3 c3 = tex2D (_MainTex3, IN.uv_MainTex3).rgb * _MainColor3.rgb;
			half3 c4 = tex2D (_MainTex4, IN.uv_MainTex4).rgb * _MainColor4.rgb;
			half3 Light1 = max (fixed3(0.0f,0.0f,0.0f) , (c1 - c2 - c3 - c4));
			half3 Light2 = max (fixed3(0.0f,0.0f,0.0f) , (c2 - c3 - c4));
			half3 Light3 = max (fixed3(0.0f,0.0f,0.0f) , (c3 - c4));
			o.Emission = _Intensity * (Light1 + Light2 + Light3 + c4) * IN.color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
