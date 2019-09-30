Shader "Custom/Show_VCAlpha" {
	Properties {
		_Control ("左VC右VC.a", range(0,1)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		fixed _Control;

		struct Input {
			float4 color:COLOR;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			if (_Control<0.5)
			{
				o.Emission = IN.color.rgb;
			}
			else
			{
				o.Emission = IN.color.a;
			}
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
