Shader "Custom/Scene/Cloud_Add" {
	Properties {
		
		_MainColor ("颜色",color) = (1.0,1.0,1.0,1.0)
		_MainTex ("贴图", 2D) = "white" {}
		_AlphaCtl ("透明通道",range(0.0,1.0)) = 1.0
		_AlphaScale("透明度调节",range(0.0,3.0)) = 1.0
		_AnimU ("流动速度U",range(-0.2,0.2)) = 0.0
		_AnimV ("流动速度V",range(-0.2,0.2)) = 0.0
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		ZWrite Off
		
		CGPROGRAM
		#pragma surface surf Lambert decal:add

		sampler2D _MainTex;
		float4 _MainColor;
		float _AlphaCtl;
		float _AlphaScale;
		float _AnimU;
		float _AnimV;

		struct Input {
			float2 uv_MainTex;
			float4 color : COLOR;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex + _Time.y * float2(_AnimU , _AnimV));
			o.Emission = _MainColor * c.rgb * _AlphaScale;		
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
