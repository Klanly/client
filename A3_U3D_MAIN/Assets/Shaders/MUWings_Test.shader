Shader "Custom/Char/MUWingsTest" {
	Properties {
		_MainTex ("贴图", 2D) = "white" {}
		_DLDensity ("流光密度",range(2.0,10.0)) = 3.0
		_DLColor ("流光颜色",color) = (0.8,0.5,0.5,0.0)
		_DLScale ("流光强度",range(0.0,1.0)) = 0.65
		_DLSpeed ("流光速度",range(-1.0,1.0)) = 0.1
	}
	SubShader {
		Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Opaque"}
		LOD 200
	
		CGPROGRAM
		#pragma surface surf Lambert
		sampler2D _MainTex;
		float _DLScale;
		float _DLDensity;
		float _DLSpeed;
		float _DLDir;
		float4 _DLColor;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half DL = abs (frac (_DLDensity * ((IN.uv_MainTex.x + 0.01 * sin (6.28 * frac (5.0 * (IN.uv_MainTex.y + 2 * _DLSpeed * _Time.y)))) + _DLSpeed * _Time.y)) - 0.5);
			o.Albedo = c.rgb;
			o.Emission = _DLScale * _DLColor.rgb * DL;
			clip (c.a - 0.5);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
