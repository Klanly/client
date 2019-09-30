Shader "Custom/Scene/Lava" {
	Properties {
		_MainTex ("基本贴图", 2D) = "white" {}
		_RimScale ("边光强度" ,range(0.0,20.0)) = 2.0
		_RimColor ("边光颜色" ,color) = (1.0,1.0,1.0,1.0)
		_Rimpow ("边光范围" ,range(0.1,5.0)) = 10.0
		_DstThreshold ("扭曲强度" ,range(0.0,1.0)) = 0.1
		_AnimationU ("流速U" ,range(-0.5,0.5)) = 0.0
		_AnimationV ("流速V" ,range(-0.5,0.5)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert
		
		sampler2D _MainTex;
		float _RimScale;
		float4 _RimColor;
		float _Rimpow;
		float _AnimationU;
		float _AnimationV;
		float _DstThreshold;
		
		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
			float3 worldNormal;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex + 0.5f * _Time.y * float2(_AnimationU , _AnimationV) + _DstThreshold * tex2D (_MainTex, IN.uv_MainTex + _Time.y * float2(_AnimationU , _AnimationV)));
			float nh = saturate (dot (IN.worldNormal, normalize(IN.viewDir)));
			float eh =1 - nh;
		    float rim = pow(eh,_Rimpow);
		    float3 RimColor = _RimScale * _RimColor * rim * c.rgb;
			o.Emission = c + RimColor;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
