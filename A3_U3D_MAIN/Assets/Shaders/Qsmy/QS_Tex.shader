// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Qsmy/QS_Tex" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        _RimColor ("Rim Color", Color) = (0, 0, 0, 0)
        _RimWidth ("Rim Width", Float) = 0
        
        _SplTex ("高光图",2D) =  "black" {}
		_SplScl ("高光强度X(R)Y(G)Z(B)范围W",Vector) = (1,0,0,0.15)
		
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
                
                float _SHLightingScale;

                struct v2f {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    fixed4 color : COLOR;
                	fixed3 SHLighting: TEXCOORD1;
                	float2 Spluv : TEXCOORD2;
                };

                uniform float4 _MainTex_ST;
                uniform fixed4 _RimColor;
                float _RimWidth;
                float4 _SplScl;
				float4 _SplTex_ST;
				uniform sampler2D _SplTex;

                v2f vert (appdata_base v) {
                    v2f o;
                    o.pos = UnityObjectToClipPos (v.vertex);

                    float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                    float dotProduct = 1 - dot(v.normal, viewDir);
                   
                    o.color = smoothstep(1 - _RimWidth, 1.0, dotProduct);
                    o.color *= _RimColor;

                    o.uv = v.texcoord.xy;
                    
                    fixed SplLight =  smoothstep(1 - _SplScl.w, 1.0, max(dot(v.normal, viewDir),0.0));  
                    o.color.a = SplLight;
                    o.Spluv = TRANSFORM_TEX (v.texcoord, _SplTex);
               	 	float3 worldNormal = mul((float3x3)unity_ObjectToWorld, v.normal);     
            		o.SHLighting = ShadeSH9(float4(worldNormal,1)) * _SHLightingScale;
                    return o;
                }

                uniform sampler2D _MainTex;
                uniform fixed4 _Color;

                fixed4 frag(v2f i) : COLOR {
                    fixed4 texcol = tex2D(_MainTex, i.uv);
                    
                    fixed4 splTex = tex2D(_SplTex, i.Spluv); 
					fixed SplScl = _SplScl.x * splTex.r + _SplScl.y * splTex.g + _SplScl.z * splTex.b;
					texcol *= _Color * float4(i.SHLighting,1.0f);
                    texcol.rgb += i.color + texcol.rgb * i.color.a * SplScl * i.SHLighting;
                    return texcol;
                }
            ENDCG
        }
    }
}