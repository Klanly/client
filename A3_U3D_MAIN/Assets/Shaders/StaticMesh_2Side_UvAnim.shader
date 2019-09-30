Shader "Custom/Scene/StaticMesh_2Side_UVAnim" {
	Properties {		
		_MainTex ("基本贴图", 2D) = "white" {}
		SpeedU ("水平移动",float) = 0.0
		SpeedV ("竖直移动",float) = 0.0
		OffScale ("偏移倍率",float) = 0.0
	}
	SubShader {
		Tags {"Queue"="Geometry" "IgnoreProjector"="False" "RenderType"="Opaque"}
		LOD 200
		Cull off
		
		CGPROGRAM
		#pragma surface surf Lambert

		
		sampler2D _MainTex;
		fixed SpeedU;
		fixed SpeedV;
		float OffScale;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float4 DifTex = tex2D (_MainTex, IN.uv_MainTex + OffScale * frac(fixed2(SpeedU , SpeedV) * _Time.y));
			o.Albedo = DifTex.rgb;
			clip(DifTex.a - 0.5f);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
