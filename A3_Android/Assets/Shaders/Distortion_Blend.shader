Shader "Custom/Fx/Distortion_Blend" {
	Properties {
		_MainColor ("基础颜色",color) = (1.0,1.0,1.0,1.0)
		_MainScale ("亮度",range(0.0,5.0)) = 1.0
		_MainTex ("基础贴图", 2D) = "white" {}
		_AlphaCtl ("透明通道",range(0.0,1.0)) = 1.0
		_AlphaScale("透明度调节",range(0.0,1.0)) = 1.0
		_DistortionScaleX("扭曲强度U",range(0.0,5.0)) = 0.1
		_DistortionScaleY("扭曲强度V",range(0.0,5.0)) = 0.1
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		//Cull Off
		ZWrite Off
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		float4 _MainColor;
		float _DistortionScaleX;
		float _DistortionScaleY;
		float _AlphaScale;
		float _MainScale;
		float _AlphaCtl;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half4 c2 = tex2D (_MainTex, IN.uv_MainTex + float2(_DistortionScaleX , _DistortionScaleY) * c);
			o.Emission = _MainScale * _MainColor * c2.rgb;
			if (_AlphaCtl > 0.5)
			{
				o.Alpha = min (_AlphaScale * c2.a , 1.0);
			}
			else
			{
				o.Alpha = min (_AlphaScale * c2.r , 1.0);
			}
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
