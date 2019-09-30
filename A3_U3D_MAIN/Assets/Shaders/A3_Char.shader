// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "A3/A3_Char" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
		_SubTex ("高光图R透贴G高光B边光",2D) =  "purple" {}
		_CubeTex ("",CUBE) = "" {}
		_SplRim ("高光强度X,范围Y,边光强度Z,范围W",Vector) = (1,0.5,1,0.5)
		_Older ("旧化程度",Range(0,1)) = 0.5
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		_SHLightingScale("LightProbe influence scale",float) = 1
    }
    SubShader {
        Pass {
        Tags {"LightMode"="ForwardBase"}
       		Lighting Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };
            
            sampler2D _MainTex; 
			sampler2D _SubTex; 		
			samplerCUBE _CubeTex;	
			fixed4 _SplRim;
			fixed _Cutoff;
            fixed _SHLightingScale;
            fixed _Older;

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldRefl : TEXCOORD1;
                fixed3 SHLighting: TEXCOORD2;
                fixed3 color : COLOR;
            };			

            v2f vert (appdata_base v) {
                v2f o;
                o.pos = UnityObjectToClipPos (v.vertex);
                float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));                
                float dotProduct = 1 - dot(v.normal, viewDir);                                
                float SplLight =  smoothstep(1 - _SplRim.y, 1.0, max(dot(v.normal, viewDir),0.0));                              	                   
                float RimLight = smoothstep(1 - _SplRim.w, 1.0, dotProduct);
                o.color.r = SplLight;
                o.color.g = RimLight;				
                o.uv = v.texcoord.xy;
                float3 worldNormal = mul((float3x3)unity_ObjectToWorld, v.normal); 
                o.worldRefl = normalize(reflect(normalize(WorldSpaceViewDir(v.vertex)), float3(1,-1,1)*worldNormal));    
            	o.SHLighting = ShadeSH9(float4(worldNormal,1)) * _SHLightingScale;
                return o;
            }             

            fixed4 frag(v2f i) : COLOR {            
                float3 MainTex = tex2D(_MainTex, i.uv).rgb;
				float3 SubTex = tex2D(_SubTex, i.uv).rgb; 
				float3 Env = texCUBE (_CubeTex, i.worldRefl).rgb;	
							
				float3 Dif = MainTex * i.SHLighting;
				float3 Spl = MainTex * SubTex.g * i.color.r * _SplRim.x * i.SHLighting * lerp(Env,0.4,_Older);
				float3 Rim = MainTex * SubTex.b * i.color.g * _SplRim.z * i.SHLighting;
				float4 Output = float4 (Dif + Spl + Rim , SubTex.r);
				
				float x_2;
				x_2 = (SubTex.r - _Cutoff);
				if ((x_2 < 0.0f)) {
					discard;
				};
                return Output;     
                //return float4(Env,1.0);
            }
        ENDCG
        }
    }
}