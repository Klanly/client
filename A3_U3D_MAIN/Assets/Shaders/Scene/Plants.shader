// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "Custom/Scene/Plants" {
	Properties {
		_MainTex ("基本贴图", 2D) = "white" {}
		_WindEdgeFlutter("风扰动强度", float) = 2.0
		_WindEdgeFlutterFreqScale("风扰动频率",float) = 0.1
	}
	SubShader {
		Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}	
		Pass {
			Cull off
			//AlphaTest Greater 0.5					   
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			//着色器变体快捷编译指令：雾效。编译出几个不同的Shader变体来处理不同类型的雾效(关闭/线性/指数/二阶指数)  
			#pragma multi_compile_fog  

			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float _WindEdgeFlutter;
			float _WindEdgeFlutterFreqScale;
			#ifndef LIGHTMAP_OFF
			// float4 unity_LightmapST;
			// sampler2D unity_Lightmap;
			#endif
			
			//定义函数SmoothTriangleWave，可以将三角波近似平滑为正弦波
			float4 SmoothCurve( float4 x ) {   
				return x * x *( 3.0 - 2.0 * x );   
			}
			float4 TriangleWave( float4 x ) {   
				return abs( frac( x + 0.5 ) * 2.0 - 1.0 );   
			}
			float4 SmoothTriangleWave( float4 x ) {   
				return SmoothCurve( TriangleWave( x ) );   
			}

	        struct v2f {
	            float4 pos : SV_POSITION;
	            float2 uv : TEXCOORD0;
	            float4 color : COLOR;
	            #ifndef LIGHTMAP_OFF
				float2 lmap : TEXCOORD1;
				#endif
				UNITY_FOG_COORDS(2)//雾数据				
	        };

	        float4 _MainTex_ST;

	        v2f vert (appdata_full v)
	        {
	            v2f o;
		            
				float bendingFact = v.color.a;      		
				float windTime = _Time.y * _WindEdgeFlutterFreqScale;			
				//顶点越往正方向值越大，反之越小
				float fVtxPhase = dot(v.vertex, _WindEdgeFlutter);			
				// 此算法来源于crysis的植被飘动,x用于边缘; y用于分枝
				float2 vWavesIn = windTime  + float2(fVtxPhase, 0.0);			
				// 频率的经验参数：1.975, 0.793, 0.375, 0.193,得到一个4元三角波函数
				float4 vWaves = (frac( vWavesIn.xxyy * float4(1.975, 0.793, 0.375, 0.193) ) * 2.0 - 1.0);			
				//平滑三角波								
				vWaves =  SmoothTriangleWave( vWaves );				
				//将xz和yw相加，得到随机性
				float2 vWavesSum = vWaves.xz + vWaves.yw;
				// 边缘(xz)与分枝(y)混合
				float3 bend = _WindEdgeFlutter * 0.1f * v.normal.xyz * bendingFact;
				bend.y = bendingFact * 0.3f;
				v.vertex.xyz += 0.5 * vWavesSum.xxx * bend; 
	            
	            o.pos = UnityObjectToClipPos (v.vertex);
	            o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
	            #ifndef LIGHTMAP_OFF
				o.lmap = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
	            o.color = v.color;

				UNITY_TRANSFER_FOG(o, o.pos);

	            return o;
	        }
			
			fixed4 frag (v2f i) : COLOR
			{
				half4 DifTex = tex2D (_MainTex, i.uv);
				#ifndef LIGHTMAP_OFF
				fixed3 lm = DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap));
				DifTex.rgb *= lm;
				#endif	
				if(DifTex.a < 0.5)
				{
					discard;
				}
				UNITY_APPLY_FOG(i.fogCoord, DifTex);
            	return DifTex;
			}
			ENDCG
		}
	} 
	Fallback "VertexLit"
}
