Shader "Custom/Fx/AddFade" {
	Properties {
		_MainTex ("基础贴图", 2D) = "white" {}
		_MainColor ("基础颜色",color) = (1.0,1.0,1.0,1.0)
	}
	SubShader {
		Tags {
      	"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
     	CGPROGRAM
		#pragma surface surf Lambert decal:add

		sampler2D _MainTex;
		float4 _MainColor;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Emission = _MainColor * c.rgb * c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
