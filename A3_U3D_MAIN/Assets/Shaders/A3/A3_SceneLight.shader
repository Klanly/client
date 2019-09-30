// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "A3/A3_SceneLight" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,0.25)
		_LightTex ("光斑贴图", 2D) = "black" {}
	}	
//	SubShader {		
//		Tags { "Queue" = "Transparent" }
//
//		        // Grab the screen behind the object into _GrabTexture
//		        GrabPass { }
//		        // Render the object with the texture generated above, and invert it's colors
//		        Pass {
//		        	ZWrite Off
//		        	Lighting On
//		        	Material {
//		                Emission [_Color]
//		            }
//		        	SetTexture [_LightTex] { combine texture * primary double }
//		            SetTexture [_GrabTexture] { combine texture * previous double }
//		            //SetTexture [_GrabTexture] { combine texture + texture * previous double }
//		            SetTexture [_GrabTexture] { combine texture + previous }
//		        }
//			}		

	Subshader {
		Tags {"Queue"="Transparent"}
		Pass {
			ZWrite Off
			Fog { Color (0, 0, 0) }
			Blend DstColor one	
				 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct v2f {
				float2 uvLight : TEXCOORD0;
				float4 pos : SV_POSITION;
			};
			
			float4 _Color;
			sampler2D _LightTex;
			float4 _LightTex_ST;
			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uvLight =  TRANSFORM_TEX (v.texcoord, _LightTex);
				return o;
			}
						
			fixed4 frag (v2f i) : SV_Target
			{
				float4 LightTex = 	tex2D(_LightTex, i.uvLight).rgba * 2.0;
				LightTex.rgb *= _Color.rgb;
				return _Color.a * LightTex;
			}
			ENDCG
		}
		Pass {
			ZWrite Off
			Fog { Color (0, 0, 0) }
			Blend DstColor one	
				 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct v2f {
				float2 uvLight : TEXCOORD0;
				float4 pos : SV_POSITION;
			};
			
			float4 _Color;
			sampler2D _LightTex;
			float4 _LightTex_ST;
			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uvLight =  TRANSFORM_TEX (v.texcoord, _LightTex);
				return o;
			}
						
			fixed4 frag (v2f i) : SV_Target
			{
				float4 LightTex = 	tex2D(_LightTex, i.uvLight).rgba * 2.0;
				LightTex.rgb *= _Color.rgb;
				return _Color.a * LightTex;
			}
			ENDCG
		}	
	}
}