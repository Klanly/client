Shader "Custom/Scene/jewel" {
	Properties {
		_MainScale ("亮度",range(1.0,20.0)) = 1.0
		_MainColor ("主色",color) = (1.0,1.0,1.0,1.0)
		_MainTex ("贴图", 2D) = "white" {}
		_Cmlpow("中心范围",range(2.0,10.0)) = 3.0
		_RimScale ("边光强度",range(0.0,20.0)) = 1.0
		_RimPow ("边光范围",range(0.1,30.0)) = 10.0
	}
	SubShader {
		Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Opaque"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		float4 _MainColor;
		float _RimScale;
		float _RimPow;
		float _MainScale;
		float _Cmlpow;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
			float3 worldNormal;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half4 c2 = tex2D (_MainTex, normalize(IN.viewDir) + 0.1 * c);
			half ch = saturate(dot(IN.worldNormal,normalize(IN.viewDir)));
			half rh = 1 - ch;
			half Rim = _RimScale * pow(rh , _RimPow);
			half Cml = pow (ch , _Cmlpow);
			o.Emission =_MainColor * c2.rgb * (Cml * _MainScale + Rim);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
