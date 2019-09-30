Shader "Custom/Scene/Water" {
	Properties {		
		_MainTex ("水底贴图", 2D) = "white" {}
		_WaterTex ("水流扰动RG_扰动,B_高光", 2D) = "black" {}
		_RflTex ("反射贴图", 2D) = "black" {}
		_WaterColor ("水体颜色",color) = (0.0,0.18,0.38,1.0)
		_WaterDepth ("水深",range(0.0,1.3)) = 1.0
		_SplColor ("高光颜色",color) = (1.0,1.0,1.0,1.0)
		_SplPow ("高光范围",range(0.1,5.0)) = 2.0
		_RflScale ("反射强度",range(0.0,0.5)) = 0.2
		_SpeedU ("水流速度U", range(-1.0,1.0)) = 0.0
		_SpeedV ("水流速度V", range(-1.0,1.0)) = 0.0
	}
	SubShader {
		Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Opaque"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		
		sampler2D _MainTex;
		sampler2D _WaterTex;
		sampler2D _RflTex;
		float4 _WaterColor;
		float _WaterDepth;
		float4 _SplColor;
		float _SplPow;
		float _RflScale;
		float _SpeedU;
		float _SpeedV;

		struct Input {
			float2 uv_MainTex;
			float2 uv_WaterTex;
			float2 uv_RflTex;
			float3 color:COLOR;
			float3 viewDir;
			float3 worldNormal;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 DisTex = tex2D (_WaterTex, IN.uv_WaterTex + float2(_SpeedU , _SpeedV) * _Time.y); 
			float2 UVDis = IN.uv_MainTex + 0.2 * _WaterDepth * IN.color * DisTex.rg;
			half4 c = tex2D (_MainTex, UVDis);
			half4 Rfl = tex2D (_RflTex, IN.uv_RflTex + 0.07 * _WaterDepth * DisTex.rg + 0.5 * normalize(IN.viewDir));
			half3 RView = reflect (IN.worldNormal , normalize(IN.viewDir));
			half4 Spl = 2.0 * pow(saturate (dot (RView, normalize(float3(0.0,1.0,0.0)))) , _SplPow); 
			half4 SplTex = tex2D (_WaterTex, IN.uv_WaterTex + 0.3 * DisTex);
			o.Albedo = lerp (c.rgb , _WaterColor , _WaterDepth * IN.color);
			o.Emission = IN.color * (_RflScale * Rfl + 5.0 * Spl * _SplColor * SplTex.b);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
