Shader "Custom/Scene/StaticMesh" {
	Properties {		
		_MainTex ("基本贴图", 2D) = "white" {}
	}
	SubShader {
		Tags {"Queue"="Geometry" "IgnoreProjector"="False" "RenderType"="Opaque"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float4 DifTex = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = DifTex.rgb;
			clip(DifTex.a - 0.5f);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
