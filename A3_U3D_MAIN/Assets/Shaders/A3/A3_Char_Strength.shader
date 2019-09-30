Shader "A3/A3_Char_Strenght" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
		_SubTex ("R边光G高光B流光",2D) =  "black" {}
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
		fixed4 _SplColor;	
		fixed4 _SplRim;
		fixed4 _ChangeColor;
		fixed _Cutoff;
		
		struct Input {
			float2 uv_MainTex;
			float2 uv_SubTex;
			float3 viewDir;
			float3 worldNormal;
		};
		
		void surf (Input IN, inout SurfaceOutput o) {
		
			float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
			float4 SubTex = tex2D(_SubTex, IN.uv_SubTex); 
			if (_ChangeColor.a > 0.5)
			{
				MainTex.rgb = lerp(MainTex.rgb ,dot(MainTex.rgb,0.4)*_ChangeColor.rgb,SubTex.a);
			};					
			float SplLight =  smoothstep(1 - _SplRim.y, 1.0, max(dot(normalize(IN.worldNormal), normalize(IN.viewDir)),0.0)); 
			float dotProduct = 1 - dot(IN.worldNormal, normalize(IN.viewDir));                              	                   
	        float RimLight = smoothstep(1 - _SplRim.w, 1.0, dotProduct);
			
			float3 Spl = MainTex.rgb * SubTex.g * SplLight * _SplRim.x * _SplColor.rgb;
			float3 Rim = MainTex.rgb * SubTex.r * RimLight * _SplRim.z;
					
			o.Albedo = MainTex.rgb + Spl + Rim;
			o.Alpha = MainTex.a;
			
			float x_2;
			x_2 = (MainTex.a - _Cutoff);
			if ((x_2 < 0.0f)) {
				discard;
		};			
	}
	ENDCG 
} 
    	
//        Pass {
//        Tags {"LightMode"="ForwardBase"}
//       		Lighting Off
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//            #include "UnityCG.cginc"
//            
//            struct appdata {
//                float4 vertex : POSITION;
//                float3 normal : NORMAL;
//                float2 texcoord : TEXCOORD0;
//            };
//            
//            sampler2D _MainTex; 
//			sampler2D _SubTex; 
//			fixed4 _SplColor;	
//			fixed4 _SplRim;
//			fixed _Cutoff;
//            fixed _SHLightingScale;
//
//
//            struct v2f {
//                float4 pos : SV_POSITION;
//                float2 uv : TEXCOORD0;
//                fixed3 SHLighting: TEXCOORD2;
//                fixed3 color : COLOR;
//            };	
//			
//            v2f vert (appdata_base v) {
//                v2f o;
//                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
//                float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));                
//                float dotProduct = 1 - dot(v.normal, viewDir);                                
//                float SplLight =  smoothstep(1 - _SplRim.y, 1.0, max(dot(v.normal, viewDir),0.0));                              	                   
//                float RimLight = smoothstep(1 - _SplRim.w, 1.0, dotProduct);
//                o.color.r = SplLight;
//                o.color.g = RimLight;				
//                o.uv = v.texcoord.xy;
//                float3 worldNormal = mul((float3x3)_Object2World, v.normal); 
//            	o.SHLighting = ShadeSH9(float4(worldNormal,1)) * _SHLightingScale;
//                return o;
//            }             
//
//            fixed4 frag(v2f i) : COLOR {            
//                float4 MainTex = tex2D(_MainTex, i.uv);
//				float3 SubTex = tex2D(_SubTex, i.uv).rgb; 							
//				float3 Dif = MainTex.rgb * i.SHLighting;
//				float3 Spl = MainTex.rgb * SubTex.g * i.color.r * _SplRim.x * i.SHLighting * _SplColor.rgb;
//				float3 Rim = MainTex.rgb * SubTex.r * i.color.g * _SplRim.z * i.SHLighting;
//				float4 Output = float4 (Dif + Spl + Rim, MainTex.a);
//				
//				float x_2;
//				x_2 = (MainTex.a - _Cutoff);
//				if ((x_2 < 0.0f)) {
//					discard;
//				};
//                return Output; 
//            }
//        ENDCG
//        }
	FallBack "Diffuse"
}