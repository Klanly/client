Shader "A3/Hide" {
	Properties {
		_RimColor ("颜色",color) = (0.0,1.0,0.0,1.0)
		_RimPow ("边界范围",range(0.1,4.0)) = 1
		_CubeTex ("环境贴图",CUBE) = "" {}
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200	
		CGPROGRAM
		#pragma surface surf Lambert alpha
		
		float _RimPow;
		float4 _RimColor;
		samplerCUBE _CubeTex;
		struct Input {
			float3 viewDir;
			float3 worldNormal;
			float3 worldRefl;			
		};
		
		void surf (Input IN, inout SurfaceOutput o) {
			half ch = saturate(dot(IN.worldNormal,normalize(IN.viewDir)));
			float Env = texCUBE (_CubeTex, IN.worldRefl).g;	
			half rh = 1.0 - ch;
			half Rim = pow(rh , _RimPow);			
			o.Emission = _RimColor;
			o.Alpha = Rim + Env;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
