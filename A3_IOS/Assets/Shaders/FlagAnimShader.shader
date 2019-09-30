Shader "Custom/Scene/FlagAnimShader" {
	Properties {
		_MainTex ("基本贴图", 2D) = "white" {}
		_Wind("X,Y,Z方向风力和收缩",Vector) = (1,1,1,1)
		_WindEdgeFlutter("风扰动强度", float) = 0.5
		_WindEdgeFlutterFreqScale("风扰动频率",float) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Transparent" "IgnoreProjector"="True" "Queue" = "Transparent"}
		Cull off
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert alpha vertex:vert

		sampler2D _MainTex;
		float4 _Wind;
		float _WindEdgeFlutter;
		float _WindEdgeFlutterFreqScale;

		struct Input {
			float2 uv_MainTex;
			float4 color:COLOR;
		};
		
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
		
      	void vert (inout appdata_full v) {
      		float4 wind;
      		
      		//通过顶点a通道的值来控制飘动幅度
      		float bendingFact = v.color.a;
      		
      		//物体空间矩阵和风向(带强度)相乘，得到物体空间下的风力
			wind.xyz = mul((float3x3)_World2Object,_Wind.xyz);
			wind.w = _Wind.w * bendingFact;
			
			//风力频率
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
			float3 bend = _WindEdgeFlutter * 0.1f * v.normal.xyz;
			bend.y = bendingFact * 0.3f;
			v.vertex.xyz += ((vWavesSum.xyx * bend) + (wind.xyz * vWavesSum.y * v.color.a)) * wind.w; 

			// 叠加基本风力并显示位置
			v.vertex.xyz += bendingFact * wind.xyz;		
      	}

		void surf (Input IN, inout SurfaceOutput o) {
			half4 Tex = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = Tex;
			o.Alpha = Tex.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
