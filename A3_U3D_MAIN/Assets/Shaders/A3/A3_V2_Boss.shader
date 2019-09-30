Shader "A3/A3_V2_Boss" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
		_RimColor("Rim Color", Color) = (0, 0, 0, 0)
		_RimWidth("Rim Width", Float) = 0
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}

		SubShader
		{
				Tags { "RenderType" = "Opaque" }
				LOD 150
				CGPROGRAM
				//#pragma surface surf Lambert noforwardadd
				#pragma surface surf myLightModel


				sampler2D _MainTex;
				fixed4 _Color;
				fixed4 _RimColor;
				fixed _RimWidth;
				fixed _Cutoff;

				struct Input
				{
					float2 uv_MainTex;
					float3 viewDir;
					float3 worldNormal;
				};

				//命名规则：Lighting接#pragma suface之后起的名字 
				//lightDir :点到光源的单位向量   viewDir:点到摄像机的单位向量   atten:衰减系数 
				float4 LightingmyLightModel(SurfaceOutput s, float3 lightDir, half3 viewDir, half atten)
				{
					float4 c;
					c.rgb = s.Albedo;
					c.a = s.Alpha;
					return c;
				}

				void surf(Input IN, inout SurfaceOutput o)
				{
					fixed4 texcol = tex2D(_MainTex, IN.uv_MainTex);

					float dotProduct = 1 - dot(IN.worldNormal, normalize(IN.viewDir));
					float RimLight = smoothstep(1 - _RimWidth / 1.5, 1.0, dotProduct);

					o.Albedo = texcol.rgb * _Color * 2;
					o.Emission = RimLight * _RimColor;
					o.Alpha = texcol.a;

					float x_2;
					x_2 = (texcol.a - _Cutoff);
					if ((x_2 < 0.0f))
					{
						discard;
					};
				}
			ENDCG


		}
		FallBack Off
}