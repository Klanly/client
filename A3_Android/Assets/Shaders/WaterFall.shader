Shader "Custom/Scene/Waterfall" {
	Properties {
		_MainTex ("基础贴图", 2D) = "white" {}
		_AlphaScale ("透明度调节",range(0.0,1.0)) = 1.0
		_AnimSpeedU ("流速调节U",range(-2.0,2.0)) = 0.0
		_AnimSpeedV ("流速调节V",range(-2.0,2.0)) = 0.0
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		ZWrite Off
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		float _AlphaScale;
		float _AnimSpeedU;
		float _AnimSpeedV;

		struct Input {
			float2 uv_MainTex;
			float2 uv_DisTex;
			float4 color : COLOR;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex + _Time.y * float2(_AnimSpeedU,_AnimSpeedV));
			half4 c2 = tex2D (_MainTex, IN.uv_MainTex + 0.6 * _Time.y * float2(_AnimSpeedU,_AnimSpeedV) - 0.2);
			o.Emission = 0.5 * c.rgb + 0.5 * c2.rgb;
			o.Alpha = _AlphaScale * IN.color;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
