Shader "Custom/Scene/Emission" {
	Properties {		
		_MainTex ("基本贴图", 2D) = "white" {}
		_EmissionTex ("自发光贴图", 2D) = "black" {}
		_Color ("发光颜色",color) = (1.0,1.0,1.0,1.0)
		_Intensity ("发光强度",range(0.0,2.0)) = 0.5
		_Damping ("衰减",range(0.0,1.0)) = 0.5
	}
	SubShader {
		Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Opaque"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		
		sampler2D _MainTex;
		sampler2D _EmissionTex;
		float4 _Color;
		float _Intensity;
		fixed _Damping;

		struct Input {
			float2 uv_MainTex;
			float2 uv_EmissionTex; 
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float4 DifTex = tex2D (_MainTex, IN.uv_MainTex);
			half4 EmiTex = tex2D (_EmissionTex, IN.uv_EmissionTex);
			half Flash1 = abs(frac(0.5f * _Time.y) - 0.5);
			half Flash2 = abs(frac(0.2f * _Time.y) - 0.5);
			o.Albedo = DifTex.rgb;
			o.Emission = min(1.0 , (Flash1 + Flash2 + _Damping)) * _Intensity * _Color.rgb * EmiTex.rgb;
			clip(DifTex.a - 0.5f);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
