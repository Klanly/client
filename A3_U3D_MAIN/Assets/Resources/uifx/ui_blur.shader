﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ui_blur"
{
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
		_X ("X", Float) = 0.015
		_Y ("Y", Float) = 0.015
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
 
			sampler2D _MainTex;
            //--------------add------------------
			float _X;
			float _Y;
			
            //--------------add------------------
			fixed4 frag(v2f IN) : SV_Target
			{
                //--------------add------------------
                //float distance = _Distance;
				float x = _X*0.125;
				float y = _Y*0.125;
				
				fixed4 computedColor = tex2D(_MainTex, half2(IN.texcoord.x + x*0.125, IN.texcoord.y));
                computedColor += tex2D(_MainTex, half2(IN.texcoord.x - x*0.125, IN.texcoord.y));
                computedColor += tex2D(_MainTex, half2(IN.texcoord.x, IN.texcoord.y + y*0.125));
                computedColor += tex2D(_MainTex, half2(IN.texcoord.x, IN.texcoord.y - y*0.125));
				
                //computedColor += tex2D(_MainTex, half2(IN.texcoord.x + distance , IN.texcoord.y - distance )) * IN.color;
                //computedColor += tex2D(_MainTex, half2(IN.texcoord.x - distance , IN.texcoord.y + distance )) * IN.color;
                //computedColor += tex2D(_MainTex, half2(IN.texcoord.x - distance , IN.texcoord.y)) * IN.color;
                //computedColor += tex2D(_MainTex, half2(IN.texcoord.x , IN.texcoord.y - distance )) * IN.color;
                computedColor = computedColor * IN.color / 4;
                return computedColor;
			}
		ENDCG
		}
	}
}

