Shader "Custom/Scene/Ice" {
	Properties {
		_MainTex ("贴图", 2D) = "white" {}
		_RflScale ("反射比率",range(0.0,1.0)) = 0.68
		_RimScale ("边光强度",range(0.0,10.0)) = 3.75
		_RimPow ("边光范围",range(0.1,30.0)) = 7.58
		_Alphaoffset ("透明偏移",range(0.5,1.0)) = 0.875
		_AlphaPow ("透明范围",range(0.1,10.0)) = 2.22
		_Posui ("破碎阈值",range(-0.01,1.0)) = 0.0
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		ZWrite Off
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		float _RimScale;
		float _RimPow;
		float _AlphaPow;
		float _Alphaoffset;
		float _RflScale;
		float _Posui;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
			float3 worldNormal;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half4 c2 = tex2D (_MainTex, normalize(IN.viewDir) + 0.1 * c.rgb);
			half ch = saturate(dot(IN.worldNormal,normalize(IN.viewDir)));
			half rh = 1.0 - ch;
			half Rim = _RimScale * pow(rh , _RimPow);
			half Alp = saturate (pow(rh , _AlphaPow) + _Alphaoffset);
			half Test = c.b - _Posui;			
			o.Emission =lerp (c.rgb , c2.rgb , _RflScale) + Rim;
			if (Test > 0)
			{
				o.Alpha = saturate (pow(rh , _AlphaPow) + _Alphaoffset);
			}
			else
			{
				o.Alpha = 0.0;	
			}			
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
