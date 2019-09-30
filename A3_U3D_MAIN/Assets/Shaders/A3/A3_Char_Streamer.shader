Shader "A3/A3_Char_Streamer" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
		_SubTex ("R边光G高光B流光",2D) =  "black" {}
		_StrColor("流光颜色",color) = (1.0,1.0,1.0,1.0)
		_SplRim ("高光强度X,范围Y,边光强度Z,范围W",Vector) = (1,0.5,1,0.5)
		_StrAnim ("流光U\V流动速度,强度",Vector) = (0,0,1,0)
		_ChangeColor("变色RGB,开关A",color) = (1.0,1.0,1.0,0.0)
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
    
    Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Opaque"}
	    	CGPROGRAM
			#pragma surface surf Lambert
			
			sampler2D _MainTex; 
			sampler2D _SubTex; 	
			fixed4 _StrColor;	
			fixed4 _SplRim;
			fixed4 _StrAnim;
			fixed4 _ChangeColor;
			fixed _Cutoff;
			
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
				float StrTex = tex2D(_SubTex, 0.05 * IN.worldRefl.zy + _StrAnim.xy * _Time.x).b;							
				if (_ChangeColor.a > 0.5)
				{
					MainTex.rgb = lerp(MainTex.rgb ,dot(MainTex.rgb,0.4)*_ChangeColor.rgb,SubTex.a);
				};
				//float dotProduct = 1 - dot(IN.worldNormal, normalize(IN.viewDir));                              	                   
		        //float RimLight = smoothstep(1 - _SplRim.w, 1.0, dotProduct);
				//float3 Rim = MainTex * SubTex.r * RimLight * _SplRim.z;
				float dotSpl = dot(IN.worldNormal, normalize(IN.viewDir));                              	                   
		        float SplLight = smoothstep(1 - _SplRim.y, 1.0, dotSpl);
				float3 Spl = MainTex * SubTex.g * SplLight * _SplRim.x;
				float3 Streamer = MainTex * (1.0f - SubTex.r) * StrTex * _StrColor.rgb * _StrAnim.z;	
						
				o.Albedo = MainTex.rgb;
				//o.Emission = Streamer + Rim;
				o.Emission = Streamer + Spl;
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
