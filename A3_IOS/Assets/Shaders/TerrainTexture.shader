Shader "Custom/Terrain/TerrainTexture" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Channel ("通道",range(0.0,1.0)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		fixed _Channel;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			if (_Channel < 0.5)
			{
				o.Emission = saturate(c.rgb);
			}
			else
			{
				o.Emission = saturate(c.a);
			}
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
