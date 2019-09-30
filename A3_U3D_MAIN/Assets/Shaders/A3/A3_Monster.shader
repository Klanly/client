Shader "A3/A3_Monster" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        _RimColor ("Rim Color", Color) = (0, 0, 0, 0)
        _RimWidth ("Rim Width", Float) = 0
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
	SubShader {    
	    Tags { "RenderType"="Opaque" }
		LOD 150
		CGPROGRAM
		#pragma surface surf Lambert noforwardadd
			
			sampler2D _MainTex; 	
			fixed4 _Color;	
			fixed4 _RimColor;
			fixed _RimWidth;	
			fixed _Cutoff;

			struct Input {
				float2 uv_MainTex;
				float3 viewDir;
				float3 worldNormal;
			};
				    
			void surf (Input IN, inout SurfaceOutput o) {
			fixed4 texcol = tex2D(_MainTex, IN.uv_MainTex);			
			texcol *= _Color;
				
			float dotProduct = 1 - dot(IN.worldNormal, normalize(IN.viewDir));                              	                   
	        float RimLight = smoothstep(1 - _RimWidth, 1.0, dotProduct);		
				
			o.Albedo = texcol.rgb;
			o.Emission = RimLight * _RimColor;
			o.Alpha = texcol.a;
			
			float x_2;
			x_2 = (texcol.a - _Cutoff);
			if ((x_2 < 0.0f)) {
				discard;
		};			
	}
	ENDCG 
	} 
	FallBack "Diffuse"
}