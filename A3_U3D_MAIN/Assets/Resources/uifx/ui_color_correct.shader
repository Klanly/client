// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ui_color_correct" {
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255
		_ColorMask ("Color Mask", Float) = 15
		_BrightnessAmount ("Brightness亮度", Float) = 1.0
		_SaturationAmount ("Saturation饱和度", Float) = 1.0
		_ContrastAmount ("Contrast对比度", Float) = 1.0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]
 
		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};
 
			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
 
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;

#ifdef UNITY_HALF_TEXEL_OFFSET
				OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
#endif

				OUT.color = IN.color * _Color;
				return OUT;
			}
			
			//计算亮度，饱和度和对比度
			float3 ContrastSaturationBrightness (float3 color, float brt, float sat, float con) {
				// Increase or decrease these values to
				// adjust r, g and b color channels separately
				float avgLumR = 0.5;
				float avgLumG = 0.5;
				float avgLumB = 0.5;
				
				// Luminance coefficients for getting luminance from the image
				float3 LuminanceCoeff = float3 (0.2125, 0.7154, 0.0721);
				
				// Operation for brightmess
				float3 avgLumin = float3 (avgLumR, avgLumG, avgLumB);
				float3 brtColor = color * brt;
				float intensityf = dot (brtColor, LuminanceCoeff);
				float3 intensity = float3 (intensityf, intensityf, intensityf);
				
				// Operation for saturation
				float3 satColor = lerp (intensity, brtColor, sat);
				
				// Operation for contrast
				float3 conColor = lerp (avgLumin, satColor, con);
				
				return conColor;
			}
 
			sampler2D _MainTex;
			fixed _BrightnessAmount;
			fixed _SaturationAmount;
			fixed _ContrastAmount;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 computedColor = tex2D(_MainTex, IN.texcoord.xy);
				computedColor.rgb = ContrastSaturationBrightness (computedColor.rgb, _BrightnessAmount, _SaturationAmount, _ContrastAmount);
				computedColor = computedColor * IN.color;
                return computedColor;
			}
		ENDCG
		}
	}
}
