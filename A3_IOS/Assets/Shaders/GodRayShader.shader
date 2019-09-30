Shader "Custom/Scene/GodRayShader" {
	Properties {
		_Color ("基本色",color) = (1.0,1.0,1.0,1.0)
		_MainTex ("基本贴图", 2D) = "white" {}
		_MainScale ("基本强度",range(0,2)) = 0.5
		_GrowSize ("闪烁阈值",range(0,1)) = 0.2
		_GrowRate ("闪烁频率",range(0,3)) = 1.0
		_GrowScale ("闪烁强度",range(0,1)) = 0.3
	}
	SubShader {
		Tags { "RenderType"="Transparent" "IgnoreProjector"="True" "Queue" = "Transparent"}
		Cull off
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert decal:add

		sampler2D _MainTex;
		float4 _Color;
		float _MainScale;
		float _GrowSize;
		float _GrowRate;
		float _GrowScale;

		struct Input {
			float2 uv_MainTex;
			float4 color:COLOR;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half wave = _GrowScale * (abs(frac(_GrowRate * _Time.y)-0.5) + saturate (abs(_SinTime.z) + 0.4));
			o.Emission = _Color * (_MainScale * c + saturate(c - _GrowSize) * wave) * IN.color;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
