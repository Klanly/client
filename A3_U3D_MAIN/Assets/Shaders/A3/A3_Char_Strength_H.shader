Shader "A3/A3_Char_Strenght_H" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
		_SubTex ("R边光G高光B流光A变色",2D) =  "black" {}
		_CubeTex ("环境贴图",CUBE) = "" {}
		_SplColor("高光颜色",color) = (1.0,1.0,1.0,1.0)
		_SplRim ("高光强度X,范围Y,边光强度Z,范围W",Vector) = (1,0.5,1,0.5)
		_ChangeColor("变色RGB,开关A",color) = (1.0,1.0,1.0,0.0)
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
	    	Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Opaque"}
	    	CGPROGRAM
			#pragma surface surf Lambert
			
			sampler2D _MainTex; 
			sampler2D _SubTex; 	
			samplerCUBE _CubeTex;
			fixed4 _SplColor;	
			fixed4 _SplRim;
			fixed _Cutoff;
			fixed4 _ChangeColor;
			
			struct Input {
				float2 uv_MainTex;
				float2 uv_SubTex;
				float3 viewDir;
				float3 worldNormal;
				float3 worldRefl;
			};
			
			void surf (Input IN, inout SurfaceOutput o) {
				float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
				float4 SubTex = tex2D(_SubTex, IN.uv_SubTex); 
				float3 Env = texCUBE (_CubeTex, IN.worldRefl).rgb;											
				if (_ChangeColor.a > 0.5)
				{
					MainTex.rgb = lerp(MainTex.rgb ,dot(MainTex.rgb,0.4)*_ChangeColor.rgb,SubTex.a);
				};
				float SplLight =  smoothstep(1 - _SplRim.y, 1.0, max(dot(IN.worldNormal, normalize(IN.viewDir)),0.0)); 
				float dotProduct = 1 - dot(IN.worldNormal, normalize(IN.viewDir));                              	                   
		        float RimLight = smoothstep(1 - _SplRim.w, 1.0, dotProduct);
				
				float3 Spl = MainTex.rgb * SubTex.g * SplLight * _SplRim.x * lerp(0.5,Env,SubTex.g)* _SplColor.rgb;
				float3 Rim = MainTex.rgb * SubTex.r * RimLight * _SplRim.z;
						
				o.Albedo = MainTex.rgb + Spl + Rim;
				//o.Emission = Spl + Rim;
				o.Alpha = MainTex.a;
				
				float x_2;
				x_2 = (MainTex.a - _Cutoff);
				if ((x_2 < 0.0f)) {
					discard;
			};			
		}
		ENDCG 
	} 
	FallBack "Diffuse"
}