// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "A3/A3_Char_Normal" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
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
			fixed _Cutoff;
            fixed _SHLightingScale;  
            
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldRefl : TEXCOORD1;
                fixed3 SHLighting: TEXCOORD2;
            };	
            		
			float4 _StrTex_ST;
			
            v2f vert (appdata_base v) {
                v2f o;
                o.pos = UnityObjectToClipPos (v.vertex);               
                o.uv = v.texcoord.xy;
                float3 worldNormal = mul((float3x3)unity_ObjectToWorld, v.normal); 
                o.worldRefl = normalize(reflect(normalize(WorldSpaceViewDir(v.vertex)), float3(1,-1,1)*worldNormal));    
            	o.SHLighting = ShadeSH9(float4(worldNormal,1)) * _SHLightingScale;            	
                return o;
            }             

            fixed4 frag(v2f i) : COLOR {            
                float4 MainTex = tex2D(_MainTex, i.uv);						
				float3 Dif = MainTex.rgb * i.SHLighting;		
				float x_2;
				x_2 = (MainTex.a - _Cutoff);
				if ((x_2 < 0.0f)) {
					discard;
				};
				float4 Output = float4 (Dif, MainTex.a);
                return Output; 
            }
        ENDCG
        }
    }
}